package controller

import (
	routing "github.com/go-ozzo/ozzo-routing/v2"
	_ "github.com/go-sql-driver/mysql"
	"pkg/dbcontext"
	"pkg/log"
	"encoding/json"
	"github.com/go-ozzo/ozzo-dbx"
)

// 申请更新IP列表
type requestData_updateIPList struct{
	Index int `json:"index"`
	IP string `json:"ip"`
	IsManager int `json:"is_manager"`
	Comment string `json:"comment"`
	IsUseful int `json:"is_useful"`
	CustomIndex int `json:"custom_index"`
	Product string `json:"product"`
}

type responseData_updateIPList struct{
	Error string `json:"error"`
}

func RegisterUpdateIPListHandlers(rg *routing.RouteGroup, logger log.Logger, db *dbcontext.DB) {
	rg.Post("/UpdateIPList", updateIPListHandler(logger, db))
}

func updateIPListHandler(logger log.Logger, db *dbcontext.DB) routing.Handler {
	return func(c *routing.Context) error {
		rd := requestData_updateIPList{}
		if err := c.Read(&rd); err != nil {
			logger.With(c.Request.Context()).Errorf("invalid request: %v", err)
			return err
		}

		_, err := db.DB().Update("tb_iplist", dbx.Params{
			"ip": rd.IP,
			"is_manager": rd.IsManager, 
			"comment": rd.Comment, 
			"is_useful": rd.IsUseful,
			"custom_index": rd.CustomIndex,
			"product": rd.Product,
		 },
			dbx.HashExp{"index": rd.Index}).Execute()
	   if err != nil{
		   logger.With(c.Request.Context()).Errorf("database update error: %v", err)
		   rpError := &responseData_updateIPList{}
		   rpError.Error = "Update data error."
		   b, err := json.Marshal(rpError)
		   if err != nil {
			   logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
			   return err
		   }
		   return c.Write(json.RawMessage(string(b)))
	   }

	   rp := &responseData_updateIPList{}
	   rp.Error = ""
	   b, err := json.Marshal(rp)
	   if err != nil {
		   logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
		   return err
	   }
	   return c.Write(json.RawMessage(string(b)))
	}
}