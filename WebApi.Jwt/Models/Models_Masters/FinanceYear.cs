using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    public class FinanceYearModel
    {
        public string FinanceYearOid { get; set; }
        public string FinanceYear { get; set; }
      
    }
    public class BudgetSourceModel
    {
        public object BudgetSourceOid { get; set; }
        public string BudgetName { get; set; }
       // public List<BudgetSourceModel> BudgetSourcelist;
    }
}