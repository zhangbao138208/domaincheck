using log4net;
using System;

namespace test
{
    public class LogHelper
    {
        static LogHelper()
        {
           
             log = LogManager.GetLogger("test");
        }
        public static log4net.ILog log = null;

        public static void Info(string msg, Type type)
        {
            Info(string.Format("{0} - {1}", type.Name, msg));
        }

        public static void Info(string msg)
        {
            log.Info(msg);
        }

        public static void Error(string msg, Exception ex, Type type)
        {
            Error(string.Format("{0} - {1}", type.Name, msg), ex);
        }

        public static void Error(string msg, Exception ex)
        {
            log.Error(msg, ex);
        }

        public static void Debug(string msg, Type type)
        {
            Debug(string.Format("{0} - {1}", type.Name, msg));
        }

        public static void Debug(string msg)
        {
            log.Debug(msg);
        }
    }
}
