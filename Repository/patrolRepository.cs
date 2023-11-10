﻿using Dapper;
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

namespace FaceIDAPI.Repository
{
    public class patrolRepository:MysqlHelper
    {
    
        public IEnumerable<patrolrecord> Get_recordList(DateTime dt)
        {
            using (var conn = new MySqlConnection(ConnectionStrings))
            {
                string term = "%" + dt.ToString("yyyy-MM-dd") + "%";
                var result = conn.Query<patrolrecord>("SELECT * FROM patrolrecord Where patrolTime like @term", new
                {
                    term = term
                });
                return result;
            }
        }
        public IEnumerable<patrolerrorrecord> Get_errorrecordList(DateTime dt,string depart)
        {
            using (var conn = new MySqlConnection(ConnectionStrings))
            {   string term = "%" + dt.ToString("yyyy-MM-dd") + "%";
                string term1 = depart+"%";
                var result = conn.Query<patrolerrorrecord>("SELECT * FROM patrolerrorrecord where startTime like @term and unitId like @term1", new { term = term,term1 = term1});
                return result;
            }
        }
        public IEnumerable<patrol_caltable> Get_pointList(DateTime dt, string depart)
        {
            using (var conn = new MySqlConnection(ConnectionStrings))
            {
                string term1 =  depart + "%";
                string term = dt.ToString("yyyy-MM-dd") + "%";

//                SELECT patrolpoint.pointId as patrolPointId,SUM(CASE when convert(TIMESTAMPDIFF(MINUTE, CONCAT(DATE_SUB(CURDATE(), INTERVAL 1 DAY), ' ', patrolcycle.startHour, ':', LPAD(patrolcycle.startMinute, 2, '0'), ':00'), CONCAT(DATE_SUB(CURDATE(), INTERVAL 1 DAY), ' ', patrolcycle.endHour, ':', LPAD(patrolcycle.endMinute, 2, '0'), ':00')), SIGNED) = 0 then 1440 DIV hoursPerTime ELSE mod(1440 + convert(TIMESTAMPDIFF(MINUTE, CONCAT(DATE_SUB(CURDATE(), INTERVAL 1 DAY), ' ', patrolcycle.startHour, ':', LPAD(patrolcycle.startMinute, 2, '0'), ':00'), CONCAT(DATE_SUB(CURDATE(), INTERVAL 1 DAY), ' ', patrolcycle.endHour, ':', LPAD(patrolcycle.endMinute, 2, '0'), ':00')), SIGNED), 1440) DIV hoursPerTime END) as count FROM patrolpoint JOIN patrolcycle
//WHERE patrolpoint.unid = patrolcycle.pointUnid
//GROUP BY patrolpoint.pointId
                ///巡邏點應巡次數
                List<patrol_cal> patrolcorrecord = conn.Query<patrol_cal>("SELECT patrolpoint.pointId as patrolPointId,SUM(CASE when convert(TIMESTAMPDIFF(MINUTE, CONCAT(DATE_SUB(CURDATE(), INTERVAL 1 DAY), ' ', patrolcycle.startHour, ':', LPAD(patrolcycle.startMinute, 2, '0'), ':00'), CONCAT(DATE_SUB(CURDATE(), INTERVAL 1 DAY), ' ', patrolcycle.endHour, ':', LPAD(patrolcycle.endMinute, 2, '0'), ':00')), SIGNED) = 0 then 1440 DIV hoursPerTime ELSE mod(1440 + convert(TIMESTAMPDIFF(MINUTE, CONCAT(DATE_SUB(CURDATE(), INTERVAL 1 DAY), ' ', patrolcycle.startHour, ':', LPAD(patrolcycle.startMinute, 2, '0'), ':00'), CONCAT(DATE_SUB(CURDATE(), INTERVAL 1 DAY), ' ', patrolcycle.endHour, ':', LPAD(patrolcycle.endMinute, 2, '0'), ':00')), SIGNED), 1440) DIV hoursPerTime END) as count FROM patrolpoint JOIN patrolcycle WHERE patrolpoint.unid = patrolcycle.pointUnid GROUP BY patrolpoint.pointId ").ToList();

                ///巡邏點例外應巡次數
                ///
                /// 
                /// 
                ///巡邏點基本檔
                List<patrolpoint> patrolpoint = conn.Query<patrolpoint>("SELECT pointId,pointName FROM patrolpoint where unitId like @term1", new { term1 = term1}).ToList();
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

                 var  query =
    from errorrecord in patrolerrorrecord
    join point in patrolpoint on errorrecord.patrolPointId equals point.pointId
    join record in patrolrecord on errorrecord.patrolPointId equals record.patrolPointId
    join correcord in patrolcorrecord on errorrecord.patrolPointId equals correcord.patrolPointId
    select new patrol_caltable
    {
        patrolPointId = point.pointId,
        patrolPointName = point.pointName,
        count = errorrecord.count
        ,count1 = record.count
        ,count2 = correcord.count

    };
                return query;
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
                var result =  httpClient.PostAsync("https://notify-api.line.me/api/notify", new FormUrlEncodedContent(content));
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
            string body = "<h1>"+ dt.Year.ToString() + "/"+ dt.Month.ToString() + "/"+ dt.Day.ToString() + " 巡邏逾期情形</h1>";
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
    }
}
