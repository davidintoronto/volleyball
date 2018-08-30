using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reservation
{
    public class Player
    {
        private String id;
        private String name;
        private String passcode;
        private bool marked;
        private List<String> feeIds = new List<String>();

        public Player()
        {
        }

        public Player(String name, String passcode, bool marked)
        {
            this.name = name;
            if (String.IsNullOrEmpty(passcode))
            {
                this.passcode = name.Split(' ')[0].ToLower();
            }
            else
            {
                this.passcode = passcode;
            }
            this.id = Guid.NewGuid().ToString();
            this.Marked = marked;
        }

        public String Id
        {
            get { return id; }
            set { id = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String Passcode
        {
            get { return passcode; }
            set { passcode = value; }
        }

        public bool Marked
        {
            get { return marked; }
            set { marked = value; }
        }

        public List<String> FeeIds
        {
            get { return feeIds; }
            set { feeIds = value; }
        }

    } 

    public class Game
    {

        private String deletePassword;
        private String id;
        private String wechatName;

       public String Id
        {
            get { return id; }
            set { id = value; }
        }

        public String DeletePassword
        {
            get { return deletePassword; }
            set { deletePassword = value; }
        }
        private String title;

        public String WechatName
        {
            get { return wechatName; }
            set { wechatName = value; }
        }
         public String Title
        {
            get { return title; }
            set { title = value; }
        }
        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }
        private String time;

        public String Time
        {
            get { return time; }
            set { time = value; }
        }
        private String location;

        public String Location
        {
            get { return location; }
            set { location = value; }
        }

        private String message;

        public String Message
        {
            get { return message; }
            set { message = value; }
        }
        private int maxPlayers = 0;

        public int MaxPlayers
        {
            get { return maxPlayers; }
            set { maxPlayers = value; }
        }
        private List<String> players = new List<String>();

        public List<String> Players
        {
            get { return players; }
            set { players = value; }
        }

        private bool isExpenseReportPublished;

        public bool IsExpenseReportPublished
        {
            get { return isExpenseReportPublished; }
            set { isExpenseReportPublished = value; }
        }
        
        private List<String> waitingListIds = new List<string>();

        public Game()
        { }

        public List<String> WaitingListIds
        {
            get { return waitingListIds; }
            set { waitingListIds = value; }
        }

        public Game(DateTime date)
        {
            this.date = date;
            this.id = Guid.NewGuid().ToString();
        }

        private bool publish;

        public bool Publish
        {
            get { return publish; }
            set { publish = value; }
        }

        private List<Fee> fees = new List<Fee>();

        public List<Fee> Fees
        {
            get { return fees; }
            set { fees = value; }
        }
    }

    public class Fee
    {
        private String id;

        public String Id
        {
            get { return id; }
            set { id = value; }
        }

        private String name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        private DateTime date;

        public DateTime Date
        {
            get { return  date; }
            set {  date = value; }
        }
        private String feeTypeId;

        public String FeeTypeId
        {
            get { return feeTypeId; }
            set { feeTypeId = value; }
        }
        private decimal amount;

        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        private List<String> dontShareList = new List<string>();

        public List<String> DontShareList
        {
            get { return dontShareList; }
            set { dontShareList = value; }
        }

        private String paidByPlayerId;

        public String PaidByPlayerId
        {
            get { return paidByPlayerId; }
            set { paidByPlayerId = value; }
        }
    }
}