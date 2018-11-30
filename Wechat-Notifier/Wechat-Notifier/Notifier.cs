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
using System.IO;

namespace Wechat_Notifier
{
    public partial class Notifier : Form
    {
        public const String DATAFILE = @"\App_Data\Reservation.data.";
        
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOZORDER = 0x0004;
        const int THREE_SND = 3000;
        const int ONE_SND = 1000;
        const int HALF_SND = 500;
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
            //ResetHourSharpTimer();
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
            IntPtr wechatMainHandler = FindWindow("WeChatMainWndForStore", "WeChat");

            // If found, position it.
            if (wechatMainHandler == IntPtr.Zero)
            {
                this.WechatStatusLb.Text = "Wechat is not running - " + DateTime.Now.ToLocalTime();
                return;
            }
            this.WechatStatusLb.Text = "Wechat is running - " + DateTime.Now.ToLocalTime();
            //Retrieve vball system wechat messages and send out
            WechatMessage[] vballMessages;
            try
            {
               vballMessages = QueryVballMessages();
               this.MessageNumberLb.Text = "Received message number : " + vballMessages.Length;
               SendMessagesToWechat(wechatMainHandler, vballMessages);
            }
            catch(Exception ex)
            {
                this.LogTb.Text = this.LogTb.Text + "\r\n" + DateTime.Now.ToString() + " " + ex.Message;
                this.MessageLb.Text = "Failed on webservice call";
                return;
            }
            //Retrieve activity wechat messages and send out
            WechatMessage[] activityMessages;
            try
            {
                activityMessages = QueryActivityMessages();
                SendMessagesToWechat(wechatMainHandler, activityMessages);
            }
            catch(Exception ex)
            {
                this.LogTb.Text = this.LogTb.Text + "\r\n" + DateTime.Now.ToString() + " " + ex.Message;
                this.MessageLb.Text = "Failed on webservice call";
                return;
            }
        }

        private void SendMessagesToWechat(IntPtr wechatMainHandler, WechatMessage[] vballMessages)
        {
            if (vballMessages ==null || vballMessages.ToArray().Length == 0)
            {
                return;
            }
            // Move the window to (0,0) without changing its size or position
            // in the Z order.
            //SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
            RECT rct = new RECT();
            GetWindowRect(wechatMainHandler, ref rct);
            foreach (WechatMessage vballMessage in vballMessages)
            {
                if (vballMessage.Message == "{ReservationLink}")
                {
                    //SendReservationLink(wechatMainHandler, rct, vballMessage);
                }
                else
                {
                    SendMessage(wechatMainHandler, rct, vballMessage);
                }
            }
        }
        //Send reservation link
        private void SendReservationLink(IntPtr wechatMainHandler, RECT rct, WechatMessage vballMessage)
        {
            try
            {
                //Bring wechat to front
                SetForegroundWindow(wechatMainHandler);
                //Click on favorates tab
                DoMouseClick(rct, 20, 190);
                Thread.Sleep(HALF_SND);
                DoMouseClick(rct, 80, 150);
                Thread.Sleep(HALF_SND);
                //Right click on the first favorate
                DoMouseRightClick(rct, 400, 110);
                Thread.Sleep(ONE_SND);
                //Select forward
                DoMouseClick(rct, 450, 150);
                Thread.Sleep(HALF_SND);
                //Thread.Sleep(WAIT);
               IntPtr selectContactWndHandler = FindWindow("SelectContactWnd", "WeChat");

                // If found, position it.
                if (selectContactWndHandler == IntPtr.Zero)
                {
                    return;
                }
                //Copy chat name or group into clipboard
                Clipboard.SetText(vballMessage.WechatName);
                SendKeys.SendWait("^{v}");
                Thread.Sleep(HALF_SND);
                 //Detect the SelectContactWnd location
                RECT rect = new RECT();
                GetWindowRect(selectContactWndHandler, ref rect);
                //Select the first on the list
                Thread.Sleep(HALF_SND);
                DoMouseClick(rect, 35, 80);
                //Click Ok button
                Thread.Sleep(ONE_SND);
                DoMouseClick(rect, 420, 450);
                //Click Cancel button in case of failure
                Thread.Sleep(HALF_SND);
                DoMouseClick(rect, 420, 490);
                Thread.Sleep(HALF_SND);
            }
            catch (Exception ex)
            {
                this.LogTb.Text = this.LogTb.Text + "\r\n" + DateTime.Now.ToString() + " " + ex.Message;
                this.MessageLb.Text = ex.Message;
            }
        }

