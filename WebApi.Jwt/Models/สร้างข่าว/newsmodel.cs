using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models.สร้างข่าว
{
    public class newsmodel
    {
        public string Oid { get; set; }
        public DateTime CreateDate { get; set; }
      
        public string Subject { get; set; }
       // public string URLimage { get; set; }
       
        public string Details { get; set; }
       
        public string CreateBy { get; set; }
      
        public int TotalTimes { get; set; }

        
        public List<ImageURL_Detail> objImage { get; set; }

    }
    public class ImageURL_Detail
      {
        public string ImageURL { get; set; }
    }
    public class newsDetail_model
    {
        public string Oid { get; set; }
      

        public string Subject { get; set; }

        public string Details { get; set; }

        public string CreateBy { get; set; }

        public int TotalTimes { get; set; }

    }
}

