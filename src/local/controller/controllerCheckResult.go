package controller

import (
	routing "github.com/go-ozzo/ozzo-routing/v2"
	_ "github.com/go-sql-driver/mysql"
	"github.com/go-ozzo/ozzo-dbx"
	"pkg/dbcontext"
	"pkg/log"
	"encoding/json"
	"time"
)

// 返回测试结果
type requestData_checkResult struct{
	Domain string `json:"domain"`
	CheckIP string `json:"check_ip"`
	Creator string `json:"creator"`
	ClientID string `json:"client_id"`
	Product string `json:"product"`
	Result string `json:"result"`
	Token string `json:"token"`
	PrintScreen [] byte `json:"printscreen"`
}

type responseData_checkResult struct{
	Error string `json:"error"`
}

func RegisterCheckResultHandlers(rg *routing.RouteGroup, logger log.Logger, db *dbcontext.DB) {
	rg.Post("/SyncCheckResult", syncCheckResultHandler(logger, db))
}

func syncCheckResultHandler(logger log.Logger, db *dbcontext.DB) routing.Handler {
	return func(c *routing.Context) error {
		rd := requestData_checkResult{}
		if err := c.Read(&rd); err != nil {
			logger.With(c.Request.Context()).Errorf("invalid request: %v", err)
			return err
		}

		decryptMAC, err := SimpleAesDecrypt(rd.Token)
		if err != nil {
			rpError := &responseData_checkResult{}
			rpError.Error = "MAC not correct."
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}
		if !IsAllowedClientMAC(decryptMAC, db) {
			rpError := &responseData_checkResult{}
			rpError.Error = "IP not allowed."
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}


		decryptDomain, err := SimpleAesDecrypt(rd.Domain)
		if err != nil {
			rpError := &responseData_checkResult{}
			rpError.Error = "Domain not correct."
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}
		//logger.Infof("1")
		// 增加日志
		_, err = db.DB().Insert("tb_logs", dbx.Params{
			"domain": decryptDomain, 
			"check_ip": rd.CheckIP, 
			"creator": rd.Creator, 
			"check_date": time.Now().Format("2006-01-02 15:04:05"),
			"result": rd.Result, 
			"client_id": rd.ClientID,
			"product": rd.Product,
			"printscreen": rd.PrintScreen }).Execute()
		if err != nil{
			logger.With(c.Request.Context()).Errorf("database insert error: %v", err)
			rpError := &responseData_checkResult{}
			rpError.Error = "Insert to DB error."
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		//logger.Infof("2")
		// 修改DOMAIN检查时间
		_, err = db.DB().Update("tb_domains", dbx.Params{"update_date": time.Now().Format("2006-01-02 15:04:05")}, 
				dbx.HashExp{"domain": decryptDomain}).Execute()
		if err != nil{
			logger.With(c.Request.Context()).Errorf("database insert error: %v", err)
			rpError := &responseData_checkResult{}
			rpError.Error = "Update DB data error."
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		//logger.Infof("3")
		rp := &responseData_checkResult{}
		rp.Error = ""
		b, err := json.Marshal(rp)
		if err != nil {
			logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
			return err
		}
		return c.Write(json.RawMessage(string(b)))
    }
}