using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.IO;

namespace VballManager
{
    public class DataAccess
    {
        static object dbLock = new object();
        public static void Save(VolleyballClub manager)
        {

            var jss = new JavaScriptSerializer();
            String data = jss.Serialize(manager);
            lock (dbLock)
            {
                File.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory + Constants.DATAFILE + DateTime.Today.ToString("yyyy-MM-dd"), data);
            }
        }

        public static VolleyballClub LoadReservation()
        {
            for (int i = 0; i < 90; i++)
            {
                String dataFilePath = System.AppDomain.CurrentDomain.BaseDirectory + Constants.DATAFILE + DateTime.Today.AddDays(-1 * i).ToString("yyyy-MM-dd");
                if (File.Exists(dataFilePath))
                {
                    lock (dbLock)
                    {
                        var jss = new JavaScriptSerializer();
                        String data = File.ReadAllText(dataFilePath);
                        VolleyballClub manager = jss.Deserialize<VolleyballClub>(data);
                        //
                        foreach (Pool pool in manager.Pools)
                        {
                            if (pool.Id == null)
                            {
                                pool.Id = Guid.NewGuid().ToString();
                                Save(manager);
                            }
                        }
                        return manager;
                    }
                }
            }
            return new VolleyballClub();
        }
    }
}