using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    /// <summary>
    /// ลงทะเบียนผู้รับบริการ
    /// </summary>
    public class RegisterCustomer
    {
        public DateTime DateTime { get; set; }
        public  object CustomerTypeOid { get; set; }
        public object Get_ServiceUser_Name { get; set; }
        public object Organization_ServiceName { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
    }
    public class RigisteOrgeService
    {
        public string OrganizationOid     { get; set; }
        public string OrgeServiceID { get; set; }
        public string OrgeServiceName { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Moo { get; set; }
        public string Soi { get; set; }
        public string Road { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string SubDistrict { get; set; }
        public string Zipcode { get; set; }

    }
    public class _CustomerTypeOid
    {
        public dynamic Oid { get; set; }
        public dynamic Name { get; set; }
    }
    public class RegisterSubscriber_User
    {
        public string OrganizationOid { get; set; }
        public string RegisterDate { get; set; }
        public string CitizenID { get; set; }
        public string TitleOid { get; set; }
        public string FirstNameTH { get; set; }
        public string LastNameTH { get; set; }
        public string Gender { get; set; }
        public string BirthDate { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string DisPlayName { get; set; }
        public string Address { get; set; }
        public string Moo { get; set; }
        public string Soi { get; set; }
        public string Road { get; set; }
        public string ProvinceOid { get; set; }
        public string DistrictOid { get; set; }
        public string SubDistrictOid { get; set; }
        public string ZipCode { get; set; }
        public string FullAddress { get; set; }
        public string Remark { get; set; }
        public string IsActive { get; set; }
        public string CusTomerName { get; set; }
 
   
    }
    public class registerOrgeService
    {
        public int ServicesNumber { get; set; }
        public string OrganizationOid { get; set; }
        public string OrgeServiceName { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Moo { get; set; }
        public string Soi { get; set; }
        public string Road { get; set; }
        public string ProvinceOid { get; set; }
        public string DistrictOid { get; set; }
        public string SubDistrictOid { get; set; }


       
       // public List<OrgeServiceDetail> OrgeServiceDetails { get;set }
        public string ZipCode { get; set; }

        public bool IsActive { get; set; }

        public string FullAddress { get; }

    }
}