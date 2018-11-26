using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VballManager
{
    public class Factor
    {
        private String id;
        private String poolName;
        private String lowPoolName;
        private int lowPoolNumberFrom;
        private int lowPoolNumberTo;
        private int coopNumberFrom;
        private int coopNumberTo;
        private String highPoolName;
        private int highPoolNumberFrom;
        private int highPoolNumberTo;
        private decimal value;

        public Factor() { }

        public Factor(String poolName, String lowPoolName, int lowPoolNumberFrom, int lowPoolNumberTo, //
            int coopNumberFrom, int coopNumberTo, String highPoolName, int highPoolNumberFrom, int highPoolNumberTo, decimal value)
        {
            this.id = Guid.NewGuid().ToString();
            this.poolName = poolName;
            this.lowPoolName = lowPoolName;
            this.lowPoolNumberFrom = lowPoolNumberFrom;
            this.lowPoolNumberTo = lowPoolNumberTo;
            this.coopNumberFrom = coopNumberFrom;
            this.coopNumberTo = coopNumberTo;
            this.highPoolName = highPoolName;
            this.highPoolNumberFrom = highPoolNumberFrom;
            this.highPoolNumberTo = highPoolNumberTo;
            this.value = value;
        }

        public String Id
        {
            get { return id; }
            set { id = value; }
        }

        public String PoolName
        {
            get { return poolName; }
            set { poolName = value; }
        }

        public String LowPoolName
        {
            get { return lowPoolName; }
            set { lowPoolName = value; }
        }

        public int LowPoolNumberFrom
        {
            get { return lowPoolNumberFrom; }
            set { lowPoolNumberFrom = value; }
        }

        public int LowPoolNumberTo
        {
            get { return lowPoolNumberTo; }
            set { lowPoolNumberTo = value; }
        }

        public int CoopNumberFrom
        {
            get { return coopNumberFrom; }
            set { coopNumberFrom = value; }
        }

        public int CoopNumberTo
        {
            get { return coopNumberTo; }
            set { coopNumberTo = value; }
        }

        public String HighPoolName
        {
            get { return highPoolName; }
            set { highPoolName = value; }
        }

        public int HighPoolNumberFrom
        {
            get { return highPoolNumberFrom; }
            set { highPoolNumberFrom = value; }
        }

        public int HighPoolNumberTo
        {
            get { return highPoolNumberTo; }
            set { highPoolNumberTo = value; }
        }

        public decimal Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }

    public class MoveRule
    {
        private String id;
        private int hour;
        private String lowPoolName;
        private int lowPoolNumberFrom;
        private int lowPoolNumberTo;
        private int coopNumberFrom;
        private int coopNumberTo;
        private int lowPoolWaiting;
        private String highPoolName;
        private int highPoolNumberFrom;
        private int highPoolNumberTo;
        private int highPoolWaiting;
        private int toMove;

        public MoveRule() { }

        public MoveRule(int hour, String lowPoolName, int lowPoolNumberFrom, int lowPoolNumberTo, int lowPoolWaiting, //
            int coopNumberFrom, int coopNumberTo, String highPoolName, int highPoolNumberFrom, int highPoolNumberTo, int highPoolWaiting, int toMove)
        {
            this.id = Guid.NewGuid().ToString();
            this.hour = hour;
            this.lowPoolName = lowPoolName;
            this.lowPoolNumberFrom = lowPoolNumberFrom;
            this.lowPoolNumberTo = lowPoolNumberTo;
            this.lowPoolWaiting = lowPoolWaiting;
            this.coopNumberFrom = coopNumberFrom;
            this.coopNumberTo = coopNumberTo;
            this.highPoolName = highPoolName;
            this.highPoolNumberFrom = highPoolNumberFrom;
            this.highPoolNumberTo = highPoolNumberTo;
            this.highPoolWaiting = highPoolWaiting;
            this.toMove = toMove;
        }

        public String Id
        {
            get { return id; }
            set { id = value; }
        }

        public int Hour
        {
            get { return hour; }
            set { hour = value; }
        }

        public int HighPoolWaiting
        {
            get { return highPoolWaiting; }
            set { highPoolWaiting = value; }
        }

        public int LowPoolWaiting
        {
            get { return lowPoolWaiting; }
            set { lowPoolWaiting = value; }
        }

        public String LowPoolName
        {
            get { return lowPoolName; }
            set { lowPoolName = value; }
        }

        public int LowPoolNumberFrom
        {
            get { return lowPoolNumberFrom; }
            set { lowPoolNumberFrom = value; }
        }

        public int LowPoolNumberTo
        {
            get { return lowPoolNumberTo; }
            set { lowPoolNumberTo = value; }
        }

        public int CoopNumberFrom
        {
            get { return coopNumberFrom; }
            set { coopNumberFrom = value; }
        }

        public int CoopNumberTo
        {
            get { return coopNumberTo; }
            set { coopNumberTo = value; }
        }

        public String HighPoolName
        {
            get { return highPoolName; }
            set { highPoolName = value; }
        }

        public int HighPoolNumberFrom
        {
            get { return highPoolNumberFrom; }
            set { highPoolNumberFrom = value; }
        }

        public int HighPoolNumberTo
        {
            get { return highPoolNumberTo; }
            set { highPoolNumberTo = value; }
        }

        public int ToMove
        {
            get { return this.toMove; }
            set { this.toMove = value; }
        }
    }


}
