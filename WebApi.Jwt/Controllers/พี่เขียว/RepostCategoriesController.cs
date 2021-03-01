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

namespace Controllers
{
    public class ReportCategoriesController : ApiController
    {
        private string strConn = ConfigurationManager.ConnectionStrings["StrApiDb"].ConnectionString;

        // GET: api/ReportCategories
        [AllowAnonymous]
        [Route("ReportCategories/GetReportCategoryAllValues")]
        [HttpGet]
        public HttpResponseMessage GetReportCategoryAllValues()  // IEnumerable(Of String)
        {
            List<ReportCategories> objCat = new List<ReportCategories>();
            DataSet Ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "SP_LoadReportCategoriesAll");
            DataTable Dt = Ds.Tables[0];
            if (Dt.Rows.Count > 0)
            {

                // Dim objtran As New Roles

                foreach (DataRow obj in Dt.Rows)
                {
                    ReportCategories Lis = new ReportCategories();

                    Lis.CategoryId = obj["CategoryId"].ToString();

                    Lis.CategoryName = obj["CategoryName"].ToString();
                  //  Lis.CategoryParent = obj["CategoryParent"].ToString();
                    objCat.Add(Lis);
                }



                return Request.CreateResponse(HttpStatusCode.OK, objCat);
            }
            else
            {
                ResponceMessage objErr = new ResponceMessage();
                objErr.code = "1";
                objErr.message = "ไม่มีข้อมูล";
                return Request.CreateResponse(HttpStatusCode.BadRequest, objErr);
            }
        }

        // GET: api/ReportCategories/5
        public string GetValue(int id)
        {
            return "value";
        }

        // POST: api/ReportCategories
        public void PostValue([FromBody()] string value)
        {
        }

        // PUT: api/ReportCategories/5
        public void PutValue(int id, [FromBody()] string value)
        {
        }

        // DELETE: api/ReportCategories/5
        public void DeleteValue(int id)
        {
        }
    }
}
