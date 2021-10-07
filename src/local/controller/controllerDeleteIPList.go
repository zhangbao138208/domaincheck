package controller

import (
	routing "github.com/go-ozzo/ozzo-routing/v2"
	_ "github.com/go-sql-driver/mysql"
	"github.com/go-ozzo/ozzo-dbx"
	"pkg/dbcontext"
	"pkg/log"
	"encoding/json"
)

type requestData_deleteIPList struct{
	Index int `json:"index"`
}

type responseData_deleteIPList struct{
	Error string `json:"error"`
}

func RegisterDeleteIPListHandlers(rg *routing.RouteGroup, logger log.Logger, db *dbcontext.DB) {
	rg.Post("/DeleteIPList", deleteIPListHandler(logger, db))
}

func deleteIPListHandler(logger log.Logger, db *dbcontext.DB) routing.Handler {
	return func(c *routing.Context) error {
		rd := requestData_deleteIPList{}
		if err := c.Read(&rd); err != nil {
			logger.With(c.Request.Context()).Errorf("invalid request: %v", err)
			return err
		}

		_, err := db.DB().Delete("tb_iplist", dbx.HashExp{"index": rd.Index}).Execute()
		if err != nil{
			logger.With(c.Request.Context()).Errorf("database delete error: %v", err)
			rpError := &responseData_deleteIPList{}
			rpError.Error = "Delete data error."
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rp := &responseData_deleteIPList{}
		rp.Error = ""
		b, err := json.Marshal(rp)
		if err != nil {
			logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
			return err
		}
		return c.Write(json.RawMessage(string(b)))
	}
}