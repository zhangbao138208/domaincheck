package controller

import (
	routing "github.com/go-ozzo/ozzo-routing/v2"
	_ "github.com/go-sql-driver/mysql"
	"pkg/dbcontext"
	"pkg/log"
	"encoding/json"
)

// 获取记录总数
type requestData_getCheckpointsCount struct{
	Product string `json:"product"`
	Creator string `json:"creator"`
}

type responseData_getCheckpointsCount struct{
	Error string `json:"error"`
	TotalCount int `json:"total_count"`
}

func RegisterGetCheckpointsCountHandlers(rg *routing.RouteGroup, logger log.Logger, db *dbcontext.DB) {
	rg.Post("/GetCheckpointsCount", getCheckpointsCountHandler(logger, db))
}


func getCheckpointsCountHandler(logger log.Logger, db *dbcontext.DB) routing.Handler {
	return func(c *routing.Context) error {
		if !IsAllowedServerIP(c.Request, db) {
			rpError := &responseData_getCheckpointsCount{}
			rpError.Error = "IP not allowed."
			rpError.TotalCount = 0
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}


		rd := requestData_getCheckpointsCount{}
		if err := c.Read(&rd); err != nil {
			logger.With(c.Request.Context()).Errorf("invalid request: %v", err)
			return err
		}

		sqlText := "SELECT * FROM tb_checkpoints"
		var product = ""
		if rd.Product == "全部" {
			product = "A01','A02','A03','A04','A05','A06','B01','C01"
		}else{
			product = rd.Product
		}

		if product != "" && rd.Creator != "" {
			sqlText += " WHERE creator = '" + rd.Creator + "' AND product IN ('" + product + "')"
		} else {
			if product != "" {
				sqlText += " WHERE product IN ('" + product + "')"
			}
			if rd.Creator != "" {
				sqlText += " WHERE creator = '" + rd.Creator + "'"
			}
		}

		q := db.DB().NewQuery(sqlText)
		var checkpoints [] DB_table_checkpoints
		err := q.All(&checkpoints)
		if err != nil{
			logger.With(c.Request.Context()).Errorf("database query error: %v", err)
			rpError := &responseData_getCheckpointsCount{}
			rpError.Error = "Database query error."
			rpError.TotalCount = 0
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rp := &responseData_getCheckpointsCount{}
		rp.TotalCount = len(checkpoints)
		rp.Error = ""
		b, err := json.Marshal(rp)
		if err != nil {
			logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
			return err
		}
		return c.Write(json.RawMessage(string(b)))
    }
}
