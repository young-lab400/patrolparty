using Dapper;
using FaceIDAPI.Models;
using FaceIDAPI.Models.Resp;
using Microsoft.AspNetCore.Mvc;

using MySqlConnector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Net;
using System.Reflection;

namespace FaceIDAPI.Repository
{
    public class patrolRepository : MysqlHelper
    {
        public IEnumerable<patrol_count> Get_pointList1(DateTime dt, string depart)
        {
            using (var conn = new MySqlConnection(ConnectionStrings))
            {
                string term1 = depart + "%";
                string term = dt.AddDays(-1).ToString("yyyy-MM-dd") + "%";
                string dt2 = dt.AddDays(-1).ToString("yyyy-MM-dd");
                //晚上0700
                string dt3 = dt.AddDays(-1).AddHours(19).ToString("yyyy-MM-dd HH:mm:ss");
                //早上0700
                string dt4 = dt.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss");
                ///巡邏點例外應巡次數 + 應巡次數
                //                WITH
                //papoint AS(SELECT unid, unitId,pointId, pointName FROM patrolpoint WHERE patrolpoint.pointId LIKE @term1),
                //                errorrecord AS(SELECT distinct unitId, pointId FROM patrolerrorrecord WHERE patrolerrorrecord.startTime >= @dt3 and endtime <= @dt4),
                //                cycleexp AS(SELECT* FROM patrolcycleexp WHERE (REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(patrolcycleexp.`week`,'六','5'),'日','6') ,'一','0'),'二','1'),'三','2'),'四','3'),'五','4') = WEEKDAY(@Date)) OR(patrolcycleexp.startDate <= @Date AND patrolcycleexp.endDate >= @DATE)),
                //                cycle AS(SELECT* from patrolcycle)
                //                SELECT papoint.unitId,papoint.pointId as pointId,
                //                IFNULL(SUM(CASE when convert(TIMESTAMPDIFF(MINUTE, CONCAT(@Date, ' ', cycleexp.startHour, ':', LPAD(cycleexp.startMinute, 2, '0'), ':00'), CONCAT(@Date, ' ', cycleexp.endHour, ':', LPAD(cycleexp.endMinute, 2, '0'), ':00')), SIGNED) = 0
                //                then 1440 DIV cycleexp.hoursPerTime
                //                ELSE mod(1440 + convert(TIMESTAMPDIFF(MINUTE, CONCAT(@Date, ' ', cycleexp.startHour, ':', LPAD(cycleexp.startMinute, 2, '0'), ':00'), CONCAT(@Date, ' ', cycleexp.endHour, ':', LPAD(cycleexp.endMinute, 2, '0'), ':00')), SIGNED), 1440) DIV cycleexp.hoursPerTime END), 0) as cycleexpcount,
                //                IFNULL(SUM(CASE when convert(TIMESTAMPDIFF(MINUTE, CONCAT(@Date, ' ', cycle.startHour, ':', LPAD(cycle.startMinute, 2, '0'), ':00'), CONCAT(@Date, ' ', cycle.endHour, ':', LPAD(cycle.endMinute, 2, '0'), ':00')), SIGNED) = 0
                //                then 1440 DIV cycle.hoursPerTime
                //                ELSE mod(1440 + convert(TIMESTAMPDIFF(MINUTE, CONCAT(@Date, ' ', cycle.startHour, ':', LPAD(cycle.startMinute, 2, '0'), ':00'), CONCAT(@Date, ' ', cycle.endHour, ':', LPAD(cycle.endMinute, 2, '0'), ':00')), SIGNED), 1440) DIV cycle.hoursPerTime END), 0) as cyclecount
                //                FROM papoint
                //                inner JOIN errorrecord ON papoint.pointId = errorrecord.pointId and papoint.unitId = errorrecord.unitId
                //                left JOIN cycleexp ON papoint.unid = cycleexp.pointUnid
                //                left JOIN cycle ON papoint.unid = cycle.pointUnid GROUP BY papoint.pointId
                List<patrol_count> patrolcorrecord = conn.Query<patrol_count>("WITH papoint AS(SELECT unid,unitId, pointId, pointName FROM patrolpoint WHERE patrolpoint.pointId LIKE @term1),errorrecord AS(SELECT distinct unitId,pointId FROM patrolerrorrecord WHERE patrolerrorrecord.startTime >= @dt3 and endtime <= @dt4),cycleexp AS(SELECT * FROM patrolcycleexp WHERE (REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(patrolcycleexp.`week`,'六','5'),'日','6') ,'一','0'),'二','1'),'三','2'),'四','3'),'五','4') = WEEKDAY(@Date)) OR(patrolcycleexp.startDate <= @Date AND patrolcycleexp.endDate >= @Date)),cycle AS(SELECT * from patrolcycle) SELECT papoint.unitId,papoint.pointId as pointId,IFNULL(SUM(CASE when convert(TIMESTAMPDIFF(MINUTE, CONCAT(@Date, ' ', cycleexp.startHour, ':', LPAD(cycleexp.startMinute, 2, '0'), ':00'), CONCAT(@Date, ' ', cycleexp.endHour, ':', LPAD(cycleexp.endMinute, 2, '0'), ':00')), SIGNED) = 0 then 1440 DIV cycleexp.hoursPerTime ELSE mod(1440 + convert(TIMESTAMPDIFF(MINUTE, CONCAT(@Date, ' ', cycleexp.startHour, ':', LPAD(cycleexp.startMinute, 2, '0'), ':00'), CONCAT(@Date, ' ', cycleexp.endHour, ':', LPAD(cycleexp.endMinute, 2, '0'), ':00')), SIGNED), 1440) DIV cycleexp.hoursPerTime END),0) as cycleexpcount,IFNULL(SUM(CASE when convert(TIMESTAMPDIFF(MINUTE, CONCAT(@Date, ' ', cycle.startHour, ':', LPAD(cycle.startMinute, 2, '0'), ':00'), CONCAT(@Date, ' ', cycle.endHour, ':', LPAD(cycle.endMinute, 2, '0'), ':00')), SIGNED) = 0 then 1440 DIV cycle.hoursPerTime ELSE mod(1440 + convert(TIMESTAMPDIFF(MINUTE, CONCAT(@Date, ' ', cycle.startHour, ':', LPAD(cycle.startMinute, 2, '0'), ':00'), CONCAT(@Date, ' ', cycle.endHour, ':', LPAD(cycle.endMinute, 2, '0'), ':00')), SIGNED), 1440) DIV cycle.hoursPerTime END),0) as cyclecount FROM papoint inner JOIN errorrecord ON papoint.pointId = errorrecord.pointId and papoint.unitId = errorrecord.unitId left JOIN cycleexp ON papoint.unid = cycleexp.pointUnid left JOIN cycle ON papoint.unid = cycle.pointUnid GROUP BY papoint.pointId ", new { term = term, term1 = term1, Date = dt2 }).ToList();
                return patrolcorrecord;
            }
        }
        public IEnumerable<patrol_cal> Get_pointList2(DateTime dt, string depart)
        {
            using (var conn = new MySqlConnection(ConnectionStrings))
            {
                string term1 = depart + "%";
                string term = dt.AddDays(-1).ToString("yyyy-MM-dd") + "%";
                string dt2 = dt.AddDays(-1).ToString("yyyy-MM-dd");
                //晚上0700
                string dt3 = dt.AddDays(-1).AddHours(19).ToString("yyyy-MM-dd HH:mm:ss");
                //早上0700
                string dt4 = dt.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss");
                List<patrol_cal> patrolerrorrecord = conn.Query<patrol_cal>("SELECT unitId,pointId ,COUNT(*) as count FROM patrolerrorrecord where startTime >= @dt3 and endtime <= @dt4 and unitId like @term1 GROUP BY pointId", new { dt3 = dt3, dt4 = dt4, term1 = term1 }).ToList();

                return patrolerrorrecord;
            }
        }
        public IEnumerable<patrol_cal> Get_pointList3(DateTime dt, string depart)
        {
            using (var conn = new MySqlConnection(ConnectionStrings))
            {
                string term1 = depart + "%";
                string term = dt.AddDays(-1).ToString("yyyy-MM-dd") + "%";
                string dt2 = dt.AddDays(-1).ToString("yyyy-MM-dd");
                //晚上0700
                string dt3 = dt.AddDays(-1).AddHours(19).ToString("yyyy-MM-dd HH:mm:ss");
                //早上0700
                string dt4 = dt.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss");
                ///實際巡邏統計
                List<patrol_cal> patrolrecord = conn.Query<patrol_cal>("WITH cte AS(SELECT distinct unitId,pointId FROM patrolerrorrecord WHERE startTime >= @dt3 and endtime <= @dt4 AND unitId LIKE @term1),cte1 AS(SELECT unitId,patrolPointId, patrolPointName FROM patrolrecord WHERE patrolTime >= @dt3 and patrolTime <= @dt4  AND unitId LIKE @term1) SELECT cte.unitId,cte.pointId as pointId,COUNT(cte1.patrolPointId) AS count FROM cte left JOIN cte1 on cte.pointId = cte1.patrolPointId  and cte.unitId = cte1.unitId GROUP BY cte1.patrolPointId", new { dt3 = dt3, dt4 = dt4, term1 = term1 }).ToList();

                return patrolrecord;
            }
        }

