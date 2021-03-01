using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using nutrition.Module;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Jwt.Models;

namespace WebApi.Jwt.Controllers
{
    public class StockSeedController : ApiController
    {
        private string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        /// <summary>
        /// เรียกสต็อคคงเหลือ
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("GetStockSeed")]
        public IHttpActionResult GetStockSeed()
        {
            string OrganizationOid;
            string FinanceYearOid;
            try
            {
                OrganizationOid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();

                FinanceYearOid = HttpContext.Current.Request.Form["FinanceYearOid"].ToString();

                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(StockSeedInfo));
                List<StockSeedInfo_Model> list_detail = new List<StockSeedInfo_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                IList<StockSeedInfo> collection = ObjectSpace.GetObjects<StockSeedInfo>(CriteriaOperator.Parse(" GCRecord is null  and OrganizationOid=? and FinanceYearOid = ?", OrganizationOid, FinanceYearOid));
                if (collection.Count > 0)
                {
                    foreach (StockSeedInfo row in collection)
                    {
                        StockSeedInfo_Model stock = new StockSeedInfo_Model();
                        stock.Oid = row.Oid.ToString();
                        stock.StockDate = row.StockDate.ToString();
                        stock.OrganizationOid = row.OrganizationOid.Oid.ToString();
                        stock.Organization = row.OrganizationOid.SubOrganizeName;
                        stock.FinanceYearOid = row.FinanceYearOid.Oid.ToString();
                        stock.FinanceYear = row.FinanceYearOid.YearName;
                        stock.BudgetSourceOid = row.BudgetSourceOid.Oid.ToString();
                        stock.BudgetSource = row.BudgetSourceOid.BudgetName;
                        stock.AnimalSeedOid = row.AnimalSeedOid.Oid.ToString();
                        stock.AnimalSeed = row.AnimalSeedOid.SeedName;
                        stock.AnimalSeedLevelOid = row.AnimalSeedLevelOid.Oid.ToString();
                        stock.AnimalSeedLevel = row.AnimalSeedLevelOid.SeedLevelName;
                        stock.StockDetail = row.StockDetail;
                        stock.TotalForward = row.TotalForward;
                        stock.TotalChange = row.TotalChange;
                        stock.TotalWeight = row.TotalWeight;
                        stock.ReferanceCode = row.ReferanceCode;
                        stock.SeedTypeOid = row.SeedTypeOid.Oid.ToString();
                        stock.SeedType = row.SeedTypeOid.SeedTypeName;
                        list_detail.Add(stock);
                    }
                    return Ok(list_detail);
                }
                else
                {
                    UserError err = new UserError();
                    err.code = "-99"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "No data";
                    //  Return resual
                    return BadRequest(err.message);
                }

                //else
                // {
            }
            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                return BadRequest(ex.Message);
            }
        }/// <summary>

         /// เรียกเสบียงสัตว์ ตามเขต
         /// </summary>
         /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("ManageAnimalSupplier")]
        public HttpResponseMessage ManageAnimalSupplier()
        {
            string OrganizationOid;
            string FinanceYearOid;
            try
            {
                OrganizationOid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();

                FinanceYearOid = HttpContext.Current.Request.Form["FinanceYearOid"].ToString();

                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(ManageAnimalSupplier));
                List<ManageAnimalSupplier_Model> list_detail = new List<ManageAnimalSupplier_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                IList<ManageAnimalSupplier> collection = ObjectSpace.GetObjects<ManageAnimalSupplier>(CriteriaOperator.Parse(" GCRecord is null and Status = 1 and OrgZoneOid=? and FinanceYearOid=?  ", OrganizationOid, FinanceYearOid));
                double Weight = 0;
                if (collection.Count > 0)
                {
                    foreach (ManageAnimalSupplier row in collection)
                    {
                        ManageAnimalSupplier_Model ManageAnimal = new ManageAnimalSupplier_Model();
                        ManageAnimal.Oid = row.Oid.ToString();
                        ManageAnimal.OrgZoneOid = row.OrgZoneOid.Oid.ToString();
                        ManageAnimal.OrgZone = row.OrgZoneOid.OrganizeNameTH;
                        ManageAnimal.OrganizationOid = row.OrganizationOid.Oid.ToString();
                        ManageAnimal.Organization = row.OrganizationOid.SubOrganizeName;
                        ManageAnimal.FinanceYearOid = row.FinanceYearOid.Oid.ToString();
                        ManageAnimal.FinanceYear = row.FinanceYearOid.YearName;
                        ManageAnimal.AnimalSupplieOid = row.AnimalSupplieOid.Oid.ToString();
                        ManageAnimal.AnimalSupplie = row.AnimalSupplieOid.AnimalSupplieName;
                        ManageAnimal.ZoneQTY = row.ZoneQTY;
                        ManageAnimal.CenterQTY = row.CenterQTY;
                        ManageAnimal.OfficeQTY = row.OfficeQTY;
                        ManageAnimal.OfficeGAPQTY = row.OfficeGAPQTY;
                        ManageAnimal.OfficeBeanQTY = row.OfficeBeanQTY;

                        List<ManageSubAnimalSupplier_Model> detail = new List<ManageSubAnimalSupplier_Model>();
                        foreach (ManageSubAnimalSupplier row2 in row.ManageSubAnimalSuppliers)
                        {
                            ManageSubAnimalSupplier_Model item = new ManageSubAnimalSupplier_Model();
                            item.ProvinceQTY = row2.ProvinceQTY;

                            item.Province = row2.ProvinceOid.ProvinceNameTH;
                            detail.Add(item);
                        }
                        ManageAnimal.Detail = detail;
                        ManageAnimal.SumProvinceQTY = row.SumProvinceQTY.Value;

                        list_detail.Add(ManageAnimal);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, list_detail);
                }
                else
                {
                    UserError err = new UserError();
                    err.code = "5"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "No data";
                    //  Return resual
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
            }
            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}