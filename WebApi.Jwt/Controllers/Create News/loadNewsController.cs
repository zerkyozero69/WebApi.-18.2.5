using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using Microsoft.ApplicationBlocks.Data;
using nutrition.Module;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Jwt.Models;
using WebApi.Jwt.Models.สร้างข่าว;

namespace WebApi.Jwt.Controllers.สร้างข่าว
{
    public class loadNewsController : ApiController
    {
        private string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        [AllowAnonymous]
        // [JwtAuthentication]
        [HttpGet]
        [Route("GetNews")]
        public HttpResponseMessage GetNews()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "Get_A_News");
                DataTable dt = new DataTable();
                dt = ds.Tables[0];

                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

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

        [AllowAnonymous]
        // [JwtAuthentication]
        [HttpPost]
        [Route("newsDetail")]
        public HttpResponseMessage news_Detail()
        {
            try
            {
                int number = 0;
                string newsOid = HttpContext.Current.Request.Form["Oid"].ToString();
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(News));
                List<newsmodel> list = new List<newsmodel>();
                List<ImageURL_Detail> detail = new List<ImageURL_Detail>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                News objNews = ObjectSpace.FindObject<News>(CriteriaOperator.Parse("  GCRecord is null and Oid =?", newsOid));
                IList<News> collection = ObjectSpace.GetObjects<News>(CriteriaOperator.Parse("GCRecord is null and Oid='" + newsOid + "'", null));
                if (objNews.Oid != null)
                {
                    newsmodel model = new newsmodel();
                    model.Oid = objNews.Oid.ToString();
                    model.CreateDate = objNews.CreateDate;
                    model.Subject = objNews.Subject;
                    model.Details = objNews.Details.Replace("/Images/News/", "http://nutritionit.dld.go.th/Images/News/");
                    model.TotalTimes = objNews.TotalTimes + 1;

                    String[] spearator = { "<img src=" };
                    string[] Arr = objNews.Details.ToString().Split(spearator, System.StringSplitOptions.RemoveEmptyEntries);

                    ImageURL_Detail objdetail = null;
                    foreach (var row in Arr)
                    {
                        if (row.Contains("Images"))
                        {
                            String[] spearator2 = { "alt=" };
                            string[] Arr2 = row.ToString().Split(spearator2, System.StringSplitOptions.RemoveEmptyEntries);
                            objdetail = new ImageURL_Detail();
                            objdetail.ImageURL = Arr2[0].ToString().Replace(@"""", "").Replace(" ", "").Replace("/Images/News/", "http://nutritionit.dld.go.th/Images/News/");
                            detail.Add(objdetail);
                        }
                    }
                    model.objImage = detail;
                    ObjectSpace.CommitChanges();

                    list.Add(model);
                }
                return Request.CreateResponse(HttpStatusCode.OK, list);
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
    }
}