package controller

import (
	routing "github.com/go-ozzo/ozzo-routing/v2"
	_ "github.com/go-sql-driver/mysql"
	"pkg/dbcontext"
	"pkg/log"
	"encoding/json"
	"strings"
)

type requestData_deleteAllDomains struct{
	ProductList string `json:"product_list"`
}

type responseData_deleteAllDomains struct{
	Error string `json:"error"`
}

func RegisterDeleteAllDomainsHandlers(rg *routing.RouteGroup, logger log.Logger, db *dbcontext.DB) {
	rg.Post("/DeleteAllDomains", deleteAllDomainsHandler(logger, db))
}

func deleteAllDomainsHandler(logger log.Logger, db *dbcontext.DB) routing.Handler {
	return func(c *routing.Context) error {
		if !IsAllowedServerIP(c.Request, db) {
			rpError := &responseData_deleteDomain{}
			rpError.Error = "IP not allowed."
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rd := requestData_deleteAllDomains{}
		if err := c.Read(&rd); err != nil {
			logger.With(c.Request.Context()).Errorf("invalid request: %v", err)
			return err
		}

		str_arr := strings.Split(rd.ProductList, `,`)
		productList := ""
		for _, product := range str_arr {
			productList += "'"
			productList += product
			productList += "',"
		}
		productList = strings.TrimRight(productList, ",")

		sqlText := "DELETE FROM tb_domains WHERE product in (" + productList + ")"

		_, err := db.DB().NewQuery(sqlText).Execute()
		if err != nil{
			logger.With(c.Request.Context()).Errorf("database delete error: %v", err)
			rpError := &responseData_deleteAllDomains{}
			rpError.Error = "Delete data error."
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		rp := &responseData_deleteAllDomains{}
		rp.Error = ""
		b, err := json.Marshal(rp)
		if err != nil {
			logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
			return err
		}
		return c.Write(json.RawMessage(string(b)))
	}
}