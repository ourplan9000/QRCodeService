using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Collections;
using System.Threading;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
namespace twQRCodeService.UIForms
{
    public class MsgProcessor
    {
        //消息队列
        private Queue reportQueue = Queue.Synchronized(new Queue());
        private Task reportThread = null;

        AutoResetEvent autoEvent;
        public MsgProcessor()
        {
            try
            {
                reportThread = new Task(() => { ExecuteThread(); });
                reportThread.Start();
                autoEvent = new AutoResetEvent(false);
            }
            catch (Exception e) { }
        }
        public void Execute(string dataMsg)
        {
            try
            {
                reportQueue.Enqueue(dataMsg);
                //autoEvent.Reset();
                autoEvent.Set();
            }
            catch(Exception e)
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
                        string model = reportQueue.Dequeue().ToString();
                        if (model != null)
                        {
                            ExecuteReport(model);
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
        public void ExecuteReport(string dataMsg)
        {
            try
            {
                //MainForm.SetQRCode(dataMsg);
                MainForm.DeCodeAndEnCode(dataMsg);
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
