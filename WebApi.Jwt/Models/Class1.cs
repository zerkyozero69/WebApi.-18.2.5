using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    /// <summary>
    /// นับพันธ์พืชอาหารสัตว์คงเหลือ
    /// </summary>
    public class RootObject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Total { get; set; }
        public string Color { get; set; }
        public List<Seed> Data { get; set; }
    }
    public class Seed
 
        {
            public string Title { get; set; }
            public double Weight { get; set; }
            public string Unit { get; set; }
        }


}