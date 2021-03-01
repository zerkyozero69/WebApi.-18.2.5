using System;
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
using nutrition.Module;
using WebApi.Jwt.Models.Models_Masters;

namespace WebApi.Jwt.Controllers.MasterData
{

    public class Delivery_StatusController : ApiController
    {
        static string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
        ///// <summary>
        ///// รายละเอียดการส่ง
        ///// </summary>
        ///// <returns></returns>
        //[AllowAnonymous]
        //[HttpGet]
        //[Route("Delivery/Status")]

        //public HttpResponseMessage Delivery_Status()
        //{
        //    try
        //    {
        //        XpoTypesInfoHelper.GetXpoTypeInfoSource();
        //        XafTypesInfo.Instance.RegisterEntity(typeof(SendStatus));
        //        XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
        //        IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
        //        IList<SendStatus> collection = ObjectSpace.GetObjects<SendStatus>(CriteriaOperator.Parse("  GCRecord is null and IsActive = 1", null));

        //        if (collection.Count > 0)
        //        {
        //            List<SendStatusModel> list = new List<SendStatusModel>();
        //            foreach (SendStatus row in collection)
        //            {
        //                SendStatusModel SendStatus = new SendStatusModel();
        //                SendStatus.Oid = row.Oid;
        //                SendStatus.StatusName = row.StatusName;
        //                list.Add(SendStatus);
        //            }
        //            return Request.CreateResponse(HttpStatusCode.OK, list);
        //        }
        //        else
        //        {
        //            UserError err = new UserError();
        //            err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
        //            err.message = "No data";
        //            //  Return resual
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, err);
        //        }
        //    }
        //    catch (Exception ex)
        //    { //Error case เกิดข้อผิดพลาด
        //        UserError err = new UserError();
        //        err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
        //        err.message = ex.Message;
        //        //  Return resual
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
        //    }





        //}
    }
}
