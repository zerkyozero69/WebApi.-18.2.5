using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    public class OrgeService_info
    {

        public string OrgeServiceOid { get; set; }
        public string OrganizationOid { get; set; }
        public string OrgeServiceID { get; set; }
        public string OrgeServiceName { get; set; }
        public string Tel { get; set; }
        public object Email { get; set; }
        public string Address { get; set; }
        public string Moo { get; set; }
        public object Soi { get; set; }
        public string Road { get; set; }
        public string ProvinceOid { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictOid { get; set; }
        public string DistrictName { get; set; }
        public string SubDistrictOid { get; set; }
        public string SubDistrictName { get; set; }
        public string IsActive { get; set; }
        public string ZipCode { get; set; }
        public string FullAddress { get; set; }
        public List<OrgeServiceDetail_Model> OrgeServiceDetails { get; set; }
    }
    public class OrgeServiceDetail_Model
    {
        public string ServiceTypeOid { get; set; }
        public string SubServiceTypeOid { get; set; }
        public string OrgeServiceOid { get; set; }

    }
    public class RegicusService_Model
    {
        public string RegicusServiceOid { get; set; }
        public string RegisterDate { get; set; }


        public string OrganizationOid { get; set; }
        public string TitleOid { get; set; }
        public string TitleName { get; set; }
        public string CitizenID { get; set; }

        public string FirstNameTH { get; set; }

        public string LastNameTH { get; set; }
        public string BirthDate { get; set; }

        public string GenderOid { get; set; }
        public string Gender { get; set; }

        public string Tel { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Moo { get; set; }
        public string Soi { get; set; }

        public string Road { get; set; }
        public string ProvinceOid { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictOid { get; set; }
        public string DistrictName { get; set; }
        public string SubDistrictOid { get; set; }
        public string SubDistrictName { get; set; }
        public string ZipCode { get; set; }
        public string FullAddress { get; set; }
        public string Remark { get; set; }

        public string FullName { get; set; }

  

        public bool IsActive { get; set; }






        //  public XPCollection<RegisterCusPetAnimalDetail> RegisterCusPetAnimalDetails { get; }

        public string ServicesNumber { get; set; }
        public List<RegisterCusServiceDetail_Model> Detail { get; set; }
    }
    public class RegisterCusServiceDetail_Model
    {

        public string ServiceTypeOid { get; set; }
        public string ServiceTypeName { get; set; }
        public string SubServiceTypeOid { get; set; }
        public string SubServiceTypeName { get; set; }

        public string ReceiveDate { get; set; }

        public string RefOid { get; set; }

        public string RegisterCusServiceOid { get; set; }
    }

    public class OrgeService_Data
    {
        public List<OrgeService_info> Data { get; set; }
    }





}