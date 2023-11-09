using FaceIDAPI.Models;
using FaceIDAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FaceIDAPI.Controllers
{
  
    public class PatrolController : ControllerBase
    {

        /// <summary>
        /// 資料操作
        /// </summary>
        private readonly patrolRepository _patrolRepository;

        /// <summary>
        /// 建構式
        /// </summary>
        public PatrolController()
        {
            this._patrolRepository = new patrolRepository();

        }
        /// <summary>
        /// 巡邏點查詢
        /// </summary>
        /// <param name="depart">巡邏點查詢</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{depart}")]
        public ActionResult<IEnumerable<patrolpoint>> Get_patrolpoint(string dt,string depart)
        {
            DateTime dt2 = DateTime.Parse(dt);
            var result = this._patrolRepository.Get_pointList(dt2,depart);
            if (result is null)
            {
                Response.StatusCode = 404;
                return null;
            }
            string mailresult = this._patrolRepository.SendEmail(dt2,result);


            return Ok(mailresult);
            //return result;
        }
      
    }
}
