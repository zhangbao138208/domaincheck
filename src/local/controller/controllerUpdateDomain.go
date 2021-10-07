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
type requestData_updateDomain struct{
	DomainIndex int `json:"domain_index"`
	Product string `json:"product"`
	CheckPointIndex int `json:"checkpoint_index"`
	Domain string `json:"domain"`
	Comment string `json:"comment"`
	Creator string `json:"creator"`
}

type responseData_updateDomain struct{
	Error string `json:"error"`
}

func RegisterUpdateDomainHandlers(rg *routing.RouteGroup, logger log.Logger, db *dbcontext.DB) {
	rg.Post("/UpdateDomain", updateDomainHandler(logger, db))
}

func updateDomainHandler(logger log.Logger, db *dbcontext.DB) routing.Handler {
	return func(c *routing.Context) error {

		if !IsAllowedServerIP(c.Request, db) {
			rpError := &responseData_updateDomain{}
			rpError.Error = "IP not allowed."
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rd := requestData_updateDomain{}
		if err := c.Read(&rd); err != nil {
			logger.With(c.Request.Context()).Errorf("invalid request: %v", err)
			return err
		}

		_, err := db.DB().Update("tb_domains", dbx.Params{
			 "domain": rd.Domain,
			 "product": rd.Product, 
			 "checkpoint_index": rd.CheckPointIndex, 
			 "creator": rd.Creator,
			 "comment": rd.Comment,
			 "update_date": time.Now().Format("2006-01-02 15:04:05") },
			 dbx.HashExp{"domain_index": rd.DomainIndex}).Execute()
		if err != nil{
			logger.With(c.Request.Context()).Errorf("database update error: %v", err)
			rpError := &responseData_updateDomain{}
			rpError.Error = "Update data error."
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rp := &responseData_updateDomain{}
		rp.Error = ""
		b, err := json.Marshal(rp)
		if err != nil {
			logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
			return err
		}
		return c.Write(json.RawMessage(string(b)))
    }
}