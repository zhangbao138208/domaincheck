using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FK域名检测工具管理器
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

        public string ToJson()
        {
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

    public class GetLogCountRequest {
        [JsonProperty("check_time_start")]
        public string CheckTimeStart;
        [JsonProperty("check_time_end")]
        public string CheckTimeEnd;
        [JsonProperty("domain")]
        public string Domain;
        [JsonProperty("check_result")]
        public string CheckResult;
        [JsonProperty("creator")]
        public string Creator;
        [JsonProperty("product")]
        public string Product;
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class GetLogCountResponse
    {
        [JsonProperty("error")]
        public string Error;
        [JsonProperty("total_count")]
        public int TotalCount;
    }

    public class GetPageLogRequest
    {
        [JsonProperty("check_time_start")]
        public string CheckTimeStart;
        [JsonProperty("check_time_end")]
        public string CheckTimeEnd;
        [JsonProperty("domain")]
        public string Domain;
        [JsonProperty("check_result")]
        public string CheckResult;
        [JsonProperty("product")]
        public string Product;
        [JsonProperty("creator")]
        public string Creator;
        [JsonProperty("page")]
        public int Page;
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class Log 
    {
        [JsonProperty("log_index")]
        public int LogIndex { get; set; }
        [JsonProperty("domain")]
        public string Domain { get; set; }
        [JsonProperty("check_ip")]
        public string CheckIP { get; set; }
        [JsonProperty("creator")]
        public string Creator { get; set; }
        [JsonProperty("check_date")]
        public string CheckDate { get; set; }
        [JsonProperty("result")]
        public string Result { get; set; }
        [JsonProperty("printscreen")]
        public byte[] PrintScreen { get; set; }
    }

    public class GetPageLogResponse {
        [JsonProperty("error")]
        public string Error;
        [JsonProperty("logs")]
        public Log [] Logs { get; set; }
    }


    public class GetDomainCountRequest
    {
        [JsonProperty("check_time_start")]
        public string CheckTimeStart;
        [JsonProperty("check_time_end")]
        public string CheckTimeEnd;
        [JsonProperty("domain")]
        public string Domain;
        [JsonProperty("is_need_check")]
        public int IsNeedCheck;
        [JsonProperty("product")]
        public string Product;
        [JsonProperty("checkpoint_index")]
        public string CheckpointIndex;
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class GetDomainCountResponse
    {
        [JsonProperty("error")]
        public string Error;
        [JsonProperty("total_count")]
        public int TotalCount;
    }

    public class GetPageDomainRequest
    {
        [JsonProperty("check_time_start")]
        public string CheckTimeStart;
        [JsonProperty("check_time_end")]
        public string CheckTimeEnd;
        [JsonProperty("domain")]
        public string Domain;
        [JsonProperty("is_need_check")]
        public int IsNeedCheck;
        [JsonProperty("product")]
        public string Product;
        [JsonProperty("checkpoint_index")]
        public string CheckpointIndex;
        [JsonProperty("page")]
        public int Page;
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class DomainInfo
    {
        [JsonProperty("domain_index")]
        public int DomainIndex { get; set; }
        [JsonProperty("domain")]
        public string Domain { get; set; }
        [JsonProperty("is_need_check")]
        public int IsNeedCheck { get; set; }
        [JsonProperty("checkpoint_index")]
        public int CheckpointIndex { get; set; }
        [JsonProperty("product")]
        public string Product { get; set; }
        [JsonProperty("creator")]
        public string Creator { get; set; }
        [JsonProperty("update_date")]
        public string UpdateDate { get; set; }
        [JsonProperty("comment")]
        public string Comment { get; set; }
    }

    public class GetPageDomainResponse
    {
        [JsonProperty("error")]
        public string Error;
        [JsonProperty("domains")]
        public DomainInfo[] Domains { get; set; }
    }

    public class UpdateDomainCheckedRequest 
    {
        [JsonProperty("domain_index")]
        public int DomainIndex { get; set; }
        [JsonProperty("is_need_check")]
        public int IsNeedCheck { get; set; }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class UpdateDomainCheckedResponse
    {
        [JsonProperty("error")]
        public string Error;
    }

    public class GetCheckpointIndexsRequest {
        [JsonProperty("product")]
        public string Product { get; set; }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class GetCheckpointIndexsResponse {
        [JsonProperty("error")]
        public string Error;
        [JsonProperty("checkpoint_indexs")]
        public string [] CheckpointIndexs { get; set; }
    }

    public class UpdateDomainRequest {
        [JsonProperty("domain_index")]
        public int DomainIndex { get; set; }
        [JsonProperty("domain")]
        public string Domain { get; set; }
        [JsonProperty("checkpoint_index")]
        public int CheckpointIndex { get; set; }
        [JsonProperty("product")]
        public string Product { get; set; }
        [JsonProperty("creator")]
        public string Creator { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class UpdateDomainResponse {
        [JsonProperty("error")]
        public string Error;
    }

    public class CreateDomainRequest
    {
        [JsonProperty("domain")]
        public string Domain { get; set; }
        [JsonProperty("checkpoint_index")]
        public int CheckpointIndex { get; set; }
        [JsonProperty("product")]
        public string Product { get; set; }
        [JsonProperty("creator")]
        public string Creator { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class CreateDomainResponse
    {
        [JsonProperty("error")]
        public string Error;
    }

    public class DeleteDomainRequest
    {
        [JsonProperty("domain_index")]
        public int DomainIndex { get; set; }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class DeleteDomainResponse
    {
        [JsonProperty("error")]
        public string Error;
    }

    public class GetCPCountRequest
    {
        [JsonProperty("product")]
        public string Product;
        [JsonProperty("creator")]
        public string Creator;
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class GetCPCountResponse
    {
        [JsonProperty("error")]
        public string Error;
        [JsonProperty("total_count")]
        public int TotalCount;
    }

    public class GetPageCPRequest
    {
        [JsonProperty("product")]
        public string Product;
        [JsonProperty("creator")]
        public string Creator;
        [JsonProperty("page")]
        public int Page;
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class CPInfo
    {
        [JsonProperty("checkpoint_index")]
        public int CheckpointIndex { get; set; }
        [JsonProperty("product")]
        public string Product { get; set; }
        [JsonProperty("creator")]
        public string Creator { get; set; }
        [JsonProperty("check_path")]
        public string CheckPath { get; set; }
        [JsonProperty("check_string")]
        public string CheckString { get; set; }
    }

    public class GetPageCPResponse
    {
        [JsonProperty("error")]
        public string Error;
        [JsonProperty("checkpoints")]
        public CPInfo[] CPInfoList { get; set; }
    }

    public class UpdateCPRequest
    {
        [JsonProperty("checkpoint_index")]
        public int CheckpointIndex { get; set; }
        [JsonProperty("product")]
        public string Product { get; set; }
        [JsonProperty("creator")]
        public string Creator { get; set; }
        [JsonProperty("check_string")]
        public string CheckSting { get; set; }
        [JsonProperty("check_path")]
        public string CheckPath { get; set; }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class UpdateCPResponse
    {
        [JsonProperty("error")]
        public string Error;
    }

    public class CreateCPRequest
    {
        [JsonProperty("product")]
        public string Product { get; set; }
        [JsonProperty("creator")]
        public string Creator { get; set; }
        [JsonProperty("check_string")]
        public string CheckSting { get; set; }
        [JsonProperty("check_path")]
        public string CheckPath { get; set; }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class CreateCPResponse
    {
        [JsonProperty("error")]
        public string Error;
    }

    public class DeleteCPRequest
    {
        [JsonProperty("checkpoint_index")]
        public int CheckpointIndex { get; set; }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class DeleteCPResponse
    {
        [JsonProperty("error")]
        public string Error;
    }

    public class ActiveClientInfo
    {
        [JsonProperty("client_id")]
        public string ClientID { get; set; }
        [JsonProperty("product")]
        public string Product { get; set; }
        [JsonProperty("check_date")]
        public string CheckDate { get; set; }
    }

    public class GetActiveClientsRequest
    {
        [JsonProperty("product")]
        public string Product { get; set; }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class GetActiveClientsResponse
    {
        [JsonProperty("error")]
        public string Error;
        [JsonProperty("active_client_infos")]
        public ActiveClientInfo[] ActiveClientInfos { get; set; }
    }

    public class IPListInfo {
        [JsonProperty("index")]
        public int Index { get; set; }
        [JsonProperty("ip")]
        public string IP { get; set; }
        [JsonProperty("is_manager")]
        public int IsManager { get; set; }
        [JsonProperty("comment")]
        public string Comment { get; set; }
        [JsonProperty("is_useful")]
        public int IsUseful { get; set; }
        [JsonProperty("custom_index")]
        public int CustomIndex { get; set; }
        [JsonProperty("product")]
        public string Product { get; set; }
    }

    public class GetIPListRequest {
        [JsonProperty("username")]
        public string Username;
        [JsonProperty("product")]
        public string Product;
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class GetIPListResponse {
        [JsonProperty("error")]
        public string Error;
        [JsonProperty("ip_lists")]
        public IPListInfo[] IPListInfos { get; set; }
    }

    public class DeleteIPListRequest
    {
        [JsonProperty("index")]
        public int Index { get; set; }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class DeleteIPListResponse
    {
        [JsonProperty("error")]
        public string Error;
    }

    public class UpdateIPListRequest
    {
        [JsonProperty("index")]
        public int Index { get; set; }
        [JsonProperty("ip")]
        public string IP { get; set; }
        [JsonProperty("is_manager")]
        public int IsManager { get; set; }
        [JsonProperty("comment")]
        public string Comment { get; set; }
        [JsonProperty("is_useful")]
        public int IsUseful { get; set; }
        [JsonProperty("custom_index")]
        public int CustomIndex { get; set; }
        [JsonProperty("product")]
        public string Product { get; set; }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class UpdateIPListResponse
    {
        [JsonProperty("error")]
        public string Error;
    }

    public class DeleteAllDomainsRequest
    {
        [JsonProperty("product_list")]
        public string ProductList { get; set; }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class DeleteAllDomainsResponse
    {
        [JsonProperty("error")]
        public string Error;
    }
}
