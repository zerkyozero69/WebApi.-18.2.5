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
using static WebApi.Jwt.Models.MasterData;

namespace WebApi.Jwt.Controllers.MasterData
{
    public class GenderController : ApiController
    {
        private string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        [AllowAnonymous]
        [HttpGet]
        [Route("Gender")]
        public HttpResponseMessage get_Gender()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(Gender));
                List<Gender_Model> list = new List<Gender_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<Gender> collection = ObjectSpace.GetObjects<Gender>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1", null));
                foreach (Gender row in collection)
                {
                    Gender_Model model = new Gender_Model();
                    model.Oid = row.Oid.ToString();
                    model.GenderName = row.GenderName;
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