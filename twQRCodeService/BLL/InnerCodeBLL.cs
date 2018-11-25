using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using twQRCodeService.utils;
using twQRCodeService.MODEL;
using twQRCodeService.DAL;

namespace twQRCodeService.BLL
{
    public class InnerCodeBLL
    {
        /// <summary>
        /// 将内网二维码集合中数据插入数据库
        /// </summary>
        public static void InsertInnerCode()
        {
            if (CacheUtil.innerCodeList != null && CacheUtil.innerCodeList.Count > 100)
            {
                List<QRCodeMsg> list = new List<QRCodeMsg>();
                lock (CacheUtil.outerCodeList)
                {
                    foreach (QRCodeMsg var in CacheUtil.innerCodeList)
                    {
                        list.Add(var);
                    }
                    CacheUtil.innerCodeList.Clear();
                }
                //InnerCodeDAL.InsertMultiple(list);
            }
        }
    }
}
