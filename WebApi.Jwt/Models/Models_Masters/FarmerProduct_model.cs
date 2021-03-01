using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models.Models_Masters
{
    public class FarmerProduct_model
    {
        public class Farmer_Modelinfo
        {
            public string Oid { get; set; }
            public string OrganizationOid { get; set; }
            public string OrganizeNameTH { get; set; }
            public string CitizenID { get; set; }
            public string TitleOid { get; set; }
            public string TitleName { get; set; }
            public string FirstNameTH { get; set; }
            public string LastNameTH { get; set; }
            public string GenderOid { get; set; }
            public string GenderName { get; set; }
            public string BirthDate { get; set; }
            public string Tel { get; set; }
            public string Email { get; set; }
            public string IsActive { get; set; }
            public string DisPlayName { get; set; }
            public string Address { get; set; }
            public string Moo { get; set; }
            public string Soi { get; set; }
            public string Road { get; set; }
            public string ProvinceOid { get; set; }
            public string ProvinceNameTH { get; set; }
            public string DistrictOid { get; set; }
            public string DistrictNameTH { get; set; }
            public string SubDistrictOid { get; set; }
            public string SubDistrictNameTH { get; set; }
            public string ZipCode { get; set; }
            public string FullAddress { get; set; }
            public string Latitude { get; set; }
            public string Longitude { get; set; }
            public string Rigister_Type { get; set; }
            public string FarmerGroupsOid { get; set; }
            public string Status { get; set; }
            public string CreateBy { get; set; }
            public string CreateDate { get; set; }
            public int RegisterType { get; set; }
            public string ForageTypeName1 { get; set; }
            public string ForageTypeName2 { get; set; }
            public string ForageTypeName3 { get; set; }
            public string ForageTypeName4 { get; set; }
           // public List<ForageType_Name> detail { get; set; }
        }
        public class Farmer_Modelinfo_province
        {
            public string Oid { get; set; }
            public string OrganizationOid { get; set; }
            public string OrganizeNameTH { get; set; }
            public string CitizenID { get; set; }
            public string TitleOid { get; set; }
            public string TitleName { get; set; }
            public string FirstNameTH { get; set; }
            public string LastNameTH { get; set; }
            public string GenderOid { get; set; }
            public string GenderName { get; set; }
            public string BirthDate { get; set; }
            public string Tel { get; set; }
            public string Email { get; set; }
            public string IsActive { get; set; }
            public string DisPlayName { get; set; }
            public string Address { get; set; }
            public string Moo { get; set; }
            public string Soi { get; set; }
            public string Road { get; set; }
            public string ProvinceOid { get; set; }
            public string ProvinceNameTH { get; set; }
            public string DistrictOid { get; set; }
            public string DistrictNameTH { get; set; }
            public string SubDistrictOid { get; set; }
            public string SubDistrictNameTH { get; set; }
            public string ZipCode { get; set; }
            public string FullAddress { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string Rigister_Type { get; set; }
            public string FarmerGroupsOid { get; set; }
            public string Status { get; set; }
            public string CreateBy { get; set; }
            public string CreateDate { get; set; }
            public int RegisterType { get; set; }
          
             public List<ForageType_Name> detail { get; set; }
        }
        public class farmerPro_detail
            {
            public string SeedTypeOid { get; set; }
      
        public bool IsActive { get; set; }

        public string GroupFarmer { get; set; }
   
        public string FarmerOid { get; set; }

        public string PlotInfoOidOid { get; set; }

        public double SalePrice { get; set; }

        public double ProductTotalYear { get; set; }
     
        public double ProductQuantity { get; set; }

        public double HarvestQuantity { get; set; }
     
        public double Area { get; set; }
   
        public string AnimalSeedOid { get; set; }

        public string AnimalSupplieOid { get; set; }
     
        public string ForageTypeOid { get; set; }

        public string refFarmerOid { get; set; }

      //  public FarmerProductionObjective FarmerProductionObjectiveOid { get; set; }

    //    public XPCollection<SupplierProductDetail> FarmerSupplierProductDetails { get; }
    }
        public class ForageType_Name
        {
            public string ForageTypeName1 { get; set; }
            public string ForageTypeName2 { get; set; }
            public string ForageTypeName3 { get; set; }

            public string ForageTypeName4 { get; set; }
        }
    }
}