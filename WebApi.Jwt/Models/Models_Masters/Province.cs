using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models.Models_Masters
{
    public class Province_Model
    {
        public Guid Oid { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceNameTH { get; set; }
        public string ProvinceNameENG { get; set; }
        public bool IsActive { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
    public class District_Model
    {
        public string Oid { get; set; }
        public string DistrictCode { get; set; }
       
        public string DistrictNameTH { get; set; }
   
        public string DistrictNameENG { get; set; }
      
        public string PostCode { get; set; }
     
        public bool IsActive { get; set; }
       
        public double Latitude { get; set; }
     
        public double Longitude { get; set; }
   

      


    }
    public class SubDistrict_Model
    {
        public string Oid { get; set; }
        public string SubDistrictCode { get; set; }

        public string SubDistrictNameTH { get; set; }
      
        public string SubDistrictNameENG { get; set; }
  
        public bool IsActive { get; set; }
     
        public double Latitude { get; set; }

        public double Longitude { get; set; }
    
    }
}