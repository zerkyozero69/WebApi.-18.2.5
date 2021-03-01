using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace WebApi.Jwt.Models
{
    public class MasterData
    { public class TitleName
        {
            public string SubTitle_Name { get; set; }
            public string Titlen_Name { get; set; }
            public Boolean IsActive { get; set; }
        }
        public class TitleName_Model
        {
            public string Oid { get; set; }
          public   string SubTitleName { get; set; }
    
            public string TitleName { get; set; }
         
            public bool IsActive { get; set; }
        }
        public class Gender_Model
        {
             public string Oid { get; set; }
            public string GenderName { get; set; }
            public Boolean IsActive { get; set; }
        }
    }
    public class _Districts
    {
        public string Oid { get; set; }
        public string DistrictName_TH { get; set; }

    }
    public class _CustomerType
    {
        public object Oid { get; set; }
        public string TypeName { get; set; }
        public bool IsActive { get; set; }
        public string Remark { get; set; }
        public string MasterCustomerType { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }

    }

    public class RegisterCusService_Model
    {
        public string Oid { get; set; }
        public string RegisterDate { get; set; }
        public string Organization { get; set; }
        public string CitizenID { get; set; }
        public string EntryCitizenID { get; }
        public string Title { get; set; }
        public string FirstNameTH { get; set; }
        public string LastNameTH { get; set; }
        public string Gender { get; set; }
        public string BirthDate { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string DisPlayName { get; }
        public string Address { get; set; }
        public string Moo { get; set; }
        public string Soi { get; set; }
        public string Road { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string SubDistrict { get; set; }
        public string ZipCode { get; set; }
        public string FullAddress { get; }
        public string Remark { get; set; }

    }
    public class EmployeeType_Model
    {
        public string EmployeeTypeName { get; set; }
        public bool IsActive { get; set; }
    }
    public class Position_Model
        {
        public string PositionName { get; set; }

       public string PositionLevelOid { get; set; }

        public string EmployeeTypeOid { get; set; }
      public string Remark { get; set; }

     public bool IsActive { get; set; }
            }
    public class PositionLevel_Model
    {
       public string PositionLevelName { get; set; }    
        public bool IsActive { get; set; }
    }
    public class UpdateResult
    {
        public string status {get; set; }
        public string message { get; set; }
        }
}