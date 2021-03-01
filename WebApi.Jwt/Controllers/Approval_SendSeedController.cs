using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using Microsoft.ApplicationBlocks.Data;
using nutrition.Module;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.Http;
using WebApi.Jwt.Models;

namespace WebApi.Jwt.Controllers
{
    public class Approval_SendSeed : ApiController
    {
        private string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        #region แบบใช้ฟังค์ชั่นของ xaf

        /// <summary>
        /// หารายละเอียดการส่งเมล็ดด้วย sendOID
        /// </summary>
        /// <returns></returns>

        [AllowAnonymous]
        [HttpPost]
        [Route("SendOrder/{Send_No}")] // ใส่ OIDSendOrderSeed ใบนำส่ง /SendOrder/226-0011
        //[JwtAuthentication]
        public IHttpActionResult SendOrderSeedDetail_ByOrderSeedID()

        {
            object Send_No = string.Empty;
            object ReceiveOrgOid = string.Empty;
            Approve_Model sendDetail = new Approve_Model();

            SendOrderSeed_Model Model = new SendOrderSeed_Model();
            try
            {
                if (HttpContext.Current.Request.Form["Send_No"].ToString() != null)
                {
                    Send_No = HttpContext.Current.Request.Form["Send_No"].ToString();
                }
                if (HttpContext.Current.Request.Form["ReceiveOrgOid"].ToString() != null)
                {
                    ReceiveOrgOid = HttpContext.Current.Request.Form["ReceiveOrgOid"].ToString();
                }
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SendOrderSeed));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc);
                List<Approve_Model> list = new List<Approve_Model>();
                List<SendOrderSeed_Model> list_detail = new List<SendOrderSeed_Model>();
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                SendOrderSeed sendOrderSeed;
                sendOrderSeed = ObjectSpace.FindObject<SendOrderSeed>(CriteriaOperator.Parse("GCRecord is null and SendStatus = 2 and SendNo=? and ReceiveOrgOid=? ", Send_No, ReceiveOrgOid));
                //sendOrderSeed = ObjectSpace.GetObject<SendOrderSeed>(CriteriaOperator.Parse("GCRecord is null and SendStatus = 2 and ReceiveOrgOid=? ", null));
                if (Send_No != null)
                {
                    double sum = 0;
                    sendDetail.Send_No = sendOrderSeed.SendNo;
                    sendDetail.SendDate = Convert.ToDateTime(sendOrderSeed.SendDate).ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                    sendDetail.SendOrgName = sendOrderSeed.SendOrgOid.SubOrganizeName;
                    sendDetail.ReceiveOrgName = sendOrderSeed.ReceiveOrgOid.SubOrganizeName;
                    sendDetail.Remark = sendOrderSeed.Remark;
                    sendDetail.SendStatus = sendOrderSeed.SendStatus.ToString();
                    sendDetail.FinanceYear = sendOrderSeed.FinanceYearOid.YearName;

                    //if (sendOrderSeed.CancelMsg == null)
                    //{
                    //    sendDetail.CancelMsg = "ไม่";
                    //}
                    //else
                    //{
                    //    sendDetail.CancelMsg = sendOrderSeed.CancelMsg;
                    //}

                    foreach (SendOrderSeedDetail row in sendOrderSeed.SendOrderSeedDetails)
                    {
                        SendOrderSeed_Model send_Detail = new SendOrderSeed_Model();
                        send_Detail.LotNumber = row.LotNumber.LotNumber;
                        send_Detail.WeightUnit = row.WeightUnitOid.UnitName;
                        send_Detail.AnimalSeedCode = row.AnimalSeedCode;
                        send_Detail.AnimalSeedLevel = row.AnimalSeedLevel;
                        send_Detail.AnimalSeedName = row.AnimalSeeName;
                        send_Detail.BudgetSource = row.BudgetSourceOid.BudgetName;
                        send_Detail.Weight = row.Weight.ToString();
                        send_Detail.Used = row.Used.ToString();
                        if (row.SendOrderSeed != null)
                        {
                            send_Detail.SendOrderSeed = row.SendOrderSeed.SendNo;
                        }

                        send_Detail.AnimalSeedOid = row.AnimalSeedOid.SeedName;
                        send_Detail.AnimalSeedLevelOid = row.AnimalSeedLevelOid.SeedLevelName;
                        send_Detail.SeedTypeOid = row.SeedTypeOid.SeedTypeName;
                        send_Detail.Amount = row.Amount;
                        sum = sum + row.Weight;

                        list_detail.Add(send_Detail);
                    }
                    sendDetail.Weight_All = sum.ToString() + " " + "กิโลกรัม";
                    sendDetail.objSeed = list_detail;

                    return Ok(sendDetail);
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

        #endregion แบบใช้ฟังค์ชั่นของ xaf

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
                SendOrderSeed ObjMaster;
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
                    var objSupplierProduct = ObjectSpace.FindObject<SupplierProductModifyDetail>(CriteriaOperator.Parse("Oid =?", objsend_Detail.LotNumber));

                    var objStockSeedInfo = ObjectSpace.GetObjects<StockSeedInfo>(CriteriaOperator.Parse("OrganizationOid= ? and FinanceYearOid=? and BudgetSourceOid=? and AnimalSeedOid=? and AnimalSeedLevelOid=? ", ObjMaster.SendOrgOid.Oid, ObjMaster.FinanceYearOid, objsend_Detail.BudgetSourceOid, objsend_Detail.AnimalSeedOid
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
                        ObjStockSeedInfoInfo.TotalForward = (double)objSupplierProduct.Weight;
                        ObjStockSeedInfoInfo.TotalChange = 0 - Convert.ToDouble(objsend_Detail.Weight);
                        ObjStockSeedInfoInfo.StockType = 0;
                        ObjStockSeedInfoInfo.SeedTypeOid = objSupplierProduct.SeedTypeOid;
                        ObjStockSeedInfoInfo.ReferanceCode = objSupplierProduct.LotNumber;
                        ObjectSpace.CommitChanges();
                    }
                    ObjMaster.SendStatus = EnumSendOrderSeedStatus.Approve;
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

        #endregion SendOrderSeedApprove ยืนยันเมล็ดพันธุ์
    }
}