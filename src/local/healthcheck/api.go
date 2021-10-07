package healthcheck

import(
	routing "github.com/go-ozzo/ozzo-routing/v2"
	"net"
	"net/http"
)

// RegisterHandlers registers the handlers that perform healthchecks.
func RegisterHandlers(r *routing.Router, version string) {
	r.To("GET,HEAD", "/health", healthcheck(version))
	r.To("GET,HEAD", "/getip", echoIP())
}

// healthcheck responds to a healthcheck request.
func healthcheck(version string) routing.Handler {
	return func(c *routing.Context) error {
		return c.Write("API access success, current version is " + version)
	}
}

func echoIP() routing.Handler {
	return func(c *routing.Context) error {
		clientIP := GetIP(c.Request)
		return c.Write("API access success, your IP is " + clientIP)
	}
}

func GetIP(req *http.Request) string {
	remoteAddr := req.RemoteAddr
	if ip := req.Header.Get("X-Real-Ip"); ip != ""{
		remoteAddr = ip
	} else if ip := req.Header.Get("X-Forwarded-For"); ip != ""{
		remoteAddr = ip
	} else{
		remoteAddr, _, _ = net.SplitHostPort(remoteAddr)
	}

	if remoteAddr == "::1"{
		remoteAddr = "127.0.0.1"
	}
	return remoteAddr
}