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
        private int lockWaitingListHour = 18;
        // private int coopReserveHour = 15;
        private List<Payment> payments = new List<Payment>();
        private String readme;
        private GameScore score = new GameScore();
        private bool clubMemberMode = true;
        private bool isDropinFeeWithCap = false;
        private int registerMembershipFee;
        private DateTime cookieExpire = DateTime.Parse("07/01/2018");
        private bool cookieAuthRequired = true;
        private String timeZoneName = "Eastern Standard Time";
        private List<Permit> permits = new List<Permit>();
        private int maxDropinFeeOwe = 20;
        private WechatNotify wechatNotifier = new WechatNotify();

 
       public int MaxDropinFeeOwe
        {
            get { return maxDropinFeeOwe; }
            set { maxDropinFeeOwe = value; }
        }

        public WechatNotify WechatNotifier
        {
            get { return wechatNotifier; }
            set { wechatNotifier = value; }
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

        public int LockWaitingListHour
        {
            get { return lockWaitingListHour; }
            set { lockWaitingListHour = value; }
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
                        if (pool.AllPlayers.Exists(player.Id))
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
            foreach (Person attendee in attendees)
            {
                Player player = FindPlayerById(attendee.PlayerId);
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
            foreach (Person attendee in attendees)
            {
                Player player = FindPlayerById(attendee.PlayerId);
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
                foreach (Game game in pool.Games)
                {
                    game.Members.Remove(player.Id);
                    game.Dropins.Remove(player.Id);
                    game.WaitingList.Remove(player.Id);
                }
            }
            players.Remove(player);
        }

        public Game FindComingGame(Pool pool)
        {
            return pool.Games.OrderBy(game => game.Date).ToList<Game>().Find(game => game.Date >= EastDateTimeToday);
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

 
        public DateTime EastDateTimeToday
        {
            get
            {
                TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneName);
                return TimeZoneInfo.ConvertTime(DateTime.Today, easternZone).Date;
            }
        }

        public DateTime EastDateTimeNow
        {
            get
            {
                TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneName);
                return TimeZoneInfo.ConvertTime(DateTime.UtcNow, easternZone);
            }
        }

        public bool IsReservationLocked(DateTime gameDate)
        {
            DateTime lockDate = TimeZoneInfo.ConvertTimeFromUtc(gameDate, TimeZoneInfo.FindSystemTimeZoneById(TimeZoneName));
            lockDate = lockDate.AddHours(-1 * lockDate.Hour + LockReservationHour);
            DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(TimeZoneName));
            return now >= lockDate;
        }


       public void AddReservationNotifyWechatMessage(String playerId, String operatorId, String result, Pool targetPool, Pool originalPool, DateTime gameDate)
        {
           if (IsReservationLocked(gameDate)) return;
            Player player = FindPlayerById(playerId);
            Player user = operatorId == null ? null : FindPlayerById(operatorId);
            String message = null;
            int numberOfReservedPlayerInTargetPool = targetPool.FindGameByDate(gameDate).NumberOfReservedPlayers;

            String poolAndGameDate = " in pool " + targetPool.Name + " for the " + targetPool.DayOfWeek + " volleyball on " + gameDate.ToString("MM/dd/yyyy") +".";
            String playerNumber = " Total player number in pool " + targetPool.Name + ": " + numberOfReservedPlayerInTargetPool;
            if (result == Constants.RESERVED || result == Constants.CANCELLED)
            {
                if (playerId == operatorId)
                {
                    message = "You " + result.ToString();
                }
                else
                {
                    message = (user == null ? "Admin" : user.Name) + " " + result.ToString() + " for you";
                }
                if (EastDateTimeToday.AddDays(7) < gameDate)
                {
                    WechatNotifier.AddNotifyWechatMessage(targetPool, player, message + poolAndGameDate);
                }
                else
                {
                    WechatNotifier.AddNotifyWechatMessage(targetPool, player, message + poolAndGameDate + playerNumber);
                    int emoType = result == Constants.RESERVED ? (int)EmoTypes.Reserve : (int)EmoTypes.Cancel;
                    WechatNotifier.AddNotifyWechatMessage(targetPool, player, wechatNotifier.GetEmoMessage(emoType, numberOfReservedPlayerInTargetPool));
                }
            }
            else if (result == Constants.WAITING_TO_RESERVED)
            {
                message = result.ToString() + poolAndGameDate + playerNumber;
                WechatNotifier.AddNotifyWechatMessage(player, message);
                if (EastDateTimeToday==gameDate.Date && EastDateTimeNow.Hour >= lockWaitingListHour) WechatNotifier.AddNotifyWechatMessage(player, "It is kind of late, if you cannot make it, please cancel it right away. Thank you!");
                WechatNotifier.AddNotifyWechatMessage(targetPool, player, message);
            }
            else if (result == Constants.MOVED)
            {
                message = result.ToString() + " from pool " + originalPool.Name + " to pool " + targetPool.Name + " for the " + targetPool.DayOfWeek + " volleyball on " + gameDate.ToString("MM/dd/yyyy");
                WechatNotifier.AddNotifyWechatMessage(player, message);
                message = message + ". Total player number in pool " + originalPool.Name + ": " + originalPool.FindGameByDate(gameDate).NumberOfReservedPlayers;
                WechatNotifier.AddNotifyWechatMessage(originalPool, player, message);
                message = message + ". Total player number in pool " + targetPool.Name + ": " + numberOfReservedPlayerInTargetPool;
                WechatNotifier.AddNotifyWechatMessage(player, message);
                WechatNotifier.AddNotifyWechatMessage(targetPool, player, message);
                WechatNotifier.AddNotifyWechatMessage(targetPool, player, wechatNotifier.GetEmoMessage((int)EmoTypes.Move, numberOfReservedPlayerInTargetPool));
            }
            else if (result == Constants.WAITING)
            {
                message = (user == null ? "Admin" : (playerId == operatorId? "You" : user.Name)) + " " + result.ToString();
                WechatNotifier.AddNotifyWechatMessage(targetPool, player, message + poolAndGameDate);
            }
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
        private VList<Member> members = new VList<Member>();
        private VList<Dropin> dropins = new VList<Dropin>();
        private String wildcardPlayer;

        private List<Game> games = new List<Game>();
        private DayOfWeek dayOfWeek;
        private int reservHourForCoop = 12;
        private int settleHourForCoop = 15;
        private int lessThanPayersForCoop = 13;
        private bool autoCoopReserve = false;
        private int maxCoopPlayers = 1;
        private String statsType = "None";
        private bool isLowPool = false;

        public bool IsLowPool
        {
            get { return isLowPool; }
            set { isLowPool = value; }
        }

        public int SettleHourForCoop
        {
            get { return settleHourForCoop; }
            set { settleHourForCoop = value; }
        }

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

        public VList<Dropin> Dropins
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


        public VList<Member> Members
        {
            get { return members; }
            set { members = value; }
        }

        public List<Game> Games
        {
            get { return games; }
            set { games = value; }
        }

        public VList<Person> AllPlayers
        {
            get
            {
                VList<Person> allPlayers = new VList<Person>();
                allPlayers.Items.AddRange(this.members.Items);
                allPlayers.Items.AddRange(this.dropins.Items);
                return allPlayers;
            }
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
            return game.Dropins.Items.Exists(dropin=>dropin.PlayerId==id && dropin.Status==InOutNoshow.In);
        }

        //Get number of members who are in for the date
        public int GetNumberOfAttendingMembers(DateTime date)
        {
            Game game = FindGameByDate(date);
            return game.Members.Items.FindAll(member=>member.Status==InOutNoshow.In).Count;
        }

        public int GetNumberOfAvailableDropinSpots(DateTime date)
        {
            Game game = FindGameByDate(date);
            int availableSpot = MaximumPlayerNumber - game.AllPlayers.Items.FindAll(player=>player.Status == InOutNoshow.In).Count;
            return availableSpot > 0 ? availableSpot : 0;
        }

        //Get number of dropin for the date
        public int GetNumberOfDropins(DateTime date)
        {
            Game game = FindGameByDate(date);
            return game.Dropins.Items.FindAll(player => player.Status == InOutNoshow.In).Count;
        }

        //Get number of coop for the date
        public int GetNumberOfReservedCoops(DateTime date)
        {
            Game game = FindGameByDate(date);
            return game.Dropins.Items.FindAll(player => player.IsCoop && player.Status == InOutNoshow.In).Count;
        }

        public void AddDropin(Player player)
        {
            Dropin dropin = new Dropin(player.Id);
            Dropins.Add(dropin);
        }

        public void RemoveMember(String id)
        {
            if (Members.Exists(id))
            {
                Members.Remove(id);
            }
        }
        public void RemoveDropin(String id)
        {
            if (Dropins.Exists(id))
            {
                Dropins.Remove(id);
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
            return game.Members.Items.Exists(member=>member.PlayerId==playerId && member.Status==InOutNoshow.In);
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
        View_Player_Details, View_All_Pools, View_Past_Games, View_Future_Games, Change_Past_Games, Add_New_Player, Reserve_Coop, Reserve_Player, Move_To_High_Pool, Power_Reserve, Change_After_Locked, Admin_Management
    }

    public enum StatsTypes
    {
        None, Day, Week
    }

    public enum PlayerBooleanProperties
    {
        IsRegisterMember, IsActive, Waiver, Marked
    }
}
