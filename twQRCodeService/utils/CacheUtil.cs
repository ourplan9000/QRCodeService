using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using twQRCodeService.MODEL;

namespace twQRCodeService.utils
{
    public class CacheUtil
    {
        public static List<Address> addrList = new List<Address>();
        public static List<QRCodeMsg> innerCodeList = new List<QRCodeMsg>();
        public static List<QRCodeShowMsg> outerCodeList = new List<QRCodeShowMsg>();

        /// <summary>
        /// 将扫描到的内网二维码内容存入到集合
        /// </summary>
        /// <param name="innerCode"></param>
        public static void AddInnerCode(string innerCode)
        {
            string[] strTemp = null;
            if (!string.IsNullOrWhiteSpace(innerCode))
            {
                strTemp = innerCode.Split(',');
            }
            if (strTemp != null && strTemp.Length >= 4)
            {
                QRCodeMsg entity = new QRCodeMsg();
                entity.name = strTemp[0];
                entity.jqCode = strTemp[1];
                entity.xw = strTemp[2];
                entity.guid = strTemp[3];
                entity.creatTime = new DateTime();
                entity.id = Guid.NewGuid().ToString("N");
                CacheUtil.innerCodeList.Add(entity);
            }
        }

        /// <summary>
        /// 将服务端返回的查询结果存入集合
        /// </summary>
        /// <param name="outerCode"></param>
        public static void AddOuterCodeAndAddress(string outerCode)
        {
            string[] strTemp = null;
            if (!string.IsNullOrWhiteSpace(outerCode) && outerCode.Contains("^"))
            {
                strTemp = outerCode.Split('^');
            }
            if (strTemp != null && strTemp.Length >= 2)
            {
                string guid = Guid.NewGuid().ToString("N");
                QRCodeShowMsg entity = new QRCodeShowMsg();
                string[] strTemp2 = strTemp[0].Split(',');
                entity.jqCode = strTemp[0];
                entity.xw = strTemp[1];
                entity.id = guid;
                CacheUtil.outerCodeList.Add(entity);

                //存入地址
                for (int i = 1; i < strTemp.Length; i++)
                {
                    string[] strTemp3 = strTemp[i].Split(',');
                    Address addr = new Address();
                    addr.name = strTemp3[0];
                    addr.lat = strTemp3[1];
                    addr.lng = strTemp3[2];
                    addr.id = Guid.NewGuid().ToString("N");
                    addr.outerCodeId = guid;
                    CacheUtil.addrList.Add(addr);
                }
            }
        }
    }
}
