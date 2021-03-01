using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models.ภัยภิบัติ
{
    public class inserts_suppile
    {
        public string QuotaTypeOid { get; set; }
        public string SeedTypeOid { get; set; }
        public string managesuboid { get; set; }
        public string PackageOid { get; set; }
        public string orgOid { get; set; }
        public double StockLimit { get; set; }
        public double Weight { get; set; }
        public string SupplierUseAnimalProductOid { get; set; }
        public string AnimalSupplieOid { get; set; }
        public string AnimalSupplieTypeOid { get; set; }
        public double? QuotaQTY { get; set; }
        public double StockUsed { get; set; }
        public double Amount { get; set; }
        public string BudgetSourceOid { get; set; }
        public double PerUnit { get; set; }
        public string WeightUnitOid { get; set; }




    }
}