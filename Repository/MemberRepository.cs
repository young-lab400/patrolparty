using Dapper;
using FaceIDAPI.Models;
using FaceIDAPI.Models.Req;
using FaceIDAPI.Models.Resp;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FaceIDAPI.Repository
{
    public class MemberRepository: SqlHelper
    {
       

        /// <summary>
        /// 查詢卡片列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MemberRelResp> GetList()
        {


            using (var conn = new SqlConnection(ConnectionStrings))
            {
                var result = conn.Query<MemberRelResp>("SELECT * FROM Member");
                return result;
            }
        }

        /// <summary>
        /// 工號查詢
        /// </summary>
        /// <returns></returns>
        public MemberRelResp Get(string No)
        {
            No = No.ToUpper();
            using (var conn = new SqlConnection(ConnectionStrings))
            {
                var result = conn.QueryFirstOrDefault<MemberRelResp>(
                    "SELECT TOP 1 * FROM Member Where No = @No",
                    new
                    {
                        No = No
                    });
                return result;
            }
        }
        public IEnumerable<string> RelGet(Member Target)
        {
            using (var conn = new SqlConnection(ConnectionStrings))
            {
                var result = conn.Query<string>(
                    "SELECT D_No FROM Device_Member_Rel Where M_No = @No",
                    new
                    {
                        No = Target.Id
                    });
                return result;
            }
        }
        public bool Create(MemberUpReq one)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionStrings))
                {
                    string strSql = "INSERT INTO Member(Name,No,Active) OUTPUT INSERTED.[Id] VALUES (@P1,@P2,1);";

                    //新增多筆參數
                    var para = new { P1 = one.Name, P2 = one.No };
                    int s = conn.QuerySingle<int>(strSql, para);

                    if (s > 0)
                    {
                        if (one.device1)
                        {
                            strSql = "INSERT INTO Device_Member_Rel(M_No,D_No) VALUES (@P1,@P2);";
                            //新增多筆參數
                            var para1 = new { P1 = s.ToString(), P2 = "245DFC6BDEF6" };
                            conn.Query(strSql, para1);
                        }
                        if (one.device2)
                        {
                            strSql = "INSERT INTO Device_Member_Rel(M_No,D_No) VALUES (@P1,@P2);";
                            //新增多筆參數
                            var para1 = new { P1 = s.ToString(), P2 = "245DFC6BDEF7" };
                            conn.Query(strSql, para1);
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex){ return false; }
        }
        public bool Update(MemberUpReq one)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionStrings))
                {
                    //string strSql = "INSERT INTO Member(Name,No) VALUES (@P1,@P2);";
                    string strSql = "UPDATE Member SET Name = @P1 ,Active = @P3 where No = @P2";
                    //新增多筆參數
                    var para = new { P1 = one.Name, P2 = one.No.ToUpper(), P3 = one.Active };
                    conn.Execute(strSql, para);

                    if (one.Id > 0)
                    {
                        strSql = "delete Device_Member_Rel where M_NO = @P1";
                        //新增多筆參數
                        var para2 = new { P1 = one.Id };
                        conn.Execute(strSql, para2);

                        if (one.device1)
                        {
                            strSql = "INSERT INTO Device_Member_Rel(M_No,D_No) VALUES (@P1,@P2);";
                            //新增多筆參數
                            var para1 = new { P1 = one.Id, P2 = "245DFC6BDEF6" };
                            conn.Query(strSql, para1);
                        }
                        if (one.device2)
                        {
                            strSql = "INSERT INTO Device_Member_Rel(M_No,D_No) VALUES (@P1,@P2);";
                            //新增多筆參數
                            var para1 = new { P1 = one.Id, P2 = "245DFC6BDEF7" };
                            conn.Query(strSql, para1);
                        }
                    }
                    return true;
                }
            }
            catch { return false; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="No">工號</param>
        /// <param name="PicNo">圖序</param>
        /// <param name="strbase64">base64</param>
        /// <returns></returns>
        public async Task<bool> PicUpload(string No, int PicNo, string strbase64)
        {
            await using(var conn = new SqlConnection(ConnectionStrings))
            {
                string strSql = "";

                if (PicNo == 1)
                    strSql = "UPDATE Member SET Pic1=@P2,Active = 1  where No=@P1";
                else if (PicNo == 2)
                    strSql = "UPDATE Member SET Pic2=@P2,Active = 1  where No=@P1";
                else if (PicNo == 3)
                    strSql = "UPDATE Member SET Pic3=@P2,Active = 1  where No=@P1";
                else
                    return false;
                //新增多筆參數
                var para = new { P1 = No, P2 = strbase64};
                conn.Execute(strSql, para);
                return true;
            }
        }
    }
}
