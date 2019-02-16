using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuaniuSearchBar
{
    public partial class BasicSettings :Form
    {
        MainForm mainForm;
        public BasicSettings(MainForm mainForm)
        {

            InitializeComponent();
            this.mainForm = mainForm;
            this.MouseDown += Start_MouseDown;
        }

        #region 无边框拖动效果
        [DllImport("user32.dll")]//拖动无窗体的控件
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        protected void Start_MouseDown(object sender, MouseEventArgs e)
        {
            //拖动窗体
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }
        #endregion
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BasicSettings_Load(object sender, EventArgs e)
        {
            mainForm.topMostEnable = false;
          
            var cfg= Config.ReadHotkeyConfigFromFile();
       
                //暂时关闭事件
                checkBox1.CheckedChanged -= checkBox1_CheckedChanged;
                comboBox1.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
                //设置画面显示
                checkBox1.Checked = cfg.enabled;
                comboBox1.SelectedIndex = cfg.hotkeyIndex;
                //恢复事件
                checkBox1.CheckedChanged += checkBox1_CheckedChanged;
                comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            
        }
     

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                AppHotKey.UnRegKey(mainForm.Handle);
                AppHotKey.RegHotKey(mainForm.Handle, comboBox1.SelectedIndex);
                Config.SaveHotkeyConfig(new Config.HotKeyConfig(comboBox1.SelectedIndex,true));
            }

        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            AppHotKey.UnRegKey(mainForm.Handle);
            if (checkBox1.Checked)
            {
                AppHotKey.RegHotKey(mainForm.Handle, comboBox1.SelectedIndex);
            }
            Config.SaveHotkeyConfig(new Config.HotKeyConfig(comboBox1.SelectedIndex,checkBox1.Enabled));

        }

        private void BasicSettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.topMostEnable = true;
        }
    }
}
