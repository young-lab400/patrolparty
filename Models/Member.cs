using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceIDAPI.Models
{
    public class Member
    {
        /// <summary>
        /// Pkey
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 工號
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 卡號
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 照片1 base64
        /// </summary>
        public string Pic1 { get; set; }
        /// <summary>
        /// 照片2 base64
        /// </summary>
        public string Pic2 { get; set; }
        /// <summary>
        /// 照片3 base64
        /// </summary>
        public string Pic3 { get; set; }

        public bool Active { get; set; }


    }
}
