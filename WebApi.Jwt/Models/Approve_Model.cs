using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    public class Approve_Model
    {
        public object Oid { get; set; }
        public string Send_No { get; set; }
        public string SendDate { get; set; }
        public string FinanceYear { get; set; }
        public string SendOrgName { get; set; }
        public string ReceiveOrgName { get; set; }
        public string Remark { get; set; }
        public string SendStatus { get; set; }
        public double Weight { get; set; }
        public string CancelMsg { get; set; }
        public string Weight_All { get; set; }
        public string Send_Messengr { get; set; }


        public List<SendOrderSeed_Model> objSeed;
    }

    public class SendOrderSeed_Model
    {

        public object LotNumber { get; set; }
        public object WeightUnit { get; set; }
        public string AnimalSeedCode { get; set; }
        public string AnimalSeedName { get; set; }

        public string AnimalSeedLevel { get; set; }
        public object BudgetSourceOid { get; set; }
        public object BudgetSource { get; set; }
        public string Weight { get; set; }
        public string Used { get; set; }
        public object SendOrderSeed { get; set; }
        public object AnimalSeedOid { get; set; }
        public string AnimalSeed { get; set; }
        public object AnimalSeedLevelOid { get; set; }
        public object SeedTypeOid { get; set; }
        public double Amount { get; set; }


    }

    public class ReceiveOrderSeed_Model
    {
        public string ReceiveNo { get; set; }
        public string ReceiveDate { get; set; }
        public string FinanceYear { get; set; }
        public object ReceiveOrgOid { get; set; }
        public string ReceiveOrgName { get; set; }
        public object SendOrgOid { get; set; }
        public string SendOrgName { get; set; }

        public string Weight_All { get; set; }
    }
    public class ReceiveOrderSeedDetail_Model
    {
        public object Oid { get; set; }
        public object LotNumber { get; set; }
        public string AnimalSeedCode { get; set; }
        public string AnimalSeeName { get; set; }
        public string AnimalSeedLevel { get; set; }
        public object BudgetSource { get; set; }
        public double Weights { get; set; }
        public string Used { get; set; }
        public string SendNo { get; set; }
        public object ReceiveOrderSeed { get; set; }

    }
    public class SupplierUseAnimalProduct_Model
    {
        public string org_oid { get; set; }

        public List<SupplierProductUser> UseACT1 { get; set; }

        public List<SupplierProductUser> UseACT2 { get; set; }

        public List<SupplierProductUser> UseACT3 { get; set; }
        public List<SupplierProductUser> UseACT4 { get; set; }
    }

    public class SupplierProductUser
    {
        /// <summary>
        /// แจกเมล็ดพันธุ์
        /// </summary>
        public string TypeMoblie { get; set; }
        public string Refno { get; set; }
        public string Oid { get; set; }

        public string UseDate { get; set; }
        public string UseNo { get; set; }
        public string FinanceYearOid { get; set; }
        public string FinanceYear { get; set; }
        public string BudgetSourceName { get; set; }
        public string OrganizationOid { get; set; }
        public string OrganizationName { get; set; }
        public string SubOrganizationName { get; set; }
        public string EmployeeName { get; set; }
        public string Remark { get; set; }
        public string ReceiverRemark { get; set; }
        public string Status { get; set; }
        public string ApproveDate { get; set; }
        public string ActivityNameOid { get; set; }
        public string ActivityName { get; set; }
        public string SubActivityName { get; set; }
        public string ReceiptNo { get; set; }
        public string SubActivityLevelName { get; set; }
        public string RegisCusServiceOid { get; set; }
        public string FullName { get; set; }
        //public string RegisCusServiceName { get; set; }
        public string FullAddress { get; set; }
        public string OrgeServiceOid { get; set; }
        public string OrgeServiceName { get; set; }
        //public string OrgeServiceName { get; set; }
        public string Weight { get; set; }
        public int ServiceCount { get; set; }
        public string TotalAmout { get; set; }
        public string TotalPrice { get; set; }

        public List<SupplierUseProductDetail_Model> Detail;
        public List<SupplierUseProductDetail_Model2> objDetail;

    }
    public class SendOrderSupplierAnimal_info
    {
        public string SendNo { get; set; }
        public string SendDate { get; set; }
        public string FinanceYear { get; set; }
        public object SendOrgOid { get; set; }
        public string SendOrgName { get; set; }
        public object ReceiveOrgOid { get; set; }
        public string ReceiveOrgName { get; set; }

        public double Weight { get; set; }
        public string Remark { get; set; }

        public string Send_Messengr { get; set; }
    }
    public class ReceiveOrderAnimal_info
    {
        public string SendNo { get; set; }
        public string SendDate { get; set; }
        public string FinanceYear { get; set; }
        public object ReceiveOrgOid { get; set; }
        public string ReceiveOrgName { get; set; }
        public object SendOrgOid { get; set; }
        public string SendOrgName { get; set; }
        public double Weight { get; set; }
        public string CancelMsg { get; set; }
        public string Package { get; set; }
        public string Send_Messengr { get; set; }
    }
    public class Seedlevel_Model
    {
        public string Oid { get; set; }
        public string SeedlevelName { get; set; }

    }

    public class SupplierUseProductDetail_Model
    {

        public string Oid { get; set; }
        public string SupplierUseProductOid { get; set; }

        public double PerPrice { get; set; }

        public double Amount { get; set; }

        public double Weight { get; set; }
        public double WeightAll { get; set; }


        public double StockUsed { get; set; }

        public double StockLimit { get; set; }

        public double Price { get; set; }

        public double QuotaQTY { get; set; }



        public string QuotaTypeName { get; set; }

        public string AnimalSupplieTypeOid { get; set; }
        public string AnimalSupplieTypeName { get; set; }

        public string AnimalSupplieName { get; set; }

        public string AnimalSeedName { get; set; }

        public string BudgetSourceName { get; set; }
        public string AnimalSeedLevelOid { get; set; }
        public string AnimalSeedLevelName { get; set; }
        public string AnimalSeedLevelSubName { get; set; }

    }
    public class SupplierUseProductDetail_Model2
    {
        public string Oid { get; set; }
        public string SupplierUseAnimalProductOid { get; set; }
        public double Price { get; set; }
        public double PerPrice { get; set; }

        public double Amount { get; set; }

        public double Weight { get; set; }
        public double StockUsed { get; set; }

        public string AnimalSupplieTypeName { get; set; }

        public string AnimalSupplieName { get; set; }

        public string AnimalSeedName { get; set; }

        public string BudgetSourceName { get; set; }


    }

    public class SupplierUseAnimalDetail_Model
    {

        public string Oid { get; set; }
        public string SupplierUseAnimalProductOid { get; set; }
        public string BudgetSourceName { get; set; }
        public string AnimalSeedName { get; set; }
        public string SeedTypeOid { get; set; }
        public string SeedTypeName { get; set; }
        public string AnimalSupplieName { get; set; }
        public string AnimalSupplieTypeName { get; set; }
        public double StockLimit { get; set; }
        public double StockUsed { get; set; }
        public double Weight { get; set; }
        public string QTYPlan { get; set; }  // หญ้าแห้ง
        public double Amount { get; set; }
        public string QuotaTypeOid { get; set; }
        public string ProvinceOid { get; set; }
        public string ProvinceName { get; set; }
        public string QuotaTypeName { get; set; }
        public double QuotaQTY { get; set; }
        public double Price { get; set; }

        public double PerPrice { get; set; }
        public string PackageOid { get; set; }
        public string PackageName { get; set; }


        public double PerUnit { get; set; }



    }
    public class sendSeed_info
    {
        public object Oid { get; set; }
        public string Send_No { get; set; }
        public string SendDate { get; set; }
        public object FinanceYearOid { get; set; }
        public string FinanceYear { get; set; }
        public object SendOrgOid { get; set; }

        public string SendOrgName { get; set; }
        public object ReceiveOrgoid { get; set; }
        public string ReceiveOrgName { get; set; }
        public string SendStatus { get; set; }
        public string Weight_All { get; set; }
    }
    public class SupplierAnimalUse_Model
    {
        /// <summary>
        /// แจกเมล็ดพันธุ์
        /// </summary>
        public string TypeMoblie { get; set; }
        public string Oid { get; set; }
        public string UseDate { get; set; }
        public string UseNo { get; set; }
        public string FinanceYearOid { get; set; }
        public string FinanceYear { get; set; }
        public string BudgetSourceName { get; set; }
        public string OrganizationOid { get; set; }
        public string OrganizationName { get; set; }
        public string EmployeeName { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public string ApproveDate { get; set; }
        public string ActivityNameOid { get; set; }
        public string ActivityName { get; set; }
        public string SubActivityOid { get; set; }
        public string SubActivityName { get; set; }
        public string SubActivityLevelOid { get; set; }
        public string SubActivityLevelName { get; set; }
        public string ReceiptNo { get; set; }
        public string RegisCusServiceOid { get; set; }
        public string RegisCusServiceName { get; set; }
        public string OrgeServiceOid { get; set; }
        public string OrgeServiceName { get; set; }
        public string FullName { get; set; }
        public string FullAddress { get; set; }
        public string ReceiverCitizenID { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverNumber { get; set; }
        public string ReceiverRemark { get; set; }
        public string Refno { get; set; }
        public string Weight { get; set; }
        public int ServiceCount { get; set; }
        public string TotalPrice { get; set; }
        public string TotalAmout { get; set; }
        public int PickupType { get; set; }
        public List<SupplierUseAnimalDetail_Model> details { get; set; }
    }
    public class data_info
    {
        public List<sendSeed_info> sendSS { get; set; }
    }
    public class SupplierProductUser_Model2
    {
        /// <summary>
        /// ใช้เมล็ดพันธุ์ SP
        /// </summary>


        public string SupplierUseAnimalProductOid { get; set; }

        public string UseDate { get; set; }
        public string UseNo { get; set; }
        public string CitizenID { get; set; }
        public string YearName { get; set; }
        public string FinanceYearOid { get; set; }
        public string FinanceYear { get; set; }
        public string BudgetSourceName { get; set; }
        public string OrganizationOid { get; set; }
        public string Remark { get; set; }
        public string ActivityNameOid { get; set; }
        public string ActivityName { get; set; }
        public string SubActivityOid { get; set; }
        public string ReceiptNo { get; set; }
        public string SubActivityLevelName { get; set; }
        public string RegisCusServiceOid { get; set; }
        public string RegisCusServiceName { get; set; }
        public string RegisCusServiceAddress { get; set; }
        public string OrgeServiceOid { get; set; }
        public string OrgeServiceName { get; set; }
        public string OrgeServiceAddress { get; set; }
        public string PickUp_Type { get; set; }
        public string Weight { get; set; }
        public int ServiceCount { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverNumber { get; set; }
        public string ReceiverRemark { get; set; }

    }
    public class return_OidSupplierUseAnimalProductOid
    {
        public string supplieruseanimalproductoid { get; set; }
        public string useno { get; set; }

    }

    public class SupplierAnimalUsecalarity_Model
    {
        /// <summary>
        /// แจกเมล็ดพันธุ์
        /// </summary>
        public string TypeMoblie { get; set; }
        public string SupplierUseAnimalProductOid { get; set; }
        public string UseDate { get; set; }
        public string UseNo { get; set; }
        public string FinanceYearOid { get; set; }
        public string FinanceYear { get; set; }
        public string BudgetSourceName { get; set; }
        public string OrganizationOid { get; set; }
        public string OrganizationName { get; set; }
        public string EmployeeName { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public string ApproveDate { get; set; }
        public string ActivityNameOid { get; set; }
        public string ActivityName { get; set; }
        public string SubActivityOid { get; set; }
        public string SubActivityName { get; set; }
        public string SubActivityLevelOid { get; set; }
        public string SubActivityLevelName { get; set; }
        public string ReceiptNo { get; set; }
        public string RegisCusServiceOid { get; set; }
        public string RegisCusServiceName { get; set; }
        public string OrgeServiceOid { get; set; }
        public string OrgeServiceName { get; set; }
        public string FullName { get; set; }
        public string FullAddress { get; set; }
        public string ReceiverCitizenID { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverNumber { get; set; }
        public string ReceiverRemark { get; set; }
        public string Refno { get; set; }
        public string Weight { get; set; }
        public int ServiceCount { get; set; }
        public string TotalPrice { get; set; }
        public string TotalAmout { get; set; }
        public int PickupType { get; set; }
        public List<SupplierUseAnimalDetail_Model> details { get; set; }
      
    }
    public class GenOidcalamity
    {
        public string SupplierUseAnimalProductOid { get; set; }
    }
        public enum APPROVE
        {
            Draft = 1,
            Approve = 2,
            Not_Approve = 3


        }


    }