package controller

import (
	routing "github.com/go-ozzo/ozzo-routing/v2"
	_ "github.com/go-sql-driver/mysql"
	"pkg/dbcontext"
	"pkg/log"
	"encoding/json"
	"github.com/go-ozzo/ozzo-dbx"
)

// 申请更新检查点信息
type requestData_updateCheckpoint struct{
	CheckpointIndex int `json:"checkpoint_index"`
	Product string `json:"product"`
	CheckPath string `json:"check_path"`
	CheckString string `json:"check_string"`
	Creator string `json:"creator"`
}

type responseData_updateCheckpoint struct{
	Error string `json:"error"`
}

func RegisterUpdateCheckpointHandlers(rg *routing.RouteGroup, logger log.Logger, db *dbcontext.DB) {
	rg.Post("/UpdateCheckpoint", updateCheckpointHandler(logger, db))
}

func updateCheckpointHandler(logger log.Logger, db *dbcontext.DB) routing.Handler {
	return func(c *routing.Context) error {
		if !IsAllowedServerIP(c.Request, db) {
			rpError := &responseData_updateCheckpoint{}
			rpError.Error = "IP not allowed."
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rd := requestData_updateCheckpoint{}
		if err := c.Read(&rd); err != nil {
			logger.With(c.Request.Context()).Errorf("invalid request: %v", err)
			return err
		}

		_, err := db.DB().Update("tb_checkpoints", dbx.Params{
			 "product": rd.Product,
			 "check_path": rd.CheckPath, 
			 "check_string": rd.CheckString, 
			 "creator": rd.Creator },
			 dbx.HashExp{"checkpoint_index": rd.CheckpointIndex}).Execute()
		if err != nil{
			logger.With(c.Request.Context()).Errorf("database update error: %v", err)
			rpError := &responseData_updateCheckpoint{}
			rpError.Error = "Update data error."
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rp := &responseData_updateCheckpoint{}
		rp.Error = ""
		b, err := json.Marshal(rp)
		if err != nil {
			logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
			return err
		}
		return c.Write(json.RawMessage(string(b)))
    }
}