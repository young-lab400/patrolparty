using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceIDAPI.Models.Resp
{
    public class MemberRelResp : Member
    {
        public IEnumerable<string> Device { get; set; }
    }
}
