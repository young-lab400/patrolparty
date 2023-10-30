using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceIDAPI.Models.Req
{
    public class PicUploadReq
    {
        public string No { get; set; }
        public int PicNo { get; set; }
        public string strbase64 { get; set; }
        
    }
}
