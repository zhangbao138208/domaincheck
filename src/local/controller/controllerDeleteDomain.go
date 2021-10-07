package controller

import (
	routing "github.com/go-ozzo/ozzo-routing/v2"
	_ "github.com/go-sql-driver/mysql"
	"github.com/go-ozzo/ozzo-dbx"
	"pkg/dbcontext"
	"pkg/log"
	"encoding/json"
)

type requestData_deleteDomain struct{
	DomainIndex int `json:"domain_index"`
}

type responseData_deleteDomain struct{
	Error string `json:"error"`
}

func RegisterDeleteDomainHandlers(rg *routing.RouteGroup, logger log.Logger, db *dbcontext.DB) {
	rg.Post("/DeleteDomain", deleteDomainHandler(logger, db))
}

func deleteDomainHandler(logger log.Logger, db *dbcontext.DB) routing.Handler {
	return func(c *routing.Context) error {
		if !IsAllowedServerIP(c.Request, db) {
			rpError := &responseData_deleteDomain{}
			rpError.Error = "IP not allowed."
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rd := requestData_deleteDomain{}
		if err := c.Read(&rd); err != nil {
			logger.With(c.Request.Context()).Errorf("invalid request: %v", err)
			return err
		}

		_, err := db.DB().Delete("tb_domains", dbx.HashExp{"domain_index": rd.DomainIndex}).Execute()
		if err != nil{
			logger.With(c.Request.Context()).Errorf("database delete error: %v", err)
			rpError := &responseData_deleteDomain{}
			rpError.Error = "Delete data error."
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rp := &responseData_deleteDomain{}
		rp.Error = ""
		b, err := json.Marshal(rp)
		if err != nil {
			logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
			return err
		}
		return c.Write(json.RawMessage(string(b)))
	}
}