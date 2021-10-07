package controller

import (
	routing "github.com/go-ozzo/ozzo-routing/v2"
	_ "github.com/go-sql-driver/mysql"
	"pkg/dbcontext"
	"pkg/log"
	"encoding/json"
	"strconv"
)

// 获取单页记录
type requestData_getPageLog struct{
	CheckTimeStart string `json:"check_time_start"`
	CheckTimeEnd string `json:"check_time_end"`
	Domain string `json:"domain"`
	CheckResult string `json:"check_result"`
	Product string `json:"product"`
	Creator string `json:"creator"`
	Page int `json:"page"`
}

type responseData_getPageLog struct{
	Error string `json:"error"`
	Logs [] DB_Table_logs `json:"logs"`
}

func RegisterGetPageLogHandlers(rg *routing.RouteGroup, logger log.Logger, db *dbcontext.DB) {
	rg.Post("/GetPageLog", getPageLogHandler(logger, db))
}

func getPageLogHandler(logger log.Logger, db *dbcontext.DB) routing.Handler {
	return func(c *routing.Context) error {
		if !IsAllowedServerIP(c.Request, db) {
			rpError := &responseData_getPageLog{}
			rpError.Error = "IP not allowed."
			rpError.Logs = make([] DB_Table_logs, 0)
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rd := requestData_getPageLog{}
		if err := c.Read(&rd); err != nil {
			logger.With(c.Request.Context()).Errorf("invalid request: %v", err)
			return err
		}

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

		var totalCount int
		sqlText := "SELECT COUNT(*) FROM tb_logs WHERE check_date BETWEEN '" + rd.CheckTimeStart + "' AND '" + rd.CheckTimeEnd + "' "
		if rd.Domain != "" {
			sqlText += " AND domain = '" + rd.Domain + "' "
		}
		if checkResult != "" {
			sqlText += " AND result IN ('" + checkResult + "')"
		}
		if rd.Creator != ""{
			sqlText += " AND creator = '" + rd.Creator + "' "
		}
		if product != "" {
			sqlText += " AND product IN ('" + product + "')"
		}
		err := db.DB().NewQuery(sqlText).Row(&totalCount)
		if err != nil{
			logger.With(c.Request.Context()).Errorf("database query error: %v", err)
			rpError := &responseData_getPageLog{}
			rpError.Error = "Database query error."
			rpError.Logs = make([] DB_Table_logs, 0)
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


		sqlText = "SELECT log_index, domain, check_ip, creator, check_date, result, printscreen FROM tb_logs WHERE check_date BETWEEN '" + rd.CheckTimeStart + "' AND '" + rd.CheckTimeEnd + "' "
		if rd.Domain != "" {
			sqlText += " AND domain = '" + rd.Domain + "' "
		}
		if checkResult != "" {
			sqlText += " AND result IN ('" + checkResult + "')"
		}
		if rd.Creator != ""{
			sqlText += " AND creator = '" + rd.Creator + "' "
		}
		if product != "" {
			sqlText += " AND product IN ('" + product + "')"
		}
		sqlText += " ORDER BY check_date DESC"
		if rd.Page != 917262936 { // magic code....ask frankie
			sqlText += " LIMIT "
			if rd.Page == 0{
				sqlText += strconv.Itoa(0)
			} else {
				sqlText += strconv.Itoa((rd.Page - 1) * 20)
			}
			sqlText += ", "
			sqlText += strconv.Itoa(leftCount)
		}

		/*
		q := db.DB().NewQuery(sqlText)
		var logs [] tmp_DB_Table_logs
		err = q.All(&logs)
		*/

		var logs [] DB_Table_logs
		rows, _ := db.DB().NewQuery(sqlText).Rows()
		for rows.Next() {
			var oneLog DB_Table_logs
			err = rows.Scan(&oneLog.LogIndex, &oneLog.Domain, &oneLog.CheckIP,
				&oneLog.Creator, &oneLog.CheckDate, &oneLog.Result, &oneLog.PrintScreen)
			// rows.ScanStruct(&oneLog)
			//logger.Infof("ps = %d", len(oneLog.PrintScreen))
			logs = append(logs, oneLog)
		}

		if err != nil{
			logger.With(c.Request.Context()).Errorf("database query error: %v", err)
			rpError := &responseData_getPageLog{}
			rpError.Error = "Database query error."
			rpError.Logs = make([] DB_Table_logs, 0)
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		var logNum = len(logs)
		if logNum <= 0 {
			logger.With(c.Request.Context()).Infof("no result")
			rpError := &responseData_getPageLog{}
			rpError.Error = ""
			rpError.Logs = make([] DB_Table_logs, 0)
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rp := &responseData_getPageLog{}
		rp.Error = ""
		for _, value := range logs{
			var oneLog DB_Table_logs
			oneLog.LogIndex = value.LogIndex
			oneLog.Domain = value.Domain
			oneLog.CheckIP = value.CheckIP
			oneLog.Creator = value.Creator
			oneLog.CheckDate = value.CheckDate
			oneLog.Result = value.Result
			oneLog.PrintScreen = make([]byte, len(value.PrintScreen))
			copy(oneLog.PrintScreen, []byte(value.PrintScreen))
			rp.Logs = append(rp.Logs, oneLog)
		}

		b, err := json.Marshal(rp)
		if err != nil {
			logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
			return err
		}
		return c.Write(json.RawMessage(string(b)))
    }
}
