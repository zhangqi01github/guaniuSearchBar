using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Renderer
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
         [DllImport("user32.dll")]
        static extern bool IsWindowVisible(IntPtr hWnd);
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
        [STAThread]
        static void Main(string[] arg)
        {
            var hwnd = (IntPtr)int.Parse(arg[0]);
            while(true)
            {
                SetWindowPos(hwnd, -1, 48, 1041, 248, 39, SWP_NOSIZE | SWP_NOREPOSITION | SWP_NOREDRAW);
            }
           
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }
    }
}
