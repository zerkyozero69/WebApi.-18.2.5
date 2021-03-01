using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models.ท่อนพันธุ์_กล้าพันธุ์
{
    public class SproutUseProduct_Modellist
    {
        public string org_oid { get; set; }

        public List<SproutUseProduct_Model> UseACT1 { get; set; }

        public List<SproutUseProduct_Model> UseACT2 { get; set; }

        public List<SproutUseProduct_Model> UseACT3 { get; set; }
        public List<SproutUseProduct_Model> UseACT4 { get; set; }
    }
    public class SproutUseProduct_Model
    {
        public string TypeMobile { get; set; }
        public string SproutUseProductOid { get; set; }
        public string UseDate { get; set; }

        public string UseNo { get; set; }
        
        public string FinanceYearOid { get; set; }
        public string FinanceYear { get; set; }
        public string OrganizationOid { get; set; }
        public string OrganizationName { get; set; }
        public string EmployeeOid { get; set; }
        public string EmployeeName { get; set; }
        public string ActivityOid { get; set; }
        //  [Appearance("DisableSubActivityOid1", TargetItems = "*", Criteria = "[ActivityOid.ActivityName] IN ('เพื่อการจำหน่าย','เพื่อสนับสนุนหน่วยงานภายนอกกรมปศุสัตว์','พัฒนาความมั่นคงด้านเสบียงสัตว์','เพื่อการแจกจ่าย (สนับสนุนเกษตรกร)')", Visibility = ViewItemVisibility.Hide)]
        public string ActivityName { get; set; }
        public string SubActivityOid { get; set; }
        public string SubActivityName { get; set; }

        public string SubActivityLevelOid { get; set; }
        public string SubActivityLevelName { get; set; }
        public string Remark { get; set; }
        public string ReceiverRemark { get; set; }
        public string Status { get; set; }
        public string ReceiptNo { get; set; }
        public string RegisCusServiceOid { get; set; }
        public string FullName { get; set; }
        public string FullAddress { get; set; }
        public string OrgeServiceOid { get; set; }
        public string OrgeServiceName { get; set; }

        public int ServiceCount { get; set; }
        public string TotalWeight { get; set; }

        public string CancelBy { get; set; }
  
        public string CancelMsg { get; set; }
        public string RefNo { get; set; }



        public List<SupplierSproutUseProductDetail_Model> Details { get; set; }

   

 
 
        public DateTime CancelDate { get; set; }

        public string ActionType { get; set; }
    }
    public class RodbreedSproutUseProduct_model_Detail
    {
        public string ActionType { get; set; }
        public string SproutUseProductOid { get; set; }
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
        public string FullName { get; set; }
        public string FullAddress { get; set; }
        public string OrgeServiceOid { get; set; }


        public string RegisCusServiceOid { get; set; }
     
        public string RegisCusServiceAddress { get; set; }

        public string ReceiptNo { get; set; }

        public string Status { get; set; }

        public string Remark { get; set; }
        public string ReceiverRemark { get; set; }
        public string CancelMsg { get; set; }
        public int ServiceCount { get; set; }

        public List<SupplierSproutUseProductDetail_Model> Details { get; set; }


        public string TotalWeight { get; set; }
        public string TotalPrice { get; set; }


    }
    public class SupplierSproutUseProductDetail_Model
    {
        public string SupplierSproutUseProductDetailOid { get; set; }
        public string AnimalSeedOid { get; set; }
        public string AnimalSeedName { get; set; }
        public double StockLimit { get; set; }
            public double Weight { get; set; } 
        public double Price { get; set; }
        public double PerPrice { get; set; }
        public string SeedTypeOid { get; set; }
        public string SeedTypeName { get; set; }
        public string SupplierSproutUseProductOid { get; set; }
    
   
        public string PackageOid { get; set; }
        public string PackageName { get; set; }



    }
}