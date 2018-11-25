using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using twQRCodeService.MODEL;
using twQRCodeService.DBHelper;

namespace twQRCodeService.DAL
{
    public class AddressDAL
    {
        /// <summary>
        /// 外网二维码地址信息记录，增加新数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns>影响行数</returns>
        public static int InsertMultiple(List<Address> list)
        {
            int result = OracleMapperSql.InsertMultiple(@"INSERT INTO point (ID,name,lat,lng,outerCodeId) VALUES (:ID,:name,:lat,:lng,:outerCodeId)", list, null);
            return result;
        }
    }
}
