using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Configuration;

namespace twQRCodeService.utils
{
    public class LoggerUtil
    {
        //控制是否写日志的标志位,1：写日志 0：不写日志
        private static string IsDebug = ConfigurationManager.AppSettings["IsDebug"].ToString();
        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="log">日志对象</param>
        /// <param name="level">日志记录级别:FATAL,ERROR,WARN,INFO,DEBUG</param>
        /// <param name="msg">日志记录消息</param>
        public static void Logger(ILog log,string level,string msg)
        {
            if (IsDebug.Equals("1"))
            {
                switch(level)
                {
                    case "FATAL":
                        log.Fatal(msg);
                        break;
                    case "WARN":
                        log.Warn(msg);
                        break;
                    case "ERROR":
                        log.Error(msg);
                        break;
                    case "INFO":
                        log.Info(msg);
                        break;
                    case "DEBUG":
                        log.Debug(msg);
                        break;
                    default:
                        log.Debug(msg);
                        break;
                }
            }
        }
    }
}
