using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models.Models_Masters
{
    public class ServiceType_Model
    {
        public string Oid { get; set; }
        public string ServiceTypeName { get; set; }
        public bool IsActive { get; set; }
        public string Remark { get; set; }
       // public ServiceType MasterServiceType { get; set; }
       // public XPCollection<ServiceType> ServiceTypeDetails { get; }

    }
}