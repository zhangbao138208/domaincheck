package controller

import (
	routing "github.com/go-ozzo/ozzo-routing/v2"
	_ "github.com/go-sql-driver/mysql"
	"pkg/dbcontext"
	"pkg/log"
	"encoding/json"
)

// 获取IP列表记录
type requestData_GetIPList struct{
	UserName string `json:"username"`
	Product string `json:"product"`
}

type responseData_GetIPList struct{
	Error string `json:"error"`
	DB_Table_IPLists [] DB_Table_IPList `json:"ip_lists"`	
}

func RegisterGetIPListHandlers(rg *routing.RouteGroup, logger log.Logger, db *dbcontext.DB) {
	rg.Post("/GetIPLists", getIPListsHandler(logger, db))
}

func getIPListsHandler(logger log.Logger, db *dbcontext.DB) routing.Handler {
	return func(c *routing.Context) error {

		rd := requestData_GetIPList{}
		if err := c.Read(&rd); err != nil {
			logger.With(c.Request.Context()).Errorf("invalid request: %v", err)
			return err
		}

		sqlText := "SELECT * FROM tb_iplist"
		if rd.Product != "全部" {
			sqlText = "SELECT * FROM tb_iplist WHERE product IN ('" + rd.Product + "', 'ALL')"
		}

		q := db.DB().NewQuery(sqlText)
		var IPLists [] DB_Table_IPList
		err := q.All(&IPLists)
		if err != nil {
			logger.With(c.Request.Context()).Errorf("database query error: %v", err)
			rpError := &responseData_GetIPList{}
			rpError.Error = "Database query error."
			rpError.DB_Table_IPLists =  make([] DB_Table_IPList, 0)
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		/*
		var IPListNum2 = 0
		var IPLists2 [] DB_Table_IPList
		if rd.Product != "全部" {
			sqlText = "SELECT * FROM tb_iplist WHERE ISNULL(product) "
			q2 := db.DB().NewQuery(sqlText)
			err = q2.All(&IPLists2)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("database query error: %v", err)
			} else {
				IPListNum2 = len(IPLists2)
			}
		}
		*/
		var IPListNum = len(IPLists)
		//IPListNum += IPListNum2
		if IPListNum <= 0{
			logger.With(c.Request.Context()).Infof("no result")
			rpError := &responseData_GetIPList{}
			rpError.Error = "no result"
			rpError.DB_Table_IPLists =  make([] DB_Table_IPList, 0)
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rp := &responseData_GetIPList{}
		rp.Error = ""
		for _, value := range IPLists {
			var oneIP DB_Table_IPList
			oneIP.Index = value.Index
			oneIP.IP = value.IP
			oneIP.IsManager = value.IsManager
			oneIP.Comment = value.Comment
			oneIP.IsUseful = value.IsUseful
			oneIP.CustomIndex = value.CustomIndex
			oneIP.Product = value.Product
			rp.DB_Table_IPLists = append(rp.DB_Table_IPLists, oneIP)
		}
		/*
		if IPListNum2 > 0 {
			for _, value := range IPLists2 {
				var oneIP DB_Table_IPList
				oneIP.Index = value.Index
				oneIP.IP = value.IP
				oneIP.IsManager = value.IsManager
				oneIP.Comment = value.Comment
				oneIP.IsUseful = value.IsUseful
				oneIP.CustomIndex = value.CustomIndex
				oneIP.Product = value.Product
				rp.DB_Table_IPLists = append(rp.DB_Table_IPLists, oneIP)
			}
		}
		*/
		b, err := json.Marshal(rp)
		if err != nil {
			logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
			return err
		}
		return c.Write(json.RawMessage(string(b)))
	}
}
