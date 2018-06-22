using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reservation
{
    public class Reservation
    {

        private List<Player> players = new List<Player>();
         private List<Game> games = new List<Game>();
         private String title;
         private bool sharePlayers = true;
         private bool openAdmin = false;

         public bool OpenAdmin
         {
             get { return openAdmin; }
             set { openAdmin = value; }
         }
         public bool SharePlayers
         {
             get { return sharePlayers; }
             set { sharePlayers = value; }
         }

         public String Title
         {
             get { return title; }
             set { title = value; }
         }
  

        public List<Player> Players
        {
            get { return players; }
            set { players = value; }
        }

        public List<Game> Games
        {
            get { return games; }
            set { games = value; }
        }
        private List<ActivityType> activityTypes = new List<ActivityType>();

        public List<ActivityType> ActivityTypes
        {
            get { return activityTypes; }
            set { activityTypes = value; }
        }


        //Find member by id
        public Player FindPlayerById(String id)
        {
            return this.players.Find(
                delegate(Player mb)
                {
                    return mb.Id == id;
                }
            );
        }

        //Find dropin by name
        public Player FindPlayerByName(String name)
        {
            return this.players.Find(
                delegate(Player nmb)
                {
                    return nmb.Name == name;
                }
            );
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

        //Find game by id
        public Game FindGameById(String id)
        {
            return this.games.Find(
                delegate(Game game)
                {
                    return game.Id == id;
                }
            );
        }
        //Find dropin by name
        public bool IsPlayerReserved(Game game, String name)
        {
            Player nmb = FindPlayerByName(name);
            if (nmb == null)
            {
                return false;
            }
            return game.Players.Contains(nmb.Id);
        }


        public void AssignASpotToWaitingList(Game game)
        {
             if (game.WaitingListIds.Count > 0)
            {
                if (!game.Players.Contains(game.WaitingListIds[0]))
                {
                    game.Players.Add(game.WaitingListIds[0]);
                }
                game.WaitingListIds.RemoveAt(0);
            }
        }

        //Get number of players for the game
        public int GetNumberOfPlayers(DateTime date)
        {
            Game game = FindGameByDate(date);
            return game.Players.Count;
        }

        public void DeletePlayer(String id)
        {
            Player user = FindPlayerById(id);

            foreach (Game game in games)
            {
                game.Players.Remove(id);
                game.WaitingListIds.Remove(id);
            }
            this.players.Remove(user);
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
        //User exists
        public bool PlayerNameExists(String name)
        {
            Player existMemeber = players.Find(
               delegate(Player member)
               {
                   return member.Name == name;
               }
           );
            return existMemeber != null;
        }
         //Game exists
        public bool GameExists(DateTime date)
        {
            return FindGameByDate(date) != null;
        }

        public ActivityType FindActivityTypeById(String id)
        {
            return this.ActivityTypes.Find(
                delegate(ActivityType type)
                {
                    return type.Id == id;
                }
            );
        }

        public FeeType FindFeeTypeById(String activityTypeId, String feeTypeId)
        {
            ActivityType activityType = this.ActivityTypes.Find(
                delegate(ActivityType type)
                {
                    return type.Id == activityTypeId;
                }
            );
            return activityType.FeeTypes.Find(
                 delegate(FeeType type)
                 {
                     return type.Id == feeTypeId;
                 }
             );

        }
    }

    public class ActivityType
    {
        public ActivityType() { }
        public ActivityType(String name)
        {
            this.id = Guid.NewGuid().ToString();
            this.name = name;
        }

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
        private List<FeeType> feeTypes = new List<FeeType>();

        public List<FeeType> FeeTypes
        {
            get { return feeTypes; }
            set { feeTypes = value; }
        }
    }

    public class FeeType
    {
        public FeeType() { }
        public FeeType(String name)
        {
            this.id = Guid.NewGuid().ToString();
            this.name = name;
        }

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
    }



}