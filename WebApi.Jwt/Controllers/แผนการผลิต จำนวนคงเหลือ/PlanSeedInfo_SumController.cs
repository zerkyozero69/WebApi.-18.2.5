using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using nutrition.Module;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Jwt.Models;
using WebApi.Jwt.Models.แผนการผลิตโมเดล;

namespace WebApi.Jwt.Controllers.แผนการผลิต
{
    public class PlanSeedInfo_SumController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
        /// <summary>
        /// Org_Oid , FinanceYearOid
        /// </summary>
        /// <returns></returns>
        //[AllowAnonymous]
        //[HttpPost]
        //[Route("PlanSeedInfo_Detail/Sum")]
        //public HttpResponseMessage PlanSeedInfo_Detail()
        //{
        //    try
        //    {
        //        string org_oid = HttpContext.Current.Request.Form["Org_Oid"].ToString();
        //        string FinanceYearOid = HttpContext.Current.Request.Form["FinanceYearOid"].ToString(); //รับ=1/ส่ง=2

        //        if (org_oid != "" && FinanceYearOid != "")
        //        {
        //            XpoTypesInfoHelper.GetXpoTypeInfoSource();
        //            XafTypesInfo.Instance.RegisterEntity(typeof(PlanSeedInfo));
        //            XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
        //            List<PlanSeedInfo_Model> list = new List<PlanSeedInfo_Model>();
        //            IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
        //            IList<PlanSeedInfo> collection = ObjectSpace.GetObjects<PlanSeedInfo>(CriteriaOperator.Parse("GCRecord is null and FinanceYearOid.Oid = ?  and OrganizationOid.Oid = ?", FinanceYearOid, org_oid));
        //            if (collection.Count > 0)
        //            {
        //                foreach (PlanSeedInfo row in collection)
        //                {
        //                    PlanSeedInfo_Model item = new PlanSeedInfo_Model();
        //                    item.StockDate = row.StockDate.ToString("dd/MM/yyyy");
        //                    item.OrganizationOid = row.OrganizationOid.SubOrganizeName;
        //                    item.FinanceYearOid = row.FinanceYearOid.YearName;
        //                    item.BudgetSourceOid = row.BudgetSourceOid.BudgetName;
        //                    item.AnimalSeedOid = row.AnimalSeedOid.SeedName;
        //                    item.AnimalSeedLevelOid = row.AnimalSeedLevelOid.SeedLevelName;
        //                    item.QTY = sum+row.QTY;
        //                    if (row.UnitOid == null)
        //                    {
        //                        item.Unit = "ไม่พบข้อมูล";
        //                    }
        //                    else
        //                    {
        //                        item.Unit = row.UnitOid.UnitName;
        //                    }
                         
        //                    item.Remark = row.Remark;
        //                    list.Add(item);
        //                }
        //                return Request.CreateResponse(HttpStatusCode.OK, list);
        //            }
        //            else
        //            {
        //                UserError err = new UserError();
        //                err.status = "false";
        //                err.code = "0";
        //                err.message = "กรุณาใส่ข้อมูล FinanceYearOid ให้เรียบร้อย";
        //                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
        //            }

        //        }
       
        //        else
        //        {
        //            UserError err = new UserError();
        //            err.status = "false";
        //            err.code = "0";
        //            err.message = "กรุณาใส่ข้อมูล FinanceYearOid ให้เรียบร้อย";
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, err);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        UserError err = new UserError();
        //        err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
        //        err.message = ex.Message;
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
        //    }
        //}
    }
}


