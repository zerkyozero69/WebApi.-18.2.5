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

namespace WebApi.Jwt.Controllers.MasterData
{
    public class PositionController : ApiController
    {
        private string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        [AllowAnonymous]
        [HttpPost]
        [Route("Position")]/// เรียกตำแหน่งเจ้าหน้าที่
        public HttpResponseMessage loadPosition()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.Position));
                List<Position_Model> list = new List<Position_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<Position> collection = ObjectSpace.GetObjects<Position>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1", null));
                foreach (Position row in collection)
                {
                    Position_Model model = new Position_Model();
                    model.PositionName = row.PositionName;
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
                //return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, err);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Position/Level")] /// เรียกลำดับชั้นของตำแหน่ง
        public HttpResponseMessage loadPosition_Tier()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.PositionLevel));
                List<PositionLevel_Model> list = new List<PositionLevel_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<PositionLevel> collection = ObjectSpace.GetObjects<PositionLevel>(CriteriaOperator.Parse("  GCRecord is null and IsActive = 1", null));
                foreach (PositionLevel row in collection)
                {
                    PositionLevel_Model model = new PositionLevel_Model();
                    model.PositionLevelName = row.PositionLevelName;
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