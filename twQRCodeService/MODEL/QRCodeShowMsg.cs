using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace twQRCodeService.MODEL
{
    /// <summary>
    /// 从服务端返回的二维码内容
    /// </summary>
    public class QRCodeShowMsg
    {
        public string id { set; get; }
        //警情编码
        public string jqCode { get; set; }
        //席位号
        public string xw { get; set; }
        //地址列表
        public List<Address> addrList { set; get; }

    }
}
