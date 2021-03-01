using nutrition.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    public class ManageAnimalSupplier_Model
    {
        public string Oid { get; set; }
        public double TotalProvinceQTY { get; set; }
        public string FinanceYearOid { get; set; }
        public string FinanceYear { get; set; }
        public string OrgZoneOid { get; set; }
        public string OrgZone { get; set; }
        public string OrganizationOid { get; set; }
        public string Organization { get; set; }
        public string AnimalSupplieOid { get; set; }
        public string AnimalSupplie { get; set; }
        public double ZoneQTY { get; set; }
        public double CenterQTY { get; set; }
        public double OfficeQTY { get; set; }
        public double OfficeGAPQTY { get; set; }
        public double OfficeBeanQTY { get; set; }
        public double SumProvinceQTY { get; set; }

        public string Status { get; set; }

      public List<ManageSubAnimalSupplier_Model> Detail { get; set; }

        public double SortID { get; set; }

    }
    public  class ManageSubAnimalSupplier_Model

    {
        public string Oid { get; set; }
        public string ProvinceOid { get; set; }
        public string Province { get; set; }
        public string AnimalSupplieTypeOid { get; set; }
        public string AnimalSupplieType { get; set; }
        public double ProvinceQTY { get; set; }
   
        public string UnitOid { get; set; }
        public string Unit { get; set; }

    }
    public class ManageAnimalSupplier_Modelinfo2
    {
       public string QuotaName { get; set; }

      public  List<objManageAnimalSupplier> detail { get; set; }
        
    }
    public class objManageAnimalSupplier
    {
        public string ProvinceOid { get; set; }
        public string Provincename { get; set; }
    }
}
