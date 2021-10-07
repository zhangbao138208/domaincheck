package controller

import (
	routing "github.com/go-ozzo/ozzo-routing/v2"
	_ "github.com/go-sql-driver/mysql"
	"pkg/dbcontext"
	"pkg/log"
	"encoding/json"
)

// 获取记录总数
type requestData_getLogsCount struct{
	CheckTimeStart string `json:"check_time_start"`
	CheckTimeEnd string `json:"check_time_end"`
	Domain string `json:"domain"`
	CheckResult string `json:"check_result"`
	Creator string `json:"creator"`
	Product string `json:"product"`
}

type responseData_getLogsCount struct{
	Error string `json:"error"`
	TotalCount int `json:"total_count"`
}

func RegisterGetLogsCountHandlers(rg *routing.RouteGroup, logger log.Logger, db *dbcontext.DB) {
	rg.Post("/GetLogsCount", getLogsCountHandler(logger, db))
}


func getLogsCountHandler(logger log.Logger, db *dbcontext.DB) routing.Handler {
	return func(c *routing.Context) error {
		if !IsAllowedServerIP(c.Request, db) {
			rpError := &responseData_getLogsCount{}
			rpError.Error = "IP not allowed."
			rpError.TotalCount = 0
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rd := requestData_getLogsCount{}
		if err := c.Read(&rd); err != nil {
			logger.With(c.Request.Context()).Errorf("invalid request: %v", err)
			return err
		}

		sqlText := "SELECT * FROM tb_logs WHERE check_date BETWEEN '" + rd.CheckTimeStart + "' AND '" + rd.CheckTimeEnd + "' "

		var checkResult = ""
		if rd.CheckResult == "全部" {
			checkResult = "CURL失败','404失败','403失败','验证失败','成功"
		}else{
			checkResult = rd.CheckResult
		}

		var product = ""
		if rd.Product == "全部" {
			product = "A01','A02','A03','A04','A05','A06','B01','C01"
		}else{
			product = rd.Product
		}

		if rd.Domain != "" {
			sqlText += " AND domain = '" + rd.Domain + "'"
		}

		if checkResult != "" {
			sqlText += " AND result IN ('" + checkResult + "')"
		}

		if rd.Creator != "" {
			sqlText += " AND creator = '" + rd.Creator + "'"
		}

		if product != "" {
			sqlText += " AND product IN ('" + product + "')"
		}

		q := db.DB().NewQuery(sqlText)
		var logs [] DB_Table_logs
		err := q.All(&logs)
		if err != nil{
			logger.With(c.Request.Context()).Errorf("database query error: %v", err)
			rpError := &responseData_getLogsCount{}
			rpError.Error = "Database query error."
			rpError.TotalCount = 0
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rp := &responseData_getLogsCount{}
		rp.TotalCount = len(logs)
		rp.Error = ""
		b, err := json.Marshal(rp)
		if err != nil {
			logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
			return err
		}
		return c.Write(json.RawMessage(string(b)))
    }
}
