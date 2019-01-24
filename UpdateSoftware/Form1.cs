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
            this.args = new string[] { "123456", "1.0.0.0" };
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


            try
            {
                var strVer = HttpHelper.HttpGet(HttpHelper.baseUrl + "main/get_version");
                Version siteVersion = new Version(strVer);

                if (siteVersion.CompareTo(currentVersion) > 0)//有更新
                {
                    lblMsg.Text = "最新版本：" + strVer;
                }
                else//无需更新
                {
                    lblMsg.Visible = true;
                }
            }
            catch (Exception)
            {

                throw;
            }


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
 
    }
}
