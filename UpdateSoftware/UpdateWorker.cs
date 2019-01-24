
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UpdateSoftware
{
    public class UpdateWorker
    {
        public const int WM_CLOSE = 0x10;
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, ref COPYDATASTRUCT lParam);

        const int WM_COPYDATA = 0x004A;

        public struct COPYDATASTRUCT

        {

            public IntPtr dwData;

            public int cbData;

            [MarshalAs(UnmanagedType.LPStr)]

            public string lpData;

        }

        IntPtr mainWndHandle;
        private Form1 form1;

        public UpdateWorker(Form1 form1)
        {
            this.form1 = form1;
        }


        private void CloseMainWnd()
        {
            string message = "GN_EXIT";
            if (mainWndHandle != IntPtr.Zero)
            {
                byte[] sarr = System.Text.Encoding.Default.GetBytes(message);
                int len = sarr.Length;
                COPYDATASTRUCT cds;
                cds.dwData = (IntPtr)Convert.ToInt16(1);//可以是任意值
                cds.cbData = len + 1;//指定lpData内存区域的字节数 
                cds.lpData = message;//发送给目标窗口所在进程的数据 
                SendMessage(mainWndHandle, WM_COPYDATA, 0, ref cds);
            }
        }

        public void StartUpdate(IntPtr mainWndHandle)
        {
            BackgroundWorker bgWorker = new BackgroundWorker();
            this.mainWndHandle = mainWndHandle;
            bgWorker.DoWork += Back_thread_DoWork;
            bgWorker.RunWorkerAsync();
        }


        private void Back_thread_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //实例化下载对象
                var downclient = new WebClient();
                downclient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(downclient_DownloadProgressChanged);
                downclient.DownloadFileCompleted += new AsyncCompletedEventHandler(downclient_DownloadFileCompleted);
                //下载远程更新包down.zip压缩文件|放在应用程序目录下|相应界面事件
                downclient.DownloadFileAsync(new Uri("http://127.0.0.1:8000/" + "main/download_soft/"), AppDomain.CurrentDomain.BaseDirectory + "\\GuaniuSearchBar._exe");
            }
            catch (Exception err) { System.Diagnostics.Debug.WriteLine(err); }
        }

        private void downclient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //  throw new NotImplementedException();
        }

        //在异步下载结束时触发该事件
        void downclient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {

                if (e.Error != null)
                {

                    MessageBox.Show("在进行远程更新时,发生错误", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                  
            
                     
                        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                        var filename = baseDirectory + "GuaniuSearchBar.exe";
                     
                        CloseMainWnd();
                        Thread.Sleep(1000);
                        new FileInfo(filename).Attributes = FileAttributes.Normal;
                    try
                    {
                        File.Delete(filename);//删除主程序
                    }
                    catch(Exception e2)
                    {

                    }
                     
                    
                    //  File.Move(filename,baseDirectory + "GuaniuSearchBar.__exe");//重命名
                    // File.Delete(baseDirectory + "GuaniuSearchBar.__exe");//删除主程序

                    File.Move(baseDirectory + "GuaniuSearchBar._exe", filename);//重命名
                    Thread.Sleep(500);
                    Process.Start(filename);
                    Environment.Exit(0);
                }
            }
            catch (Exception err) { }
            
        }
    }
}
