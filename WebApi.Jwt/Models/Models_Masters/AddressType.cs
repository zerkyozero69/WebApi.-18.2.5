using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models.Models_Masters
{
    public class AddressType_Model
    {
        public string AddressTypeName { get; set; }
        public bool IsActive { get; set; }
    }
    public class xxx
    {
    }
    public class DLDArea_Model
    {
        public string DLDAreaOid { get; set; }
        public string DLDAreaName { get; set; }

        public string OrganizeName { get; set; }
    }
    public class listdetail
    {

        public string OId { get; set; }
 

        public string DLDName { get; set; }
        public listdetail()
        {
            Detail = new List<listDLD>();
        }
        public List<listDLD> Detail { get; set; }
   


    }

    public class listDLD
    {
        public string ORGOid { get; set; }
        public string OrganizationName { get; set; }
    }
}