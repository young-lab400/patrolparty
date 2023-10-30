using FaceIDAPI.Models;
using FaceIDAPI.Models.Req;
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
    public class MemberController : Controller
    {
        /// <summary>
        /// 資料操作
        /// </summary>
        private readonly MemberRepository _MemberRepository;


        private readonly DeviceRepository _DeviceRepository;

        private readonly ILogger<MemberController> _logger;


        /// <summary>
        /// 建構式
        /// </summary>
        public MemberController(ILogger<MemberController> logger)
        {
            this._MemberRepository = new MemberRepository();
            this._DeviceRepository = new DeviceRepository();
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Member>> GetList()
        {
            IEnumerable<MemberRelResp> re = _MemberRepository.GetList();
            foreach (MemberRelResp one in re)
            {
                one.Device = _MemberRepository.RelGet(one);
            }
            return Ok(re);
        }
        [Route("Cr")]
        [HttpPost]
        public ActionResult Create(MemberUpReq one)
        {
            GetRecordsResp result = new GetRecordsResp();
            // Device device = _DeviceRepository.Get(No);
            try
            {
                if (_MemberRepository.Get(one.No) == null)
                {
                    // await _DeviceRepository.MemberCreate(one, device);
                    if (_MemberRepository.Create(one))
                    {
                        result.result = 1;
                        result.success = "true";
                        return Ok(result);
                    }
                }
                result.result = 0;
                result.success = "false";
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error" + ex.ToString());
                Response.StatusCode = 404;
                return NotFound();
            }
        }
        [Route("SyncCr")]
        [HttpPost]
        public ActionResult SyncCreate(MemberUpReq one)
        {
            GetRecordsResp result = new GetRecordsResp();
            try
            {
                MemberRelResp member = _MemberRepository.Get(one.No);
                if (one.device1)
                {
                    Device device = _DeviceRepository.Get("245DFC6BDEF6");
                    _DeviceRepository.MemberCreate(member, device);
                    _DeviceRepository.PicUp(member.No, 1, member.Pic1, device);
                }
                if (one.device2)
                {
                    Device device = _DeviceRepository.Get("245DFC6BDEF7");
                    _DeviceRepository.MemberCreate(member, device);
                    _DeviceRepository.PicUp(member.No, 1, member.Pic1, device);
                }
                result.result = 1;
                result.success = "true";
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error" + ex.ToString());
                Response.StatusCode = 404;
                return NotFound();
            }
        }
        [Route("Up")]
        [HttpPost]
        public async Task<ActionResult> Update(MemberUpReq one)
        {
            GetRecordsResp result = new GetRecordsResp();
            try
            {
                //if (one.device1)
                //{
                //    Device device = _DeviceRepository.Get("245DFC6BDEF6");
                //    await _DeviceRepository.MemberUpdate(one, device);
                //}
                //if (one.device2)
                //{
                //    Device device = _DeviceRepository.Get("245DFC6BDEF7");
                //    await _DeviceRepository.MemberUpdate(one, device);
                //}
                result.result = 1;
                result.success = "true";
                if (_MemberRepository.Update(one))
                    return Ok(result);
                else
                {
                    result.result = 0;
                    result.success = "false";
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error" + ex.ToString());
           
                return NotFound();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="No"> 工號</param>
        /// <param name="PicNo">照片123</param>
        /// <param name="strbase64">照片轉base64</param>
        /// <returns></returns>
        [Route("PicUp")]
        [HttpPost]
        public async Task<ActionResult> PicUpload(PicUploadReq strbase64)
        {
            GetRecordsResp result = new GetRecordsResp();
            //Device device =  _DeviceRepository.Get(DeviceNo);
            //await _DeviceRepository.PicUp(strbase64.No, strbase64.PicNo, strbase64.strbase64, device);
            try
            {
                result.result = 1;
                result.success = "true";
                bool resultPic = await _MemberRepository.PicUpload(strbase64.No, strbase64.PicNo, strbase64.strbase64);
                if (resultPic)
                    return Ok(result);
                else

                    Response.StatusCode = 404;
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error" + ex.ToString());
                Response.StatusCode = 404;
                return null;
            }
        }
        [Route("Sync")]
        [HttpPost]
        public ActionResult SyncToDevice(MemberUpReq one)
        {
            GetRecordsResp result = new GetRecordsResp();
            try
            {
                MemberRelResp member = _MemberRepository.Get(one.No);
                if (one.device1)
                {
                    Device device = _DeviceRepository.Get("245DFC6BDEF6");
                    if (member.Active)
                    {
                        _DeviceRepository.PicUp(member.No, 1, member.Pic1, device);
                        _DeviceRepository.MemberUpdate(member, device);
                    }
                    else if (!member.Active)
                         _DeviceRepository.MemberDelete(member, device);
                }
                if (one.device2)
                {
                    Device device = _DeviceRepository.Get("245DFC6BDEF7");
                    if (member.Active)
                    {
                        _DeviceRepository.PicUp(member.No, 1, member.Pic1, device);
                        _DeviceRepository.MemberUpdate(member, device);
                    }
                    else if (!member.Active)
                        _DeviceRepository.MemberDelete(member, device);
                }
                result.result = 1;
                result.success = "true";
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error" + ex.ToString());

                Response.StatusCode = 404;
                return NotFound();
            }

        }
        [Route("getNo")]
        [HttpPost]
        public ActionResult<IEnumerable<Member>> GetNo(Member one)
        {
            MemberRelResp Target = _MemberRepository.Get(one.No);
            Target.Device = _MemberRepository.RelGet(Target);
            return Ok(Target);

        }
    }
}
