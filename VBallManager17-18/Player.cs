﻿using System;
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
        private bool suspend = false;
        private List<Fee> fees = new List<Fee>();
        private decimal prePaidBalance;
        private int freeDropin;
        private bool isRegisterdMember;
        private List<String> authorizedUsers = new List<string>();
        private bool deviceLinked;
        private List<Notification> notifications = new List<Notification>();
        public int MondayPlayedCount;
        public int FridayPlayedCount;
        public int TotalPlayedCount;

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

        public bool Suspend
        {
            get { return suspend; }
            set { suspend = value; }
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

    public class Attendee
    {
        private String id;
        private bool isSuspended;

        public Attendee() { }

        public Attendee(String id)
        {
            this.id = id;
        }

        public String Id
        {
            get { return id; }
            set { id = value; }
        }

        public bool IsSuspended
        {
            get { return isSuspended; }
            set { isSuspended = value; }
        }
        private bool preRegistered = false;

        public bool PreRegistered
        {
            get { return preRegistered; }
            set { preRegistered = value; }
        }
        private int playedCount;

        public int PlayedCount
        {
            get { return playedCount; }
            set { playedCount = value; }
        }

    }
    public class Dropin : Attendee
    {
        private bool isCoop;
        private DateTime lastCoopDate;

        public Dropin() { }

        public DateTime LastCoopDate
        {
            get { return lastCoopDate; }
            set { lastCoopDate = value; }
        }


        public Dropin(String id)
        {
            this.Id = id;
        }

        public bool IsCoop
        {
            get { return isCoop; }
            set { isCoop = value; }
        }

        public void DropinSuspend()
        {
            this.IsSuspended = true;
        }

        public void ResumeSuspend()
        {
            this.IsSuspended = false;
        }
    }

    public class Member : Attendee
    {
        private DateTime joinDate;
        private DateTime cancelDate;
        private bool isCancelled;

        public Member() { }
        public Member(String id)
        {
            this.Id = id;
            this.joinDate = DateTime.Today;
        }

        public void CancelMember()
        {
            this.isCancelled = true;
            this.cancelDate = DateTime.Today;
        }
        public void SuspendMember()
        {
            this.IsSuspended = true;
        }
        public void ResumeMember()
        {
            this.isCancelled = false;
            this.IsSuspended = false;
            this.cancelDate = DateTime.MinValue;
        }

        public DateTime JoinDate
        {
            get { return joinDate; }
            set { joinDate = value; }
        }

        public DateTime CancelDate
        {
            get { return cancelDate; }
            set { cancelDate = value; }
        }

        public bool IsCancelled
        {
            get { return isCancelled; }
            set { isCancelled = value; }
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
}