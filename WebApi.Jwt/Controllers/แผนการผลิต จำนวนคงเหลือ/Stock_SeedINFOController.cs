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
using WebApi.Jwt.Models.นับจำนวนกิจกรรม;


namespace WebApi.Jwt.Controllers.แผนการผลิต_จำนวนคงเหลือ
{
    public class Stock_SeedINFOController : ApiController
    {


        SqlConnection scc = new SqlConnection( ConfigurationManager.ConnectionStrings["scc"].ConnectionString);


        /// <summary>
        /// เรียกเสบียงสัตว์คงเหลือ ตามศูนย์
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SupplierProductAmount/Count")]   ///SupplierProductAmount/Count
        public HttpResponseMessage Stockseedanimal()
        {
            try
            {


                string OrganizeOid = null; // Oid จังหวัด
                string BudgetSourceOid = null;
                string DLD = null;


                if (HttpContext.Current.Request.Form["OrganizationOid"].ToString() != null)
                {
                    if (HttpContext.Current.Request.Form["OrganizationOid"].ToString() != "")
                    {
                        OrganizeOid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();
                    }
                }


                DataSet ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "sp_Stock_count", new SqlParameter("@OrganizationOid", OrganizeOid));
              

                List<Stock_info> titile_Groups = new List<Stock_info>();
                Stock_info stock_Info = new Stock_info();
                List<SeedAnimal_info> detail = new List<SeedAnimal_info>();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int number = 0;
                    string Temp_Group_Name = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (Temp_Group_Name == dr["SeedLevelCode"].ToString())
                        {
                            number = number;
                            SeedAnimal_info item = new SeedAnimal_info();
                            item.Title = dr["SeedName"].ToString();
                            item.Weight = Convert.ToDouble(dr["TotalWeight"]);
                            item.Unit = dr["WeightUnit"].ToString();


                            //status.Add(item);
                            stock_Info.Data.Add(item);
                        }
                        else
                        {
                            SeedAnimal_info item = new SeedAnimal_info();
                            item.Title = dr["SeedName"].ToString();

                            item.Weight = Convert.ToDouble(dr["TotalWeight"]);
                            item.Unit = dr["WeightUnit"].ToString();

                            number = number + 1;

                            Temp_Group_Name = dr["SeedLevelCode"].ToString();
                            stock_Info = new Stock_info();

                            stock_Info.Id = number;

                            stock_Info.Title = dr["SeedLevelCode"].ToString();
                            stock_Info.Total = Convert.ToDouble(dr["SumWeight"].ToString());

                            switch (dr["SeedLevelCode"].ToString())
                            {
                                case "BS":
                                    stock_Info.Color = "#F1948A";
                                    break;
                                case "CS":
                                    stock_Info.Color = "#FF7F27";
                                    break;
                                case "FS":
                                    stock_Info.Color = "#00E142";
                                    break;
                                case "RS":
                                    stock_Info.Color = "#99D9EA";
                                    break;
                                default:
                                    stock_Info.Color = "#ABB2B9";
                                    break;
                            }


                            //status.Add(item);
                            //Group_.Status_List = status;
                            stock_Info.Data.Add(item);
                            titile_Groups.Add(stock_Info);
                        }

                    }
                    UserError err = new UserError();
                    err.code = ""; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "OK";
                    return Request.CreateResponse(HttpStatusCode.OK, titile_Groups);
                }
                else
                {

                    UserError err2 = new UserError();
                    err2.code = "0"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err2.message = "กรุณาระบุศูนย์";
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
           finally
            {
                SqlConnection.ClearAllPools();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("SupplierAnimalAmount/Count")]   ///SupplierProductAmount/Count
        public HttpResponseMessage StockAnimal()
        {
            try
            {


                string OrganizeOid = null; // Oid จังหวัด
                string BudgetSourceOid = null;
                string DLD = null;

                if (HttpContext.Current.Request.Form["OrganizationOid"].ToString() != null)
                {
                    if (HttpContext.Current.Request.Form["OrganizationOid"].ToString() != "")
                    {
                        OrganizeOid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();
                    }
                }
                //    BudgetSourceOid = HttpContext.Current.Request.Form["BudgetSourceOid"].ToString();
                //    DLD = HttpContext.Current.Request.Form["DLD"].ToString();

                //}
                DataSet ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "sp_StockAnimal_count", new SqlParameter("@OrganizationOid", OrganizeOid)
                );

                List<StockAnimals> titile_Groups = new List<StockAnimals>();
                StockAnimals stock_Info = new StockAnimals();
                List<SeedAnimalStock_info> detail = new List<SeedAnimalStock_info>();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int number = 0;
                    string Temp_Group_Name = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (Temp_Group_Name == dr["AnimalSupplieName"].ToString())
                        {
                            number = number;
                            SeedAnimalStock_info item = new SeedAnimalStock_info();
                            item.Title = dr["SeedName"].ToString();
                            item.Weight = Convert.ToDouble(dr["TotalWeight"]);
                            item.Unit = dr["WeightUnit"].ToString();


                            //status.Add(item);
                            stock_Info.Data.Add(item);
                        }
                        else
                        {
                            SeedAnimalStock_info item = new SeedAnimalStock_info();
                            item.Title = dr["SeedName"].ToString();

                            item.Weight = Convert.ToDouble(dr["TotalWeight"]);
                            item.Unit = dr["WeightUnit"].ToString();

                            number = number + 1;

                            Temp_Group_Name = dr["AnimalSupplieName"].ToString();
                            stock_Info = new StockAnimals();

                            stock_Info.Id = number;

                            stock_Info.Title = dr["AnimalSupplieName"].ToString();
                            stock_Info.Total = Convert.ToDouble(dr["SumWeight"].ToString());

                            switch (dr["AnimalSupplieName"].ToString())
                            {
                                case "สด":
                                    stock_Info.Color = "#F1948A";
                                    break;
                                case "หมัก":
                                    stock_Info.Color = "#FF7F27";
                                    break;
                                case "แห้ง":
                                    stock_Info.Color = "#00E142";
                                    break;

                                default:
                                    stock_Info.Color = "#ABB2B9";
                                    break;
                            }


                            //status.Add(item);
                            //Group_.Status_List = status;
                            stock_Info.Data.Add(item);
                            titile_Groups.Add(stock_Info);
                        }

                    }
                    UserError err = new UserError();
                    err.code = ""; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "OK";
                    return Request.CreateResponse(HttpStatusCode.OK, titile_Groups);
                }


