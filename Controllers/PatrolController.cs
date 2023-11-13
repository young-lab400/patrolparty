using FaceIDAPI.Models;
using FaceIDAPI.Repository;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace FaceIDAPI.Controllers
{
  
    public class PatrolController : ControllerBase
    {

        /// <summary>
        /// 資料操作
        /// </summary>
        private readonly patrolRepository _patrolRepository;
        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// 建構式
        /// </summary>
        public PatrolController(IHostingEnvironment hostingEnvironment)
        {
            this._patrolRepository = new patrolRepository();
            _hostingEnvironment = hostingEnvironment;
        }
        /// <summary>
        /// 巡邏點查詢
        /// </summary>
        /// <param name="depart">巡邏點查詢</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{depart}")]
        public ActionResult<string> Get_patrolpoint(string dt,string depart)
        {
            DateTime dt2 = DateTime.Parse(dt);
            try
            {
                var result = this._patrolRepository.Get_pointList(dt2, depart);
                if (result is null)
                {
                    Response.StatusCode = 404;
                    return "統計錯誤";
                }
                string mailresult = this._patrolRepository.SendEmail(dt2, result);
                //return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
            //get webrootpath
            string webRootPath = _hostingEnvironment.WebRootPath;
            try
            {
                StreamReader reader = new StreamReader(webRootPath + "/APIpub/APP_DATA/data.json");

                var json = reader.ReadToEnd();
                string Lineresult = "空";
                List<Jsonobject> Jobjs = JsonConvert.DeserializeObject<List<Jsonobject>>(json);
                foreach (var item in Jobjs)
                {
                    if (item.key == "Linetoken")
                    {
                        string linetoken = item.value;
                        Lineresult = this._patrolRepository.LineNotify(linetoken, depart);
                    }
                }
                return Ok(Lineresult);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
      
    }
}
