using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VballManager
{

    public class VolleyballClub
    {
        private String superAdmin = "adm!n";
        private String adminEmail;
        private int timezoneOffset = 0;
        private int maxTransfers = 10;
        private List<Player> players = new List<Player>();
        private List<Pool> pools = new List<Pool>();
        private int dropinSpotOpeningHour = 8;
        private int dropinFee = 5;
        private List<LogHistory> logs = new List<LogHistory>();
        private int lockReservationHour = 20;
        // private int coopReserveHour = 15;
        private List<Payment> payments = new List<Payment>();
        private String readme;
        private GameScore score = new GameScore();
        private bool clubMemberMode = true;
        private bool isDropinFeeWithCap = false;
        private int registerMembershipFee;
        private DateTime cookieExpire = DateTime.Parse("07/01/2018");
        private bool cookieAuthRequired = false;
        private String timeZoneName = "Eastern Standard Time";
        private List<Permit> permits = new List<Permit>();
        private List<WechatMessage> wechatMessages = new List<WechatMessage>();
        private int maxDropinFeeOwe = 20;

        public int MaxDropinFeeOwe
        {
            get { return maxDropinFeeOwe; }
            set { maxDropinFeeOwe = value; }
        }

        public List<WechatMessage> WechatMessages
        {
            get { return wechatMessages; }
            set { wechatMessages = value; }
        }

        public String AdminEmail
        {
            get { return adminEmail; }
            set { adminEmail = value; }
        }

        public List<Permit> Permits
        {
            get { return permits; }
            set { permits = value; }
        }

        public String TimeZoneName
        {
            get { return timeZoneName; }
            set { timeZoneName = value; }
        }

        public bool CookieAuthRequired
        {
            get { return cookieAuthRequired; }
            set { cookieAuthRequired = value; }
        }

        public DateTime CookieExpire
        {
            get { return cookieExpire; }
            set { cookieExpire = value; }
        }

        public int RegisterMembeshipFee
        {
            get { return registerMembershipFee; }
            set { registerMembershipFee = value; }
        }

        public bool IsDropinFeeWithCap
        {
            get { return isDropinFeeWithCap; }
            set { isDropinFeeWithCap = value; }
        }

        public bool ClubMemberMode
        {
            get { return clubMemberMode; }
            set { clubMemberMode = value; }
        }

        public GameScore GameScores
        {
            get { return score; }
            set { score = value; }
        }

        public String Readme
        {
            get { return readme; }
            set { readme = value; }
        }

        public List<Payment> GetPayments()
        {
            return payments;
        }

        public List<Payment> Payments
        {
            get { return payments; }
            set { payments = value; }
        }

        public int LockReservationHour
        {
            get { return lockReservationHour; }
            set { lockReservationHour = value; }
        }

        public List<LogHistory> Logs
        {
            get { return logs; }
            set { logs = value; }
        }

        public int DropinFee
        {
            get { return dropinFee; }
            set { dropinFee = value; }
        }

        public List<Pool> Pools
        {
            get { return pools; }
            set { pools = value; }
        }

        public int DropinSpotOpeningHour
        {
            get { return dropinSpotOpeningHour; }
            set { dropinSpotOpeningHour = value; }
        }


        public int MaxTransfers
        {
            get { return maxTransfers; }
            set { maxTransfers = value; }
        }
        private bool passcodeAuthen;

        public List<Player> Players
        {
            get { return players; }
            set { players = value; }
        }

        public int TimezoneOffset
        {
            get { return timezoneOffset; }
            set { timezoneOffset = value; }
        }
        public bool PasscodeAuthen
        {
            get { return passcodeAuthen; }
            set { passcodeAuthen = value; }
        }

        public String SuperAdmin
        {
            get { return superAdmin; }
            set { superAdmin = value; }
        }

        //Find or create new player by name
        public Player FindOrCreateNewPlayer(String name)
        {
            Player player = FindPlayerByName(name);
            if (player == null)
            {
                player = new Player(name, null, false);
            }
            return player;
        }

        //Find payment by id
        public Payment FindPaymentById(String paymentId)
        {
            return this.payments.Find(
                delegate(Payment payment)
                {
                    return payment.PaymentId == paymentId;
                }
            );
        }

        //Find player by id
        public Player FindPlayerById(String id)
        {
            return this.players.Find(
                delegate(Player player)
                {
                    return player.Id == id;
                }
            );
        }
        //Find player by name
        public Player FindPlayerByName(String name)
        {
            return this.players.Find(
                delegate(Player player)
                {
                    return player.Name == name;
                }
            );
        }
        //Find pool by name
        public Pool FindPoolByName(String name)
        {
            return this.Pools.Find(
                delegate(Pool pool)
                {
                    return pool.Name == name;
                }
            );
        }
        //Find active players
        public List<Player> ActivePlayers
        {
            get
            {
                List<Player> activePlayers = new List<Player>();
                foreach (Player player in Players)
                {
                    if (!player.IsActive) continue;
                    foreach (Pool pool in Pools)
                    {
                        if (pool.Members.Exists(attendee => attendee.Id == player.Id) || pool.Dropins.Exists(attendee => attendee.Id == player.Id))
                        {
                            activePlayers.Add(player);
                            break;
                        }
                    }
                }
                return activePlayers;
            }
        }
        //Find pool by id
        public Pool FindPoolById(String id)
        {
            return this.Pools.Find(
                delegate(Pool pool)
                {
                    return pool.Id == id;
                }
            );
        }
        //Find player list with given Ids
        public List<Player> FindPlayersByIds(List<String> ids)
        {
            List<Player> poolPlayers = new List<Player>();
            foreach (String id in ids)
            {
                Player player = FindPlayerById(id);
                if (player != null)
                {
                    poolPlayers.Add(player);
                }
            }
            return poolPlayers;
        }
        //Find player list with given members
        public List<Player> FindPlayersByAttendees(List<Member> attendees)
        {
            List<Player> poolPlayers = new List<Player>();
            foreach (Attendee attendee in attendees)
            {
                Player player = FindPlayerById(attendee.Id);
                if (player != null)
                {
                    poolPlayers.Add(player);
                }
            }
            return poolPlayers;
        }
        //Find player list with given dropins
        public List<Player> FindPlayersByAttendees(List<Dropin> attendees)
        {
            List<Player> poolPlayers = new List<Player>();
            foreach (Attendee attendee in attendees)
            {
                Player player = FindPlayerById(attendee.Id);
                poolPlayers.Add(player);
            }
            return poolPlayers;
        }

        //Player exists
        public bool PlayerNameExists(String name)
        {
            Player existPlayer = players.Find(
               delegate(Player player)
               {
                   return player.Name == name;
               }
           );
            return existPlayer != null;
        }
        //Delete player
        public void DeletePlayer(String id)
        {
            Player player = FindPlayerById(id);
            foreach (Pool pool in pools)
            {
                pool.RemoveDropin(id);
                pool.RemoveMember(id);
            }
            players.Remove(player);
        }
        public String ReversedId(String id)
        {
            String reversedId = id.Replace('0', 'Z');
            reversedId = reversedId.Replace('9', '0');
            reversedId = reversedId.Replace('Z', '9');
            reversedId = reversedId.Replace('1', 'Z');
            reversedId = reversedId.Replace('8', '1');
            reversedId = reversedId.Replace('Z', '8');
            reversedId = reversedId.Replace('2', 'Z');
            reversedId = reversedId.Replace('7', '2');
            reversedId = reversedId.Replace('Z', '7');
            reversedId = reversedId.Replace('3', 'Z');
            reversedId = reversedId.Replace('6', '3');
            reversedId = reversedId.Replace('Z', '6');
            reversedId = reversedId.Replace('4', 'Z');
            reversedId = reversedId.Replace('5', '4');
            reversedId = reversedId.Replace('Z', '5');
            return reversedId;
        }

        public bool ActionPermitted(Actions action, int role)
        {
            Permit featurePermit = Permits.Find(permit => permit.Action == action);
            if (featurePermit != null && featurePermit.Role <= role) return true;
            return false;
        }

        public void AddNotifyWechatMessage(Player player, String message)
        {
            if (!String.IsNullOrEmpty(player.WechatName))
            {
                WechatMessage wechat = new WechatMessage();
                wechat.Date = DateTime.Today;
                wechat.Message = player.WechatName + "|" + message;
                WechatMessages.Add(wechat);
            }
        }
        public void AddNotifyWechatMessage(Pool pool, String message)
        {
            if (!String.IsNullOrEmpty(pool.WechatGroupName))
            {
                WechatMessage wechat = new WechatMessage();
                wechat.Date = DateTime.Today;
                wechat.Message = pool.WechatGroupName + "|" + message;
                WechatMessages.Add(wechat);
            }
        }

        public void AddReservationNotifyWechatMessage(String playerId, String operatorId, String result, Pool pool, Pool originalPool, DateTime gameDate)
        {
            Player player = FindPlayerById(playerId);
            Player user = operatorId == null ? null : FindPlayerById(operatorId);
            WechatMessage message = new WechatMessage();
            message.Date = DateTime.Today.Date;
            String text = null;
            if (result == Constants.RESERVED || result == Constants.CANCELLED)
            {
                if (playerId == operatorId)
                {
                    text = WechatAt(player) + "You " + result.ToString();
                }
                else
                {
                    text = WechatAt(player) + (user == null ? "Admin" : user.Name) + " " + result.ToString() + " for you";
                }
            }
            else if (result == Constants.WAITING_TO_RESERVED)
            {
                text = result.ToString() + " in pool " + pool.Name + GetForDate(gameDate);
                this.AddNotifyWechatMessage(player, text);
                text = WechatAt(player) + result.ToString();
            }
            else if (result == Constants.MOVED)
            {
                text = WechatAt(player) + result.ToString() + " " + pool.Name;
            }
            else if (result == Constants.WAITING && playerId != operatorId)
            {
                text = WechatAt(player) + (user == null ? "Admin" : user.Name) + " " + result.ToString();
            }

            message.Message = originalPool.WechatGroupName + "|" + text + " in pool " + pool.Name + GetForDate(gameDate);

            this.WechatMessages.Add(message);
        }

        private String WechatAt(Player player)
        {
            if (String.IsNullOrEmpty(player.WechatName))
            {
                return player.Name + ", ";
            }

            return "@" + player.WechatName + "{ENTER} ";
        }

        private String GetForDate(DateTime gameDate)
        {
            if (gameDate.Date == DateTime.Today.Date) return " for the volleyball tonight.";
            return " for the volleyball on " + gameDate.ToString("ddd, MMM d, yyyy");
        }
    }

    public class Pool
    {
        private String id;
        private String name;
        private String title;
        private String wechatGroupName;
        private String messageBoard;
        private int maximumPlayerNumber = 14;
        private String scheduledGameTime;
        private bool allowAddNewDropinName;
        private int membershipFee;
        private int daysToReserve4Memeber = 0;
        private int daysToReserve = 0;
        // private List<String> members = new List<String>();
        // private List<String> dropins = new List<String>();
        private List<Member> members = new List<Member>();
        private List<Dropin> dropins = new List<Dropin>();
        private String wildcardPlayer;

        private List<Game> games = new List<Game>();
        private DayOfWeek dayOfWeek;
        private int reservHourForCoop = 12;
        private int lessThanPayersForCoop = 13;
        private bool autoCoopReserve = false;
        private int maxCoopPlayers = 1;
        private String statsType = "None";

        public String StatsType
        {
            get { return statsType; }
            set { statsType = value; }
        }

        public String WechatGroupName
        {
            get { return wechatGroupName; }
            set { wechatGroupName = value; }
        }

        public int MaxCoopPlayers
        {
            get { return maxCoopPlayers; }
            set { maxCoopPlayers = value; }
        }

        public bool AutoCoopReserve
        {
            get { return autoCoopReserve; }
            set { autoCoopReserve = value; }
        }

        //----------------------------------
        public String Id
        {
            get { return id; }
            set { id = value; }
        }


        public int DaysToReserve4Member
        {
            get { return daysToReserve4Memeber; }
            set { daysToReserve4Memeber = value; }
        }

        public int DaysToReserve
        {
            get { return daysToReserve; }
            set { daysToReserve = value; }
        }

        public int ReservHourForCoop
        {
            get { return reservHourForCoop; }
            set { reservHourForCoop = value; }
        }

        public int LessThanPayersForCoop
        {
            get { return lessThanPayersForCoop; }
            set { lessThanPayersForCoop = value; }
        }

        public DayOfWeek DayOfWeek
        {
            get { return dayOfWeek; }
            set { dayOfWeek = value; }
        }


        public int MembershipFee
        {
            get { return membershipFee; }
            set { membershipFee = value; }
        }


        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        public String WildcardPlayer
        {
            get { return wildcardPlayer; }
            set { wildcardPlayer = value; }
        }


        public String MessageBoard
        {
            get { return messageBoard; }
            set { messageBoard = value; }
        }
        public String GameScheduleTime
        {
            get { return scheduledGameTime; }
            set { scheduledGameTime = value; }
        }

        public List<Dropin> Dropins
        {
            get { return dropins; }
            set { dropins = value; }
        }

        public String Title
        {
            get { return title; }
            set { title = value; }
        }

        public bool AllowAddNewDropinName
        {
            get { return allowAddNewDropinName; }
            set { allowAddNewDropinName = value; }
        }


        public int MaximumPlayerNumber
        {
            get { return maximumPlayerNumber; }
            set { maximumPlayerNumber = value; }
        }

        public bool HasCap
        {
            get { return maximumPlayerNumber > 0; }

        }


        public List<Member> Members
        {
            get { return members; }
            set { members = value; }
        }

        public List<Game> Games
        {
            get { return games; }
            set { games = value; }
        }


        //Find game by date
        public Game FindGameByDate(DateTime date)
        {
            return this.games.Find(
                delegate(Game game)
                {
                    return game.Date.ToShortDateString() == date.ToShortDateString();
                }
            );
        }
        //Find dropin by name
        public bool IsDropinReserved(DateTime date, String id)
        {
            Game game = FindGameByDate(date);
            return game.Pickups.Exists(id);
        }

        //Get number of members who are in for the date
        public int GetNumberOfAttendingMembers(DateTime date)
        {
            Game game = FindGameByDate(date);
            return GetNumberOfAvaliableMembers() - game.Absences.Count;
        }

        //Get number of members who are not cancelled or suspended.
        public int GetNumberOfAvaliableMembers()
        {
            int count = members.Count;
            foreach (Member member in Members)
            {
                if (member.IsCancelled)// || member.IsSuspended)
                {
                    count--;
                }
            }
            return count;
        }

        public int GetNumberOfOnHoldMemberSpots(DateTime date)
        {
            Game game = FindGameByDate(date);
            return game.Absences.Count;
        }

        public int GetNumberOfAvailableDropinSpots(DateTime date)
        {
            Game game = FindGameByDate(date);
            int availableSpot = MaximumPlayerNumber - (members.Count - game.Absences.Count);
            return availableSpot > 0 ? availableSpot : 0;
        }

        //Get number of dropin for the date
        public int GetNumberOfDropins(DateTime date)
        {
            Game game = FindGameByDate(date);
            return game.Pickups.Count;
        }

        //Get number of coop for the date
        public int GetNumberOfReservedCoops(DateTime date)
        {
            Game game = FindGameByDate(date);
            int coop = 0;
            foreach (Pickup pickup in game.Pickups.Items)
            {
                Dropin dropin = this.Dropins.Find(drop_in => drop_in.Id == pickup.PlayerId);
                if (dropin != null && dropin.IsCoop) coop++;
            }
            return coop;
        }

        public void AddDropin(Player player)
        {
            Dropin dropin = new Dropin(player.Id);
            Dropins.Add(dropin);
        }

        public void RemoveMember(String id)
        {
            if (Members.Exists(member => member.Id == id))
            {
                Members.Remove(Members.Find(member => member.Id == id));
            }
        }
        public void RemoveDropin(String id)
        {
            if (Dropins.Exists(dropin => dropin.Id == id))
            {
                Dropins.Remove(Dropins.Find(dropin => dropin.Id == id));
            }
        }

        //Remove game
        public void DeleteGame(DateTime date)
        {
            Game game = FindGameByDate(date);
            if (game != null)
            {
                games.Remove(game);
            }
        }

        //Game exists
        public bool GameExists(DateTime date)
        {
            return FindGameByDate(date) != null;
        }

        //Get member attendeance on a game
        public bool GetMemberAttendance(String playerId, DateTime date)
        {
            Game game = FindGameByDate(date);
            return !game.Absences.Exists(playerId);
        }

        //Reverse member attendance
        public bool ReverseMemberAttendance(String id, DateTime date, String transferId)
        {
            Game game = FindGameByDate(date);
            Absence absence = (Absence)game.Absences.FindByPlayerId(id);
            if (absence != null)
            {
                game.Absences.Remove(absence);
                return true;
            }
            absence = new Absence(id, transferId);
            game.Absences.Add(absence);
            return false;
        }
    }

    public class Permit
    {
        private Actions action;

        public Actions Action
        {
            get { return action; }
            set { action = value; }
        }
        private int role;

        public int Role
        {
            get { return role; }
            set { role = value; }
        }
    }

    public enum Actions
    {
        View_All_Pools, View_Past_Games, Reserve_All_Pools, Reserve_Pool, Power_Reserve, Admin_Management
    }

    public enum StatsTypes
    {
        None, Day, Week
    }

    public class WechatMessage
    {
        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        private String message;

        public String Message
        {
            get { return message; }
            set { message = value; }
        }
    }
}