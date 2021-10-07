package controller

import (
	routing "github.com/go-ozzo/ozzo-routing/v2"
	_ "github.com/go-sql-driver/mysql"
	"pkg/dbcontext"
	"pkg/log"
	"encoding/json"
	"github.com/go-ozzo/ozzo-dbx"
	"time"
)

// 申请更新域名信息
type requestData_updateDomainChecked struct{
	DomainIndex int `json:"domain_index"`
	IsNeedCheck int `json:"is_need_check"`
}

type responseData_updateDomainChecked struct{
	Error string `json:"error"`
}

func RegisterUpdateDomainCheckedHandlers(rg *routing.RouteGroup, logger log.Logger, db *dbcontext.DB) {
	rg.Post("/UpdateDomainChecked", updateDomainCheckedHandler(logger, db))
}

func updateDomainCheckedHandler(logger log.Logger, db *dbcontext.DB) routing.Handler {
	return func(c *routing.Context) error {
		if !IsAllowedServerIP(c.Request, db) {
			rpError := &responseData_updateDomainChecked{}
			rpError.Error = "IP not allowed."
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rd := requestData_updateDomainChecked{}
		if err := c.Read(&rd); err != nil {
			logger.With(c.Request.Context()).Errorf("invalid request: %v", err)
			return err
		}

		_, err := db.DB().Update("tb_domains", dbx.Params{
			 "is_need_check": rd.IsNeedCheck,
			 "update_date": time.Now().Format("2006-01-02 15:04:05") },
			 dbx.HashExp{"domain_index": rd.DomainIndex}).Execute()
		if err != nil{
			logger.With(c.Request.Context()).Errorf("database update error: %v", err)
			rpError := &responseData_updateDomainChecked{}
			rpError.Error = "Update data error."
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rp := &responseData_updateDomainChecked{}
		rp.Error = ""
		b, err := json.Marshal(rp)
		if err != nil {
			logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
			return err
		}
		return c.Write(json.RawMessage(string(b)))
    }
}