using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    
    public class FarmerCitizen
    {
 
        public object CitizenID { get; set; }
        public object titleName { get; set; }
        public object FirstNameTH { get; set; }
        public object LastNameTH { get; set; }
        public object genderName { get; set; }
        public object birthDate { get; set; }
        public object tel { get; set; }
        public object email { get; set; }
        public object address { get; set; }
        public object moo { get; set; }
        public object soi { get; set; }
        public object road { get; set; }
        public object provinceNameTH { get; set; }
        public object districtNameTH { get; set; }
        public object subDistrictNameTH { get; set; }
        public object PostCode { get; set; }
        public object latitude { get; set; }
        public object longitude { get; set; }

    }
}