using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceIDAPI.Models.Req
{
    public class MemberUpReq:Member
    {
       public bool device1 { get; set; }
       public bool device2 { get; set; }

    }
}
