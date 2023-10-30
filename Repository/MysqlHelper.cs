using System.Data.SqlClient;
using System;
using MySqlConnector;

namespace FaceIDAPI
{
    public class MysqlHelper
    {
        //this field gets initialized at Startup.cs
        public static string ConnectionStrings;

        public static MySqlConnection GetConnection()
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(ConnectionStrings);
                return connection;
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
