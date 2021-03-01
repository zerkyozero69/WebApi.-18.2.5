using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Controllers.MasterData
{
    public class ภัยพิบัติ
    {
        public class Disaster_Model
        {
            public string Oid { get; set; }
            public string ActivityName { get; set; }

            public string ObjectTypeOid { get; set; }
            public string ObjectTypeName { get; set; }
            public bool IsActive { get; set; }

            public string Remark { get; set; }

            public string MasterActivity { get; set; }

          //  public XPCollection<Activity> ActivityDetails { get; }
        }
    }
}