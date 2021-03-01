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
    public class RodBreed_Controller : ApiController
    {
        private string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        // GET: api/RodBreed_
        /// <summary>
        ///
        /// </summary>
        /// <returns> <see langword="await"/></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("RodBreed/List")]
        public HttpResponseMessage RodBreedList()
        {
            try
            {
                string Org = HttpContext.Current.Request.Form["organizationOid"].ToString();
                //  string type = HttpContext.Current.Request.Form["type"].ToString();
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierRodBreedUseProduct));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                List<Rodbreed_model> Detail = new List<Rodbreed_model>();
                IList<SupplierRodBreedUseProduct> collection = ObjectSpace.GetObjects<SupplierRodBreedUseProduct>(CriteriaOperator.Parse(" GCRecord is null and Status = 1 and OrganizationOid = ? ", Org));
                var query = from Q in collection orderby Q.UseNo select Q;
                if (collection.Count > 0)
                {
                    foreach (SupplierRodBreedUseProduct row in query)
                    {
                        Rodbreed_model item = new Rodbreed_model();
                        item.SupplierRodBreedUseProductOid = row.Oid.ToString();
                        item.FinanceYearOid = row.FinanceYearOid.Oid.ToString();
                        item.FinanceYear = row.FinanceYearOid.YearName;
                        item.ActivityNameOid = row.ActivityOid.Oid.ToString();
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
                            item.EmployeeName = row.EmployeeOid.FullName;
                        }
                        if (row.RegisCusServiceOid != null)
                        {
                            item.RegisCusServiceOid = row.RegisCusServiceOid.Oid.ToString();
                            item.FullName = row.RegisCusServiceOid.DisPlayName;
                            item.FullAddress = row.RegisCusServiceOid.FullAddress;
                        }
                        if (row.OrgeServiceOid != null)
                        {
                            item.OrgeServiceOid = row.OrgeServiceOid.Oid.ToString();
                            item.FullName = row.OrgeServiceOid.OrgeServiceName;
                            item.FullAddress = row.OrgeServiceOid.FullAddress;
                        }

                        item.ServiceCount = row.ServiceCount;
                        item.TotalWeight = row.SupplierRodBreedUseProductDetails.Sum((c => c.Weight)).ToString() + " กิโลกรัม";
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
                    err2.message = "ไม่มีข้อมูลรายการ การใช้ท่อนพันธุ์";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err2);
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

        //// GET: api/RodBreed_/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/RodBreed_
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/RodBreed_/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/RodBreed_/5
        public void Delete(int id)
        {
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RodBreed/Detail")]
        public HttpResponseMessage RodBreedListdetail()
        {
            try
            {
                string UseNo = HttpContext.Current.Request.Form["useNo"].ToString();

                if (UseNo != "")
                {
                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierRodBreedUseProduct));
                    Rodbreed_model_Detail item = null;
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                    IList<SupplierRodBreedUseProduct> collection = ObjectSpace.GetObjects<SupplierRodBreedUseProduct>(CriteriaOperator.Parse("GCRecord is null and UseNo='" + UseNo + "'", null));

                    foreach (SupplierRodBreedUseProduct row in collection)
                    {
                        item = new Rodbreed_model_Detail();
                        item.SupplierRodBreedUseProductOid = row.Oid.ToString();
                        item.FinanceYearOid = row.FinanceYearOid.Oid.ToString();
                        item.FinanceYear = row.FinanceYearOid.YearName;
                        item.UseNo = row.UseNo;
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
                        item.TotalWeight = row.SupplierRodBreedUseProductDetails.Sum((c => c.Weight)).ToString() + "" + "กิโลกรัม";
                        item.TotalPrice = row.SupplierRodBreedUseProductDetails.Sum((c => c.Price)).ToString() + "" + "บาท";
                        item.CancelMsg = row.CancelMsg;
                        if (row.ActivityOid.ActivityName == "เพื่อการจำหน่าย")
                        {
                            item.ReceiptNo = row.ReceiptNo;
                        }
                        List<SupplierRodBreedUseProductDetail_Model> details = new List<SupplierRodBreedUseProductDetail_Model>();
                        SupplierRodBreedUseProductDetail_Model _dt = null;

                        foreach (SupplierRodBreedUseProductDetail rw in row.SupplierRodBreedUseProductDetails)
                        {
                            _dt = new SupplierRodBreedUseProductDetail_Model();
                            _dt.AnimalSeedOid = rw.AnimalSeedOid.Oid.ToString();
                            _dt.SupplierRodBreedUseProductDetailOid = rw.Oid.ToString();
                            _dt.StockLimit = rw.StockLimit;
                            _dt.Weight = rw.Weight;
                            _dt.WeightUnitOid = rw.WeightUnitOid.Oid.ToString();
                            _dt.WeightUnitName = rw.WeightUnitOid.UnitName;
                            _dt.SeedTypeOid = rw.SeedTypeOid.Oid.ToString();
                            _dt.SeedTypeName = rw.SeedTypeOid.SeedTypeName;
                            _dt.AnimalSeedName = rw.AnimalSeedOid.SeedName;
                            _dt.Price = rw.Price;
                            _dt.PerPrice = rw.PerPrice;
                            _dt.SupplierRodBreedUseProductOid = rw.SupplierRodBreedUseProductOid.Oid.ToString();
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

        [AllowAnonymous]
        [HttpPost]
        [Route("RodBreed/UpdateApprove")]
        public HttpResponseMessage RodBreedUseApprove()
        {
            //  object objStockAnimalInfo = null;
            HistoryWork ObjHistory = null;
            IList<StockSeedInfo> objStockSeedInfo = null;
            string Username = " ";
            object objDetailService = null;

            try
            {
                string RefNo = HttpContext.Current.Request.Form["RefNo"].ToString(); //ข้อมูลเลขที่อ้างอิง
                                                                                     // string Status = HttpContext.Current.Request.Form["Status"].ToString(); //สถานะ
                string CancelMsg = HttpContext.Current.Request.Form["Remark"].ToString(); //หมายเหตุ
                Username = HttpContext.Current.Request.Form["Username"].ToString();
                if (RefNo != "")
                {
                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierRodBreedUseProduct));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.StockRodBreedInfo));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.StockRodBreedInfo_Report));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.HistoryWork));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierProductModifyDetail));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierProductModify));
                    XafTypesInfo.Instance.RegisterEntity(typeof(UserInfo));
                    XafTypesInfo.Instance.RegisterEntity(typeof(ReceiveLotNumber));
                    XafTypesInfo.Instance.RegisterEntity(typeof(RegisterCusServiceDetail));
                    XafTypesInfo.Instance.RegisterEntity(typeof(OrgeServiceDetail));
                    XafTypesInfo.Instance.RegisterEntity(typeof(ServiceType));

                    List<SupplierRodBreedUseProduct> list = new List<SupplierRodBreedUseProduct>();
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                    UserInfo objUserInfo = ObjectSpace.FindObject<UserInfo>(CriteriaOperator.Parse("[UserName]=?", Username));

                    SupplierRodBreedUseProduct ObjMaster = ObjectSpace.FindObject<SupplierRodBreedUseProduct>(CriteriaOperator.Parse("UseNo=?", RefNo));

                    if (ObjMaster.SupplierRodBreedUseProductDetails != null)
                    {
                        foreach (SupplierRodBreedUseProductDetail row in ObjMaster.SupplierRodBreedUseProductDetails)
                        {

                            if (ObjMaster.Status == EnumRodBreedProductSeedStatus.Accepet)
                            {

                                //'Update สถานะ IsApprove ให้เป็น True
                                var objStockRodBreedInfoEdit = ObjectSpace.FindObject<StockRodBreedInfo>(CriteriaOperator.Parse("[RodBreedProductNumber]=? and [FinanceYearOid]=? and [OrganizationOid]=? and [AnimalSeedOid]=? and [SeedTypeOid]=?", row.SupplierRodBreedUseProductOid.UseNo, row.SupplierRodBreedUseProductOid.FinanceYearOid, row.SupplierRodBreedUseProductOid.OrganizationOid, row.AnimalSeedOid, row.SeedTypeOid));
                                if (objStockRodBreedInfoEdit != null)
                                {
                                    objStockRodBreedInfoEdit.IsApprove = true;

                                }


                                //          ''Stock สำหรับ กปศ4ว
                                //         '=======================================================================

                                //    'Update สถานะ IsApprove ให้เป็น True
                                var objStockRodBreedInfoReportEdit = ObjectSpace.FindObject<StockRodBreedInfo_Report>(CriteriaOperator.Parse("[RodBreedProductNumber]=? and [FinanceYearOid]=? and [OrganizationOid]=? and [AnimalSeedOid]=? and [SeedTypeOid]=?", row.SupplierRodBreedUseProductOid.UseNo, row.SupplierRodBreedUseProductOid.FinanceYearOid, row.SupplierRodBreedUseProductOid.OrganizationOid, row.AnimalSeedOid, row.SeedTypeOid));
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
                                objDetailService = ObjectSpace.FindObject<RegisterCusServiceDetail>(CriteriaOperator.Parse("[RegisterCusServiceOid]=? and [ServiceTypeOid.ServiceTypeName] like '%จำหน่าย%' and [SubServiceTypeOid.ServiceTypeName]='ท่อนพันธุ์' and [ReceiveDate]=?", ObjMaster.RegisCusServiceOid, ObjMaster.UseDate));
                                if (objDetailService == null)
                                {
                                    RegisterCusServiceDetail objDetailService_new = ObjectSpace.CreateObject<RegisterCusServiceDetail>();
                                    objDetailService_new.RegisterCusServiceOid = ObjMaster.RegisCusServiceOid;

                                    ServiceType objServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[ServiceTypeName] like '%จำหน่าย%'"));
                                    objDetailService_new.ServiceTypeOid = objServiceType;

                                    ServiceType objSubServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[MasterServiceType]=? and [ServiceTypeName]='ท่อนพันธุ์'", objServiceType.Oid));
                                    objDetailService_new.SubServiceTypeOid = objSubServiceType;

                                    objDetailService_new.ReceiveDate = ObjMaster.UseDate;
                                    objDetailService_new.RefOid = ObjMaster.Oid.ToString();
                                }
                            }
                            else if (ObjMaster.ChkGroupService == true)
                            {
                                objDetailService = ObjectSpace.FindObject<OrgeServiceDetail>(CriteriaOperator.Parse("[OrgeServiceOid]=? and [ServiceTypeOid.ServiceTypeName] like '%จำหน่าย%' and [SubServiceTypeOid.ServiceTypeName]='ท่อนพันธุ์' and [ReceiveDate]=?", ObjMaster.OrgeServiceOid, ObjMaster.UseDate));
                                if (objDetailService == null) //'ถ้ายังไม่มีข้อมูลการขอรับบบริการให้ Insert ลงในส่วนของรายละเอียดการขอรับบริการด้วย
                                {
                                    OrgeServiceDetail objDetailService_new = ObjectSpace.CreateObject<OrgeServiceDetail>();
                                    objDetailService_new.OrgeServiceOid = ObjMaster.OrgeServiceOid;
                                    ServiceType objServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[ServiceTypeName] like '%จำหน่าย%'"));
                                    objDetailService_new.ServiceTypeOid = objServiceType;
                                    ServiceType objSubServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[MasterServiceType]=? and [ServiceTypeName]='ท่อนพันธุ์'", objServiceType.Oid));
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
                                objDetailService = ObjectSpace.FindObject<RegisterCusServiceDetail>(CriteriaOperator.Parse("[RegisterCusServiceOid]=? and [ServiceTypeOid.ServiceTypeName] like '%แจกจ่าย%' and [SubServiceTypeOid.ServiceTypeName]='ท่อนพันธุ์' and [ReceiveDate]=?", ObjMaster.RegisCusServiceOid, ObjMaster.UseDate));
                                if (objDetailService == null)// 'ถ้ายังไม่มีข้อมูลการขอรับบบริการให้ Insert ลงในส่วนของรายละเอียดการขอรับบริการด้วย
                                {
                                    RegisterCusServiceDetail objDetailService_new = ObjectSpace.CreateObject<RegisterCusServiceDetail>();
                                    objDetailService_new.RegisterCusServiceOid = ObjMaster.RegisCusServiceOid;


                                    ServiceType objServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[ServiceTypeName] like '%แจกจ่าย%'"));
                                    objDetailService_new.ServiceTypeOid = objServiceType;
                                    ServiceType objSubServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[MasterServiceType]=? and [ServiceTypeName]='ท่อนพันธุ์'", objServiceType.Oid));
                                    objDetailService_new.SubServiceTypeOid = objSubServiceType;

                                    objDetailService_new.ReceiveDate = ObjMaster.UseDate;
                                    objDetailService_new.RefOid = ObjMaster.Oid.ToString();
                                }
                            }
                            else if (ObjMaster.ChkGroupService == true)
                            {
                                objDetailService = ObjectSpace.FindObject<OrgeServiceDetail>(CriteriaOperator.Parse("[OrgeServiceOid]=? and [ServiceTypeOid.ServiceTypeName] like '%แจกจ่าย%' and [SubServiceTypeOid.ServiceTypeName]='ท่อนพันธุ์' and [ReceiveDate]=?", ObjMaster.OrgeServiceOid, ObjMaster.UseDate));
                                if (objDetailService == null) //'ถ้ายังไม่มีข้อมูลการขอรับบบริการให้ Insert ลงในส่วนของรายละเอียดการขอรับบริการด้วย
                                {
                                    OrgeServiceDetail objDetailService_new = ObjectSpace.CreateObject<OrgeServiceDetail>();
                                    objDetailService_new.OrgeServiceOid = ObjMaster.OrgeServiceOid;
                                    ServiceType objServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[ServiceTypeName] like '%แจกจ่าย%'"));
                                    objDetailService_new.ServiceTypeOid = objServiceType;
                                    ServiceType objSubServiceType = ObjectSpace.FindObject<ServiceType>(CriteriaOperator.Parse("[MasterServiceType]=? and [ServiceTypeName]='ท่อนพันธุ์'", objServiceType.Oid));
                                    objDetailService_new.SubServiceTypeOid = objSubServiceType;

                                    objDetailService_new.ReceiveDate = ObjMaster.UseDate;
                                    objDetailService_new.RefOid = ObjMaster.Oid.ToString();
                                }

                            }
                        }


                        ObjMaster.Status = EnumRodBreedProductSeedStatus.Approve;
                        ObjMaster.ApproveDate = DateTime.Now;
                        //     ObjectSpace.CommitChanges();

                        ObjHistory = ObjectSpace.CreateObject<HistoryWork>();
                        // ประวัติ
                        ObjHistory.RefOid = ObjMaster.Oid.ToString();
                        ObjHistory.FormName = "ท่อนพันธุ์";
                        ObjHistory.Message = "อนุมัติ (ขอใช้ท่อนพันธุ์ (Mobile Application) ) ลำดับที่ : " + ObjMaster.UseNo;
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
                        return Request.CreateResponse(HttpStatusCode.NotFound, ret);
                    }
                }
                else
                {
                    UpdateResult ret = new UpdateResult();
                    ret.status = "False";
                    ret.message = "ไม่มีข้อมูลรายการข้อมูลท่อนพันธุ์";
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
        [Route("RodBreed/UpdateEject")]
        public HttpResponseMessage UpdateRodBreedUseEject()
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
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierRodBreedUseProduct));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.StockRodBreedInfo));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.StockRodBreedInfo_Report));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.HistoryWork));
                    XafTypesInfo.Instance.RegisterEntity(typeof(UserInfo));
                    XafTypesInfo.Instance.RegisterEntity(typeof(ReceiveLotNumber));

                    List<SupplierRodBreedUseProduct> list = new List<SupplierRodBreedUseProduct>();
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                    UserInfo objUserInfo = ObjectSpace.FindObject<UserInfo>(CriteriaOperator.Parse("[UserName]=?", Username));

                    SupplierRodBreedUseProduct ObjMaster = ObjectSpace.FindObject<SupplierRodBreedUseProduct>(CriteriaOperator.Parse("UseNo=?", RefNo));

                    foreach (SupplierRodBreedUseProductDetail row in ObjMaster.SupplierRodBreedUseProductDetails)
                    {
                        if (ObjMaster.Status == EnumRodBreedProductSeedStatus.Approve || ObjMaster.Status == EnumRodBreedProductSeedStatus.Accepet)
                        {
                            var objCheckStockRodBreedInfo = ObjectSpace.GetObjects<StockRodBreedInfo>(CriteriaOperator.Parse("RodBreedProductNumber=? and FinanceYearOid=? and AnimalSeedOid=? and OrganizationOid=? and [SeedTypeOid]=?", row.SupplierRodBreedUseProductOid.UseNo, row.SupplierRodBreedUseProductOid.FinanceYearOid, row.AnimalSeedOid, row.SupplierRodBreedUseProductOid.OrganizationOid, row.SeedTypeOid));

                            var objStockRodBreedInfo_Detail = ObjectSpace.GetObjects<StockRodBreedInfo_Report>(CriteriaOperator.Parse("[FinanceYearOid]=? and [OrganizationOid]=? and [AnimalSeedOid]=? and [SeedTypeOid]=?", ObjMaster.FinanceYearOid, ObjMaster.OrganizationOid, row.AnimalSeedOid, row.SeedTypeOid));


                            if (objCheckStockRodBreedInfo.Count > 0)
                            {
                                //   'Update สถานะ IsApprove ให้เป็น True
                                var objStockRodBreedInfoEdit = ObjectSpace.FindObject<StockRodBreedInfo>(CriteriaOperator.Parse("[RodBreedProductNumber]=? and [FinanceYearOid]=? and [OrganizationOid]=? and [AnimalSeedOid]=? and [SeedTypeOid]=?", row.SupplierRodBreedUseProductOid.UseNo, row.SupplierRodBreedUseProductOid.FinanceYearOid, row.SupplierRodBreedUseProductOid.OrganizationOid, row.AnimalSeedOid, row.SeedTypeOid));
                                if (objStockRodBreedInfoEdit != null)
                                {
                                    objStockRodBreedInfoEdit.IsApprove = true;
                                }
                                //SqlParameter[] prm = new SqlParameter[10];
                                //prm[0] = new SqlParameter("@RodBreedProductNumber", row.SupplierRodBreedUseProductOid.UseNo.ToString());
                                //prm[1] = new SqlParameter("@FinanceYearOid", row.SupplierRodBreedUseProductOid.FinanceYearOid.Oid);
                                //prm[2] = new SqlParameter("@BudgetSourceOid", null);
                                //prm[3] = new SqlParameter("@AnimalSeedOid", row.AnimalSeedOid.Oid);
                                //prm[4] = new SqlParameter("@OrganizationOid", row.SupplierRodBreedUseProductOid.OrganizationOid.Oid);
                                //prm[5] = new SqlParameter("@Weight", row.Weight);
                                //prm[6] = new SqlParameter("@Remark", "คืนสต๊อกเนื่องจากไม่อนุมัติการใช้ท่อนพันธุ์  (Mobile Application)");
                                //prm[7] = new SqlParameter("@SeedTypeOid", row.SeedTypeOid.Oid);
                                //prm[8] = new SqlParameter("@IsApprove", true);
                                //prm[9] = new SqlParameter("@Description", null);
                                //SqlHelper.ExecuteNonQuery(scc, CommandType.StoredProcedure, "SP_StockRodBreedInfo", prm);

                                var objStockRodBreedInfo = ObjectSpace.CreateObject<StockRodBreedInfo>();
                                {
                                    objStockRodBreedInfo.TransactionDate = DateTime.Now;
                                    objStockRodBreedInfo.RodBreedProductNumber = row.SupplierRodBreedUseProductOid.UseNo;
                                    objStockRodBreedInfo.FinanceYearOid = row.SupplierRodBreedUseProductOid.FinanceYearOid;
                                    // '.BudgetSourceOid = objCheckStockRodBreedInfo(0).BudgetSourceOid
                                    objStockRodBreedInfo.AnimalSeedOid = row.AnimalSeedOid;
                                    objStockRodBreedInfo.OrganizationOid = row.SupplierRodBreedUseProductOid.OrganizationOid;
                                    objStockRodBreedInfo.Weight = row.Weight;
                                    objStockRodBreedInfo.Remark = "คืนสต๊อกเนื่องจากไม่อนุมัติการใช้ท่อนพันธุ์ (Mobile Application)";
                                    objStockRodBreedInfo.SeedTypeOid = row.SeedTypeOid;
                                    objStockRodBreedInfo.IsApprove = true;
                                }
                            }
                            //                ''Stock สำหรับ กปศ4ว

                            //if (ObjMaster.Status == EnumRodBreedProductSeedStatus.Approve || ObjMaster.Status == EnumRodBreedProductSeedStatus.Accepet)
                            //{
                             objStockRodBreedInfo_Detail = ObjectSpace.GetObjects<StockRodBreedInfo_Report>(CriteriaOperator.Parse("[FinanceYearOid]=? and [OrganizationOid]=? and [AnimalSeedOid]=? and [SeedTypeOid]=?", ObjMaster.FinanceYearOid, ObjMaster.OrganizationOid, row.AnimalSeedOid, row.SeedTypeOid));
                            if (objStockRodBreedInfo_Detail.Count > 0)
                            {
                                var objStockRodBreedInfo_DetailNew = ObjectSpace.CreateObject<StockRodBreedInfo_Report>();
                                //'Update สถานะ IsApprove ให้เป็น True
                                var objStockRodBreedInfoEdit = ObjectSpace.FindObject<StockRodBreedInfo_Report>(CriteriaOperator.Parse("[RodBreedProductNumber]=? and [FinanceYearOid]=? and [OrganizationOid]=? and [AnimalSeedOid]=? and [SeedTypeOid]=?", row.SupplierRodBreedUseProductOid.UseNo, row.SupplierRodBreedUseProductOid.FinanceYearOid, row.SupplierRodBreedUseProductOid.OrganizationOid, row.AnimalSeedOid, row.SeedTypeOid));
                                if (objStockRodBreedInfoEdit != null)
                                {
                                    objStockRodBreedInfoEdit.IsApprove = true;
                                }
                                var ObjStockRodBreedInfo_DetailSource = (from Item in objStockRodBreedInfo_Detail
                                                                         orderby Item.TransactionDate descending
                                                                         select Item).First();
                                objStockRodBreedInfo_DetailNew.TransactionDate = DateTime.Now;
                                objStockRodBreedInfo_DetailNew.RodBreedProductNumber = ObjMaster.UseNo;
                                objStockRodBreedInfo_DetailNew.FinanceYearOid = ObjMaster.FinanceYearOid;
                                // .BudgetSourceOid = objStockRodBreedInfo_Detail(0).BudgetSourceOid
                                objStockRodBreedInfo_DetailNew.OrganizationOid = ObjMaster.OrganizationOid;
                                objStockRodBreedInfo_DetailNew.AnimalSeedOid = row.AnimalSeedOid;
                                objStockRodBreedInfo_DetailNew.TotalForward = ObjStockRodBreedInfo_DetailSource.TotalWeight;
                                objStockRodBreedInfo_DetailNew.TotalChange = row.Weight;
                                objStockRodBreedInfo_DetailNew.SeedTypeOid = row.SeedTypeOid;
                                objStockRodBreedInfo_DetailNew.Description = "คืนสต๊อกเนื่องจากไม่อนุมัติการใช้ท่อนพันธุ์ : " + ObjMaster.OrganizationOid.SubOrganizeName + "(Mobile Application)";
                                objStockRodBreedInfo_DetailNew.IsApprove = true;
                            }
                        }
                        //}
                    }
                    //     SupplierSproutUseProduct TmpObjMaster ;
                    ObjMaster.CancelMsg = CancelMsg;
                    ObjMaster.Status = EnumRodBreedProductSeedStatus.Eject;
                    ObjMaster.CancelBy = objUserInfo.DisplayName;
                    ObjMaster.ActionType = EnumAction.Eject;
                    ObjMaster.CancelDate = DateTime.Now;

                    ObjHistory = ObjectSpace.CreateObject<HistoryWork>();
                    // ประวัติ
                    ObjHistory.RefOid = ObjMaster.Oid.ToString();
                    ObjHistory.FormName = "ท่อนพันธุ์";
                    ObjHistory.Message = "ไม่อนุมัติ (ขอใช้ท่อนพันธุ์ (Mobile Application)) ลำดับที่ : " + ObjMaster.UseNo;
                    ObjHistory.CreateBy = objUserInfo.UserName;
                    ObjHistory.CreateDate = DateTime.Now;
                    ObjectSpace.CommitChanges();

                    UpdateResult ret = new UpdateResult();
                    ret.status = "true";
                    ret.message = "บันทึกข้อมูลไม่อนุมัติเรียบร้อยแล้ว";
                    return Request.CreateResponse(HttpStatusCode.OK, ret);
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
    }
}