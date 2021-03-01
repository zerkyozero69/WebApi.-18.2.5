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
    public class Product_unitController : ApiController
    {
        private string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        /// <summary>
        /// หน่วยวัด
        /// </summary>
        /// <returns></returns>
        //[JwtAuthentication]
        [AllowAnonymous]
        [HttpGet]
        [Route("Product_unit")]
        public HttpResponseMessage get_Product_unit()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.Unit));
                List<Unit_Model> list = new List<Unit_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<Unit> collection = ObjectSpace.GetObjects<Unit>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1 ", null));
                foreach (Unit row in collection)
                {
                    Unit_Model model = new Unit_Model();
                    model.UnitOid = row.Oid.ToString();
                    model.UnitCode = row.UnitCode;
                    model.UnitName = row.UnitName;
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

        [AllowAnonymous]
        [HttpGet]
        [Route("Package_List")]
        public HttpResponseMessage loadPackage_Info() ///ข้อมูลบรรจุภัณท์
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.Package));
                List<Package_Model> list = new List<Package_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<Package> collection = ObjectSpace.GetObjects<Package>(CriteriaOperator.Parse("  GCRecord is null and IsActive = 1 ", null));
                foreach (Package row in collection)
                {
                    Package_Model model = new Package_Model();
                    model.PackageOid = row.Oid.ToString();
                    model.PackageName = row.PackageName;
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