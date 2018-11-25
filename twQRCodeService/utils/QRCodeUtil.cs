using System;
using System.Collections.Generic;
using log4net;
using twQRCodeService.MODEL;

namespace twQRCodeService.utils
{
    public class QRCodeUtil
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(QRCodeUtil));

        //判断二维码是否重复
        public static bool Contain(string result,List<QRCodeMsg> qrCodeMsgList)
        {
            QRCodeMsg qrCodeMsg = new QRCodeMsg();
            qrCodeMsg = ConvertToQRCodeMsg(result);
            for (int i=0; i< qrCodeMsgList.Count;i++ )
            {
                try
                {
                    if (qrCodeMsg.guid.Equals(qrCodeMsgList[i].guid))
                    {
                        return true;
                    }
                }
                catch (Exception e)
                { return false; }
            }

            return false;
        }

        //将扫描到的二维码字符串转化为QRCodeMsg对象
        public static QRCodeMsg ConvertToQRCodeMsg(string result)
        {
            QRCodeMsg qrCodeMsg = new QRCodeMsg();
            string[] strTemp = result.Trim().Replace('，',',').Split(',');
            if (strTemp.Length >= 4)
            {
                qrCodeMsg.name = strTemp[0];
                qrCodeMsg.jqCode = strTemp[1];
                qrCodeMsg.xw = strTemp[2];
                qrCodeMsg.guid = strTemp[3];
                qrCodeMsg.creatTime = new DateTime();
            }
            return qrCodeMsg;
        }
    }
}
