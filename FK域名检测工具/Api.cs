namespace FK域名检测工具
{
    public static class Api
    {
        private static string Ip => IniConfigMgr.IniInstance.LoadConfig("ServerIP", false);
        private static string Protocol => "https";
        
        public static string Login => $"{Protocol}://{Ip}/v1/Login";
        public static string GetDomain => $"{Protocol}://{Ip}/v1/GetDomain";
        public static string SyncCheckResult => $"{Protocol}://{Ip}/v1/SyncCheckResult";
    }
}