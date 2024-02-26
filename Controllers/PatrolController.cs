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
        [Route("Get_patrolpoint")]
        public ActionResult<string> Get_patrolpoint(string dt,string depart,int num,int opt)
        {
            DateTime dt2 = DateTime.Parse(dt);

            try
            {
             
                if (opt == 1)
                {
                    var result2 = this._patrolRepository.Get_pointList1(dt2, depart);
                    if (result2 is null)
                    {
                        Response.StatusCode = 404;
                        return "統計錯誤";
                    }
                    //string mailresult = this._patrolRepository.SendEmail(dt2.AddDays(-1), result2);
                    return Ok(result2);
                }
                else if (opt == 2)
                {
                    var result2 = this._patrolRepository.Get_pointList2(dt2, depart);
                    if (result2 is null)
                    {
                        Response.StatusCode = 404;
                        return "統計錯誤";
                    }
                    //string mailresult = this._patrolRepository.SendEmail(dt2.AddDays(-1), result2);
                    return Ok(result2);
                }
                else if (opt == 3)
                {
                    var result2 = this._patrolRepository.Get_pointList3(dt2, depart);
                    if (result2 is null)
                    {
                        Response.StatusCode = 404;
                        return "統計錯誤";
                    }
                    //string mailresult = this._patrolRepository.SendEmail(dt2.AddDays(-1), result2);
                    return Ok(result2);
                }
                else
                {
                    var result2 = this._patrolRepository.Get_pointList(dt2, depart);
                    if (result2 is null)
                    {
                        Response.StatusCode = 404;
                        return "統計錯誤";
                    }
                    string mailresult = this._patrolRepository.SendEmail(dt2.AddDays(-1), result2,depart);
                    return Ok(mailresult);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
            //get webrootpath
            string webRootPath = _hostingEnvironment.WebRootPath;
            try
            {
                IEnumerable<patrol_caltable> result = this._patrolRepository.geterrorhistory(dt2, depart, num);
                 
                StreamReader reader = new StreamReader(webRootPath + "/APIpub/APP_DATA/data.json");

                var json = reader.ReadToEnd();
                string Lineresult = "空";
                List<Jsonobject> Jobjs = JsonConvert.DeserializeObject<List<Jsonobject>>(json);
                foreach (var item in Jobjs)
                {
                    if (item.key == "Linetoken" && item.depart == depart)
                    {

                        string linetoken = item.value;
                        string msg = "";
                        if (result.Count() > 0)
                        {
                            foreach (var rawdata in result)
                            {
                                //msg += rawdata.patrolPointId + " " +rawdata.patrolPointName+" " +rawdata.count + "次";
                                msg += rawdata.patrolPointId + ",";
                            }
                            msg += " 連續" + num + "天逾期";
                            Lineresult = this._patrolRepository.LineNotify(linetoken, msg);
                        }
                    }
                }
                return Ok(Lineresult);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        /// <summary>
        /// 逾期即時通知
        /// </summary>
        /// <param name="depart">單位</param>
        /// <param name="num">分鐘</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Get_ErrorPatrol")]
        public ActionResult<string> GetErrorHistory(string depart, int num)
        {
            //get webrootpath
            string webRootPath = _hostingEnvironment.WebRootPath;
            try
            {
                IEnumerable<patrolerrorrecord> result = this._patrolRepository.GetErrorHistory(depart, num);

                StreamReader reader = new StreamReader(webRootPath + "/APIpub/APP_DATA/data.json");
                var json = reader.ReadToEnd();
                string Lineresult = "空";
                List<Jsonobject> Jobjs = JsonConvert.DeserializeObject<List<Jsonobject>>(json);
                foreach (var item in Jobjs)
                {
                    //&& item.depart == depart)

                    if (item.key == "Linetoken")
                    {
                        string linetoken = item.value;
                        string msg = "";
                        if (result.Count() > 0)
                        {
                            foreach (var rawdata in result)
                            {

                                //msg += rawdata.unitId + ",";
                                msg += rawdata.pointId + ",";
                                //msg += rawdata.pointName + ",";
                                //msg += rawdata.startTime + ",";
                                msg += rawdata.endTime.ToString("HH:mm") + ",  ";
                            }

                            Lineresult = this._patrolRepository.LineNotify(linetoken, msg);
                        }
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
