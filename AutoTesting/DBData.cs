using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTesting
{
    public class DBData
    {
        static public string genConnstr(string env, string country)
        {
            var dbdata = DBS[env][country];
            return "data source=" + dbdata[0] + ";" +
                   "initial catalog=" + dbdata[1] + ";" +
                   "persist security info=True;" +
                   "Integrated Security=SSPI;";
        }
        static public int getResInt(string env, string country, string d)
        {
            int res = -1;
            var constr = genConnstr(env, country);

            try
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    string sql = "select a " +
                    " from b (nolock) " +
                    " where c = '" + d + "' ";

                    con.Open();
                    var cmd = new SqlCommand(sql, con);
                    res = (int)cmd.ExecuteScalar();
                }
            }
            catch (SqlException e) { res = -1; }
            return res;
        }
        static public string getResStr(string env, string country, string d)
        {
            var res = "";
            var constr = genConnstr(env, country);

            try
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    string sql = "select a " +
                    " from b (nolock) " +
                    " where c = '" + d + "' ";

                    con.Open();
                    var cmd = new SqlCommand(sql, con);
                    res = cmd.ExecuteScalar().ToString();
                }
            }
            catch (SqlException e) { res = ""; }
            return res;
        }

        /*
        * configure environments and sites 
        */
        static public Dictionary<string, Dictionary<string, string[]>> DBS =
            new Dictionary<string, Dictionary<string, string[]>>()
        {
            { "PROD", new Dictionary<string, string[]>(){
                {"SITE0", new string[] { "SERVER", "DATABASE", "USER", "PASS" } }, // Site 0
                {"SITE1", new string[] { "SERVER", "DATABASE", "USER", "PASS" } }, // Site 1
                {"SITE2", new string[] { "SERVER", "DATABASE", "USER", "PASS" } }, // Site 2
                {"SITE3", new string[] { "SERVER", "DATABASE", "USER", "PASS" } }, // Site 3
                {"SITE4", new string[] { "SERVER", "DATABASE", "USER", "PASS" } }, // Site 4
                {"SITE5", new string[] { "SERVER", "DATABASE", "USER", "PASS" } }  // Site 5
            }},
            { "QA", new Dictionary<string, string[]>(){
                {"SITE0", new string[] { "SERVER", "DATABASE", "USER", "PASS" } }, // Site 0
                {"SITE1", new string[] { "SERVER", "DATABASE", "USER", "PASS" } }, // Site 1
                {"SITE2", new string[] { "SERVER", "DATABASE", "USER", "PASS" } }, // Site 2
                {"SITE3", new string[] { "SERVER", "DATABASE", "USER", "PASS" } }, // Site 3
                {"SITE4", new string[] { "SERVER", "DATABASE", "USER", "PASS" } }, // Site 4
                {"SITE5", new string[] { "SERVER", "DATABASE", "USER", "PASS" } }  // Site 5
            }},
            { "DEV", new Dictionary<string, string[]>(){
                {"SITE0", new string[] { "SERVER", "DATABASE", "USER", "PASS" } }, // Site 0
                {"SITE1", new string[] { "SERVER", "DATABASE", "USER", "PASS" } }, // Site 1
                {"SITE2", new string[] { "SERVER", "DATABASE", "USER", "PASS" } }, // Site 2
                {"SITE3", new string[] { "SERVER", "DATABASE", "USER", "PASS" } }, // Site 3
                {"SITE4", new string[] { "SERVER", "DATABASE", "USER", "PASS" } }, // Site 4
                {"SITE5", new string[] { "SERVER", "DATABASE", "USER", "PASS" } }  // Site 5
            }}
        };
    }
}
