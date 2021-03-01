using Controllers;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Jwt.Models;

namespace WebApi.Jwt.Controllers.โควตา
{
    public class Delete_ListController : ApiController
    {
        private string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        [AllowAnonymous]
        // GET: api/ReportCategories
        [Route("DeleteReportsByRID")]
        [HttpGet]
        public HttpResponseMessage DeleteReportsByRID()  // IEnumerable(Of String)
        {
            try
            {
                string Oid = "";

                if (HttpContext.Current.Request.Form["Oid   "].ToString() != null)
                {
                    Oid = HttpContext.Current.Request.Form["Oid "].ToString();
                }
                List<DisplayOnApp> objOnApp = new List<DisplayOnApp>();
                DataSet Ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "sp_DeleteList_UseAnimal",
                    new SqlParameter("@Oid", Oid));
                DataTable Dt = Ds.Tables[0];
                if (Dt.Rows.Count > 0)
                {
                    DeleteReportsByRID_Model delete = new DeleteReportsByRID_Model();
                    delete.Status = "1";
                    delete.Message = "ลบข้อมูลสำเร็จ";

                    return Request.CreateResponse(HttpStatusCode.OK, delete);
                }
                else
                {
                    ResponceMessage objErr = new ResponceMessage();
                    objErr.code = "0";
                    objErr.message = "ลบข้อมูลไม่สำเร็จ";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, objErr);
                }
            }
            catch (Exception ex)
            {
                //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.status = "ผิดพลาด";
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }

        //public HttpResponseMessage DeleteReportsByLinq()  // IEnumerable(Of String)
        //{
        //    try
        //    {
        //        string ReportId = "";

        //        if (HttpContext.Current.Request.Form["ReportId"].ToString() != null)
        //        {
        //            ReportId = HttpContext.Current.Request.Form["ReportId"].ToString();

        //        }

        //        else
        //        {
        //            ResponceMessage objErr = new ResponceMessage();
        //            objErr.code = "0";
        //            objErr.message = "ลบข้อมูลไม่สำเร็จ";
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, objErr);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //Error case เกิดข้อผิดพลาด
        //        UserError err = new UserError();
        //        err.status = "ผิดพลาด";
        //        err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

        //        err.message = ex.Message;
        //        //  Return resual
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
        //    }
        //}
    }
}