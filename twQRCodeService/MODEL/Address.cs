using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace twQRCodeService.MODEL
{
    public class Address
    {
        public string id { set; get; }

        //地址
        public string name { set; get; }
        //纬度
        public string lat { set; get; }
        //经度
        public string lng { set; get; }

        public string address { set; get; }

        public string outerCodeId { set; get; }

    }
}
