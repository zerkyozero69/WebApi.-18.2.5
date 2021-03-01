using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{

    public class refresh_Token
    {
        public enum ApplicationTypes
        {
            JavaScript = 0,
            NativeConfidential = 1
        };
    }
}