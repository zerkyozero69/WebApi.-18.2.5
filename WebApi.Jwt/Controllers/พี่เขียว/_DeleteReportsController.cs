using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;
using System.Data;
using System.Web;
using Controllers;
using WebApi.Jwt.Models;
using System.Data.SqlClient;

namespace WebApi.Jwt.Controllers.พี่เขียว
{
    public class _DeleteReportsController : ApiController
    {
        string strConn = ConfigurationManager.ConnectionStrings["StrApiDb"].ConnectionString;

        [AllowAnonymous]
        // GET: api/ReportCategories
        [Route("DeleteReportsByRID")]
        [HttpGet]
        public HttpResponseMessage DeleteReportsByRID()  // IEnumerable(Of String)
        {
            try
            {
                string ReportId = "";

                if (HttpContext.Current.Request.Form["ReportId"].ToString() != null)
                {

                    ReportId = HttpContext.Current.Request.Form["ReportId"].ToString();

                }
                List<DisplayOnApp> objOnApp = new List<DisplayOnApp>();
                DataSet Ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "SP_DeleteReportsByRID",
                    new SqlParameter("@ReportId", ReportId));
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
        [AllowAnonymous]
        // GET: api/ReportCategories
        [Route("LoadReportBy/RoleId")]
        [HttpGet]
        public HttpResponseMessage LoadReportByRoleId()  // IEnumerable(Of String)
        {

            string ReportId = "";

            if (HttpContext.Current.Request.Form["ReportId"].ToString() != null)
            {

                ReportId = HttpContext.Current.Request.Form["ReportId"].ToString();

            }
            List<LoadReportByRoleId_Model> objOnApp = new List<LoadReportByRoleId_Model>();
            DataSet Ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "SP_LoadReportByRoleId"
                , new SqlParameter("@ReportId", ReportId));
            DataTable Dt = Ds.Tables[0];
            if (Dt.Rows.Count > 0)
            {
                foreach (DataRow obj in Dt.Rows)
                {
                    LoadReportByRoleId_Model item = new LoadReportByRoleId_Model();
                    item.RoleId = obj["RoleId"].ToString();
                    item.RoleName = obj["RoleName"].ToString();
                    item.Description = obj["Description"].ToString();
                    item.FullRole = obj["FullRole"].ToString();
                    objOnApp.Add(item);


                }

       return Request.CreateResponse(HttpStatusCode.OK, objOnApp);
            }
            else
            {
                ResponceMessage objErr = new ResponceMessage();
                objErr.code = "0";
                objErr.message = "NoData";
                return Request.CreateResponse(HttpStatusCode.BadRequest, objErr);
            }


        }
        [AllowAnonymous]
        // GET: api/ReportCategories
        [Route("LoadReportAllRoleId")]
        [HttpGet]
        public HttpResponseMessage LoadReportAllRole()  // IEnumerable(Of String)
        {

      
            List<LoadReportByRoleId_Model> objOnApp = new List<LoadReportByRoleId_Model>();
            DataSet Ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "SP_LoadReportAllRole");
            DataTable Dt = Ds.Tables[0];
            if (Dt.Rows.Count > 0)
            {
                foreach (DataRow obj in Dt.Rows)
                {
                    LoadReportByRoleId_Model item = new LoadReportByRoleId_Model();
                    item.RoleId = obj["RoleId"].ToString();
                    item.RoleName = obj["RoleName"].ToString();
                    item.Description = obj["Description"].ToString();
                    item.FullRole = obj["FullRole"].ToString();
                    objOnApp.Add(item);

                }

                return Request.CreateResponse(HttpStatusCode.OK, objOnApp);
            }
            else
            {
                ResponceMessage objErr = new ResponceMessage();
                objErr.code = "0";
                objErr.message = "NoData";
                return Request.CreateResponse(HttpStatusCode.BadRequest, objErr);
            }


        }
        [AllowAnonymous]
        // GET: api/ReportCategories
        [Route("Update_ReportPermission")]
        [HttpPost]
        public HttpResponseMessage Update_ReportPermission()  // IEnumerable(Of String)
        {
            string ReportId = " ";
            string RoleId = "";
            if (HttpContext.Current.Request.Form["ReportId"].ToString() != null)
            {

                ReportId = HttpContext.Current.Request.Form["ReportId"].ToString();

            }
            RoleId = HttpContext.Current.Request.Form["RoleId"].ToString();

            List<LoadReportByRoleId_Model> objOnApp = new List<LoadReportByRoleId_Model>();
            DataSet Ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "SP_Update_ReportPermission",
                new SqlParameter("@ReportId", ReportId),new SqlParameter("@RoleId", RoleId));
            DataTable dt = new DataTable();
            dt = Ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
                return Request.CreateResponse(HttpStatusCode.OK, rows);
            }

            else
            {
                ResponceMessage objErr = new ResponceMessage();
                objErr.code = "0";
                objErr.message = "NoData";
                return Request.CreateResponse(HttpStatusCode.BadRequest, "NoData");
            }
        }

    }
}