        /// <summary>
        /// 計算應巡邏次數,逾期次數,實際巡邏次數
        /// </summary>
        /// <param name="dt">'2023-10-10' 時間點</param>
        /// <param name="depart">'AK' 單位代號</param>
        /// <returns></returns>
        public IEnumerable<patrol_caltable> Get_pointList(DateTime dt, string depart)
        {
            using (var conn = new MySqlConnection(ConnectionStrings))
            {
                string term1 = depart + "%";
                string term = dt.AddDays(-1).ToString("yyyy-MM-dd") + "%";
                string dt2 = dt.AddDays(-1).ToString("yyyy-MM-dd");
                //晚上0700
                string dt3 = dt.AddDays(-1).AddHours(19).ToString("yyyy-MM-dd HH:mm:ss");
                //早上0700
                string dt4 = dt.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss");
                ///巡邏點例外應巡次數 + 應巡次數
                //                WITH
                //papoint AS(SELECT unid, unitId,pointId, pointName FROM patrolpoint WHERE patrolpoint.pointId LIKE @term1),
                //                errorrecord AS(SELECT distinct unitId, pointId FROM patrolerrorrecord WHERE patrolerrorrecord.startTime >= @dt3 and endtime <= @dt4),
                //                cycleexp AS(SELECT* FROM patrolcycleexp WHERE (REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(patrolcycleexp.`week`,'六','5'),'日','6') ,'一','0'),'二','1'),'三','2'),'四','3'),'五','4') = WEEKDAY(@Date)) OR(patrolcycleexp.startDate <= @Date AND patrolcycleexp.endDate >= @DATE)),
                //                cycle AS(SELECT* from patrolcycle)
                //                SELECT papoint.unitId,papoint.pointId as pointId,
                //                IFNULL(SUM(CASE when convert(TIMESTAMPDIFF(MINUTE, CONCAT(@Date, ' ', cycleexp.startHour, ':', LPAD(cycleexp.startMinute, 2, '0'), ':00'), CONCAT(@Date, ' ', cycleexp.endHour, ':', LPAD(cycleexp.endMinute, 2, '0'), ':00')), SIGNED) = 0
                //                then 1440 DIV cycleexp.hoursPerTime
                //                ELSE mod(1440 + convert(TIMESTAMPDIFF(MINUTE, CONCAT(@Date, ' ', cycleexp.startHour, ':', LPAD(cycleexp.startMinute, 2, '0'), ':00'), CONCAT(@Date, ' ', cycleexp.endHour, ':', LPAD(cycleexp.endMinute, 2, '0'), ':00')), SIGNED), 1440) DIV cycleexp.hoursPerTime END), 0) as cycleexpcount,
                //                IFNULL(SUM(CASE when convert(TIMESTAMPDIFF(MINUTE, CONCAT(@Date, ' ', cycle.startHour, ':', LPAD(cycle.startMinute, 2, '0'), ':00'), CONCAT(@Date, ' ', cycle.endHour, ':', LPAD(cycle.endMinute, 2, '0'), ':00')), SIGNED) = 0
                //                then 1440 DIV cycle.hoursPerTime
                //                ELSE mod(1440 + convert(TIMESTAMPDIFF(MINUTE, CONCAT(@Date, ' ', cycle.startHour, ':', LPAD(cycle.startMinute, 2, '0'), ':00'), CONCAT(@Date, ' ', cycle.endHour, ':', LPAD(cycle.endMinute, 2, '0'), ':00')), SIGNED), 1440) DIV cycle.hoursPerTime END), 0) as cyclecount
                //                FROM papoint
                //                inner JOIN errorrecord ON papoint.pointId = errorrecord.pointId and papoint.unitId = errorrecord.unitId
                //                left JOIN cycleexp ON papoint.unid = cycleexp.pointUnid
                //                left JOIN cycle ON papoint.unid = cycle.pointUnid GROUP BY papoint.pointId
                List<patrol_count> patrolcorrecord = conn.Query<patrol_count>("WITH papoint AS(SELECT unid,unitId, pointId, pointName FROM patrolpoint WHERE patrolpoint.pointId LIKE @term1),errorrecord AS(SELECT distinct unitId,pointId FROM patrolerrorrecord WHERE patrolerrorrecord.startTime >= @dt3 and endtime <= @dt4),cycleexp AS(SELECT * FROM patrolcycleexp WHERE (REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(patrolcycleexp.`week`,'六','5'),'日','6') ,'一','0'),'二','1'),'三','2'),'四','3'),'五','4') = WEEKDAY(@Date)) OR(patrolcycleexp.startDate <= @Date AND patrolcycleexp.endDate >= @Date)),cycle AS(SELECT * from patrolcycle) SELECT papoint.unitId,papoint.pointId as pointId,IFNULL(SUM(CASE when convert(TIMESTAMPDIFF(MINUTE, CONCAT(@Date, ' ', cycleexp.startHour, ':', LPAD(cycleexp.startMinute, 2, '0'), ':00'), CONCAT(@Date, ' ', cycleexp.endHour, ':', LPAD(cycleexp.endMinute, 2, '0'), ':00')), SIGNED) = 0 then 1440 DIV cycleexp.hoursPerTime ELSE mod(1440 + convert(TIMESTAMPDIFF(MINUTE, CONCAT(@Date, ' ', cycleexp.startHour, ':', LPAD(cycleexp.startMinute, 2, '0'), ':00'), CONCAT(@Date, ' ', cycleexp.endHour, ':', LPAD(cycleexp.endMinute, 2, '0'), ':00')), SIGNED), 1440) DIV cycleexp.hoursPerTime END),0) as cycleexpcount,IFNULL(SUM(CASE when convert(TIMESTAMPDIFF(MINUTE, CONCAT(@Date, ' ', cycle.startHour, ':', LPAD(cycle.startMinute, 2, '0'), ':00'), CONCAT(@Date, ' ', cycle.endHour, ':', LPAD(cycle.endMinute, 2, '0'), ':00')), SIGNED) = 0 then 1440 DIV cycle.hoursPerTime ELSE mod(1440 + convert(TIMESTAMPDIFF(MINUTE, CONCAT(@Date, ' ', cycle.startHour, ':', LPAD(cycle.startMinute, 2, '0'), ':00'), CONCAT(@Date, ' ', cycle.endHour, ':', LPAD(cycle.endMinute, 2, '0'), ':00')), SIGNED), 1440) DIV cycle.hoursPerTime END),0) as cyclecount FROM papoint inner JOIN errorrecord ON papoint.pointId = errorrecord.pointId and papoint.unitId = errorrecord.unitId left JOIN cycleexp ON papoint.unid = cycleexp.pointUnid left JOIN cycle ON papoint.unid = cycle.pointUnid GROUP BY papoint.pointId ", new { term = term, term1 = term1, Date = dt2,dt3 = dt3,dt4 = dt4 }).ToList();


                ///巡邏點基本檔
                List<patrolpoint> patrolpoint = conn.Query<patrolpoint>("SELECT unitId,pointId,pointName FROM patrolpoint where unitId like @term1", new { term1 = term1 }).ToList();
                //                SELECT unitId,pointId as patrolPointId,COUNT(*) as COUNT
                //FROM patrolerrorrecord where startTime >= @dt3 and endtime <= @dt4 and unitId like @term1 GROUP BY pointId
                ///逾期巡邏統計
                List<patrol_cal> patrolerrorrecord = conn.Query<patrol_cal>("SELECT unitId,pointId ,COUNT(*) as count FROM patrolerrorrecord where startTime >= @dt3 and endtime <= @dt4 and unitId like @term1 GROUP BY pointId", new { dt3 = dt3,dt4 = dt4, term1 = term1 }).ToList();

 //               WITH
 //cte AS
 //(SELECT distinct unitId, pointId FROM patrolerrorrecord WHERE startTime >= @dt3 and endtime <= @dt4 AND unitId LIKE @term1)
 //                              ,cte1 AS(SELECT unitId, patrolPointId, patrolPointName FROM patrolrecord WHERE patrolTime >= @dt3 and patrolTime <= @dt4 AND unitId LIKE @term1)
 //                              SELECT cte.unitId,cte.pointId,COUNT(cte1.patrolPointId) AS COUNT FROM cte
 //                              left JOIN cte1
 //                              on cte.pointId = cte1.patrolPointId and cte.unitId = cte1.unitId
 //                              GROUP BY cte1.patrolPointId

                ///實際巡邏統計
                List<patrol_cal> patrolrecord = conn.Query<patrol_cal>("WITH cte AS(SELECT distinct unitId,pointId FROM patrolerrorrecord WHERE startTime >= @dt3 and endtime <= @dt4 AND unitId LIKE @term1),cte1 AS(SELECT unitId,patrolPointId, patrolPointName FROM patrolrecord WHERE patrolTime >= @dt3 and patrolTime <= @dt4  AND unitId LIKE @term1) SELECT cte.unitId,cte.pointId as pointId,COUNT(cte1.patrolPointId) AS count FROM cte left JOIN cte1 on cte.pointId = cte1.patrolPointId  and cte.unitId = cte1.unitId GROUP BY cte1.patrolPointId", new { dt3 = dt3, dt4 = dt4, term1 = term1 }).ToList();

                var query =
   from errorrecord in patrolerrorrecord
   join point in patrolpoint on new { errorrecord.pointId, errorrecord.unitId } equals new { point.pointId,point.unitId }
   //errorrecord.patrolPointId equals point.pointId && errorrecord.unitId equals point.unitId
   join record in patrolrecord on new {errorrecord.pointId, errorrecord.unitId} equals new {record.pointId, record.unitId}
   //errorrecord.patrolPointId equals record.patrolPointId && errorrecord.unitId equals record.unitId
   join correcord in patrolcorrecord on new { errorrecord.pointId, errorrecord.unitId } equals new { correcord.pointId, correcord.unitId }
   //errorrecord.patrolPointId equals correcord.patrolPointId
   select new patrol_caltable
   {
       unitId = point.unitId,
       patrolPointId = point.pointId,
       patrolPointName = point.pointName,
       count = errorrecord.count,
       count1 = record.count,
       count2 = correcord.cyclecount + correcord.cycleexpcount
   };
                return query;
                //return patrolcorrecord;
            }
        }
        public string LineNotify(string token, string message)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new Dictionary<string, string>();
            content.Add("message", message);
            try
            {
                var result = httpClient.PostAsync("https://notify-api.line.me/api/notify", new FormUrlEncodedContent(content));
                return "成功";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public string SendEmail(DateTime dt, IEnumerable<patrol_caltable> data)
        {

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("spam.csc.com.tw");
            string body = "<h1>" + dt.Year.ToString() + "/" + dt.Month.ToString() + "/" + dt.Day.ToString() + " 巡邏逾期情形</h1>";
            body += "<table border='1'>";
            body += "<tr>";
            body += "<th>";
            body += "駐點代號";
            body += "</th>";
            body += "<th>";
            body += "巡邏點代號";
            body += "</th>";
            body += "<th>";
            body += "巡邏點名稱";
            body += "</th>";
            body += "<th>";
            body += "未巡";
            body += "</th>";
            body += "<th>";
            body += "應巡";
            body += "</th>";
            body += "<th>";
            body += "實巡";
            body += "</th>";
            body += "</tr>";
            foreach (patrol_caltable obj in data)
            {
                body += "<tr>";
                body += "<td>";
                body += obj.unitId;
                body += "</td>";
                body += "<td>";
                body += obj.patrolPointId;
                body += "</td>";
                body += "<td>";
                body += obj.patrolPointName;
                body += "</td>";
                body += "<td>";
                body += obj.count;
                body += "</td>";
                body += "<td>";
                body += obj.count2;
                body += "</td>";
                body += "<td>";
                body += obj.count1;
                body += "</td>";
                body += "</tr>";
            }
            body += "</table>";

            mail.From = new MailAddress("hr_system@csccss.com.tw", "hr_system@csccss.com.tw");
            mail.To.Add(new MailAddress("S66995@csccss.com.tw", "廖健智"));
            mail.To.Add(new MailAddress("S33141@csccss.com.tw", "林榮皇"));
            mail.Subject = "巡邏逾期情形";
            mail.Body = body;
            mail.IsBodyHtml = true;
            //System.Net.Mail.Attachment attachment;
            // attachment = new System.Net.Mail.Attachment("c:/textfile.txt");
            // mail.Attachments.Add(attachment);
            SmtpServer.Port = 25;
            SmtpServer.Credentials = new System.Net.NetworkCredential("hr_system@csccss.com.tw", "16099725#css");
            SmtpServer.EnableSsl = true;
            try
            {
                SmtpServer.Send(mail);
                return "寄信成功";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        /// <summary>
        /// 取得長達3天以上的逾期紀錄
        /// </summary>
        /// <returns></returns>
        public IEnumerable<patrol_caltable> geterrorhistory(DateTime dt,string depart,int num)
        {
            string term1 = depart + "%";
            int num1 = 0 - num;
            string dt2 = dt.AddDays(num1).ToString("yyyy-MM-dd");
            using (var conn = new MySqlConnection(ConnectionStrings))
            {
                IEnumerable<patrol_caltable> result = conn.Query<patrol_caltable>("WITH rawbyday AS (SELECT unitId,pointId,pointName,DATE(startTime) AS startDate,COUNT(*) AS count from patrolerrorrecord WHERE startTime >= @dt2 AND startTime < @dt AND pointId LIKE @term1 GROUP BY pointId,DATE(startTime)),result AS ( SELECT pointId AS patrolPointId,pointName AS patrolPointName,COUNT(pointId) AS count,COUNT(pointId) AS COUNT1,COUNT(pointId) AS COUNT2 FROM  rawbyday GROUP BY pointId)  SELECT * FROM result WHERE COUNT = @num ", new { dt2 = dt2,dt = dt.ToString("yyyy-MM-dd"), term1 = term1 , num = num });
                return result;
            }
        }
        /// <summary>
        /// 取得num分鐘內的逾期紀錄
        /// </summary>
        /// <returns></returns>
        public IEnumerable<patrolerrorrecord> GetErrorHistory(string depart, int num)
        {
            string term1 = depart + "%";
            int num1 = 0 - num;
            string dt2 = DateTime.Now.AddMinutes(num1).ToString("yyyy-MM-dd HH:mm:ss");
            using (var conn = new MySqlConnection(ConnectionStrings))
            {

                ///patrolerrorrecord
                IEnumerable<patrolerrorrecord> result = conn.Query<patrolerrorrecord>("select * from patrolerrorrecord where createdTime > @dt2 and unitId LIKE @term1", new { dt2 = dt2, term1 = term1 });
                return result;
            }
        }
    }
}
