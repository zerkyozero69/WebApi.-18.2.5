using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using nutrition.Module;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Jwt.Models;

namespace WebApi.Jwt.Controllers.MasterData
{
    public class Budget_YearController : ApiController
    {
        private string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        /// <summary>
        /// รายละเอียดงบประมาณ
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("Budget/Year")]
        public HttpResponseMessage GetBudget_Year()
        {
            try

            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(FinanceYear));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<FinanceYear> collection = ObjectSpace.GetObjects<FinanceYear>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1", null));
                var Qurey = (from Q in collection orderby Q.YearName descending select Q);
                if (collection.Count > 0)
                {
                    List<FinanceYearModel> list = new List<FinanceYearModel>();
                    foreach (FinanceYear row in Qurey)
                    {
                        FinanceYearModel Finance = new FinanceYearModel();
                        Finance.FinanceYearOid = row.Oid.ToString();
                        Finance.FinanceYear = row.YearName;
                        list.Add(Finance);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, list);
                }
                else
                {
                    UserError err = new UserError();
                    err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "No data";
                    //  Return resual
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
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

        /// <summary>
        /// รายละเอียด งบประมาณ
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("Budget/info")]
        public HttpResponseMessage GetBudget_Info()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(BudgetSource));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<BudgetSource> collection = ObjectSpace.GetObjects<BudgetSource>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1", null));

                if (collection.Count > 0)
                {
                    List<BudgetSourceModel> list = new List<BudgetSourceModel>();
                    foreach (BudgetSource row in collection)
                    {
                        BudgetSourceModel budget_type = new BudgetSourceModel();
                        budget_type.BudgetSourceOid = row.Oid;
                        budget_type.BudgetName = row.BudgetName;
                        list.Add(budget_type);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, list);
                }
                else
                {
                    UserError err = new UserError();
                    err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "No data";
                    //  Return resual
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
            }
            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            finally
            {
            }
        }
    }
}