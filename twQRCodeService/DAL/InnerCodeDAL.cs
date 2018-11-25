using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using twQRCodeService.MODEL;
using twQRCodeService.DBHelper;

namespace twQRCodeService.DAL
{
    public class InnerCodeDAL
    {
        /// <summary>
        /// 内网二维码记录，增加新数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns>影响行数</returns>
        public static int InsertMultiple(List<QRCodeMsg> list)
        {
            int result = OracleMapperSql.InsertMultiple(@"INSERT INTO innerCode (ID,name,jqCode,xw,guid,creatTime) VALUES (:ID,:name,:jqCode,:xw,:guid,:creatTime)", list, null);
            return result;
        }
    }
}
