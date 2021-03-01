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
    public class RoleController : ApiController
    {
         string strConn = ConfigurationManager.ConnectionStrings["StrApiDb"].ConnectionString;
        [AllowAnonymous]
        // GET: api/Role
        [Route("Roles/GetValues")]
        [HttpGet]
        public HttpResponseMessage GetValues() // IEnumerable(Of String)
        {
            List<Roles> objRoles = new List<Roles>();
            DataSet Ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "Sp_getRoles");
            DataTable Dt = Ds.Tables[0];
            if (Dt.Rows.Count > 0)
            {

                // Dim objtran As New Roles

                foreach (DataRow obj in Dt.Rows)
                {
                    Roles Lis = new Roles();

                    Lis.RoleId = obj["RoleId"].ToString();

                    Lis.RoleName = obj["RoleName"].ToString();
                    Lis.Description = obj["Description"].ToString();
                    objRoles.Add(Lis);
                }



                return Request.CreateResponse(HttpStatusCode.OK, objRoles);
            }
            else
            {
                ResponceMessage objErr = new ResponceMessage();
                objErr.code = "1";
                objErr.message = "ไม่มีข้อมูล";
                return Request.CreateResponse(HttpStatusCode.BadRequest, objErr);
            }
        }


        // GET: api/Role/5
        public string GetValue(int id)
        {
            return "value";
        }

        // POST: api/Role
        public void PostValue([FromBody()] string value)
        {
        }

        // PUT: api/Role/5
        public void PutValue(int id, [FromBody()] string value)
        {
        }

        // DELETE: api/Role/5
        public void DeleteValue(int id)
        {
        }
    }
}
