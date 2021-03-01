using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models.Models_Masters
{
    public class Objective_Used_Model
    {
        public string Oid { get; set; }
        public string ObjectTypeName { get; set; }    
      
    }
    public class ProductionObjective_Model
    {
        public string  ProductObjectiveName { get; set; }
       
        public bool IsActive { get; set; }

    }
}