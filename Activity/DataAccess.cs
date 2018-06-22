using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.IO;

namespace Reservation
{
    public class DataAccess
    {

        public static void Save(Reservation reservation)
        {

            var jss = new JavaScriptSerializer();
            String data = jss.Serialize(reservation);
            File.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory + Constants.DATAFILE, data);
        }

        public static Reservation LoadReservation()
        {
            if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + Constants.DATAFILE))
            {
                var jss = new JavaScriptSerializer();
                String data = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + Constants.DATAFILE);
                return jss.Deserialize<Reservation>(data);
            }
            return new Reservation();
        }
    }
}