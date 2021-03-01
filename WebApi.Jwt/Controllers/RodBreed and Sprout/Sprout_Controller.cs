using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using Microsoft.ApplicationBlocks.Data;
using nutrition.Module;
using nutrition.Module.EmployeeAsUserExample.Module.BusinessObjects;
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
using WebApi.Jwt.Models.ท่อนพันธุ์_กล้าพันธุ์;

namespace WebApi.Jwt.Controllers.ท่อนพันธุ์_กล้าพันธุ์
{
    public class Sprout_Controller : ApiController

    {
        private string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        // GET: api/Sprout_
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Sprout_/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Sprout_
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Sprout_/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Sprout_/5
        [AllowAnonymous]
        [HttpPost]
        [Route("SproutUpdate/Approve")]
        public HttpResponseMessage UpdateSprout()
        {
            //  object objStockAnimalInfo = null;
            HistoryWork ObjHistory = null;
            IList<StockSeedInfo> objStockSeedInfo = null;
            object objDetailService = null;
            string Username = " ";

            try
            {
                string RefNo = HttpContext.Current.Request.Form["RefNo"].ToString(); //ข้อมูลเลขที่อ้างอิง
                                                                                     //  string Status = HttpContext.Current.Request.Form["Status"].ToString(); //สถานะ
                string CancelMsg = HttpContext.Current.Request.Form["Remark"].ToString(); //หมายเหตุ
                Username = HttpContext.Current.Request.Form["Username"].ToString();

                if (RefNo != "")
                {
                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierSproutUseProduct));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.StockSproutInfo));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.StockSproutInfo_Report));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.HistoryWork));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierProductModifyDetail));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierProductModify));
                    XafTypesInfo.Instance.RegisterEntity(typeof(UserInfo));
                    XafTypesInfo.Instance.RegisterEntity(typeof(ReceiveLotNumber));
                    XafTypesInfo.Instance.RegisterEntity(typeof(ReceiveLotNumber));
                    XafTypesInfo.Instance.RegisterEntity(typeof(RegisterCusServiceDetail));
                    XafTypesInfo.Instance.RegisterEntity(typeof(OrgeServiceDetail));
                    XafTypesInfo.Instance.RegisterEntity(typeof(ServiceType));

                    List<SupplierSproutUseProduct> list = new List<SupplierSproutUseProduct>();
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                    UserInfo objUserInfo = ObjectSpace.FindObject<UserInfo>(CriteriaOperator.Parse("[UserName]=?", Username));

                    SupplierSproutUseProduct ObjMaster = ObjectSpace.FindObject<SupplierSproutUseProduct>(CriteriaOperator.Parse("UseNo=?", RefNo));

                    foreach (SupplierSproutUseProductDetail row in ObjMaster.SupplierSproutUseProductDetails)
                    {
                        string TempDescription = "";
                        if (ObjMaster.Status == EnumRodBreedProductSeedStatus.Accepet)
                        {
                            //    'Update สถานะ IsApprove ให้เป็น True
                            var objStockGetBudgetSource = ObjectSpace.FindObject<StockSproutInfo>(CriteriaOperator.Parse("[FinanceYearOid]=? and [AnimalSeedOid]=? and [OrganizationOid]=? and [SeedTypeOid]=?", row.SupplierSproutUseProduct.FinanceYearOid, row.AnimalSeedOid, row.SupplierSproutUseProduct.OrganizationOid, row.SeedTypeOid));
                            if (objStockGetBudgetSource != null)
                            {
                                var objStockSproutInfoEdit = ObjectSpace.FindObject<StockSproutInfo>(CriteriaOperator.Parse("[SupplierSproutNumber]=? and [FinanceYearOid]=? and [BudgetSourceOid]=? and [OrganizationOid]=? and [AnimalSeedOid]=? and [SeedTypeOid]=?", row.SupplierSproutUseProduct.UseNo, row.SupplierSproutUseProduct.FinanceYearOid, objStockGetBudgetSource.BudgetSourceOid, row.SupplierSproutUseProduct.OrganizationOid, row.AnimalSeedOid, row.SeedTypeOid));
                                if (objStockSproutInfoEdit != null)
                                {
                                    objStockSproutInfoEdit.IsApprove = true;
                                }
                            }
                                // 'Stock สำหรับ กปศ4ว
                                //    'Update สถานะ IsApprove ให้เป็น True

                                var objStockGetBudgetSourceReport = ObjectSpace.FindObject<StockSproutInfo_Report>(CriteriaOperator.Parse("[FinanceYearOid]=? and [AnimalSeedOid]=? and [OrganizationOid]=? and [SeedTypeOid]=?", row.SupplierSproutUseProduct.FinanceYearOid, row.AnimalSeedOid, row.SupplierSproutUseProduct.OrganizationOid, row.SeedTypeOid));
                                var objStockRodBreedInfoReportEdit = ObjectSpace.FindObject<StockSproutInfo_Report>(CriteriaOperator.Parse("[SproutProductNumber]=? and [FinanceYearOid]=? and [BudgetSourceOid]=? and [OrganizationOid]=? and [AnimalSeedOid]=? and [SeedTypeOid]=?", row.SupplierSproutUseProduct.UseNo, row.SupplierSproutUseProduct.FinanceYearOid, objStockGetBudgetSource.BudgetSourceOid, row.SupplierSproutUseProduct.OrganizationOid, row.AnimalSeedOid, row.SeedTypeOid));
                                var objStockSproutInfo_DetailNew = ObjectSpace.CreateObject<StockSproutInfo_Report>();
                                // ==========================================
                                if (objStockRodBreedInfoReportEdit != null)
                                {
                                    objStockRodBreedInfoReportEdit.IsApprove = true;
                                }
                            }
                        }
                    
                   
                    if (ObjMaster.ActivityOid.ActivityName.Contains("จำหน่าย") == true)
                    {

                        if (ObjMaster.ChkOneService == true)
                        {
                            objDetailService = ObjectSpace.FindObject<RegisterCusServiceDetail>(CriteriaOperator.Parse("[RegisterCusServiceOid]=? and [ServiceTypeOid.ServiceTypeName] like '%จำหน่าย%' and [SubServiceTypeOid.ServiceTypeName]='กล้าพันธุ์' and [ReceiveDate]=?", ObjMaster.RegisCusServiceOid, ObjMaster.UseDate));
                            if (objDetailService == null)
                            {
                                RegisterCusServiceDetail objDetailService_new = ObjectSpace.CreateObject<RegisterCusServiceDetail>();
                                objDetailService_new.RegisterCusServiceOid = ObjMaster.RegisCusServiceOid;

                                ServiceType objServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[ServiceTypeName] like '%จำหน่าย%'"));
                                objDetailService_new.ServiceTypeOid = objServiceType;

                                ServiceType objSubServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[MasterServiceType]=? and [ServiceTypeName]='กล้าพันธุ์'", objServiceType.Oid));
                                objDetailService_new.SubServiceTypeOid = objSubServiceType;

                                objDetailService_new.ReceiveDate = ObjMaster.UseDate;
                                objDetailService_new.RefOid = ObjMaster.Oid.ToString();
                            }
                        }
                        else if (ObjMaster.ChkGroupService == true)
                        {
                            objDetailService = ObjectSpace.FindObject<OrgeServiceDetail>(CriteriaOperator.Parse("[OrgeServiceOid]=? and [ServiceTypeOid.ServiceTypeName] like '%จำหน่าย%' and [SubServiceTypeOid.ServiceTypeName]='กล้าพันธุ์' and [ReceiveDate]=?", ObjMaster.OrgeServiceOid, ObjMaster.UseDate));
                            if (objDetailService == null) //'ถ้ายังไม่มีข้อมูลการขอรับบบริการให้ Insert ลงในส่วนของรายละเอียดการขอรับบริการด้วย
                            {
                                OrgeServiceDetail objDetailService_new = ObjectSpace.CreateObject<OrgeServiceDetail>();
                                objDetailService_new.OrgeServiceOid = ObjMaster.OrgeServiceOid;
                                ServiceType objServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[ServiceTypeName] like '%จำหน่าย%'"));
                                objDetailService_new.ServiceTypeOid = objServiceType;
                                ServiceType objSubServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[MasterServiceType]=? and [ServiceTypeName]='กล้าพันธุ์'", objServiceType.Oid));
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
                            objDetailService = ObjectSpace.FindObject<RegisterCusServiceDetail>(CriteriaOperator.Parse("[RegisterCusServiceOid]=? and [ServiceTypeOid.ServiceTypeName] like '%แจกจ่าย%' and [SubServiceTypeOid.ServiceTypeName]='กล้าพันธุ์' and [ReceiveDate]=?", ObjMaster.RegisCusServiceOid, ObjMaster.UseDate));
                            if (objDetailService == null)// 'ถ้ายังไม่มีข้อมูลการขอรับบบริการให้ Insert ลงในส่วนของรายละเอียดการขอรับบริการด้วย
                            {
                                RegisterCusServiceDetail objDetailService_new = ObjectSpace.CreateObject<RegisterCusServiceDetail>();
                                objDetailService_new.RegisterCusServiceOid = ObjMaster.RegisCusServiceOid;


                                ServiceType objServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[ServiceTypeName] like '%แจกจ่าย%'"));
                                objDetailService_new.ServiceTypeOid = objServiceType;

                                ServiceType objSubServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[MasterServiceType]=? and [ServiceTypeName]='กล้าพันธุ์'", objServiceType.Oid));
                                objDetailService_new.SubServiceTypeOid = objSubServiceType;
                                objDetailService_new.ReceiveDate = ObjMaster.UseDate;
                                objDetailService_new.RefOid = ObjMaster.Oid.ToString();
                            }
                        }
                        else if (ObjMaster.ChkGroupService == true)
                        {
                            objDetailService = ObjectSpace.FindObject<OrgeServiceDetail>(CriteriaOperator.Parse("[OrgeServiceOid]=? and [ServiceTypeOid.ServiceTypeName] like '%แจกจ่าย%' and [SubServiceTypeOid.ServiceTypeName]='กล้าพันธุ์' and [ReceiveDate]=?", ObjMaster.OrgeServiceOid, ObjMaster.UseDate));
                            if (objDetailService == null) //'ถ้ายังไม่มีข้อมูลการขอรับบบริการให้ Insert ลงในส่วนของรายละเอียดการขอรับบริการด้วย
                            {
                                OrgeServiceDetail objDetailService_new = ObjectSpace.CreateObject<OrgeServiceDetail>();
                                objDetailService_new.OrgeServiceOid = ObjMaster.OrgeServiceOid;
                                ServiceType objServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[ServiceTypeName] like '%แจกจ่าย%'"));
                                objDetailService_new.ServiceTypeOid = objServiceType;
                                ServiceType objSubServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[MasterServiceType]=? and [ServiceTypeName]='กล้าพันธุ์'", objServiceType.Oid));
                                objDetailService_new.SubServiceTypeOid = objSubServiceType;

                                objDetailService_new.ReceiveDate = ObjMaster.UseDate;
                                objDetailService_new.RefOid = ObjMaster.Oid.ToString();
                            }
                        }
                        
                    }
                           
                            ObjMaster.Status = EnumRodBreedProductSeedStatus.Approve;
                    ObjMaster.ApproveDate = DateTime.Now;
                 

                    ObjHistory = ObjectSpace.CreateObject<HistoryWork>();
                    // ประวัติ
                    ObjHistory.RefOid = ObjMaster.Oid.ToString();
                    ObjHistory.FormName = "กล้าพันธุ์";
                    ObjHistory.Message = "อนุมัติ (ขอใช้กล้าพันธุ์ (Mobile Application) ) ลำดับที่ : " + ObjMaster.UseNo;
                    ObjHistory.CreateBy = objUserInfo.UserName;
                    ObjHistory.CreateDate = DateTime.Now;
                    ObjectSpace.CommitChanges();

                    UpdateResult ret = new UpdateResult();
                    ret.status = "true";
                    ret.message = "บันทึกข้อมูลอนุมัติเรียบร้อยแล้ว";
                    return Request.CreateResponse(HttpStatusCode.OK, ret);
                }
                else
                {
                    UpdateResult ret = new UpdateResult();
                    ret.status = "False";
                    ret.message = "กรุณากรอก RefNo";
                    return Request.CreateResponse(HttpStatusCode.NoContent, ret);
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

        [AllowAnonymous]
        [HttpPost]
        [Route("SproutUpdate/Eject")]
        public HttpResponseMessage UpdateSproutUseEject()
        {
            //  object objStockAnimalInfo = null;
            HistoryWork ObjHistory = null;
            IList<StockSeedInfo> objStockSeedInfo = null;
            string Username = " ";
            try
            {
                //string RefNo = HttpContext.Current.Request.Form["RefNo"].ToString(); //ข้อมูลเลขที่อ้างอิง
                //string Status = HttpContext.Current.Request.Form["Status"].ToString(); //สถานะ
                //string CancelMsg = HttpContext.Current.Request.Form["Remark"].ToString(); //หมายเหตุ
                //Username = HttpContext.Current.Request.Form["Username"].ToString();

                string RefNo = HttpContext.Current.Request.Form["RefNo"].ToString(); //ข้อมูลเลขที่อ้างอิง
                                                                                     // string Status = HttpContext.Current.Request.Form["Status"].ToString(); //สถานะ
                string CancelMsg = HttpContext.Current.Request.Form["Remark"].ToString(); //หมายเหตุ
                Username = HttpContext.Current.Request.Form["Username"].ToString();
                if (RefNo != "")
                {
                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierSproutUseProduct));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.StockSproutInfo));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.StockSproutInfo_Report));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.HistoryWork));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierProductModifyDetail));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierProductModify));
                    XafTypesInfo.Instance.RegisterEntity(typeof(UserInfo));
                    XafTypesInfo.Instance.RegisterEntity(typeof(ReceiveLotNumber));
                    XafTypesInfo.Instance.RegisterEntity(typeof(RegisterCusServiceDetail));
                    XafTypesInfo.Instance.RegisterEntity(typeof(OrgeServiceDetail));
                    XafTypesInfo.Instance.RegisterEntity(typeof(ServiceType));
                    List<SupplierSproutUseProduct> list = new List<SupplierSproutUseProduct>();
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                    UserInfo objUserInfo = ObjectSpace.FindObject<UserInfo>(CriteriaOperator.Parse("[UserName]=?", Username));

                    SupplierSproutUseProduct ObjMaster = ObjectSpace.FindObject<SupplierSproutUseProduct>(CriteriaOperator.Parse("UseNo=?", RefNo));
                    if (ObjMaster.SupplierSproutUseProductDetails != null)
                    {
                        foreach (SupplierSproutUseProductDetail row in ObjMaster.SupplierSproutUseProductDetails)
                        {
                            if (ObjMaster.Status == EnumRodBreedProductSeedStatus.Accepet || ObjMaster.Status == EnumRodBreedProductSeedStatus.Approve)
                            {
                                var objCheckStockSproutInfo = ObjectSpace.GetObjects<StockSproutInfo>(CriteriaOperator.Parse("SupplierSproutNumber=? and FinanceYearOid=? and AnimalSeedOid=? and OrganizationOid=? and SeedTypeOid=?", row.SupplierSproutUseProduct.UseNo, row.SupplierSproutUseProduct.FinanceYearOid, row.AnimalSeedOid, row.SupplierSproutUseProduct.OrganizationOid, row.SeedTypeOid));
                                if (objCheckStockSproutInfo.Count > 0)
                                {
                                    objCheckStockSproutInfo = ObjectSpace.GetObjects<StockSproutInfo>(CriteriaOperator.Parse("FinanceYearOid=? and AnimalSeedOid=? and OrganizationOid=? and SeedTypeOid=?", row.SupplierSproutUseProduct.FinanceYearOid.Oid, row.AnimalSeedOid.Oid, row.SupplierSproutUseProduct.OrganizationOid.Oid, row.SeedTypeOid));

                                    //   'Update สถานะ IsApprove ให้เป็น True
                                    var objStockSproutInfoEdit = ObjectSpace.FindObject<StockSproutInfo>(CriteriaOperator.Parse("[SupplierSproutNumber]=? and [FinanceYearOid]=? and [BudgetSourceOid]=? and [OrganizationOid]=? and [AnimalSeedOid]=? and [SeedTypeOid]=?", row.SupplierSproutUseProduct.UseNo, row.SupplierSproutUseProduct.FinanceYearOid, objCheckStockSproutInfo[0].BudgetSourceOid, row.SupplierSproutUseProduct.OrganizationOid, row.AnimalSeedOid, row.SeedTypeOid));

                                    //--------------------------------------------------------
                                    IList<StockSproutInfo> collection = ObjectSpace.GetObjects<StockSproutInfo>(CriteriaOperator.Parse("GCRecord is null and [SupplierSproutNumber] = ?", row.SupplierSproutUseProduct.UseNo));
                                    if (objStockSproutInfoEdit != null)
                                    {
                                        objStockSproutInfoEdit.IsApprove = true;
                                    }
                                    //var q = (from Item in collection
                                    //                  orderby Item.SupplierSproutNumber descending
                                    //                                       select Item).First();
                                    //SqlParameter[] prm = new SqlParameter[10];
                                    //prm[0] = new SqlParameter("@SupplierSproutNumber", row.SupplierSproutUseProduct.UseNo.ToString());
                                    //prm[1] = new SqlParameter("@FinanceYearOid", row.SupplierSproutUseProduct.FinanceYearOid.Oid);
                                    //prm[2] = new SqlParameter("@BudgetSourceOid", objCheckStockSproutInfo.First().BudgetSourceOid.Oid);
                                    //prm[3] = new SqlParameter("@AnimalSeedOid", row.AnimalSeedOid.Oid);
                                    //prm[4] = new SqlParameter("@OrganizationOid", row.SupplierSproutUseProduct.OrganizationOid.Oid);
                                    //prm[5] = new SqlParameter("@Weight", row.Weight);
                                    //prm[6] = new SqlParameter("@Remark", "คืนสต๊อกเนื่องจากไม่อนุมัติการใช้กล้าพันธุ์  (Mobile Application)");
                                    //prm[7] = new SqlParameter("@SeedTypeOid", row.SeedTypeOid.Oid);
                                    //prm[8] = new SqlParameter("@IsApprove", true);
                                    //prm[9] = new SqlParameter("@Description", null);
                                    //SqlHelper.ExecuteNonQuery(scc, CommandType.StoredProcedure, "SP_inserts_StockSproutInfo", prm);

                                    var objStockSproutInfo = ObjectSpace.CreateObject<StockSproutInfo>();
                                    objStockSproutInfo.TransactionDate = DateTime.Now;
                                    objStockSproutInfo.SupplierSproutNumber = row.SupplierSproutUseProduct.UseNo;
                                    objStockSproutInfo.FinanceYearOid = row.SupplierSproutUseProduct.FinanceYearOid;
                                    objStockSproutInfo.BudgetSourceOid = objCheckStockSproutInfo.First().BudgetSourceOid;
                                    objStockSproutInfo.AnimalSeedOid = row.AnimalSeedOid;
                                    objStockSproutInfo.OrganizationOid = row.SupplierSproutUseProduct.OrganizationOid;
                                    objStockSproutInfo.Weight = row.Weight;
                                    objStockSproutInfo.Remark = "คืนสต๊อกเนื่องจากไม่อนุมัติการใช้กล้าพันธุ์  (Mobile Application)";
                                    objStockSproutInfo.SeedTypeOid = row.SeedTypeOid;
                                    objStockSproutInfo.Description = "คืนสต๊อกเนื่องจากไม่อนุมัติการใช้กล้าพันธุ์  : " + ObjMaster.OrganizationOid.SubOrganizeName + " (Mobile Application)";
                                    objStockSproutInfo.IsApprove = true;

                                    //        ''Stock สำหรับ กปศ4ว
                                    //'=======================================================================
                                    //if (ObjMaster.Status == EnumRodBreedProductSeedStatus.Accepet || ObjMaster.Status == EnumRodBreedProductSeedStatus.Approve)
                                    //{
                                    var objStockSproutInfo_Detail = ObjectSpace.GetObjects<StockSproutInfo_Report>(CriteriaOperator.Parse("[FinanceYearOid]=? and [OrganizationOid]=? and [AnimalSeedOid]=? and [SeedTypeOid]=?", ObjMaster.FinanceYearOid, ObjMaster.OrganizationOid, row.AnimalSeedOid, row.SeedTypeOid));
                                    if (objStockSproutInfo_Detail.Count > 0)
                                    {
                                        var ObjStockSproutInfo_DetailSource = (from Item in objStockSproutInfo_Detail
                                                                               orderby Item.TransactionDate descending
                                                                               select Item).First();
                                        var objStockRodBreedInfoReportEdit = ObjectSpace.FindObject<StockSproutInfo_Report>(CriteriaOperator.Parse("[SproutProductNumber]=? and [FinanceYearOid]=? and [BudgetSourceOid]=? and [OrganizationOid]=? and [AnimalSeedOid]=? and [SeedTypeOid]=?", row.SupplierSproutUseProduct.UseNo, row.SupplierSproutUseProduct.FinanceYearOid, ObjStockSproutInfo_DetailSource.BudgetSourceOid, row.SupplierSproutUseProduct.OrganizationOid, row.AnimalSeedOid, row.SeedTypeOid));
                                        if (objStockRodBreedInfoReportEdit != null)
                                        {
                                            objStockRodBreedInfoReportEdit.IsApprove = true;
                                        }

                                        var objStockSproutInfo_DetailNew = ObjectSpace.CreateObject<StockSproutInfo_Report>();
                                        objStockSproutInfo_DetailNew.TransactionDate = DateTime.Now;
                                        objStockSproutInfo_DetailNew.SproutProductNumber = ObjMaster.UseNo;
                                        objStockSproutInfo_DetailNew.FinanceYearOid = ObjMaster.FinanceYearOid;
                                        objStockSproutInfo_DetailNew.BudgetSourceOid = ObjStockSproutInfo_DetailSource.BudgetSourceOid;
                                        objStockSproutInfo_DetailNew.OrganizationOid = ObjMaster.OrganizationOid;
                                        objStockSproutInfo_DetailNew.AnimalSeedOid = row.AnimalSeedOid;
                                        objStockSproutInfo_DetailNew.TotalForward = ObjStockSproutInfo_DetailSource.TotalWeight;
                                        objStockSproutInfo_DetailNew.TotalChange = row.Weight;
                                        objStockSproutInfo_DetailNew.SeedTypeOid = row.SeedTypeOid;
                                        objStockSproutInfo_DetailNew.Description = "คืนสต๊อกเนื่องจากไม่อนุมัติการใช้กล้าพันธุ์  : " + ObjMaster.OrganizationOid.SubOrganizeName + " (Mobile Application)";
                                        objStockSproutInfo_DetailNew.IsApprove = true;

                                    }
                                }
                            }
                            // }
                        }

                        SupplierSproutUseProduct TmpObjMaster;
                        ObjMaster.CancelMsg = CancelMsg;
                        ObjMaster.Status = EnumRodBreedProductSeedStatus.Eject;
                        ObjMaster.CancelBy = objUserInfo.UserName;
                        ObjMaster.ActionType = EnumAction.Eject;
                        ObjMaster.CancelDate = DateTime.Now;


                        ObjHistory = ObjectSpace.CreateObject<HistoryWork>();
                        // ประวัติ
                        ObjHistory.RefOid = ObjMaster.Oid.ToString();
                        ObjHistory.FormName = "กล้าพันธุ์";
                        ObjHistory.Message = "ไม่อนุมัติ (ขอใช้กล้าพันธุ์ (Mobile Application)) ลำดับที่ : " + ObjMaster.UseNo;
                        ObjHistory.CreateBy = objUserInfo.UserName;
                        ObjHistory.CreateDate = DateTime.Now;
                        ObjectSpace.CommitChanges();
                        
                        UpdateResult ret = new UpdateResult();
                        ret.status = "true";
                        ret.message = "บันทึกข้อมูลไม่อนุมัติเรียบร้อยแล้ว";
                        // ret.message = "บันทึกข้อมูลไม่อนุมัติเรียบร้อยแล้ว"+ "(Mobile Application)";
                        return Request.CreateResponse(HttpStatusCode.OK, ret);
                    }
                    else
                    {
                        UpdateResult ret = new UpdateResult();
                        ret.status = "-99";
                        ret.message = "ไม่สามารถบันทึกข้อมูลได้ ไม่มีข้อมูลกล้าพันธุ์";
                        // ret.message = "บันทึกข้อมูลไม่อนุมัติเรียบร้อยแล้ว"+ "(Mobile Application)";
                        return Request.CreateResponse(HttpStatusCode.NotFound, ret);
                    }
                }
                else
                {
                    UpdateResult ret = new UpdateResult();
                    ret.status = "False";
                    ret.message = "กรุณากรอก RefNo";
                    return Request.CreateResponse(HttpStatusCode.NotFound, ret);
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
        [Route("Sprout/list")]
        public HttpResponseMessage Sproutlist()
        {
            try
            {
                string Org = HttpContext.Current.Request.Form["organizationOid"].ToString();

                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierSproutUseProduct));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                List<SproutUseProduct_Model> Detail = new List<SproutUseProduct_Model>();
                IList<SupplierSproutUseProduct> collection = ObjectSpace.GetObjects<SupplierSproutUseProduct>(CriteriaOperator.Parse(" GCRecord is null and Status = 1 and OrganizationOid = ? ", Org));
                var query = from Q in collection orderby Q.UseNo select Q;
                if (collection.Count > 0)
                {
                    foreach (SupplierSproutUseProduct row in query)

                    {
                        SproutUseProduct_Model item = new SproutUseProduct_Model();
                        item.SproutUseProductOid = row.Oid.ToString();
                        item.FinanceYear = row.FinanceYearOid.YearName;
                        item.FinanceYearOid = row.FinanceYearOid.Oid.ToString();
                        item.ActivityOid = row.ActivityOid.Oid.ToString();
                        item.ActivityName = row.ActivityOid.ActivityName;
                        if (row.SubActivityLevelOid != null)
                        {
                            item.SubActivityLevelOid = row.SubActivityLevelOid.Oid.ToString();
                            item.SubActivityLevelName = row.SubActivityLevelOid.ActivityName;
                        }

                        item.FinanceYearOid = row.FinanceYearOid.Oid.ToString();
                        item.FinanceYear = row.FinanceYearOid.YearName;
                        item.UseNo = row.UseNo;
                        item.UseDate = row.UseDate.ToString("dd/MM/yy");
                        item.OrganizationOid = row.OrganizationOid.Oid.ToString();
                        item.OrganizationName = row.OrganizationOid.SubOrganizeName;
                        if (row.EmployeeOid != null)
                        {
                            item.EmployeeOid = row.EmployeeOid.Oid.ToString();
                            item.FullName = row.EmployeeOid.FullName;
                        }
                        if (row.RegisCusServiceOid != null)
                        {
                            item.RegisCusServiceOid = row.RegisCusServiceOid.Oid.ToString();
                            item.FullName = row.RegisCusServiceOid.DisPlayName;
                            item.FullName = row.RegisCusServiceOid.FullAddress;
                        }
                        if (row.OrgeServiceOid != null)
                        {
                            item.OrgeServiceOid = row.OrgeServiceOid.Oid.ToString();
                            item.FullName = row.OrgeServiceOid.OrgeServiceName;
                            item.FullAddress = row.OrgeServiceOid.FullAddress;
                        }

                        item.ServiceCount = row.ServiceCount;
                        item.TotalWeight = row.SupplierSproutUseProductDetails.Sum((c => c.Weight)).ToString() + " กิโลกรัม";
                        Detail.Add(item);
                    }
                    directProvider.Dispose();
                    ObjectSpace.Dispose();
                    //lists.Receive = ReceiveItems;
                    return Request.CreateResponse(HttpStatusCode.OK, Detail);
                }
                else
                {
                    UserError err2 = new UserError();
                    err2.status = "false";
                    err2.code = "-9";
                    err2.message = "ไม่มีข้อมูลรายการ การใช้กล้าพันธุ์";
                    return Request.CreateResponse(HttpStatusCode.NotFound, err2);
                }
            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.status = "false";
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.ToString();
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Sprout/Detail")]
        public HttpResponseMessage Sproutlisttdetail()
        {
            try
            {
                string UseNo = HttpContext.Current.Request.Form["useNo"].ToString();

                if (UseNo != "")
                {
                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierSproutUseProduct));
                    RodbreedSproutUseProduct_model_Detail item = null;
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                    IList<SupplierSproutUseProduct> collection = ObjectSpace.GetObjects<SupplierSproutUseProduct>(CriteriaOperator.Parse("GCRecord is null and UseNo='" + UseNo + "'", null));

                    foreach (SupplierSproutUseProduct row in collection)
                    {
                        item = new RodbreedSproutUseProduct_model_Detail();
                        item.SproutUseProductOid = row.Oid.ToString();
                        item.UseNo = row.UseNo;
                        item.FinanceYear = row.FinanceYearOid.YearName;
                        item.FinanceYearOid = row.FinanceYearOid.Oid.ToString();
                        item.UseDate = row.UseDate.ToString("dd/MM/yyyy");
                        item.OrganizationOid = row.OrganizationOid.Oid.ToString();
                        item.OrganizationName = row.OrganizationOid.SubOrganizeName;
                        item.ActivityOid = row.ActivityOid.Oid.ToString();
                        item.Remark = row.Remark;
                        item.ActivityName = row.ActivityOid.ActivityName;
                        item.FinanceYearOid = row.FinanceYearOid.Oid.ToString();
                        item.FinanceYear = row.FinanceYearOid.YearName;

                        if (row.OrgeServiceOid != null)
                        {
                            item.OrgeServiceOid = row.OrgeServiceOid.Oid.ToString();
                            item.FullName = row.OrgeServiceOid.OrgeServiceName;
                            item.FullAddress = row.OrgeServiceOid.FullAddress;
                        }
                        if (row.RegisCusServiceOid != null)
                        {
                            item.RegisCusServiceOid = row.RegisCusServiceOid.Oid.ToString();
                            item.FullName = row.RegisCusServiceOid.DisPlayName;
                            item.FullAddress = row.RegisCusServiceOid.FullAddress;
                        }
                        if (row.SubActivityOid != null)
                        {
                            item.SubActivityOid = row.SubActivityOid.Oid.ToString();
                            item.SubActivityName = row.SubActivityOid.ActivityName;
                        }
                        if (row.SubActivityLevelOid != null)
                        {
                            item.SubActivityLevelOid = row.SubActivityLevelOid.Oid.ToString();
                            item.SubActivityLevelName = row.SubActivityLevelOid.ActivityName;
                        }

                        item.Status = row.Status.ToString();
                        item.ServiceCount = row.ServiceCount;
                        item.TotalWeight = row.SupplierSproutUseProductDetails.Sum((c => c.Weight)).ToString() + "" + "กิโลกกรัม";
                        item.TotalPrice = row.SupplierSproutUseProductDetails.Sum((c => c.Price)).ToString() + "" + "บาท ";
                        item.CancelMsg = row.CancelMsg;
                        if (row.ActivityOid.ActivityName == "เพื่อการจำหน่าย")
                        {
                            item.ReceiptNo = row.ReceiptNo;
                        }
                        List<SupplierSproutUseProductDetail_Model> details = new List<SupplierSproutUseProductDetail_Model>();
                        SupplierSproutUseProductDetail_Model _dt = null;

                        foreach (SupplierSproutUseProductDetail rw in row.SupplierSproutUseProductDetails)
                        {
                            _dt = new SupplierSproutUseProductDetail_Model();
                            _dt.AnimalSeedOid = rw.AnimalSeedOid.Oid.ToString();
                            _dt.SupplierSproutUseProductDetailOid = rw.Oid.ToString();
                            _dt.StockLimit = rw.StockLimit;
                            _dt.Weight = rw.Weight;
                            _dt.SeedTypeOid = rw.SeedTypeOid.Oid.ToString();
                            _dt.SeedTypeName = rw.SeedTypeOid.SeedTypeName;
                            _dt.AnimalSeedName = rw.AnimalSeedOid.SeedName;
                            _dt.Price = rw.Price;
                            _dt.PerPrice = rw.PerPrice;
                            _dt.SupplierSproutUseProductOid = rw.SupplierSproutUseProduct.Oid.ToString();
                            _dt.PackageOid = rw.PackageOid.Oid.ToString();
                            _dt.PackageName = rw.PackageOid.PackageName;
                            details.Add(_dt);
                        }

                        item.Details = details;
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, item);
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
            finally
            {
                SqlConnection.ClearAllPools();
            }
        }
    }
}