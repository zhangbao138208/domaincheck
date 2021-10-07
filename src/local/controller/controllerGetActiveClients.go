package controller

import (
	routing "github.com/go-ozzo/ozzo-routing/v2"
	_ "github.com/go-sql-driver/mysql"
	"pkg/dbcontext"
	"pkg/log"
	"encoding/json"
)

// 获取单页记录
type requestData_getActiveClients struct{
	Product string `json:"product"`
}

type ActiveClientInfo struct{
	ClientID string `json:"client_id"`
	Product string `json:"product"`
	CheckDate string `json:"check_date"`
}

type responseData_getActiveClients struct{
	Error string `json:"error"`
	ActiveClientInfos [] ActiveClientInfo `json:"active_client_infos"`
}

func RegisterGetActiveClientsHandlers(rg *routing.RouteGroup, logger log.Logger, db *dbcontext.DB) {
	rg.Post("/GetActiveClients", getActiveClientsHandler(logger, db))
}

func getActiveClientsHandler(logger log.Logger, db *dbcontext.DB) routing.Handler {
	return func(c *routing.Context) error {
		if !IsAllowedServerIP(c.Request, db) {
			rpError := &responseData_getActiveClients{}
			rpError.Error = "IP not allowed."
			rpError.ActiveClientInfos = make([] ActiveClientInfo, 0)
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rd := requestData_getActiveClients{}
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


		var sqlText = "select t.check_date, t.client_id, t.product from domaincheckerdb.tb_logs t inner join (select client_id, product, max(check_date) as MaxDate from domaincheckerdb.tb_logs where product IN ('" + product + "')" + " group by client_id ) tm on t.check_date = tm.MaxDate and t.client_id = tm.client_id and t.product = tm.product"

		q := db.DB().NewQuery(sqlText)
		var activeClients [] ActiveClientInfo
		err := q.All(&activeClients)
		if err != nil{
			logger.With(c.Request.Context()).Errorf("database query error: %v", err)
			rpError := &responseData_getActiveClients{}
			rpError.Error = "Database query error."
			rpError.ActiveClientInfos = make([] ActiveClientInfo, 0)
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		var logNum = len(activeClients)
		if logNum <= 0 {
			logger.With(c.Request.Context()).Infof("no result")
			rpError := &responseData_getActiveClients{}
			rpError.Error = ""
			rpError.ActiveClientInfos = make([] ActiveClientInfo, 0)
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rp := &responseData_getActiveClients{}
		rp.Error = ""
		for _, value := range activeClients{
			var oneClient ActiveClientInfo
			oneClient.ClientID = value.ClientID
			oneClient.Product = value.Product
			oneClient.CheckDate = value.CheckDate
			logger.Infof(value.CheckDate)
			rp.ActiveClientInfos = append(rp.ActiveClientInfos, oneClient)
		}

		b, err := json.Marshal(rp)
		if err != nil {
			logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
			return err
		}
		return c.Write(json.RawMessage(string(b)))
    }
}
