package controller

import (
	routing "github.com/go-ozzo/ozzo-routing/v2"
	_ "github.com/go-sql-driver/mysql"
	"pkg/dbcontext"
	"pkg/log"
	"encoding/json"
	"strconv"
)

// 获取域名总数
type requestData_getDomainsCount struct{
	CheckTimeStart string `json:"check_time_start"`
	CheckTimeEnd string `json:"check_time_end"`
	Domain string `json:"domain"`
	IsNeedCheck int `json:"is_need_check"`
	Product string `json:"product"`
	CheckpointIndex string `json:"checkpoint_index"`
}

type responseData_getDomainsCount struct{
	Error string `json:"error"`
	TotalCount int `json:"total_count"`
}

func RegisterGetDomainsCountHandlers(rg *routing.RouteGroup, logger log.Logger, db *dbcontext.DB) {
	rg.Post("/GetDomainsCount", getDomainsCountHandler(logger, db))
}

func getDomainsCountHandler(logger log.Logger, db *dbcontext.DB) routing.Handler {
	return func(c *routing.Context) error {
		if !IsAllowedServerIP(c.Request, db) {
			rpError := &responseData_getDomainsCount{}
			rpError.Error = "IP not allowed."
			rpError.TotalCount = 0
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rd := requestData_getDomainsCount{}
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

		sqlText := "SELECT * FROM tb_domains WHERE update_date BETWEEN '" + rd.CheckTimeStart + " 00:00:01' AND '" + rd.CheckTimeEnd + " 23:59:59' "

		if rd.Domain != "" {
			sqlText += " AND domain = '" + rd.Domain + "' "
		}
		if product != "" {
			sqlText += " AND product IN ('" + product + "')"
		}
		if rd.CheckpointIndex != ""{
			sqlText += " AND checkpoint_index IN (" + rd.CheckpointIndex + ")"
		}
		sqlText += " AND is_need_check = " + strconv.Itoa(rd.IsNeedCheck)

		q := db.DB().NewQuery(sqlText)
		var domains [] DB_table_domains
		err := q.All(&domains)
		if err != nil{
			logger.With(c.Request.Context()).Errorf("database query error: %v", err)
			rpError := &responseData_getDomainsCount{}
			rpError.Error = "Database query error."
			rpError.TotalCount = 0
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rp := &responseData_getDomainsCount{}
		rp.TotalCount = len(domains)
		rp.Error = ""
		b, err := json.Marshal(rp)
		if err != nil {
			logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
			return err
		}
		return c.Write(json.RawMessage(string(b)))
    }
}
