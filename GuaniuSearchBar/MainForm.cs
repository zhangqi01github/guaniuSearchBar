using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuaniuSearchBar
{
    public partial class MainForm : Form
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

        // handle of TaskBar
        IntPtr hTaskBar = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Shell_TrayWnd", null);
        int startButtonWidth;
        int rebarNewWidth = 0;
        int rebarOriginalWidth = 0;

        int trayNotifyOriginalWidth = 0;

        PopupMainWnd popupWnd;
        PopupKeywordWnd keywordWnd;
        LeftPopupWnd leftPopupWnd;

        private void Form1_Load(object sender, EventArgs e)
        {
            MoveTaskBarButtons(this.Width);
            // Move this window to taskbar.
            SetParent(Handle, hTaskBar);
            MoveWindow(Handle, startButtonWidth, 0, this.Width, this.Height, true);
            pbLeftIcon.Tag = "百度";
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
            MoveTaskBarButtons(this.Width);
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
                    popupWnd.Top = this.Top - popupWnd.Height;
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
                // ca;ll keyword changed event
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
            Application.Exit();
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
    }
}
