using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wechat_Notifier
{


    public class WechatMessage
    {
        public WechatMessage()
        { }

        public WechatMessage(String wechatName, String message)
        {
            this.date = DateTime.Today;
            this.wechatName = wechatName;
            this.message = message;
        }


        public WechatMessage(String wechatName, String name, String message)
        {
            this.date = DateTime.Today;
            this.wechatName = wechatName;
            this.name = name;
            this.message = message;
        }

        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        private String wechatName;

        public String WechatName
        {
            get { return wechatName; }
            set { wechatName = value; }
        }

        private String at;

        public String At
        {
            get { return at; }
            set { at = value; }
        }

        private String name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        private String message;

        public String Message
        {
            get { return message; }
            set { message = value; }
        }
    }
}
