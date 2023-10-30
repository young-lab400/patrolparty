using System;
using System.Data.SqlClient;

namespace FaceIDAPI
{
    public class SqlHelper
    {
        //this field gets initialized at Startup.cs
        public static string ConnectionStrings;

        public static SqlConnection GetConnection()
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConnectionStrings);
                return connection;
            }
            catch (Exception e)
            {
                
                throw;
            }
        }
    }
}
