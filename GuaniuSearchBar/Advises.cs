﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuaniuSearchBar
{
    public partial class Advises : Form
    {
        MainForm mainForm;
        public Advises(MainForm mainForm)
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (this.tbContact.Text.Length==0)
            {
                lblWarning.Visible = true;
                return;
            }
            HttpHelper.HttpGet(HttpHelper.baseUrl + "advise/" + this.tbContact.Text + "/" + tbProblem.Text + "/");

            this.pbFeedback.Visible = true;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Advises_Load(object sender, EventArgs e)
        {
            mainForm.topMostEnable = false;
        }

        private void Advises_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.topMostEnable = true;
        }
    }
}
