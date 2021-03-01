using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using Microsoft.ApplicationBlocks.Data;
using nutrition.Module;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Jwt.Models;
using static WebApi.Jwt.Models.นับจำนวนกิจกรรม.Count_Approve;

namespace WebApi.Jwt.Controllers.นับจำนวนเมล็ดพันธุ์_เสีบงสัตว์_คงเหลือ
{
    public class Count_ApproveController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString;

        // POST: api/Count_Approve
        [AllowAnonymous]
        [HttpPost]
        [Route("Count_Number/Accepet")]
        public HttpResponseMessage Count_NumberAccepet()
        {
            try
            {
                string OrganizationOid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();
                SqlConnection objConn = new SqlConnection(scc);
                if (objConn.State != ConnectionState.Open)
                    objConn.Open();
                DataSet ds = SqlHelper.ExecuteDataset(scc,CommandType.StoredProcedure, "SP_CountNumber_Approve",new SqlParameter("@OrganizationOid", OrganizationOid));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(Organization));
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                    Organization Organization = ObjectSpace.FindObject<Organization>(CriteriaOperator.Parse("GCRecord is null and Oid =?", OrganizationOid));
                    Count_Number number = new Count_Number();
                    number.OrganizationName = Organization.OrganizeNameTH;
                    number.CountNumber = (int)ds.Tables[0].Rows[0]["Count_Total"];

                    directProvider.Dispose();
                    ObjectSpace.Dispose();
                    return Request.CreateResponse(HttpStatusCode.OK, number);
                }
                else
                {

                    UserError err = new UserError();
                    err.code = "-1"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "ไม่พบข้อมูลรายการ";
                    return Request.CreateResponse(HttpStatusCode.NoContent, err);
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

        // PUT: api/Count_Approve/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Count_Approve/5
        public void Delete(int id)
        {
        }
    }
}
