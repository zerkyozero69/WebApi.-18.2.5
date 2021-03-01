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
    public class DisplayOnAppController : ApiController
    {
         string strConn = ConfigurationManager.ConnectionStrings["StrApiDb"].ConnectionString;

        [AllowAnonymous]
        // GET: api/ReportCategories
        [Route("DisplayOnApp/GetDisplayOnAppValues")]
        [HttpGet]
        public HttpResponseMessage GetDisplayOnAppValues()  // IEnumerable(Of String)
        {
            List<DisplayOnApp> objOnApp = new List<DisplayOnApp>();
            DataSet Ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "SP_LoadDisplayOnAppAll");
            DataTable Dt = Ds.Tables[0];
            if (Dt.Rows.Count > 0)
            {
                foreach (DataRow obj in Dt.Rows)
                {
                    DisplayOnApp Lis = new DisplayOnApp();

                    Lis.DisplayOnAppId = obj["DisplayOnAppId"].ToString();

                    Lis.DisplayOnAppName = obj["DisplayOnAppName"].ToString();

                    objOnApp.Add(Lis);
                }



                return Request.CreateResponse(HttpStatusCode.OK, objOnApp);
            }
            else
            {
                ResponceMessage objErr = new ResponceMessage();
                objErr.code = "1";
                objErr.message = "ไม่มีข้อมูล";
                return Request.CreateResponse(HttpStatusCode.BadRequest, objErr);
            }
        }

        // GET: api/DisplayOnApp/5
        public string GetValue(int id)
        {
            return "value";
        }

        // POST: api/DisplayOnApp
        public void PostValue([FromBody()] string value)
        {
        }

        // PUT: api/DisplayOnApp/5
        public void PutValue(int id, [FromBody()] string value)
        {
        }

        // DELETE: api/DisplayOnApp/5
        public void DeleteValue(int id)
        {
        }
    }
}
