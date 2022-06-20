using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTesting
{
    public static class Setup
    {
        // SETUP MANDATORY 
        public static string ENV = "";
        public static string SITE = "";

        // ACCESS
        public static string URL = "";
        public static string USER = "";
        public static string PASSWORD = "";
        public static bool LOGIN = false;
        public static bool CONNECT = false;

        // CHECK VARIABLES
        public static bool CHECK_LOGIN = false;
        public static bool CHECK_ADMIN = false;
        public static bool CHECK_STUFF = false;

        // BROWSER
        public static bool HEADLESS = false;
        public static bool START_MAXIMIZED = false;
        public static int TIMEOUT = 32;
        public static bool CHROME = false;
        public static bool EDGE = false;
        public static bool LOG = true;

        // USER DATA
        public static string ITEM0 = "";
        public static string ITEM1 = "";
        public static string ITEM2 = "";
        public static string[] OPTION = { };
        public static string[] OPTION_EXCEPTION = { };

        // DEVOPS BUILTINS
        public static string CURRENT_STAGE = "";

        static public void TestInitialize()
        {
            // ACCESS
            ENV = Environment.GetEnvironmentVariable("ENV", EnvironmentVariableTarget.Process);
            URL = Environment.GetEnvironmentVariable("URL", EnvironmentVariableTarget.Process);
            SITE = Environment.GetEnvironmentVariable("SITE", EnvironmentVariableTarget.Process);
            CONNECT = Environment.GetEnvironmentVariable("CONNECT", EnvironmentVariableTarget.Process) == "1";
            LOGIN = Environment.GetEnvironmentVariable("LOGIN", EnvironmentVariableTarget.Process) == "1";
            USER = Environment.GetEnvironmentVariable("USER", EnvironmentVariableTarget.Process);
            PASSWORD = Environment.GetEnvironmentVariable("PASSWORD", EnvironmentVariableTarget.Process);

            // TEST CASES
            CHECK_LOGIN = Environment.GetEnvironmentVariable("CHECK_LOGIN", EnvironmentVariableTarget.Process) == "1";
            CHECK_ADMIN = Environment.GetEnvironmentVariable("CHECK_ADMIN", EnvironmentVariableTarget.Process) == "1";
            CHECK_STUFF = Environment.GetEnvironmentVariable("CHECK_STUFF", EnvironmentVariableTarget.Process) == "1";

            // BROWSER
            HEADLESS = Environment.GetEnvironmentVariable("HEADLESS", EnvironmentVariableTarget.Process) == "1";
            START_MAXIMIZED = Environment.GetEnvironmentVariable("START_MAXIMIZED", EnvironmentVariableTarget.Process) == "1";
            TIMEOUT = int.Parse(Environment.GetEnvironmentVariable("TIMEOUT", EnvironmentVariableTarget.Process) ?? "32");
            CHROME = Environment.GetEnvironmentVariable("CHROME", EnvironmentVariableTarget.Process) == "1";
            EDGE = Environment.GetEnvironmentVariable("EDGE", EnvironmentVariableTarget.Process) == "1";
            LOG = Environment.GetEnvironmentVariable("LOG", EnvironmentVariableTarget.Process) == "1";

            // USER DATA
            ITEM0 = Environment.GetEnvironmentVariable("ITEM0", EnvironmentVariableTarget.Process);
            ITEM1 = Environment.GetEnvironmentVariable("ITEM1", EnvironmentVariableTarget.Process);
            ITEM2 = Environment.GetEnvironmentVariable("ITEM2", EnvironmentVariableTarget.Process);
            string OPT = Environment.GetEnvironmentVariable("OPTION", EnvironmentVariableTarget.Process);
            OPTION = csv2array(OPT);
            string OEXC = Environment.GetEnvironmentVariable("OPTION_EXCEPTION", EnvironmentVariableTarget.Process);
            OPTION_EXCEPTION = csv2array(OEXC);

            // DEVOPS BUILTINS
            CURRENT_STAGE = Environment.GetEnvironmentVariable("CURRENT_STAGE", EnvironmentVariableTarget.Process);
        }
        public static void Log_Configuration(Action<string> log)
        {
            log("Load Release Pipeline Variables");

            // SETUP
            log("ENV: " + ENV);
            log("SITE: " + SITE);

            // ACCESS
            log("URL: " + URL);
            log("USER: " + USER);
            log("PASSWORD: " + PASSWORD);
            log("CONNECT: " + CONNECT.ToString());
            log("LOGIN: " + LOGIN.ToString());

            // TEST CASES
            log("CHECK_LOGIN: " + CHECK_LOGIN.ToString());
            log("CHECK_ADMIN: " + CHECK_ADMIN.ToString());
            log("CHECK_STUFF: " + CHECK_STUFF.ToString());

            // BROWSER
            log("HEADLESS: " + HEADLESS.ToString());
            log("START_MAXIMIZED:" + START_MAXIMIZED.ToString());
            log("TIMEOUT:" + TIMEOUT.ToString());
            log("CHROME: " + CHROME.ToString());
            log("EDGE: " + EDGE.ToString());
            log("LOG: " + LOG.ToString());

            // USER DATA
            log("ITEM0: " + ITEM0);
            log("ITEM1: " + ITEM1);
            log("ITEM2: " + ITEM2);
            log("OPTION: " + (OPTION.Length == 1 ? OPTION[0] : OPTION.Length.ToString()));
            log("OPTION EXCEPTION: " + (OPTION_EXCEPTION.Length == 1 ? OPTION_EXCEPTION[0] : OPTION_EXCEPTION.Length.ToString()));

            // DEVOPS BUILTINS
            log("CURRENT_STAGE: " + CURRENT_STAGE);
            log("AUTOMATION VERSION: 1.001");

            log("");
        }
        public static void set_local_setup(string env, string siteid,
                                            string user, bool login = true, string url2test = "")
        {
            // ACCESS
            ENV = env;
            URL = url2test;
            SITE = siteid;
            CONNECT = true;
            LOGIN = login;
            USER = user;

            // CHECKS
            CHECK_LOGIN = true;
            CHECK_ADMIN = true;
            CHECK_STUFF = true;

            // BROWSER
            HEADLESS = true;
            START_MAXIMIZED = false;
            TIMEOUT = 32;
            CHROME = true;
            EDGE = false;
            LOG = true;

            // USER DATA
            ITEM0 = "";
            ITEM1 = "";
            ITEM2 = "";
            OPTION = new string[] { };
            OPTION_EXCEPTION = new string[] { };

            // DEVOPS BUILTINS
            CURRENT_STAGE = "LOCAL";
        }

        public static void set_user_data(string item0, string item1, string item2,
                                                    string[] option = null, string[] option_exception = null)
        {
            set_user_data(item0, item1, item2,
                            string.Join(",", option ?? new string[] { }),
                            string.Join(",", option_exception ?? new string[] { }));

        }
        public static void set_user_data(string item0, string item1, string item2,
                                            string option = "", string option_exception = "")
        {
            ITEM0 = item0;
            ITEM1 = item1;
            ITEM2 = item2;
            OPTION = csv2array(option);
            OPTION_EXCEPTION = csv2array(option_exception);
        }

        public static void override_checks(
            bool check_login = true,
            bool check_admin = true,
            bool check_stuff = false)
        {
            CHECK_LOGIN = check_login;
            CHECK_ADMIN = check_admin;
            CHECK_STUFF = check_stuff;
        }
        public static void override_browser(
            bool headless = true,
            bool start_maximized = false,
            int timeout = 32,
            bool chrome = true,
            bool edge = false,
            bool log = true)
        {
            HEADLESS = headless;
            START_MAXIMIZED = start_maximized;
            TIMEOUT = timeout;
            CHROME = chrome;
            EDGE = edge;
            LOG = log;
        }
        public static string[] csv2array(string rawstr)
        {
            if (rawstr == null || rawstr == "") return new string[] { };
            var cleanstr = "";
            var areinval = false;
            foreach (var c in rawstr)
            {
                if (!areinval && (char.IsLetterOrDigit(c) || c == '(')) areinval = true;
                if (areinval) cleanstr += c;
                if (areinval && c == ',') areinval = false;
            }
            return cleanstr.TrimEnd().Split(',');
        }
    }
}
