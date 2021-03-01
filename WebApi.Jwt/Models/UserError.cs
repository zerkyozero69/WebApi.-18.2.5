using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    public class UserError
    {
        public string status { get; set; }
        public string code { get; set; }
        public string message { get; set; }
    }
    public class InsertResult
    {
        public string status { get; set; }
        public string message { get; set; }
    }
    public class err2
    {
        public string message { get; set; } = "รหัสหรือไอดีผิด";
    }
}