using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VballManager
{
    public class LogHistory
    {
        public LogHistory()
        {
        }

        public LogHistory(DateTime date, String userInfo, String poolName, String playerName, String type, String operatorName)
        {
            this.date = date;
            this.userInfo = userInfo;
            this.poolName = poolName;
            this.playerName = playerName;
            this.type = type;
            this.operatorName = operatorName;
        }

        private String operatorName;

        public String OperatorName
        {
            get { return operatorName; }
            set { operatorName = value; }
        }

        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }
        private String type;

        public String Type
        {
            get { return type; }
            set { type = value; }
        }
        private String poolName;

        public String PoolName
        {
            get { return poolName; }
            set { poolName = value; }
        }
        private String playerName;

        public String PlayerName
        {
            get { return playerName; }
            set { playerName = value; }
        }
        private String userInfo;

        public String UserInfo
        {
            get { return userInfo; }
            set { userInfo = value; }
        }
    }
}