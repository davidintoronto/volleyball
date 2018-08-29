using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VballManager
{
    public class WechatNotify
    {
        private bool enable;
        private List<WechatMessage> wechatMessages = new List<WechatMessage>();
        private String wechatMemberWelcomeMessage;
        private String wechatDropinWelcomeMessage;
        private String wechatPoolMessage;
        private String wechatPrimaryMemberMessage;
        private String wechatToAllTestMessage;
        private List<EmoMessage> emoMessages = new List<EmoMessage>();


        public WechatNotify() { }

        public bool Enable
        {
            get { return enable; }
            set { enable = value; }
        }
        public List<EmoMessage> EmoMessages
        {
            get { return emoMessages; }
            set { emoMessages = value; }
        }

        public String WechatToAllTestMessage
        {
            get { return wechatToAllTestMessage; }
            set { wechatToAllTestMessage = value; }
        }

        public String WechatMemberWelcomeMessage
        {
            get { return wechatMemberWelcomeMessage; }
            set { wechatMemberWelcomeMessage = value; }
        }

        public String WechatDropinWelcomeMessage
        {
            get { return wechatDropinWelcomeMessage; }
            set { wechatDropinWelcomeMessage = value; }
        }

        public String WechatPoolMessage
        {
            get { return wechatPoolMessage; }
            set { wechatPoolMessage = value; }
        }

        public String WechatPrimaryMemberMessage
        {
            get { return wechatPrimaryMemberMessage; }
            set { wechatPrimaryMemberMessage = value; }
        }
        public List<WechatMessage> WechatMessages
        {
            get { return wechatMessages; }
            set { wechatMessages = value; }
        }

        public void AddNotifyWechatMessage(Player player, String message)
        {
            if (Enable && !String.IsNullOrEmpty(player.WechatName))
            {
                WechatMessage wechat = new WechatMessage(player.WechatName, player.Name, message);
                WechatMessages.Add(wechat);
            }
        }
        public void AddNotifyWechatMessage(Pool pool, String message)
        {
            if (Enable && !String.IsNullOrEmpty(pool.WechatGroupName))
            {
                WechatMessage wechat = new WechatMessage(pool.WechatGroupName, message);
                WechatMessages.Add(wechat);
            }
        }

        public void AddNotifyWechatMessage(Pool pool, Player player, String message)
        {
            if (Enable && !String.IsNullOrEmpty(pool.WechatGroupName) && !String.IsNullOrEmpty(message))
            {
                WechatMessage wechat = new WechatMessage(pool.WechatGroupName, player, message);
                WechatMessages.Add(wechat);
            }
        }

        public EmoMessage FindEmoMessageById(String id)
        {
            return this.emoMessages.Find(emo => emo.Id == id);
        }

        public String GetEmoMessage(int type, int playerNumber) {
            List<EmoMessage> emos = this.emoMessages.FindAll(emo => (int)emo.Type == type && playerNumber >= emo.Min && playerNumber <= emo.Max);
            if (emos.Count > 0)
            {
                int index = new Random().Next(emos.Count);
                return emos[index].Message;
            }
            return null;
        }
    }

    public enum EmoTypes
    {
        Reserve=1, Cancel=0, Move=2
    }

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

        public WechatMessage(String wechatName, Player player, String message)
        {
            this.date = DateTime.Today;
            this.wechatName = wechatName;
            this.message = message;
            if (String.IsNullOrEmpty(player.WechatName))
            {
                this.name = player.Name;
            }
            else
            {
                this.at = player.WechatName;
            }
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

    public class EmoMessage
    {
        private String id;
        private int type = 0;
        private int min;
        private int max;
        private String message;
        public EmoMessage()
        {
        }
        public EmoMessage(int type, int min, int max, String message)
        {
            this.type = type;
            this.id = Guid.NewGuid().ToString();
            this.min = min;
            this.max = max;
            this.message = message;
        }

        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        public String Id
        {
            get { return id; }
            set { id = value; }
        }

        public int Min
        {
            get { return min; }
            set { min = value; }
        }

        public int Max
        {
            get { return max; }
            set { max = value; }
        }

        public String Message
        {
            get { return message; }
            set { message = value; }
        }
    }

}
