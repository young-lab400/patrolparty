using Dapper;
using FaceIDAPI.Models;
using FaceIDAPI.Models.Resp;
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
        public IEnumerable<patrolpoint> Get_pointList(DateTime dt, string depart)
        {
            using (var conn = new MySqlConnection(ConnectionStrings))
            {
                string term1 = "%" + depart + "%";
                string term = "%" + dt.ToString("yyyy-MM-dd") + "%";
                List<patrolpoint> patrolpoint = conn.Query<patrolpoint>("SELECT * FROM patrolpoint where unitId like @term1",new { term = term1}).ToList();
                ///SELECT pointId,pointName,COUNT(*) FROM patrolerrorrecord
                ///WHERE startTime like '2023-10-10%' AND unitId LIKE 'AK%'
                ///GROUP BY pointId
                List<patrolerrorrecord> patrolerrorrecord = conn.Query<patrolerrorrecord>("SELECT pointId,pointName,COUNT(*) FROM patrolerrorrecord where startTime like @term and unitId like @term1 GROUP BY pointId", new { term = term, term1 = term1 }).ToList();

//                WITH
//  cte AS(SELECT distinct pointId FROM patrolerrorrecord WHERE startTime like '2023-10-10%' AND unitId LIKE 'AK%')
//, cte1 AS(SELECT patrolPointId, patrolPointName FROM patrolrecord WHERE patrolTime like '2023-10-10%' AND unitId LIKE 'AK%')
// SELECT cte1.patrolPointId,cte1.patrolPointName,COUNT(cte1.patrolPointId) FROM cte1 JOIN cte
// WHERE cte.pointId = cte1.patrolPointId
// GROUP BY cte1.patrolPointId


                List<patrolrecord> patrolrecord = conn.Query<patrolrecord>("SELECT * FROM patrolrecord where startTime like @term and unitId like @term1", new { term = term, term1 = term1 }).ToList();

                return null;
            }
        }
    }
}
