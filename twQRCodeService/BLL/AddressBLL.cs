using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using twQRCodeService.utils;
using twQRCodeService.MODEL;
using twQRCodeService.DAL;
using System.Threading.Tasks;
using System.Collections;
using System.Threading;

namespace twQRCodeService.BLL
{
    public class AddressBLL
    {
        static AddrMsgProcessor process = new AddrMsgProcessor();
        /// <summary>
        /// 将外网二维码中包含的地址列表数据插入数据库
        /// </summary>
        public static void InsertAddress()
        {
            if (CacheUtil.addrList != null && CacheUtil.addrList.Count > 100)
            {
                List<Address> list = new List<Address>();
                lock (CacheUtil.addrList)
                {
                    foreach (Address var in CacheUtil.addrList)
                    {
                        list.Add(var);
                    }
                    CacheUtil.addrList.Clear();
                }
                //AddressDAL.InsertMultiple(list);
                //new Task(() => { AddressDAL.InsertMultiple(list); }).Start();
                process.Execute(list);
            }
        }
    }

    class AddrMsgProcessor
    {
        //消息队列
        private Queue reportQueue = Queue.Synchronized(new Queue());
        private Task reportThread = null;

        AutoResetEvent autoEvent;
        public AddrMsgProcessor()
        {
            try
            {
                reportThread = new Task(() => { ExecuteThread(); });
                reportThread.Start();
                autoEvent = new AutoResetEvent(false);
            }
            catch (Exception e) { }
        }
        public void Execute(List<Address> list)
        {
            try
            {
                reportQueue.Enqueue(list);
                //autoEvent.Reset();
                autoEvent.Set();
            }
            catch (Exception e)
            { }
        }

        private void ExecuteThread()
        {
            try
            {
                while (true)
                {
                    if (reportQueue.Count > 0)
                    {
                        List<Address> list = (List<Address>)reportQueue.Dequeue();
                        if (list != null)
                        {
                            ExecuteReport(list);
                        }
                    }
                    else
                    {
                        autoEvent.WaitOne();
                    }
                }
            }
            catch (Exception e)
            { }
        }
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="dataMsg"></param>
        public void ExecuteReport(List<Address> list)
        {
            try
            {
                AddressDAL.InsertMultiple(list);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.StackTrace.ToString());
            }

        }
        #region IProcessor 成员
        public void DestoryThread()
        {
            if (reportThread != null)
            {
                try
                {
                    reportThread.Dispose();
                    //reportThread.Abort();
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e.StackTrace.ToString());
                }
            }
            autoEvent.Close();
        }
        #endregion
    }
}
