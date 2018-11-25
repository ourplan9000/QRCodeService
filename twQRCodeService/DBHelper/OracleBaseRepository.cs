using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace twQRCodeService.DBHelper
{
    public class OracleBaseRepository
    {
        public static string DbUrl
        {
            get { return ConfigurationManager.ConnectionStrings["orcl"].ConnectionString; }
        }
    }
}
