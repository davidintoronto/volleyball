using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;
namespace JobResumeSender
{
    public class DataAcess
    {
        public static void Save(Companies companies)
        {
            var jss = new JavaScriptSerializer();
            String data = jss.Serialize(companies);

            File.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory + "Companies.data", data);
        }

        public static Companies LoadCompanyList()
        {
            for (int i = 0; i < 90; i++)
            {
                String dataFilePath = System.AppDomain.CurrentDomain.BaseDirectory + "Companies.data";
                if (File.Exists(dataFilePath))
                {
                    var jss = new JavaScriptSerializer();
                    String data = File.ReadAllText(dataFilePath);
                    Companies companies = jss.Deserialize<Companies>(data);
                    //
                    return companies;
                }
            }
            return new Companies();
        }
        public static Companies LoadCompanyList500()
        {
            for (int i = 0; i < 90; i++)
            {
                String dataFilePath = System.AppDomain.CurrentDomain.BaseDirectory + "Companies500.data";
                if (File.Exists(dataFilePath))
                {
                    var jss = new JavaScriptSerializer();
                    String data = File.ReadAllText(dataFilePath);
                    Companies companies = jss.Deserialize<Companies>(data);
                    //
                    return companies;
                }
            }
            return new Companies();
        }
    }
}