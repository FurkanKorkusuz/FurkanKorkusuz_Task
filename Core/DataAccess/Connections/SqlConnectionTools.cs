using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.Connections
{
    public class SqlConnectionTools
    {
        public const bool UseTestDatabase = true;

        public static string ProductionIP
        {
            get
            {            
                    return "0.0.0.0";    // Server IP
            }
        }

     
        public static string ProductionServerForConnectionString
        {
            get
            {

                if (UseTestDatabase)
                {
                    return ".";
                }
                else
                {
#if DEBUG
                    return $"{SqlConnectionTools.ProductionIP}\\.,4444"; // Server Port
#else
                    return ".";
#endif 
                }

            }
        }

        public static string ConnectionString(string user, string password)
        {

            string server = ProductionServerForConnectionString;

            return $"Server={server};Database=furkan_Task;Uid={user};Pwd={password};pooling='true';Max Pool Size=400;";
        }

    }
}
