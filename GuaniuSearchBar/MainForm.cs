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

        DisplayMode displayMode= DisplayMode.Desktop;
        // handle of TaskBar
        IntPtr hTaskBar = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Shell_TrayWnd", null);
        int startButtonWidth;
        int rebarNewWidth = 0;
        int rebarOriginalWidth = 0;

        int trayNotifyOriginalWidth = 0;
        Point desktopModeLocation=new Point(Screen.PrimaryScreen.WorkingArea.Width/3,300);

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
        public const int HotKeyID = 0x3572; //热键ID
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_HOTKEY: //窗口消息-热键ID
                    switch (m.WParam.ToInt32())
                    {
                        case HotKeyID: //热键ID
                            HotKeyPressedHandler();
                            break;
                        default:
                            break;
                    }
                    break;
                case WM_CREATE: //窗口消息-创建
                    
                    break;
                case WM_DESTROY: //窗口消息-销毁
                    AppHotKey.UnRegKey(Handle, HotKeyID); //销毁热键
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

                    base.WndProc(ref m);


                    break;
            }
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
                if (leftPopupWnd != null && leftPopupWnd.Visible)
                {
                    leftPopupWnd.Location = new Point(leftPopupWnd.Location.X + e.X - mPoint.X, leftPopupWnd.Location.Y + e.Y - mPoint.Y);
  
                }
                if (popupWnd != null && popupWnd.Visible)
                {
                    popupWnd.Location = new Point(popupWnd.Location.X + e.X - mPoint.X, popupWnd.Location.Y + e.Y - mPoint.Y);
                }
            }
        }


        private void pbLeft_MouseDown(object sender, MouseEventArgs e)
        {
            if (displayMode==DisplayMode.Desktop)
            {
                oldLocation = this.Location;
                mPoint = new Point(e.X, e.Y);
                movingWindowFlag = true;
            }
          
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            // Move this window to taskbar.
            SetDisplayMode();
            pbLeftIcon.Tag = "百度";
            Search.GetBaiduHotKeywords();
            Search.GetDefaultExplorer();

        }

        private void SetDisplayMode()
        {
            if (displayMode == DisplayMode.TaskBar)
            {
                MoveTaskBarButtons(this.Width);
                SetParent(Handle, hTaskBar);
                MoveWindow(Handle, startButtonWidth, 2, this.Width, this.Height, true);
               
                在桌面显示ToolStripMenuItem.Text = "在桌面显示";
                TransparencyKey = Color.Empty;
            }
            else
            {
                MoveTaskBarButtons(0);
                SetParent(Handle, IntPtr.Zero);
                //MoveWindow(Handle, startButtonWidth, 0, this.Width, this.Height, true);
                在桌面显示ToolStripMenuItem.Text = "在任务栏显示";
                this.TransparencyKey = Color.Black;
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
            if (displayMode == DisplayMode.TaskBar)
            {
                MoveTaskBarButtons(this.Width);
            }
           // this.TopMost = true;
        }


        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            ShowPopupWnd();  
        }

        /// <summary>
        /// Show popup window
        /// </summary>
        private void ShowPopupWnd()
        {
            var handler = new System.EventHandler(this.tbSearch_LostFocus);
            tbSearch.LostFocus -= handler;

            // If search keyword is empty, show main popup window.
            if (tbSearch.Text == string.Empty)
            {
                if (popupWnd == null || popupWnd.Visible == false)
                {
                    if (keywordWnd != null && keywordWnd.Visible == true)
                    {
                        keywordWnd.Close();
                    }
                    popupWnd = new PopupMainWnd(tbSearch);
                    popupWnd.Show();
                    popupWnd.Left = this.Left;
                    if (this.Top - popupWnd.Height<0)
                    {
                        popupWnd.Top = this.Top + this.Height;
                    }
                    else
                    {
                        popupWnd.Top = this.Top - popupWnd.Height ;
                    }
               
                    popupWnd.TopMost = true;
                    tbSearch.Focus();
                    popupWnd.Deactivate += (_sender, _e) => { popupWnd.Close(); };
                }
            }
            else // if search keyword isn't empty, show keyword window.
            {

                if (keywordWnd == null || keywordWnd.Visible == false)
                {
                    if (popupWnd != null && popupWnd.Visible == true)
                    {
                        popupWnd.Close();
                    }
                    keywordWnd = new PopupKeywordWnd(tbSearch,this);
                    keywordWnd.Show();
                    keywordWnd.Left = this.Left;
                    keywordWnd.TopMost = true;
                    tbSearch.Focus();
                    keywordWnd.Deactivate += (_sender, _e) => { keywordWnd.Close(); };
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
            if( this.Location!= oldLocation && displayMode==DisplayMode.Desktop)
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
                    leftPopupWnd.Show();
                    leftPopupWnd.Left = this.Left;
                    leftPopupWnd.Top = this.Top - leftPopupWnd.Height;
                    leftPopupWnd.TopMost = true;
                    //tbSearch.Focus();
                    leftPopupWnd.Deactivate += (_sender, _e) =>
                    {
                        leftPopupWnd.Close();
                        pbArrow.Image = Properties.Resources.less_btn;
                    };
                    pbArrow.Image = Properties.Resources.more_btn;
                }
                else
                {
                    leftPopupWnd.Close();
                }
            }
        }

        private void tbSearch_LostFocus(object sender, EventArgs e)
        {
          
            if (popupWnd!=null && !popupWnd.Focused)
            {
                popupWnd.Close();
            }
            if (keywordWnd != null && !keywordWnd.Focused)
            {
                keywordWnd.Close();
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            CallSearchEngine(tbSearch.Text);
        }

        public  void CallSearchEngine( string keywords)
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
            ShowPopupWnd();
        }

        private void 意见反馈ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Advices advices = new Advices();
            
            advices.Show();
        }

        private void 基础设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BasicSettings bsWnd = new BasicSettings(this);
            bsWnd.Show();
        }

        private void 在桌面显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (displayMode==DisplayMode.TaskBar)
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
            //Process.Start("");
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
            string ver = Assembly.GetExecutingAssembly().GetName().Version.ToString();
   
            Process.Start(AppDomain.CurrentDomain.BaseDirectory + "updatesoftware.exe", this.Handle.ToString()+";"+ver);
               
        }

        public void HotKeyPressedHandler()
        {
            MessageBox.Show("快捷键被调用！");
        }
    }
}
