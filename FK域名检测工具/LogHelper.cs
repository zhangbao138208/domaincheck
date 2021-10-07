using log4net;
using System;

namespace FK域名检测工具
{
    public static class LogHelper
    {
        static LogHelper()
        {
           
             Log = LogManager.GetLogger("FK域名检测工具");
        }

        private static readonly ILog Log;

        public static void Info(string msg, Type type)
        {
            Info($"{type.Name} - {msg}");
        }

        public static void Info(string msg)
        {
            Log.Info(msg);
        }

        public static void Error(string msg, Exception ex, Type type)
        {
            Error($"{type.Name} - {msg}", ex);
        }

        public static void Error(string msg, Exception ex)
        {
            Log.Error(msg, ex);
        }

        public static void Error(string msg)
        {
            Log.Error(msg);
        }

        public static void Debug(string msg, Type type)
        {
            Debug($"{type.Name} - {msg}");
        }

        public static void Debug(string msg)
        {
            Log.Debug(msg);
        }
    }
}
