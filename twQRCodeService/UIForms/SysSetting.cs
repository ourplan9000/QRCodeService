using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using log4net;
using twQRCodeService.utils;
using System.Configuration;
using System.Runtime.InteropServices;

namespace twQRCodeService.UIForms
{
    /// <summary>
    /// 系统设置
    /// </summary>
    public partial class SysSetting : Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SysSetting));

        public SysSetting()
        {
            InitializeComponent();

            numericUpDownScanTime.Value = decimal.Parse(ConfigurationManager.AppSettings["scanTime"].ToString());
            numericUpDownOuterCodeSize.Value = decimal.Parse(ConfigurationManager.AppSettings["outerCodeSize"].ToString());
            numericUpDownpointCountSize.Value = decimal.Parse(ConfigurationManager.AppSettings["pointCountSize"].ToString());
            numericUpDownAppExit.Value = decimal.Parse(ConfigurationManager.AppSettings["_timerExit"].ToString());
        }

        /// <summary>
        /// 还原默认配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            InitSysSetting();
            numericUpDownScanTime.Value = 500;
            numericUpDownOuterCodeSize.Value = 450;
            numericUpDownpointCountSize.Value = 10;
            numericUpDownAppExit.Value = 50;
        }

        #region 还原默认配置
        /// <summary>
        /// 还原默认配置
        /// </summary>
        public void InitSysSetting()
        {
            //默认二维码尺寸
            MainForm.outerCodeSize = 450;
            //默认扫描频率(mm)
            MainForm.scanTime = 500;
            //默认地点显示个数
            BdMapUtil.pointCountSize = 10;
            //默认重启时间(秒)
            MainForm.timerExit = 50;

            changeAppSettings("outerCodeSize", MainForm.outerCodeSize.ToString());
            changeAppSettings("scanTime", MainForm.scanTime.ToString());
            changeAppSettings("pointCountSize", "10");
            changeAppSettings("_timerExit", "50");
        }
        #endregion

        #region 应用配置
        /// <summary>
        /// 应用配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                MainForm.scanTime = int.Parse(numericUpDownScanTime.Value.ToString());
                MainForm.outerCodeSize = int.Parse(numericUpDownOuterCodeSize.Value.ToString());
                BdMapUtil.pointCountSize = int.Parse(numericUpDownpointCountSize.Value.ToString());
                MainForm.timerExit = int.Parse(numericUpDownAppExit.Value.ToString());

                changeAppSettings("outerCodeSize", MainForm.outerCodeSize.ToString());
                changeAppSettings("scanTime", MainForm.scanTime.ToString());
                changeAppSettings("pointCountSize", numericUpDownpointCountSize.Value.ToString());
                changeAppSettings("_timerExit", numericUpDownAppExit.Value.ToString());

                MessageBox.Show("设置成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("应用失败，请重新设置");
                //LoggerUtil.Logger(log, "ERROR", ex.StackTrace.ToString());
            }
        }
        #endregion

        #region 修改app.config文件的AppSettings节点
        /// <summary>
        /// 修改app.config文件的AppSettings节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void changeAppSettings(string key,string value)
        {
            //获取Configuration对象
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //根据Key读取<add>元素的Value
            string temp = config.AppSettings.Settings[key].Value;
            if (temp != null)
            {
                //删除<add>元素
                config.AppSettings.Settings.Remove(key);
            }
            //增加<add>元素
            config.AppSettings.Settings.Add(key, value);
            //一定要记得保存，写不带参数的config.Save()也可以
            config.Save(ConfigurationSaveMode.Modified);
            //刷新，否则程序读取的还是之前的值（可能已装入内存）
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
        }
        #endregion
    }
}
