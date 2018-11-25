using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using twQRCodeService.utils;
using twQRCodeService.MODEL;
using twQRCodeService.DAL;
using System.Threading.Tasks;
using System.Collections;

namespace twQRCodeService.BLL
{
    public class OuterCodeBLL
    {
        static OuterMsgProcessor process = new OuterMsgProcessor();
        /// <summary>
        /// 
        /// </summary>
        public static void InsertOuterCode()
        {
            if (CacheUtil.outerCodeList != null && CacheUtil.outerCodeList.Count > 100)
            {
                List<QRCodeShowMsg> list = new List<QRCodeShowMsg>();
                lock (CacheUtil.outerCodeList)
                {
                    foreach (QRCodeShowMsg var in CacheUtil.outerCodeList)
                    {
                        list.Add(var);
                    }
                    CacheUtil.outerCodeList.Clear();
                }
                //OuterCodeDAL.InsertMultiple(list);
                //new Task(() => { OuterCodeDAL.InsertMultiple(list); }).Start();
                process.Execute(list);
            }
        }
    }


    class OuterMsgProcessor
    {
        //消息队列
        private Queue reportQueue = Queue.Synchronized(new Queue());
        private Task reportThread = null;

        AutoResetEvent autoEvent;
        public OuterMsgProcessor()
        {
            try
            {
                reportThread = new Task(() => { ExecuteThread(); });
                reportThread.Start();
                autoEvent = new AutoResetEvent(false);
            }
            catch (Exception e) { }
        }
        public void Execute(List<QRCodeShowMsg> list)
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
                        List<QRCodeShowMsg> list = (List<QRCodeShowMsg>)reportQueue.Dequeue();
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
        public void ExecuteReport(List<QRCodeShowMsg> list)
        {
            try
            {
                OuterCodeDAL.InsertMultiple(list);
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
