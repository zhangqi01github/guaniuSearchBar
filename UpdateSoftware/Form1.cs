using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace UpdateSoftware
{
    public partial class Form1 : Form
    {
        string[] args = null;
        IntPtr mainWndHandle;
        Version currentVersion;

        public Form1()
        {
            InitializeComponent();
            //test
            this.args = "123456;1.0".Split(';');
            this.Text = this.args[1].ToString();
            mainWndHandle = (IntPtr)long.Parse(this.args[0]);
            currentVersion = new Version(this.args[1]);
        }

        public Form1(string[] args)
        {
            InitializeComponent();
          
            this.args = args[0].Split(';');
            this.Text = this.args[1].ToString();
            mainWndHandle = (IntPtr)long.Parse(this.args[0]);
            currentVersion = new Version(this.args[1]);
         
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            label1.Text = label1.Text + currentVersion;
            UpdateMsg("正在检查更新");
            new Thread(new ThreadStart(() => {
                try
                {
                    var strVer = HttpHelper.HttpGet(HttpHelper.baseUrl + "get_version/");
                    Version siteVersion = new Version(strVer);

                    if (siteVersion.CompareTo(currentVersion) > 0)//有更新
                    {
                        UpdateMsg("最新版本：" + strVer);

                            SetBtnVisibility(true);
                        }
                    else//无需更新
                    {
                        UpdateMsg("已经是最新版本");
                            SetBtnVisibility(false);
                    }
                    }
                    catch (Exception )
                    {

                        UpdateMsg("联网失败");
                        SetBtnVisibility(false);
                    }
                })).Start();
              
         


        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateWorker upw = new UpdateWorker(this);
            upw.StartUpdate(mainWndHandle);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            File.Move(AppDomain.CurrentDomain.BaseDirectory + "GuaniuSearchBar._exe", AppDomain.CurrentDomain.BaseDirectory+ "GuaniuSearchBar.exe");//重命名
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }


        #region 无边框拖动效果
        [DllImport("user32.dll")]//拖动无窗体的控件
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        private void Start_MouseDown(object sender, MouseEventArgs e)
        {
            //拖动窗体
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }
        #endregion
        private void SetBtnVisibility(bool b)
        {
            this.Invoke(new MethodInvoker(() => { btnUpdate.Visible = b; }));
        }
        public void UpdateMsg(string msg)
        {
            this.Invoke(new MethodInvoker(() => { lblMsg.Text = msg; }));
         

        }


    }
}
