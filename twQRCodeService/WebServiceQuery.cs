using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;
using log4net;
using System.Configuration;
using twQRCodeService.utils;
using twQRCodeService.MODEL;
using System.Threading.Tasks;
using System.Threading;

namespace twQRCodeService
{
    /// <summary>
    /// 调用百度MapAPI服务
    /// </summary>
    public class WebServiceQuery
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WebServiceQuery));
        public static void searchPlace(string qrMsg)
        {
            QRCodeMsg innerCode = new QRCodeMsg();
            //if (!string.IsNullOrEmpty(qrMsg))
            //{
            //    qrMsg = qrMsg.Replace("，", ",");
            //    innerCode = QRCodeUtil.ConvertToQRCodeMsg(qrMsg);
            //}
            //LoggerUtil.Logger(log, "INFO", innerCode.xw+"  ,"+innerCode.name);
            //BdMapUtil.BdStrToTwStr(innerCode);

            if (!string.IsNullOrEmpty(qrMsg))
            {
                //CacheUtil.AddInnerCode(qrMsg);
                //qrMsg = Base64.EncodeBase64(qrMsg.Replace("，", ","));

                qrMsg = qrMsg.Replace("，", ",");
                innerCode = QRCodeUtil.ConvertToQRCodeMsg(qrMsg);
                qrMsg = qrMsg.Replace(",", "&");

            }
            string region = ConfigurationManager.AppSettings["region"].ToString();
            string mapAPI = ConfigurationManager.AppSettings["mapAPI"].ToString();
            mapAPI = mapAPI.Replace("^region^", region);
            //qrMsg = qrMsg.Replace(",","&");
            string url = mapAPI + qrMsg;
            GetHtml(url, innerCode);
            innerCode = null;
        }


        public static string GetHtml(string URL, QRCodeMsg innerCode)
        {
            WebRequest wrt = null;
            string reader = string.Empty;
            //Thread.Sleep(200);
            try
            {
                Task.Factory.StartNew(() =>
                {
                    LoggerUtil.Logger(log, "INFO", DateTime.Now.ToString("yy-MM-dd HH:mm:ss:fffffff"));
                });
                wrt = WebRequest.Create(URL);
                //LoggerUtil.Logger(log, "ERROR", "连接服务器失败,"+e.StackTrace.ToString());
                wrt.Credentials = CredentialCache.DefaultCredentials;
                WebResponse wrp;
                wrp = wrt.GetResponse();
                reader = new StreamReader(wrp.GetResponseStream(), Encoding.GetEncoding("UTF-8")).ReadToEnd();
                //Console.WriteLine("reader=" + reader);
                //将百度xml转换为tw字符串
                //reader = BdMapUtil.BdStrToTwStr2(reader, innerCode);
                BdMapUtil.BdStrToTwStr2(reader, innerCode);
            }
            catch (Exception ex)
            {
                //throw ex;
                //LoggerUtil.Logger(log, "ERROR", "远程服务器搜索异常，" + ex.StackTrace.ToString());
            }
            finally
            {
                try
                {
                    wrt.GetResponse().Close();
                }
                catch (Exception e)
                {
                    //LoggerUtil.Logger(log, "ERROR", "关闭远程连接异常，" + e.StackTrace.ToString());
                }
            }
            //Console.WriteLine("TwStr=" + reader);
            return reader;
        }
    }
}
