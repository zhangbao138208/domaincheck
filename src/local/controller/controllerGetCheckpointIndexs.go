package controller

import (
	routing "github.com/go-ozzo/ozzo-routing/v2"
	_ "github.com/go-sql-driver/mysql"
	"pkg/dbcontext"
	"pkg/log"
	"encoding/json"
	"strconv"
)

// 获取记录总数
type requestData_getCheckpointIndexs struct{
	Product string `json:"product"`
}

type responseData_getCheckpointIndexs struct{
	Error string `json:"error"`
	CheckpointIndexs [] string `json:"checkpoint_indexs"`
}

func RegisterGetCheckpointIndexsHandlers(rg *routing.RouteGroup, logger log.Logger, db *dbcontext.DB) {
	rg.Post("/GetCheckpointIndexs", getCheckpointIndexsHandler(logger, db))
}


func getCheckpointIndexsHandler(logger log.Logger, db *dbcontext.DB) routing.Handler {
	return func(c *routing.Context) error {
		if !IsAllowedServerIP(c.Request, db) {
			rpError := &responseData_getCheckpointIndexs{}
			rpError.Error = "IP not allowed."
			rpError.CheckpointIndexs =  [] string {}
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rd := requestData_getCheckpointIndexs{}
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

		/*
		q := db.DB().Select("*").From("tb_checkpoints").
			Where(dbx.In("product", product)).
			OrderBy("checkpoint_index")
			*/
		sqlText := "SELECT * FROM tb_checkpoints WHERE product IN ('" + product + "')" + " ORDER BY checkpoint_index DESC"
		q := db.DB().NewQuery(sqlText)
		var checkpointIndexs [] DB_table_checkpoints
		err := q.All(&checkpointIndexs)
		if err != nil{
			logger.With(c.Request.Context()).Errorf("database query error: %v", err)
			rpError := &responseData_getCheckpointIndexs{}
			rpError.Error = "Database query error."
			rpError.CheckpointIndexs =  [] string {}
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rp := &responseData_getCheckpointIndexs{}
		for _, value := range checkpointIndexs{
			rp.CheckpointIndexs = append(rp.CheckpointIndexs, value.Product + "_" + strconv.Itoa(value.CheckpointIndex))
		}
		rp.Error = ""
		b, err := json.Marshal(rp)
		if err != nil {
			logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
			return err
		}
		return c.Write(json.RawMessage(string(b)))
    }
}
