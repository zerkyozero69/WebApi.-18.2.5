using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    /// <summary>
    /// เสบียงสัตว์
    /// </summary>
    public class SendOrderSupplierModel
    {
        public string org_oid { get; set; }
        /// <summary>
        /// รายการข้อมูลที่ส่ง
        /// </summary>
        public List<SendOrderSupplierType> Sender { get; set; }
        /// <summary>
        /// รายการข้อมูลที่รับ
        /// </summary>
        public List<ReceiveOrderSupplierType> Receive { get; set; }
    }

    public class SendOrderSupplierType
    {
        public string SendNo { get; set; }
        public string  SendDate { get; set; }
        public string FinanceYear { get; set; }
        public string BudgetSourceName { get; set; }
        public string SendOrgOid { get; set; }
        public string SendOrgName { get; set; }
        //[XafDisplayName("หน่วยจัดส่ง")]
        public string SendOrgFullName { get; set; }
        public string ReceiveOrgOid { get; set; }
        public string ReceiveOrgName { get; set; }
        public string ReceiveOrgFullName { get; set; }
        public string SendStatus { get; set; }
        public string Remark { get; set; }
        public string CancelMsg { get; set; }
        public string AnimalSupplieTypeName { get; set; }
        public string RefNo { get; set; }

        public string AnimalSeedName { get; set; }

        //     [XafDisplayName("หน่วยนับ")]
        public string UnitName { get; set; }


        // [XafDisplayName("ภาชนะบรรจุ")]
        public string PackageName { get; set; }

        //  [XafDisplayName("ปริมาณ")]
        public double QTY { get; set; }

        //  [XafDisplayName("วัตถุประสงค์")]
        public string ObjectTypeName { get; set; }
        //      [XafDisplayName("โควตา")]
        public string QuotaTypeName { get; set; }

        //    [XafDisplayName("ผลผลิตคงเหลือ")]
        public double StockLimit { get; set; }


        //  [XafDisplayName("ประเภทชนิดเสบียงสัตว์")]
        public string AnimalSupplieName { get; set; }

        //    [XafDisplayName("หน่วยที่ได้รับ")]
      public string PerUnit { get; set; }

        // [XafDisplayName("ปศุสัตว์จังหวัด")]
        //    public ManageSubAnimalSupplier ManageSubAnimalSupplierOid { get; set; }
        public string TotalWeight { get; set; }

         public List<SendOrderSupplierType_Model> Details { get; set; }
    }
    public class ReceiveOrderSupplierType
    {
        public string SendNo { get; set; }
        public string SendDate { get; set; }
        public string FinanceYear { get; set; }
        public string BudgetSourceName { get; set; }
        public string SendOrgOid { get; set; }
        public string SendOrgName { get; set; }
        //[XafDisplayName("หน่วยจัดส่ง")]
        public string SendOrgFullName { get; set; }
        public string ReceiveOrgOid { get; set; }
        public string ReceiveOrgName { get; set; }
        public string ReceiveOrgFullName { get; set; }
        public string ReceiveStatus { get; set; }
        public string Remark { get; set; }
        public string CancelMsg { get; set; }
        public string AnimalSupplieTypeName { get; set; }
        public string RefNo { get; set; }

        public string AnimalSeedName { get; set; }

        //     [XafDisplayName("หน่วยนับ")]
        public string UnitName { get; set; }


        // [XafDisplayName("ภาชนะบรรจุ")]
        public string PackageName { get; set; }

        //  [XafDisplayName("ปริมาณ")]
        public double QTY { get; set; }

        //  [XafDisplayName("วัตถุประสงค์")]
        public string ObjectTypeName { get; set; }
        //      [XafDisplayName("โควตา")]
        public string QuotaTypeName { get; set; }

        //    [XafDisplayName("ผลผลิตคงเหลือ")]
        public double StockLimit { get; set; }


        //  [XafDisplayName("ประเภทชนิดเสบียงสัตว์")]
        public string AnimalSupplieName { get; set; }

        //    [XafDisplayName("หน่วยที่ได้รับ")]
        public string PerUnit { get; set; }

        // [XafDisplayName("ปศุสัตว์จังหวัด")]
        //    public ManageSubAnimalSupplier ManageSubAnimalSupplierOid { get; set; }
        public string TotalWeight { get; set; }

        public List<SendOrderSupplierType_Model> Details { get; set; }
    }

    public class ManageSubAnimalSupplierOid_Model
    {
        public string ProvinceName { get; set; }
    
        public string AnimalSupplieTypeName { get; set; }
       
        public double ProvinceQTY { get; set; }
        
        public string Unit { get; set; }
       
    }

    public class SendOrderSupplierType_Model
    {
        public string BudgetSourceName { get; set; }
        public string AnimalSupplieTypeName { get; set; }
        public string AnimalSupplieName { get; set; }
        public string AnimalSeedName { get; set; }
        public string QTY { get; set; }
        public string TotalWeight { get; set; }
        public string PackageName { get; set; }

    }


}