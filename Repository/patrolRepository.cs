using Dapper;
using FaceIDAPI.Models;
using FaceIDAPI.Models.Resp;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

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
        public IEnumerable<patrol_cal> Get_pointList(DateTime dt, string depart)
        {
            using (var conn = new MySqlConnection(ConnectionStrings))
            {
                string term1 =  depart + "%";
                string term = dt.ToString("yyyy-MM-dd") + "%";

                //SELECT patrolpoint.pointId,
                //CASE
                //    when convert(TIMESTAMPDIFF(MINUTE, CONCAT(DATE_SUB(CURDATE(), INTERVAL 1 DAY), ' ', patrolcycle.startHour, ':', LPAD(patrolcycle.startMinute, 2, '0'), ':00')
                //, CONCAT(DATE_SUB(CURDATE(), INTERVAL 1 DAY), ' ', patrolcycle.endHour, ':', LPAD(patrolcycle.endMinute, 2, '0'), ':00')
                //), SIGNED) = 0 then 1440 DIV hoursPerTime
                //    ELSE
                //        mod(1440 + convert(TIMESTAMPDIFF(MINUTE, CONCAT(DATE_SUB(CURDATE(), INTERVAL 1 DAY), ' ', patrolcycle.startHour, ':', LPAD(patrolcycle.startMinute, 2, '0'), ':00')
                //, CONCAT(DATE_SUB(CURDATE(), INTERVAL 1 DAY), ' ', patrolcycle.endHour, ':', LPAD(patrolcycle.endMinute, 2, '0'), ':00')
                //), SIGNED), 1440) DIV hoursPerTime
                //end
                //AS gap
                // FROM patrolpoint JOIN patrolcycle
                //WHERE patrolpoint.unid = patrolcycle.pointUnid
                ///巡邏點應巡次數
                List<patrol_cal> patrolcorrecord = conn.Query<patrol_cal>("SELECT patrolpoint.pointId as patrolPointId,CASE when convert(TIMESTAMPDIFF(MINUTE, CONCAT(DATE_SUB(CURDATE(), INTERVAL 1 DAY), ' ', patrolcycle.startHour, ':', LPAD(patrolcycle.startMinute, 2, '0'), ':00'), CONCAT(DATE_SUB(CURDATE(), INTERVAL 1 DAY), ' ', patrolcycle.endHour, ':', LPAD(patrolcycle.endMinute, 2, '0'), ':00')), SIGNED) = 0 then 1440 DIV hoursPerTime ELSE mod(1440 + convert(TIMESTAMPDIFF(MINUTE, CONCAT(DATE_SUB(CURDATE(), INTERVAL 1 DAY), ' ', patrolcycle.startHour, ':', LPAD(patrolcycle.startMinute, 2, '0'), ':00'), CONCAT(DATE_SUB(CURDATE(), INTERVAL 1 DAY), ' ', patrolcycle.endHour, ':', LPAD(patrolcycle.endMinute, 2, '0'), ':00')), SIGNED), 1440) DIV hoursPerTime end as count FROM patrolpoint JOIN patrolcycle WHERE patrolpoint.unid = patrolcycle.pointUnid").ToList();


                ///巡邏點基本檔
                List<patrolpoint> patrolpoint = conn.Query<patrolpoint>("SELECT pointId,pointName FROM patrolpoint where unitId like @term1", new { term1 = term1}).ToList();
                ///SELECT pointId,pointName,COUNT(*) FROM patrolerrorrecord
                ///WHERE startTime like '2023-10-10%' AND unitId LIKE 'AK%'
                ///GROUP BY pointId
                ///逾期巡邏統計
                List<patrol_cal> patrolerrorrecord = conn.Query<patrol_cal>("SELECT pointId as patrolPointId,COUNT(*) as count FROM patrolerrorrecord where startTime like @term and unitId like @term1 GROUP BY pointId", new { term = term, term1 = term1 }).ToList();

                //                WITH
                //  cte AS(SELECT distinct pointId FROM patrolerrorrecord WHERE startTime like @term AND unitId LIKE @term1)
                //, cte1 AS(SELECT patrolPointId, patrolPointName FROM patrolrecord WHERE patrolTime like @term AND unitId LIKE @term1)
                // SELECT cte1.patrolPointId,cte1.patrolPointName,COUNT(cte1.patrolPointId) FROM cte1 JOIN cte
                // WHERE cte.pointId = cte1.patrolPointId
                // GROUP BY cte1.patrolPointId

                ///實際巡邏統計
                List<patrol_cal> patrolrecord = conn.Query<patrol_cal>("WITH cte AS(SELECT distinct pointId FROM patrolerrorrecord WHERE startTime like @term AND unitId LIKE @term1),cte1 AS(SELECT patrolPointId, patrolPointName FROM patrolrecord WHERE patrolTime like @term AND unitId LIKE @term1) SELECT cte1.patrolPointId,COUNT(cte1.patrolPointId) AS count FROM cte1 JOIN cte WHERE cte.pointId = cte1.patrolPointId GROUP BY cte1.patrolPointId", new { term = term, term1 = term1 }).ToList();

                return patrolcorrecord;
            }
        }
    }
}
