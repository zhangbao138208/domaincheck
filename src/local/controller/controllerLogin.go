package controller

import (
	"encoding/json"
	"github.com/go-ozzo/ozzo-dbx"
	routing "github.com/go-ozzo/ozzo-routing/v2"
	_ "github.com/go-sql-driver/mysql"
	"pkg/dbcontext"
	"pkg/log"
)

// 用户登录
type requestData_login struct{
	UserName string `json:"username"`
	Password string `json:"password"`
	IsManager string `json:"is_manager"`
	MAC string `json:"mac"`
}

type responseData_login struct{
	Error string `json:"error"`
	Product string `json:"product"`
	UserName string `json:"username"`
	CustomIndex int `json:"custom_index"`
}


func RegisterLoginHandlers(rg *routing.RouteGroup, logger log.Logger, db *dbcontext.DB) {
	rg.Post("/Login", loginHandler(logger, db))
}

func loginHandler(logger log.Logger, db *dbcontext.DB) routing.Handler {
	return func(c *routing.Context) error {
		// 消息解析
		rd := requestData_login{}
		if err := c.Read(&rd); err != nil {
			logger.With(c.Request.Context()).Errorf("invalid request: %v", err)
			rpError := &responseData_login{}
			rpError.Error = "Request data not correct."
			rpError.Product = ""
			rpError.UserName = ""
			rpError.CustomIndex = 0
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}


		var customIndex = 0
		if rd.IsManager == "0" {
			// 监测MAC
			decryptMAC, err := SimpleAesDecrypt(rd.MAC)
			if err != nil {
				rpError := &responseData_login{}
				rpError.Error = "MAC not correct."
				rpError.Product = ""
				rpError.UserName = ""
				rpError.CustomIndex = 0
				b, err := json.Marshal(rpError)
				if err != nil {
					logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
					return err
				}
				return c.Write(json.RawMessage(string(b)))
			}
			q := db.DB().Select("index", "ip", "is_manager", "comment", "is_useful", "custom_index").
				From("tb_iplist").
				Where(dbx.HashExp{"ip": decryptMAC, "is_manager": 0})
			var ipLists [] DB_Table_IPList
			err = q.All(&ipLists)
			// 查询出错
			if err != nil{
				rpError := &responseData_login{}
				rpError.Error = "IP not allowed." 
				rpError.Product = ""
				rpError.UserName = ""
				rpError.CustomIndex = 0
				b, err := json.Marshal(rpError)
				if err != nil {
					logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
					return err
				}
				return c.Write(json.RawMessage(string(b)))
			}
			// 无数据
			var ipListNum = len(ipLists)
			if ipListNum <= 0{
				_, err = db.DB().Insert("tb_iplist", dbx.Params{
					"ip": decryptMAC,
					"is_manager": 0,
					"comment": "",
					"is_useful": 0,
					"custom_index": 0}).Execute()
				//if err != nil{
					rpError := &responseData_login{}
					rpError.Error = "IP not allowed, please connect server administrator for open white list."
					rpError.Product = ""
					rpError.UserName = ""
					rpError.CustomIndex = 0
					b, err := json.Marshal(rpError)
					if err != nil {
						logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
						return err
					}
					return c.Write(json.RawMessage(string(b)))
				//}
			}
			// 有数据，但仍然无效
			if ipLists[0].IsUseful == 0 {
				rpError := &responseData_login{}
				rpError.Error = "IP not allowed, please connect server administrator for open white list."
				rpError.Product = ""
				rpError.UserName = ""
				rpError.CustomIndex = 0
				b, err := json.Marshal(rpError)
				if err != nil {
					logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
					return err
				}
				return c.Write(json.RawMessage(string(b)))
			}

			// 查询成功
			customIndex = ipLists[0].CustomIndex
		} else if rd.IsManager == "1" {
			// 监测IP
			clientIP := GetIP(c.Request)
			q := db.DB().Select("index", "ip", "is_manager", "comment", "is_useful", "custom_index").
				From("tb_iplist").
				Where(dbx.HashExp{"ip": clientIP, "is_manager": 1})
			var ipLists [] DB_Table_IPList
			err := q.All(&ipLists)
			// 查询出错
			if err != nil{
				rpError := &responseData_login{}
				rpError.Error = "DB query failed." 
				rpError.Product = ""
				rpError.UserName = ""
				b, err := json.Marshal(rpError)
				if err != nil {
					logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
					return err
				}
				return c.Write(json.RawMessage(string(b)))
			}
			// 无数据
			var ipListNum = len(ipLists)
			if ipListNum <= 0{
				if rd.UserName == "admin" {
					_, err = db.DB().Insert("tb_iplist", dbx.Params{
						"ip": clientIP,
						"is_manager": 1,
						"comment": "",
						"is_useful": 1,
						"custom_index": 0}).Execute()
				} else {
					_, err = db.DB().Insert("tb_iplist", dbx.Params{
						"ip": clientIP,
						"is_manager": 1,
						"comment": "",
						"is_useful": 0,
						"custom_index": 0}).Execute()
					
					rpError := &responseData_login{}
					rpError.Error = "IP not allowed, please connect server administrator for open white list."
					rpError.Product = ""
					rpError.UserName = ""
					rpError.CustomIndex = 0
					b, err := json.Marshal(rpError)
					if err != nil {
						logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
						return err
					}
					return c.Write(json.RawMessage(string(b)))
				}
			} else if ipLists[0].IsUseful == 0 {
				if rd.UserName == "admin" {
					_, err = db.DB().Update("tb_iplist", dbx.Params{"is_useful": 1}, 
						dbx.HashExp{"index": ipLists[0].Index}).Execute()
				} else {
					// 有数据，但仍然无效
					rpError := &responseData_login{}
					rpError.Error = "IP not allowed, please connect server administrator for open white list."
					rpError.Product = ""
					rpError.UserName = ""
					rpError.CustomIndex = 0
					b, err := json.Marshal(rpError)
					if err != nil {
						logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
						return err
					}
					return c.Write(json.RawMessage(string(b)))
				}
			}		
		} else {
			// 拒绝
			rpError := &responseData_login{}
			rpError.Error = "Error data."
			rpError.Product = ""
			rpError.UserName = ""
			rpError.CustomIndex = 0
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}


		decryptPassword, err := SimpleAesDecrypt(rd.Password)
		if err != nil {
			rpError := &responseData_login{}
			rpError.Error = "LoginName or password not correct."
			rpError.Product = ""
			rpError.UserName = ""
			rpError.CustomIndex = 0
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}
		q := db.DB().Select("username", "password", "product", "is_manager").
			From("tb_users").
			Where(dbx.HashExp{"username": rd.UserName, "password": decryptPassword}).
			OrderBy("username")

		// 查询失败
		var users [] DB_table_users
		err = q.All(&users)
		if err != nil {
			logger.With(c.Request.Context()).Errorf("database query error: %v", err)
			rpError := &responseData_login{}
			rpError.Error = "LoginName or password not correct."
			rpError.Product = ""
			rpError.UserName = ""
			rpError.CustomIndex = 0
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		// 无数据
		var usersNum = len(users)
		if usersNum <= 0 {
			logger.With(c.Request.Context()).Errorf("database query error: %v", err)
			rpError := &responseData_login{}
			rpError.Error = "LoginName or password not correct."
			rpError.Product = ""
			rpError.UserName = ""
			rpError.CustomIndex = 0
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}

		// 客户端和管理员身份不匹配
		if users[0].IsManager != rd.IsManager {
			rpError := &responseData_login{}
			rpError.Error = "Permission not correct."
			rpError.Product = ""
			rpError.UserName = ""
			rpError.CustomIndex = 0
			b, err := json.Marshal(rpError)
			if err != nil {
				logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
				return err
			}
			return c.Write(json.RawMessage(string(b)))
		}
		
		rp := &responseData_login{}
		rp.Product = users[0].Product
		rp.UserName = rd.UserName
		rp.CustomIndex = customIndex
		rp.Error = ""	
		b, err := json.Marshal(rp)
		if err != nil {
			logger.With(c.Request.Context()).Errorf("response format to json error: %v", err)
			return err
		}
		return c.Write(json.RawMessage(string(b)))
    }
}