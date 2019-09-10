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
        private bool[] surveys;
        private int transfers = 0;
        private int transferUsed = 0;
        private bool suspend = false;
        private List<Fee> fees = new List<Fee>();
        private int freeDropin;

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

        public int Transfers
        {
            get { return transfers; }
            set { transfers = value; }
        }

        public int TransferUsed
        {
            get { return transferUsed; }
            set { transferUsed = value; }
        }

        public bool[] Surveys
        {
            get
            {
                if (surveys == null)
                {
                    return new bool[4];
                }
                return surveys;
            }
            set { surveys = value; }
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
            this.surveys = new bool[4];
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
    }
    public class Dropin : Attendee
    {
        private bool isCoop;

        public Dropin() { }

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

    public class Game
    {
        private DateTime date;
        private List<String> absenceIds = new List<String>();
        private List<String> dropinIds = new List<String>();
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
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }
        public List<String> DropinIds
        {
            get { return dropinIds; }
            set { dropinIds = value; }
        }

        public List<String> AbsenceIds
        {
            get { return absenceIds; }
            set { absenceIds = value; }
        }
    }

    public class Payment
    {
        private String playerId;
        private String paymentId;
        private DateTime date;
        private DayOfWeek dayOfWeek;
        private int amount;
        private String note;
 
 
        public String PaymentId
        {
            get { return paymentId; }
            set { paymentId = value; }
        }

        public String PlayerId
        {
            get { return playerId; }
            set { playerId = value; }
        }
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

        public int Amount
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

    public class Fee
    {
        public static string FEETYPE_DROPIN = "Dropin fee ({0})";
        public static string FEETYPE_MEMBERSHIP = "Membership fee ({0})";
        
        public Fee()
        {
            this.feeId = Guid.NewGuid().ToString();
        }
        public Fee(int amount)
        {
            this.date = DateTime.Today;
            this.amount = amount;
            this.feeId = Guid.NewGuid().ToString();
        }
        public Fee(String feeType, int amount)
        {
            this.date = DateTime.Today;
            this.amount = amount;
            this.feeType = feeType;
            this.feeId = Guid.NewGuid().ToString();
        }

        private DateTime date;
        private int amount;
        private bool isPaid;
        private DateTime payDate;
        private String feeType;
        private String feeId;

        public String FeeId
        {
            get { return feeId; }
            set { feeId = value; }
        }

        public String FeeType
        {
            get { return feeType; }
            set { feeType = value; }
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

        public int Amount
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

}