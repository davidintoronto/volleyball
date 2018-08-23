using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace Wechat_Notifier
{
    public partial class Notifier : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOZORDER = 0x0004;
        const int WAIT = 1000;
        const String ENTER = "{ENTER}";

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindowDC(IntPtr window);
        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern uint GetPixel(IntPtr dc, int x, int y);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int ReleaseDC(IntPtr window, IntPtr dc);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
        [StructLayout(LayoutKind.Sequential)]

        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public Notifier()
        {
            InitializeComponent();
            ResetHourSharpTimer();
            WechatTimer.Start();
        }

        private void ResetHourSharpTimer()
        {
            int minutes = DateTime.Now.Minute;
            int seconds = DateTime.Now.Second;
            int interval = ((60 - minutes) * 60 + (60 - seconds)) * 1000;
            this.HourSharpTimer.Interval = interval;
            this.HourSharpTimer.Start();
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            ScheduleTaskTimer_Tick(sender, e);
            SendMessagesToWechat();
        }

        private void SendMessagesToWechat()
        {
            // Find Wechat handler
            IntPtr hWnd = FindWindow("WeChatMainWndForStore", "WeChat");

            // If found, position it.
            if (hWnd == IntPtr.Zero)
            {
                return;
            }
            VballMangerWebservice.WechatMessage[] vballMessages;
            try
            {
               vballMessages = QueryVballMessages();
            }
            catch
            {
                this.MessageLb.Text = "Failed on webservice call";
                return;
            }
            if (vballMessages ==null || vballMessages.ToArray().Length == 0)
            {
                return;
            }
            // Move the window to (0,0) without changing its size or position
            // in the Z order.
            //SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
            RECT rct = new RECT();
            GetWindowRect(hWnd, ref rct);
            foreach (VballMangerWebservice.WechatMessage vballMessage in vballMessages)
            {
                try
                {
                    //Bring wechat to front
                    SetForegroundWindow(hWnd);
                    //Click on clean search field
                    DoMouseClick(rct, 243, 35);
                    //Click on search 
                    DoMouseClick(rct, 100, 35);
                    //Thread.Sleep(WAIT);
                    //Copy chat name or group into clipboard
                    Clipboard.SetText(vballMessage.WechatName);
                    SendKeys.SendWait("^{v}");
                    Thread.Sleep(WAIT);
                    //Check to see if chat name or group found
                    if (NoMatchChat(rct))
                    {
                        continue;
                    }
                    //Thread.Sleep(WAIT);
                    //Click on first match one
                    DoMouseClick(rct, 80, 120);
                    //Check to see if message contains {ENTER}
                    Thread.Sleep(WAIT);
                    if (!String.IsNullOrEmpty(vballMessage.At))
                    {
                        SendKeys.SendWait("@" + vballMessage.At);
                        SendKeys.SendWait(ENTER);
                        //Thread.Sleep(WAIT);
                    }
                    if (String.IsNullOrEmpty(vballMessage.Name))
                    {
                        Clipboard.SetText(vballMessage.Message);
                    }
                    else
                    {
                        Clipboard.SetText("Hi, " + vballMessage.Name + ". " + vballMessage.Message);
                    }
                    Thread.Sleep(WAIT);
                    SendKeys.SendWait("^{v}");
                    SendKeys.SendWait(ENTER);
                }
                catch (Exception ex)
                {
                    this.MessageLb.Text = ex.Message;
                }
            }

            //    this.MessageLb.Text = this.Location.X + "-" + this.Location.Y;
        }

        private VballMangerWebservice.WechatMessage[] QueryVballMessages()
        {
            VballMangerWebservice.VballWebServiceSoapClient client = new VballMangerWebservice.VballWebServiceSoapClient();
            return client.WechatMessages();
        }

        private void DoMouseClick(RECT rct, int x, int y)
        {
            Cursor.Position = new Point(rct.Left + x, rct.Top + y);
            //Call the imported function with the cursor's current position
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }

        private bool IsNoMatchColorAt(int x, int y)
        {
            int R = 25;
            int G = 173;
            int B = 26;
            IntPtr desk = GetDesktopWindow();
            IntPtr dc = GetWindowDC(desk);
            int a = (int)GetPixel(dc, x, y);
            ReleaseDC(desk, dc);
            Color color = Color.FromArgb(255, (a >> 0) & 0xff, (a >> 8) & 0xff, (a >> 16) & 0xff);
            return color.R == R && color.G == G && color.B == B;
        }

        private bool NoMatchChat(RECT rct)
        {
            return IsNoMatchColorAt(rct.Left + 68, rct.Top + 80) && IsNoMatchColorAt(rct.Left + 95, rct.Top + 80) && IsNoMatchColorAt(rct.Left + 83, rct.Top + 120) && IsNoMatchColorAt(rct.Left + 68, rct.Top + 133) && IsNoMatchColorAt(rct.Left + 95, rct.Top + 133);

        }

        private void ScheduleTaskTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                VballMangerWebservice.VballWebServiceSoapClient client = new VballMangerWebservice.VballWebServiceSoapClient();
                client.RunScheduleTasks(DateTime.Now.Hour);
            }catch(Exception){
            }
            ResetHourSharpTimer();
        }

        private void WechatTimer_Tick(object sender, EventArgs e)
        {
            SendMessagesToWechat();
            WechatTimer.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                VballMangerWebservice.VballWebServiceSoapClient client = new VballMangerWebservice.VballWebServiceSoapClient();
                client.RunScheduleTasks(DateTime.Now.Hour);
            }
            catch (Exception ex)
            {
                this.MessageLb.Text = ex.Message;
            }
        }
    }
}
