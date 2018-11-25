using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace twQRCodeService.MODEL
{
    /// <summary>
    /// 从内网读取到的二维码内容
    /// </summary>
    public class QRCodeMsg : IComparer<QRCodeMsg>, IComparable<QRCodeMsg>
    {
        public string id { set; get; }
        //地址
        public string name { get; set; }
        //警情编码
        public string jqCode { get; set; }
        //席位号
        public string xw { get; set; }
        //随机码
        public string guid { get; set; }
        //创建时间
        public DateTime creatTime { get; set; }

        int IComparer<QRCodeMsg>.Compare(QRCodeMsg x, QRCodeMsg y)
        {
            return DateTime.Compare(x.creatTime, y.creatTime);
        }

        //重写排序算法，根据二维码创建的时间来升序排列，时间最早排在最前
        public int CompareTo(QRCodeMsg other)
        {
            return DateTime.Compare(this.creatTime, other.creatTime);
        }
    }
}