                UserError err2 = new UserError();
                err2.code = "0"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err2.message = "กรุณาระบุศูนย์";
                return Request.CreateResponse(HttpStatusCode.BadRequest);


            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            finally
            {
                SqlConnection.ClearPool(scc);
            }
        }
        /// <summary>
        /// เรียกเสบียงสัตว์คงเหลือ ตามศูนย์
        /// </summary>
        /// <returns></returns>
        //[AllowAnonymous]
        //[HttpPost]
        //[Route("SupplierProductAmount/Count")]   ///SupplierProductAmount/Count
        //public HttpResponseMessage Stockseedanimaldemo()
        //{
        //    try
        //    {


        //        string OrganizeOid = null; // Oid จังหวัด
        //        string BudgetSourceOid = null;
        //        string DLD = null;


        //        if (HttpContext.Current.Request.Form["OrganizationOid"].ToString() != null)
        //        {
        //            if (HttpContext.Current.Request.Form["OrganizationOid"].ToString() != "")
        //            {
        //                OrganizeOid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();
        //            }
        //        }

        //        BudgetSourceOid = HttpContext.Current.Request.Form["BudgetSourceOid"].ToString();
        //        DLD = HttpContext.Current.Request.Form["DLD"].ToString();

        //        XpoTypesInfoHelper.GetXpoTypeInfoSource();
        //        XafTypesInfo.Instance.RegisterEntity(typeof(StockSeedInfo));
        //        XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
        //        IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

        //        //IList<StockSeedInfo> objstockseed = ObjectSpace.GetObject <StockSeedInfo>(CriteriaOperator.Parse("[OrganizationOid]=? and Status=1",DLD ));
        //        //UserError err = new UserError();
        //        //err.code = ""; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
        //        //err.message = "OK";
        //        //return Request.CreateResponse(HttpStatusCode.OK, "");
            


        //        UserError err2 = new UserError();
        //    err2.code = "0"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
        //    err2.message = "กรุณาระบุศูนย์";
        //    return Request.CreateResponse(HttpStatusCode.BadRequest);

        //}
        //    catch (Exception ex)
        //    {
        //        UserError err = new UserError();
        //err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
        //        err.message = ex.Message;
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
        //    }

        //}
        
    }
    
}
