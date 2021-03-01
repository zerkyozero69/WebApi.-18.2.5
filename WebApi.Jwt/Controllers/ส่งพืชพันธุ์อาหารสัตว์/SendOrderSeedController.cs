using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using nutrition.Module;
using nutrition.Module.EmployeeAsUserExample.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Jwt.Models;

namespace WebApi.Jwt.Controllers
{
    /// <summary>
    /// ข้อมูลส่ง-รับเมล็ดพันธุ์ให้หน่วยงาน
    /// ==================
    /// SendStatus:
    ///     รับเมล็ด
    ///         NotSend = 0 (อยู่ระหว่างดำเนินการ)
    ///         Send = 1(จัดส่งเมล็ดพันธุ์แล้ว)
    ///         Accept = 2 (ตรวจสอบแล้ว)
    ///         Approve = 3(อนุมัติรับเมล็ดพันธุ์)
    ///         Cancel = 4 (ยกเลิก)
    ///         Eject = 8(ไม่อนุมัติ)
    ///     ==================
    ///     ส่งเมล็ด
    ///         SendAccept = 5(ตรวจสอบแล้ว)
    ///         SendApprove = 6 (อนุมัติส่งเมล็ดพันธุ์)
    ///         SendCancel = 7(ยกเลิกส่งเมล็ดพันธุ์)
    ///         SendEject = 9 (ไม่อนุมัติส่งเมล็ดพันธุ์)
    /// </summary>
    public class SendOrderSeedController : ApiController
    {
        //database connection.
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        /// <summary>
        /// แสดงข้อมูลส่ง-รับเมล็ดพันธุ์ให้หน่วยงาน
        /// </summary>
        /// <param name="Org_Oid">OID ของหน่วยงาน</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SendOrderSeed/List")]
        public HttpResponseMessage GetSendOrderSeed()
        {
            try
            {
                string org_oid = HttpContext.Current.Request.Form["Org_Oid"].ToString();
                string type = HttpContext.Current.Request.Form["type"].ToString(); //รับ=1/ส่ง=2

                if (org_oid != "" && type != "")
                {
                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSeed));

                    List<SendOrderSeedType> SendItems = new List<SendOrderSeedType>();
                    List<ReceiveOrderSeedType> ReceiveItems = new List<ReceiveOrderSeedType>();
                    SendOrderSeedModel lists = new SendOrderSeedModel();
                    lists.org_oid = org_oid;

                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);

                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                    if (type == "2")
                    {  //หน่วยส่ง

                        IList<SendOrderSeed> collection = ObjectSpace.GetObjects<SendOrderSeed>(CriteriaOperator.Parse("GCRecord is null and SendStatus=1 and [SendOrgOid.Oid]='" + org_oid + "'", null));
                        var query = from Q in collection orderby Q.SendNo select Q;
                        if (collection.Count > 0)
                        {
                            foreach (SendOrderSeed row in query)
                            {
                                SendOrderSeedType item = new SendOrderSeedType();
                                item.SendNo = row.SendNo;
                                item.SendDate = row.SendDate.ToString("dd/MM/yyyy");
                                item.SendOrgOid = row.SendOrgOid.Oid.ToString();
                                item.SendOrgName = row.SendOrgOid.SubOrganizeName;
                                item.SendOrgFullName = row.SendOrgOid.OrganizeNameTH;
                                item.Remark = row.Remark;
                                item.SendStatus = row.SendStatus.ToString();
                                item.FinanceYear = row.FinanceYearOid.YearName;
                                item.CancelMsg = row.CancelMsg;
                                item.ReceiveOrgOid = row.ReceiveOrgOid.Oid.ToString();
                                item.ReceiveOrgName = row.ReceiveOrgOid.SubOrganizeName;
                                item.ReceiveOrgFullName = row.ReceiveOrgOid.OrganizeNameTH;
                                item.RefNo = row.SendNo + "|" + row.SendOrgOid.Oid.ToString() + "|2";

                                item.TotalWeight = row.SendOrderSeedDetails.Sum((c => c.Weight)).ToString() + " กิโลกรัม";
                                SendItems.Add(item);
                            }
                        }
                        //lists.Sender = null; //SendItems;
                        return Request.CreateResponse(HttpStatusCode.OK, SendItems);
                    }
                    else if (type == "1")
                    {  //รับ

                        IList<SendOrderSeed> collection2 = ObjectSpace.GetObjects<SendOrderSeed>(CriteriaOperator.Parse("GCRecord is null and SendStatus = 2 and  ReceiveStatus=1 and ReceiveOrgOid.Oid='" + org_oid + "'", null));
                        var query = from Q in collection2 orderby Q.SendNo select Q;
                        if (collection2.Count > 0)
                        {
                            foreach (SendOrderSeed row in query)
                            {
                                ReceiveOrderSeedType item = new ReceiveOrderSeedType();
                                item.SendNo = row.SendNo;
                                item.SendDate = row.SendDate.ToString("dd/MM/yyyy");
                                item.SendOrgOid = row.SendOrgOid.Oid.ToString();
                                item.SendOrgName = row.SendOrgOid.SubOrganizeName;
                                item.SendOrgFullName = row.SendOrgOid.OrganizeNameTH;
                                item.Remark = row.Remark;
                                item.ReceiveOrderStatus = row.ReceiveStatus.ToString();
                                item.FinanceYear = row.FinanceYearOid.YearName;
                                item.CancelMsg = row.CancelMsg;
                                item.ReceiveOrgOid = row.ReceiveOrgOid.Oid.ToString();
                                item.ReceiveOrgName = row.ReceiveOrgOid.SubOrganizeName;
                                item.ReceiveOrgFullName = row.ReceiveOrgOid.OrganizeNameTH;
                                item.RefNo = row.SendNo + "|" + row.ReceiveOrgOid.Oid.ToString() + "|1";
                                item.TotalWeight = row.SendOrderSeedDetails.Sum((c => c.Weight)).ToString() + " กิโลกรัม";
                                ReceiveItems.Add(item);
                            }
                            directProvider.Dispose();
                            ObjectSpace.Dispose();
                            //lists.Receive = ReceiveItems;
                            return Request.CreateResponse(HttpStatusCode.OK, ReceiveItems);

                        }
                    }

