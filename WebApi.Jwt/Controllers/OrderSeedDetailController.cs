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
using nutrition.Module.EmployeeAsUserExample.Module.BusinessObjects;
using nutrition.Module;
using DevExpress.Xpo;
using System.Globalization;
using static WebApi.Jwt.Models.Supplier;

namespace WebApi.Jwt.Controllers
{
    public class OrderSeedDetailController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
        /// <summary>
        /// หน้ารายละเอียดการส่งแบบระบุเลขที่นำส่ง
        /// </summary>
        /// <param name="SendOrderSeed"></param>
        /// <returns></returns>
        #region เมล็ดพันธุ์
        [AllowAnonymous]
        [HttpPost]
        [Route("SendSeed/Order")]
        public IHttpActionResult OrderseedDetail(string SendOrderSeed)
        {
            OrderSeedDetail OrderSeedDetail = new OrderSeedDetail();
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSeedDetail));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                SendOrderSeedDetail OrderSeed;

                OrderSeed = ObjectSpace.FindObject<SendOrderSeedDetail>(new BinaryOperator("SendOrderSeed", SendOrderSeed));
                if (SendOrderSeed != null)
                {
                    OrderSeedDetail.LotNumber = OrderSeed.LotNumber.LotNumber;
                    OrderSeedDetail.WeightUnit = OrderSeed.WeightUnitOid.UnitName;
                    OrderSeedDetail.AnimalSeedCode = OrderSeed.AnimalSeedCode;
                    OrderSeedDetail.AnimalSeeName = OrderSeed.AnimalSeeName;
                    OrderSeedDetail.AnimalSeedLevel = OrderSeed.AnimalSeedLevel;
                    OrderSeedDetail.BudgetSource = OrderSeed.BudgetSourceOid.BudgetName;
                    OrderSeedDetail.Weight = OrderSeed.Weight;
                    OrderSeedDetail.Used = OrderSeed.Used;
                    OrderSeedDetail.SendOrderSeed = OrderSeed.SendOrderSeed.SendNo;
                    return Ok(OrderSeedDetail);
                }
                {
                    return BadRequest("NoData");
                }

            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// โหลดหน้าสถานะ  2 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        //[JwtAuthentication]
        [HttpPost]
        [Route("SendOrderSeed/accept")] /*ส่งเมล็ดให้หน่วยงานอื่น*/
        public HttpResponseMessage LoadSendSeed()
        {
            object SendOrgOid;

            try
            {

                SendOrgOid = HttpContext.Current.Request.Form["SendOrgOid"].ToString();


                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSeed));

                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                List<SendOrderSeed_Model> list_detail = new List<SendOrderSeed_Model>();

                List<sendSeed_info> list = new List<sendSeed_info>();
                data_info Temp_data = new data_info();
                IList<SendOrderSeed> collection = ObjectSpace.GetObjects<SendOrderSeed>(CriteriaOperator.Parse(" GCRecord is null and SendStatus = 5 and SendOrgOid=? ", SendOrgOid));

                double Amount = 0;
                if (collection.Count > 0)
                {

                    foreach (SendOrderSeed row in collection)
                    {
                        double sum = 0;
                        string WeightUnit;
                        sendSeed_info Approve = new sendSeed_info();
                        Approve.Send_No = row.SendNo;
                        Approve.SendDate = row.SendDate.ToString("dd-MM-yyyy", new CultureInfo("us-US")); /* convet เวลา*/
                        Approve.FinanceYear = row.FinanceYearOid.YearName;
                        Approve.SendOrgOid = row.SendOrgOid.Oid;
                        Approve.SendOrgName = row.SendOrgOid.SubOrganizeName;
                        Approve.ReceiveOrgoid = row.ReceiveOrgOid.Oid;
                        Approve.ReceiveOrgName = row.ReceiveOrgOid.SubOrganizeName;

                        foreach (SendOrderSeedDetail row2 in row.SendOrderSeedDetails)
                        {
                            sum = sum + row2.Weight;
                            WeightUnit = row2.WeightUnitOid.ToString();
                        }
                        Approve.Weight_All = sum.ToString() + " " + "กิโลกรัม";


                        list.Add(Approve);
                    }
                    Temp_data.sendSS = list;
                    return Request.CreateResponse(HttpStatusCode.OK, list);
                }

                else
                {
                    UserError err = new UserError();
                    err.code = "5"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "No data";
                    ////    Return resual
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }

            }
            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                ////  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }

        /// <summary>
        /// แสดงรายละเอียดข้อมูลการรับเมล็ด ใช้ศูนย์ส่ง where หา
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("ReceiveOrderSeed/accept")]
        public IHttpActionResult ReceiveOrderSeed()
        {
            object ReceiveOrgOid;

            try
            {

                ReceiveOrgOid = HttpContext.Current.Request.Form["ReceiveOrgOid"].ToString();
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSeed));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                List<SendOrderSeed_Model> list_detail = new List<SendOrderSeed_Model>();
                List<ReceiveOrderSeed_Model> list = new List<ReceiveOrderSeed_Model>();
                IList<SendOrderSeed> collection = ObjectSpace.GetObjects<SendOrderSeed>(CriteriaOperator.Parse("GCRecord is null and SendStatus = 2 and ReceiveOrgOid=? ", ReceiveOrgOid));
                double sum = 0;
                string WeightUnit;
                if (collection.Count > 0)
                {
                    foreach (SendOrderSeed row in collection)
                    {
                        ReceiveOrderSeed_Model Model = new ReceiveOrderSeed_Model();

                        Model.ReceiveNo = row.SendNo;
                        Model.ReceiveDate = row.SendDate.ToString("dd-MM-yyyy", new CultureInfo("us-US")); ;
                        //    FinanceYear = ObjectSpace.GetObject<nutrition.Module.FinanceYear>(CriteriaOperator.Parse(nameof"Oid = @FinanceYearOid ", null));
                        Model.FinanceYear = row.FinanceYearOid.YearName;
                        Model.ReceiveOrgOid = row.ReceiveOrgOid.Oid;
                        Model.ReceiveOrgName = row.ReceiveOrgOid.SubOrganizeName;
                        Model.SendOrgOid = row.SendOrgOid.Oid;
                        Model.SendOrgName = row.SendOrgOid.SubOrganizeName;
                        foreach (SendOrderSeedDetail row2 in row.SendOrderSeedDetails)
                        {
                            sum = sum + row2.Weight;
                            WeightUnit = row2.WeightUnitOid.ToString();
                        }
                        Model.Weight_All = sum.ToString() + " " + "กิโลกรัม";
                        list.Add(Model);

                    }
                    return Ok(list);
                }

                else
                {
                    return BadRequest("NoData");
                }
            }

            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                return BadRequest(ex.Message);
            }
        }



        #endregion
        #region ใช้เมล็ดพันธุ์
        /// <summary>
        /// เรียกหน้าใช้เสบียงสัตว์
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("LoadSupplierUseProduct")]
        public IHttpActionResult LoadSupplierUse_accept()
        {
            object OrganizationOid;
            try
            {

                OrganizationOid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();


                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseProduct));
                SupplierUseProduct supplier_UseProduct;
                List<SupplierProductUser> list_detail = new List<SupplierProductUser>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<SupplierUseProduct> collection = ObjectSpace.GetObjects<SupplierUseProduct>(CriteriaOperator.Parse(" GCRecord is null and Stauts = 1 and OrganizationOid=?", OrganizationOid));
                double Weight = 0;
                if (collection != null)
                {
                    foreach (SupplierUseProduct row in collection)
                    {

                        SupplierProductUser Supplier = new SupplierProductUser();
                        Supplier.OrgeService = row.UseDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                        Supplier.OrgeServiceName = row.UseNo.ToString();
                        Supplier.FinanceYear = row.FinanceYearOid.YearName;
                        Supplier.OrganizationName = row.OrganizationOid.SubOrganizeName;
                        if (row.EmployeeOid == null)
                        {
                            Supplier.EmployeeName = "ไม่มีรายชื่อผู้ขอรับบริการ";
                        }
                        else
                        {
                            Supplier.EmployeeName = row.EmployeeOid.EmployeeFirstName + " " + row.EmployeeOid.EmployeeLastName;
                        }

                        Supplier.Remark = row.Remark;
                        //Supplier.Stauts = row.Stauts;
                        Supplier.ApproveDate = row.ApproveDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                        Supplier.ActivityName = row.ActivityOid.ActivityName;

                        if (row.SubActivityOid.ActivityName == null)
                        {
                            Supplier.SubActivityName = "ไม่มีข้อมูลกิจกรรม";
                        }
                        else
                        {
                            Supplier.SubActivityName = row.SubActivityOid.ActivityName;
                        }
                        foreach (SupplierUseProductDetail row2 in row.SupplierUseProductDetails)
                        {
                            Weight = row2.Weight;
                        }
                        Supplier.Weight = Weight + " " + "กิโลกรัม";
                        Supplier.ServiceCount = row.ServiceCount;

                        //if (row.RegisCusServiceOid.DisPlayName == "")
                        //{
                        //    Supplier.RegisCusServiceOid = "ไม่มีข้อมูล";
                        //}
                        //else
                        //{
                        //    Supplier.RegisCusServiceOid = row.RegisCusServiceOid.DisPlayName;
                        //}
                        //if (row.OrgeServiceOid.OrgeServiceName == "")
                        //{
                        //    Supplier.OrgeServiceOid = "ไม่มีข้อมูล";
                        //}
                        //else
                        //{
                        //    Supplier.OrgeServiceOid = row.OrgeServiceOid.OrgeServiceName;
                        //}
                        list_detail.Add(Supplier);
                    }

                    return Ok(list_detail);
                }

                else if (list_detail == null)
                {
                    UserError err = new UserError();
                    err.code = "3"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "No data";
                    //  Return resual
                    return BadRequest(err.message);
                }
                else
                {
                    UserError err = new UserError();
                    err.code = "5"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "No data";
                    //  Return resual
                    return BadRequest("NoData");
                }
            }
            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                return BadRequest(ex.Message);
            }
        }

        #endregion ใช้เมล็ดพันธุ์


        #region SendOrderSeedApprove ยืนยันเมล็ดพันธุ์
        [AllowAnonymous]
        [HttpPost]
        [Route("SendSeed/ApprovalSend2")]

        public IHttpActionResult ApprovalSend_SupplierUseProduct(string Send_No) 
        {

         


            SendOrderSeed_Model Model = new SendOrderSeed_Model();
            try
            {
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
              sendSeed_info sendDetail = new sendSeed_info(); 
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSeed));
                SendOrderSeed ObjMaster ;
                ObjMaster = ObjectSpace.FindObject<SendOrderSeed>(CriteriaOperator.Parse("SendNo=?", Send_No));
                List<SendOrderSeed_Model> list_detail = new List<SendOrderSeed_Model>();
            
             ;
                //SendOrderSeed sendOrderSeed;
                //sendOrderSeed = ObjectSpace.FindObject<SendOrderSeed>(CriteriaOperator.Parse("SendNo=?", Send_No));

                DataSet ds = SqlHelper.ExecuteDataset(scc, CommandType.Text, "select SendNo from SendOrderSeed where SendNo = '" + Send_No + "'");
                if (ds.Tables[0].Rows.Count != 0)          
                {
                    double sum = 0;
                    //sendDetail.Oid = sendOrderSeed.Oid;
                    //sendDetail.Send_No = sendOrderSeed.SendNo;
                    //sendDetail.SendDate = Convert.ToDateTime(sendOrderSeed.SendDate).ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                    //sendDetail.SendOrgOid = sendOrderSeed.SendOrgOid.Oid;
                    //sendDetail.SendOrgName = sendOrderSeed.SendOrgOid.SubOrganizeName;
                    //sendDetail.ReceiveOrgoid = sendOrderSeed.ReceiveOrgOid.Oid;
                    //sendDetail.ReceiveOrgName = sendOrderSeed.ReceiveOrgOid.SubOrganizeName;
                    //sendDetail.FinanceYearOid = sendOrderSeed.FinanceYearOid;
                    //sendDetail.FinanceYear = sendOrderSeed.FinanceYearOid.YearName;
                  
                    SendOrderSeed_Model objsend_Detail = new SendOrderSeed_Model();
                    foreach (SendOrderSeedDetail row in ObjMaster.SendOrderSeedDetails)
                    {

                        objsend_Detail.LotNumber = row.LotNumber.Oid;

                        objsend_Detail.WeightUnit = row.WeightUnitOid.UnitName;
                        objsend_Detail.AnimalSeedLevelOid = row.AnimalSeedLevelOid;
                        objsend_Detail.BudgetSourceOid = row.BudgetSourceOid;
                        objsend_Detail.BudgetSource = row.BudgetSourceOid.BudgetName;
                        objsend_Detail.Weight = row.Weight.ToString();
                        objsend_Detail.Used = row.Used.ToString();         
                        objsend_Detail.AnimalSeedOid = row.AnimalSeedOid;
                        objsend_Detail.AnimalSeedLevelOid = row.AnimalSeedLevelOid;
                        objsend_Detail.SeedTypeOid = row.SeedTypeOid.SeedTypeName;
                        objsend_Detail.Amount = row.Amount;
                        sum = sum + row.Weight;
                        list_detail.Add(objsend_Detail);
        
                    }      
                    nutrition.Module.StockSeedInfo ObjStockSeedInfoInfo;
       var objSupplierProduct = ObjectSpace.FindObject<SupplierProductModifyDetail>(CriteriaOperator.Parse("Oid =?",objsend_Detail.LotNumber));

                    var objStockSeedInfo = ObjectSpace.GetObjects<StockSeedInfo>(CriteriaOperator.Parse("OrganizationOid= ? and FinanceYearOid=? and BudgetSourceOid=? and AnimalSeedOid=? and AnimalSeedLevelOid=? and StockType=0 ", ObjMaster.SendOrgOid.Oid, ObjMaster.FinanceYearOid ,objsend_Detail.BudgetSourceOid, objsend_Detail.AnimalSeedOid
                    , objsend_Detail.AnimalSeedLevelOid, objsend_Detail.LotNumber));
                    if (objStockSeedInfo == null)
                    {

                        //var stockSeedInfos = from Item in objStockSeedInfo
                        //                     orderby Item.StockDate descending
                        //                     select Item;
                        XafTypesInfo.Instance.RegisterEntity(typeof(StockSeedInfo));
                        ObjStockSeedInfoInfo = ObjectSpace.CreateObject<StockSeedInfo>();

                        ObjStockSeedInfoInfo.StockDate = DateTime.Now;
                        ObjStockSeedInfoInfo.OrganizationOid = ObjMaster.SendOrgOid;
                        ObjStockSeedInfoInfo.FinanceYearOid = ObjMaster.FinanceYearOid;
                        ObjStockSeedInfoInfo.BudgetSourceOid = objSupplierProduct.BudgetSourceOid;
                        ObjStockSeedInfoInfo.AnimalSeedOid = objSupplierProduct.AnimalSeedOid;
                        ObjStockSeedInfoInfo.AnimalSeedLevelOid = objSupplierProduct.AnimalSeedLevelOid;
                        ObjStockSeedInfoInfo.StockDetail = "ส่งเมล็ดพันธุ์ Lot Number: " + objSupplierProduct.LotNumber;
                        ObjStockSeedInfoInfo.TotalForward = objSupplierProduct.Weight;
                        ObjStockSeedInfoInfo.TotalChange = 0 - Convert.ToDouble(objsend_Detail.Weight);
                        ObjStockSeedInfoInfo.StockType = 0;
                        ObjStockSeedInfoInfo.SeedTypeOid = objSupplierProduct.SeedTypeOid;
                        ObjStockSeedInfoInfo.ReferanceCode = objSupplierProduct.LotNumber;
                        ObjectSpace.CommitChanges();
                    }
                         ObjMaster.SendStatus = EnumSendOrderSeedStatus.SendApprove;
                        ObjectSpace.CommitChanges();
                        return Ok(true);
                 
                }
                else
                {
                    return BadRequest(); 
                }


            }

            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                return BadRequest(ex.Message);
            }
        }
       
        #endregion
    }
}




