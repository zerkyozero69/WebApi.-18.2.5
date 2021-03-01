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
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using Newtonsoft.Json;
using System.Configuration;
using System.Data;
using System.Web;

namespace Controllers
{
    public class PublishReportController : ApiController
    {
        private string strConn = ConfigurationManager.ConnectionStrings["StrApiDb"].ConnectionString;
        [AllowAnonymous]
        // GET: api/PublishReport
        public IEnumerable<string> GetValues()
        {
            return new string[] { "value1", "value2" };
        }
        [AllowAnonymous]
        // GET: api/PublishReport/5
        public string GetValue(int id)
        {
            return "value";
        }
        [AllowAnonymous]

        [Route("PublishReport/AddpublishReport")]
        [HttpPost]
        public HttpResponseMessage AddpublishReport()
        {
            try
            {
                SqlConnection objConn = new SqlConnection(strConn);
                if (objConn.State != ConnectionState.Open)
                    objConn.Open();

                PublishReport objPReport = new PublishReport();
                objPReport.ReportName = HttpContext.Current.Request.Form["ReportName"].ToString();
                objPReport.ReportDescription = HttpContext.Current.Request.Form["ReportDescription"].ToString();
                objPReport.ReportData = HttpContext.Current.Request.Form["ReportData"].ToString();
                objPReport.PublishByUsername = HttpContext.Current.Request.Form["PublishByUsername"].ToString();
                objPReport.IsPublish = Convert.ToBoolean(HttpContext.Current.Request.Form["IsPublish"]);
                objPReport.ReportId = HttpContext.Current.Request.Form["ReportId"];
                objPReport.CategoryId = HttpContext.Current.Request.Form["CategoryId"];
                objPReport.DisplayOnAppId = HttpContext.Current.Request.Form["DisplayOnAppId"];

                SqlParameter[] adparam = new SqlParameter[8];
                adparam[0] = new SqlParameter("@ReportName", objPReport.ReportName);
                adparam[1] = new SqlParameter("@ReportDescription", objPReport.ReportDescription);
                adparam[2] = new SqlParameter("@ReportData", objPReport.ReportData);
                adparam[4] = new SqlParameter("@PublishByUsername", objPReport.PublishByUsername);
                adparam[5] = new SqlParameter("@IsPublish", objPReport.IsPublish);
                adparam[6] = new SqlParameter("@CategoryId", objPReport.CategoryId);
                adparam[7] = new SqlParameter("@DisplayOnAppId", objPReport.DisplayOnAppId);

                var getRoleId = SqlHelper.ExecuteScalar(strConn, CommandType.StoredProcedure, "SP_Insert_Reports", adparam);
                objPReport.ReportId = getRoleId.ToString();

                return Request.CreateResponse(HttpStatusCode.OK, objPReport);
            }
            catch (Exception ex)
            {
                ResponceMessage objErr = new ResponceMessage();
                objErr.code = "1";
                objErr.message = "ไม่มีข้อมูล";
                return Request.CreateResponse(HttpStatusCode.BadRequest, objErr);
            }
        }

        [AllowAnonymous]
        [Route("ReportPermission/AddReportPermission")]
        [HttpPost]
        public HttpResponseMessage AddReportPermission()
        {
            try
            {
                SqlConnection objConn = new SqlConnection(strConn);
                if (objConn.State != ConnectionState.Open)
                    objConn.Open();

                ReportPermission objPReport = new ReportPermission();
                objPReport.ReportId = System.Web.HttpContext.Current.Request.Form["ReportId"].ToString();
                objPReport.RoleId = System.Web.HttpContext.Current.Request.Form["RoleId"].ToString();



                SqlParameter[] adparam = new SqlParameter[2];
                adparam[0] = new SqlParameter("@ReportId", objPReport.ReportId);
                adparam[1] = new SqlParameter("@RoleId", objPReport.RoleId);


                SqlHelper.ExecuteNonQuery(strConn, CommandType.StoredProcedure, "SP_Insert_ReportPermission", adparam);

                return Request.CreateResponse(HttpStatusCode.OK, objPReport);
            }
            catch (Exception ex)
            {
                ResponceMessage objErr = new ResponceMessage();
                objErr.code = "1";
                objErr.message = "ไม่มีข้อมูล";
                return Request.CreateResponse(HttpStatusCode.BadRequest, objErr);
            }
        }


        // POST: api/PublishReport
        public void PostValue([FromBody()] string value)
        {
        }

        // PUT: api/PublishReport/5
        public void PutValue(int id, [FromBody()] string value)
        {
        }

        // DELETE: api/PublishReport/5
        public void DeleteValue(int id)
        {
        }
    }
}
