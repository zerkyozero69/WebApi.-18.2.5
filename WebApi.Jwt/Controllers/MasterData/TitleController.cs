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
    public class TitleController : ApiController
    {
        private string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        [AllowAnonymous]
        [HttpGet]
        [Route("Titlename/titlename")]
        public HttpResponseMessage loadTitleName()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.Title));
                List<TitleName_Model> list = new List<TitleName_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<Title> collection = ObjectSpace.GetObjects<Title>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1 ", null));
                foreach (Title row in collection)
                {
                    TitleName_Model model = new TitleName_Model();
                    model.Oid = row.Oid.ToString();
                    model.SubTitleName = row.SubTitleName;
                    model.TitleName = row.TitleName;
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