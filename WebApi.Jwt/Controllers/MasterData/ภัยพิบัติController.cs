using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using nutrition.Module;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Jwt.Models;
using static WebApi.Jwt.Controllers.MasterData.ภัยพิบัติ;

namespace WebApi.Jwt.Controllers.MasterData
{
    public class ภัยพิบัติController : ApiController
    {
        private string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        [AllowAnonymous]
        [HttpGet]
        [Route("Disaster")]
        public HttpResponseMessage DisasterList()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.Activity));
                List<Disaster_Model> list = new List<Disaster_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<Activity> collection = ObjectSpace.GetObjects<Activity>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1 ", null));
                foreach (Activity row in collection)
                {
                    Disaster_Model model = new Disaster_Model();
                    model.Oid = row.Oid.ToString();
                    model.ObjectTypeOid = row.ObjectTypeOid.Oid.ToString();
                    model.ObjectTypeName = row.ObjectTypeOid.ObjectTypeName;
                    model.ActivityName = row.ActivityName;
                    model.IsActive = row.IsActive;
                    list.Add(model);
                }
                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }
    }
}