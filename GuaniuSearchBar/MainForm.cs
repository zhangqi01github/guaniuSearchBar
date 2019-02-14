using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuaniuSearchBar
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private delegate bool WNDENUMPROC(IntPtr hWnd, int lParam);
        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndlnsertAfter, int X, int Y, int cx, int cy, uint Flags);
        #region SETWINDOWPOS
        const int HWND_TOPMOST = -1;
        const int SWP_NOSIZE = 1;// {忽略 cx、cy, 保持大小
        const int SWP_NOMOVE = 2; //{忽略 X、Y, 不改变位置
        const int SWP_NOZORDER = 4; //{忽略 hWndInsertAfter, 保持 Z 顺序}
        const int SWP_NOREDRAW = 8;// {不重绘}
        const int SWP_NOACTIVATE = 0x10;// {不激活}
        const int SWP_FRAMECHANGED = 0x20; //{强制发送 WM_NCCALCSIZE 消息, 一般只是在改变大小时才发送此消息}
        const int SWP_SHOWWINDOW = 0x40;// {显示窗口}
        const int SWP_HIDEWINDOW = 0x80;// {隐藏窗口}
        const int SWP_NOCOPYBITS = 0x100; //{丢弃客户区}
        const int SWP_NOOWNERZORDER = 0x200;// {忽略 hWndInsertAfter, 不改变 Z 序列的所有者}
        const int SWP_NOSENDCHANGING = 0x400; //{不发出 WM_WINDOWPOSCHANGING 消息}
        const int SWP_DRAWFRAME = SWP_FRAMECHANGED;// {画边框}
        const int SWP_NOREPOSITION = SWP_NOOWNERZORDER;//{}
        const int SWP_DEFERERASE = 0x2000;// {防止产生 WM_SYNCPAINT 消息}
        const int SWP_ASYNCWINDOWPOS = 0x4000;// {若调用进程不拥有窗口, 系统会向拥有窗口的线程发出需求}
        #endregion
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        enum GetWindow_Cmd : uint
        {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }
        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool lpSystemInfo);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll", EntryPoint = "SetParent")]
        static extern int SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle lpRect);
        [DllImport("user32.dll")]
        static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool BRePaint);
        [DllImport("user32.dll")]
        private static extern int GetWindowTextW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        private static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll", ExactSpelling = true)]
        private static extern bool EnumChildWindows(IntPtr hwndParent, WNDENUMPROC lpEnumFunc, int lParam);
        public struct WindowInfo
        {
            public IntPtr hWnd;
            public string szWindowName;
            public string szClassName;
        }
        enum DisplayMode
        {
            Desktop,
            TaskBar
        }




        DisplayMode displayMode = DisplayMode.Desktop;
        // handle of TaskBar
        IntPtr hTaskBar = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Shell_TrayWnd", null);
        int startButtonWidth;
        int rebarNewWidth = 0;
        int rebarOriginalWidth = 0;

        int trayNotifyOriginalWidth = 0;
        Point desktopModeLocation = new Point(Screen.PrimaryScreen.WorkingArea.Width / 3, 300);

        PopupMainWnd popupWnd;
        PopupKeywordWnd keywordWnd;
        LeftPopupWnd leftPopupWnd;

        #region 接收来自软件更新器的消息以关闭程序  + 热键处理
        const int WM_COPYDATA = 0x004A;
        public struct COPYDATASTRUCT

        {

            public IntPtr dwData;

            public int cbData;

            [MarshalAs(UnmanagedType.LPStr)]

            public string lpData;

        }



        private const int WM_HOTKEY = 0x312; //窗口消息-热键
        private const int WM_CREATE = 0x1; //窗口消息-创建
        private const int WM_DESTROY = 0x2; //窗口消息-销毁
        const int WM_WINDOWPOSCHANGED = 0x0047;
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_HOTKEY: //窗口消息-热键ID
                    switch (m.WParam.ToInt32())
                    {
                        case AppHotKey.HotKeyID: //热键ID
                            HotKeyPressedHandler();
                            break;
                        default:
                            break;
                    }
                    break;

                case WM_DESTROY: //窗口消息-销毁
                    AppHotKey.UnRegKey(Handle, AppHotKey.HotKeyID); //销毁热键
                    break;

                case WM_COPYDATA:
                    COPYDATASTRUCT cds = new COPYDATASTRUCT();

                    Type t = cds.GetType();

                    cds = (COPYDATASTRUCT)m.GetLParam(t);

                    string strResult = cds.lpData;

                    string strType = cds.dwData.ToString();
                    if (strResult == "GN_EXIT")
                    {
                        Environment.Exit(0);
                    }
                    break;


                default:




                    break;
            }
            base.WndProc(ref m);
        }
        #endregion
        #region 无边框拖动效果
        bool movingWindowFlag = false;
        private Point mPoint;
        private Point oldLocation;

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbLeftIcon_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && displayMode == DisplayMode.Desktop && movingWindowFlag)
            {
                this.Location = new Point(this.Location.X + e.X - mPoint.X, this.Location.Y + e.Y - mPoint.Y);

            }
        }


        private void pbLeft_MouseDown(object sender, MouseEventArgs e)
        {
            if (displayMode == DisplayMode.Desktop)
            {
                oldLocation = this.Location;
                mPoint = new Point(e.X, e.Y);
                movingWindowFlag = true;
            }

        }
        #endregion
        Task topMostTask;
        bool topMostEnable = true;
        List<Form> popupFormList;
        private void Form1_Load(object sender, EventArgs e)
        {
            //注册热键
            AppHotKey.RegHotKey(Handle, Config.ReadHotkeyConfigFromFile().hotkeyIndex);
            // Move this window to taskbar.
            SetDisplayMode();
            pbLeftIcon.Tag = "百度";
            Search.GetBaiduHotKeywords();
            Search.GetDefaultExplorer();
            popupFormList = new List<Form>();
            //总在最前
            topMostTask = new Task(() =>
          {
               //设置为最前
               while (true)
              {
                  this.Invoke(new MethodInvoker(
                       () =>
                       {
                           if (displayMode == DisplayMode.TaskBar)
                           {
                               MoveTaskBarButtons(this.Width);
                           }
                            //SetWindowPos(this.Handle, -1, Left, this.Top, this.Width, this.Height, SWP_NOSIZE | SWP_NOREPOSITION | SWP_DEFERERASE | SWP_NOREDRAW);

                            if (topMostEnable)
                           {
                               this.TopMost = true;
                           }
                       }
                       ));
                  Thread.Sleep(30);
              }
          });
            topMostTask.Start();

        }

        private void SetDisplayMode()
        {
            if (displayMode == DisplayMode.TaskBar)
            {
                MoveTaskBarButtons(this.Width);
                //var r=SetParent(Handle, hTaskBar);
                // MoveWindow(Handle, startButtonWidth, 2, this.Width, this.Height, true);
                this.Left = startButtonWidth;
                DisplayTop = Screen.GetBounds(this).Height - mainPanel.Height;

                在桌面显示ToolStripMenuItem.Text = "在桌面显示";
                TransparencyKey = Color.Black;
                //TransparencyKey = Color.Empty;
            }
            else
            {
                MoveTaskBarButtons(0);
                SetParent(Handle, IntPtr.Zero);
                //MoveWindow(Handle, startButtonWidth, 0, this.Width, this.Height, true);
                在桌面显示ToolStripMenuItem.Text = "在任务栏显示";
          
                this.TransparencyKey = Color.Green;
                MoveWindow(Handle, desktopModeLocation.X, desktopModeLocation.Y, this.Width, this.Height, true);
            }

        }

        /// <summary>
        /// GetClient Rect.
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        private Rectangle MyGetRectangle(IntPtr hWnd)
        {
            Rectangle rc = new Rectangle();
            GetWindowRect(hWnd, ref rc);
            var width = rc.Width - rc.X;
            var height = rc.Height - rc.Y;
            rc.Width = width;
            rc.Height = height;
            return rc;
        }

        int cnt = 0;

        // A list of buttons and icons on the taskbar.
        List<WindowInfo> buttonAndBar = new List<WindowInfo>();

        /// <summary>
        /// move taskbar icons to the right
        /// </summary>
        /// <param name="widthToMove"></param>
        private void MoveTaskBarButtons(int widthToMove)
        {
            var hShell = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Shell_TrayWnd", null);
            var hRebar = FindWindowEx(hShell, IntPtr.Zero, "ReBarWindow32", null);
            var hTrayNotify = FindWindowEx(hShell, IntPtr.Zero, "TrayNotifyWnd", null);
            if (rebarOriginalWidth == 0)
            {
                rebarOriginalWidth = MyGetRectangle(hRebar).Width;
            }
            if (trayNotifyOriginalWidth == 0)
            {
                trayNotifyOriginalWidth = MyGetRectangle(hTrayNotify).Width;
            }
            rebarNewWidth = rebarOriginalWidth - widthToMove - (MyGetRectangle(hTrayNotify).Width - trayNotifyOriginalWidth);
            List<IntPtr> listHandles = new List<IntPtr>();
            List<WindowInfo> wndList = new List<WindowInfo>();
            EnumChildWindows(hShell, delegate (IntPtr hWnd, int lParam)
            {
                WindowInfo wnd = new WindowInfo();
                StringBuilder sb = new StringBuilder(256);
                //get hwnd 
                wnd.hWnd = hWnd;
                //get window name 
                GetWindowTextW(hWnd, sb, sb.Capacity);
                wnd.szWindowName = sb.ToString();
                //get window class 
                GetClassNameW(hWnd, sb, sb.Capacity);
                wnd.szClassName = sb.ToString();
                //add it to list 
                wndList.Add(wnd);
                return true;
            }, 0);
            if (buttonAndBar.Count == 0)
            {
                var tmpList = wndList.Where(it => it.szClassName == "ReBarWindow32" || it.szClassName == "TrayButton").ToList();
                foreach (var item in tmpList)
                {
                    if (IsWindowVisible(item.hWnd))
                    {
                        buttonAndBar.Add(item);
                    }
                }
            }

            WindowInfo? startButton = wndList.Where(it => it.szClassName == "Start").FirstOrDefault();

            if (startButton != null)
            {
                if (startButtonWidth == 0)
                {
                    startButtonWidth = MyGetRectangle(startButton.Value.hWnd).Width;
                }
            }

            if (wndList.Count < 21)
            {
                cnt++;
            }

            int widthBeforeThis = startButtonWidth;

            foreach (var item in buttonAndBar)
            {

                var rc = MyGetRectangle(item.hWnd);
                if (item.szClassName == "ReBarWindow32")
                {
                    MoveWindow(item.hWnd, widthToMove + widthBeforeThis, 0, rebarNewWidth, rc.Height, true);
                }
                else
                {
                    MoveWindow(item.hWnd, widthToMove + widthBeforeThis, 0, rc.Width, rc.Height, true);

                }

                widthBeforeThis += rc.Width;
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
            var hStartBtn = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Start", null);
            var rc = MyGetRectangle(hStartBtn);
            var startBtnWidth = rc.Width - rc.X;

            MoveWindow(this.Handle, startBtnWidth, 0, this.Width, this.Height, true);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }


        private void tbSearch_MouseClick(object sender, MouseEventArgs e)
        {
            ShowMainPopupWnd();
        }



        public void AddChildForm(Form child,bool show=true)
        {
            this.Deactivate -= MainForm_Deactivate;
            child.TopLevel = false;
            child.Parent = this;
            this.Height = this.Height + child.Height;
            child.Left = 0;
            if (this.DisplayTop - child.Height > 0)
            {
                child.Top = mainPanel.Top - child.Height;
            }
            else
            {
                child.Top = mainPanel.Top + mainPanel.Height;
            }
            if (!popupFormList.Exists((x)=> { return x == child; }))
            {
                popupFormList.Add(child);
            }
           
            if (show)
                child.Show();
            this.Deactivate += MainForm_Deactivate;
        }
        /// <summary>
        /// Show popup window
        /// </summary>
        private void ShowMainPopupWnd()
        {
            var handler = new System.EventHandler(this.tbSearch_LostFocus);
            tbSearch.LostFocus -= handler;

            // If search keyword is empty, show main popup window.
            if (tbSearch.Text == string.Empty)
            {
                if (keywordWnd != null && keywordWnd.Visible == true)
                {
                    keywordWnd.Hide();
                    
                }
                if (popupWnd == null || popupWnd.Visible == false)
                {
                
                    popupWnd = new PopupMainWnd(tbSearch);
                    AddChildForm(popupWnd);
                    tbSearch.Focus();

                }
            }
            else // if search keyword isn't empty, show keyword window.
            {
                if (popupWnd != null && popupWnd.Visible != false)
                {
                    popupWnd.Close();
                }
                if (keywordWnd == null )
                {

                    keywordWnd = new PopupKeywordWnd(tbSearch, this);
                    //MyDebug.Print("keyword");
                    keywordWnd.Visible = false;
                    keywordWnd.Left = -1000;
                    keywordWnd.Top = -800;
                    keywordWnd.TopLevel = false;
                    keywordWnd.Parent = this;
                    keywordWnd.Show();
               

                    tbSearch.Focus();

                }
                if (keywordWnd != null && keywordWnd.Visible == false)
                {
                  
                    //keywordWnd.Visible = true;

                }
                
                    // call keyword changed event
                    keywordWnd.KeywordUpdated_Handler();
            }

            tbSearch.LostFocus += handler;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Moveback all buttons and icons to original postion.
            MoveTaskBarButtons(0);
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }



        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CallSearchEngine(tbSearch.Text);
            }
        }

        private void pbLeft_Click(object sender, MouseEventArgs e)
        {
            if (this.Location != oldLocation && displayMode == DisplayMode.Desktop)
            {
                return;
            }

            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(e.Location.X + Location.X, e.Location.Y + Location.Y);
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (leftPopupWnd == null || leftPopupWnd.Visible == false)
                {
                    leftPopupWnd = new LeftPopupWnd(pbLeftIcon);


                    //总在最前
                    AddChildForm(leftPopupWnd);

                    pbArrow.Image = Properties.Resources.more_btn;
                }
     
            }
        }

        private void tbSearch_LostFocus(object sender, EventArgs e)
        {

  
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            CallSearchEngine(tbSearch.Text);
        }

        public void CallSearchEngine(string keywords)
        {
            string url;
            string searchEngineName = ((string)pbLeftIcon.Tag);
            switch (searchEngineName)
            {
                case "百度":
                    url = "https://www.baidu.com/baidu?word=" + keywords;
                    Process.Start(url);
                    break;
                case "搜狗":

                    url = "https://www.sogou.com/web?query=a" + keywords;
                    Process.Start(url);
                    break;
                case "360搜索":
                    url = "https://www.so.com/s?q=" + keywords;
                    Process.Start(url);
                    break;

            }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            ShowMainPopupWnd();
        }

        private void 意见反馈ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Advises advices = new Advises();

            advices.Show();
        }

        private void 基础设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BasicSettings bsWnd = new BasicSettings(this);
            bsWnd.Show();
        }

        private void 在桌面显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (displayMode == DisplayMode.TaskBar)
            {
                displayMode = DisplayMode.Desktop;
            }
            else
            {
                displayMode = DisplayMode.TaskBar;
            }
            SetDisplayMode();


        }

        private void 在线帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HttpHelper.Goto(HttpHelper.baseUrl);

        }

        private void pbLeftIcon_MouseUp(object sender, MouseEventArgs e)
        {

            movingWindowFlag = false;
        }

        private void pbLeftIcon_Click(object sender, EventArgs e)
        {

        }

        private void 检查更新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string ver = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string ver = VERSION.currentVersion;
            Process.Start(AppDomain.CurrentDomain.BaseDirectory + "updatesoftware.exe", this.Handle.ToString() + ";" + ver);

        }

        public void HotKeyPressedHandler()
        {

            ShowMainPopupWnd();
            this.tbSearch.Focus();
            //    MessageBox.Show("快捷键被调用！");
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            BringToFront();
        }
        private int DisplayTop
        {
            get
            {
                return this.Top + 500;
            }
            set
            {
                this.Top = value - 500;
            }
        }

        private void MainForm_Deactivate(object sender, EventArgs e)
        {

           while(popupFormList.Count>0)
            {
                if (popupFormList[0] is PopupKeywordWnd)
                {
                    popupFormList[0].Visible = false;
                    MyDebug.Print("hide");
                   
                }
                else
                {
                    popupFormList[0].Close();
                   
                }

                popupFormList.RemoveAt(0);
            }
        }
    }
}
