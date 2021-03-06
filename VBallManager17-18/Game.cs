﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VballManager
{


 

    public class Game
    {
        private DateTime date;
        private VList<Absence> absences = new VList<Absence>();
        private VList<Pickup> pickups = new VList<Pickup>();
        private VList<Waiting> waitingList = new VList<Waiting>();

        public Game()
        { }

        public VList<Waiting> WaitingList
        {
            get { return waitingList; }
            set { waitingList = value; }
        }

        public Game(DateTime date)
        {
            this.date = date;
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }
        public VList<Pickup> Pickups
        {
            get { return pickups; }
            set { pickups = value; }
        }

        public VList<Absence> Absences
        {
            get { return absences; }
            set { absences = value; }
        }
    }

    public class Payment : Identifier
    {
       // private String playerId;
        private String paymentId;
        private DateTime date;
        private DayOfWeek dayOfWeek;
        private Decimal amount;
        private String note;


        public String PaymentId
        {
            get { return paymentId; }
            set { paymentId = value; }
        }

        //public String PlayerId
        //{
       //     get { return playerId; }
      //      set { playerId = value; }
      //  }
        public DayOfWeek DayOfWeek
        {
            get { return dayOfWeek; }
            set { dayOfWeek = value; }
        }

        public String Note
        {
            get { return note; }
            set { note = value; }
        }

        public Decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }
    }

    public enum FeeTypeEnum
    {
        Membership,  Dropin, Prepaid, Credit, Admin, Other
    }

    public class Fee
    {
        public static string FEETYPE_DROPIN = "Dropin fee ({0})";
        public static string FEETYPE_MEMBERSHIP = "Membership fee ({0})";
        public static string FEETYPE_CLUB_MEMBERSHIP = "Club Membership Fee";
        public static string FEETYPE_DROPIN_PRE_PAID = "Dropin pre-paid";

        public Fee()
        {
            this.feeId = Guid.NewGuid().ToString();
        }
        public Fee(decimal amount)
        {
            this.date = DateTime.Today;
            this.amount = amount;
            this.feeId = Guid.NewGuid().ToString();
        }
        public Fee(String feeDesc, decimal amount)
        {
            this.date = DateTime.Today;
            this.amount = amount;
            this.feeDesc = feeDesc;
            this.feeId = Guid.NewGuid().ToString();
        }

        private DateTime date;
        private String feeType;
        private decimal amount;
        private bool isPaid;
        private DateTime payDate;
        private String feeDesc;
        private String feeId;

        public String FeeId
        {
            get { return feeId; }
            set { feeId = value; }
        }

        public String FeeType
        {
            get { return feeType==null? "" : feeType; }
            set { feeType = value; }
        }

        public String FeeDesc
        {
            get { return feeDesc; }
            set { feeDesc = value; }
        }

        public DateTime PayDate
        {
            get { return payDate; }
            set { payDate = value; }
        }

        public bool IsPaid
        {
            get { return isPaid; }
            set { isPaid = value; }
        }

        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }


        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }


    }

    public class Absence : Identifier
    {
        public Absence() { }
        public Absence(String playerId)
        {
            this.playerId = playerId;
         }
        public Absence(String playerId, String transferId)
        {
            this.playerId = playerId;
            this.transferId = transferId;
        }

        //private String playerId;
        private String transferId;

        public String TransferId
        {
            get { return transferId; }
            set { transferId = value; }
        }
    }

    public class Pickup : Waiting
    {
        public Pickup() { }
        private CostReference costReference;
        public Pickup(String playerId, CostReference costReference)
        {
            this.playerId = playerId;
            this.costReference = costReference;
        }


        public CostReference CostReference
        {
            get { return costReference; }
            set { costReference = value; }
        }

    }

    public enum CostType
    {
        FEE, TRANSFER, FREE, CLUB_MEMBER, REACH_MAX, PRE_PAID
    }

    public class CostReference
    {
        public CostReference() { }
        public CostReference(CostType costType, String referenceId)
        {
            this.costType = costType;
            this.referenceId = referenceId;
        }

        private CostType costType;

        public CostType CostType
        {
            get { return costType; }
            set { costType = value; }
        }
        private String referenceId;

        public String ReferenceId
        {
            get { return referenceId; }
            set { referenceId = value; }
        }
    }

    public class Transfer
    {
        public Transfer() { }
        public Transfer(DateTime fromGameDate)
        {
            this.transferId = Guid.NewGuid().ToString();
            this.fromGameDate = fromGameDate;
        }

        private String transferId;
        private DateTime fromGameDate;
        private bool isUsed;
        private DateTime applyGameDate;

        public String TransferId
        {
            get { return transferId; }
            set { transferId = value; }
        }

        public DateTime FromGameDate
        {
            get { return fromGameDate; }
            set { fromGameDate = value; }
        }

        public bool IsUsed
        {
            get { return isUsed; }
            set { isUsed = value; }
        }

        public DateTime ApplyGameDate
        {
            get { return applyGameDate; }
            set { applyGameDate = value; }
        }
    }

    public class Identifier
    {
        public Identifier() { }
        protected String playerId;
        public String PlayerId
        {
            get { return playerId; }
            set { playerId = value; }
        }
    }

    public class Waiting : Identifier
    {
        public Waiting() { }

        public Waiting(String playerId)
        {
            this.playerId = playerId;
        }

        private String operatorId;
        public String OperatorId
        {
            get { return operatorId; }
            set { operatorId = value; }
        }
    }

    public class VList<T> where T : Identifier
    {
        private List<T> items = new List<T>();

        public List<T> Items
        {
            get { return items; }
            set { items = value; }
        }

        public bool Exists(String playerId)
        {
            return this.items.Exists(iden => iden.PlayerId == playerId);
        }

        public T FindByPlayerId(String playerId)
        {
            T iden = this.items.Find(
             delegate(T id)
             {
                 return id.PlayerId == playerId;
             }
             );
            return iden;
        }

        public List<String> PlayerIds()
        {
            List<String> playerIds = new List<string>();
            foreach (T id in this.items)
            {
                playerIds.Add(id.PlayerId);
            }
            return playerIds;
        }

        public void Add(T iden)
        {
            this.items.Add(iden);
        }

        public void Remove(String playerId)
        {
            T iden = this.items.Find(
             delegate(T id)
             {
                 return id.PlayerId == playerId;
             }
            );
            this.items.Remove(iden);
        }

        public void Remove(T iden)
        {
            this.items.Remove(iden);
        }

        public T this[int i]
        {
            get { return (T)this.items[i]; }
            set { this.items[i] = value; }
        }

        public int Count
        {
            get { return this.items.Count; }
        }

        public void Clear()
        {
            this.items.Clear();
        }

    }
}