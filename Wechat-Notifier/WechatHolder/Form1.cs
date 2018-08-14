using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;

namespace WechatHolder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //dockIt();
           // HostWechat();
        }

        private Process pDocked;
        private IntPtr hWndOriginalParent;
        private IntPtr hWndParent;
        private IntPtr hWndDocked;

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);


        [DllImport("User32.dll")]
        static extern IntPtr GetParent(IntPtr hwnd);
        
        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOZORDER = 0x0004;
        const int WM_LBUTTONDOWN = 0x0201;
        const int WM_LBUTTONUP = 0x0202;


        private void dockIt()
        {
            if (hWndDocked != IntPtr.Zero) //don't do anything if there's already a window docked.
                return;
            hWndParent = IntPtr.Zero;
            String wechatFilePath = @"C:\Tencent\WeChat\WeChat.exe";
            if (!File.Exists(wechatFilePath)) return;
            pDocked = Process.Start(wechatFilePath);
            while (hWndDocked == IntPtr.Zero)
            {
                pDocked.WaitForInputIdle(1000); //wait for the window to be ready for input;
                pDocked.Refresh();              //update process info
                if (pDocked.HasExited)
                {
                    return; //abort if the process finished before we got a handle.
                }
                hWndDocked = pDocked.MainWindowHandle;  //cache the window handle
            }
            //Windows API call to change the parent of the target window.
            //It returns the hWnd of the window's parent prior to this call.
            hWndOriginalParent = GetParent(hWndDocked);
            SetParent(hWndDocked, this.WechatPanel.Handle);

            //Wire up the event to keep the window sized to match the control
            WechatPanel.SizeChanged += new EventHandler(Panel1_Resize);
            //Perform an initial call to set the size.
            Panel1_Resize(new Object(), new EventArgs());
        }

        public int MakeLParam(int LoWord, int HiWord)
{
    return (int)((HiWord << 16) | (LoWord & 0xFFFF));
}



        private void undockIt()
        {
            //Restores the application to it's original parent.
            SetParent(hWndDocked, hWndOriginalParent);
        }

        private void Panel1_Resize(object sender, EventArgs e)
        {
            //Change the docked windows size to match its parent's size. 
           // MoveWindow(hWndDocked, 0, 0, WechatPanel.Width, WechatPanel.Height, true);
            MoveWindow(hWndDocked, 0, 0, this.Width, this.Height, true);
        }

        private void HostWechat()
        {
            // Find Wechat handler
            IntPtr hWndDocked = FindWindow("WeChatMainWndForStore", "WeChat");

            // If found, position it.
            if (hWndDocked == IntPtr.Zero)
            {
                return;
            }
            hWndOriginalParent = GetParent(hWndDocked);
            SetParent(hWndDocked, this.WechatPanel.Handle);
            //Wire up the event to keep the window sized to match the control
            WechatPanel.SizeChanged += new EventHandler(Panel1_Resize);
            //Perform an initial call to set the size.
            //Panel1_Resize(new Object(), new EventArgs());
            MoveWindow(hWndDocked, 0, 0, WechatPanel.Width, WechatPanel.Height, true);
           // MoveWindow(hWndDocked, 0, 0, this.Width, this.Height, true);
         }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Restores the application to it's original parent.
           // SetParent(hWndDocked, hWndOriginalParent);

        }

        private void Dockbtn_Click(object sender, EventArgs e)
        {
            HostWechat();
        }

        private void UndockBtn_Click(object sender, EventArgs e)
        {
            PostMessage(this.WechatPanel.Handle, WM_LBUTTONDOWN, 1, MakeLParam(100, 35));
            PostMessage(this.WechatPanel.Handle, WM_LBUTTONUP, 0, MakeLParam(100, 35));
        }
    }
}