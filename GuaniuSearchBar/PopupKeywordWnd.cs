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
    public partial class PopupKeywordWnd : System.Windows.Forms.Form
    {
        Label[] linkLabels;
        TextBox tbSearch;
        Thread QProcessThread;
        Form mainForm;


        public PopupKeywordWnd(TextBox tbSearch, Form mainForm)
        {
            InitializeComponent();
            this.tbSearch = tbSearch;
            this.mainForm = mainForm;


            QProcessThread=new Thread(new ThreadStart(() =>
            {
                while (!IsHandleCreated)
                    Thread.Sleep(100);
                while (true)
                {
                    if (currentTask != null)
                    {
                        if (currentTask.ThreadState != System.Threading.ThreadState.Running)
                        {
                            currentTask = null;
                        }
                        if (taskQ.Count >0 && currentTask !=null && currentTask.ThreadState == System.Threading.ThreadState.Running)
                        {
                            currentTask.Abort();
                            currentTask = null;
                        }
                    }
                    if (taskQ.Count > 0 && currentTask == null)
                    {
                        currentTask = taskQ.Dequeue();
                        currentTask.Start();
                    }
                    Thread.Sleep(50);
                }
     
            }));
            QProcessThread.Start();
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
            if (sugguestions.Count()==0)
            {
                this.Visible = false;
                return;
            }
            if (linkLabels != null)
            {
                for (int i = 0; i < linkLabels.Length; i++)
                {
                    if (linkLabels[i] != null)
                    {
                        linkLabels[i].Dispose();
                    }
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

                    ((dynamic)mainForm).CallSearchEngine(__sender.Text);

                };

            }
            // update window size
           if((mainForm as MainForm).tbSearch.Text!="")
            {
                this.Height = linkLabels.Count() * label1.Height + 30;
                (mainForm as MainForm).AddChildForm(this);
            }
    
 
        }
        

        private void PopupMainWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
        // Keyword updated callback.
        int cnt = 0;

        Queue<Thread> taskQ = new Queue<Thread>();
        Thread currentTask;

        public void KeywordUpdated_Handler()
        {
            Thread tGetKeyword;
            string keyword = tbSearch.Text;
            label1.Text = keyword;

            tGetKeyword = new Thread(new ThreadStart(
               () =>
               {
                   cnt++;
                   int _cnt = cnt;
                   MyDebug.Print(_cnt.ToString() + " 开始");
                   var url = "http://suggestion.baidu.com/su?wd=" + keyword + "&ie=utf-8";
                   var strResultJson = HttpHelper.HttpGet(url);
                   if (strResultJson != string.Empty)
                   {
                       strResultJson = strResultJson.Remove(strResultJson.Length - 2, 2);
                       strResultJson = strResultJson.Replace("window.baidu.sug(", "");
                   }
                   try
                   {
                       JObject jsonResult = (JObject)JsonConvert.DeserializeObject(strResultJson);
                       sugguestions = jsonResult["s"].ToArray();

                       IAsyncResult asyncResult = this.BeginInvoke(new MethodInvoker(() =>
                        {
                            UpdateLinkLabels();
                        }));

                       this.EndInvoke(asyncResult);
                       MyDebug.Print(_cnt.ToString() + " 结束");

                   }
                   catch (Exception ex)
                   {
                    

                   }
               }));
            taskQ.Enqueue(tGetKeyword);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {


        }
    }
}
