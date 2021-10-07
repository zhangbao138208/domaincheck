package controller

import (
	routing "github.com/go-ozzo/ozzo-routing/v2"
	_ "github.com/go-sql-driver/mysql"
	"pkg/dbcontext"
	"pkg/log"
	"encoding/json"
	"strconv"
)

// 获取单页检查点记录
type requestData_getPageCheckpoint struct{
	Product string `json:"product"`
	Creator string `json:"creator"`
	Page int `json:"page"`
}

type responseData_getPageCheckpoint struct{
	Error string `json:"error"`
	Checkpoints [] DB_table_checkpoints `json:"checkpoints"`
}

func RegisterGetPageCheckpointHandlers(rg *routing.RouteGroup, logger log.Logger, db *dbcontext.DB) {
	rg.Post("/GetPageCheckpoint", getPageCheckpointHandler(logger, db))
}


func getPageCheckpointHandler(logger log.Logger, db *dbcontext.DB) routing.Handler {
	return func(c *routing.Context) error {
		if !IsAllowedServerIP(c.Request, db) {
			rpError := &responseData_getPageCheckpoint{}
			rpError.Error = "IP not allowed."
			rpError.Checkpoints = make([] DB_table_checkpoints, 0)
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rd := requestData_getPageCheckpoint{}
		if err := c.Read(&rd); err != nil {
			logger.With(c.Request.Context()).Errorf("invalid request: %v", err)
			return err
		}

		var product = ""
		if rd.Product == "全部" {
			product = "A01','A02','A03','A04','A05','A06','B01','C01"
		}else{
			product = rd.Product
		}

		var totalCount int
		sqlText := "SELECT COUNT(*) FROM tb_checkpoints"
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
		err := db.DB().NewQuery(sqlText).Row(&totalCount)
		if err != nil{
			logger.With(c.Request.Context()).Errorf("database query error: %v", err)
			rpError := &responseData_getPageCheckpoint{}
			rpError.Error = "Database query error."
			rpError.Checkpoints = make([] DB_table_checkpoints, 0)
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}
		var leftCount = totalCount - (rd.Page - 1) * 20
		if leftCount <= 0 {
			leftCount = 0
		} else if leftCount >= 20 {
			leftCount = 20
		}

		sqlText = "SELECT * FROM tb_checkpoints"
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
		sqlText += " ORDER BY checkpoint_index DESC LIMIT "
		sqlText += strconv.Itoa((rd.Page - 1) * 20)
		sqlText += ", "
		sqlText += strconv.Itoa(leftCount)

		q := db.DB().NewQuery(sqlText)
		var checkpoints [] DB_table_checkpoints
		err = q.All(&checkpoints)
		if err != nil{
			logger.With(c.Request.Context()).Errorf("database query error: %v", err)
			rpError := &responseData_getPageCheckpoint{}
			rpError.Error = "Database query error."
			rpError.Checkpoints = make([] DB_table_checkpoints, 0)
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		var checkpointNum = len(checkpoints)
		if checkpointNum <= 0 {
			logger.With(c.Request.Context()).Errorf("database query error: %v", err)
			rpError := &responseData_getPageCheckpoint{}
			rpError.Error = ""
			rpError.Checkpoints = make([] DB_table_checkpoints, 0)
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rp := &responseData_getPageCheckpoint{}
		rp.Error = ""
		for _, value := range checkpoints{
			var oneCheckpoint DB_table_checkpoints
			oneCheckpoint.CheckpointIndex = value.CheckpointIndex
			oneCheckpoint.Product = value.Product
			oneCheckpoint.Creator = value.Creator
			oneCheckpoint.CheckPath = value.CheckPath
			oneCheckpoint.CheckString = value.CheckString
			rp.Checkpoints = append(rp.Checkpoints, oneCheckpoint)
		}

		b, err := json.Marshal(rp)
		if err != nil {
			logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
			return err
		}
		return c.Write(json.RawMessage(string(b)))
    }
}
