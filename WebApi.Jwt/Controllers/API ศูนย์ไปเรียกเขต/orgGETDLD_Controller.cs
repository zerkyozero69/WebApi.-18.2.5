using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using nutrition.Module;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Jwt.Models;
using WebApi.Jwt.Models.Models_Masters;

namespace WebApi.Jwt.Controllers.API_ศูนย์ไปเรียกเขต
{
    public class orgGETDLD_Controller : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        // GET: api/orgGETDLD_
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/orgGETDLD_/5
        public string Get(int id)
        {
            return "value";
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("GetDLD_byORG")]
        public HttpResponseMessage GetDLD_byORG()
        {
            string OrganizationOid = null;

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

                foreach (Organization row in query)
                {
                    listdetail listsa = new listdetail();
                    listsa.OId = row.Oid.ToString();
                    listsa.DLDName = row.OrganizeNameTH;

                    IList<Organization> collection2 = ObjectSpace.GetObjects<Organization>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1 and MasterOrganization ='" + row.Oid + "' ", null));
                    List<listDLD> listDLDs = new List<listDLD>();
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
            catch (Exception ex)
            {
                err2 err = new err2();
                err.message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                throw;

            }



        }
    }
}