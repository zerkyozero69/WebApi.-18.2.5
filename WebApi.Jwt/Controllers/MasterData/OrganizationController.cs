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
using WebApi.Jwt.Models.Models_Masters;
using Organization = nutrition.Module.Organization;

namespace WebApi.Jwt.Controllers.MasterData
{
    public class OrganizationController : ApiController

    {
        private string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        /// <summary>
        /// เลือกหน่วยงาน
        /// </summary>
        ///
        [AllowAnonymous]
        // [JwtAuthentication]
        [HttpPost]
        [Route("Organization_info")]
        public HttpResponseMessage get_Organization()
        {
            try
            {
                string Oid = null;
                Oid = HttpContext.Current.Request.Form["Oid"];
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_GetOrganization", new SqlParameter("@Oid", Oid));
                DataTable dt = new DataTable();
                dt = ds.Tables[0];

                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row = null;

                if(dt.Rows.Count > 0 & row == null)
                {
                    string OidOrg = ConfigurationManager.AppSettings["OidOrgall"];
                    string TextOrg = ConfigurationManager.AppSettings["textOrgall"];

                    row = new Dictionary<string, object>();
                    row.Add("organizationOid", OidOrg);
                    row.Add("OrganizeNameTH", TextOrg);
                    rows.Add(row);
                }

                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    //row.Add("Oid", dr["Oid"].ToString());
                    //row.Add("OrganizeNameTH", dr["OrganizeNameTH"].ToString());
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
                if (rows != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, rows);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, "NoData");
            }
            catch (Exception ex)
            {
                //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }

        /// <summary>
        /// ค้นหา DLD Zone
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        // [JwtAuthentication]
        [HttpPost]
        [Route("getDLDArea/List")]
        public HttpResponseMessage getDLDarea()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(DLDArea));
                XafTypesInfo.Instance.RegisterEntity(typeof(Organization));
                List<listdetail> DLD = new List<listdetail>();

                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);

                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                //IList<Organization> collection = ObjectSpace.GetObjects<Organization>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1 and OrganizeNameTH LIKE 'เขต%'", null));
                IList<Organization> collection = ObjectSpace.GetObjects<Organization>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1 and OrganizeNameTH like 'เขต%'", null));

                var query = from Q in collection orderby Q.OrganizeNameTH select Q;
                {
                    if (query != null)
                    {
                        string Oidzone = ConfigurationManager.AppSettings[ "Oidzoneall"];
                        string Textzone = ConfigurationManager.AppSettings["textzoneall"];
                        string OidOrg = ConfigurationManager.AppSettings["OidOrgall"];
                        string TextOrg = ConfigurationManager.AppSettings["textOrgall"];

                        listdetail listsa = new listdetail();
                        listsa.OId = Oidzone;
                        listsa.DLDName = Textzone;

                        List<listDLD> listDLDs = new List<listDLD>();
                        listDLD item = new listDLD();
                        item.ORGOid = OidOrg;
                        item.OrganizationName = TextOrg;
                        listDLDs.Add(item);
                        listsa.Detail = listDLDs;
                        DLD.Add(listsa);

                    }
                    foreach (Organization row in query)
                    {
                        listdetail listsa = new listdetail();
                        listsa.OId = row.Oid.ToString();
                        listsa.DLDName = row.OrganizeNameTH;

                        IList<Organization> collection2 = ObjectSpace.GetObjects<Organization>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1 and MasterOrganization ='" + row.Oid + "' ", null));
                        List<listDLD> listDLDs = new List<listDLD>();
                        //if (collection2 != null && listDLDs.Count == 0)
                        //{
                        //    string OidOrg = ConfigurationManager.AppSettings["OidOrgall"];
                        //    string TextOrg = ConfigurationManager.AppSettings["textOrgall"];
                        //    listDLD item = new listDLD();
                        //    item.ORGOid = OidOrg;
                        //    item.OrganizationName = TextOrg;
                        //    listDLDs.Add(item);

                        //}
                        foreach (Organization row2 in collection2)
                        {
                            listDLD item = new listDLD();
                            item.ORGOid = row2.Oid.ToString();
                            item.OrganizationName = row2.OrganizeNameTH;
                            listDLDs.Add(item);
                        }
                        listsa.Detail = listDLDs;
                        DLD.Add(listsa);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, DLD);
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
    }
}