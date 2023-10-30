using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceIDAPI.Models.Resp
{
    public class GetRecordsResp
    {
        /// <summary>
        /// 此次操作是否成功，成功為 true，失敗為 false
        /// </summary>
        public string success { get; set; }
        /// <summary>
        /// 表示介面是否調通，1 成功，0 失敗
        /// </summary>
        public int result { get; set; }

    }
}
