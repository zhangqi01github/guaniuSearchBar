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
    public partial class PopupKeywordWnd : Form
    {
        Label[] linkLabels;
        PictureBox[] pictureBoxes;
        Label[] siteNameLabels;
        TextBox tbSearch;
        int startTopPosition;
        MainForm mainForm;


        public PopupKeywordWnd(TextBox tbSearch, MainForm mainForm)
        {
            InitializeComponent();
            this.tbSearch = tbSearch;
            this.mainForm = mainForm;
            this.startTopPosition = mainForm.Top;
        }

        private void Form2_Deactivate(object sender, EventArgs e)
        {

        


        }


        private void Form2_Load(object sender, EventArgs e)
        {
         
        }

        private void Label_MouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).BackColor = Color.White;
        }

        private void Label_MouseEnter(object sender, EventArgs e)
        {
            ((Label)sender).BackColor = Color.Gray;
        }

        JToken[] sugguestions = { "1", "2", "3" };
        private void UpdateLinkLabels()
        {

            if (linkLabels!=null)
            {
                for (int i = 0; i < linkLabels.Length; i++)
                {
          
                    
                   linkLabels[i].Dispose();
                }
            }
            linkLabels = new Label[sugguestions.Count()];
            for (int i = 0; i < sugguestions.Count(); i++)
            {
                linkLabels[i] = new Label();
                linkLabels[i].Left = 0;
                linkLabels[i].Size = label1.Size;
                linkLabels[i].Top = label1.Top + i * label1.Height;
                linkLabels[i].Text = sugguestions[i].ToString();
                linkLabels[i].MouseEnter += Label_MouseEnter;
                linkLabels[i].MouseLeave += Label_MouseLeave;

                this.Controls.Add(linkLabels[i]);
                // Add event handler to labels
                linkLabels[i].Click += (_sender, _e) =>
                {
                    Label __sender = _sender as Label;
                   
                    mainForm.CallSearchEngine(__sender.Text);
                    
                };

            }

            // update window size

            this.Height = linkLabels.Count() * label1.Height+30;
           

            this.Top = startTopPosition - this.Height;
        }
    

        private void GetNews()
        {
            Action UpdateNews = () =>
            {
                try
                {
                    string s = HttpHelper.HttpGet("http://127.0.0.1:8000/main/get_dummy_data/");
                    JArray jsonResult = (JArray)JsonConvert.DeserializeObject(s);

                    for (int i = 0; i < jsonResult.Count; i++)
                    {
                        linkLabels[i].Invoke(new MethodInvoker(() =>
                        {
                            linkLabels[i].Text = jsonResult[i]["title"].ToString();
                            linkLabels[i].Tag = jsonResult[i]["url"];
                        }));
                    }
                }
                catch (Exception e)
                {
                    Debug.Print(e.Message);
                }
            };


            new Thread(new ThreadStart(UpdateNews)).Start();

        }

        private void PopupMainWnd_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        // Keyword updated callback.

        public void KeywordUpdated_Handler()
        {
            string keyword = tbSearch.Text;
            label1.Text = keyword;
            // results =keyword.Split(',');
            var url = "http://suggestion.baidu.com/su?wd=" + keyword+ "&ie=utf-8";
            var strResultJson = HttpHelper.HttpGet(url);
            if (strResultJson!=string.Empty)
            {
                strResultJson= strResultJson.Remove(strResultJson.Length - 2, 2);
                strResultJson=strResultJson.Replace("window.baidu.sug(", "");
            }
            try
            {
                JObject jsonResult = (JObject)JsonConvert.DeserializeObject(strResultJson);
                sugguestions = jsonResult["s"].ToArray();
                UpdateLinkLabels();
            }
            catch (Exception)
            {

                throw;
            }
         
            //for (int i = 0; i < jsonResult; i++)
            //{

            //}
            //Debug.Print(keyword);
            
            
        }
    }
}
