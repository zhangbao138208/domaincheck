package controller

import (
	routing "github.com/go-ozzo/ozzo-routing/v2"
	_ "github.com/go-sql-driver/mysql"
	"pkg/dbcontext"
	"pkg/log"
	"encoding/json"
	"github.com/go-ozzo/ozzo-dbx"
)

// 申请创建检车点
type requestData_createCheckpoint struct{
	Product string `json:"product"`
	CheckPath string `json:"check_path"`
	CheckString string `json:"check_string"`
	Creator string `json:"creator"`
}

type responseData_createCheckpoint struct{
	Error string `json:"error"`
}

func RegisterCreateCheckpointHandlers(rg *routing.RouteGroup, logger log.Logger, db *dbcontext.DB) {
	rg.Post("/CreateCheckpoint", createCheckpointHandler(logger, db))
}

func createCheckpointHandler(logger log.Logger, db *dbcontext.DB) routing.Handler {
	return func(c *routing.Context) error {
		if !IsAllowedServerIP(c.Request, db) {
			rpError := &responseData_createCheckpoint{}
			rpError.Error = "IP not allowed."
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rd := requestData_createCheckpoint{}
		if err := c.Read(&rd); err != nil {
			logger.With(c.Request.Context()).Errorf("invalid request: %v", err)
			return err
		}

		_, err := db.DB().Insert("tb_checkpoints", dbx.Params{
			"product": rd.Product, 
			"check_path": rd.CheckPath, 
			"check_string": rd.CheckString,
			"creator": rd.Creator }).Execute()
		if err != nil{
			logger.With(c.Request.Context()).Errorf("database insert error: %v", err)
			rpError := &responseData_createCheckpoint{}
			rpError.Error = "Insert data error."
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rp := &responseData_createCheckpoint{}
		rp.Error = ""
		b, err := json.Marshal(rp)
		if err != nil {
			logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
			return err
		}
		return c.Write(json.RawMessage(string(b)))
    }
}