using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace GuaniuSearchBar
{
    public partial class PopupMainWnd : System.Windows.Forms.Form
    {
        Label[] linkLabels;
        PictureBox[] pictureBoxes;
        Label[] siteNameLabels;
        TextBox tbSearch;
        public PopupMainWnd(TextBox tbSearch)
        {
            InitializeComponent();
            this.tbSearch = tbSearch;
        }

      
        // Define paddings here.
        const int pictureBoxPadding = 20;
        private void Form2_Load(object sender, EventArgs e)
        {
            linkLabels = new Label[10];
            for (int i = 0; i < 10; i++)
            {
                linkLabels[i] = new Label();
                linkLabels[i].Left = 0;
                linkLabels[i].Size = label1.Size;
                linkLabels[i].Top = label1.Top + i * label1.Height;
                linkLabels[i].Text = string.Empty;
                linkLabels[i].TextAlign = ContentAlignment.MiddleLeft;
                linkLabels[i].MouseEnter += Label_MouseEnter;
                linkLabels[i].MouseLeave += Label_MouseLeave;

                this.Controls.Add(linkLabels[i]);
                // Add event handler to labels
                linkLabels[i].Click += (_sender, _e) =>
                {
                    Label __sender = _sender as Label;
                    if (__sender.Tag != null)
                    {
                        Process.Start(__sender.Tag.ToString());
                    }
                };
            }
            UpdateLinkIcons();
            Search.GetBaiduHotKeywords(linkLabels);
        }
        /// <summary>
        /// 更新上方的十个图标
        /// </summary>
        private void UpdateLinkIcons()
        {
            pictureBoxes = new PictureBox[10];
            siteNameLabels = new Label[10];
            for (int i = 0; i < 10; i++)
            {
                pictureBoxes[i] = new PictureBox();
                pictureBoxes[i].Left = pictureBox1.Left + (i >= 5 ? i - 5 : i) * (pictureBox1.Width + pictureBoxPadding);
                pictureBoxes[i].Size = pictureBox1.Size;
                pictureBoxes[i].Top = i >= 5 ? pictureBox1.Height + 30 + pictureBox1.Top : pictureBox1.Top;
                pictureBoxes[i].BackColor = pictureBox1.BackColor;
                pictureBoxes[i].Cursor = Cursors.Hand;
                // Draw Border
                pictureBoxes[i].Paint += (_sender, _e) =>
                {
                    _e.Graphics.DrawRectangle(Pens.LightGray, 0, 0, (_sender as PictureBox).Width - 1, (_sender as PictureBox).Height - 1);
                };
                var ImgSrc = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images", (i + 1).ToString() + ".png");
                if (File.Exists(ImgSrc))
                {
                    pictureBoxes[i].Image = Image.FromFile(ImgSrc);
                }
                pictureBoxes[i].Tag = Search.urls[i];
                pictureBoxes [i].Click+= (_s, _e) =>
                {
                    HttpHelper.Goto((_s as PictureBox).Tag.ToString());
                };
                this.Controls.Add(pictureBoxes[i]);

                // News labels
                siteNameLabels[i] = new Label();
                siteNameLabels[i].Text = Search.sitenames[i];
                siteNameLabels[i].AutoSize=true;
                siteNameLabels[i].Top = pictureBoxes[i].Top + pictureBoxes[i].Height + 5;
                siteNameLabels[i].Cursor = Cursors.Hand;
                siteNameLabels[i].Tag =Search.urls[i];
                siteNameLabels[i].Click += (_s, _e) =>
                {
                    HttpHelper.Goto((_s as Label).Tag.ToString());
                };
              
                this.Controls.Add(siteNameLabels[i]);
                siteNameLabels[i].Left = pictureBoxes[i].Left - (siteNameLabels[i].Width - pictureBoxes[i].Width) / 2;
                //// Left align to PictureBox.
                //siteNameLabels[i].Paint += (_sender, _e) =>
                //{
                //    int _i = (int)(_sender as Label).Tag;
                //    siteNameLabels[_i].Left = pictureBoxes[_i].Left + (pictureBoxes[_i].Width - siteNameLabels[_i].Width) / 2;
                //};

            }
        }

        private void Label_MouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).BackColor = Color.White;
        }

        private void Label_MouseEnter(object sender, EventArgs e)
        {
            ((Label)sender).BackColor = Color.Gray;
        }
        
        private void ChangeHotKeywords()
        {
            Search.GetBaiduHotKeywords(linkLabels, true);
        }
        private void label4_Click(object sender, EventArgs e)
        {
            ChangeHotKeywords();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            ChangeHotKeywords();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            HttpHelper.Goto("http://zhuomian.kmshuru.com");
        }
    }
}
