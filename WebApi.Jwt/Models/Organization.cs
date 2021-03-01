using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    public class OrganizationModel
    {
        public string Oid { get;set;}
        public string OrganizationCode { get; set; }
        public string SubOrganizeName { get; set; }
        public string OrganizeNameTH { get; set; }
        public string OrganizeNameENG { get; set; }
        public string Address { get; set; }
        public string Moo { get; set; }
        public string Soi { get; set; }
        public string Road { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string DLDZone { get; set; }
        public string ProvinceOid { get; set; }
        public string DistrictOid { get; set; }
        public string SubDistrictOid { get; set; }
        public string IsFactory { get; set; }
        public string IsActive { get; set; }
        public string ZipCode { get; set; }
        public string FullAddress { get; set; }
        public string Organization { get; set; }
        public string LatitudetrictOid { get; set; }
        public string Longitude { get; set; }
        public string MasterOrganization { get; set; }
        public string FacToryNo { get; set; }
  }
}