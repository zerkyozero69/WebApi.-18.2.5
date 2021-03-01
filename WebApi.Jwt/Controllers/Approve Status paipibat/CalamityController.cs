using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using Microsoft.ApplicationBlocks.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using nutrition.Module;
using nutrition.Module.EmployeeAsUserExample.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Jwt.Models;
using WebApi.Jwt.Models.Models_Masters;
using WebApi.Jwt.Models.ภัยภิบัติ;
using static WebApi.Jwt.Models.Farmerinfo;
using static WebApi.Jwt.Models.แผนการผลิตโมเดล.การใช้เมล็ดพันธุ์อนุมัติภัยพิบัติ;

namespace WebApi.Jwt.Controllers.อนุมัติภัยพิบัตื
{
    public class CalamityController : ApiController
    {
         string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
        /// <summary>
        /// อนุมัติ-ไม่อนุมัติการช่วยเหลือภัยพิบัติ เมล็ดพันธุ์
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("ApprovalDisasterSupplierUseProduct/Update")]
        public HttpResponseMessage UpdateDisaster()  ///SupplierUseAnimalProduct/Update
        {
            string TempDescription = "";
            string Username = "";
            bool result = false;
            try
            {

                string requestString = Request.Content.ReadAsStringAsync().Result;
                JObject jObject = (JObject)JsonConvert.DeserializeObject(requestString);

                string RefNo = jObject.SelectToken("RefNo").Value<string>();//ข้อมูลเลขที่อ้างอิง
                string Status = jObject.SelectToken("Status").Value<string>(); //สถานะ 1 บันทึกการส่งแต่ไม่เปลี่ยนสถานนะ/ 2 ยืนยันการส่ง
                string Remark = jObject.SelectToken("Remark").Value<string>(); //หมายเหตุ
                string Type = jObject.SelectToken("Type").Value<string>(); //1 รายเดี่ยว /2 รายกลุ่ม
                string activityNameOid = jObject.SelectToken("activityNameOid").Value<string>();

                Username = jObject.SelectToken("Username").Value<string>();


                if (RefNo != "" && Status != "")
                {
                    string[] arr = RefNo.Split('|');
                    string _refno = arr[0]; //เลขที่อ้างอิง
                    string _org_oid = arr[1]; //oid หน่วยงาน
                    string _type = arr[2]; //ประเภทส่ง(2)-รับ(1)

                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierUseProduct));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.Activity));
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    directProvider.Dispose();
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                    ObjectSpace.Dispose();

                    SupplierUseProduct ObjMaster = ObjectSpace.FindObject<SupplierUseProduct>(CriteriaOperator.Parse("UseNo=? and ActivityOid= ?  ", _refno, activityNameOid));

                    Activity objActivity = ObjectSpace.FindObject<Activity>(CriteriaOperator.Parse("Oid = ?", activityNameOid));


                    if (ObjMaster != null)
                    {

                        if (Status == "1")  //1 อนุมัติ
                        { //บันทึกการส่ง

                            foreach (SupplierUseProductDetail row in ObjMaster.SupplierUseProductDetails)
                            {
                                var objStockSeedInfo = ObjectSpace.GetObjects<StockSeedInfo>(CriteriaOperator.Parse("OrganizationOid= ? and AnimalSeedOid=? and AnimalSeedLevelOid=? and SeedTypeOid=? and ReferanceCode=?  ", ObjMaster.OrganizationOid.Oid, row.AnimalSeedOid.Oid, row.AnimalSeedLevelOid.Oid, row.SeedTypeOid.Oid, row.LotNumber.LotNumber));
                                if (objStockSeedInfo.Count > 0)
                                {
                                    if (ObjMaster.ActivityOid.ActivityName == "เพื่อใช้ในกิจกรรมของศูนย์ฯ")
                                    {
                                        EmployeeInfo objEmployees = ObjectSpace.FindObject<EmployeeInfo>(CriteriaOperator.Parse("[Oid]=?", ObjMaster.EmployeeOid));
                                        if (objEmployees != null)
                                        {
                                            TempDescription = "ใช้ในกิจกรรมของศูนย์ฯ (Mobile Application)-" + ObjMaster.SubActivityOid.ActivityName + "" + " : " + "" + objEmployees.FullName;
                                        }
                                        else
                                        {
                                            TempDescription = "ใช้ในกิจกรรมของศูนย์ฯ (Mobile Application)- " + ObjMaster.SubActivityOid.ActivityName + " : ";
                                        }
                                    }

                                    if (ObjMaster.ActivityOid.ActivityName == "เพื่อช่วยเหลือภัยพิบัติ")
                                    {
                                        if (ObjMaster.RegisCusServiceOid != null)
                                        {
                                            RegisterCusService objRegisCusService = ObjectSpace.FindObject<RegisterCusService>(CriteriaOperator.Parse("[Oid]=?", ObjMaster.RegisCusServiceOid));
                                            if (objRegisCusService != null)
                                            {
                                                TempDescription = "ช่วยเหลือภัยพิบัติ (Mobile Application)-" + ObjMaster.SubActivityLevelOid.ActivityName + " : " + objRegisCusService.DisPlayName;
                                            }
                                            else
                                            {
                                                TempDescription = "ช่วยเหลือภัยพิบัติ (Mobile Application)--" + ObjMaster.SubActivityLevelOid.ActivityName + " : ";
                                            }
                                        }
                                    }
                                    if (ObjMaster.ActivityOid.ActivityName == "เพื่อการจำหน่าย")
                                    {
                                        RegisterCusService objRegisCusService = ObjectSpace.FindObject<RegisterCusService>(CriteriaOperator.Parse("[Oid]=?", ObjMaster.RegisCusServiceOid));
                                        if (objRegisCusService != null)
                                        {
                                            TempDescription = "จำหน่ายให้ (Mobile Application): " + objRegisCusService.DisPlayName;
                                        }
                                        else
                                        {
                                            TempDescription = "จำหน่ายให้ (Mobile Application): ";
                                        }
                                    }
                                    else if (ObjMaster.OrgeServiceOid != null)
                                    {
                                        OrgeService objOrgeService = ObjectSpace.FindObject<OrgeService>(CriteriaOperator.Parse("[Oid]=?", ObjMaster.OrgeServiceOid));
                                        if (objOrgeService != null)
                                        {
                                            TempDescription = "จำหน่ายให้ (Mobile Application): " + objOrgeService.OrgeServiceName;
                                        }
                                        else
                                        {
                                            TempDescription = "จำหน่ายให้ (Mobile Application)-: ";
                                        }

                                    }
                                    if (ObjMaster.RegisCusServiceOid != null)
                                    {
                                        RegisterCusService objRegisCusService = ObjectSpace.FindObject<RegisterCusService>(CriteriaOperator.Parse("[Oid]=?", ObjMaster.RegisCusServiceOid));
                                        if (objRegisCusService != null)
                                        {
                                            TempDescription = "แจกจ่ายให้ (Mobile Application): " + objRegisCusService.DisPlayName;
                                        }
                                        else
                                        {
                                            TempDescription = "แจกจ่ายให้ (Mobile Application): ";
                                        }
                                    }
                                    else if (ObjMaster.OrgeServiceOid != null)
                                    {
                                        OrgeService objOrgeService = ObjectSpace.FindObject<OrgeService>(CriteriaOperator.Parse("[Oid]=?", ObjMaster.OrgeServiceOid));
                                        if (objOrgeService != null)
                                        {
                                            TempDescription = "แจกจ่ายให้ (Mobile Application): " + objOrgeService.OrgeServiceName;
                                        }

                                        else
                                        {
                                            TempDescription = "แจกจ่ายให้ (Mobile Application): ";
                                        }
                                    }

                                }
                                var ObjSubStockCardSource = (from Item in objStockSeedInfo orderby Item.StockDate descending select Item).First();
                           

                                var ObjStockSeedInfoInfo = ObjectSpace.CreateObject<StockSeedInfo>();
                                // ObjMaster.ReceiveOrgOid.Oid, ObjMaster.FinanceYearOid.Oid, objSupplierProduct.BudgetSourceOid, objSupplierProduct.AnimalSeedOid.Oid, objSupplierProduct.AnimalSeedLevelOid.Oid))
                                {
                                    var withBlock = ObjStockSeedInfoInfo;
                                    withBlock.StockDate = DateTime.Now;
                                    withBlock.OrganizationOid = ObjMaster.OrganizationOid;
                                    withBlock.FinanceYearOid = ObjSubStockCardSource.FinanceYearOid;
                                    withBlock.BudgetSourceOid = ObjSubStockCardSource.BudgetSourceOid;
                                    withBlock.AnimalSeedOid = ObjSubStockCardSource.AnimalSeedOid;
                                    withBlock.AnimalSeedLevelOid = ObjSubStockCardSource.AnimalSeedLevelOid;
                                    withBlock.StockDetail = "เบิกเมล็ดพันธุ์ (Mobile Application) ลำดับที่ : " + ObjMaster.UseNo;
                                    withBlock.TotalForward = ObjSubStockCardSource.TotalForward;
                                    withBlock.SeedTypeOid = ObjSubStockCardSource.SeedTypeOid;
                                    withBlock.TotalChange = 0 - row.Weight;
                                    withBlock.StockType = EnumStockType.ReceiveProduct;
                                    withBlock.ReferanceCode = row.LotNumber.LotNumber;
                                    withBlock.Description = TempDescription;
                                    withBlock.UseNo = ObjMaster.UseNo;
                                }
                                if (ObjStockSeedInfoInfo.TotalWeight == 0)
                                {
                                    row.LotNumber.IsActive = false;
                                }
                                ObjectSpace.CommitChanges();
                            }
                            //   OrgeServiceOid = Session.FindObject<OrgeService>(CriteriaOperator.Parse("Oid=?", OrgeServiceOid))OrgeServiceOid
                            //objSupplierUseProduct = ObjectSpace.CreateObject<SupplierUseProduct>();

                            if (Remark != "")
                            {
                                ObjMaster.Remark = Remark;
                            }

                            {
                                var withBlock = ObjMaster;
                                withBlock.Status = EnumSupplierUseStatus.Approve;
                                withBlock.ApproveDate = DateTime.Now;
                            }
                            ObjectSpace.CommitChanges();

                            HistoryWork ObjHistory = null;
                            ObjHistory = ObjectSpace.CreateObject<HistoryWork>();
                            // ประวัติ
                            ObjHistory.RefOid = ObjMaster.Oid.ToString();
                            ObjHistory.FormName = "เมล็ดพันธุ์";
                            ObjHistory.Message = "อนุมัติ (ขอเบิกเมล็ดพันธุ์ (Mobile Application)) ลำดับที่ : " + ObjMaster.UseNo;
                            ObjHistory.CreateBy = Username;
                            ObjHistory.CreateDate = DateTime.Now;
                            ObjectSpace.CommitChanges();
                           
                        }
                 
                        if (Status == "2")
                        { //ไม่อนุมัติ
                            foreach (SupplierUseProductDetail row in ObjMaster.SupplierUseProductDetails)
                            {
                                var objStockSeedInfo = ObjectSpace.GetObjects<StockSeedInfo>(CriteriaOperator.Parse("OrganizationOid= ? and AnimalSeedOid=? and AnimalSeedLevelOid=? and SeedTypeOid=? and ReferanceCode=? ", ObjMaster.OrganizationOid.Oid, row.AnimalSeedOid.Oid, row.AnimalSeedLevelOid.Oid, row.SeedTypeOid.Oid, row.LotNumber.LotNumber));
                                if (objStockSeedInfo.Count > 0)
                                {
                                    var ObjSubStockCardSource = (from Item in objStockSeedInfo orderby Item.StockDate descending select Item).First().TotalWeight;
                                    var ObjSubStockCardSource_BudgetSourceOid = (from Item in objStockSeedInfo orderby Item.StockDate descending select Item).First().BudgetSourceOid;
                                    var ObjSubStockCardSource_FinanceYearOid = (from Item in objStockSeedInfo orderby Item.StockDate descending select Item).First().FinanceYearOid;
                                    var ObjSubStockCardSource_AnimalSeedOid = (from Item in objStockSeedInfo orderby Item.StockDate descending select Item).First().AnimalSeedOid;
                                    var ObjSubStockCardSource_AnimalSeedLevelOid = (from Item in objStockSeedInfo orderby Item.StockDate descending select Item).First().AnimalSeedLevelOid;
                                    var ObjSubStockCardSource_SeedTypeOid = (from Item in objStockSeedInfo orderby Item.StockDate descending select Item).First().SeedTypeOid;


                                    var ObjStockSeedInfoInfo = ObjectSpace.CreateObject<StockSeedInfo>();
                                    // ObjMaster.ReceiveOrgOid.Oid, ObjMaster.FinanceYearOid.Oid, objSupplierProduct.BudgetSourceOid, objSupplierProduct.AnimalSeedOid.Oid, objSupplierProduct.AnimalSeedLevelOid.Oid))
                                    {
                                        var withBlock1 = ObjStockSeedInfoInfo;
                                        withBlock1.StockDate = DateTime.Now;
                                        withBlock1.OrganizationOid = ObjMaster.OrganizationOid;
                                        withBlock1.FinanceYearOid = ObjSubStockCardSource_FinanceYearOid;
                                        withBlock1.BudgetSourceOid = ObjSubStockCardSource_BudgetSourceOid;
                                        withBlock1.AnimalSeedOid = ObjSubStockCardSource_AnimalSeedOid;
                                        withBlock1.AnimalSeedLevelOid = ObjSubStockCardSource_AnimalSeedLevelOid;
                                        withBlock1.StockDetail = "ไม่อนุมัติการใช้เมล็ดพันธุ์ (Mobile Application) ลำดับที่ : " + ObjMaster.UseNo;
                                        withBlock1.TotalForward = ObjSubStockCardSource;
                                        withBlock1.SeedTypeOid = ObjSubStockCardSource_SeedTypeOid;
                                        withBlock1.TotalChange = row.Weight;
                                        withBlock1.StockType = EnumStockType.ReceiveProduct;
                                        withBlock1.ReferanceCode = row.LotNumber.LotNumber;
                                        withBlock1.Description = "ไม่อนุมัติการใช้เมล็ดพันธุ์ (Mobile Application) : " + ObjMaster.UseNo;
                                        withBlock1.UseNo = ObjMaster.UseNo;
                                    }
                                    if (ObjStockSeedInfoInfo.TotalWeight == 0)
                                        row.LotNumber.IsActive = false;
                                    ObjectSpace.CommitChanges();
                                                 }

                            }
                            if (Remark != "")
                            {
                                ObjMaster.Remark = Remark;
                            }
                            var withBlock = ObjMaster;
                            withBlock.Status = EnumSupplierUseStatus.Eject;
                            withBlock.ApproveDate = DateTime.Now; //2

                            HistoryWork ObjHistory = null;
                            ObjHistory = ObjectSpace.CreateObject<HistoryWork>();
                            // ประวัติ
                            ObjHistory.RefOid = ObjMaster.Oid.ToString();
                            ObjHistory.FormName = "เมล็ดพันธุ์";
                            ObjHistory.Message = "ไม่อนุมัติ (ขอเบิกเมล็ดพันธุ์(Mobile Application)) ลำดับที่ : " + ObjMaster.UseNo;
                            ObjHistory.CreateBy = Username;
                            ObjHistory.CreateDate = DateTime.Now;
                            ObjectSpace.CommitChanges();

                        }


                        UpdateResult ret = new UpdateResult();
                        ret.status = "true";
                        ret.message = "บันทึกข้อมูลไม่อนุมัติเสร็จเรียบร้อยแล้ว";
                        return Request.CreateResponse(HttpStatusCode.OK, ret);
                    }


                    else
                    {
                        UserError err = new UserError();
                        err.status = "false";
                        err.code = "-1";
                        err.message = "ไม่พบข้อมูล";
                        return Request.CreateResponse(HttpStatusCode.NotFound, err);
                    }


                }
                else
                {
                    UserError err = new UserError();
                    err.status = "false";
                    err.code = "0";
                    err.message = "กรุณาใส่ข้อมูล RefNo ให้เรียบร้อยก่อน";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
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
        /// <summary>
        ///  sp อัพเดทเมล็ดพันธุ์
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("insertsCalamity/SupplierUserProduct")]
        public HttpResponseMessage inserts_Calamity_SupplierUserProduct()
        {
            SupplierProductUser_Model2 productUser = new SupplierProductUser_Model2();

            try
            {

                string requestString = Request.Content.ReadAsStringAsync().Result;
                JObject jObject = (JObject)JsonConvert.DeserializeObject(requestString);
                string TempForageType = string.Empty;
                if (jObject != null)
                {

                    //string[] arr = RefNo.Split('|');
                    //string _refno = arr[0]; //เลขที่อ้างอิง
                    //string _org_oid = arr[1]; //oid หน่วยงาน
                    //string _type = arr[2]; //ประเภทส่ง(2)-รับ(1)

                    string iDate = jObject.SelectToken("UseDate").Value<string>();
                    DateTime oDate = Convert.ToDateTime(iDate);
                    productUser.UseNo = jObject.SelectToken("UseNo").Value<string>();
                    productUser.UseDate = oDate.Year + "-" + oDate.Month + "-" + oDate.Day;
                    productUser.FinanceYearOid = jObject.SelectToken("FinanceYearOid").Value<string>();
                    productUser.OrganizationOid = jObject.SelectToken("OrganizationOid").Value<string>();
                    productUser.Remark = jObject.SelectToken("Remark").Value<string>();
                    productUser.ActivityNameOid = jObject.SelectToken("ActivityNameOid").Value<string>();
                    productUser.CitizenID = jObject.SelectToken("CitizenID").Value<string>();


                    productUser.YearName = jObject.SelectToken("YearName").Value<string>();

                    if (jObject.SelectToken("SubActivityOid") != null)
                    {
                        productUser.SubActivityOid = jObject.SelectToken("SubActivityOid").Value<string>();
                    }
                    if (jObject.SelectToken("SubActivityLevelName") != null)
                    { productUser.SubActivityLevelName = jObject.SelectToken("SubActivityLevelName").Value<string>(); }

                    if (jObject.SelectToken("RegisCusServiceOid ") != null)
                    {
                        productUser.RegisCusServiceOid = jObject.SelectToken("RegisCusServiceOid ").Value<string>();
                    }
                    if (jObject.SelectToken("OrgeServiceOid ") != null)
                    {
                        productUser.OrgeServiceOid = jObject.SelectToken("OrgeServiceOid ").Value<string>();
                    }

                    productUser.ServiceCount = jObject.SelectToken("ServiceCount").Value<int>();


                    if (productUser.UseNo == "")
                    {
                        XpoTypesInfoHelper.GetXpoTypeInfoSource();
                        XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.RunNumber));
                        XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.Organization));

                        XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                        directProvider.Dispose();
                        IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                        ObjectSpace.Dispose();
                        Organization objORG = ObjectSpace.FindObject<Organization>(CriteriaOperator.Parse("Oid=?", productUser.OrganizationOid));
                        //SendOrderSeed objSupplierProduct = ObjectSpace.FindObject<SendOrderSeed>(CriteriaOperator.Parse("SendNo=?", _refno));
                        RunNumber runningNumber = ObjectSpace.FindObject<RunNumber>(CriteriaOperator.Parse("FormType ='UseProduct' and BudgetYear =? and OrgCode=? ", productUser.YearName, objORG.OrganizationCode));
                        if (runningNumber != null)
                        {
                            string customerNumberFormat = string.Empty;
                            string Postfix = "000" + runningNumber.LastNumber + 1;
                            var FullNumber = objORG.OrganizationCode + "-" + productUser.YearName.Substring(productUser.YearName.Length - 2, 2).PadLeft(2, '0') + "-" + (runningNumber.LastNumber + 1).ToString().PadLeft(6, '0');

                            productUser.UseNo = FullNumber;
                        }
                        else
                        {
                            DataSet ds2;
                            SqlParameter[] prm2 = new SqlParameter[10];

                            prm2[0] = new SqlParameter("@orgcode", objORG.OrganizationCode);
                            prm2[1] = new SqlParameter("@BudgetYear", productUser.YearName);
                            prm2[2] = new SqlParameter("@LastNumber", 1);
                            prm2[3] = new SqlParameter("@FormType", "UseProduct");

                            ds2 = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "Insert_RuningNumber", prm2);

                            productUser.UseNo = objORG.OrganizationCode + "-" + productUser.YearName.Substring(productUser.YearName.Length - 2, 2).PadLeft(2, '0') + "-000001";
                        }
                    }

                    DataSet ds;
                    SqlParameter[] prm = new SqlParameter[10];

                    prm[0] = new SqlParameter("@UseNo", productUser.UseNo);
                    prm[1] = new SqlParameter("@UseDate", productUser.UseDate);
                    prm[2] = new SqlParameter("@YearName", productUser.YearName);
                    prm[3] = new SqlParameter("@OrganizationOid", productUser.OrganizationOid);
                    prm[4] = new SqlParameter("@Remark", productUser.Remark);
                    prm[5] = new SqlParameter("@ActivityOid", productUser.ActivityNameOid);
                    prm[6] = new SqlParameter("@RegisCusServiceOid", productUser.RegisCusServiceOid);
                    prm[7] = new SqlParameter("@OrgeServiceOid", productUser.OrgeServiceOid);
                    prm[8] = new SqlParameter("@ServiceCount", productUser.ServiceCount);
                    prm[9] = new SqlParameter("@CitizenID", productUser.CitizenID);


                    ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MoblieInserts_Calamity_SupplierUseProduct", prm);
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];

                    //    using (DataSet ds = SqlHelper.ExecuteDataset(scc, "spt_Moblieinsert_RegisterFarmer", ))

                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        //return Request.CreateResponse(HttpStatusCode.OK);
                        return Request.CreateResponse(HttpStatusCode.OK, productUser);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, productUser);
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "กรอกข้อมูลไม่ครบ");
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
        /// <summary>
        /// เจนเลข oid ใบลงทะเบียน
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("getOid/SupplierUseAnimalProduct")]
        public HttpResponseMessage GetList_cataclysm()
        {
            GenOidcalamity genOid = new GenOidcalamity();
            string OrganizationOid = HttpContext.Current.Request.Form["Organizationoid"].ToString();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "GenOidSupplierUseAnimalProduc", new SqlParameter("@OrganizationOid", OrganizationOid));
                DataTable Dt = ds.Tables[0];
                if (Dt.Rows.Count > 0)
                {
                    foreach (DataRow obj in Dt.Rows)

                        genOid.SupplierUseAnimalProductOid = obj["Oid"].ToString();
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                return Request.CreateResponse(HttpStatusCode.OK, genOid);
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
                Dispose();
            }
        }


        /// <summary>
        /// อัพเดทภัยพิบัติ เสบียงสัตว์
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("insertsCalamity/SupplierUseAnimalProduct")]
        public HttpResponseMessage inserts_Calamity_SupplierUseAnimalProductt()
        {
            return_OidSupplierUseAnimalProductOid item = new return_OidSupplierUseAnimalProductOid();
            SupplierProductUser_Model2 productUser = new SupplierProductUser_Model2();
            int? Type = 0;
            int? Typestatus = 0;
            string Username = "";


            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(Activity));
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseAnimalProduct));
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseProductDetail)); 
                XafTypesInfo.Instance.RegisterEntity(typeof(UserInfo));
                XafTypesInfo.Instance.RegisterEntity(typeof(StockAnimalInfo));
                XafTypesInfo.Instance.RegisterEntity(typeof(GetBlance));
                XafTypesInfo.Instance.RegisterEntity(typeof(StockSeedInfo));
                XafTypesInfo.Instance.RegisterEntity(typeof(QualityAnalysis));
                XafTypesInfo.Instance.RegisterEntity(typeof(HistoryWork));
                XafTypesInfo.Instance.RegisterEntity(typeof(StockAnimalInfo_Report));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                string requestString = Request.Content.ReadAsStringAsync().Result;
                JObject jObject = (JObject)JsonConvert.DeserializeObject(requestString);
                string TempForageType = string.Empty;
                if (jObject != null)
                {

                    //string[] arr = RefNo.Split('|');
                    //string _refno = arr[0]; //เลขที่อ้างอิง
                    //string _org_oid = arr[1]; //oid หน่วยงาน
                    //string _type = arr[2]; //ประเภทส่ง(2)-รับ(1)

                    string iDate = jObject.SelectToken("UseDate").Value<string>();
                    DateTime oDate = Convert.ToDateTime(iDate);
                    if (jObject.SelectToken("ActivityNameOid") != null)
                    {
                        productUser.SubActivityOid = jObject.SelectToken("ActivityNameOid").Value<string>();
                    }

                    productUser.SupplierUseAnimalProductOid = jObject.SelectToken("supplierUseAnimalProductOid").Value<string>();
                    if (jObject.SelectToken("useNo") != null)
                    {
                        productUser.UseNo = jObject.SelectToken("useNo").Value<string>();
                    }
         
                    productUser.UseDate = oDate.Year + "-" + oDate.Month + "-" + oDate.Day;
                    productUser.FinanceYearOid = jObject.SelectToken("FinanceYearOid").Value<string>();
                    productUser.OrganizationOid = jObject.SelectToken("OrganizationOid").Value<string>();
                    productUser.Remark = jObject.SelectToken("Remark").Value<string>();
                    if (jObject.SelectToken("activityNameOid") == null)
                    {
                        Activity ActivityOid = ObjectSpace.FindObject<Activity>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1 and ActivityName=? ", "เพื่อช่วยเหลือภัยพิบัติ"));
                        productUser.ActivityNameOid = ActivityOid.Oid.ToString();
                    }

                    productUser.CitizenID = jObject.SelectToken("CitizenID").Value<string>();
                    RegisterFarmerController best = new RegisterFarmerController();
                    if (best.CheckCitizenID(jObject.SelectToken("CitizenID").ToString()) == false)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "หมายเลขบัตรประชาชนไม่ถูกต้อง กรุณาตรวจสอบ");
                    }
                    productUser.YearName = jObject.SelectToken("FinanceYear").Value<string>(); ///ใช้สำหรับเจนเลข ออโต้

                    if (jObject.SelectToken("SubActivityLevelOid") != null)
                    { productUser.SubActivityLevelName = jObject.SelectToken("SubActivityLevelOid").Value<string>(); }

                    if (jObject.SelectToken("RegisCusServiceOid") != null)
                    {
                        productUser.RegisCusServiceOid = jObject.SelectToken("RegisCusServiceOid").Value<string>();
                    }
                    if (jObject.SelectToken("OrgeServiceOid") != null)
                    {
                        productUser.OrgeServiceOid = jObject.SelectToken("OrgeServiceOid").Value<string>();
                    }

                    productUser.ServiceCount = jObject.SelectToken("ServiceCount").Value<int>();

                    productUser.PickUp_Type = jObject.SelectToken("PickUpType").Value<string>();
                    if (jObject.SelectToken("FullName") != null)
                    {
                        productUser.ReceiverName = jObject.SelectToken("FullName").Value<string>();
                    }

                    if (jObject.SelectToken("ReceiverNumber") != null || jObject.SelectToken("receiverNumber") != null)
                    {
                        productUser.ReceiverNumber = jObject.SelectToken("ReceiverNumber").Value<string>();
                    }
                    if (jObject.SelectToken("receiverRemark") != null)
                    {
                        productUser.ReceiverRemark = jObject.SelectToken("receiverRemark").Value<string>();
                    }
                    productUser.ReceiverAddress = jObject.SelectToken("FullAddress").Value<string>();
                    Typestatus = jObject.SelectToken("type").Value<int>();
                    Username = jObject.SelectToken("username").Value<string>();
      

                    if (productUser.UseNo == ""|| productUser.UseNo == null) 
                        ///ถ้าไม่มีเลข useno ให้่มันสร้างใหม่ แต่ถ้ามี ไม่ต้องสร้าง
                    {
                        XpoTypesInfoHelper.GetXpoTypeInfoSource();
                        XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.RunNumber));
                        XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.Organization));



                        Organization objORG = ObjectSpace.FindObject<Organization>(CriteriaOperator.Parse("Oid=?", productUser.OrganizationOid));
                        //SendOrderSeed objSupplierProduct = ObjectSpace.FindObject<SendOrderSeed>(CriteriaOperator.Parse("SendNo=?", _refno));
                        /// รอเปลี่ยน
                        RunNumber runningNumber = ObjectSpace.FindObject<RunNumber>(CriteriaOperator.Parse("FormType ='nutrition' and BudgetYear =? and OrgCode=? ", productUser.YearName, objORG.OrganizationCode));
                        if (runningNumber != null)
                        {
                            string customerNumberFormat = string.Empty;
                            int numberfix = Convert.ToInt32(runningNumber.LastNumber) + 1;
                            string Postfix = "000" + numberfix;
                            var FullNumber = objORG.OrganizationCode + "-" + productUser.YearName.Substring(productUser.YearName.Length - 2, 2).PadLeft(2, '0') + "-" + (runningNumber.LastNumber + 1).ToString().PadLeft(6, '0');

                            productUser.UseNo = FullNumber;
                            SqlParameter[] prm2 = new SqlParameter[5];

                            prm2[0] = new SqlParameter("@orgcode", objORG.OrganizationCode);
                            prm2[1] = new SqlParameter("@BudgetYear", productUser.YearName);
                            prm2[2] = new SqlParameter("@LastNumber", numberfix);
                            prm2[3] = new SqlParameter("@FormType", "nutrition");
                            prm2[4] = new SqlParameter("@Type", 1);

                           
                            SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "Insert_RuningNumber", prm2);

                        }
                        else
                        {
                            DataSet ds2;
                            SqlParameter[] prm2 = new SqlParameter[5];

                            prm2[0] = new SqlParameter("@orgcode", objORG.OrganizationCode);
                            prm2[1] = new SqlParameter("@BudgetYear", productUser.YearName);
                            prm2[2] = new SqlParameter("@LastNumber", 1);
                            prm2[3] = new SqlParameter("@FormType", "nutrition");
                            prm2[4] = new SqlParameter("@Type", 0);

                            ds2 = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "Insert_RuningNumber", prm2);


                            productUser.UseNo = objORG.OrganizationCode + "-" + productUser.YearName.Substring(productUser.YearName.Length - 2, 2).PadLeft(2, '0') + "-000001";
                        }
                    }
                    
                    if (productUser.PickUp_Type == "เป็นตัวแทนรับ")
                    {
                        Type = 2;
                    }
                    else
                    {
                        Type = 1;
                    }
                        string TempDescription = "";
                        if (Typestatus == 2) //ส่งให้ผอ.
                        {
                        DataSet ds;
                        SqlParameter[] prm = new SqlParameter[19];
                        prm[0] = new SqlParameter("@UseNo", productUser.UseNo);
                        prm[1] = new SqlParameter("@UseDate", productUser.UseDate);
                        prm[2] = new SqlParameter("@YearName", productUser.YearName);
                        prm[3] = new SqlParameter("@OrganizationOid", productUser.OrganizationOid);
                        prm[4] = new SqlParameter("@Remark", productUser.Remark);
                        prm[5] = new SqlParameter("@ActivityOid", productUser.ActivityNameOid);
                        prm[6] = new SqlParameter("@RegisCusServiceOid", productUser.RegisCusServiceOid);
                        prm[7] = new SqlParameter("@OrgeServiceOid", productUser.OrgeServiceOid);
                        // prm[8] = new SqlParameter("@ServiceCount", productUser.ServiceCount);  
                        prm[8] = new SqlParameter("@CitizenID", productUser.CitizenID);
                        prm[9] = new SqlParameter("@SubActivityOid", productUser.SubActivityOid);
                        prm[10] = new SqlParameter("@SubActivityLevelOid", productUser.SubActivityLevelName);
                        prm[11] = new SqlParameter("@PickUp_Type", Type);
                        prm[12] = new SqlParameter("@oid", productUser.SupplierUseAnimalProductOid);
                        prm[13] = new SqlParameter("@ReceiverName", productUser.ReceiverName);
                        prm[14] = new SqlParameter("@ReceiverAddress", productUser.ReceiverAddress);
                        prm[15] = new SqlParameter("@ReceiverNumber", productUser.ServiceCount);
                        prm[16] = new SqlParameter("@ReceiverRemark", productUser.ReceiverRemark);
                        prm[17] = new SqlParameter("@ServiceCount", productUser.ServiceCount);
                        prm[18] = new SqlParameter("@Type", 0);
             

                        ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MoblieInserts_Calamity_SupplierUseAnimalProduct_Update", prm);

                        SupplierUseAnimalProduct objSupplierUseAnimalProduct = ObjectSpace.FindObject<SupplierUseAnimalProduct>(CriteriaOperator.Parse(" GCRecord is null  and Oid=?  ",productUser.SupplierUseAnimalProductOid));
                            //SupplierUseProductDetail objSupplierUseProductDetail;
                            SupplierUseProductDetail objSupplierUseProductDetail = ObjectSpace.FindObject<SupplierUseProductDetail>(CriteriaOperator.Parse(" GCRecord is null   ", null));
                            UserInfo objUserInfo = ObjectSpace.FindObject<UserInfo>(CriteriaOperator.Parse("[UserName]=?", Username));
                          //  objSupplierUseAnimalProduct.Status = EnumRodBreedProductSeedStatus.Accepet; //1
                            if (productUser.Remark != "")
                            {
                                objSupplierUseAnimalProduct.Remark = productUser.Remark;
                            }
                            if (objSupplierUseAnimalProduct.ActivityOid.ActivityName.Contains("เพื่อช่วยเหลือภัยพิบัติ") == true)
                            {
                                if (objSupplierUseAnimalProduct.RegisCusServiceOid != null)
                                {
                                    RegisterCusService objRegisCusService = ObjectSpace.FindObject<RegisterCusService>(CriteriaOperator.Parse("[Oid]=?", objSupplierUseAnimalProduct.RegisCusServiceOid));
                                    if (objRegisCusService != null)
                                    {
                                        TempDescription = "ช่วยเหลือภัยพิบัติ (Mobile Application)-" + objSupplierUseAnimalProduct.SubActivityLevelOid.ActivityName + " : " + objRegisCusService.DisPlayName;
                                    }
                                    else
                                    {
                                        TempDescription = "ช่วยเหลือภัยพิบัติ (Mobile Application)-" + objSupplierUseAnimalProduct.SubActivityLevelOid.ActivityName + " : ";
                                    }

                                }
                                else if (objSupplierUseAnimalProduct.OrgeServiceOid != null)
                                {
                                    OrgeService objOrgeService = ObjectSpace.FindObject<OrgeService>(CriteriaOperator.Parse("[Oid]=?", objSupplierUseAnimalProduct.OrgeServiceOid));
                                    if (objOrgeService != null)
                                    {
                                        TempDescription = "ช่วยเหลือภัยพิบัติ (Mobile Application)-" + objSupplierUseAnimalProduct.SubActivityLevelOid.ActivityName + " : " + objOrgeService.OrgeServiceName;
                                    
                                    }
                                    else
                                        TempDescription = "ช่วยเหลือภัยพิบัติ (Mobile Application)-" + objSupplierUseAnimalProduct.SubActivityLevelOid.ActivityName + " : ";

                                }
                            }

                            //      'ตัด Stock
                            //////     '=================================================================================
                            string tmpWeightError = "";
                            bool chkAmountOver = false;
                            foreach (SupplierUseAnimalProductDetail row in objSupplierUseAnimalProduct.SupplierUseAnimalProductDetails)
                            {
                                switch (row.AnimalSupplieOid.AnimalSupplieName)
                                {
                                    case "TMR":
                                        {
                                            XPCollection<StockAnimalInfo> objChkStockAnimalLimit = new XPCollection<StockAnimalInfo>(((XPObjectSpace)ObjectSpace).Session);
                                            objChkStockAnimalLimit.Criteria = CriteriaOperator.Parse("[AnimalSupplieOid]=? AND [OrganizationOid]=? and BudgetSourceOid=? and AnimalSupplieTypeOid=? ", row.AnimalSupplieOid, row.SupplierUseAnimalProductOid.OrganizationOid, row.BudgetSourceOid, row.AnimalSupplieTypeOid);
                                            DataSet Ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "sp_StockAnimalInfo_TMR"
                                               , new SqlParameter("@OrganizationOid", row.SupplierUseAnimalProductOid.OrganizationOid)
                                               , new SqlParameter("@AnimalSupplieTypeOid", row.AnimalSupplieTypeOid)
                                               , new SqlParameter("@BudgetSourceOid", row.BudgetSourceOid)
                                               , new SqlParameter("@AnimalSupplieOid", row.AnimalSupplieOid)
                                               , new SqlParameter("@SeedTypeOid", null));
                                            if (Ds.Tables[0].Rows.Count > 0)
                                            {
                                                if (row.Weight > (double)Ds.Tables[0].Rows[0]["Total_Current"])
                                                {
                                                    //   tmpWeightError += "[ประเภทชนิดเสบียงสัตว์ : " + row.AnimalSupplieOid.AnimalSupplieName + "] [ประเภทการผลิตเสบียงสัตว์ : " + row.AnimalSupplieTypeOid.SupplietypeName + "] (คงเหลือ : " + (double)Ds.Tables[0].Rows[0]["Total_Current"]+")" ;

                                                    chkAmountOver = true;
                                                    return Request.CreateResponse(HttpStatusCode.BadRequest, tmpWeightError);
                                                }
                                            }

                                            if ((double)Ds.Tables[0].Rows[0]["Total_Current"] > 0)
                                            {
                                                if (row.Weight > (double)Ds.Tables[0].Rows[0]["Total_Current"])
                                                {
                                                    chkAmountOver = true;
                                                }
                                            }
                                            else
                                            {
                                                return Request.CreateResponse(HttpStatusCode.BadRequest, "เสบียงสัตว์มีจำนวนไม่พอเบิก กรุณากลับไปแก้ไข");
                                            }
                                            break;
                                        }

                                    case "แห้ง":
                                    case "สด":
                                    case "หมัก":
                                        {
                                            var objChkStockAnimalLimit = new XPCollection<StockAnimalInfo>(((XPObjectSpace)ObjectSpace).Session);
                                            if (row.AnimalSupplieTypeOid == null)
                                            {
                                                objChkStockAnimalLimit.Criteria = CriteriaOperator.Parse("[AnimalSupplieOid]=? AND [OrganizationOid]=? and BudgetSourceOid=? AND [SeedTypeOid]=?", row.AnimalSupplieOid, row.SupplierUseAnimalProductOid.OrganizationOid, row.BudgetSourceOid, row.SeedTypeOid);
                                            }
                                            else
                                            {
                                                objChkStockAnimalLimit.Criteria = CriteriaOperator.Parse("[AnimalSupplieOid]=? AND [OrganizationOid]=? and BudgetSourceOid=? and AnimalSupplieTypeOid=? AND [SeedTypeOid]=?", row.AnimalSupplieOid, row.SupplierUseAnimalProductOid.OrganizationOid, row.BudgetSourceOid, row.AnimalSupplieTypeOid, row.SeedTypeOid);
                                            }

                                            var GetStockRodBreedInfo = objChkStockAnimalLimit.GroupBy(Filed => Filed.AnimalSupplieOid).Select(TmpStockRodBreedInfo => new GetBlance()
                                            {
                                                Name = TmpStockRodBreedInfo.First().AnimalSupplieOid.AnimalSupplieName,
                                                Total = Convert.ToDecimal(TmpStockRodBreedInfo.Sum(c => c.Weight).ToString())
                                            }).ToList();
                                            if (GetStockRodBreedInfo.Count > 0)
                                            {
                                                if (row.Weight > Convert.ToDouble(GetStockRodBreedInfo[0].Total))
                                                {
                                                    // tmpWeightError += "[ประเภทชนิดเสบียงสัตว์ : " + row.AnimalSupplieOid.AnimalSupplieName + "] [ประเภทการผลิตเสบียงสัตว์ : " + row.AnimalSupplieTypeOid.SupplietypeName + "] [ประเภทเสบียงสัตว์ : " + row.SeedTypeOid.SeedTypeName + "] (คงเหลือ : " + GetStockRodBreedInfo[0].Total + ")" + (char)13 + (char)10 + (char)13 + (char)10;
                                                    chkAmountOver = true;
                                                }
                                            }
                                            else
                                            {
                                                return Request.CreateResponse(HttpStatusCode.BadRequest, "เสบียงสัตว์มีจำนวนไม่พอเบิก กรุณากลับไปแก้ไข");
                                            }


                                            break;
                                        }

                                }
                                ////                    // =================================================================================

                                if (chkAmountOver == false)
                                {
                                    ////                        // Stock การเบิกใช้
                                    ////                        // ==========================================
                                    var objOrganizationOid = row.SupplierUseAnimalProductOid.OrganizationOid;
                                    var objAnimalSupplieOid = row.AnimalSupplieOid;
                                    var objAnimalSupplieTypeOid = row.AnimalSupplieTypeOid;
                                    var objQuotaType = row.QuotaTypeOid;
                                    var objManageSubAnimalSupplierOid = row.ManageSubAnimalSupplierOid;
                                    var objInsStockAnimalUseInfo = ObjectSpace.CreateObject<StockAnimalUseInfo>();

                                    objInsStockAnimalUseInfo.OrganizationOid = objOrganizationOid;
                                    objInsStockAnimalUseInfo.TransactionDate = DateTime.Now;
                                    objInsStockAnimalUseInfo.AnimalSupplieOid = objAnimalSupplieOid;
                                    objInsStockAnimalUseInfo.AnimalSupplieTypeOid = objAnimalSupplieTypeOid;
                                    objInsStockAnimalUseInfo.QuotaTypeOid = objQuotaType;
                                    objInsStockAnimalUseInfo.ManageSubAnimalSupplierOid = objManageSubAnimalSupplierOid;
                                    // .AnimalSeedOid = ObjAnimalSeedOid
                                    objInsStockAnimalUseInfo.BudgetSourceOid = row.BudgetSourceOid; 
                                    objInsStockAnimalUseInfo.Weight = row.Weight;
                                    objInsStockAnimalUseInfo.Remark = "อนุมัติใช้เสบียงสัตว์ (Mobile Application)";
                                    objInsStockAnimalUseInfo.ActivityOid = row.SupplierUseAnimalProductOid.ActivityOid;
                                    objInsStockAnimalUseInfo.SubActivityOid = row.SupplierUseAnimalProductOid.SubActivityOid;
                                    objInsStockAnimalUseInfo.FinanceYearOid = row.SupplierUseAnimalProductOid.FinanceYearOid;
                                    objInsStockAnimalUseInfo.SupplierUseAnimalDetailOid = row;
                                    objInsStockAnimalUseInfo.SeedTypeOid = row.SeedTypeOid;
                                    objInsStockAnimalUseInfo.Description = TempDescription;
                                    objInsStockAnimalUseInfo.AnimalUseNumber = objSupplierUseAnimalProduct.UseNo;
                                    objInsStockAnimalUseInfo.IsApprove = false;

                                    //                        // Stock หลัก
                                    //                        // ==========================================
                                    var objStockAnimalInfo = ObjectSpace.CreateObject<StockAnimalInfo>();
                                    objStockAnimalInfo.AnimalProductNumber = objSupplierUseAnimalProduct.UseNo;
                                    objStockAnimalInfo.AnimalSupplieOid = row.AnimalSupplieOid;
                                    objStockAnimalInfo.FinanceYearOid = objSupplierUseAnimalProduct.FinanceYearOid;
                                    objStockAnimalInfo.BudgetSourceOid = row.BudgetSourceOid;
                                    objStockAnimalInfo.OrganizationOid = objSupplierUseAnimalProduct.OrganizationOid;
                                    objStockAnimalInfo.AnimalSupplieTypeOid = row.AnimalSupplieTypeOid;
                                    // .AnimalSeedOid = row.AnimalSeedOid
                                    objStockAnimalInfo.Weight = 0 - row.Weight;
                                    objStockAnimalInfo.Remark = "ยอดใช้เสบียงสัตว์ (Mobile Application)";
                                    objStockAnimalInfo.SeedTypeOid = row.SeedTypeOid;
                                    objStockAnimalInfo.Description = TempDescription;
                                    objStockAnimalInfo.IsApprove = false;
                                    // View.ObjectSpace.CommitChanges();
                                    // ==========================================

                                    // 'Stock สำหรับ กปศ4ว
                                    // ==========================================
                                    var objStockAnimalInfo_Detail = ObjectSpace.GetObjects<StockAnimalInfo_Report>();
                                    if (row.SeedTypeOid != null)
                                    {
                                        objStockAnimalInfo_Detail = ObjectSpace.GetObjects<StockAnimalInfo_Report>(CriteriaOperator.Parse("[BudgetSourceOid]=? and [OrganizationOid]=? and [AnimalSupplieOid]=? and [AnimalSupplieTypeOid]=? and [SeedTypeOid]=?", row.BudgetSourceOid, objSupplierUseAnimalProduct.OrganizationOid, row.AnimalSupplieOid, row.AnimalSupplieTypeOid, row.SeedTypeOid));
                                    }
                                    else
                                    {
                                        objStockAnimalInfo_Detail = ObjectSpace.GetObjects<StockAnimalInfo_Report>(CriteriaOperator.Parse("[BudgetSourceOid]=? and [OrganizationOid]=? and [AnimalSupplieOid]=? and [AnimalSupplieTypeOid]=?", row.BudgetSourceOid, objSupplierUseAnimalProduct.OrganizationOid, row.AnimalSupplieOid, row.AnimalSupplieTypeOid));
                                    }

                                    var objStockAnimalInfo_DetailNew = ObjectSpace.CreateObject<StockAnimalInfo_Report>();
                                    if (objStockAnimalInfo_Detail.Count > 0)
                                    {
                                        var ObjStockAnimalInfo_DetailSource = from Item in objStockAnimalInfo_Detail
                                                                              orderby Item.TransactionDate descending
                                                                              select Item;
                                        objStockAnimalInfo_DetailNew.AnimalProductNumber = objSupplierUseAnimalProduct.UseNo;
                                        objStockAnimalInfo_DetailNew.FinanceYearOid = objSupplierUseAnimalProduct.FinanceYearOid;
                                        objStockAnimalInfo_DetailNew.BudgetSourceOid = row.BudgetSourceOid;
                                        objStockAnimalInfo_DetailNew.OrganizationOid = objSupplierUseAnimalProduct.OrganizationOid;
                                        objStockAnimalInfo_DetailNew.AnimalSupplieOid = objAnimalSupplieOid;
                                        objStockAnimalInfo_DetailNew.AnimalSupplieTypeOid = objAnimalSupplieTypeOid;
                                        objStockAnimalInfo_DetailNew.TotalForward = ObjStockAnimalInfo_DetailSource.ElementAtOrDefault(0).TotalWeight;
                                        objStockAnimalInfo_DetailNew.TotalChange = 0 - row.Weight;
                                        objStockAnimalInfo_DetailNew.SeedTypeOid = row.SeedTypeOid;
                                        objStockAnimalInfo_DetailNew.Description = TempDescription;
                                        objStockAnimalInfo_DetailNew.IsApprove = false;
                                    }
                                    // ==========================================
                                }

                            }
                            if (chkAmountOver == true)
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "เสบียงสัตว์มีจำนวนไม่พอเบิก กรุณากลับไปแก้ไข");
                            }

                            objSupplierUseAnimalProduct.Status = EnumRodBreedProductSeedStatus.Accepet;
                            objSupplierUseAnimalProduct.ReasonStatus_Use = 0;
                            objSupplierUseAnimalProduct.ReasonMsg_Use = productUser.Remark;

                            // View.ObjectSpace.CommitChanges()

                            HistoryWork ObjHistory = null;
                            ObjHistory = ObjectSpace.CreateObject<HistoryWork>();
                            // ประวัติ
                            ObjHistory.RefOid = objSupplierUseAnimalProduct.Oid.ToString();
                            ObjHistory.FormName = "เสบียงสัตว์";
                            ObjHistory.Message = "ส่งให้ ผอ.อนุมัติ (ขอเบิกเสบียงสัตว์ (Mobile Application)) ลำดับที่ : " + objSupplierUseAnimalProduct.UseNo;
                            ObjHistory.CreateBy = objUserInfo.UserName;
                            ObjHistory.CreateDate = DateTime.Now;
                            ObjectSpace.CommitChanges();

                            UpdateResult ret = new UpdateResult();
                            item.supplieruseanimalproductoid = ds.Tables[1].Rows[0]["oid"].ToString();
                            item.useno = ds.Tables[1].Rows[0]["UseNo"].ToString();
                            productUser.UseNo = productUser.UseNo;
                          //  directProvider.Dispose();
                           // ObjectSpace.Dispose();
                            return Request.CreateResponse(HttpStatusCode.OK, item);
                        }
                       else if (Typestatus == 0) //บันทึกเฉยๆ ไม่ได้ส่งให้ผอ.
                    {
                        DataSet ds;
                        SqlParameter[] prm = new SqlParameter[19];
                        prm[0] = new SqlParameter("@UseNo", productUser.UseNo);
                        prm[1] = new SqlParameter("@UseDate", productUser.UseDate);
                        prm[2] = new SqlParameter("@YearName", productUser.YearName);
                        prm[3] = new SqlParameter("@OrganizationOid", productUser.OrganizationOid);
                        prm[4] = new SqlParameter("@Remark", productUser.Remark);
                        prm[5] = new SqlParameter("@ActivityOid", productUser.ActivityNameOid);
                        prm[6] = new SqlParameter("@RegisCusServiceOid", productUser.RegisCusServiceOid);
                        prm[7] = new SqlParameter("@OrgeServiceOid", productUser.OrgeServiceOid);
                        // prm[8] = new SqlParameter("@ServiceCount", productUser.ServiceCount);  
                        prm[8] = new SqlParameter("@CitizenID", productUser.CitizenID);
                        prm[9] = new SqlParameter("@SubActivityOid", productUser.SubActivityOid);
                        prm[10] = new SqlParameter("@SubActivityLevelOid", productUser.SubActivityLevelName);
                        prm[11] = new SqlParameter("@PickUp_Type", Type);
                        prm[12] = new SqlParameter("@oid", productUser.SupplierUseAnimalProductOid);
                        prm[13] = new SqlParameter("@ReceiverName", productUser.ReceiverName);
                        prm[14] = new SqlParameter("@ReceiverAddress", productUser.ReceiverAddress);
                        prm[15] = new SqlParameter("@ReceiverNumber", productUser.ServiceCount);
                        prm[16] = new SqlParameter("@ReceiverRemark", productUser.ReceiverRemark);
                        prm[17] = new SqlParameter("@ServiceCount", productUser.ServiceCount);
                        prm[18] = new SqlParameter("@Type", Typestatus);

                        ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MoblieInserts_Calamity_SupplierUseAnimalProduct_Update", prm);

                        //productUser.SupplierUseAnimalProductOid ;
                        item.supplieruseanimalproductoid = ds.Tables[1].Rows[0]["oid"].ToString();
                        item.useno = ds.Tables[1].Rows[0]["UseNo"].ToString();
                        productUser.UseNo = productUser.UseNo;
                        //return Request.CreateResponse(HttpStatusCode.OK);
                        return Request.CreateResponse(HttpStatusCode.OK, item);
                    }
                    
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, productUser);
                    }

                }

                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "กรอกข้อมูลไม่ครบ");
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
      

  

        /// <summary>
        /// อนุมัติ-ไม่อนุมัติการช่วยเหลือภัยพิบัติ เสบียงสัตว์ ฟั่งชั่นของปุ่มในหน้าช่วยเหลือภัยพิบัติ
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("ApprovalDisasterSupplierUseAnimalProduct/Update")]/// ใช้สร้างใหม่
        public HttpResponseMessage UpdateDisasterSupplierUseAnimalProduct()  ///SupplierUseAnimalProduct/Update
        {
            string TempDescription = "";
            string Remark = "";
            _Registerfarmer Registerfarmer = new _Registerfarmer();
            try
            {
                string SupplierUseAnimalProductOid = HttpContext.Current.Request.Form["supplierUseAnimalProductOid"].ToString();

                string Status = HttpContext.Current.Request.Form["Status"].ToString(); //สถานะ
                if (HttpContext.Current.Request.Form["Remark"] != null)
                {
                    Remark = HttpContext.Current.Request.Form["Remark"].ToString();
                    //หมายเหตุ
                }
                string Username = HttpContext.Current.Request.Form["username"].ToString();
                
                if (SupplierUseAnimalProductOid != "" && Status != "")
                {


                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierUseAnimalProduct));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierUseProductDetail));
                    XafTypesInfo.Instance.RegisterEntity(typeof(UserInfo));
                    XafTypesInfo.Instance.RegisterEntity(typeof(StockAnimalInfo));
                    XafTypesInfo.Instance.RegisterEntity(typeof(GetBlance));
                    XafTypesInfo.Instance.RegisterEntity(typeof(StockSeedInfo));
                    XafTypesInfo.Instance.RegisterEntity(typeof(QualityAnalysis));
                    XafTypesInfo.Instance.RegisterEntity(typeof(HistoryWork));
                    XafTypesInfo.Instance.RegisterEntity(typeof(StockAnimalInfo_Report));
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);

                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                    SupplierUseAnimalProduct objSupplierUseAnimalProduct = ObjectSpace.FindObject<SupplierUseAnimalProduct>(CriteriaOperator.Parse(" GCRecord is null  and Oid=?  ", SupplierUseAnimalProductOid));
                    //SupplierUseProductDetail objSupplierUseProductDetail;
                    SupplierUseProductDetail objSupplierUseProductDetail = ObjectSpace.FindObject<SupplierUseProductDetail>(CriteriaOperator.Parse(" GCRecord is null   ", null));

                    ///    spt_UpdateOidSupplierUseProductDetail
                    if (objSupplierUseAnimalProduct.Oid != null)
                    {

                        if (Status == "0") //บันทึก
                        { //รอการตรวจสอบ                    

                            //objSupplierUseProductDetail.SupplierUseProduct = ;
                            objSupplierUseAnimalProduct.Status = EnumRodBreedProductSeedStatus.InProgess; //0
                            if (Remark != "")
                            {
                                objSupplierUseAnimalProduct.Remark = Remark;
                            }
                            ObjectSpace.CommitChanges();
                        }
                        else if (Status == "2")
                        { // อนุมัติให้ ผ.อ. 
                            UserInfo objUserInfo = ObjectSpace.FindObject<UserInfo>(CriteriaOperator.Parse("[UserName]=?", Username));
                            objSupplierUseAnimalProduct.Status = EnumRodBreedProductSeedStatus.Accepet; //1
                            if (Remark != "")
                            {
                                objSupplierUseAnimalProduct.Remark = Remark;
                            }
                            if (objSupplierUseAnimalProduct.ActivityOid.ActivityName.Contains("เพื่อช่วยเหลือภัยพิบัติ") == true)
                            {
                                if (objSupplierUseAnimalProduct.RegisCusServiceOid != null)
                                {
                                    RegisterCusService objRegisCusService = ObjectSpace.FindObject<RegisterCusService>(CriteriaOperator.Parse("[Oid]=?", objSupplierUseAnimalProduct.RegisCusServiceOid));
                                    if (objRegisCusService != null)
                                    {
                                        TempDescription = "ช่วยเหลือภัยพิบัติ-" + objSupplierUseAnimalProduct.SubActivityLevelOid.ActivityName + " : " + objRegisCusService.DisPlayName;
                                    }
                                    else
                                    {
                                        TempDescription = "ช่วยเหลือภัยพิบัติ-" + objSupplierUseAnimalProduct.SubActivityLevelOid.ActivityName + " : ";
                                    }

                                }
                                else if (objSupplierUseAnimalProduct.OrgeServiceOid != null)
                                {
                                    OrgeService objOrgeService = ObjectSpace.FindObject<OrgeService>(CriteriaOperator.Parse("[Oid]=?", objSupplierUseAnimalProduct.OrgeServiceOid));
                                    if (objOrgeService != null)
                                    {
                                        TempDescription = "ช่วยเหลือภัยพิบัติ-" + objSupplierUseAnimalProduct.SubActivityLevelOid.ActivityName + " : "+ objOrgeService.OrgeServiceName;
                                    }
                                    else
                                        TempDescription = "ช่วยเหลือภัยพิบัติ-" + objSupplierUseAnimalProduct.SubActivityLevelOid.ActivityName + " : ";

                                }
                            }

                            //      'ตัด Stock
                            //////     '=================================================================================
                            string tmpWeightError = "";
                            bool chkAmountOver = false;
                            foreach (SupplierUseAnimalProductDetail row in objSupplierUseAnimalProduct.SupplierUseAnimalProductDetails)
                            {
                                switch (row.AnimalSupplieOid.AnimalSupplieName)
                                {
                                    case "TMR":
                                        {
                                            XPCollection<StockAnimalInfo> objChkStockAnimalLimit = new XPCollection<StockAnimalInfo>(((XPObjectSpace)ObjectSpace).Session);
                                            objChkStockAnimalLimit.Criteria = CriteriaOperator.Parse("[AnimalSupplieOid]=? AND [OrganizationOid]=? and BudgetSourceOid=? and AnimalSupplieTypeOid=? ", row.AnimalSupplieOid, row.SupplierUseAnimalProductOid.OrganizationOid, row.BudgetSourceOid, row.AnimalSupplieTypeOid);
                                            DataSet Ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "sp_StockAnimalInfo_TMR"
                                               , new SqlParameter("@OrganizationOid", row.SupplierUseAnimalProductOid.OrganizationOid)
                                               , new SqlParameter("@AnimalSupplieTypeOid", row.AnimalSupplieTypeOid)
                                               , new SqlParameter("@BudgetSourceOid", row.BudgetSourceOid)
                                               , new SqlParameter("@AnimalSupplieOid", row.AnimalSupplieOid)
                                               , new SqlParameter("@SeedTypeOid", null));
                                            if (Ds.Tables[0].Rows.Count > 0)
                                            {
                                                if (row.Weight > (double)Ds.Tables[0].Rows[0]["Total_Current"])
                                                {
                                                    //   tmpWeightError += "[ประเภทชนิดเสบียงสัตว์ : " + row.AnimalSupplieOid.AnimalSupplieName + "] [ประเภทการผลิตเสบียงสัตว์ : " + row.AnimalSupplieTypeOid.SupplietypeName + "] (คงเหลือ : " + (double)Ds.Tables[0].Rows[0]["Total_Current"]+")" ;

                                                    chkAmountOver = true;
                                                    return Request.CreateResponse(HttpStatusCode.BadRequest, tmpWeightError);
                                                }
                                            }
                                            //var GetStockRodBreedInfo = objChkStockAnimalLimit.GroupBy(Filed => Filed.AnimalSupplieOid).Select(TmpStockRodBreedInfo => new GetBlance()
                                            //{
                                            //    Name = TmpStockRodBreedInfo.First().AnimalSupplieTypeOid.SupplietypeName,
                                            //    Total = Convert.ToDecimal(TmpStockRodBreedInfo.Sum(c => c.Weight).ToString())
                                            //}).ToList();
                                            if ((double)Ds.Tables[0].Rows[0]["Total_Current"] > 0)
                                            {
                                                if (row.Weight > (double)Ds.Tables[0].Rows[0]["Total_Current"])
                                                {
                                                    //tmpWeightError += "[ประเภทชนิดเสบียงสัตว์ : " + row.AnimalSupplieOid.AnimalSupplieName + "] [ประเภทการผลิตเสบียงสัตว์ : " + row.AnimalSupplieTypeOid.SupplietypeName + "] (คงเหลือ : " + GetStockRodBreedInfo[0].Total + ")" + (char)13 + (char)10 + (char)13 + (char)10;
                                                    chkAmountOver = true;
                                                }
                                            }
                                            else
                                            {
                                                return Request.CreateResponse(HttpStatusCode.BadRequest, "เมล็ดพันธุ์มีจำนวนไม่พอเบิก");
                                            }
                                            break;
                                        }


                                    case "แห้ง":
                                    case "สด":
                                    case "หมัก":
                                        {
                                    var objChkStockAnimalLimit = new XPCollection<StockAnimalInfo>(((XPObjectSpace)ObjectSpace).Session);
                                            if (row.AnimalSupplieTypeOid == null)
                                            {
                                                objChkStockAnimalLimit.Criteria = CriteriaOperator.Parse("[AnimalSupplieOid]=? AND [OrganizationOid]=? and BudgetSourceOid=? AND [SeedTypeOid]=?", row.AnimalSupplieOid, row.SupplierUseAnimalProductOid.OrganizationOid, row.BudgetSourceOid, row.SeedTypeOid);
                                            }
                                            else
                                            {
                                                objChkStockAnimalLimit.Criteria = CriteriaOperator.Parse("[AnimalSupplieOid]=? AND [OrganizationOid]=? and BudgetSourceOid=? and AnimalSupplieTypeOid=? AND [SeedTypeOid]=?", row.AnimalSupplieOid, row.SupplierUseAnimalProductOid.OrganizationOid, row.BudgetSourceOid, row.AnimalSupplieTypeOid, row.SeedTypeOid);
                                            }

                                            var GetStockRodBreedInfo = objChkStockAnimalLimit.GroupBy(Filed => Filed.AnimalSupplieOid).Select(TmpStockRodBreedInfo => new GetBlance()
                                    {
                                        Name = TmpStockRodBreedInfo.First().AnimalSupplieOid.AnimalSupplieName,
                                        Total = Convert.ToDecimal(TmpStockRodBreedInfo.Sum(c => c.Weight).ToString())
                                    }).ToList();
                                    if (GetStockRodBreedInfo.Count > 0)
                                    {
                                        if (row.Weight > Convert.ToDouble(GetStockRodBreedInfo[0].Total))
                                        {
                                           // tmpWeightError += "[ประเภทชนิดเสบียงสัตว์ : " + row.AnimalSupplieOid.AnimalSupplieName + "] [ประเภทการผลิตเสบียงสัตว์ : " + row.AnimalSupplieTypeOid.SupplietypeName + "] [ประเภทเสบียงสัตว์ : " + row.SeedTypeOid.SeedTypeName + "] (คงเหลือ : " + GetStockRodBreedInfo[0].Total + ")" + (char)13 + (char)10 + (char)13 + (char)10;
                                            chkAmountOver = true;
                                        }
                                    }
                                    else
                                    {
                                        return Request.CreateResponse(HttpStatusCode.BadRequest, "เมล็ดพันธุ์มีจำนวนไม่พอเบิก");
                                    }


                                    break;
                                }

                            }
                            ////                    // =================================================================================

                            if (chkAmountOver == false)
                                {
                                    ////                        // Stock การเบิกใช้
                                    ////                        // ==========================================
                                    var objOrganizationOid = row.SupplierUseAnimalProductOid.OrganizationOid;
                            var objAnimalSupplieOid = row.AnimalSupplieOid;
                            var objAnimalSupplieTypeOid = row.AnimalSupplieTypeOid;
                            var objQuotaType = row.QuotaTypeOid;
                            var objManageSubAnimalSupplierOid = row.ManageSubAnimalSupplierOid;
                            var objInsStockAnimalUseInfo = ObjectSpace.CreateObject<StockAnimalUseInfo>();
                            objInsStockAnimalUseInfo.OrganizationOid = objOrganizationOid;
                            objInsStockAnimalUseInfo.TransactionDate = DateTime.Now;
                            objInsStockAnimalUseInfo.AnimalSupplieOid = objAnimalSupplieOid;
                            objInsStockAnimalUseInfo.AnimalSupplieTypeOid = objAnimalSupplieTypeOid;
                            objInsStockAnimalUseInfo.QuotaTypeOid = objQuotaType;
                            objInsStockAnimalUseInfo.ManageSubAnimalSupplierOid = objManageSubAnimalSupplierOid;
                            // .AnimalSeedOid = ObjAnimalSeedOid
                            objInsStockAnimalUseInfo.BudgetSourceOid = row.BudgetSourceOid;
                            objInsStockAnimalUseInfo.Weight = row.Weight;
                            objInsStockAnimalUseInfo.Remark = "อนุมัติใช้เสบียงสัตว์";
                            objInsStockAnimalUseInfo.ActivityOid = row.SupplierUseAnimalProductOid.ActivityOid;
                            objInsStockAnimalUseInfo.SubActivityOid = row.SupplierUseAnimalProductOid.SubActivityOid;
                            objInsStockAnimalUseInfo.FinanceYearOid = row.SupplierUseAnimalProductOid.FinanceYearOid;
                            objInsStockAnimalUseInfo.SupplierUseAnimalDetailOid = row;
                            objInsStockAnimalUseInfo.SeedTypeOid = row.SeedTypeOid;
                            objInsStockAnimalUseInfo.Description = TempDescription;
                            objInsStockAnimalUseInfo.AnimalUseNumber = objSupplierUseAnimalProduct.UseNo;
                            objInsStockAnimalUseInfo.IsApprove = false;
                            //                        // 'เช็คปริมาณคงเหลือก่อนเบิกใช้

                            //                        // Stock หลัก
                            //                        // ==========================================
                            var objStockAnimalInfo = ObjectSpace.CreateObject<StockAnimalInfo>();
                            objStockAnimalInfo.AnimalProductNumber = objSupplierUseAnimalProduct.UseNo;
                            objStockAnimalInfo.AnimalSupplieOid = row.AnimalSupplieOid;
                            objStockAnimalInfo.FinanceYearOid = objSupplierUseAnimalProduct.FinanceYearOid;
                            objStockAnimalInfo.BudgetSourceOid = row.BudgetSourceOid;
                            objStockAnimalInfo.OrganizationOid = objSupplierUseAnimalProduct.OrganizationOid;
                            objStockAnimalInfo.AnimalSupplieTypeOid = row.AnimalSupplieTypeOid;
                            // .AnimalSeedOid = row.AnimalSeedOid
                            objStockAnimalInfo.Weight = 0 - row.Weight;
                            objStockAnimalInfo.Remark = "ยอดใช้เสบียงสัตว์";
                            objStockAnimalInfo.SeedTypeOid = row.SeedTypeOid;
                            objStockAnimalInfo.Description = TempDescription;
                            objStockAnimalInfo.IsApprove = false;
                            // View.ObjectSpace.CommitChanges();
                            // ==========================================

                            // 'Stock สำหรับ กปศ4ว
                            // ==========================================
                            var objStockAnimalInfo_Detail = ObjectSpace.GetObjects<StockAnimalInfo_Report>();
                            if (row.SeedTypeOid is object)
                            {
                                objStockAnimalInfo_Detail = ObjectSpace.GetObjects<StockAnimalInfo_Report>(CriteriaOperator.Parse("[BudgetSourceOid]=? and [OrganizationOid]=? and [AnimalSupplieOid]=? and [AnimalSupplieTypeOid]=? and [SeedTypeOid]=?", row.BudgetSourceOid, objSupplierUseAnimalProduct.OrganizationOid, row.AnimalSupplieOid, row.AnimalSupplieTypeOid, row.SeedTypeOid));
                            }
                            else
                            {
                                objStockAnimalInfo_Detail = ObjectSpace.GetObjects<StockAnimalInfo_Report>(CriteriaOperator.Parse("[BudgetSourceOid]=? and [OrganizationOid]=? and [AnimalSupplieOid]=? and [AnimalSupplieTypeOid]=?", row.BudgetSourceOid, objSupplierUseAnimalProduct.OrganizationOid, row.AnimalSupplieOid, row.AnimalSupplieTypeOid));
                            }

                            var objStockAnimalInfo_DetailNew = ObjectSpace.CreateObject<StockAnimalInfo_Report>();
                            if (objStockAnimalInfo_Detail.Count > 0)
                            {
                                var ObjStockAnimalInfo_DetailSource = from Item in objStockAnimalInfo_Detail
                                                                      orderby Item.TransactionDate descending
                                                                      select Item;
                                objStockAnimalInfo_DetailNew.AnimalProductNumber = objSupplierUseAnimalProduct.UseNo;
                                objStockAnimalInfo_DetailNew.FinanceYearOid = objSupplierUseAnimalProduct.FinanceYearOid;
                                objStockAnimalInfo_DetailNew.BudgetSourceOid = row.BudgetSourceOid;
                                objStockAnimalInfo_DetailNew.OrganizationOid = objSupplierUseAnimalProduct.OrganizationOid;
                                objStockAnimalInfo_DetailNew.AnimalSupplieOid = objAnimalSupplieOid;
                                objStockAnimalInfo_DetailNew.AnimalSupplieTypeOid = objAnimalSupplieTypeOid;
                                objStockAnimalInfo_DetailNew.TotalForward = ObjStockAnimalInfo_DetailSource.ElementAtOrDefault(0).TotalWeight;
                                objStockAnimalInfo_DetailNew.TotalChange = 0 - row.Weight;
                                objStockAnimalInfo_DetailNew.SeedTypeOid = row.SeedTypeOid;
                                objStockAnimalInfo_DetailNew.Description = TempDescription;
                                objStockAnimalInfo_DetailNew.IsApprove = false;
                            }
                            // ==========================================
                        }

                    }
                    if (chkAmountOver == true)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "เมล็ดพันธุ์มีจำนวนไม่พอเบิก");
                    }
                    objSupplierUseAnimalProduct.Status = EnumRodBreedProductSeedStatus.Accepet;
                    objSupplierUseAnimalProduct.ReasonStatus_Use = 0;
                    objSupplierUseAnimalProduct.ReasonMsg_Use = Remark;

                    // View.ObjectSpace.CommitChanges()

                    HistoryWork ObjHistory = null;
                            ObjHistory = ObjectSpace.CreateObject<HistoryWork>();
                            // ประวัติ
                            ObjHistory.RefOid = objSupplierUseAnimalProduct.Oid.ToString();
                            ObjHistory.FormName = "เสบียงสัตว์";
                            ObjHistory.Message = "ส่งให้ ผอ.อนุมัติ (ขอเบิกเสบียงสัตว์) ลำดับที่ : " + objSupplierUseAnimalProduct.UseNo;
                            ObjHistory.CreateBy = objUserInfo.UserName;
                            ObjHistory.CreateDate = DateTime.Now;
                           ObjectSpace.CommitChanges();

                        }

                        UpdateResult ret = new UpdateResult();
                        ret.status = "true";
                        ret.message = "ยืนยันการส่งเรียบร้อยแล้ว";
                        directProvider.Dispose();
                        ObjectSpace.Dispose();
                        return Request.CreateResponse(HttpStatusCode.OK, ret);

                    }
                    else
                    {
                        UserError err = new UserError();
                        err.status = "false";
                        err.code = "-1";
                        err.message = "ไม่พบข้อมูล";
                        return Request.CreateResponse(HttpStatusCode.NotFound, err);
                    }
                }
                else
                {
                    UserError err = new UserError();
                    err.status = "false";
                    err.code = "0";
                    err.message = "กรุณาใส่ข้อมูล RefNo ให้เรียบร้อยก่อน";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
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
                Dispose();
            }
        }
        /// <summary>
        /// อัพเดทอนุมัติการใช้เสบียงสัตว์
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SupplierUseAnimal/Update")] //SupplierUseAnimal/Update here
        public HttpResponseMessage SupplierUseAnimal_Update()  ///SupplierUseAnimalProduct/Update
        {
            object objDetailService = null;
            string Username = "";
            _Registerfarmer Registerfarmer = new _Registerfarmer();
            try
            {
                string CancelMsg = "";
  
                string RefNo = HttpContext.Current.Request.Form["RefNo"].ToString(); //ข้อมูลเลขที่อ้างอิง
                string Status = HttpContext.Current.Request.Form["Status"].ToString(); //สถานะ
                if (CancelMsg != null)
                {
                    CancelMsg = HttpContext.Current.Request.Form["Remark"].ToString(); //หมายเหตุ
                }

                Username = HttpContext.Current.Request.Form["Username"].ToString();
                //  string Approve = HttpContext.Current.Request.Form["approve"].ToString();
                if (RefNo != "" && Status != "")
                {
                    string[] arr = RefNo.Split('|');
                    string _refno = arr[0]; //เลขที่อ้างอิง
                    string _org_oid = arr[1]; //oid หน่วยงาน
                    string _type = arr[2]; //ประเภทส่ง(2)-รับ(1)

                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierUseAnimalProduct));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.StockAnimalInfo_Report));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.HistoryWork));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.StockAnimalUseInfo));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.StockAnimalInfo));
                    XafTypesInfo.Instance.RegisterEntity(typeof(ReceiveLotNumber));
                    XafTypesInfo.Instance.RegisterEntity(typeof(RegisterCusServiceDetail));
                    XafTypesInfo.Instance.RegisterEntity(typeof(OrgeServiceDetail));
                    XafTypesInfo.Instance.RegisterEntity(typeof(ServiceType));

                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                    SupplierUseAnimalProduct ObjMaster = ObjectSpace.FindObject<SupplierUseAnimalProduct>(CriteriaOperator.Parse("UseNo=?  ", _refno));


                    if (ObjMaster != null)
                    {

                        if (Status == "1") //เคสอนุมัติ
                        {
                            if (ObjMaster.Status == EnumRodBreedProductSeedStatus.Accepet)
                            {
                                foreach (SupplierUseAnimalProductDetail row in ObjMaster.SupplierUseAnimalProductDetails)
                                {
                                    if (ObjMaster.Status == EnumRodBreedProductSeedStatus.Accepet)
                                    {
                                        var objOrganizationOid = row.SupplierUseAnimalProductOid.OrganizationOid;
                                        var objAnimalSupplieOid = row.AnimalSupplieOid;
                                        var objAnimalSupplieTypeOid = row.AnimalSupplieTypeOid;
                                        var objQuotaType = row.QuotaTypeOid;
                                        var objManageSubAnimalSupplierOid = row.ManageSubAnimalSupplierOid;
                                        var ObjSeedTypeOid = row.SeedTypeOid;
                                        StockAnimalUseInfo objStockAnimalUseInfoEdit = null;
                                        StockAnimalInfo objStockAnimalInfoEdit = null;

                                        if (row.QuotaTypeOid != null)
                                        {
                                            if (row.ManageSubAnimalSupplierOid != null)
                                            {
                                                objStockAnimalUseInfoEdit = ObjectSpace.FindObject<StockAnimalUseInfo>(CriteriaOperator.Parse("OrganizationOid=? and AnimalSupplieOid=? and AnimalSupplieTypeOid=? and QuotaTypeOid=? and ManageSubAnimalSupplierOid=? and SeedTypeOid=? and AnimalUseNumber=?", objOrganizationOid, objAnimalSupplieOid, objAnimalSupplieTypeOid, objQuotaType, objManageSubAnimalSupplierOid, ObjSeedTypeOid, ObjMaster.UseNo));
                                            }
                                            else
                                            {
                                                objStockAnimalUseInfoEdit = ObjectSpace.FindObject<StockAnimalUseInfo>(CriteriaOperator.Parse("OrganizationOid=? and AnimalSupplieOid=? and AnimalSupplieTypeOid=? and SeedTypeOid=? and AnimalUseNumber=?", objOrganizationOid, objAnimalSupplieOid, objAnimalSupplieTypeOid, ObjSeedTypeOid, ObjMaster.UseNo));

                                            }
                                            objStockAnimalInfoEdit = ObjectSpace.FindObject<StockAnimalInfo>(CriteriaOperator.Parse("OrganizationOid=? and AnimalSupplieOid=? and AnimalSupplieTypeOid=? and SeedTypeOid=? and AnimalProductNumber=?", objOrganizationOid, objAnimalSupplieOid, objAnimalSupplieTypeOid, ObjSeedTypeOid, ObjMaster.UseNo));

                                        }
                                        else
                                        {
                                            if (row.SeedTypeOid != null)
                                            {
                                                objStockAnimalUseInfoEdit = ObjectSpace.FindObject<StockAnimalUseInfo>(CriteriaOperator.Parse("OrganizationOid=? and AnimalSupplieOid=? and AnimalSupplieTypeOid=? and SeedTypeOid=? and AnimalUseNumber=?", objOrganizationOid, objAnimalSupplieOid, objAnimalSupplieTypeOid, ObjSeedTypeOid, ObjMaster.UseNo));
                                                objStockAnimalInfoEdit = ObjectSpace.FindObject<StockAnimalInfo>(CriteriaOperator.Parse("OrganizationOid=? and AnimalSupplieOid=? and AnimalSupplieTypeOid=? and SeedTypeOid=? and AnimalProductNumber=?", objOrganizationOid, objAnimalSupplieOid, objAnimalSupplieTypeOid, ObjSeedTypeOid, ObjMaster.UseNo));
                                            }
                                            else
                                            {
                                                objStockAnimalUseInfoEdit = ObjectSpace.FindObject<StockAnimalUseInfo>(CriteriaOperator.Parse("OrganizationOid=? and AnimalSupplieOid=? and AnimalSupplieTypeOid=? and AnimalUseNumber=?", objOrganizationOid, objAnimalSupplieOid, objAnimalSupplieTypeOid, ObjMaster.UseNo));
                                                objStockAnimalInfoEdit = ObjectSpace.FindObject<StockAnimalInfo>(CriteriaOperator.Parse("OrganizationOid=? and AnimalSupplieOid=? and AnimalSupplieTypeOid=? and AnimalProductNumber=?", objOrganizationOid, objAnimalSupplieOid, objAnimalSupplieTypeOid, ObjMaster.UseNo));

                                            }
                                        }


                                        //               'Stock การใช้
                                        if (objStockAnimalUseInfoEdit != null)
                                        {
                                            objStockAnimalUseInfoEdit.IsApprove = true;
                                        }
                                        //  'Stock หลัก
                                        //      '=======================================================================

                                        if (objStockAnimalInfoEdit != null)
                                        {
                                            objStockAnimalInfoEdit.IsApprove = true;
                                        }

                                        //                    'กปศ4ว
                                        StockAnimalInfo_Report objStockAnimalInfoReportEdit = null;
                                        var objStockAnimalInfo_Detail = ObjectSpace.GetObjects<StockAnimalInfo_Report>();
                                        if (row.SeedTypeOid != null)
                                        {
                                            objStockAnimalInfo_Detail = ObjectSpace.GetObjects<StockAnimalInfo_Report>(CriteriaOperator.Parse("OrganizationOid= ? and FinanceYearOid=? and BudgetSourceOid=? and SeedTypeOid=? and AnimalSupplieOid=?", ObjMaster.OrganizationOid.Oid, ObjMaster.FinanceYearOid.Oid, row.BudgetSourceOid, row.SeedTypeOid.Oid, row.AnimalSupplieOid));

                                            objStockAnimalInfoReportEdit = ObjectSpace.FindObject<StockAnimalInfo_Report>(CriteriaOperator.Parse("OrganizationOid= ? and FinanceYearOid=? and BudgetSourceOid=? and SeedTypeOid=? and AnimalSupplieOid=? and AnimalProductNumber=?", ObjMaster.OrganizationOid.Oid, ObjMaster.FinanceYearOid.Oid, row.BudgetSourceOid, row.SeedTypeOid.Oid, row.AnimalSupplieOid, ObjMaster.UseNo));

                                        }
                                        else
                                        {
                                            objStockAnimalInfo_Detail = ObjectSpace.GetObjects<StockAnimalInfo_Report>(CriteriaOperator.Parse("OrganizationOid= ? and FinanceYearOid=? and BudgetSourceOid=? and AnimalSupplieOid=?", ObjMaster.OrganizationOid.Oid, ObjMaster.FinanceYearOid.Oid, row.BudgetSourceOid, row.AnimalSupplieOid));

                                            objStockAnimalInfoReportEdit = ObjectSpace.FindObject<StockAnimalInfo_Report>(CriteriaOperator.Parse("OrganizationOid= ? and FinanceYearOid=? and BudgetSourceOid=? and AnimalSupplieOid=? and AnimalProductNumber=?", ObjMaster.OrganizationOid.Oid, ObjMaster.FinanceYearOid.Oid, row.BudgetSourceOid, row.AnimalSupplieOid, ObjMaster.UseNo));
                                        }
                                        if (objStockAnimalInfoReportEdit != null)
                                        {
                                            objStockAnimalInfoReportEdit.IsApprove = true;
                                        }
                                    }
                                    //          '=======================================================================
                                }

                                if (ObjMaster.ActivityOid.ActivityName.Contains("จำหน่าย") == true)
                                {

                                    if (ObjMaster.ChkOneService == true)
                                    {
                                        objDetailService = ObjectSpace.FindObject<RegisterCusServiceDetail>(CriteriaOperator.Parse("[RegisterCusServiceOid]=? and [ServiceTypeOid.ServiceTypeName] like '%จำหน่าย%' and [SubServiceTypeOid.ServiceTypeName]='เสบียงสัตว์' and [ReceiveDate]=?", ObjMaster.RegisCusServiceOid, ObjMaster.UseDate));
                                        if (objDetailService == null)
                                        {
                                            var objDetailService_new = ObjectSpace.CreateObject<RegisterCusServiceDetail>();
                                            objDetailService_new.RegisterCusServiceOid = ObjMaster.RegisCusServiceOid;

                                            var objServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[ServiceTypeName] like '%จำหน่าย%'"));
                                            objDetailService_new.ServiceTypeOid = objServiceType;

                                            var objSubServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[MasterServiceType]=? and [ServiceTypeName]='เสบียงสัตว์'", objServiceType.Oid));
                                            objDetailService_new.SubServiceTypeOid = objSubServiceType;

                                            objDetailService_new.ReceiveDate = ObjMaster.UseDate;
                                            objDetailService_new.RefOid = ObjMaster.Oid.ToString();
                                        }
                                    }
                                    else if (ObjMaster.ChkGroupService == true)
                                    {
                                        objDetailService = ObjectSpace.FindObject<OrgeServiceDetail>(CriteriaOperator.Parse("[OrgeServiceOid]=? and [ServiceTypeOid.ServiceTypeName] like '%จำหน่าย%' and [SubServiceTypeOid.ServiceTypeName]='เสบียงสัตว์' and [ReceiveDate]=?", ObjMaster.OrgeServiceOid, ObjMaster.UseDate));
                                        if (objDetailService == null) //'ถ้ายังไม่มีข้อมูลการขอรับบริการให้ Insert ลงในส่วนของรายละเอียดการขอรับบริการด้วย
                                        {
                                            var objDetailService_new = ObjectSpace.CreateObject<OrgeServiceDetail>();
                                            objDetailService_new.OrgeServiceOid = ObjMaster.OrgeServiceOid;
                                            var objServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[ServiceTypeName] like '%จำหน่าย%'"));
                                            objDetailService_new.ServiceTypeOid = objServiceType;
                                            var objSubServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[MasterServiceType]=? and [ServiceTypeName]='เสบียงสัตว์'", objServiceType.Oid));
                                            objDetailService_new.SubServiceTypeOid = objSubServiceType;

                                            objDetailService_new.ReceiveDate = ObjMaster.UseDate;
                                            objDetailService_new.RefOid = ObjMaster.Oid.ToString();
                                        }
                                    } 

                                }
                                else if (ObjMaster.ActivityOid.ActivityName.Contains("แจกจ่าย") == true)
                                {
                                    if (ObjMaster.ChkOneService == true) //'รายเดี่ยว
                                    {
                                        objDetailService = ObjectSpace.FindObject<RegisterCusServiceDetail>(CriteriaOperator.Parse("[RegisterCusServiceOid]=? and [ServiceTypeOid.ServiceTypeName] like '%แจกจ่าย%' and [SubServiceTypeOid.ServiceTypeName]='เสบียงสัตว์' and [ReceiveDate]=?", ObjMaster.RegisCusServiceOid, ObjMaster.UseDate));
                                        if (objDetailService == null)// 'ถ้ายังไม่มีข้อมูลการขอรับบบริการให้ Insert ลงในส่วนของรายละเอียดการขอรับบริการด้วย
                                        {
                                            var objDetailService_new = ObjectSpace.CreateObject<RegisterCusServiceDetail>();
                                            objDetailService_new.RegisterCusServiceOid = ObjMaster.RegisCusServiceOid;


                                            var objServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[ServiceTypeName] like '%แจกจ่าย%'"));
                                            objDetailService_new.ServiceTypeOid = objServiceType;
                                            var objSubServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[MasterServiceType]=? and [ServiceTypeName]='เสบียงสัตว์'", objServiceType.Oid));
                                            objDetailService_new.SubServiceTypeOid = objSubServiceType;
                                            objDetailService_new.ReceiveDate = ObjMaster.UseDate;
                                            objDetailService_new.RefOid = ObjMaster.Oid.ToString();
                                        }
                                    }
                                    else if (ObjMaster.ChkGroupService == true)
                                    {
                                        objDetailService = ObjectSpace.FindObject<OrgeServiceDetail>(CriteriaOperator.Parse("[OrgeServiceOid]=? and [ServiceTypeOid.ServiceTypeName] like '%แจกจ่าย%' and [SubServiceTypeOid.ServiceTypeName]='เสบียงสัตว์' and [ReceiveDate]=?", ObjMaster.OrgeServiceOid, ObjMaster.UseDate));
                                        if (objDetailService == null) //'ถ้ายังไม่มีข้อมูลการขอรับบบริการให้ Insert ลงในส่วนของรายละเอียดการขอรับบริการด้วย
                                        {
                                            var objDetailService_new = ObjectSpace.CreateObject<OrgeServiceDetail>();
                                            objDetailService_new.OrgeServiceOid = ObjMaster.OrgeServiceOid;
                                            var objServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[ServiceTypeName] like '%แจกจ่าย%'"));
                                            objDetailService_new.ServiceTypeOid = objServiceType;
                                            var objSubServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[MasterServiceType]=? and [ServiceTypeName]='เสบียงสัตว์'", objServiceType.Oid));
                                            objDetailService_new.SubServiceTypeOid = objSubServiceType;

                                            objDetailService_new.ReceiveDate = ObjMaster.UseDate;
                                            objDetailService_new.RefOid = ObjMaster.Oid.ToString();
                                        }
                                    }
                                    
                                }




                                //       '=======================================================================
                                ObjMaster.Status = EnumRodBreedProductSeedStatus.Approve;//2
                                ObjMaster.ApproveDate = DateTime.Now;
                                ObjMaster.Remark = CancelMsg;


                                HistoryWork ObjHistory = null;
                                ObjHistory = ObjectSpace.CreateObject<HistoryWork>();
                                // ประวัติ
                                ObjHistory.RefOid = ObjMaster.Oid.ToString();
                                ObjHistory.FormName = "เสบียงสัตว์";
                                ObjHistory.Message = "อนุมัติ (ขอเบิกเสบียงสัตว์ (Mobile Application)) ลำดับที่ : " + ObjMaster.UseNo;
                                ObjHistory.CreateBy = Username;
                                ObjHistory.CreateDate = DateTime.Now;
                                ObjectSpace.CommitChanges();


                            }
                        }
                    
                    else if (Status == "2") //เคสไม่อนุมัติ
                    {

                        foreach (SupplierUseAnimalProductDetail row in ObjMaster.SupplierUseAnimalProductDetails)
                        {
                            var objOrganizationOid = row.SupplierUseAnimalProductOid.OrganizationOid;
                            var objAnimalSupplieOid = row.AnimalSupplieOid;
                            var objAnimalSupplieTypeOid = row.AnimalSupplieTypeOid;
                            var objQuotaType = row.QuotaTypeOid;
                            var objManageSubAnimalSupplierOid = row.ManageSubAnimalSupplierOid;
                            var ObjSeedTypeOid = row.SeedTypeOid;
                            object tmpSeedType = null;

                            // stocklimit = ObjectSpace.GetObject<SupplierUseAnimalProductDetail>(CriteriaOperator.Parse(" GCRecord  is null and AnimalSupplieOid=?", row.AnimalSupplieOid));
                            var ObjSupplierUseAnimalProductDetails = (from Q in ObjMaster.SupplierUseAnimalProductDetails orderby row.StockLimit descending select Q).First().StockLimit;

                            IList<StockAnimalUseInfo> objStockAnimalUseInfo;
                            object objStockAnimalInfo = null;
                            StockAnimalUseInfo objStockAnimalUseInfoEdit = null;
                            StockAnimalInfo objStockAnimalInfoEdit = null;
                            if (objQuotaType != null)
                            {
                                if (objManageSubAnimalSupplierOid != null)
                                {
                                    // objStockAnimalUseInfo = View.ObjectSpace.GetObjects(Of StockAnimalUseInfo)(CriteriaOperator.Parse("OrganizationOid=? and AnimalSupplieOid=? and AnimalSupplieTypeOid=? and QuotaTypeOid=? and ManageSubAnimalSupplierOid=? and AnimalSeedOid=?", objOrganizationOid.Oid, objAnimalSupplieOid.Oid, objAnimalSupplieTypeOid.Oid, objQuotaType, objManageSubAnimalSupplierOid.Oid, ObjAnimalSeedOid.Oid))
                                    objStockAnimalUseInfo = ObjectSpace.GetObjects<StockAnimalUseInfo>(CriteriaOperator.Parse("OrganizationOid=? and AnimalSupplieOid=? and AnimalSupplieTypeOid=? and QuotaTypeOid=? and ManageSubAnimalSupplierOid=? and SeedTypeOid=?", objOrganizationOid.Oid, objAnimalSupplieOid.Oid, objAnimalSupplieTypeOid.Oid, objQuotaType, objManageSubAnimalSupplierOid.Oid, ObjSeedTypeOid.Oid));
                                    objStockAnimalUseInfoEdit = ObjectSpace.FindObject<StockAnimalUseInfo>(CriteriaOperator.Parse("OrganizationOid=? and AnimalSupplieOid=? and AnimalSupplieTypeOid=? and QuotaTypeOid=? and ManageSubAnimalSupplierOid=? and SeedTypeOid=? and AnimalUseNumber=?", objOrganizationOid.Oid, objAnimalSupplieOid.Oid, objAnimalSupplieTypeOid.Oid, objQuotaType.Oid, objManageSubAnimalSupplierOid.Oid, ObjSeedTypeOid, ObjMaster.UseNo));
                                }
                                else
                                {
                                    objStockAnimalUseInfo = ObjectSpace.GetObjects<StockAnimalUseInfo>(CriteriaOperator.Parse("OrganizationOid=? and AnimalSupplieOid=? and AnimalSupplieTypeOid=? and SeedTypeOid=?", objOrganizationOid.Oid, objAnimalSupplieOid.Oid, objAnimalSupplieTypeOid.Oid, ObjSeedTypeOid.Oid));
                                    objStockAnimalUseInfoEdit = ObjectSpace.FindObject<StockAnimalUseInfo>(CriteriaOperator.Parse("OrganizationOid=? and AnimalSupplieOid=? and AnimalSupplieTypeOid=? and SeedTypeOid=? and AnimalUseNumber=?", objOrganizationOid.Oid, objAnimalSupplieOid.Oid, objAnimalSupplieTypeOid.Oid, ObjSeedTypeOid.Oid, ObjMaster.UseNo));


                                    objStockAnimalInfo = ObjectSpace.GetObjects<StockAnimalInfo>(CriteriaOperator.Parse("OrganizationOid=? and AnimalSupplieOid=? and AnimalSupplieTypeOid=? and SeedTypeOid=?", objOrganizationOid.Oid, objAnimalSupplieOid.Oid, objAnimalSupplieTypeOid.Oid, ObjSeedTypeOid.Oid));
                                    objStockAnimalInfoEdit = ObjectSpace.FindObject<StockAnimalInfo>(CriteriaOperator.Parse("OrganizationOid=? and AnimalSupplieOid=? and AnimalSupplieTypeOid=? and SeedTypeOid=? and AnimalProductNumber=?", objOrganizationOid.Oid, objAnimalSupplieOid.Oid, objAnimalSupplieTypeOid.Oid, ObjSeedTypeOid.Oid, ObjMaster.UseNo));
                                }

                            }

                            else
                            {
                                if (ObjSeedTypeOid != null)
                                {
                                    objStockAnimalUseInfo = ObjectSpace.GetObjects<StockAnimalUseInfo>(CriteriaOperator.Parse("OrganizationOid=? and AnimalSupplieOid=? and AnimalSupplieTypeOid=? and SeedTypeOid=?", objOrganizationOid.Oid, objAnimalSupplieOid.Oid, objAnimalSupplieTypeOid.Oid, ObjSeedTypeOid.Oid));
                                    objStockAnimalInfo = ObjectSpace.GetObjects<StockAnimalInfo>(CriteriaOperator.Parse("OrganizationOid=? and AnimalSupplieOid=? and AnimalSupplieTypeOid=? and SeedTypeOid=?", objOrganizationOid.Oid, objAnimalSupplieOid.Oid, objAnimalSupplieTypeOid.Oid, ObjSeedTypeOid.Oid));

                                    objStockAnimalUseInfoEdit = ObjectSpace.FindObject<StockAnimalUseInfo>(CriteriaOperator.Parse("OrganizationOid=? and AnimalSupplieOid=? and AnimalSupplieTypeOid=? and SeedTypeOid=? and AnimalUseNumber=?", objOrganizationOid.Oid, objAnimalSupplieOid.Oid, objAnimalSupplieTypeOid.Oid, ObjSeedTypeOid.Oid, ObjMaster.UseNo));
                                    objStockAnimalInfoEdit = ObjectSpace.FindObject<StockAnimalInfo>(CriteriaOperator.Parse("OrganizationOid=? and AnimalSupplieOid=? and AnimalSupplieTypeOid=? and SeedTypeOid=? and AnimalProductNumber=?", objOrganizationOid.Oid, objAnimalSupplieOid.Oid, objAnimalSupplieTypeOid.Oid, ObjSeedTypeOid.Oid, ObjMaster.UseNo));
                                }
                                else
                                {
                                    objStockAnimalUseInfo = ObjectSpace.GetObjects<StockAnimalUseInfo>(CriteriaOperator.Parse("OrganizationOid=? and AnimalSupplieOid=? and AnimalSupplieTypeOid=?", objOrganizationOid.Oid, objAnimalSupplieOid.Oid, objAnimalSupplieTypeOid.Oid));
                                    objStockAnimalInfo = ObjectSpace.GetObjects<StockAnimalInfo>(CriteriaOperator.Parse("OrganizationOid=? and AnimalSupplieOid=? and AnimalSupplieTypeOid=?", objOrganizationOid.Oid, objAnimalSupplieOid.Oid, objAnimalSupplieTypeOid.Oid));

                                    objStockAnimalUseInfoEdit = ObjectSpace.FindObject<StockAnimalUseInfo>(CriteriaOperator.Parse("OrganizationOid=? and AnimalSupplieOid=? and AnimalSupplieTypeOid=? and AnimalUseNumber=?", objOrganizationOid.Oid, objAnimalSupplieOid.Oid, objAnimalSupplieTypeOid.Oid, ObjMaster.UseNo));
                                    objStockAnimalInfoEdit = ObjectSpace.FindObject<StockAnimalInfo>(CriteriaOperator.Parse("OrganizationOid=? and AnimalSupplieOid=? and AnimalSupplieTypeOid=? and AnimalProductNumber=?", objOrganizationOid.Oid, objAnimalSupplieOid.Oid, objAnimalSupplieTypeOid.Oid, ObjMaster.UseNo));
                                }
                            }

                                // ตัดยอดออกจาก Stock การใช้ เนื่องจากยกเลิกการอนุมัติ
                                // ========================================================
                                if (ObjMaster.Status == EnumRodBreedProductSeedStatus.Approve || ObjMaster.Status == EnumRodBreedProductSeedStatus.Accepet)
                                {
                                    if (objStockAnimalUseInfo.Count > 0)
                                    {
                                        if (objStockAnimalUseInfoEdit != null)
                                        {
                                            objStockAnimalUseInfoEdit.IsApprove = true;
                                        }

                                        var objInsStockAnimalUseInfo = ObjectSpace.CreateObject<StockAnimalUseInfo>();
                                        var withBlock = objInsStockAnimalUseInfo;
                                        withBlock.OrganizationOid = objOrganizationOid;
                                        withBlock.TransactionDate = DateTime.Now;
                                        withBlock.AnimalSupplieOid = objAnimalSupplieOid;
                                        withBlock.AnimalSupplieTypeOid = objAnimalSupplieTypeOid;
                                        withBlock.QuotaTypeOid = objQuotaType;
                                        if (objManageSubAnimalSupplierOid != null) ;
                                        {
                                            withBlock.ManageSubAnimalSupplierOid = objManageSubAnimalSupplierOid;
                                        }
                                        // .AnimalSeedOid = ObjAnimalSeedOid
                                        withBlock.Weight = 0 - row.Weight;
                                        withBlock.Remark = " ไม่อนุมัติการใช้เสบียงสัตว์ (Mobile Application)";
                                        withBlock.FinanceYearOid = row.SupplierUseAnimalProductOid.FinanceYearOid;
                                        withBlock.SeedTypeOid = row.SeedTypeOid;
                                        withBlock.BudgetSourceOid = row.BudgetSourceOid;
                                        withBlock.AnimalUseNumber = ObjMaster.UseNo;
                                        withBlock.ActivityOid = row.SupplierUseAnimalProductOid.ActivityOid;
                                        withBlock.SubActivityOid = row.SupplierUseAnimalProductOid.SubActivityOid;
                                        withBlock.SupplierUseAnimalDetailOid = row;
                                        withBlock.IsApprove = true;

                                    }
                                    //    'คืนยอดเข้า Stock การผลิต เนื่องจากยกเลิกการอนุมัติ
                                    //   '========================================================
                                    if (objStockAnimalInfo != null)
                                    {
                                        if (objStockAnimalInfoEdit != null)
                                        {
                                            objStockAnimalInfoEdit.IsApprove = true;
                                        }

                                        var objInsStockAnimalInfo = ObjectSpace.CreateObject<StockAnimalInfo>();

                                        objInsStockAnimalInfo.OrganizationOid = objOrganizationOid;
                                        objInsStockAnimalInfo.TransactionDate = DateTime.Now;
                                        objInsStockAnimalInfo.AnimalSupplieOid = objAnimalSupplieOid;
                                        objInsStockAnimalInfo.AnimalSupplieTypeOid = objAnimalSupplieTypeOid;
                                        //            '.QuotaTypeOid = objQuotaType
                                        //            '.ManageSubAnimalSupplierOid = IIf(objManageSubAnimalSupplierOid IsNot Nothing, objManageSubAnimalSupplierOid, Nothing)
                                        //           '.AnimalSeedOid = ObjAnimalSeedOid
                                        objInsStockAnimalInfo.BudgetSourceOid = row.BudgetSourceOid;
                                        objInsStockAnimalInfo.Weight = row.Weight;
                                        objInsStockAnimalInfo.Remark = "รับคืนเนื่องจากไม่อนุมัติให้ใช้เสบียงสัตว์ (Mobile Application)";
                                        objInsStockAnimalInfo.FinanceYearOid = row.SupplierUseAnimalProductOid.FinanceYearOid;
                                        objInsStockAnimalInfo.SeedTypeOid = row.SeedTypeOid;
                                        objInsStockAnimalInfo.AnimalProductNumber = ObjMaster.UseNo;
                                        objInsStockAnimalInfo.IsApprove = true;
                                    }
                                    // 'Stock สำหรับ กปศ4ว
                                    // =======================================================================
                                    //if (ObjMaster.Status == EnumRodBreedProductSeedStatus.Approve || ObjMaster.Status == EnumRodBreedProductSeedStatus.Accepet)
                                    //{
                                    StockAnimalInfo_Report objStockAnimalInfoReportEdit = null;
                                    IList<StockAnimalInfo_Report> objStockAnimalInfo_Detail = ObjectSpace.GetObjects<StockAnimalInfo_Report>();
                                    if (row.SeedTypeOid != null)
                                    {
                                        objStockAnimalInfo_Detail = ObjectSpace.GetObjects<StockAnimalInfo_Report>(CriteriaOperator.Parse("OrganizationOid= ? and FinanceYearOid=? and BudgetSourceOid=? and SeedTypeOid=? and AnimalSupplieOid=?", ObjMaster.OrganizationOid.Oid, ObjMaster.FinanceYearOid.Oid, row.BudgetSourceOid, row.SeedTypeOid.Oid, row.AnimalSupplieOid));

                                        objStockAnimalInfoReportEdit = ObjectSpace.FindObject<StockAnimalInfo_Report>(CriteriaOperator.Parse("OrganizationOid= ? and FinanceYearOid=? and BudgetSourceOid=? and SeedTypeOid=? and AnimalSupplieOid=? and AnimalProductNumber=?", ObjMaster.OrganizationOid.Oid, ObjMaster.FinanceYearOid.Oid, row.BudgetSourceOid, row.SeedTypeOid.Oid, row.AnimalSupplieOid, ObjMaster.UseNo));
                                    }
                                    else
                                    {
                                        objStockAnimalInfo_Detail = ObjectSpace.GetObjects<StockAnimalInfo_Report>(CriteriaOperator.Parse("OrganizationOid= ? and FinanceYearOid=? and BudgetSourceOid=? and AnimalSupplieOid=?", ObjMaster.OrganizationOid.Oid, ObjMaster.FinanceYearOid.Oid, row.BudgetSourceOid, row.AnimalSupplieOid));

                                        objStockAnimalInfoReportEdit = ObjectSpace.FindObject<StockAnimalInfo_Report>(CriteriaOperator.Parse("OrganizationOid= ? and FinanceYearOid=? and BudgetSourceOid=? and AnimalSupplieOid=? and AnimalProductNumber=?", ObjMaster.OrganizationOid.Oid, ObjMaster.FinanceYearOid.Oid, row.BudgetSourceOid, row.AnimalSupplieOid, ObjMaster.UseNo));
                                    }
                                    if (objStockAnimalInfo_Detail.Count > 0)
                                    {
                                        if (objStockAnimalInfoReportEdit != null)
                                        {
                                            objStockAnimalInfoReportEdit.IsApprove = true;
                                        }
                                        var objStockAnimalInfo_DetailNew = ObjectSpace.CreateObject<StockAnimalInfo_Report>();
                                        var ObjSubStockCardSource = (from Item in objStockAnimalInfo_Detail orderby Item.TransactionDate descending select Item).First();
                                        var withBlock = objStockAnimalInfo_DetailNew;
                                        withBlock.TransactionDate = DateTime.Now;
                                        withBlock.AnimalProductNumber = ObjMaster.UseNo;
                                        withBlock.FinanceYearOid = ObjMaster.FinanceYearOid;
                                        withBlock.BudgetSourceOid = row.BudgetSourceOid;
                                        withBlock.OrganizationOid = ObjMaster.OrganizationOid;
                                        withBlock.AnimalSupplieOid = row.AnimalSupplieOid;
                                        withBlock.AnimalSupplieTypeOid = row.AnimalSupplieTypeOid;
                                        withBlock.TotalForward = ObjSubStockCardSource.TotalWeight;
                                        withBlock.TotalChange = row.Weight;
                                        withBlock.SeedTypeOid = row.SeedTypeOid;
                                        withBlock.Description = "ไม่อนุมัติการใช้เสบียงสัตว์ (Mobile Application): " + ObjMaster.UseNo;
                                        withBlock.IsApprove = true;
                                    }
                                    //}
                                }
                            }

                            ObjMaster.CancelMsg = CancelMsg;
                        ObjMaster.Status = EnumRodBreedProductSeedStatus.Eject;
                        ObjMaster.CancelBy = Username;
                        ObjMaster.ActionType = EnumAction.Eject;
                        ObjMaster.CancelDate = DateTime.Now;


                        HistoryWork ObjHistory = null;
                        ObjHistory = ObjectSpace.CreateObject<HistoryWork>();
                        // ประวัติ
                        ObjHistory.RefOid = ObjMaster.Oid.ToString();
                        ObjHistory.FormName = "เสบียงสัตว์";
                        ObjHistory.Message = "ไม่อนุมัติ (ขอเบิกเสบียงสัตว์ (Mobile Application)) ลำดับที่ : " + ObjMaster.UseNo;
                        ObjHistory.CreateBy = Username;
                        ObjHistory.CreateDate = DateTime.Now;
                        ObjectSpace.CommitChanges();
                    }



                            UpdateResult ret = new UpdateResult();


                            ret.status = "true";
                            ret.message = "บันทึกข้อมูลเรียบร้อยแล้ว";

                            return Request.CreateResponse(HttpStatusCode.OK, ret);
                            //else
                            //{
                            //    UpdateResult ret = new UpdateResult();//ใช้ดัก ไม่อนุมัติ
                            //    ret.status = "False";
                            //    ret.message = "ไม่สามารถอนุมัติได้";
                            //    return Request.CreateResponse(HttpStatusCode.NotFound, ret);
                            //}
                        }
                        else
                        {
                            UpdateResult ret = new UpdateResult();
                            ret.status = "False";
                            ret.message = "ไม่มีข้อมูล";
                            return Request.CreateResponse(HttpStatusCode.BadRequest, ret);
                        }
                    }
                else
                {
                    UpdateResult ret = new UpdateResult();
                    ret.status = "False";
                    ret.message = "กรุณากรอก RefNo";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ret);
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
        #region ตัดสต้อค
        /// <summary>
        /// อนุมัติ-ไม่อนุมัติการใช้เมล็ดพันธุ์
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SupplierUseProduct/Update")]   //การใช้เมล็ด
        public HttpResponseMessage UpdateSupplierUseProduct()
        {
            string TempDescription = null;
            string Username = "";
            string Temp = "";
            object objDetailService = null;

            double stocklimit = 0;
            try
            {
                string Remark = HttpContext.Current.Request.Form["Remark"].ToString();
                string RefNo = HttpContext.Current.Request.Form["RefNo"].ToString(); //ข้อมูลเลขที่อ้างอิง
                string Status = HttpContext.Current.Request.Form["Status"].ToString(); //สถานะ
                Username = HttpContext.Current.Request.Form["Username"].ToString();

                if (RefNo != "" && Status != "")
                {
                    string[] arr = RefNo.Split('|');
                    string _refno = arr[0]; //เลขที่อ้างอิง
                    string _org_oid = arr[1]; //oid หน่วยงาน
                    string _type = arr[2]; //ประเภทส่ง(2)-รับ(1)

                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierUseAnimalProduct));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.StockAnimalInfo_Report));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.HistoryWork));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.StockAnimalUseInfo));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.StockSeedInfo));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierUseProduct));
                    XafTypesInfo.Instance.RegisterEntity(typeof(ReceiveLotNumber));
                    XafTypesInfo.Instance.RegisterEntity(typeof(RegisterCusServiceDetail));
                    XafTypesInfo.Instance.RegisterEntity(typeof(OrgeServiceDetail));
                    XafTypesInfo.Instance.RegisterEntity(typeof(ServiceType));
                    XafTypesInfo.Instance.RegisterEntity(typeof(QualityAnalysis));
                    
                    List<SendOrderSeed> list = new List<SendOrderSeed>();
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                    SupplierUseProduct ObjMaster = ObjectSpace.FindObject<SupplierUseProduct>(CriteriaOperator.Parse("UseNo=?", _refno));
                    if (ObjMaster != null)
                    {
                        bool result = false;
                        if (Status == "1")  //1 อนุมัติ
                        { //บันทึกการส่ง

                            foreach (SupplierUseProductDetail row in ObjMaster.SupplierUseProductDetails)
                            {
                                if (ObjMaster.Status == EnumSupplierUseStatus.Accepet)
                                {

                                    Temp = row.AnimalSeedOid.SeedName;
                                    var objStockSeedInfo = ObjectSpace.GetObjects<StockSeedInfo>(CriteriaOperator.Parse("OrganizationOid= ? and FinanceYearOid=? and BudgetSourceOid=? and AnimalSeedOid=? and AnimalSeedLevelOid=?  and ReferanceCode=? and UseNo=? ", ObjMaster.OrganizationOid, ObjMaster.FinanceYearOid.Oid, row.BudgetSourceOid, row.AnimalSeedOid.Oid, row.AnimalSeedLevelOid.Oid, row.LotNumber.LotNumber, ObjMaster.UseNo));
                                    if (objStockSeedInfo.Count > 0)
                                    {
                                        foreach (StockSeedInfo rowEdit in objStockSeedInfo)
                                        {
                                            rowEdit.IsApprove = true;
                                        }
                                    }
                                }
                            }
                            if (ObjMaster.ActivityOid.ActivityName.Contains("จำหน่าย") == true)
                            {
                                if (ObjMaster.ChkOneService == true)
                                {
                                    objDetailService = ObjectSpace.FindObject<RegisterCusServiceDetail>(CriteriaOperator.Parse("[RegisterCusServiceOid]=? and [ServiceTypeOid.ServiceTypeName] like '%จำหน่าย%' and [SubServiceTypeOid.ServiceTypeName]='เมล็ดพันธุ์' and [ReceiveDate]=?", ObjMaster.RegisCusServiceOid, ObjMaster.UseDate));
                                    if (objDetailService == null)
                                    {
                                        var objDetailService_new = ObjectSpace.CreateObject<RegisterCusServiceDetail>();
                                        objDetailService_new.RegisterCusServiceOid = ObjMaster.RegisCusServiceOid;

                                        var objServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[ServiceTypeName] like '%จำหน่าย%'"));
                                        objDetailService_new.ServiceTypeOid = objServiceType;

                                        var objSubServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[MasterServiceType]=? and [ServiceTypeName]='เมล็ดพันธุ์'", objServiceType.Oid));
                                        objDetailService_new.SubServiceTypeOid = objSubServiceType;

                                        objDetailService_new.ReceiveDate = ObjMaster.UseDate;
                                        objDetailService_new.RefOid = ObjMaster.Oid.ToString();
                                    }
                                }
                                else if (ObjMaster.ChkGroupService == true)
                                {
                                    objDetailService = ObjectSpace.FindObject<OrgeServiceDetail>(CriteriaOperator.Parse("[OrgeServiceOid]=? and [ServiceTypeOid.ServiceTypeName] like '%จำหน่าย%' and [SubServiceTypeOid.ServiceTypeName]='เมล็ดพันธุ์' and [ReceiveDate]=?", ObjMaster.OrgeServiceOid, ObjMaster.UseDate));
                                    if (objDetailService == null) //'ถ้ายังไม่มีข้อมูลการขอรับบบริการให้ Insert ลงในส่วนของรายละเอียดการขอรับบริการด้วย
                                    {
                                        var objDetailService_new = ObjectSpace.CreateObject<OrgeServiceDetail>();
                                        objDetailService_new.OrgeServiceOid = ObjMaster.OrgeServiceOid;
                                        var objServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[ServiceTypeName] like '%จำหน่าย%'"));
                                        objDetailService_new.ServiceTypeOid = objServiceType;
                                        var objSubServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[MasterServiceType]=? and [ServiceTypeName]='เมล็ดพันธุ์'", objServiceType.Oid));
                                        objDetailService_new.SubServiceTypeOid = objSubServiceType;

                                        objDetailService_new.ReceiveDate = ObjMaster.UseDate;
                                        objDetailService_new.RefOid = ObjMaster.Oid.ToString();
                                    }
                                }
                            }
                            else if (ObjMaster.ActivityOid.ActivityName.Contains("แจกจ่าย") == true)
                                if (ObjMaster.ChkOneService == true) //'รายเดี่ยว
                                {
                                    objDetailService = ObjectSpace.FindObject<RegisterCusServiceDetail>(CriteriaOperator.Parse("[RegisterCusServiceOid]=? and [ServiceTypeOid.ServiceTypeName] like '%แจกจ่าย%' and [SubServiceTypeOid.ServiceTypeName]='เมล็ดพันธุ์' and [ReceiveDate]=?", ObjMaster.RegisCusServiceOid, ObjMaster.UseDate));
                                    if (objDetailService == null)// 'ถ้ายังไม่มีข้อมูลการขอรับบบริการให้ Insert ลงในส่วนของรายละเอียดการขอรับบริการด้วย
                                    {
                                        var objDetailService_new = ObjectSpace.CreateObject<RegisterCusServiceDetail>();
                                        objDetailService_new.RegisterCusServiceOid = ObjMaster.RegisCusServiceOid;


                                        var objServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[ServiceTypeName] like '%แจกจ่าย%'"));
                                        objDetailService_new.ServiceTypeOid = objServiceType;
                                        var objSubServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[MasterServiceType]=? and [ServiceTypeName]='เมล็ดพันธุ์'", objServiceType.Oid));
                                        objDetailService_new.SubServiceTypeOid = objSubServiceType;
                                        objDetailService_new.ReceiveDate = ObjMaster.UseDate;
                                        objDetailService_new.RefOid = ObjMaster.Oid.ToString();
                                    }
                                }
                                else if (ObjMaster.ChkGroupService == true)
                                {
                                    objDetailService = ObjectSpace.FindObject<OrgeServiceDetail>(CriteriaOperator.Parse("[OrgeServiceOid]=? and [ServiceTypeOid.ServiceTypeName] like '%แจกจ่าย%' and [SubServiceTypeOid.ServiceTypeName]='เมล็ดพันธุ์' and [ReceiveDate]=?", ObjMaster.OrgeServiceOid, ObjMaster.UseDate));
                                    if (objDetailService == null) //'ถ้ายังไม่มีข้อมูลการขอรับบบริการให้ Insert ลงในส่วนของรายละเอียดการขอรับบริการด้วย
                                    {
                                        var objDetailService_new = ObjectSpace.CreateObject<OrgeServiceDetail>();
                                        objDetailService_new.OrgeServiceOid = ObjMaster.OrgeServiceOid;
                                        var objServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[ServiceTypeName] like '%แจกจ่าย%'"));
                                        objDetailService_new.ServiceTypeOid = objServiceType;
                                        var objSubServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[MasterServiceType]=? and [ServiceTypeName]='เมล็ดพันธุ์'", objServiceType.Oid));
                                        objDetailService_new.SubServiceTypeOid = objSubServiceType;

                                        objDetailService_new.ReceiveDate = ObjMaster.UseDate;
                                        objDetailService_new.RefOid = ObjMaster.Oid.ToString();
                                    }
                                
                                }


                            var withBlock = ObjMaster;
                            withBlock.Status = EnumSupplierUseStatus.Approve;
                            withBlock.ApproveDate = DateTime.Now;

                            HistoryWork ObjHistory = null;
                            ObjHistory = ObjectSpace.CreateObject<HistoryWork>();
                            // ประวัติ
                            ObjHistory.RefOid = ObjMaster.Oid.ToString();
                            ObjHistory.FormName = "เมล็ดพันธุ์";
                            ObjHistory.Message = "อนุมัติ (ขอเบิกเมล็ดพันธุ์ (Mobile Application)) ลำดับที่ : " + ObjMaster.UseNo;
                            ObjHistory.CreateBy = Username;
                            ObjHistory.CreateDate = DateTime.Now;
                            ObjectSpace.CommitChanges();
                            result = true;

                        }

                
                        else if (Status == "2")
                        { //ไม่อนุมัติ
                      
                            if (ObjMaster.Status == EnumSupplierUseStatus.Accepet )
                                {
                                foreach (SupplierUseProductDetail row in ObjMaster.SupplierUseProductDetails)
                                {
                                    
                              //      'Update สถานะ IsApprove เป็น True เพื่อปรับให้รายการ Stock สมบูรณ์
               //     '==============================================================
                                   IList<StockSeedInfo>  objStockSeedInfoApproveStatus = ObjectSpace.GetObjects<StockSeedInfo>(); 
                             
                                    objStockSeedInfoApproveStatus = ObjectSpace.GetObjects<StockSeedInfo>(CriteriaOperator.Parse("OrganizationOid= ? and FinanceYearOid=? and BudgetSourceOid=? and AnimalSeedOid=? and AnimalSeedLevelOid=? and ReferanceCode=? and UseNo=? ", ObjMaster.OrganizationOid, ObjMaster.FinanceYearOid.Oid, row.BudgetSourceOid, row.AnimalSeedOid.Oid, row.AnimalSeedLevelOid.Oid, row.LotNumber.LotNumber, ObjMaster.UseNo));
                                    if (objStockSeedInfoApproveStatus.Count > 0)
                                    {
                                       foreach(  StockSeedInfo rowEdit in objStockSeedInfoApproveStatus)
                                            {
                                            rowEdit.IsApprove = true;
                                        }
                                    }
                                    var objStockSeedInfo = ObjectSpace.GetObjects<StockSeedInfo>(CriteriaOperator.Parse("OrganizationOid= ? and AnimalSeedOid=? and AnimalSeedLevelOid=? and SeedTypeOid=? and ReferanceCode=?", ObjMaster.OrganizationOid.Oid, row.AnimalSeedOid.Oid, row.AnimalSeedLevelOid.Oid, row.SeedTypeOid.Oid, row.LotNumber.LotNumber));
                                    if (objStockSeedInfo.Count > 0)
                                    {
                                        var ObjSubStockCardSource = (from Item in objStockSeedInfo orderby Item.StockDate descending select Item).First();
                                        var ObjStockSeedInfoInfo = ObjectSpace.CreateObject<StockSeedInfo>();
                                        // ObjMaster.ReceiveOrgOid.Oid, ObjMaster.FinanceYearOid.Oid, objSupplierProduct.BudgetSourceOid, objSupplierProduct.AnimalSeedOid.Oid, objSupplierProduct.AnimalSeedLevelOid.Oid))

                                        var withBlock1 = ObjStockSeedInfoInfo;
                                        withBlock1.StockDate = DateTime.Now;
                                        withBlock1.OrganizationOid = ObjMaster.OrganizationOid;
                                        withBlock1.FinanceYearOid = ObjSubStockCardSource.FinanceYearOid;
                                        withBlock1.BudgetSourceOid = ObjSubStockCardSource.BudgetSourceOid;
                                        withBlock1.AnimalSeedOid = ObjSubStockCardSource.AnimalSeedOid;
                                        withBlock1.AnimalSeedLevelOid = ObjSubStockCardSource.AnimalSeedLevelOid;
                                        withBlock1.StockDetail = "ไม่อนุมัติการใช้เมล็ดพันธุ์ (Mobile Application) ลำดับที่ : " + ObjMaster.UseNo;
                                        withBlock1.TotalForward = ObjSubStockCardSource.TotalWeight;
                                        withBlock1.SeedTypeOid = ObjSubStockCardSource.SeedTypeOid;
                                        withBlock1.TotalChange = row.Weight;
                                        withBlock1.StockType = EnumStockType.ReceiveProduct;
                                        withBlock1.ReferanceCode = row.LotNumber.LotNumber;
                                        withBlock1.Description = "ไม่อนุมัติการใช้เมล็ดพันธุ์ (Mobile Application) : " + ObjMaster.UseNo;
                                        withBlock1.UseNo = ObjMaster.UseNo;
                                        withBlock1.IsApprove = true;
                                        

                                        if (ObjStockSeedInfoInfo.TotalWeight == 0)
                                        {
                                            row.LotNumber.IsActive = false;
                                        }

                                    }
                                    QualityAnalysis objQualityAnalysis = ObjectSpace.FindObject<QualityAnalysis>(CriteriaOperator.Parse("[LotNumber]=? and [AnalysisType]=0 and [OrganizationOid]=?", row.LotNumber, ObjMaster.OrganizationOid));
                                    if (objQualityAnalysis != null)
                                    {
                                        objQualityAnalysis.Weight += row.Weight;
                                    }

                                }
                            }
                            if (Remark != "")
                            {
                                ObjMaster.Remark = Remark;
                            }
                            var withBlock = ObjMaster;
                            withBlock.Status = EnumSupplierUseStatus.Eject;
                            withBlock.ApproveDate = DateTime.Now;
                            withBlock.Remark = Remark;

                            HistoryWork ObjHistory = null;
                            ObjHistory = ObjectSpace.CreateObject<HistoryWork>();
                            // ประวัติ
                            ObjHistory.RefOid = ObjMaster.Oid.ToString();
                            ObjHistory.FormName = "เมล็ดพันธุ์";
                            ObjHistory.Message = "ไม่อนุมัติ (ขอเบิกเมล็ดพันธุ์(Mobile Application)) ลำดับที่ : " + ObjMaster.UseNo;
                            ObjHistory.CreateBy = Username;
                            ObjHistory.CreateDate = DateTime.Now;
                            ObjectSpace.CommitChanges();
                        }
                                if (Status == "1")
                                {
                            
                                    return Request.CreateResponse(HttpStatusCode.OK,"อนุมัติสำเร็จ");
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.OK, "ไม่อนุมัติสำเร็จ");
                                }

                    }
                    else
                    {
                        UserError err = new UserError();
                        err.status = "false";
                        err.code = "0";
                        err.message = "กรุณาใส่ข้อมูล RefNo ให้เรียบร้อยก่อน";
                        return Request.CreateResponse(HttpStatusCode.NotFound, err);
                    }


                }
                else
                {
                    UserError err = new UserError();
                    err.status = "false";
                    err.code = "0";
                    err.message = "กรุณาใส่ข้อมูล RefNo ให้เรียบร้อยก่อน";
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
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
     
        #endregion ตัดสต็อค
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SupplierUseAnimalProductDetail/List")] //SupplierUseProductDetail
        public HttpResponseMessage SupplierUseAnimalProduct_Detail()  ///SupplierUseAnimalProduct/Update
        {
            _Registerfarmer Registerfarmer = new _Registerfarmer();
            try
            {
                string org_oid = "";
                string type = "";
                org_oid = HttpContext.Current.Request.Form["OrganizationOid"];
                type = HttpContext.Current.Request.Form["type"];

                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);

                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                //การแจกจ่าย=1/การจำหน่าย=2/ภัยพิบัติ=3
                if (org_oid != "" && type != "")
                {
                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseAnimalProduct));

                    List<SupplierProductUser> UseACT1 = new List<SupplierProductUser>();
                    List<SupplierProductUser> UseACT2 = new List<SupplierProductUser>();
                    List<SupplierAnimalUsecalarity_Model> UseACT3 = new List<SupplierAnimalUsecalarity_Model>();
                    List<SupplierProductUser> UseACT4 = new List<SupplierProductUser>();
        

                    if (org_oid != null)
                    {
                        //การอนุมัติภัยพิบัติ
                        Activity ActivityOid = ObjectSpace.FindObject<Activity>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1 and ActivityName ='เพื่อช่วยเหลือภัยพิบัติ'  ", null));
                        IList<SupplierUseAnimalProduct> collection3 = ObjectSpace.GetObjects<SupplierUseAnimalProduct>(CriteriaOperator.Parse(" UseNo is not null and GCRecord is null and SubActivityOid is not null and PickupType is not null and Status = 0 and [OrganizationOid] like '%" + org_oid + "%' and [ActivityOid] = '" + ActivityOid.Oid + "'  ", null));
                        var query = from Q in collection3 orderby Q.UseNo select Q;
                        if (collection3.Count > 0)
                        {
                            foreach (SupplierUseAnimalProduct row in query)
                            {

                                SupplierAnimalUsecalarity_Model Supplier_3 = new SupplierAnimalUsecalarity_Model();
                                Supplier_3.SupplierUseAnimalProductOid = org_oid;
                                Supplier_3.TypeMoblie = type;
                                Supplier_3.SupplierUseAnimalProductOid = row.Oid.ToString();
                                Supplier_3.UseDate = row.UseDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                                Supplier_3.UseNo = row.UseNo;

                                if (row.RegisCusServiceOid != null)
                                {
                                    Supplier_3.RegisCusServiceOid = row.RegisCusServiceOid.Oid.ToString();
                                    Supplier_3.RegisCusServiceName = row.RegisCusServiceOid.DisPlayName;
                                    Supplier_3.FullName = row.RegisCusServiceOid.DisPlayName;
                                   
                            
                                }
                                if (row.OrgeServiceOid != null)
                                {
                                    Supplier_3.OrgeServiceOid = row.OrgeServiceOid.Oid.ToString();
                                    Supplier_3.OrgeServiceName = row.OrgeServiceOid.OrgeServiceName;
                          
                                  
                                }
                                Supplier_3.ActivityNameOid = row.ActivityOid.Oid.ToString();
                                Supplier_3.ActivityName = row.ActivityOid.ActivityName.ToString();
                                Supplier_3.OrganizationOid = row.OrganizationOid.Oid.ToString();
                                if (row.SubActivityOid != null)
                                {
                                    Supplier_3.SubActivityOid = row.SubActivityOid.Oid.ToString();
                                    Supplier_3.SubActivityName = row.SubActivityOid.ActivityName;
                                }
                                if (row.SubActivityLevelOid != null)
                                {
                                    Supplier_3.SubActivityLevelOid = row.SubActivityLevelOid.Oid.ToString();
                                    Supplier_3.SubActivityLevelName = row.SubActivityLevelOid.ActivityName;
                                }

                                Supplier_3.FinanceYear = row.FinanceYearOid.YearName.ToString();
                                Supplier_3.OrganizationName = row.OrganizationOid.SubOrganizeName.ToString();
                                if (row.EmployeeOid == null)
                                {
                                    Supplier_3.EmployeeName = "ไม่มีรายชื่อผู้ขอรับบริการ";
                                }
                                else
                                {
                                    Supplier_3.EmployeeName =row.EmployeeOid.FullName;
                                }

                                Supplier_3.Remark = row.ReceiverRemark;
                                Supplier_3.Status = row.Status.ToString();
                                Supplier_3.ApproveDate = row.ApproveDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                                Supplier_3.ActivityNameOid = row.ActivityOid.Oid.ToString();
                                Supplier_3.ActivityName = row.ActivityOid.ActivityName.ToString();
                                Supplier_3.PickupType = (int)row.PickupType;
                                Supplier_3.ReceiptNo = row.ReceiptNo;
                                Supplier_3.Refno = row.UseNo + "|" + row.OrganizationOid.Oid.ToString() + "|3";
                                Supplier_3.Weight = row.SupplierUseAnimalProductDetails.Sum((c => c.Weight)).ToString() + " " + "กิโลกรัม";
                                Supplier_3.ServiceCount = row.ReceiverNumber;
                                if (row.OrgeServiceOid != null)
                                {
                                    Supplier_3.FullAddress = row.OrgeServiceOid.FullAddress;
                                }
                                else
                                {
                                    Supplier_3.FullAddress = row.ReceiverAddress;
                                }
                                if (row.RegisCusServiceOid != null)
                                {
                                    Supplier_3.FullAddress = row.RegisCusServiceOid.FullAddress;
                                }
                                else
                                {
                                    Supplier_3.FullAddress = row.ReceiverAddress;
                                }
                                Supplier_3.ReceiverName = row.ReceiverName;
                                Supplier_3.ReceiverRemark = row.ReceiverRemark;
                                UseACT3.Add(Supplier_3);
                            }
                            //lists.UseACT2 = UseACT2;
                            //directProvider.Dispose();
                            //ObjectSpace.Dispose();
                            return Request.CreateResponse(HttpStatusCode.OK, UseACT3);
                            
                        }
                        else
                        {
                            UpdateResult ret = new UpdateResult();
                            ret.status = "False";
                            ret.message = "ไม่พบข้อมูล";
                            return Request.CreateResponse(HttpStatusCode.NotFound, ret);
                        }
                    }
                    else
                    {
                        UpdateResult ret = new UpdateResult();
                        ret.status = "False";
                        ret.message = "ไม่พบข้อมูล";
                        return Request.CreateResponse(HttpStatusCode.NotFound, ret);
                    }

                }
                else
                {
                    UpdateResult ret = new UpdateResult();
                    ret.status = "False";
                    ret.message = "กรุณากรอก RefNo";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ret);
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
