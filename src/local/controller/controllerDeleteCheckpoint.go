package controller

import (
	routing "github.com/go-ozzo/ozzo-routing/v2"
	_ "github.com/go-sql-driver/mysql"
	"github.com/go-ozzo/ozzo-dbx"
	"pkg/dbcontext"
	"pkg/log"
	"encoding/json"
)

type requestData_deleteCheckpoint struct{
	CheckpointIndex int `json:"checkpoint_index"`
}

type responseData_deleteCheckpoint struct{
	Error string `json:"error"`
}

func RegisterDeleteCheckpointHandlers(rg *routing.RouteGroup, logger log.Logger, db *dbcontext.DB) {
	rg.Post("/DeleteCheckpoint", deleteCheckpointHandler(logger, db))
}

func deleteCheckpointHandler(logger log.Logger, db *dbcontext.DB) routing.Handler {
	return func(c *routing.Context) error {
		if !IsAllowedServerIP(c.Request, db) {
			rpError := &responseData_deleteCheckpoint{}
			rpError.Error = "IP not allowed."
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rd := requestData_deleteCheckpoint{}
		if err := c.Read(&rd); err != nil {
			logger.With(c.Request.Context()).Errorf("invalid request: %v", err)
			return err
		}

		_, err := db.DB().Delete("tb_checkpoints", dbx.HashExp{"checkpoint_index": rd.CheckpointIndex}).Execute()
		if err != nil{
			logger.With(c.Request.Context()).Errorf("database delete error: %v", err)
			rpError := &responseData_deleteCheckpoint{}
			rpError.Error = "Delete data error."
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rp := &responseData_deleteCheckpoint{}
		rp.Error = ""
		b, err := json.Marshal(rp)
		if err != nil {
			logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
			return err
		}
		return c.Write(json.RawMessage(string(b)))
	}
}