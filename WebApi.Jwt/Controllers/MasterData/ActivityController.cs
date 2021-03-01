using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using nutrition.Module;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Jwt.Models;
using WebApi.Jwt.Models.Models_Masters;

namespace WebApi.Jwt.Controllers.MasterData
{
    public class ActivityController : ApiController
    {
        private string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        /// <summary>
        /// เรียกกิจกรรม ภัยพิบัติ
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("Activity/List")]
        public HttpResponseMessage Activity_info()

        {
            try
            {
                string ObjectTypeOid = "b100c7c1-4755-4af0-812e-3dd6ba372d45"; //เพื่อช่วยเหลือภัยพิบัติ

                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(Activity));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                List<activity_Model> list = new List<activity_Model>();
                List<activityDetails_Model> detail = new List<activityDetails_Model>();
                IList<Activity> collection = ObjectSpace.GetObjects<Activity>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1 and MasterActivity = '" + ObjectTypeOid + "' ", null));
                {
                    if (collection.Count > 0)

                    {
                        foreach (Activity row in collection)
                        {
                            activity_Model Item = new activity_Model();
                            Item.ActivityNameOid = row.Oid.ToString();
                            Item.ActivityName = row.ActivityName;
                            Item.ObjectTypeOid = row.ObjectTypeOid.Oid.ToString();
                            Item.ObjectTypeName = row.ObjectTypeOid.ObjectTypeName;
                            if (row.MasterActivity != null)
                            {
                                Item.MasterActivity = row.MasterActivity.Oid.ToString();
                            }

                            list.Add(Item);
                           
                        }
                    }
                    else
                    {          //invalid
                        UserError err = new UserError();
                        err.status = "false";
                        err.code = "0";
                        err.message = "กรุณาใส่ข้อมูลให้เรียบร้อย";
                        return Request.CreateResponse(HttpStatusCode.NotFound, err);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, list);
                }
                
            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }

        /// <summary>
        /// เรียกประเภท รายเดียว-หน่วยงาน
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("Activity/Sup")]
        public HttpResponseMessage Activity_Sup()

        {
            try
            {
                string ObjectTypeOid = "a29d77a9-4bcb-4774-9744-ff97a373353e";

                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(Activity));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                List<activitySub_Model> list = new List<activitySub_Model>();
                List<activityDetails_Model> detail = new List<activityDetails_Model>();
                IList<Activity> collection = ObjectSpace.GetObjects<Activity>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1 and MasterActivity = '" + ObjectTypeOid + "' ", null));
                {
                    if (collection.Count > 0)

                    {
                        foreach (Activity row in collection)
                        {
                            activitySub_Model Item = new activitySub_Model();
                            Item.SubActivityLevelOid = row.Oid.ToString();
                            Item.SubActivityLevelName = row.ActivityName;

                            list.Add(Item);
                        }
                    }
                    else
                    {          //invalid
                        UserError err = new UserError();
                        err.status = "false";
                        err.code = "0";
                        err.message = "กรุณาใส่ข้อมูลให้เรียบร้อย";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, list);
                }
            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }
        /// <summary>
        /// เรียกวัตุประสงค์การใช้
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("ObjectType/List")]
        public HttpResponseMessage ObjectType()

        {
            try
            {
                string ObjectTypeOid = HttpContext.Current.Request.Form["ObjectTypeOid"].ToString();

                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(ObjectType));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                List<Objective_Used_Model> list = new List<Objective_Used_Model>();
                List<activityDetails_Model> detail = new List<activityDetails_Model>();
                IList<ObjectType> collection = ObjectSpace.GetObjects<ObjectType>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1   ", null));
                {
                    if (collection.Count > 0)

                    {
                        foreach (ObjectType row in collection)
                        {
                            Objective_Used_Model Item = new Objective_Used_Model();
                            Item.Oid = row.Oid.ToString();
                            Item.ObjectTypeName = row.ObjectTypeName;
                            list.Add(Item);
                        }
                    }
                    else
                    {          //invalid
                        UserError err = new UserError();
                        err.status = "false";
                        err.code = "0";
                        err.message = "กรุณาใส่ข้อมูลให้เรียบร้อย";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, list);
                }
            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            finally
            {
                SqlConnection.ClearAllPools();
            }
        }
    }
}