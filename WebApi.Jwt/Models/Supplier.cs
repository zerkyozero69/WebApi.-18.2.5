using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    public class Supplier
    {
        public class SupplierSend_Model
        {
            public string SendNo { get; set; }
            public string SendNumber { get; set; }
            public DateTime CreateDate { get; set; }
            public object FinanceYearOid { get; set; }
            public object OrganizationSendOid { get; set; }
            public object OrganizationReceiveOid { get; set; }
            public string Remark { get; set; }
            public object SendStatusOid { get; set; }


        }
        public class SupplierProduct_Model
        {
            public string LotNumber { get; set; }
            public string FinanceYearOid { get; set; }
            public string BudgetSourceOid { get; set; }
            public string OrganizationOid { get; set; }
            public string AnimalSeedOid { get; set; }
            public string AnimalSeedLevelOid { get; set; }
            public string PlotHeaderOid { get; set; }
            public string Status { get; set; }
            public double Weight { get; set; }
            public string UnitOid { get; set; }
            public DateTime LastCleansingDate { get; set; }
            public bool Used { get; set; }
            public string ReferanceUsed { get; set; }
            public string PlotInfoOidOid { get; set; }
            public string FormType { get; set; }
            public string SeedTypeOid { get; set; }
        

        }
        public class SupplierAnimalProduct_info
        {
            public string SupplierAnimalNumber { get; set; }
            public string FinanceYear { get; set; }
            public string BudgetSource { get; set; }
            public string OrganizationName { get; set; }
            public string AnimalSupplie { get; set; }
            public string AnimalSeed { get; set; }
            public string PlotInfoOid{ get; set; }
            public string Weight { get; set; }
            public string Unit { get; set; }
            public string AnimalSupplieType { get; set; }
            public string Area { get; set; }
            public string ManufactureDate { get; set; }
    
        }

        public class SupplierAnimalUseProduct_Model
        {
            public string Oid { get; set; }
            public string UseDate { get; set; }
            public string UseNo { get; set; }
            public string FinanceYearOid { get; set; }
            public string FinanceYear { get; set; }
            public string OrganizationOid { get; set; }
            public string Organization { get; set; }
            public string EmployeeOid { get; set; }
            public string Employee { get; set; }
            public string ActivityOid { get; set; }
            public string ActivityName { get; set; }
            public string SubActivityOid { get; set; }
            public string SubActivityName { get; set; }
            public string Remark { get; set; }
            public string Status { get; set; }
            public string ApproveDate { get; set; }
            public string ReceiptNo { get; set; }
            public string RegisCusServiceOid { get; set; }
            public string RegisCusService { get; set; }
            public string OrgeServiceOid { get; set; }
            public string OrgeService { get; set; }
            public int  ServiceCount { get; set; }
            IList<SupplierAnimalUseProductDetail_Model> OBJ_ProductDetail { get; set; }
        }
        public class SupplierAnimalUseProductDetail_Model
        {
            public string Oid { get; set; }
            public string AnimalSeedOid { get; set; }
            public string AnimalSeed { get; set; }
            public string AnimalSeedLevelOid { get; set; }
            public string AnimalSeedLevel { get; set; }
            public double StockLimit { get; set; }
            public double Weight { get; set; }
            public string WeightUnitOid { get; set; }
            public string WeightUnit { get; set; }
            public string BudgetSourceOid { get; set; }
            public string BudgetSource { get; set; }
            public string SupplierUseProduct { get; set; }
            public string LotNumber { get; set; }
            public string SeedTypeOid { get; set; }
            public string SeedType { get; set; }
            public double PerPrice { get; set; }
            public double Price { get; set; }

        }
        public class SupplierUseAnimalProductDetail_Model
        {
            public string BudgetSource { get; set; }

            //[XafDisplayName("แหล่งงบประมาณ")]
          


            public string AnimalSupplie { get; set; }

            //[XafDisplayName("ประเภทชนิดเสบียงสัตว์")]
            //    public SupplierUseAnimalProduct SupplierUseAnimalProductOid { get; set; }

 
            public string AnimalSupplieType { get; set; }
            // [XafDisplayName("ประเภทการผลิตเสบียงสัตว์")]
            public string AnimalSeed { get; set; }
            //[XafDisplayName("พันธุ์พืชอาหารสัตว์")]

            public string QuotaType { get; set; }
            //[XafDisplayName("โควตา")]

            public double QuotaQTY { get; set; }


            // [XafDisplayName("แผนจัดสรร (กก.)")]

          
            public double Price { get; set; }
            //   [XafDisplayName("ราคาจำหน่าย")]

            public double StockLimit { get; set; }

            //     [XafDisplayName("คงเหลือตามจริง")]


            //[XafDisplayName("ปศุสัตว์จังหวัด")]
            //   public ManageSubAnimalSupplier ManageSubAnimalSupplierOid { get; set; }


            public double StockUsed { get; set; }
            //       [XafDisplayName("จำนวนที่เบิกไปแล้ว")]
            public double Weight { get; set; }
            //         [XafDisplayName("จำนวนที่ขอใช้(กก.)")]

            public double Amount { get; set; }
            //[XafDisplayName("จำนวนคงเหลือตามแผนจัดสรร")]


            public double PerPrice { get; set; }
        }
    }
}