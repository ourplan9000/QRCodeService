using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using twQRCodeService.MODEL;
using twQRCodeService.DBHelper;

namespace twQRCodeService.DAL
{
    public class OuterCodeDAL
    {
        /// <summary>
        /// 外网二维码记录，增加新数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns>影响行数</returns>
        public static int InsertMultiple(List<QRCodeShowMsg> list)
        {
            int result = OracleMapperSql.InsertMultiple(@"INSERT INTO outerCode (ID,jqCode,xw) VALUES (:ID,:jqCode,:xw)", list, null);
            return result;
        }
    }
}
