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
using Controllers;
using System.Data.SqlClient;
using System.Web;

namespace WebApi.Jwt.Controllers
{
    public class LoadReportByRIdController : ApiController
    {
        string strConn = ConfigurationManager.ConnectionStrings["StrApiDb"].ConnectionString;

        [AllowAnonymous]
        // GET: api/ReportCategories
        [Route("LoadReportByRId")]
        [HttpGet]
        public HttpResponseMessage GetLoadReport()  // IEnumerable(Of String)
        {
            List<SP_LoadReportByRId_Model> objLoadReport = new List<SP_LoadReportByRId_Model>();
            DataSet Ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "SP_LoadReportByRId");
            DataTable Dt = Ds.Tables[0];
            if (Dt.Rows.Count > 0)
            {
                foreach (DataRow obj in Dt.Rows)
                {
                    SP_LoadReportByRId_Model Lis = new SP_LoadReportByRId_Model();

                    Lis.ReportId = obj["ReportId"].ToString();
                    Lis.ReportName = obj["ReportName"].ToString();
                    Lis.ReportDescription = obj["ReportDescription"].ToString();
                    Lis.ReportData = obj["ReportData"].ToString();
                    Lis.IsPublish = obj["IsPublish"].ToString();
                    Lis.PublishDate = obj["PublishDate"].ToString();
                    Lis.PublishByUsername = obj["PublishByUsername"].ToString();

                    objLoadReport.Add(Lis);
                }
                return Request.CreateResponse(HttpStatusCode.OK, objLoadReport);
            }
            else
            {
                ResponceMessage objErr = new ResponceMessage();
                objErr.code = "1";
                objErr.message = "ไม่มีข้อมูล";
                return Request.CreateResponse(HttpStatusCode.BadRequest, objErr);
            }
        }
        [AllowAnonymous]
        // GET: api/ReportCategories
        [Route("LoadReportPermissionBy/CategoryId")]
        [HttpGet]
        public HttpResponseMessage LoadReportPermissionBy()  // IEnumerable(Of String)
        {
            string CategoryId  = HttpContext.Current.Request.Form["CategoryId"].ToString();
            string RoleId = HttpContext.Current.Request.Form["RoleId"].ToString();
            List<SP_LoadReportByRId_Model> objLoadReport = new List<SP_LoadReportByRId_Model>();
            DataSet Ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "SP_LoadReportPermissionBy",
                new SqlParameter("@CategoryId", CategoryId)
                ,new SqlParameter("@RoleId",RoleId));
            DataTable Dt = Ds.Tables[0];
            if (Dt.Rows.Count > 0)
            {
                foreach (DataRow obj in Dt.Rows)
                {
                    SP_LoadReportByRId_Model Lis = new SP_LoadReportByRId_Model();

                    Lis.ReportId = obj["ReportId"].ToString();

                    Lis.ReportName = obj["ReportName"].ToString();
                    
                    objLoadReport.Add(Lis);
                }
                return Request.CreateResponse(HttpStatusCode.OK, objLoadReport);
            }
            else
            {
                ResponceMessage objErr = new ResponceMessage();
                objErr.code = "1";
                objErr.message = "ไม่มีข้อมูล";
                return Request.CreateResponse(HttpStatusCode.BadRequest, objErr);
            }

        }

        [AllowAnonymous]
        // GET: api/ReportCategories
        [Route("LoadReportsAll")]
        [HttpGet]
        public HttpResponseMessage LoadReportsAll()  // IEnumerable(Of String)
        {
            List<LoadReportsAll_Model> objLoadReport = new List<LoadReportsAll_Model>();
            DataSet Ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "SP_LoadReportsAll");
            DataTable Dt = Ds.Tables[0];
            if (Dt.Rows.Count > 0)
            {
                foreach (DataRow obj in Dt.Rows)
                {
                    LoadReportsAll_Model Lis = new LoadReportsAll_Model();

                    Lis.ReportId = obj["ReportId"].ToString();

                    Lis.ReportName = obj["ReportName"].ToString();
                    Lis.CategoryId = obj["CategoryId"].ToString();
                    Lis.CategoryName = obj["CategoryName"].ToString();
                    Lis.ReportDescription = obj["ReportDescription"].ToString();
                    Lis.ReportData = obj["ReportData"].ToString();
                    Lis.IsPublish = obj["IsPublish"].ToString();
                    Lis.PublishDate = obj["PublishDate"].ToString();
                    Lis.PublishByUsername = obj["PublishByUsername"].ToString();
                    Lis.DisplayOnAppId = obj["DisplayOnAppId"].ToString();
                    Lis.DisplayOnAppName = obj["DisplayOnAppName"].ToString();


                    objLoadReport.Add(Lis);
                }
                return Request.CreateResponse(HttpStatusCode.OK, objLoadReport);
            }
            else
            {
                ResponceMessage objErr = new ResponceMessage();
                objErr.code = "1";
                objErr.message = "ไม่มีข้อมูล";
                return Request.CreateResponse(HttpStatusCode.BadRequest, objErr);
            }

        }

        
        
    }
}
