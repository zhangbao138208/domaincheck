package controller

import (
	routing "github.com/go-ozzo/ozzo-routing/v2"
	_ "github.com/go-sql-driver/mysql"
	"pkg/dbcontext"
	"pkg/log"
	"encoding/json"
	"github.com/go-ozzo/ozzo-dbx"
	"time"
	"strings"
)

// 申请创建域名
type requestData_createDomain struct{
	Product string `json:"product"`
	CheckPointIndex int `json:"checkpoint_index"`
	Domain string `json:"domain"`
	Comment string `json:"comment"`
	Creator string `json:"creator"`
}

type responseData_createDomain struct{
	Error string `json:"error"`
}

func RegisterCreateDomainHandlers(rg *routing.RouteGroup, logger log.Logger, db *dbcontext.DB) {
	rg.Post("/CreateDomain", createDomainHandler(logger, db))
}

func createDomainHandler(logger log.Logger, db *dbcontext.DB) routing.Handler {
	return func(c *routing.Context) error {
		if !IsAllowedServerIP(c.Request, db) {
			rpError := &responseData_createDomain{}
			rpError.Error = "IP not allowed."
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rd := requestData_createDomain{}
		if err := c.Read(&rd); err != nil {
			logger.With(c.Request.Context()).Errorf("invalid request: %v", err)
			return err
		}

		// 多域名支持
		domainList := strings.Split(rd.Domain, ";")
		if len(domainList) <= 0 {
			rpError := &responseData_createDomain{}
			rpError.Error = "Not avaliable domain."
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		for _, oneDomain := range domainList{
			var isNeedInsert bool = true
			// 去除域名前后空格
			oneNewDomain := strings.TrimSpace(oneDomain)
			// 禁止空域名
			if oneNewDomain == ""{
				isNeedInsert = false
				if len(domainList) == 1{
					rpError := &responseData_createDomain{}
					rpError.Error = "Empty domain."
					b, err := json.Marshal(rpError)
					if err != nil {
						logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
						return err
					}
					return c.Write(json.RawMessage(string(b)))
				}
			}
			// 检查域名是否存在
			var count int
			sqlText := "SELECT COUNT(*) FROM tb_domains WHERE domain = '" + oneNewDomain + "'"
			err := db.DB().NewQuery(sqlText).Row(&count)
			if err != nil{
				isNeedInsert = false
				if len(domainList) == 1{
					logger.With(c.Request.Context()).Errorf("database query error: %v", err)
					rpError := &responseData_createDomain{}
					rpError.Error = "Query data error."
					b, err := json.Marshal(rpError)
					if err != nil {
						logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
						return err
					}
					return c.Write(json.RawMessage(string(b)))
				}
			} else if count > 0{
				isNeedInsert = false
				if len(domainList) == 1 {
					rpError := &responseData_createDomain{}
					rpError.Error = "Domain already exists."
					b, err := json.Marshal(rpError)
					if err != nil {
						logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
						return err
					}
					return c.Write(json.RawMessage(string(b)))
				}
			}

			if isNeedInsert {
				_, err = db.DB().Insert("tb_domains", dbx.Params{
					"domain": oneNewDomain, 
					"is_need_check": 1, 
					"checkpoint_index": rd.CheckPointIndex, 
					"product": rd.Product,
					"creator": rd.Creator,
					"update_date": time.Now().Format("2006-01-02 15:04:05"), 
					"comment": rd.Comment }).Execute()
				if err != nil{
					if len(domainList) == 1 {
						logger.With(c.Request.Context()).Errorf("database insert error: %v", err)
						rpError := &responseData_createDomain{}
						rpError.Error = "Insert data error."
						b, err := json.Marshal(rpError)
						if err != nil {
							logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
							return err
						}
						return c.Write(json.RawMessage(string(b)))
					}
				}
			}
		}

		rp := &responseData_createDomain{}
		rp.Error = ""
		b, err := json.Marshal(rp)
		if err != nil {
			logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
			return err
		}
		return c.Write(json.RawMessage(string(b)))
    }
}