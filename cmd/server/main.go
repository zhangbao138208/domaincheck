package main

import(
	"flag"
	"os"
	"fmt"
	"time"
	"context"
	"database/sql"
	"net/http"

	"github.com/go-ozzo/ozzo-dbx"
	_ "github.com/go-sql-driver/mysql"

	"github.com/go-ozzo/ozzo-routing/v2"
	"github.com/go-ozzo/ozzo-routing/v2/content"
	"github.com/go-ozzo/ozzo-routing/v2/cors"
	"github.com/robfig/cron"

	"pkg/log"
	"pkg/accesslog"
	"pkg/dbcontext"

	"local/config"
	_ "local/album"
	_ "local/auth"
	"local/healthcheck"
	"local/errors"
	"local/controller"
)

var Version = "1.0.6"
// 1.0.1	增加空域名监测，增加域名前后空格去除功能
// 1.0.2	域名接口增加了 多监测点 查询支持
// 1.0.3	增加服务器接口的IP限制
// 1.0.4 	增加客户端分编号任务分配
// 1.0.5 	9月21日批量大修改
// 1.0.6 	去除数据库库名限制，避免不同库名导致异常
var AppConfig = flag.String("config", "./config/dev.yml", "path to the config file")

func main(){
	// parse command line args.
	flag.Parse()
	fmt.Printf("args=%s, num=%d\n", flag.Args(), flag.NArg())
    for i := 0; i != flag.NArg(); i++ {
        fmt.Printf("arg[%d] = %s\n", i, flag.Arg(i))
    }
	// create logger with server's version.
	logger := log.New().With(nil, "version", Version)
	logger.Info("server init...")

	// load application's configurations.
	cfg, err := config.Load(*AppConfig, logger)
	if err != nil {
		logger.Errorf("failed to load application configuration: %s", err)
		os.Exit(-1)
	}

	// connect to the database.
	db, err := dbx.MustOpen("mysql", cfg.DSN)
	if err != nil {
		logger.Errorf("failed to connect database: %s", err)
		os.Exit(-1)
	}
	// register callback funcions.
	db.QueryLogFunc = logDBQuery(logger)
	db.ExecLogFunc = logDBExec(logger)

	// register to close database's connect.
	defer func() {
		if err := db.Close(); err != nil {
			logger.Error(err)
		}
	}()

	// creater timer
	c := cron.New(cron.WithSeconds())
	// spec := "*/1 * * * * ?"
	spec := "00 00 04 * * ?"
	c.AddFunc(spec, func(){
		tm := time.Now().AddDate(0, 0, -3)
		stm := tm.Format("2006-01-02 15:04:05")
		_, err = db.Delete("tb_logs", dbx.NewExp("check_date<'"+stm+"'")).Execute()
		if err != nil{
			fmt.Println("if error occurs here, you should open MYSQL safe mode. Ex: SET SQL_SAFE_UPDATES = 0;")
			return
		}
	})
	c.Start()
	defer c.Stop()

	// create HTTP server.
	address := fmt.Sprintf(":%v", cfg.ServerPort)
	hs := &http.Server{
		Addr:    address,
		Handler: HTTPHandler(logger, dbcontext.New(db), cfg),
	}

	// start HTTP server and registe for shutdown.
	go routing.GracefulShutdown(hs, 10*time.Second, logger.Infof)
	logger.Infof("server %v is running at %v", Version, address)

	if err := hs.ListenAndServe(); err != nil && err != http.ErrServerClosed {
		logger.Error(err)
		os.Exit(-1)
	}
}

