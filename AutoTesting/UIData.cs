using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTesting
{
    public class UIData
    {
        /*
         * Configure environments and sites 
         */
        static public Dictionary<string, Dictionary<string, string>>
            URLS = new Dictionary<string, Dictionary<string, string>>()
        {
            { "PROD", new Dictionary<string, string>(){
                {"SITE0","http://google.com" }, // Site 0
                {"SITE1","http://google.com" }, // Site 1
                {"SITE2","http://google.com" }, // Site 2
                {"SITE3","http://google.com" }, // Site 3
                {"SITE4","http://google.com" }, // Site 4
                {"SITE5","http://google.com" }  // Site 5
            }},
            { "QA", new Dictionary<string, string>(){
                {"SITE0","http://google.com" }, // Site 0
                {"SITE1","http://google.com" }, // Site 1
                {"SITE2","http://google.com" }, // Site 2
                {"SITE3","http://google.com" }, // Site 3
                {"SITE4","http://google.com" }, // Site 4
                {"SITE5","http://google.com" }  // Site 5
            }},
            { "DEV", new Dictionary<string, string>(){
                {"SITE0","http://google.com" }, // Site 0
                {"SITE1","http://google.com" }, // Site 1
                {"SITE2","http://google.com" }, // Site 2
                {"SITE3","http://google.com" }, // Site 3
                {"SITE4","http://google.com" }, // Site 4
                {"SITE5","http://google.com" }  // Site 5
            }}
        };

        /*
         * Relation Station ID on javascript with ID on Database
         */
        static public Dictionary<string, int>
            STATUS = new Dictionary<string, int>()
        {
            { "OPTION0", 30 },
            { "OPTION1", 42 },
            { "OPTION2", 42 },
            { "OPTION3", 42 },
            { "OPTION4", 42 },
            { "OPTION5", 42 },
         };

        /*
         * Some options are not handle on some countries
         */
        public static bool doTestStation(string station)
        {
            if ((Setup.SITE == "SITE0" || Setup.SITE == "SITE1") && station == "OPTION0")
                return false;
            return true;
        }

        /*
         * Some options does not have security messuares taken popup
         */
        public static bool StationHasPopUps(string station)
        {
            return station != "OPTION1" &&
                    station != "OPTION2";
        }

        /*
         * Some options does not use work orders
         */
        public static bool StationUseWO(string station)
        {
            return station != "OPTION3" &&
                   station != "OPTION4";
        }

        /*
         * Get station DB ID from javascript ID
         */
        public static int GetStationStatus(string station)
        {
            return STATUS[station];
        }

        /*
         * Get javascript ID from station DB ID
         */
        public static string GetStationStatus(int station)
        {
            return STATUS.FirstOrDefault(x => x.Value == station).Key;
        }

        /*
         * Master data of IDs on javascript for the pages/stations/option
         */
        public static string[] MES_ids = {
                                    "OPTION0" ,
                                    "OPTION1" ,
                                    "OPTION2" ,
                                    "OPTION3" ,
                                    "OPTION4" ,
                                    "OPTION5" 
                                };
    }
}
