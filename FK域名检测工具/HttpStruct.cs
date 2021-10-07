using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FK域名检测工具
{
    public class LoginRequest
    {
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("is_manager")]
        public string IsManager { get; set; }
        [JsonProperty("mac")]
        public string MAC { get; set; }

        public string ToJson() {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class LoginResponse 
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("product")]
        public string Product { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("custom_index")]
        public int CustomIndex { get; set; }
    }

    public class GetDomainRequest 
    {
        [JsonProperty("products")]
        public string[] Products { get; set; }
        [JsonProperty("token")]
        public string MAC { get; set; }
        [JsonProperty("custom_index")]
        public int CustomIndex { get; set; }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class DomainCheckCondition
    {
        [JsonProperty("domain")]
        public string Domain { get; set; }
        [JsonProperty("check_path")]
        public string CheckPath { get; set; }
        [JsonProperty("check_string")]
        public string CheckString { get; set; }
        [JsonProperty("creator")]
        public string Creator { get; set; }
        [JsonProperty("product")]
        public string Product { get; set; }
    }

    public class GetDomainResponse 
    {
        [JsonProperty("error")]
        public string Error { get; set; }
        [JsonProperty("domain_check_conditions")]
        public DomainCheckCondition[] DomainCheckConditions { get; set; }
    }

    public class CheckDomainResultRequest
    {
        [JsonProperty("domain")]
        public string Domain;
        [JsonProperty("check_ip")]
        public string CheckIP;
        [JsonProperty("creator")]
        public string Creator;
        [JsonProperty("client_id")]
        public string ClientID;
        [JsonProperty("product")]
        public string Product;
        [JsonProperty("result")]
        public string Result;
        [JsonProperty("token")]
        public string MAC;
        [JsonProperty("printscreen")]
        public byte[] Printscreen;

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class CheckDomainResultResponse {
        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
