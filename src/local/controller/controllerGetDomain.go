package controller

import (
	"encoding/json"
	routing "github.com/go-ozzo/ozzo-routing/v2"
	_ "github.com/go-sql-driver/mysql"
	"pkg/dbcontext"
	"pkg/log"
	"strconv"
	"strings"
	"time"
)


// 申请检查域名
type requestData_getDomain struct{
	Products [] string `json:"products"`
	CustomIndex int `json:"custom_index"`
	Token string `json:"token"`
}

type domainCheckCondition struct{
	Domain string `json:"domain"`
	CheckPath string `json:"check_path"`
	CheckString string `json:"check_string"`
	Creator string `json:"creator"`
	Product string `json:"product"`
}

type responseData_getDomain struct{
	Error string `json:"error"`
	DomainCheckConditions [] domainCheckCondition `json:"domain_check_conditions"`
}


func RegisterGetDomainHandlers(rg *routing.RouteGroup, logger log.Logger, db *dbcontext.DB) {
	rg.Post("/GetDomain", getDomainHandler(logger, db))
}


func getDomainHandler(logger log.Logger, db *dbcontext.DB) routing.Handler {
	return func(c *routing.Context) error {
		rd := requestData_getDomain{}
		if err := c.Read(&rd); err != nil {
			logger.With(c.Request.Context()).Errorf("invalid request: %v", err)
			rpError := &responseData_getDomain{}
			rpError.Error = "Request data error."
			rpError.DomainCheckConditions = make([]domainCheckCondition, 0)
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		decryptMAC, err := SimpleAesDecrypt(rd.Token)
		if err != nil {
			rpError := &responseData_getDomain{}
			rpError.Error = "MAC not correct."
			rpError.DomainCheckConditions = make([]domainCheckCondition, 0)
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}
		decryptMACSplit := strings.Split(decryptMAC,"|")
		if len(decryptMACSplit) == 2 {
			decryptMAC = decryptMACSplit[0]
			dateStr := decryptMACSplit[1]
			now := time.Now().Format("2006-01-02")
			if dateStr != now {
				rpError := &responseData_getDomain{}
				rpError.Error = "Token not correct."
				rpError.DomainCheckConditions = make([]domainCheckCondition, 0)
				b, err := json.Marshal(rpError)
				if err != nil {
					logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
					return err
				}
				return c.Write(json.RawMessage(string(b)))
			}
		}else {
			rpError := &responseData_getDomain{}
			rpError.Error = "Token not enough."
			rpError.DomainCheckConditions = make([]domainCheckCondition, 0)
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		if !IsAllowedClientMAC(decryptMAC, db) {
			rpError := &responseData_getDomain{}
			rpError.Error = "IP not allowed."
			rpError.DomainCheckConditions = make([]domainCheckCondition, 0)
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		productList := ""
		for _, product := range rd.Products {
			productList += "'"
			productList += product
			productList += "',"
		}
		productList = strings.TrimRight(productList, ",")


		sqlText := "SELECT * FROM tb_domains WHERE product IN (" + productList +
		") AND is_need_check = 1 ORDER BY update_date ASC LIMIT 0, 5"
		if rd.CustomIndex != 0 {
			customIndex := strconv.Itoa(rd.CustomIndex - 1)
			sqlText = "SELECT * FROM tb_domains WHERE product IN (" + productList +
			") AND is_need_check = 1 AND domain_index % 3 IN (" + customIndex + 
			")  ORDER BY update_date ASC LIMIT 0, 5"
		}

		q := db.DB().NewQuery(sqlText)
		var tobeCheckDomains [] DB_table_domains
		err = q.All(&tobeCheckDomains)
		if err != nil {
			logger.With(c.Request.Context()).Errorf("database query error: %v", err)
			rpError := &responseData_getDomain{}
			rpError.Error = "Database query error."
			rpError.DomainCheckConditions = make([]domainCheckCondition, 0)
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		var domainNumbers = len(tobeCheckDomains)
		if domainNumbers <= 0 {
			rpError := &responseData_getDomain{}
			rpError.Error = "Not enough data."
			rpError.DomainCheckConditions = make([]domainCheckCondition, 0)
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rp := &responseData_getDomain{}
		for _, value := range tobeCheckDomains{
			var oneDomainCheckCondition domainCheckCondition
			oneDomainCheckCondition.Creator = value.Creator
			encryptDomain, err := SimpleAesEncrypt(value.Domain)
			if err != nil {
				continue
			}
			oneDomainCheckCondition.Domain = encryptDomain
			oneDomainCheckCondition.Product = value.Product
			
			sqlText = "SELECT * FROM tb_checkpoints WHERE checkpoint_index = " + strconv.Itoa(value.CheckpointIndex)
			q = db.DB().NewQuery(sqlText)
			var checkpoints [] DB_table_checkpoints
			err = q.All(&checkpoints)
			if err != nil {
				continue
			} else {
				if len(checkpoints) <= 0{
					oneDomainCheckCondition.CheckString = ""
					oneDomainCheckCondition.CheckPath = ""
					rp.DomainCheckConditions = append(rp.DomainCheckConditions, oneDomainCheckCondition)
				} else {
					encryptCheckString, err := SimpleAesEncrypt(checkpoints[0].CheckString)
					if err != nil {
						continue
					}
					oneDomainCheckCondition.CheckString = encryptCheckString
					encryptCheckPath, err := SimpleAesEncrypt(checkpoints[0].CheckPath)
					if err != nil {
						continue
					}
					oneDomainCheckCondition.CheckPath = encryptCheckPath
					rp.DomainCheckConditions = append(rp.DomainCheckConditions, oneDomainCheckCondition)
				}
			}
		}
		if len(rp.DomainCheckConditions) <= 0 {
			logger.With(c.Request.Context()).Errorf("Not enough data: %v", err)
			rpError := &responseData_getDomain{}
			rpError.Error = "Not enough data."
			rpError.DomainCheckConditions = make([]domainCheckCondition, 0)
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		b, err := json.Marshal(rp)
		if err != nil {
			logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
			return err
		}
		return c.Write(json.RawMessage(string(b)))
    }
}