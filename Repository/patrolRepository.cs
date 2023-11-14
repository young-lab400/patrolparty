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

                //SELECT patrolpoint.pointId as patrolPointId,SUM(CASE when convert(TIMESTAMPDIFF(MINUTE, CONCAT(DATE_SUB(CURDATE(), INTERVAL 1 DAY), ' ', patrolcycle.startHour, ':', LPAD(patrolcycle.startMinute, 2, '0'), ':00'), CONCAT(DATE_SUB(CURDATE(), INTERVAL 1 DAY), ' ', patrolcycle.endHour, ':', LPAD(patrolcycle.endMinute, 2, '0'), ':00')), SIGNED) = 0 then 1440 DIV hoursPerTime ELSE mod(1440 + convert(TIMESTAMPDIFF(MINUTE, CONCAT(DATE_SUB(CURDATE(), INTERVAL 1 DAY), ' ', patrolcycle.startHour, ':', LPAD(patrolcycle.startMinute, 2, '0'), ':00'), CONCAT(DATE_SUB(CURDATE(), INTERVAL 1 DAY), ' ', patrolcycle.endHour, ':', LPAD(patrolcycle.endMinute, 2, '0'), ':00')), SIGNED), 1440) DIV hoursPerTime END) as count FROM patrolpoint JOIN patrolcycle
                //WHERE patrolpoint.unid = patrolcycle.pointUnid
                //GROUP BY patrolpoint.pointId
                ///巡邏點應巡次數
                //List<patrol_cal> patrolcorrecord = conn.Query<patrol_cal>("SELECT patrolpoint.pointId as patrolPointId,SUM(CASE when convert(TIMESTAMPDIFF(MINUTE, CONCAT(DATE_SUB(CURDATE(), INTERVAL 1 DAY), ' ', patrolcycle.startHour, ':', LPAD(patrolcycle.startMinute, 2, '0'), ':00'), CONCAT(DATE_SUB(CURDATE(), INTERVAL 1 DAY), ' ', patrolcycle.endHour, ':', LPAD(patrolcycle.endMinute, 2, '0'), ':00')), SIGNED) = 0 then 1440 DIV hoursPerTime ELSE mod(1440 + convert(TIMESTAMPDIFF(MINUTE, CONCAT(DATE_SUB(CURDATE(), INTERVAL 1 DAY), ' ', patrolcycle.startHour, ':', LPAD(patrolcycle.startMinute, 2, '0'), ':00'), CONCAT(DATE_SUB(CURDATE(), INTERVAL 1 DAY), ' ', patrolcycle.endHour, ':', LPAD(patrolcycle.endMinute, 2, '0'), ':00')), SIGNED), 1440) DIV hoursPerTime END) as count FROM patrolpoint JOIN patrolcycle WHERE patrolpoint.unid = patrolcycle.pointUnid GROUP BY patrolpoint.pointId ").ToList();


                ///巡邏點例外應巡次數 + 應巡次數
                //           WITH
                //papoint AS(SELECT unid, pointId, pointName FROM patrolpoint WHERE patrolpoint.pointId LIKE 'AK%'),
                //           	errorrecord AS(SELECT distinct pointId FROM patrolerrorrecord WHERE patrolerrorrecord.startTime like '2023-10-10%'),
                //           	cycleexp AS(SELECT * FROM patrolcycleexp WHERE (REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(patrolcycleexp.`week`,'六','5'),'日','6') ,'一','0'),'二','1'),'三','2'),'四','3'),'五','4') = WEEKDAY('2023-10-10'))
                //           	 OR(patrolcycleexp.startDate <= '2023-10-10' AND patrolcycleexp.endDate >= '2023-10-10')),
                //           	cycle AS(SELECT * from patrolcycle)
                //           SELECT papoint.pointId
                //           ,SUM(CASE when

                //               convert(TIMESTAMPDIFF(MINUTE, CONCAT('2023-10-10', ' ', cycleexp.startHour, ':', LPAD(cycleexp.startMinute, 2, '0'), ':00'), CONCAT('2023-10-10', ' ', cycleexp.endHour, ':', LPAD(cycleexp.endMinute, 2, '0'), ':00')), SIGNED) = 0

                //               then 1440 DIV cycleexp.hoursPerTime

                //               ELSE

                //               mod(1440 + convert(TIMESTAMPDIFF(MINUTE, CONCAT('2023-10-10', ' ', cycleexp.startHour, ':', LPAD(cycleexp.startMinute, 2, '0'), ':00'), CONCAT('2023-10-10', ' ', cycleexp.endHour, ':', LPAD(cycleexp.endMinute, 2, '0'), ':00')), SIGNED), 1440) DIV cycleexp.hoursPerTime END) as cycleexpcount
                //           ,SUM(CASE when

                //               convert(TIMESTAMPDIFF(MINUTE, CONCAT('2023-10-10', ' ', cycle.startHour, ':', LPAD(cycle.startMinute, 2, '0'), ':00'), CONCAT('2023-10-10', ' ', cycle.endHour, ':', LPAD(cycle.endMinute, 2, '0'), ':00')), SIGNED) = 0

                //               then 1440 DIV cycle.hoursPerTime

                //               ELSE

                //               mod(1440 + convert(TIMESTAMPDIFF(MINUTE, CONCAT('2023-10-10', ' ', cycle.startHour, ':', LPAD(cycle.startMinute, 2, '0'), ':00'), CONCAT('2023-10-10', ' ', cycle.endHour, ':', LPAD(cycle.endMinute, 2, '0'), ':00')), SIGNED), 1440) DIV cycle.hoursPerTime END) as cyclecount
                //           FROM papoint
                //           inner JOIN errorrecord ON papoint.pointId = errorrecord.pointId
                //           left JOIN cycleexp ON papoint.unid = cycleexp.pointUnid
                //           left JOIN cycle ON papoint.unid = cycle.pointUnid
                //           GROUP BY papoint.pointId
                List<patrol_count> patrolcorrecord = conn.Query<patrol_count>("WITH papoint AS(SELECT unid, pointId, pointName FROM patrolpoint WHERE patrolpoint.pointId LIKE @term1),errorrecord AS(SELECT distinct pointId FROM patrolerrorrecord WHERE patrolerrorrecord.startTime like @term),cycleexp AS(SELECT * FROM patrolcycleexp WHERE (REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(patrolcycleexp.`week`,'六','5'),'日','6') ,'一','0'),'二','1'),'三','2'),'四','3'),'五','4') = WEEKDAY(@Date)) OR(patrolcycleexp.startDate <= @Date AND patrolcycleexp.endDate >= @Date)),cycle AS(SELECT * from patrolcycle) SELECT papoint.pointId as patrolPointId,IFNULL(SUM(CASE when convert(TIMESTAMPDIFF(MINUTE, CONCAT(@Date, ' ', cycleexp.startHour, ':', LPAD(cycleexp.startMinute, 2, '0'), ':00'), CONCAT(@Date, ' ', cycleexp.endHour, ':', LPAD(cycleexp.endMinute, 2, '0'), ':00')), SIGNED) = 0 then 1440 DIV cycleexp.hoursPerTime ELSE mod(1440 + convert(TIMESTAMPDIFF(MINUTE, CONCAT(@Date, ' ', cycleexp.startHour, ':', LPAD(cycleexp.startMinute, 2, '0'), ':00'), CONCAT(@Date, ' ', cycleexp.endHour, ':', LPAD(cycleexp.endMinute, 2, '0'), ':00')), SIGNED), 1440) DIV cycleexp.hoursPerTime END),0) as cycleexpcount,IFNULL(SUM(CASE when convert(TIMESTAMPDIFF(MINUTE, CONCAT(@Date, ' ', cycle.startHour, ':', LPAD(cycle.startMinute, 2, '0'), ':00'), CONCAT(@Date, ' ', cycle.endHour, ':', LPAD(cycle.endMinute, 2, '0'), ':00')), SIGNED) = 0 then 1440 DIV cycle.hoursPerTime ELSE mod(1440 + convert(TIMESTAMPDIFF(MINUTE, CONCAT(@Date, ' ', cycle.startHour, ':', LPAD(cycle.startMinute, 2, '0'), ':00'), CONCAT(@Date, ' ', cycle.endHour, ':', LPAD(cycle.endMinute, 2, '0'), ':00')), SIGNED), 1440) DIV cycle.hoursPerTime END),0) as cyclecount FROM papoint inner JOIN errorrecord ON papoint.pointId = errorrecord.pointId left JOIN cycleexp ON papoint.unid = cycleexp.pointUnid left JOIN cycle ON papoint.unid = cycle.pointUnid GROUP BY papoint.pointId ", new { term = term, term1 = term1, Date = dt2 }).ToList();


                ///巡邏點基本檔
                List<patrolpoint> patrolpoint = conn.Query<patrolpoint>("SELECT pointId,pointName FROM patrolpoint where unitId like @term1", new { term1 = term1 }).ToList();
                ///SELECT pointId,pointName,COUNT(*) FROM patrolerrorrecord
                ///WHERE startTime like '2023-10-10%' AND unitId LIKE 'AK%'
                ///GROUP BY pointId
                ///逾期巡邏統計
                List<patrol_cal> patrolerrorrecord = conn.Query<patrol_cal>("SELECT pointId as patrolPointId,COUNT(*) as count FROM patrolerrorrecord where startTime like @term and unitId like @term1 GROUP BY pointId", new { term = term, term1 = term1 }).ToList();

                //              WITH
                //cte AS(SELECT distinct pointId FROM patrolerrorrecord WHERE startTime like @term AND unitId LIKE @term1)
                //              , cte1 AS(SELECT patrolPointId, patrolPointName FROM patrolrecord WHERE patrolTime like @term AND unitId LIKE @term1)
                //               SELECT cte1.patrolPointId,cte1.patrolPointName,COUNT(cte1.patrolPointId) FROM cte1 JOIN cte
                //               WHERE cte.pointId = cte1.patrolPointId
                //               GROUP BY cte1.patrolPointId

                ///實際巡邏統計
                List<patrol_cal> patrolrecord = conn.Query<patrol_cal>("WITH cte AS(SELECT distinct pointId FROM patrolerrorrecord WHERE startTime like @term AND unitId LIKE @term1),cte1 AS(SELECT patrolPointId, patrolPointName FROM patrolrecord WHERE patrolTime like @term AND unitId LIKE @term1) SELECT cte1.patrolPointId,COUNT(cte1.patrolPointId) AS count FROM cte1 JOIN cte WHERE cte.pointId = cte1.patrolPointId GROUP BY cte1.patrolPointId", new { term = term, term1 = term1 }).ToList();

                var query =
   from errorrecord in patrolerrorrecord
   join point in patrolpoint on errorrecord.patrolPointId equals point.pointId
   join record in patrolrecord on errorrecord.patrolPointId equals record.patrolPointId
   join correcord in patrolcorrecord on errorrecord.patrolPointId equals correcord.patrolPointId
   select new patrol_caltable
   {
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
                IEnumerable<patrol_caltable> result = conn.Query<patrol_caltable>("WITH rawbyday AS (SELECT pointId,pointName,DATE(startTime) AS startDate,COUNT(*) AS count from patrolerrorrecord WHERE startTime >= @dt2 AND startTime < @dt AND pointId LIKE @term1 GROUP BY pointId,DATE(startTime)),result AS ( SELECT pointId AS patrolPointId,pointName AS patrolPointName,COUNT(pointId) AS count,COUNT(pointId) AS COUNT1,COUNT(pointId) AS COUNT2 FROM  rawbyday GROUP BY pointId)  SELECT * FROM result WHERE COUNT = @num ", new { dt2 = dt2,dt = dt.ToString("yyyy-MM-dd"), term1 = term1 , num = num });
                return result;
            }
        }
    }
}
