using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    public class OrderSeedDetail
    {
        public object Oid { get; set; }
        public object LotNumber { get; set; }
        public object WeightUnit { get; set; }
        public string AnimalSeedCode { get; set; }
        public string AnimalSeeName { get; set; }
        public string AnimalSeedLevel { get; set; }
        public object AnimalSeedOid { get; set; }
        public string AnimalSeedLevelOid { get; set; }
        public string SeedType { get; set; }
        public object BudgetSource { get; set; }
        public object Weight { get; set; }
        public object Amount { get; set; }
        public object Used { get; set; }
        public object SendOrderSeed { get; set; }
    }
    public class DetailAnimalin
    {
        public string SupplierUseProductDetailOid { get; set; }
        public string AnimalSeedOid { get; set; }
        public string AnimalSeedName { get; set; }
        public string AnimalSeedLevelOid { get; set; }
        public string AnimalSeedLevelname { get; set; }
        public double StockLimit { get; set; }
        public string Weight { get; set; }
        public string WeightUnitOid { get; set; }
        public string WeightUnit { get; set; }
        public string BudgetSourceOid { get; set; }
        public string BudgetSourceName { get; set; }
        public string SupplierUseProduct { get; set; }
        public string LotNumber { get; set; }
        public string SeedTypeOid { get; set; }
        public string SeedTypename { get; set; }
        public double PerPrice { get; set; }
        public double Price { get; set; }
   
        
       
    }
}