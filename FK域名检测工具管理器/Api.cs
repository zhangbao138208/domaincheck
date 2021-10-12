namespace FK域名检测工具管理器
{
    public static class Api
    {
        private static string Ip =>  IniConfigMgr.IniInstance.LoadConfig("ServerIP");
        private static string Protocol => "https";
        
        public static string Login => $"{Protocol}://{Ip}/v1/Login";
        public static string GetPageLog => $"{Protocol}://{Ip}/v1/GetPageLog";
        public static string GetLogsCount => $"{Protocol}://{Ip}/v1/GetLogsCount";
        public static string GetDomainsCount => $"{Protocol}://{Ip}/v1/GetDomainsCount";
        public static string GetPageDomain => $"{Protocol}://{Ip}/v1/GetPageDomain";
        public static string GetCheckPointIndex => $"{Protocol}://{Ip}/v1/GetCheckpointIndexs";
        public static string DeleteDomain => $"{Protocol}://{Ip}/v1/DeleteDomain";
        public static string UpdateDomainChecked => $"{Protocol}://{Ip}/v1/UpdateDomainChecked";
        public static string GetCheckpointsCount => $"{Protocol}://{Ip}/v1/GetCheckpointsCount";
        public static string GetPageCheckpoint => $"{Protocol}://{Ip}/v1/GetPageCheckpoint";
        public static string DeleteCheckpoint => $"{Protocol}://{Ip}/v1/DeleteCheckpoint";
        public static string GetActiveClients => $"{Protocol}://{Ip}/v1/GetActiveClients";
        public static string GetIpList => $"{Protocol}://{Ip}/v1/GetIPLists";
        public static string UpdateIpList => $"{Protocol}://{Ip}/v1/UpdateIPList";
        public static string DeleteIpList => $"{Protocol}://{Ip}/v1/DeleteIPList";
        public static string DeleteAllDomains => $"{Protocol}://{Ip}/v1/DeleteAllDomains";
    }
}