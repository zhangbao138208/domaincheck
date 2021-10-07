package controller

import (
	routing "github.com/go-ozzo/ozzo-routing/v2"
	_ "github.com/go-sql-driver/mysql"
	"pkg/dbcontext"
	"pkg/log"
	"encoding/json"
	"strconv"
)

// 获取单页域名记录
type requestData_getPageDomain struct{
	CheckTimeStart string `json:"check_time_start"`
	CheckTimeEnd string `json:"check_time_end"`
	Domain string `json:"domain"`
	IsNeedCheck int `json:"is_need_check"`
	Product string `json:"product"`
	CheckpointIndex string `json:"checkpoint_index"`
	Page int `json:"page"`
}

type responseData_getPageDomain struct{
	Error string `json:"error"`
	Domains [] DB_table_domains `json:"domains"`
}

func RegisterGetPageDomainHandlers(rg *routing.RouteGroup, logger log.Logger, db *dbcontext.DB) {
	rg.Post("/GetPageDomain", getPageDomainHandler(logger, db))
}


func getPageDomainHandler(logger log.Logger, db *dbcontext.DB) routing.Handler {
	return func(c *routing.Context) error {
		if !IsAllowedServerIP(c.Request, db) {
			rpError := &responseData_getPageDomain{}
			rpError.Error = "IP not allowed."
			rpError.Domains = make([] DB_table_domains, 0)
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rd := requestData_getPageDomain{}
		if err := c.Read(&rd); err != nil {
			logger.With(c.Request.Context()).Errorf("invalid request: %v", err)
			return err
		}

		var product = ""
		if rd.Product == "全部" {
			product = "A01','A02','A03','A04','A05','A06','B01','C01"
		}else{
			product = rd.Product
		}

		var totalCount int
		sqlText := "SELECT COUNT(*) FROM tb_domains WHERE update_date BETWEEN '" + rd.CheckTimeStart + " 00:00:01' AND '" + rd.CheckTimeEnd + " 23:59:59' "
		if rd.Domain != "" {
			sqlText += " AND domain = '" + rd.Domain + "'"
		}
		if product != "" {
			sqlText += " AND product IN ('" + product + "')"
		}
		if rd.CheckpointIndex != ""{
			sqlText += " AND checkpoint_index IN (" + rd.CheckpointIndex + ")"
		}
		sqlText += " AND is_need_check = " + strconv.Itoa(rd.IsNeedCheck)
		err := db.DB().NewQuery(sqlText).Row(&totalCount)
		if err != nil{
			logger.With(c.Request.Context()).Errorf("database query error: %v", err)
			rpError := &responseData_getPageDomain{}
			rpError.Error = "Database query error."
			rpError.Domains = make([] DB_table_domains, 0)
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}
		var leftCount = totalCount - (rd.Page - 1) * 20
		if leftCount <= 0 {
			leftCount = 0
		} else if leftCount >= 20 {
			leftCount = 20
		}

		sqlText = "SELECT * FROM tb_domains WHERE update_date BETWEEN '" + rd.CheckTimeStart + " 00:00:01' AND '" + rd.CheckTimeEnd + " 23:59:59' "
		if rd.Domain != "" {
			sqlText += " AND domain = '" + rd.Domain + "'"
		}
		if product != "" {
			sqlText += " AND product IN ('" + product + "')"
		}
		if rd.CheckpointIndex != ""{
			sqlText += " AND checkpoint_index IN (" + rd.CheckpointIndex + ")"
		}
		sqlText += " AND is_need_check = " + strconv.Itoa(rd.IsNeedCheck)
		sqlText += " ORDER BY update_date DESC "

		if rd.Page != 917262936 { // magic code....ask frankie
			sqlText += " LIMIT "
			if rd.Page == 0{
				sqlText += strconv.Itoa(0)
			} else {
				sqlText += strconv.Itoa((rd.Page - 1) * 20)
			}
			sqlText += ", "
			sqlText += strconv.Itoa(leftCount)
		}

		q := db.DB().NewQuery(sqlText)
		var domains [] DB_table_domains
		err = q.All(&domains)
		if err != nil{
			logger.With(c.Request.Context()).Errorf("database query error: %v", err)
			rpError := &responseData_getPageDomain{}
			rpError.Error = "Database query error."
			rpError.Domains = make([] DB_table_domains, 0)
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		var domainNum = len(domains)
		if domainNum <= 0 {
			logger.With(c.Request.Context()).Errorf("database query error: %v", err)
			rpError := &responseData_getPageDomain{}
			rpError.Error = ""
			rpError.Domains = make([] DB_table_domains, 0)
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rp := &responseData_getPageDomain{}
		rp.Error = ""
		for _, value := range domains{
			var oneDomain DB_table_domains
			oneDomain.DomainIndex = value.DomainIndex
			oneDomain.Domain = value.Domain
			oneDomain.IsNeedCheck = value.IsNeedCheck
			oneDomain.CheckpointIndex = value.CheckpointIndex
			oneDomain.Product = value.Product
			oneDomain.Creator = value.Creator
			oneDomain.UpdateDate = value.UpdateDate
			oneDomain.Comment = value.Comment

			rp.Domains = append(rp.Domains, oneDomain)
		}

		b, err := json.Marshal(rp)
		if err != nil {
			logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
			return err
		}
		return c.Write(json.RawMessage(string(b)))
    }
}
