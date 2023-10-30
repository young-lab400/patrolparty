using FaceIDAPI.Models;
using FaceIDAPI.Models.Resp;
using FaceIDAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceIDAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeviceController : ControllerBase
    {

        private readonly ILogger<DeviceController> _logger;

        /// <summary>
        /// 資料操作
        /// </summary>
        private readonly DeviceRepository _DeviceRepository;

        private readonly MemberRepository _MemberRepository;


        private readonly DBTREMINALRepository _DBTREMINALRepository;
        /// <summary>
        /// 建構式
        /// </summary>
        public DeviceController(ILogger<DeviceController> logger)
        {
            this._DeviceRepository = new DeviceRepository();
            this._MemberRepository = new MemberRepository();
            this._DBTREMINALRepository = new DBTREMINALRepository();
            _logger = logger;
        }

        [Route("List")]
        [HttpGet]
        public ActionResult<IEnumerable<Device>> GetList()
        {
               return Ok(_DeviceRepository.GetList());
        }
        [Route("Call")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<DBTERMINAL>>> CallMC(string No)
        {
            Device device  = _DeviceRepository.Get(No);
            return Ok(await _DeviceRepository.getDeviceKey(device));
        }
        [Route("Open")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<DBTERMINAL>>> CallDoor(string No)
        {
            Device device = _DeviceRepository.Get(No);
            return Ok(await _DeviceRepository.CallDoor(device));
        }
        [Route("Record")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<DBTERMINAL>>> GetRecord(string No,string DeviceNo,DateTime start, DateTime end)
        {
            Device device = _DeviceRepository.Get(DeviceNo);
            Member member = _MemberRepository.Get(No);
            return Ok(await _DeviceRepository.GetRecord(start,end,member,0,-1,device));
        }
        [Route("RecordAlways")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<GetRecordsResp>>> GetRecords()
        {
            GetRecordsResp result = new GetRecordsResp();
            try
            {
                string ip = Request.Form["ip"];
                string deviceKey = Request.Form["deviceKey"];
                Device device = _DeviceRepository.Get(deviceKey);
                string personId = Request.Form["personId"];
                Member member = _MemberRepository.Get(personId);
                string time = Request.Form["time"];
                if (string.IsNullOrEmpty(time))
                    time = "NO Data";
                string strtemperature = Request.Form["temperature"];
                if (string.IsNullOrEmpty(strtemperature))
                    strtemperature = "NO Data";
                long lontime;
                long.TryParse(time,out lontime);
                lontime = lontime / 1000;
                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 當地時區
                DateTime dt = startTime.AddSeconds(lontime);

                double temperature = 0;
                double.TryParse(strtemperature,out temperature);
                _DBTREMINALRepository.SetRecords(dt, member, temperature, device);
                result.result = 1;
                result.success = "true";
                _logger.LogInformation("OK:" + temperature.ToString("0.00"));
                return Ok(result); 
                //_logger.LogInformation(personId);
                //_logger.LogInformation(deviceKey);
                //_logger.LogInformation(dt.ToString());
                //_logger.LogInformation("OK:"+ strtemperature);


            }
            catch (Exception ex)
            {
                result.result = 0;
                result.success = "false";
                _logger.LogInformation("Error" + ex.ToString());
                return Ok(result);
            }
           
        }
    }
}
