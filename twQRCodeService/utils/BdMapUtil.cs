using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using twQRCodeService.MODEL;
using System.Configuration;
using log4net;
using twQRCodeService.UIForms;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace twQRCodeService.utils
{
    /// <summary>
    /// 百度地图工具
    /// </summary>
    public class BdMapUtil
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(BdMapUtil));

        /// <summary>
        /// 显示的地址个数
        /// </summary>
        public static int pointCountSize = int.Parse(ConfigurationManager.AppSettings["pointCountSize"].ToString());

        /// <summary>
        /// 调用JS函数查询百度MapAPI获取地址
        /// tw:guid,jqCode,xw^name,lat,lng^name,lat,lng^name,lat,lng...
        /// </summary>
        /// <param name="bdStr"><PlaceSearchResponse><status>OK</status><results>...</results></PlaceSearchResponse></param>
        /// <returns></returns>
        public static void BdStrToTwStr(QRCodeMsg innerCode)
        {

            Thread.Sleep(200);
            try
            {
                Task.Factory.StartNew(() =>
                {
                    LoggerUtil.Logger(log, "INFO", DateTime.Now.ToString("yy-MM-dd HH:mm:ss:fffffff"));
                });
            }
            catch (Exception e)
            { }
            string name = innerCode.name;

            string guid = innerCode.guid;
            string jqCode = innerCode.jqCode;
            string xw = innerCode.xw;
            
            if (MainForm.main.webBrowser1.InvokeRequired)
            {   
                {   
                    MethodInvoker code = delegate
                    {
                        //执行JS函数
                        //初始化,初始化 查询范围边界信息
                        MainForm.main.webBrowser1.Document.InvokeScript("init");
                        //查询函数,该JS函数中将查询的结果集拼装成tw格式字符串，再调用C#函数 将生成及显示二维码的任务加入Task
                        MainForm.main.webBrowser1.Document.InvokeScript("search", new string[] { name, guid, jqCode, xw, pointCountSize.ToString() });
                        innerCode = null;
                    };
                    MainForm.main.webBrowser1.Invoke(code, null);
                }
            }
            else
            {
                MainForm.main.webBrowser1.Document.InvokeScript("init");
                //查询函数,该JS函数中将查询的结果集拼装成tw格式字符串，再调用C#函数 将生成及显示二维码的任务加入Task
                MainForm.main.webBrowser1.Document.InvokeScript("search", new string[] { name, guid, jqCode, xw, pointCountSize.ToString() });
                innerCode = null;
            }
        }


        /// <summary>
        /// 百度地图字符串转换为TW格式字符串
        /// tw:guid,jqCode,xw^name,lat,lng^name,lat,lng^name,lat,lng...
        /// </summary>
        /// <param name="bdStr"><PlaceSearchResponse><status>OK</status><results>...</results></PlaceSearchResponse></param>
        /// <returns></returns>
        public static void BdStrToTwStr2(string bdStr, QRCodeMsg innerCode)
        {
      
            //int pointCountSize = int.Parse(ConfigurationManager.AppSettings["pointCountSize"].ToString());

            StringBuilder sb = new StringBuilder();
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(bdStr);
            XmlNode root = null;
            XmlNodeList nodeList = null;
            XmlNodeList resultsNodeList = null;

            //tw开始拼接
            sb.Append(innerCode.guid + ",");
            sb.Append(innerCode.jqCode + ",");
            sb.Append(innerCode.xw);
            sb.Append("^");

            try
            {
                root = xdoc.SelectSingleNode("PlaceSearchResponse");
                nodeList = root.ChildNodes;
                foreach (XmlNode xn in nodeList)
                {
                    if (xn.Name.Equals("results", StringComparison.CurrentCultureIgnoreCase))
                    {
                       
                        resultsNodeList = xn.ChildNodes;

                        for (int i = 0; i < resultsNodeList.Count; i++)
                        {
                            //循环拼接 ^name,lat,lng
                            if (i > (pointCountSize - 1))
                            {
                                break;
                            }
                            XmlNode result = resultsNodeList[i];
                            //地点名称
                            XmlNode name = result["name"];
                            //坐标
                            XmlNode location = result["location"];
                            //纬度
                            XmlNode lat = location["lat"];
                            //经度
                            XmlNode lng = location["lng"];

                            //经度
                            XmlNode address = result["address"];

                            //if (!string.IsNullOrEmpty(name.InnerText.ToString().Trim()) && !string.IsNullOrEmpty(lat.InnerText.ToString().Trim()) && !string.IsNullOrEmpty(lng.InnerText.ToString().Trim()) && !string.IsNullOrEmpty(address.InnerText.ToString().Trim()))
                            //{
                                sb.Append(name.InnerText.ToString().Trim().Replace("钦州", "qz") + ",");
                                sb.Append(lat.InnerText.ToString().Trim() + ",");
                                sb.Append(lng.InnerText.ToString().Trim() + ",");
                                sb.Append(address.InnerText.ToString().Trim().Replace("钦州", "qz"));
                                sb.Append("^");

                                //将查询出的地点存入数据库中
                                Address addr = new Address();
                                addr.id = Guid.NewGuid().ToString("N");
                                addr.name = name.InnerText.ToString().Trim();
                                addr.lat = lat.InnerText.ToString().Trim();
                                addr.lng = lng.InnerText.ToString().Trim();
                                addr.address = address.InnerText.ToString().Trim();
                                CacheUtil.addrList.Add(addr);
                            //}
                        }
                    }
                }
                if (!string.IsNullOrEmpty(sb.ToString()))
                {
                    MainForm.main.SetQRCode(sb.ToString().Replace("广西壮族自治区","").Replace("广西",""));
                }
            }
            catch (XmlException e)
            {
                LoggerUtil.Logger(log, "ERROR", "解析百度查询XML异常," + e.StackTrace.ToString());
            }

        }
    }
}
