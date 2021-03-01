using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models.แผนการผลิตโมเดล
{
    public class PlanSeedInfo_Model
    {
        public string StockDate { get; set; }

     //   [XafDisplayName("หน่วยงาน")]
        public string OrganizationOid { get; set; }
      
      //  [XafDisplayName("ปีงบประมาณ")]
        public string FinanceYearOid { get; set; }
      
       // [XafDisplayName("แหล่งงบประมาณ")]
        public string BudgetSourceOid { get; set; }
    
      //  [XafDisplayName("พันธุ์พืชอาหารสัตว์")]
        public string AnimalSeedOid { get; set; }
        
       // [XafDisplayName("ชั้นพันธุ์")]
        public string AnimalSeedLevelOid { get; set; }
   
     //   [XafDisplayName("แผนการผลิต")]
        public double QTY { get; set; }
       
       // [XafDisplayName("หน่วยน้ำหนัก")]
        public string Unit { get; set; }
        
        //[XafDisplayName("หมายเหตุ")]
        public string Remark { get; set; }

    }
}