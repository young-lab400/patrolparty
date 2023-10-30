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
    [ApiController]
    [Route("[controller]")]
    public class DBTERMINALController : ControllerBase
    {
    

        /// <summary>
        /// 資料操作
        /// </summary>
        private readonly DBTREMINALRepository _DBTREMINALRepository;
        

        /// <summary>
        /// 建構式
        /// </summary>
        public DBTERMINALController()
        {
            this._DBTREMINALRepository = new DBTREMINALRepository();
            
        }

        // GET: api/Question
        [HttpGet]
        public ActionResult<IEnumerable<DBTERMINAL_D1>> GetList()
        {
            return Ok(_DBTREMINALRepository.GetList());
        }

       
        /// <summary>
        /// 工號查詢
        /// </summary>
        /// <param name="id">卡片編號</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{No}")]
        public ActionResult<IEnumerable<DBTERMINAL_D1>> Get(APIRe Re)
        {
            var result = this._DBTREMINALRepository.GetById(Re.id);
            if (result is null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return Ok(result);
            //return result;
        }


        public class APIRe
        {
            public string id { get; set; }
        }
    }
}
