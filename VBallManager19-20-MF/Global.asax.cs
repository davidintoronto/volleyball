using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Net;
using System.IO;

namespace VballManager
{
    public class Global : System.Web.HttpApplication
    {
        private Timer timer;

        void Application_Start(object sender, EventArgs e)
        {
            Application[Constants.DATA] = DataAccess.LoadReservation();
            Double msToNextHourSharp = ((60 - DateTime.UtcNow.Minute) * 60 - DateTime.UtcNow.Second) * 1000;
           // timer = new Timer(msToNextHourSharp);
            //timer.Elapsed += OnTimedEvent;
            //timer.Enabled = true;
            //timer.Start();
            
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            
            //File.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory + Constants.IP_FILE + DateTime.Now.ToString("HHmmss"), "=");
           // AutoAssignCoopSpots(DateTime.UtcNow.Hour);
            //Run auto reserve
            //Uri url = this.Context.Request.Url;
            //Calculate interval for next hour run
            Double msToNextHourSharp = ((60 - DateTime.UtcNow.Minute) * 60 - DateTime.UtcNow.Second) * 1000;
            timer.Interval = msToNextHourSharp;
            //timer.Elapsed += OnTimedEvent;
            timer.Enabled = true;
            timer.Start();
            
        }

        private void AutoAssignCoopSpots(int hour)
        {
            String slash = "/";
            String autoReserveUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, HttpContext.Current.Request.ApplicationPath);
            File.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory + Constants.IP_FILE + DateTime.Now.ToString("HHmmss"), autoReserveUrl);
            if (!autoReserveUrl.EndsWith(slash)) autoReserveUrl = autoReserveUrl + slash;
            autoReserveUrl = autoReserveUrl + Constants.AUTO_RESERVE + "?hour=" + hour;
            var http = (HttpWebRequest)WebRequest.Create(autoReserveUrl);
            var response = http.GetResponse();
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

    }
}
