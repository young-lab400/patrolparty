using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceIDAPI.Models
{
    public class person
    {
        /// <summary>
        /// 工號
        /// </summary>
        public string id { get;set;}
        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 卡號 
        /// </summary>
        public string idcardNum { get; set; }

    }
}
