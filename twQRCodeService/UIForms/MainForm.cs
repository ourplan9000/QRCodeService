using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using log4net;
using twQRCodeService.MODEL;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using twQRCodeService.utils;
using System.Threading;
using System.Diagnostics;
using System.Configuration;
using System.IO;
using Ozeki.Media.MediaHandlers.Video;
using Ozeki.Media.MediaHandlers;
using Ozeki.Media.MediaHandlers.Video.CV.ImageTools;
using Ozeki.Media.MediaHandlers.Video.CV;
using Ozeki.Media.MediaHandlers.Video.CV.BarcodeData;
using Ozeki.Media.Video;
using System.Linq;
using System.Threading.Tasks;

namespace twQRCodeService.UIForms
{
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class MainForm : Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MainForm));

        #region 全局参数设置
        //扫描二维码计时器
        System.Timers.Timer _timer = null;
        //扫描时间频率
        public static int scanTime =500;
        //生成二维码的尺寸
        public static int outerCodeSize = 450;
        /// <summary>
        /// 扫描成功次数
        /// </summary>
        public static int successCount = 0;
        /// <summary>
        /// 程序重启频率（秒）
        /// </summary>
        public static int timerExit = 0;
        #endregion

        #region 全局对象设置
        /// <summary>
        /// 临时二维码集合，用于判断读取的二维码是否重复
        /// </summary>
        private static List<QRCodeMsg> qrCodeMsgList = new List<QRCodeMsg>();
        /// <summary>
        /// HTML文件，调用其中的JS函数
        /// </summary>
        public static string htmlText = string.Empty;
        /// <summary>
        /// 时间测试
        /// </summary>
        public static Stopwatch MyStopWatch = new Stopwatch();
        /// <summary>
        /// 消息处理队列，用于将扫描到的二维码解析并地址定位生成二维码显示到界面
        /// </summary>
        MsgProcessor process=null;
        /// <summary>
        /// MainForm窗口
        /// </summary>
        public static MainForm main=null;
        /// <summary>
        /// 摄像头集合
        /// </summary>
        List<VideoDeviceInfo> devices=null;

        EncodingOptions options = null;
        BarcodeWriter writer = null;

        #endregion

        #region 界面初始化
        public MainForm()
        {
            main = this;
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();

            //读取扫描设备
            devices = WebCamera.GetDevices();
            if (devices.Count <= 0)
            {
                MessageBox.Show("没有发现图像扫描设备!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            foreach (VideoDeviceInfo ds in devices)
            {
                cmbDevice.Items.Add(ds.Name);
            }
            if (cmbDevice.Items.Count > 0)
            {
                cmbDevice.SelectedIndex = 0;
            }
            _webCamera = WebCamera.GetDevice(devices.Where(t => t.DeviceID == cmbDevice.SelectedIndex).FirstOrDefault());

            btnStop.Enabled = false;
            try
            {
                //读取配置文件的扫描频率
                scanTime = int.Parse(ConfigurationManager.AppSettings["scanTime"].ToString());
                //读取配置文件的生成二维码尺寸
                outerCodeSize = int.Parse(ConfigurationManager.AppSettings["outerCodeSize"].ToString());
                //读取配置文件的生成二维码尺寸
                timerExit = int.Parse(ConfigurationManager.AppSettings["_timerExit"].ToString());
                //加载HTML文件，
                string url = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"MyHtml.html");
                this.webBrowser1.Navigate(url);
            }
            catch (FileNotFoundException e)
            {
                LoggerUtil.Logger(log, "ERROR", "百度边界判断文件加载异常！");
            }
            catch (Exception e)
            {
                LoggerUtil.Logger(log, "ERROR", "读取配置文件异常！");
            }
            //设置一个对COM可见的对象(上面已将该类设置对COM可见)
            this.webBrowser1.ObjectForScripting = this;
            process = new MsgProcessor();

          

            if (outerCodeSize >= main.picBoxBaiduXml.Width)
            {
                outerCodeSize = main.picBoxBaiduXml.Width;
            }
            options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = outerCodeSize,
                Height = outerCodeSize
            };
            writer = new BarcodeWriter();
            writer.Format = ZXing.BarcodeFormat.QR_CODE;
            writer.Options = options;
        }
        #endregion

        #region 点击开始扫描
        //点击开始扫描
        private void btnStart_Click(object sender, EventArgs e)
        {
            _webCamera = WebCamera.GetDefaultDevice();
            if (_webCamera != null)
            {
                _connector.Connect(_webCamera, _imageProvider);
                videoViewer.SetImageProvider(_imageProvider);
                videoViewer.Start();
                _webCamera.Start();

                btnStart.Enabled = false;
                btnStop.Enabled = true;

                System.Timers.Timer _timer = new System.Timers.Timer(scanTime);
                _timer.Start();
                _timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Elapsed);

                System.Timers.Timer _timerExit = new System.Timers.Timer(timerExit * 1000);
                _timerExit.Start();
                _timerExit.Elapsed += new System.Timers.ElapsedEventHandler(_timerExit_Elapsed);
            }
        }
        #endregion

        #region 程序重启
        /// <summary>
        /// 程序重启
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _timerExit_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Environment.Exit(Environment.ExitCode);
        }
        #endregion

        #region 定时器工作，扫描二维码
        /// <summary>
        /// 扫描二维码 定时器工作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                //扫描次数
                //totalCount++;
                Bitmap map = new Bitmap(this.videoViewer.Width, this.videoViewer.Height);
                Graphics g = Graphics.FromImage(map);
                Point p = new Point(0, 0);
                if (this.videoViewer.InvokeRequired)
                {
                    MethodInvoker code = delegate
                    {
                        p = this.PointToScreen(this.videoViewer.Location);
                    };
                    this.videoViewer.Invoke(code, null);
                }
                else
                {
                    if (this.videoViewer != null)
                    {
                        p = this.PointToScreen(this.videoViewer.Location);
                    }
                }
                g.CopyFromScreen(p, new Point(0, 0), map.Size);

               
                string result = RQDecode(map);

                g.Dispose();

                if (!string.IsNullOrEmpty(result))
                {
                    successCount++;
                    process.Execute(result);
                    //DeCodeAndEnCode(result);
                }
            }
            catch (Exception ex)
            {
                LoggerUtil.Logger(log, "ERROR", "扫描异常," + ex.StackTrace.ToString());
            }
        }
        #endregion



        
        #region 提供JS查询出结果时调用,生成二维码并显示到界面
        /// <summary>
        /// 提供JS查询出结果时调用
        /// 生成二维码并显示到界面
        /// </summary>
        /// <param name="twContent"></param>
        public void SetQRCode(string twContent)
        {
            Thread.Sleep(200);
            //生成二维码
            //LoggerUtil.Logger(log, "INFO", "查询结果:" + twContent);
            Bitmap bitmap = writer.Write(twContent);
            //bitmap.Save(Guid.NewGuid().ToString("N")+".bmp");
            main.picBoxBaiduXml.Image = bitmap;
            try
            {
                Task.Factory.StartNew(() =>
                {
                    string[] strTemp = twContent.Split('^');
                    StringBuilder sb = new StringBuilder();
                    for (int i = 1; i < strTemp.Length; i++)
                    {
                        string[] strTemp2 = strTemp[i].Split(',');
                        if (null != strTemp2)
                        {
                            if (strTemp2.Length > 3)
                            {
                                strTemp2[0] = strTemp2[0].Replace("qz", "钦州");
                                sb.Append("地址：" + strTemp2[0] + "\r\n");
                                sb.Append("纬度：" + strTemp2[1] + "\r\n");
                                sb.Append("经度：" + strTemp2[2] + "\r\n");
                                strTemp2[3] = strTemp2[3].Replace("qz", "钦州");
                                sb.Append("详细地址：" + strTemp2[3] + "\r\n");
                            }
                            strTemp2 = null;
                        }
                    }
                    if (MainForm.main.textBoxMsg2.InvokeRequired)
                    {
                        {
                            MethodInvoker code = delegate
                            {
                                main.textBoxMsg2.Text = sb.ToString();
                            };
                            MainForm.main.textBoxMsg2.Invoke(code, null);
                        }
                    }
                    else
                    {
                        main.textBoxMsg2.Text = sb.ToString();
                    }
                });

                Task.Factory.StartNew(() =>
                {
                    LoggerUtil.Logger(log, "INFO", DateTime.Now.ToString("yy-MM-dd HH:mm:ss:fffffff"));
                });
            }
            catch (Exception e)
            { }
            
        }
        #endregion 

        #region 显示二维码并调用地址定位服务
        /// <summary>
        /// 显示二维码并调用地址定位服务
        /// </summary>
        /// <param name="result"></param>
        public static void DeCodeAndEnCode(string result)
        {
            try
            {
                showMsg0(result);
                //WebServiceQuery.searchPlace(result);
                //判断二维码是否重复
                if (!QRCodeUtil.Contain(result, qrCodeMsgList))
                {
                    //LoggerUtil.Logger(log, "INFO", result);
                    //调用查询服务.
                    string res = result;
                    WebServiceQuery.searchPlace(res);
                    //1若不重复，扫描，加入集合，移除集合中时间最久二维码信息
                    if (qrCodeMsgList.Count > 100)
                    {
                        //按时间排序，时间最早在前
                        qrCodeMsgList.OrderBy(t => t.creatTime);
                        //移除时间最早的二维码信息
                        for (int i = 0; i < 50; i++)
                        {
                            qrCodeMsgList.Remove(qrCodeMsgList[i]);
                        }
                        qrCodeMsgList.Add(QRCodeUtil.ConvertToQRCodeMsg(result));
                    }
                    else
                    {
                        qrCodeMsgList.Add(QRCodeUtil.ConvertToQRCodeMsg(result));
                    }
                }
            }
            catch (Exception e)
            { }
            //2若重复，不扫描
            Console.WriteLine("扫描完毕");
        }
        #endregion

        #region 解析二维码图像
        /// <summary>
        /// 解析二维码图像
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static string RQDecode(Bitmap img)
        {
            string errText = string.Empty;
            Result result = null;
            if (img != null)
            {
                try
                {
                    result = new BarcodeReader().Decode(img);
                }
                catch(Exception e)
                {
                    LoggerUtil.Logger(log, "ERROR", "解码异常," + e.StackTrace.ToString());
                    return errText;
                }
                if (result != null)
                {
                    //LoggerUtil.Logger(log, "INFO", result.Text);
                    return result.Text;
                }
                else
                {
                    return errText;
                }
            }
            else
            {
                return errText;
            }
        }
        #endregion

        #region 显示扫描到的二维码信息
        /// <summary>
        /// 显示扫描信息
        /// </summary>
        /// <param name="result"></param>
        private static void showMsg0(string result)
        {
            string[] codeMsg = result.ToString().Replace("，", ",").Split(',');
            if (null != codeMsg)
            {
                if (codeMsg.Length>2)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("警情编码：" + codeMsg[1] + "\r\n");
                    sb.Append("事件地点：" + codeMsg[0] + "\r\n");
                    sb.Append("席位号：" + codeMsg[2] + "\r\n");
                    sb.Append("扫描成功次数：" + successCount);
                    main.textBoxMsg1.Text = sb.ToString();
                    //LoggerUtil.Logger(log, "INFO", sb.ToString());
                    sb = null;
                }
                codeMsg = null;
            }
        }
        #endregion

        #region 提供JS未查询出结果时调用
        /// <summary>
        /// 提供JS未查询出结果时调用
        /// </summary>
        public void ClearMsg2()
        {
            picBoxBaiduXml.Image = null;
            this.textBoxMsg2.Text = "未搜索到该地点。";
        }
        #endregion

        #region 加载JS初始化函数
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (MainForm.main.webBrowser1.InvokeRequired)
            {
                MethodInvoker code = delegate
                {
                    MainForm.main.webBrowser1.Document.InvokeScript("init");
                };
                MainForm.main.webBrowser1.Invoke(code, null);
            }
        }
        #endregion       

        #region 点击停止扫描
        //点击停止扫描
        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                btnStop.Enabled = false;
                btnStart.Enabled = true;
                textBoxMsg1.Text = string.Empty;

                qrCodeMsgList.Clear();
                videoViewer.Stop();
                _webCamera.Stop();
                _timer.Dispose();
            }
            catch (Exception ex)
            {
                //LoggerUtil.Logger(log, "ERROR", "停止扫描异常," + ex.StackTrace.ToString());
            }
        }
        #endregion

        #region 内存回收
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        /// <summary>
        /// 释放内存
        /// </summary>
        public static void ClearMemory()
        {
            GC.WaitForPendingFinalizers();
            GC.Collect();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                MainForm.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }
        

        private void timer1_Tick(object sender, EventArgs e)
        {
            ClearMemory();
        }
        #endregion

        #region 加载摄像头
        /// <summary>
        /// 摄像头
        /// </summary>
        private WebCamera _webCamera;
        private DrawingImageProvider _imageProvider;
        private MediaConnector _connector;

        private void MainForm_Load(object sender, EventArgs e)
        {
            _imageProvider = new DrawingImageProvider();
            _connector = new MediaConnector();
            videoViewer.SetImageProvider(_imageProvider);
        }
        #endregion

        #region 系统设置
        /// <summary>
        /// 系统设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSys_Click(object sender, EventArgs e)
        {
            new SysSetting().ShowDialog();
        }
        #endregion
    }
}
