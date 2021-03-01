using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    public class SendOrderSeedModel
    {
        public string org_oid { get; set; }
        /// <summary>
        /// รายการข้อมูลที่ส่ง
        /// </summary>
        public List<SendOrderSeedType> Sender { get; set; }
        /// <summary>
        /// รายการข้อมูลที่รับ
        /// </summary>
        public List<ReceiveOrderSeedType> Receive { get; set; }
    }

    public class SendOrderSeedType
    {
        public string SendNo { get; set; }
        public string SendDate { get; set; }
        public string FinanceYear { get; set; }
        public string SendOrgOid { get; set; }
        public string SendOrgName { get; set; }
        public string SendOrgFullName { get; set; }
        public string ReceiveOrgOid { get; set; }
        public string ReceiveOrgName { get; set; }
        public string ReceiveOrgFullName { get; set; }
        public string Remark { get; set; } 
        public string SendStatus { get; set; }
        public string CancelMsg { get; set; }
        public string TotalWeight { get; set; }
        public string RefNo { get; set; } //รหัสอ้างอิงสำหรับแสดงรายละเอียด
        public List<SendOrderSeedDetailType> Details { get; set; }
    }
    public class ReceiveOrderSeedType
    {
        public string SendNo { get; set; }
        public string SendDate { get; set; }
        public string FinanceYear { get; set; }
        public string SendOrgOid { get; set; }
        public string SendOrgName { get; set; }
        public string SendOrgFullName { get; set; }
        public string ReceiveOrgOid { get; set; }
        public string ReceiveOrgName { get; set; }
        public string ReceiveOrgFullName { get; set; }
        public string Remark { get; set; }
        public string ReceiveOrderStatus { get; set; }
        public string CancelMsg { get; set; }
        public string TotalWeight { get; set; }
        public string RefNo { get; set; } //รหัสอ้างอิงสำหรับแสดงรายละเอียด
        public List<SendOrderSeedDetailType> Details { get; set; }
    }

    public class SendOrderSeedDetailType
    {
        //[XafDisplayName("Lot Number")]
        public string LotNumber { get; set; }
        //[XafDisplayName("หน่วย")]
        public string WeightUnitOid { get; set; }
        public string WeightUnitName { get; set; }
        //[XafDisplayName("รหัสพันธุ์")]
        public string AnimalSeedCode { get; set; }
        //[XafDisplayName("ชื่อเมล็ด")]
        public string AnimalSeeName { get; set; }
        //[XafDisplayName("ชั้นพันธุ์")]
        public string AnimalSeedLevel { get; set; }
        public string AnimalSeedLevelName { get; set; }
        //[XafDisplayName("พันธุ์พืชอาหารสัตว์")]
        public string AnimalSeedOid { get; set; }
        public string AnimalSeedName { get; set; }
        //[XafDisplayName("ชั้นพันธุ์")]
        public string AnimalSeedLevelOid { get; set; }
        //[XafDisplayName("ประเภทเมล็ดพันธุ์")]
        public string SeedTypeOid { get; set; }
        public string SeedTypeName { get; set; }
        //[XafDisplayName("แหล่งงบประมาณ")]
        public string BudgetSourceOid { get; set; }
        public string BudgetSourceName { get; set; }
        //[XafDisplayName("นน.เมล็ด(กก.)")]
        public double Weight { get; set; }
        //[XafDisplayName("คงเหลือ(กก.)")]
        public double Amount { get; set; }
        public bool Used { get; set; }
        public string SendOrderSeed { get; set; }
    }

    public class SupplierUseProduct_Model
    {
        public string org_oid { get; set; }

        public List<SupplierProductUser_Model> UseACT1 { get; set; }

        public List<SupplierProductUser_Model> UseACT2 { get; set; }

        public List<SupplierProductUser_Model> UseACT3 { get; set; }
        public List<SupplierProductUser_Model> UseACT4 { get; set; }
    }

    public class SupplierProductUser_Model
    {
        public string Oid { get; set; }
        public string RegisCusServiceOid { get; set; }
        public string RegisCusService { get; set; }
        public string UseDate { get; set; }
        public string UseNo { get; set; }
        public string Refno { get; set; }
        public string FinanceYearOid { get; set; }
        public string FinanceYear { get; set; }
        public string FullName { get; set; }
        public string OrganizationName { get; set; }
   public string FullAddress { get; set; }
        public string EmployeeName { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public string ApproveDate { get; set; }
        public string ActivityNameOid { get; set; }
        public string ActivityName { get; set; }
        public string SubActivityName { get; set; }
        public string SubActivityLevelName { get; set; }
        public string ReceiptNo { get; set; }

        public string OrgeServiceOid { get; set; }

        //public string RegisCusServiceName { get; set; }
        //public string OrgeServiceName { get; set; }

        public string Weight { get; set; }
        public int ServiceCount { get; set; }

        public List<SupplierUseDetail_Model> objProduct;

    }
    public class SupplierUseDetail_Model
    {
        public string SupplierUseAnimalProductOid { get; set; }
        public double PerPrice { get; set; }

        public string SupplierUseAnimalProductName { get; set; }

        public double Amount { get; set; }

        public double Weight { get; set; }

        public double StockUsed { get; set; }

        public double StockLimit { get; set; }

        public double Price { get; set; }

        public double QuotaQTY { get; set; }



        public string QuotaTypeName { get; set; }


        public string AnimalSupplieTypeName { get; set; }

        public string AnimalSupplieName { get; set; }

        public string AnimalSeedName { get; set; }

        public string BudgetSourceName { get; set; }

    }


}