        private void SendMessage(IntPtr wechatMainHandler,  RECT rct, WechatMessage vballMessage)
        {
            try
            {
                //Bring wechat to front
                SetForegroundWindow(wechatMainHandler);
                //Click on message tab
                DoMouseClick(rct, 20, 90);
                Thread.Sleep(HALF_SND);
                //Click on clean search field
                DoMouseClick(rct, 243, 35);
                //Click on search 
                DoMouseClick(rct, 100, 35);
                //Thread.Sleep(WAIT);
                //Copy chat name or group into clipboard
                Clipboard.SetText(vballMessage.WechatName);
                SendKeys.SendWait("^{v}");
                Thread.Sleep(ONE_SND);
                //Check to see if chat name or group found
                /*if (NoMatchChat(rct))
                {
                    continue;
                }*/
                //Thread.Sleep(WAIT);
                //Click on first match one
                DoMouseClick(rct, 80, 120);
                Thread.Sleep(ONE_SND);
                //Check if wechat search window pops up
                if (GetForegroundWindow() != wechatMainHandler)
                {
                    return;
                }
                //Check to see if message contains {ENTER}
                Thread.Sleep(HALF_SND);
                if (!String.IsNullOrEmpty(vballMessage.At))
                {
                    SendKeys.SendWait("@" + vballMessage.At);
                    Thread.Sleep(HALF_SND);
                    SendKeys.SendWait(ENTER);
                }
                if (String.IsNullOrEmpty(vballMessage.Name))
                {
                    Clipboard.SetText(vballMessage.Message);
                }
                else
                {
                    Clipboard.SetText("Hi, " + vballMessage.Name + ". " + vballMessage.Message);
                }
                Thread.Sleep(HALF_SND);
                SendKeys.SendWait("^{v}");
                Thread.Sleep(HALF_SND);
                SendKeys.SendWait(ENTER);
                Thread.Sleep(HALF_SND);
            }
            catch (Exception ex)
            {
                this.LogTb.Text = this.LogTb.Text + "\r\n" + DateTime.Now.ToString() + " " + ex.Message;
                //this.MessageLb.Text = ex.Message;
            }
        }

        private WechatMessage[] QueryVballMessages()
        {
            VballManagerWebservice.VballWebServiceSoapClient client = new VballManagerWebservice.VballWebServiceSoapClient();
            List<WechatMessage> wechatMessages = new List<WechatMessage>();
            foreach (VballManagerWebservice.WechatMessage wechatMessage in client.WechatMessages())
            {
                WechatMessage theWechatMessage = new WechatMessage();
                theWechatMessage.At = wechatMessage.At;
                theWechatMessage.Date = wechatMessage.Date;
                theWechatMessage.Message = wechatMessage.Message;
                theWechatMessage.Name = wechatMessage.Name;
                theWechatMessage.WechatName = wechatMessage.WechatName;
                wechatMessages.Add(theWechatMessage);
            }
            return wechatMessages.ToArray();
        }
        private WechatMessage[] QueryActivityMessages()
        {
            ActivityWebservice.ActivityWebServiceSoapClient client = new ActivityWebservice.ActivityWebServiceSoapClient();
            List<WechatMessage> wechatMessages = new List<WechatMessage>();
            foreach (ActivityWebservice.WechatMessage wechatMessage in client.WechatMessages())
            {
                WechatMessage theWechatMessage = new WechatMessage();
                theWechatMessage.At = wechatMessage.At;
                theWechatMessage.Date = wechatMessage.Date;
                theWechatMessage.Message = wechatMessage.Message;
                theWechatMessage.Name = wechatMessage.Name;
                theWechatMessage.WechatName = wechatMessage.WechatName;
                wechatMessages.Add(theWechatMessage);
            }
            return wechatMessages.ToArray();
        }

