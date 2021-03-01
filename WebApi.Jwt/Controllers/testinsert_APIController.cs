using System;
using System.Transactions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Xml;
using System.Data.SqlClient;
using System.Configuration;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using Microsoft.ApplicationBlocks.Data;
using DevExpress.Persistent.BaseImpl;
using System.Text;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using System.Web.Http;
using System.Web;
using static WebApi.Jwt.helpclass.helpController;
using static WebApi.Jwt.Models.user;
using System.Data;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base.Security;
using DevExpress.ExpressApp.Security;
using WebApi.Jwt.Models;
using WebApi.Jwt.Filters;
using WebApi.Jwt.helpclass;
using NTi.CommonUtility;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static WebApi.Jwt.Models.Farmerinfo;
using nutrition.Module;
using nutrition.Module.EmployeeAsUserExample.Module.BusinessObjects;

namespace WebApi.Jwt.Controllers
{
    public class testinsert_APIController : ApiController
    {


        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        [AllowAnonymous]
        [HttpGet]
        [Route("test/insert2")]
        public IHttpActionResult insertsdemo_Webapi()
        {
            string UserName = "";
        
            XPObjectSpaceProvider osProvider = new XPObjectSpaceProvider(scc, null);
            IObjectSpace objectSpace = osProvider.CreateObjectSpace();
            UserInfo ObjInUser;
            XafTypesInfo.Instance.RegisterEntity(typeof(UserInfo));


            ObjInUser = objectSpace.CreateObject<UserInfo>();
            ObjInUser.UserName = UserName;
           
            objectSpace.CommitChanges();
            return Ok(ObjInUser);

        }
    }


    
        
}
