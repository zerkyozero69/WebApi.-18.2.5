using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models.แผนการผลิตโมเดล
{
    public class การใช้เมล็ดพันธุ์อนุมัติภัยพิบัติ
    {
        public class SupplierUseProduct_inserts
        {
            public string OrgeServiceOid { get; set; }
 
            public string RegisCusServiceOid { get; set; }

            public string ReceiptNo { get; set; }
   
            public DateTime ApproveDate { get; set; }
   
            public string Status { get; set; }
        
            public string Remark { get; set; }
    
            public bool ChkGroupService { get; set; }

   
            public string SubActivityLevelOid { get; set; }
     
            public string SubActivityOid { get; set; }

            public string ActivityOid { get; set; }
  
            public string EmployeeOid { get; set; }
         
            public string OrganizationOid { get; set; }
   
            public string FinanceYearOid { get; set; }
            
            public string UseNo { get; set; }
          
            public string UseDate { get; set; }
     
      //      public XPCollection<SupplierUseProductDetail> SupplierUseProductDetails { get; }
   
            public int ServiceCount { get; set; }
        }
    }
}