        private void DoMouseClick(RECT rct, int x, int y)
        {
            Cursor.Position = new Point(rct.Left + x, rct.Top + y);
            //Call the imported function with the cursor's current position
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }

        private void DoMouseRightClick(RECT rct, int x, int y)
        {
            Cursor.Position = new Point(rct.Left + x, rct.Top + y);
            //Call the imported function with the cursor's current position
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, X, Y, 0, 0);
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
            DoScheduleTasks();
        }

        private void DoScheduleTasks()
        {
            try
            {
                VballManagerWebservice.VballWebServiceSoapClient client = new VballManagerWebservice.VballWebServiceSoapClient();
                client.RunScheduleTasks(DateTime.Now.Hour);
                String reservationData = client.RetrieveData(DateTime.Now.Hour);
                if (!String.IsNullOrEmpty(reservationData)) File.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory + DateTime.Today.ToString("yyyy-MM-dd")+ DATAFILE , reservationData);
            }catch(Exception ex){
                this.LogTb.Text = this.LogTb.Text + "\r\n" + DateTime.Now.ToString() + " " + ex.Message;
            }
           // ResetHourSharpTimer();
        }

        private void WechatTimer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.Minute == 0)
            {
                DoScheduleTasks();
            }
            SendMessagesToWechat();
            int seconds = DateTime.Now.Second;
            int interval = (60 - seconds) * 1000;
            WechatTimer.Interval = interval;
            WechatTimer.Start();
        }

        private void HourSharpBtn_Click(object sender, EventArgs e)
        {
            try
            {
                VballManagerWebservice.VballWebServiceSoapClient client = new VballManagerWebservice.VballWebServiceSoapClient();
                client.RunScheduleTasks(DateTime.Now.Hour);
                //String reservationData = client.RetrieveData(DateTime.Now.Hour);
                //if (!String.IsNullOrEmpty(reservationData)) File.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory + DATAFILE + DateTime.Today.ToString("yyyy-MM-dd"), reservationData);
            }
            catch (Exception ex)
            {
                this.LogTb.Text = this.LogTb.Text + "\r\n" + DateTime.Now.ToString() + " " + ex.Message;
            }
        }

        private void ShowWindowStartLocation_Click(object sender, EventArgs e)
        {
            this.LogTb.Text = this.Location.X + "|" + this.Location.Y; 
        }

        private void Test_GetForeWindow(object sender, EventArgs e)
        {
            // Find Wechat handler
            IntPtr wechatMainHandler = FindWindow("WeChatMainWndForStore", "WeChat");

            // If found, position it.
            if (wechatMainHandler == IntPtr.Zero)
            {
                return;
            }
            SetForegroundWindow(wechatMainHandler);
            RECT rct = new RECT();
            GetWindowRect(wechatMainHandler, ref rct);
            //Click on clean search field
            DoMouseClick(rct, 243, 35);
            //Click on search 
            DoMouseClick(rct, 100, 35);
            //Thread.Sleep(WAIT);
            //Copy chat name or group into clipboard
            Clipboard.SetText("vballMessage.WechatName");
            SendKeys.SendWait("^{v}");
            Thread.Sleep(ONE_SND);
            //Check to see if chat name or group found
            /*if (NoMatchChat(rct))
            {
                continue;
            }*/
            //Thread.Sleep(WAIT);
            //Click on first match one
            DoMouseClick(rct, 80, 120);
            //Check if wechat search window pops up
            Thread.Sleep(ONE_SND);
            if (GetForegroundWindow() != wechatMainHandler)
            {
                this.MessageLb.Text="Wechat search window is open";
            }
        }

    }
}
