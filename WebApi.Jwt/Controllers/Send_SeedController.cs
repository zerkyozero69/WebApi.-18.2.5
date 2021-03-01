using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Xml;
using System.Data.SqlClient;
using System.Configuration;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using Microsoft.ApplicationBlocks.Data;
using DevExpress.Persistent.BaseImpl;
using System.Text;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using System.Web.Http;
using System.Web;
using static WebApi.Jwt.helpclass.helpController;
using static WebApi.Jwt.Models.user;
using System.Data;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base.Security;
using DevExpress.ExpressApp.Security;
using WebApi.Jwt.Models;
using WebApi.Jwt.Filters;
using WebApi.Jwt.helpclass;
using NTi.CommonUtility;
using System.IO;

namespace WebApi.Jwt.Controllers
{
    public class Send_SeedController : ApiController
    {

        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
        /// <summary>
        /// ส่งข้อมูลการปรับสภาพเมล็ดพันธุ์
        /// </summary>
        /// <returns></returns>
        //public HttpResponseMessage Send_Seed()
        //{
        //    try {
        //        string send_number = "";
        //        string datetime = "";
        //        string year_buget = "";
        //        string department_send = "";
        //        string factory_fix = "";
        //        string note = "";
        //        if (HttpContext.Current.Request.Form["send_number"].ToString() != null)
        //        {
        //            send_number = HttpContext.Current.Request.Form["send_number"].ToString();

        //        }
        //        if (HttpContext.Current.Request.Form["datetime"].ToString() != null)
        //        {
        //            datetime = HttpContext.Current.Request.Form["datetime"].ToString();

        //        }
        //        if (HttpContext.Current.Request.Form["year_buget"].ToString() != null)
        //        {
        //            year_buget = HttpContext.Current.Request.Form["year_buget"].ToString();

        //        }
        //        if (HttpContext.Current.Request.Form["department_send"].ToString() != null)
        //        {
        //            department_send = HttpContext.Current.Request.Form["department_send"].ToString();

        //        }
        //        if (HttpContext.Current.Request.Form["factory_fix"].ToString() != null)
        //        {
        //            factory_fix = HttpContext.Current.Request.Form["factory_fix"].ToString();

        //        }
        //        if (HttpContext.Current.Request.Form["note"].ToString() != null)
        //        {
        //            note = HttpContext.Current.Request.Form["note"].ToString();

        //        }
        //    }
        //}

        [AllowAnonymous]
        [HttpPost]
        [Route("Forage_Seed/send")]
        public HttpResponseMessage forage_seed()
        {
            try
            {
                string lot_number = "";
                string budget_year = "";
                string bugget_type = "";
                string department = "";
                string grainInfo = "";
                string grainlevel = "";
                string production_plot = "";
                string harvesting = "";
                string weight_seed = "";
                string product_unit = "";
                string perclean_seed = "";


                
                if (HttpContext.Current.Request.Form["lot_number "].ToString() != null)
                {
                    lot_number = HttpContext.Current.Request.Form["lot_number "].ToString();

                }
                if (HttpContext.Current.Request.Form["budget_year "].ToString() != null)
                {
                    budget_year = HttpContext.Current.Request.Form["budget_year "].ToString();
                }
                if (HttpContext.Current.Request.Form["bugget_type "].ToString() != null)
                {
                    bugget_type = HttpContext.Current.Request.Form["bugget_type "].ToString();
                }
                if (HttpContext.Current.Request.Form["department "].ToString() != null)
                {
                    department = HttpContext.Current.Request.Form["department "].ToString();
                }

                if (HttpContext.Current.Request.Form["grainlevel"].ToString() != null)
                {
                    grainlevel = HttpContext.Current.Request.Form["grainlevel"].ToString();
                }
                if (HttpContext.Current.Request.Form["grainInfo "].ToString() != null)
                {
                    grainInfo = HttpContext.Current.Request.Form["grainInfo "].ToString();
                }
                    if (HttpContext.Current.Request.Form["harvesting"].ToString() != null)
                    {
                        harvesting = HttpContext.Current.Request.Form["harvesting"].ToString();
                    }
                    if (HttpContext.Current.Request.Form["grainlevel  "].ToString() != null)
                    {
                    grainlevel = HttpContext.Current.Request.Form["grainlevel  "].ToString();
                    }
                    if (HttpContext.Current.Request.Form["harvesting"].ToString() != null)
                    {
                        harvesting = HttpContext.Current.Request.Form["harvesting "].ToString();
                    }
                    if (HttpContext.Current.Request.Form["weight_seed"].ToString() != null)
                    {
                        weight_seed = HttpContext.Current.Request.Form["weight_seed"].ToString();
                    }
                    if (HttpContext.Current.Request.Form["product_unit"].ToString() != null)
                    {
                        product_unit = HttpContext.Current.Request.Form["product_unit"].ToString();
                    }
                    if (HttpContext.Current.Request.Form["perclean_seed"].ToString() != null)
                    {
                        perclean_seed = HttpContext.Current.Request.Form["perclean_seed"].ToString();
                    }
                    DataSet ds = new DataSet();
                    ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MobileInsertProduceSeed"
                        , new SqlParameter("@lot_number", lot_number)
                        , new SqlParameter("@budget_year ", budget_year)
                        , new SqlParameter("@bugget_type ", bugget_type)
                        , new SqlParameter("@Organization  ", department)
                        , new SqlParameter("@grainInfo ", grainInfo)
                        , new SqlParameter("@grainlevel ", grainlevel)
                        , new SqlParameter("@harvesting ", harvesting)
                        , new SqlParameter("@production_plot ", production_plot)
                        , new SqlParameter("@weight_seed", weight_seed)
                        , new SqlParameter("@perclean_seed ", perclean_seed));
                if (ds.Tables[0].Rows.Count != 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "เพิ่มเมล็ดสำเร็จ");
                }
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "ใส่ข้อมูลผลผลิตไม่ครบ");
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
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, err);
            }
            
        }
    }
}