                    //invalid
                    UserError err = new UserError();
                    err.status = "false";
                    err.code = "0";
                    err.message = "กรุณาใส่ข้อมูล Org_Oid และ type ให้เรียบร้อยก่อน";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);

                }
                else
                {
                    UserError err = new UserError();
                    err.status = "false";
                    err.code = "0";
                    err.message = "กรุณาใส่ข้อมูล Org_Oid ให้เรียบร้อยก่อน";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.status = "false";
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = "ไม่พบข้อมูล";
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            finally
            {
                SqlConnection.ClearAllPools();
            }
        }

        /// <summary>
        /// แสดงรายละเอียดข้อมูลส่ง-รับเมล็ดพันธุ์
        /// </summary>
        /// <param name="RefNo">เลขที่อ้างอิง|Oid หน่วยงาน|ประเภท</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SendOrderSeed/Detail")]
        public HttpResponseMessage GetSendOrderSeedDetail()
        {
            try
            {
                string RefNo = HttpContext.Current.Request.Form["RefNo"].ToString();

                if (RefNo != "")
                {
                    string[] arr = RefNo.Split('|');
                    string _refno = arr[0]; //เลขที่อ้างอิง
                    string _org_oid = arr[1]; //oid หน่วยงาน
                    string _type = arr[2]; //ประเภทส่ง(2)-รับ(1)

                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SendOrderSeed));
                    SendOrderSeedType item = null;
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                    IList<SendOrderSeed> collection = ObjectSpace.GetObjects<SendOrderSeed>(CriteriaOperator.Parse("GCRecord is null and SendNo='" + _refno + "'", null));

                    foreach (SendOrderSeed row in collection)
                    {
                        item = new SendOrderSeedType();
                        item.SendNo = row.SendNo;
                        item.SendDate = row.SendDate.ToString("dd/MM/yyyy");
                        item.SendOrgOid = row.SendOrgOid.Oid.ToString();
                        item.SendOrgName = row.SendOrgOid.SubOrganizeName;
                        item.SendOrgFullName = row.SendOrgOid.OrganizeNameTH;
                        item.Remark = row.Remark;
                        item.SendStatus = row.SendStatus.ToString();
                        item.FinanceYear = row.FinanceYearOid.YearName;
                        item.CancelMsg = row.CancelMsg;
                        item. ReceiveOrgOid = row.ReceiveOrgOid.Oid.ToString();
                        item.ReceiveOrgName = row.ReceiveOrgOid.SubOrganizeName;
                        item.ReceiveOrgFullName = row.ReceiveOrgOid.OrganizeNameTH;
                        item.RefNo = RefNo;
                        item.TotalWeight = row.SendOrderSeedDetails.Sum((c => c.Weight)).ToString();

                        List<SendOrderSeedDetailType> details = new List<SendOrderSeedDetailType>();
                        SendOrderSeedDetailType _dt = null;

                        foreach (SendOrderSeedDetail rw in row.SendOrderSeedDetails)
                        {
                            _dt = new SendOrderSeedDetailType();

                            _dt.LotNumber = rw.LotNumber.LotNumber;
                            _dt.Amount = rw.Amount;
                            _dt.Weight = rw.Weight;
                            _dt.WeightUnitOid = rw.WeightUnitOid.Oid.ToString();
                            _dt.WeightUnitName = rw.WeightUnitOid.UnitName;
                            _dt.Used = rw.Used;
                            _dt.SeedTypeOid = rw.SeedTypeOid.Oid.ToString();
                            _dt.SeedTypeName = rw.SeedTypeOid.SeedTypeName;
                            _dt.BudgetSourceOid = rw.BudgetSourceOid.Oid.ToString();
                            _dt.BudgetSourceName = rw.BudgetSourceOid.BudgetName;
                            _dt.AnimalSeeName = rw.AnimalSeeName;
                            _dt.AnimalSeedOid = rw.AnimalSeedOid.Oid.ToString();
                            _dt.AnimalSeedName = rw.AnimalSeedOid.SeedName;
                            _dt.AnimalSeedLevelOid = rw.AnimalSeedLevelOid.Oid.ToString();
                            _dt.AnimalSeedLevel = rw.AnimalSeedLevel;
                            _dt.AnimalSeedLevelName = rw.AnimalSeedLevelOid.SeedLevelName;
                            _dt.AnimalSeedCode = rw.AnimalSeedCode;
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
                SqlConnection.ClearAllPools();
            }
        }

        /// <summary>
        /// ปรับปรุงข้อมูลส่ง-รับเมล็ดพันธุ์
        /// </summary>
        /// <param name="RefNo">เลขที่อ้างอิง|Oid หน่วยงาน|ประเภท</param>
        /// <param name="Status">สถานะการตรวจสอบ (1=อนุมัติ/2=ไม่อนุมัติ)</param>
        /// <param name="CancelMsg">หมายเหตุกรณีไม่อนุมัติ</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SendOrderSeed/Update")]
        public HttpResponseMessage UpdateSendOrderSeed()
        {
            QualityAnalysis objQualityAnalysis = null;
            //  object objStockAnimalInfo = null;
            HistoryWork ObjHistory = null;
            IList<StockSeedInfo> objStockSeedInfo = null;
            string Username = " ";

            try
            {
                string RefNo = HttpContext.Current.Request.Form["RefNo"].ToString(); //ข้อมูลเลขที่อ้างอิง
                string Status = HttpContext.Current.Request.Form["Status"].ToString(); //สถานะ
                string CancelMsg = HttpContext.Current.Request.Form["CancelMsg"].ToString(); //หมายเหตุ
                Username = HttpContext.Current.Request.Form["Username"].ToString();

                if (RefNo != "" && Status != "")
                {
                    string[] arr = RefNo.Split('|');
                    string _refno = arr[0]; //เลขที่อ้างอิง
                    string _org_oid = arr[1]; //oid หน่วยงาน
                    string _type = arr[2]; //ประเภทส่ง(2)-รับ(1)

                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SendOrderSeed));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.StockSeedInfo));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.QualityAnalysis));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.HistoryWork));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierProductModifyDetail));
                    XafTypesInfo.Instance.RegisterEntity(typeof(nutrition.Module.SupplierProductModify));
                    XafTypesInfo.Instance.RegisterEntity(typeof(UserInfo));
                    XafTypesInfo.Instance.RegisterEntity(typeof(ReceiveLotNumber));

                    List<SendOrderSeed> list = new List<SendOrderSeed>();
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                    UserInfo objUserInfo = ObjectSpace.FindObject<UserInfo>(CriteriaOperator.Parse("[UserName]=?", Username));

                    SendOrderSeed ObjMaster = ObjectSpace.FindObject<SendOrderSeed>(CriteriaOperator.Parse("SendNo=?", _refno));

                    if (_type == "1") //รับ
                    {
                        if (Status == "1")
                        { //Approve
                            foreach (SendOrderSeedDetail row in ObjMaster.SendOrderSeedDetails)
                            {
                                SupplierProductModifyDetail objSupplierProduct = ObjectSpace.FindObject<SupplierProductModifyDetail>(CriteriaOperator.Parse("Oid=?", row.LotNumber.Oid));
                                if (objSupplierProduct != null)
                                {
                                    objStockSeedInfo = ObjectSpace.GetObjects<StockSeedInfo>(CriteriaOperator.Parse("OrganizationOid= ? and FinanceYearOid=? and BudgetSourceOid=? and AnimalSeedOid=? and AnimalSeedLevelOid=? and StockType=1 and ReferanceCode=? ", ObjMaster.ReceiveOrgOid.Oid, ObjMaster.FinanceYearOid.Oid, objSupplierProduct.BudgetSourceOid, objSupplierProduct.AnimalSeedOid.Oid, objSupplierProduct.AnimalSeedLevelOid.Oid, row.LotNumber.LotNumberFactory));

                                    if (objStockSeedInfo.Count > 0)
                                    {
                                        var ObjSubStockCardSource = (from Item in objStockSeedInfo orderby Item.StockDate descending select Item).First().TotalWeight;
                                        var ObjStockSeedInfoInfo = ObjectSpace.CreateObject<StockSeedInfo>();
                                        ObjStockSeedInfoInfo.StockDate = DateTime.Today;
                                        ObjStockSeedInfoInfo.OrganizationOid = ObjMaster.ReceiveOrgOid;
                                        ObjStockSeedInfoInfo.FinanceYearOid = ObjMaster.FinanceYearOid;
                                        ObjStockSeedInfoInfo.BudgetSourceOid = objSupplierProduct.BudgetSourceOid;
                                        ObjStockSeedInfoInfo.AnimalSeedOid = objSupplierProduct.AnimalSeedOid;
                                        ObjStockSeedInfoInfo.AnimalSeedLevelOid = objSupplierProduct.AnimalSeedLevelOid;
                                        ObjStockSeedInfoInfo.StockDetail = "รับเมล็ดพันธุ์ Lot Number : " + row.LotNumber.LotNumberFactory + "(Mobile Application)";
                                        ObjStockSeedInfoInfo.TotalForward = ObjSubStockCardSource;
                                        ObjStockSeedInfoInfo.TotalChange = row.Weight;
                                        ObjStockSeedInfoInfo.StockType = EnumStockType.ReceiveProduct;
                                        ObjStockSeedInfoInfo.SeedTypeOid = objSupplierProduct.SeedTypeOid;
                                        ObjStockSeedInfoInfo.ReferanceCode = row.LotNumber.LotNumberFactory;
                                        ObjStockSeedInfoInfo.Description = "รับเมล็ดพันธุ์จาก : " + ObjMaster.SendOrgOid.SubOrganizeName + "(Mobile Application)";
                                        ObjStockSeedInfoInfo.UseNo = ObjMaster.SendNo;
                                        ObjectSpace.CommitChanges();
                                    }
                                    else
                                    {
                                        var ObjStockSeedInfoInfo = ObjectSpace.CreateObject<StockSeedInfo>();
                                        ObjStockSeedInfoInfo.StockDate = DateTime.Now;
                                        ObjStockSeedInfoInfo.OrganizationOid = ObjMaster.ReceiveOrgOid;
                                        ObjStockSeedInfoInfo.FinanceYearOid = ObjMaster.FinanceYearOid;
                                        ObjStockSeedInfoInfo.BudgetSourceOid = objSupplierProduct.BudgetSourceOid;
                                        ObjStockSeedInfoInfo.AnimalSeedOid = objSupplierProduct.AnimalSeedOid;
                                        ObjStockSeedInfoInfo.AnimalSeedLevelOid = objSupplierProduct.AnimalSeedLevelOid;
                                        ObjStockSeedInfoInfo.StockDetail = "รับเมล็ดพันธุ์ Lot Number : " + row.LotNumber.LotNumberFactory + "(Mobile Application)";
                                        ObjStockSeedInfoInfo.TotalForward = 0;
                                        ObjStockSeedInfoInfo.TotalChange = row.Weight;
                                        ObjStockSeedInfoInfo.StockType = EnumStockType.ReceiveProduct;
                                        ObjStockSeedInfoInfo.SeedTypeOid = objSupplierProduct.SeedTypeOid;
                                        ObjStockSeedInfoInfo.ReferanceCode = row.LotNumber.LotNumberFactory;
                                        ObjStockSeedInfoInfo.Description = "รับเมล็ดพันธุ์จาก : " + ObjMaster.SendOrgOid.SubOrganizeName + "(Mobile Application)";
                                        ObjStockSeedInfoInfo.UseNo = ObjMaster.SendNo;
                                        ObjectSpace.CommitChanges();
                                    }
                                    SupplierProductModifyDetail objAddSupplierProductModifyDetail = ObjectSpace.FindObject<SupplierProductModifyDetail>(CriteriaOperator.Parse("LotNumberFactory=? and OrganizationOid=?", objSupplierProduct.LotNumberFactory, ObjMaster.ReceiveOrgOid.Oid));
                                    if (objAddSupplierProductModifyDetail != null)
                                    {
                                        objAddSupplierProductModifyDetail = ObjectSpace.CreateObject<SupplierProductModifyDetail>();
                                        objAddSupplierProductModifyDetail.LotNumber = objSupplierProduct.LotNumber;
                                        objAddSupplierProductModifyDetail.LotNumberFactory = objSupplierProduct.LotNumberFactory;
                                        objAddSupplierProductModifyDetail.FactoryName = objSupplierProduct.FactoryName;
                                        objAddSupplierProductModifyDetail.WeightSend = objSupplierProduct.WeightSend;
                                        objAddSupplierProductModifyDetail.SendUnitOid = objSupplierProduct.SendUnitOid;
                                        objAddSupplierProductModifyDetail.WeightBefore = objSupplierProduct.WeightBefore;
                                        objAddSupplierProductModifyDetail.BeforeUnitOid = objSupplierProduct.BeforeUnitOid;
                                        objAddSupplierProductModifyDetail.WeightAfter = objSupplierProduct.WeightAfter;
                                        objAddSupplierProductModifyDetail.AfterUnitOid = objSupplierProduct.AfterUnitOid;
                                        objAddSupplierProductModifyDetail.AnimalSeedOid = objSupplierProduct.AnimalSeedOid;
                                        objAddSupplierProductModifyDetail.AnimalSeedLevelOid = objSupplierProduct.AnimalSeedLevelOid;
                                        objAddSupplierProductModifyDetail.BudgetSourceOid = objSupplierProduct.BudgetSourceOid;
                                        objAddSupplierProductModifyDetail.Moisture = objSupplierProduct.Moisture;
                                        objAddSupplierProductModifyDetail.Purity = objSupplierProduct.Purity;
                                        objAddSupplierProductModifyDetail.OtherSeed = objSupplierProduct.OtherSeed;
                                        objAddSupplierProductModifyDetail.Weight = objSupplierProduct.Weight;
                                        objAddSupplierProductModifyDetail.UnitOid = objSupplierProduct.UnitOid;
                                        objAddSupplierProductModifyDetail.Germination = objSupplierProduct.Germination;
                                        objAddSupplierProductModifyDetail.SeedTypeOid = objSupplierProduct.SeedTypeOid;
                                        objAddSupplierProductModifyDetail.OrganizationOid = ObjMaster.ReceiveOrgOid;
                                    }
                                }
                                Organization objOrganizationOid = ObjectSpace.FindObject<Organization>(CriteriaOperator.Parse("Oid=?", objUserInfo.Organization.Oid.ToString()));
                                ReceiveLotNumber objReceiveLotNumber = ObjectSpace.FindObject<ReceiveLotNumber>(CriteriaOperator.Parse("OrganizationOid=? and LotNumber=? and [IsActive]=1", objOrganizationOid.Oid, row.LotNumber.LotNumberFactory));
                                if (objReceiveLotNumber != null)
                                {
                                    objReceiveLotNumber = ObjectSpace.CreateObject<ReceiveLotNumber>();
                                    SupplierProductModifyDetail objModifyDetail = ObjectSpace.FindObject<SupplierProductModifyDetail>(CriteriaOperator.Parse("LotNumberFactory=?", row.LotNumber.LotNumberFactory));
                                    objReceiveLotNumber.AnimalSeedOid = objModifyDetail.AnimalSeedOid;
                                    objReceiveLotNumber.AnimalSeedLevelOid = objModifyDetail.AnimalSeedLevelOid;
                                    objReceiveLotNumber.BudgetSourceOid = ObjectSpace.GetObject<BudgetSource>(row.BudgetSourceOid);
                                    objReceiveLotNumber.OrganizationOid = objOrganizationOid;
                                    objReceiveLotNumber.LotNumber = row.LotNumber.LotNumberFactory;
                                    objReceiveLotNumber.SeedTypeOid = objSupplierProduct.SeedTypeOid;
                                    objReceiveLotNumber.IsActive = true;
                                }
                            }
                            ObjHistory = ObjectSpace.CreateObject<HistoryWork>();
                            // ประวัติ
                            ObjHistory.RefOid = ObjMaster.Oid.ToString();
                            ObjHistory.FormName = "เมล็ดพันธุ์";
                            ObjHistory.Message = "อนุมัติ (รับเมล็ดพันธุ์จากหน่วยงานในสังกัด (Mobile Application)) เลขที่ส่ง : " + ObjMaster.SendNo;
                            ObjHistory.CreateBy = SecuritySystem.CurrentUserName;
                            ObjHistory.CreateDate = DateTime.Now;
                            ObjectSpace.CommitChanges();
                            ObjMaster.ReceiveStatus = EnumReceiveOrderSeedStatus.Approve;
                            ObjectSpace.CommitChanges();

                        }

                        else if (Status == "2")
                        { //Reject
                            if (ObjMaster.SendStatus == EnumSendOrderSeedStatus.Approve)
                            {
                                foreach (SendOrderSeedDetail row in ObjMaster.SendOrderSeedDetails)
                                {
                                    SupplierProductModifyDetail objSupplierProduct = ObjectSpace.FindObject<SupplierProductModifyDetail>(CriteriaOperator.Parse("Oid=?", row.LotNumber.Oid));
                                    if (objSupplierProduct != null)
                                    {
                                        objStockSeedInfo = ObjectSpace.GetObjects<StockSeedInfo>(CriteriaOperator.Parse("OrganizationOid= ? and FinanceYearOid=? and BudgetSourceOid=? and AnimalSeedOid=? and AnimalSeedLevelOid=? and StockType=1 and ReferanceCode=? ", ObjMaster.ReceiveOrgOid.Oid, ObjMaster.FinanceYearOid.Oid, objSupplierProduct.BudgetSourceOid, objSupplierProduct.AnimalSeedOid.Oid, objSupplierProduct.AnimalSeedLevelOid.Oid, row.LotNumber.LotNumberFactory));
                                        if (objStockSeedInfo.Count > 0)
                                        {
                                            var ObjStockAnimalInfo_DetailSource = (from Item in objStockSeedInfo orderby Item.StockDate descending select Item).First().TotalWeight;
                                            var ObjStockSeedInfoInfo = ObjectSpace.CreateObject<StockSeedInfo>();
                                            // ObjMaster.ReceiveOrgOid.Oid, ObjMaster.FinanceYearOid.Oid, objSupplierProduct.BudgetSourceOid, objSupplierProduct.AnimalSeedOid.Oid, objSupplierProduct.AnimalSeedLevelOid.Oid))    
                                            var withBlock = ObjStockSeedInfoInfo;
                                            withBlock.StockDate = DateTime.Now;
                                            withBlock.OrganizationOid = ObjMaster.ReceiveOrgOid;
                                            withBlock.FinanceYearOid = ObjMaster.FinanceYearOid;
                                            withBlock.BudgetSourceOid = objSupplierProduct.BudgetSourceOid;
                                            withBlock.AnimalSeedOid = objSupplierProduct.AnimalSeedOid;
                                            withBlock.AnimalSeedLevelOid = objSupplierProduct.AnimalSeedLevelOid;
                                            withBlock.StockDetail = "ไม่อนุมัติการรับเมล็ดพันธุ์จากหน่วยงานในสังกัด (Mobile Application)  Lot Number : " + row.LotNumber.LotNumberFactory;
                                            withBlock.TotalForward = ObjStockAnimalInfo_DetailSource;
                                            withBlock.TotalChange = 0 - row.Weight;
                                            withBlock.StockType = EnumStockType.ReceiveProduct;
                                            withBlock.SeedTypeOid = objSupplierProduct.SeedTypeOid;
                                            withBlock.ReferanceCode = row.LotNumber.LotNumberFactory;
                                            ObjectSpace.CommitChanges();


                                        }
                                    }
                                }

                                foreach (SendOrderSeedDetail row in ObjMaster.SendOrderSeedDetails)
                                {
                                    SupplierProductModifyDetail objSupplierProduct = ObjectSpace.FindObject<SupplierProductModifyDetail>(CriteriaOperator.Parse("Oid=?", row.LotNumber.Oid));
                                    if (objSupplierProduct != null)
                                    {
                                        objStockSeedInfo = ObjectSpace.GetObjects<StockSeedInfo>(CriteriaOperator.Parse("OrganizationOid= ? and FinanceYearOid=? and BudgetSourceOid=? and AnimalSeedOid=? and AnimalSeedLevelOid=? and StockType=0 and ReferanceCode=? ", ObjMaster.SendOrgOid.Oid, ObjMaster.FinanceYearOid.Oid, objSupplierProduct.BudgetSourceOid, objSupplierProduct.AnimalSeedOid.Oid, objSupplierProduct.AnimalSeedLevelOid.Oid, row.LotNumber.LotNumberFactory));
                                        if (objStockSeedInfo.Count > 0)
                                        {
                                            var ObjSubStockCardSource = (from Item in objStockSeedInfo orderby Item.StockDate descending select Item).First().TotalWeight;
                                            var ObjStockSeedInfoInfo = ObjectSpace.CreateObject<StockSeedInfo>();
                                            // ObjMaster.ReceiveOrgOid.Oid, ObjMaster.FinanceYearOid.Oid, objSupplierProduct.BudgetSourceOid, objSupplierProduct.AnimalSeedOid.Oid, objSupplierProduct.AnimalSeedLevelOid.Oid))

                                            var withBlock1 = ObjStockSeedInfoInfo;
                                            withBlock1.StockDate = DateTime.Now;
                                            withBlock1.OrganizationOid = ObjMaster.SendOrgOid;
                                            withBlock1.FinanceYearOid = ObjMaster.FinanceYearOid;
                                            withBlock1.BudgetSourceOid = objSupplierProduct.BudgetSourceOid;
                                            withBlock1.AnimalSeedOid = objSupplierProduct.AnimalSeedOid;
                                            withBlock1.AnimalSeedLevelOid = objSupplierProduct.AnimalSeedLevelOid;
                                            withBlock1.StockDetail = "รับเมล็ดพันธุ์คืนเนื่องจากไม่ได้รับการอนุมัติ (Mobile Application) สาเหตุ : " + CancelMsg;
                                            withBlock1.TotalForward = ObjSubStockCardSource;
                                            withBlock1.TotalChange = row.Weight;
                                            withBlock1.StockType = EnumStockType.ModifyProduct;
                                            withBlock1.SeedTypeOid = objSupplierProduct.SeedTypeOid;
                                            withBlock1.ReferanceCode = row.LotNumber.LotNumberFactory;
                                            ObjectSpace.CommitChanges();
                                        }
                                    }

                                    objQualityAnalysis = ObjectSpace.FindObject<QualityAnalysis>(CriteriaOperator.Parse("[LotNumber]=? and [AnalysisType]=0 and [OrganizationOid]=?", row.LotNumber.LotNumberFactory, row.SendOrderSeed.SendOrgOid));
                                    if (objQualityAnalysis != null)
                                    {
                                        objQualityAnalysis.Weight += row.Weight;
                                    }
                                    QualityAnalysis objQualityAnalysisType1 = ObjectSpace.FindObject<QualityAnalysis>(CriteriaOperator.Parse("[LotNumber]=? and [AnalysisType]=1 and [OrganizationOid]=?", row.LotNumber.LotNumberFactory, row.SendOrderSeed.ReceiveOrgOid));
                                    if (objQualityAnalysisType1 != null)
                                    {
                                        objQualityAnalysisType1.Weight -= row.Weight;
                                    }
                                }
                            }
                            ObjectSpace.CommitChanges();
                            ObjHistory = ObjectSpace.CreateObject<HistoryWork>();
                            // ประวัติ
                            ObjHistory.RefOid = ObjMaster.Oid.ToString();
                            ObjHistory.FormName = "เมล็ดพันธุ์";
                            ObjHistory.Message = "ไม่อนุมัติ (รับเมล็ดพันธุ์จากหน่วยงานในสังกัด (Mobile Application)) เลขที่ส่ง : " + ObjMaster.SendNo;
                            ObjHistory.CreateBy = Username;
                            ObjHistory.CreateDate = DateTime.Now;
                            ObjectSpace.CommitChanges();
                            ObjMaster.SendStatus = EnumSendOrderSeedStatus.Eject; //4
                            ObjMaster.ReceiveStatus = EnumReceiveOrderSeedStatus.Eject;//4
                            ObjMaster.CancelMsg = CancelMsg;
                            ObjectSpace.CommitChanges();

                        }
                    }





                    else if (_type == "2") //ส่ง
                    {
                        if (Status == "1")
                        { //Approve
                            foreach (SendOrderSeedDetail row in ObjMaster.SendOrderSeedDetails)
                            {
                                SupplierProductModifyDetail objSupplierProduct = ObjectSpace.FindObject<SupplierProductModifyDetail>(CriteriaOperator.Parse("Oid=?", row.LotNumber.Oid));
                                if (objSupplierProduct != null)
                                {

                                    if (ObjMaster.SendOrgOid.IsFactory == true)
                                    {
                                        objStockSeedInfo = ObjectSpace.GetObjects<StockSeedInfo>(CriteriaOperator.Parse("OrganizationOid= ? and FinanceYearOid=? and BudgetSourceOid=? and AnimalSeedOid=? and AnimalSeedLevelOid=? and StockType=0 and ReferanceCode=? ", ObjMaster.SendOrgOid.Oid, ObjMaster.FinanceYearOid.Oid, objSupplierProduct.BudgetSourceOid, objSupplierProduct.AnimalSeedOid.Oid, objSupplierProduct.AnimalSeedLevelOid.Oid, row.LotNumber.LotNumberFactory));
                                    }
                                    else
                                    {
                                        objStockSeedInfo = ObjectSpace.GetObjects<StockSeedInfo>(CriteriaOperator.Parse("OrganizationOid= ? and FinanceYearOid=? and BudgetSourceOid=? and AnimalSeedOid=? and AnimalSeedLevelOid=? and StockType=1 and ReferanceCode=? ", ObjMaster.SendOrgOid.Oid, ObjMaster.FinanceYearOid.Oid, objSupplierProduct.BudgetSourceOid, objSupplierProduct.AnimalSeedOid.Oid, objSupplierProduct.AnimalSeedLevelOid.Oid, row.LotNumber.LotNumberFactory));
                                    }

                                    if (objStockSeedInfo.Count > 0)
                                    {
                                        var ObjSubStockCardSource = (from Item in objStockSeedInfo orderby Item.StockDate descending select Item).First().TotalWeight;
                                        var ObjStockSeedInfoInfo = ObjectSpace.CreateObject<StockSeedInfo>();
                                        // ObjMaster.ReceiveOrgOid.Oid, ObjMaster.FinanceYearOid.Oid, objSupplierProduct.BudgetSourceOid, objSupplierProduct.AnimalSeedOid.Oid, objSupplierProduct.AnimalSeedLevelOid.Oid))

                                        var withBlock = ObjStockSeedInfoInfo;
                                        withBlock.StockDate = DateTime.Now;
                                        withBlock.OrganizationOid = ObjMaster.SendOrgOid;
                                        withBlock.FinanceYearOid = ObjMaster.FinanceYearOid;
                                        withBlock.BudgetSourceOid = objSupplierProduct.BudgetSourceOid;
                                        withBlock.AnimalSeedOid = objSupplierProduct.AnimalSeedOid;
                                        withBlock.AnimalSeedLevelOid = objSupplierProduct.AnimalSeedLevelOid;
                                        withBlock.StockDetail = "ส่งเมล็ดพันธุ์ Lot Number (Mobile Application): " + row.LotNumber.LotNumberFactory;
                                        withBlock.TotalForward = ObjSubStockCardSource;
                                        withBlock.TotalChange = 0 - row.Weight;
                                        if (ObjMaster.SendOrgOid.IsFactory == true)
                                        {
                                            withBlock.StockType = EnumStockType.ModifyProduct;
                                        }
                                        else
                                        {
                                            withBlock.StockType = EnumStockType.ReceiveProduct;
                                        }


                                        withBlock.SeedTypeOid = objSupplierProduct.SeedTypeOid;
                                        withBlock.ReferanceCode = row.LotNumber.LotNumberFactory;
                                        withBlock.Description = "ส่งเมล็ดพันธุ์ให้ : " + "" + ObjMaster.ReceiveOrgOid.SubOrganizeName + "" + "(Mobile Application)";
                                        withBlock.UseNo = ObjMaster.SendNo;
                                        ObjectSpace.CommitChanges();
                                    }
                                }
                                else
                                {
                                    var ObjStockSeedInfoInfo = ObjectSpace.CreateObject<StockSeedInfo>();
                                    var withBlock = ObjStockSeedInfoInfo;
                                    withBlock.StockDate = DateTime.Now;
                                    withBlock.OrganizationOid = ObjMaster.SendOrgOid;
                                    withBlock.FinanceYearOid = ObjMaster.FinanceYearOid;
                                    withBlock.BudgetSourceOid = objSupplierProduct.BudgetSourceOid;
                                    withBlock.AnimalSeedOid = objSupplierProduct.AnimalSeedOid;
                                    withBlock.AnimalSeedLevelOid = objSupplierProduct.AnimalSeedLevelOid;
                                    withBlock.StockDetail = "ส่งเมล็ดพันธุ์ Lot Number : " + "" + row.LotNumber.LotNumberFactory + "" + "(Mobile Application)";
                                    withBlock.TotalForward = 0;
                                    withBlock.TotalChange = 0 - row.Weight;
                                    if (ObjMaster.SendOrgOid.IsFactory == true)
                                    {
                                        withBlock.StockType = EnumStockType.ModifyProduct;
                                    }
                                    else
                                    {
                                        withBlock.StockType = EnumStockType.ReceiveProduct;
                                    }
                                    withBlock.SeedTypeOid = objSupplierProduct.SeedTypeOid;
                                    withBlock.ReferanceCode = row.LotNumber.LotNumberFactory;
                                    withBlock.Description = "ส่งเมล็ดพันธุ์ให้  : " + "" + ObjMaster.ReceiveOrgOid.SubOrganizeName + "" + "(Mobile Application)";
                                    withBlock.UseNo = ObjMaster.SendNo;
                                    ObjectSpace.CommitChanges();
                                }
                                objQualityAnalysis = ObjectSpace.FindObject<QualityAnalysis>(CriteriaOperator.Parse("[LotNumber]=? and [AnalysisType]=0 and [OrganizationOid]=?", row.LotNumber.LotNumberFactory, row.SendOrderSeed.SendOrgOid));
                                if (objQualityAnalysis != null)
                                {
                                    objQualityAnalysis.Weight -= row.Weight;
                                }
                                QualityAnalysis objQualityAnalysisType1 = ObjectSpace.FindObject<QualityAnalysis>(CriteriaOperator.Parse("[LotNumber]=? and [AnalysisType]=1 and [OrganizationOid]=?", row.LotNumber.LotNumberFactory, row.SendOrderSeed.ReceiveOrgOid));
                                if (objQualityAnalysisType1 != null)
                                {
                                    objQualityAnalysisType1.Weight += row.Weight;
                                }
                                else
                                {
                                    var ObjQualityAnalysisInfo = ObjectSpace.CreateObject<QualityAnalysis>();
                                    SupplierProductModifyDetail objSupplierProductModifyDetail = ObjectSpace.FindObject<SupplierProductModifyDetail>(CriteriaOperator.Parse("LotNumberFactory=?", row.LotNumber.LotNumberFactory));

                                    var withBlock = ObjQualityAnalysisInfo;
                                    withBlock.AnimalSeedOid = objSupplierProductModifyDetail.AnimalSeedOid;
                                    withBlock.AnimalSeedLevelOid = objSupplierProductModifyDetail.AnimalSeedLevelOid;
                                    withBlock.SeedTypeOid = objSupplierProductModifyDetail.SeedTypeOid;
                                    withBlock.LotNumber = objSupplierProductModifyDetail.LotNumberFactory;
                                    withBlock.Weight = row.Weight;
                                    withBlock.Moisture = objSupplierProductModifyDetail.Moisture;
                                    withBlock.Germination = objSupplierProductModifyDetail.Germination;
                                    withBlock.AnalysisType = 1.ToString();
                                    withBlock.OrganizationOid = ObjMaster.ReceiveOrgOid;
                                    ObjectSpace.CommitChanges();
                                }

                            }
                            ObjHistory = ObjectSpace.CreateObject<HistoryWork>();
                            // ประวัติ
                            ObjHistory.RefOid = ObjMaster.Oid.ToString();
                            ObjHistory.FormName = "เมล็ดพันธุ์";
                            ObjHistory.Message = "อนุมัติ (ส่งเมล็ดพันธุ์ให้หน่วยงานในสังกัด (Mobile Application)) เลขที่ส่ง : " + ObjMaster.SendNo;
                            ObjHistory.CreateBy = Username;
                            ObjHistory.CreateDate = DateTime.Now;
                            ObjectSpace.CommitChanges();
                            ObjMaster.SendStatus = EnumSendOrderSeedStatus.Approve; //2
                            ObjMaster.ReceiveStatus = EnumReceiveOrderSeedStatus.InProgess;//1
                            ObjMaster.Remark = CancelMsg;
                            ObjectSpace.CommitChanges();
                        }

                        else if (Status == "2") //ไม่อนุมัติ ฝั่งส่ง
                        { //Reject
                            ObjMaster.CancelMsg = CancelMsg;

                            foreach (SendOrderSeedDetail row in ObjMaster.SendOrderSeedDetails)
                            {
                                SupplierProductModifyDetail objSupplierProduct = ObjectSpace.FindObject<SupplierProductModifyDetail>(CriteriaOperator.Parse("Oid=?", row.LotNumber.Oid));
                                if (objSupplierProduct != null)
                                {
                                    objStockSeedInfo = ObjectSpace.GetObjects<StockSeedInfo>(CriteriaOperator.Parse("OrganizationOid= ? and FinanceYearOid=? and BudgetSourceOid=? and AnimalSeedOid=? and AnimalSeedLevelOid=? and StockType=0 and ReferanceCode=? ", ObjMaster.SendOrgOid.Oid, ObjMaster.FinanceYearOid.Oid, objSupplierProduct.BudgetSourceOid, objSupplierProduct.AnimalSeedOid.Oid, objSupplierProduct.AnimalSeedLevelOid.Oid, row.LotNumber.LotNumberFactory));
                                    if (objStockSeedInfo.Count > 0)
                                    {
                                        var ObjStockAnimalInfo_DetailSource = (from Item in objStockSeedInfo orderby Item.StockDate descending select Item).First().TotalWeight;

                                        var ObjStockSeedInfoInfo = ObjectSpace.CreateObject<StockSeedInfo>();
                                        // ObjMaster.ReceiveOrgOid.Oid, ObjMaster.FinanceYearOid.Oid, objSupplierProduct.BudgetSourceOid, objSupplierProduct.AnimalSeedOid.Oid, objSupplierProduct.AnimalSeedLevelOid.Oid))

                                        var withBlock = ObjStockSeedInfoInfo;
                                        withBlock.StockDate = DateTime.Now;
                                        withBlock.OrganizationOid = ObjMaster.SendOrgOid;
                                        withBlock.FinanceYearOid = ObjMaster.FinanceYearOid;
                                        withBlock.BudgetSourceOid = objSupplierProduct.BudgetSourceOid;
                                        withBlock.AnimalSeedOid = objSupplierProduct.AnimalSeedOid;
                                        withBlock.AnimalSeedLevelOid = objSupplierProduct.AnimalSeedLevelOid;
                                        withBlock.StockDetail = "ไม่อนุมัติการส่งเมล็ดพันธุ์ (Mobile Application) สาเหตุ :" + CancelMsg;
                                        withBlock.TotalForward = ObjStockAnimalInfo_DetailSource;
                                        withBlock.TotalChange = row.Weight;
                                        withBlock.StockType = EnumStockType.ModifyProduct;
                                        withBlock.SeedTypeOid = objSupplierProduct.SeedTypeOid;
                                        withBlock.ReferanceCode = row.LotNumber.LotNumberFactory;
                                        ObjectSpace.CommitChanges();


                                    }
                                }

                            }

                            ObjHistory = ObjectSpace.CreateObject<HistoryWork>();
                            // ประวัติ
                            ObjHistory.RefOid = ObjMaster.Oid.ToString();
                            ObjHistory.FormName = "เมล็ดพันธุ์";
                            ObjHistory.Message = "ไม่อนุมัติ (ส่งเมล็ดพันธุ์ให้หน่วยงานในสังกัด (Mobile Application)) เลขที่ส่ง : " + ObjMaster.SendNo;
                            ObjHistory.CreateBy = Username;
                            ObjHistory.CreateDate = DateTime.Now;
                            ObjectSpace.CommitChanges();

                            ObjMaster.SendStatus = EnumSendOrderSeedStatus.Eject; //4

                            ObjectSpace.CommitChanges();

                        }
                    }




                    UpdateResult ret = new UpdateResult();
                    ret.status = "true";
                    ret.message = "บันทึกข้อมูลเสร็จเรียบร้อยแล้ว";
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