func HTTPHandler(logger log.Logger, db *dbcontext.DB, cfg *config.Config) http.Handler {
	router := routing.New()
	router.Use(
		accesslog.Handler(logger),
		errors.Handler(logger),
		content.TypeNegotiator(content.JSON),
		cors.Handler(cors.AllowAll),
	)

	// register health check handler.
	// if we want add more handlers with no groups, pls see ref: internal/healthcheck/api.go
	healthcheck.RegisterHandlers(router, Version)

	// create v1 router group
	rg_v1 := router.Group("/v1")

	/* if you need JWT auth, open this comment
	authHandler := auth.Handler(cfg.JWTSigningKey)
	album.RegisterHandlers(rg_v1.Group(""),
		album.NewService(album.NewRepository(db, logger), logger),
		authHandler, logger,
	)
	auth.RegisterHandlers(rg_v1.Group(""),
		auth.NewService(cfg.JWTSigningKey, cfg.JWTExpiration, logger),
		logger,
	)
	*/

	// my core http msg handler code.

	// for client handler
	controller.RegisterLoginHandlers(rg_v1.Group(""), logger, db)
	controller.RegisterGetDomainHandlers(rg_v1.Group(""), logger, db)
	controller.RegisterCheckResultHandlers(rg_v1.Group(""), logger, db)

	// for server handler
	controller.RegisterGetLogsCountHandlers(rg_v1.Group(""), logger, db)
	controller.RegisterGetPageLogHandlers(rg_v1.Group(""), logger, db)

	controller.RegisterGetDomainsCountHandlers(rg_v1.Group(""), logger, db)
	controller.RegisterGetPageDomainHandlers(rg_v1.Group(""), logger, db)
	controller.RegisterCreateDomainHandlers(rg_v1.Group(""), logger, db)
	controller.RegisterUpdateDomainHandlers(rg_v1.Group(""), logger, db)
	controller.RegisterUpdateDomainCheckedHandlers(rg_v1.Group(""), logger, db)
	controller.RegisterDeleteDomainHandlers(rg_v1.Group(""), logger, db)
	controller.RegisterGetCheckpointIndexsHandlers(rg_v1.Group(""), logger, db)
	controller.RegisterDeleteAllDomainsHandlers(rg_v1.Group(""), logger, db)

	controller.RegisterGetCheckpointsCountHandlers(rg_v1.Group(""), logger, db)
	controller.RegisterGetPageCheckpointHandlers(rg_v1.Group(""), logger, db)
	controller.RegisterCreateCheckpointHandlers(rg_v1.Group(""), logger, db)
	controller.RegisterUpdateCheckpointHandlers(rg_v1.Group(""), logger, db)
	controller.RegisterDeleteCheckpointHandlers(rg_v1.Group(""), logger, db)

	// no safe check apis
	controller.RegisterGetActiveClientsHandlers(rg_v1.Group(""), logger, db)
	controller.RegisterGetIPListHandlers(rg_v1.Group(""), logger, db)
	controller.RegisterDeleteIPListHandlers(rg_v1.Group(""), logger, db)
	controller.RegisterUpdateIPListHandlers(rg_v1.Group(""), logger, db)

	// for test
	//controller.RegisterEchoIP(rg_v1.Group(""), logger, db)

	/* test code
	rg_v1.Get("/test1", func(c *routing.Context) error {
		return c.Write("GET example")
	})
	rg_v1.Get("/test2/<id>", func (c *routing.Context) error {
		fmt.Fprintf(c.Response, "ID: %v", c.Param("id"))
		return c.Write("example with params")
	})
	rg_v1.Post("/test3", func(c *routing.Context) error {
		data := &struct{
			A string
			B bool
		}{}
		// assume the body data is: {"A":"abc", "B":true}
		// data will be populated as: {A: "abc", B: true}
		if err := c.Read(&data); err != nil {
			return err
		}
		return c.Write("POST example")
	})
	// only accept numbers id.
	rg_v1.Put(`/test4/<id:\d+>`, func(c *routing.Context) error {
		return c.Write("example with limit params: " + c.Param("id"))
	})
	*/


	return router
}



// logDBQuery returns a logging function that can be used to log SQL queries.
func logDBQuery(logger log.Logger) dbx.QueryLogFunc {
	return func(ctx context.Context, t time.Duration, sql string, rows *sql.Rows, err error) {
		if err == nil {
			logger.With(ctx, "duration", t.Milliseconds(), "sql", sql).Info("DB query successful")
		} else {
			logger.With(ctx, "sql", sql).Errorf("DB query error: %v", err)
		}
	}
}

// logDBExec returns a logging function that can be used to log SQL executions.
func logDBExec(logger log.Logger) dbx.ExecLogFunc {
	return func(ctx context.Context, t time.Duration, sql string, result sql.Result, err error) {
		if err == nil {
			logger.With(ctx, "duration", t.Milliseconds(), "sql", sql).Info("DB execution successful")
		} else {
			logger.With(ctx, "sql", sql).Errorf("DB execution error: %v", err)
		}
	}
}