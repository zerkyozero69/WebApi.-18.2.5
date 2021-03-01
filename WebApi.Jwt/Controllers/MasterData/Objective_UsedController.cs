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
using WebApi.Jwt.Models.Models_Masters;

namespace WebApi.Jwt.Controllers.MasterData
{
    public class Objective_UsedController : ApiController
    {
        private string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        [AllowAnonymous]
        [HttpGet]
        [Route("Objective_Used")]
        public HttpResponseMessage loadObjective_Used()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.ObjectType));
                List<Objective_Used_Model> list = new List<Objective_Used_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<ObjectType> collection = ObjectSpace.GetObjects<ObjectType>(CriteriaOperator.Parse("  GCRecord is null and IsActive = 1", null));
                foreach (ObjectType row in collection)
                {
                    Objective_Used_Model model = new Objective_Used_Model();
                    model.ObjectTypeName = row.ObjectTypeName;
                    model.Oid = row.Oid.ToString();
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

        [AllowAnonymous]
        [HttpGet]
        [Route("Objective_Usedinfo")]
        public HttpResponseMessage get_ProductionObjective()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.ProductionObjective));
                List<ProductionObjective_Model> list = new List<ProductionObjective_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<ProductionObjective> collection = ObjectSpace.GetObjects<ProductionObjective>(CriteriaOperator.Parse("  GCRecord is null and IsActive = 1", null));
                foreach (ProductionObjective row in collection)
                {
                    ProductionObjective_Model model = new ProductionObjective_Model();
                    model.ProductObjectiveName = row.ProductObjectiveName;
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