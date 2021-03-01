using nutrition.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models.ท่อนพันธุ์_กล้าพันธุ์
{
  


    public class Rodbreed_model
    {
        public string TypeMobile { get; set; }
        public string SupplierRodBreedUseProductOid { get; set; }
        public string UseNo { get; set; }
        public string UseDate { get; set; }
        public string FinanceYearOid { get; set; }
        public string FinanceYear { get; set; }
        public string OrganizationOid { get; set; }
        public string OrganizationName { get; set; }
        public string EmployeeOid { get; set; }
        public string EmployeeName { get; set; }
        public string ActivityNameOid { get; set; }
        public string ActivityName { get; set; }
        public string SubActivityOid { get; set; }
        public string SubActivityName { get; set; }
        public string SubActivityLevelOid { get; set; }
        public string SubActivityLevelName { get; set; }
        public string FullName { get; set; }
        public string FullAddress { get; set; }
        public string OrgeServiceOid { get; set; }
        public string OrgeServiceName { get; set; }
        public string RegisCusServiceOid { get; set; }
        public string RegisCusServiceName { get; set; }

        public string ReceiptNo { get; set; }

        public string ApproveDate { get; set; }

        public string Status { get; set; }

        public string Remark { get; set; }
        public string ReceiverRemark { get; set; }

        public string CancelMsg { get; set; }





        public int ServiceCount { get; set; }

        //   public XPCollection<SupplierRodBreedUseProductDetail> SupplierRodBreedUseProductDetails { get; }


        public string TotalWeight { get; set; }
        public DateTime CancelDate { get; set; }


    }
    public class Rodbreed_model_Detail
    {
        public string ActionType { get; set; }
        public string SupplierRodBreedUseProductOid { get; set; }
        public string UseNo { get; set; }
        public string UseDate { get; set; }
        public string FinanceYearOid { get; set; }
        public string FinanceYear { get; set; }
        public string OrganizationOid { get; set; }
        public string OrganizationName { get; set; }
        public string EmployeeOid { get; set; }
        public string EmployeeName { get; set; }
        public string ActivityOid { get; set; }
        public string ActivityName { get; set; }
        public string SubActivityOid { get; set; }
        public string SubActivityName { get; set; }
        public string SubActivityLevelOid { get; set; }
        public string SubActivityLevelName { get; set; }
        public string OrgeServiceOid { get; set; }
        public string FullName { get; set; }
        public string FullAddress { get; set; }
        public string RegisCusServiceOid { get; set; }
        public string RegisCusServiceName { get; set; }
        public string RegisCusServiceAddress { get; set; }

        public string ReceiptNo { get; set; }

        public string Status { get; set; }

        public string Remark { get; set; }
        public string ReceiverRemark { get; set; }

        public string CancelMsg { get; set; }
        public int ServiceCount { get; set; }

        public List<SupplierRodBreedUseProductDetail_Model> Details { get; set; }


        public string TotalWeight { get; set; }

        public string TotalPrice { get; set; }

    }
    public class SupplierRodBreedUseProductDetail_Model
    {
        public string SupplierRodBreedUseProductDetailOid { get; set; }
            public string AnimalSeedOid { get; set; }
         public string AnimalSeedName { get; set; }
        public double StockLimit { get; set; }
 
        public double Weight { get; set; }

        public string WeightUnitOid { get; set; }
        public string WeightUnitName { get; set; }
        public double Price { get; set; }
    
        public double PerPrice { get; set; }
        public string SeedTypeOid { get; set; }
        public string SeedTypeName { get; set; }
        public string SupplierRodBreedUseProductOid { get; set; }



    }
}