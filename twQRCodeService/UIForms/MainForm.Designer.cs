using System;
using System.Runtime.InteropServices;
namespace twQRCodeService.UIForms
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        IntPtr m_ip = IntPtr.Zero;
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //if (components != null)
                //{
                //    components.Dispose();
                //}
            }
            base.Dispose(disposing);
            if (m_ip != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(m_ip);
                m_ip = IntPtr.Zero;
            }
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.picBoxBaiduXml = new System.Windows.Forms.PictureBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.textBoxMsg1 = new System.Windows.Forms.TextBox();
            this.textBoxMsg2 = new System.Windows.Forms.TextBox();
            this.btnSys = new System.Windows.Forms.Button();
            this.cmbDevice = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.videoViewer = new Ozeki.Media.Video.Controls.VideoViewerWF();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxBaiduXml)).BeginInit();
            this.SuspendLayout();
            // 
            // picBoxBaiduXml
            // 
            this.picBoxBaiduXml.BackColor = System.Drawing.SystemColors.Window;
            this.picBoxBaiduXml.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picBoxBaiduXml.Location = new System.Drawing.Point(5, 26);
            this.picBoxBaiduXml.Name = "picBoxBaiduXml";
            this.picBoxBaiduXml.Size = new System.Drawing.Size(700, 700);
            this.picBoxBaiduXml.TabIndex = 1;
            this.picBoxBaiduXml.TabStop = false;
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStart.Location = new System.Drawing.Point(5, 1);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "开始扫描";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStop.Location = new System.Drawing.Point(91, 1);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "停止扫描";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // textBoxMsg1
            // 
            this.textBoxMsg1.Location = new System.Drawing.Point(704, 332);
            this.textBoxMsg1.Multiline = true;
            this.textBoxMsg1.Name = "textBoxMsg1";
            this.textBoxMsg1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxMsg1.Size = new System.Drawing.Size(300, 77);
            this.textBoxMsg1.TabIndex = 15;
            // 
            // textBoxMsg2
            // 
            this.textBoxMsg2.Location = new System.Drawing.Point(704, 415);
            this.textBoxMsg2.Multiline = true;
            this.textBoxMsg2.Name = "textBoxMsg2";
            this.textBoxMsg2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxMsg2.Size = new System.Drawing.Size(300, 281);
            this.textBoxMsg2.TabIndex = 16;
            // 
            // btnSys
            // 
            this.btnSys.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSys.Location = new System.Drawing.Point(177, 1);
            this.btnSys.Name = "btnSys";
            this.btnSys.Size = new System.Drawing.Size(75, 23);
            this.btnSys.TabIndex = 17;
            this.btnSys.Text = "系统设置";
            this.btnSys.UseVisualStyleBackColor = true;
            this.btnSys.Click += new System.EventHandler(this.btnSys_Click);
            // 
            // cmbDevice
            // 
            this.cmbDevice.FormattingEnabled = true;
            this.cmbDevice.Location = new System.Drawing.Point(369, 3);
            this.cmbDevice.Name = "cmbDevice";
            this.cmbDevice.Size = new System.Drawing.Size(154, 20);
            this.cmbDevice.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(287, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 17);
            this.label1.TabIndex = 19;
            this.label1.Text = "选择摄像头：";
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(711, 702);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(293, 20);
            this.webBrowser1.TabIndex = 20;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // videoViewer
            // 
            this.videoViewer.BackColor = System.Drawing.Color.DarkGray;
            this.videoViewer.FlipMode = Ozeki.Media.Video.FlipMode.None;
            this.videoViewer.ForeColor = System.Drawing.Color.DarkGray;
            this.videoViewer.Location = new System.Drawing.Point(704, 26);
            this.videoViewer.Name = "videoViewer";
            this.videoViewer.RotateAngle = 0;
            this.videoViewer.Size = new System.Drawing.Size(300, 300);
            this.videoViewer.TabIndex = 21;
            this.videoViewer.Text = "videoViewerWF3";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 734);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbDevice);
            this.Controls.Add(this.btnSys);
            this.Controls.Add(this.textBoxMsg2);
            this.Controls.Add(this.textBoxMsg1);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.picBoxBaiduXml);
            this.Controls.Add(this.videoViewer);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "二维码地图定位服务器";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxBaiduXml)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picBoxBaiduXml;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TextBox textBoxMsg1;
        private System.Windows.Forms.TextBox textBoxMsg2;
        private System.Windows.Forms.Button btnSys;
        private System.Windows.Forms.ComboBox cmbDevice;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Timer timer1;
        private Ozeki.Media.Video.Controls.VideoViewerWF videoViewer;
    }
}