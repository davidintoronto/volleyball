using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VballManager
{
    public class Player
    {
        private String id;
        private String name;
        private String passcode;
        private bool marked;
        private List<Transfer> transfers = new List<Transfer>();
        private bool isActive = true;
        private List<Fee> fees = new List<Fee>();
        private decimal prePaidBalance;
        private int freeDropin;
        private bool isRegisterdMember;
        private List<String> authorizedUsers = new List<string>();
        private bool deviceLinked;
        private int role = (int)Roles.Player;
        private String wechatName;
        private List<Notification> notifications = new List<Notification>();
        public int MondayPlayedCount;
        public int FridayPlayedCount;
        public int TotalPlayedCount;
  
        public String WechatName
        {
            get { return wechatName; }
            set { wechatName = value; }
        }
      public List<Notification> Notifications
        {
            get { return notifications; }
            set { notifications = value; }
        }

        public bool DeviceLinked
        {
            get { return deviceLinked; }
            set { deviceLinked = value; }
        }

        public int Role
        {
            get { return role; }
            set { role = value; }
        }

        public List<String> AuthorizedUsers
        {
            get { return authorizedUsers; }
            set { authorizedUsers = value; }
        }

        public bool IsRegisterdMember
        {
            get { return isRegisterdMember; }
            set { isRegisterdMember = value; }
        }
        public int FreeDropin
        {
            get { return freeDropin; }
            set { freeDropin = value; }
        }

        public List<Fee> Fees
        {
            get { return fees; }
            set { fees = value; }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public List<Transfer> Transfers
        {
            get { return transfers; }
            set { transfers = value; }
        }

        public decimal PrePaidBalance
        {
            get { return prePaidBalance; }
            set { prePaidBalance = value; }
        }

        public int TransferUsed
        {
            get
            {
                return this.transfers.Count(transfer => transfer.IsUsed);
            }
        }

        public int AvailableTransferCount
        {
            get
            {
                return this.transfers.Count(transfer => !transfer.IsUsed);
            }
        }

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

        //Find fee by id
        public Fee FindFeeById(String feeId)
        {
            return this.Fees.Find(
                delegate(Fee fee)
                {
                    return fee.FeeId == feeId;
                }
            );
        }

        public Transfer FindTransferByGameDate(DateTime gameDate)
        {
            return this.transfers.Find(
                delegate(Transfer transfer)
                {
                    return transfer.FromGameDate == gameDate;
                }
            );

        }

        public Transfer FindTransferById(String transferId)
        {
            return this.transfers.Find(
                delegate(Transfer transfer)
                {
                    return transfer.TransferId == transferId;
                }
            );

        }

        public bool RemoveTransferByGameDate(DateTime gameDate)
        {
            Transfer trans = this.transfers.Find(
                delegate(Transfer transfer)
                {
                    return transfer.FromGameDate == gameDate;
                }
            );
            if (trans != null)
            {
                this.transfers.Remove(trans);
                return true;
            }
            return false;
        }

        public Transfer GetAvailableTransfer(DateTime gameDate)
        {
            foreach (Transfer transfer in this.transfers)
            {
                if (!transfer.IsUsed && transfer.FromGameDate < gameDate)
                {
                    return transfer;
                }
            }
            return null;
        }
    }

    public class Person : Identifier
    {
        public Person() { }

        public Person(String playerId)
        {
            this.playerId = playerId;
        }

        private bool preRegistered = false;

        public bool PreRegistered
        {
            get { return preRegistered; }
            set { preRegistered = value; }
        }
        private int playedCount ;

        public int PlayedCount
        {
            get { return playedCount; }
            set { playedCount = value; }
        }

    }
    public class Dropin : Person
    {
        private bool isCoop;

        public Dropin() { }

        public Dropin(String id)
        {
            this.PlayerId = id;
        }

        public bool IsCoop
        {
            get { return isCoop; }
            set { isCoop = value; }
        }
    }

    public class Member : Person
    {
        private DateTime joinDate;

        public Member() { }
        public Member(String id)
        {
            this.PlayerId = id;
            this.joinDate = DateTime.Today;
        }

        public DateTime JoinDate
        {
            get { return joinDate; }
            set { joinDate = value; }
        }

       }

    public class Notification
    {
        public Notification() { }
        public Notification(DateTime date, String text)
        {
            this.date = date;
            this.text = text;
        }

        private DateTime date;

        public DateTime Date
        {
          get { return date; }
          set { date = value; }
        }
                private String text;

        public String Text
        {
          get { return text; }
          set { text = value; }
        }
    }

    public enum Roles
    {
        SuperAdmin=9, Admin=8, Manager=6, Captain=4, Player=2, User=0
    }
}