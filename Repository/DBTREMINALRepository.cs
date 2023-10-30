using Dapper;
using FaceIDAPI.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FaceIDAPI.Repository
{
    public class DBTREMINALRepository: SqlHelper
    {
        /// <summary>
        /// 連線字串
        /// </summary>
        //private readonly string _connectString = @"Data Source=DESKTOP-USMBRO1\SQLEXPRESS;Initial Catalog=CSS;Integrated Security=false;Connect Timeout=30;User ID=API_User;Password=sQW#4246dfs;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

       
        /// <summary>
        /// 查詢卡片列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DBTERMINAL_D1> GetList()
        {
            using (var conn = new SqlConnection(ConnectionStrings))
            {
                var result = conn.Query<DBTERMINAL_D1>("SELECT  top 100 ID,NO,NAME,RID,TDATE1+' '+Tdate2 as 'time', TEMPCEN FROM DBTERMINAL_D1");
                return result;
            }
        }

        /// <summary>
        /// 功號查詢紀錄
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DBTERMINAL_D1> GetById(string id)
        {
            using (var conn = new SqlConnection(ConnectionStrings))
            {
                var result = conn.Query<DBTERMINAL_D1>(
                    "SELECT  ID,NO,NAME,RID,TDATE1+' '+Tdate2 as 'time', TEMPCEN FROM DBTERMINAL_D1 Where NO = @id",
                    new
                    {
                        id = id,
                    });
                return result;
            }
        }

        public bool SetRecords(DateTime start, Member member,double temperature, Device device)
        {
            try
            {
                temperature = double.Parse(temperature.ToString("0.00"));

                if (device.IP == "192.168.51.158")
                {
                    using (var conn = new SqlConnection(ConnectionStrings))
                    {
                        string strSql = "INSERT INTO DBTERMINAL_D1(NO,NAME,TDATE1,TDATE2,TEMPCEN) VALUES (@P1,@P2,@P3,@P4,@P5);";

                        //新增多筆參數
                        var para = new { P1 = member.No, P2 = member.Name, P3 = start.ToString("yyyy/MM/dd"), P4 = start.Hour.ToString() + ":" + start.Minute.ToString(),P5 = temperature };
                        conn.Execute(strSql, para);
                        return true;

                    }

                }
                else if (device.IP == "192.168.13.11")
                {
                    using (var conn = new SqlConnection(ConnectionStrings))
                    {
                        string strSql = "INSERT INTO DBTERMINAL_D1(NO,NAME,TDATE1,TDATE2,TEMPCEN) VALUES (@P1,@P2,@P3,@P4,@P5);";

                        //新增多筆參數
                        var para = new { P1 = member.No, P2 = member.Name, P3 = start.ToString("yyyy/MM/dd"), P4 = start.Hour.ToString() + ":" + start.Minute.ToString(), P5 = temperature };
                        conn.Execute(strSql, para);
                        return true;
                    }

                }
                else
                {
                    using (var conn = new SqlConnection(ConnectionStrings))
                    {

                        string strSql = "INSERT INTO DBTERMINAL_D1(NO,NAME,TDATE1,TDATE2,TEMPCEN) VALUES (@P1,@P2,@P3,@P4,@P5);";

                        //新增多筆參數
                        var para = new { P1 = member.No, P2 = member.Name, P3 = start.ToString("yyyy/MM/dd"), P4 = start.Hour.ToString() +":"+ start.Minute.ToString(), P5 = temperature };
                        conn.Execute(strSql, para);

                        return true;

                    }

                }
            }
            catch (Exception ex)
            {
                
                return true;
            }
            
        }
    }
}
