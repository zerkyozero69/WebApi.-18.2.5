using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using nutrition.Module;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Http;
using WebApi.Jwt.Models;
using static WebApi.Jwt.Models.Supplier;

namespace WebApi.Jwt.Controllers
{
    public class ManageAnimalSupplierController : ApiController
    {
        private string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        ///// <summary>
        ///// เรียกข้อมูลรายการเสบียงสัตว์
        ///// </summary>
        ///// <param name="OrganizationOid"></param>
        ///// <param name="FinanceYearOid"></param>
        ///// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("ManageAnimalSupplier/list")]
        public IHttpActionResult ManageAnimalSupplier()
        {
            try
            {
                string OrganizationOid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();
                string FinanceYearOid = HttpContext.Current.Request.Form["FinanceYearOid"].ToString();
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(ManageAnimalSupplier));
                List<ManageAnimalSupplier_Model> list_detail = new List<ManageAnimalSupplier_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<ManageAnimalSupplier> collection = ObjectSpace.GetObjects<ManageAnimalSupplier>(CriteriaOperator.Parse(" GCRecord is null and Status = 1 and OrganizationOid=? and FinanceYearOid = ?", OrganizationOid, FinanceYearOid));

                //ManageAnimalSupplier ObjMaster;
                //ObjMaster = ObjectSpace.FindObject<ManageAnimalSupplier>(CriteriaOperator.Parse("GCRecord is null and StockType = 1 and OrganizationOid =? and FinanceYearOid = ? ", OrganizationOid, FinanceYearOid));
                foreach (ManageAnimalSupplier row in collection)
                {
                    ManageAnimalSupplier_Model Model = new ManageAnimalSupplier_Model();
                    Model.Oid = row.Oid.ToString();
                    Model.FinanceYearOid = row.FinanceYearOid.Oid.ToString();
                    Model.FinanceYear = row.FinanceYearOid.YearName;
                    Model.OrgZoneOid = row.OrgZoneOid.Oid.ToString();
                    Model.OrgZone = row.OrgZoneOid.OrganizeNameTH;
                    Model.OrganizationOid = row.OrganizationOid.Oid.ToString();
                    Model.Organization = row.OrganizationOid.SubOrganizeName;
                    Model.AnimalSupplieOid = row.AnimalSupplieOid.Oid.ToString();
                    Model.AnimalSupplie = row.AnimalSupplieOid.AnimalSupplieName;
                    Model.ZoneQTY = row.ZoneQTY;
                    Model.CenterQTY = row.CenterQTY;
                    Model.OfficeQTY = row.OfficeQTY;
                    Model.OfficeGAPQTY = row.OfficeGAPQTY;
                    Model.OfficeBeanQTY = row.OfficeBeanQTY;
                    Model.Status = row.Status.ToString();
                    Model.SortID = row.SortID;
                    list_detail.Add(Model);
                }

                return Ok(true);
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

        //public IHttpActionResult insertManageSubAnimalSupplier()
        //{
        //    try
        //    {
        //        XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
        //        IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
        //        XpoTypesInfoHelper.GetXpoTypeInfoSource();
        //        XafTypesInfo.Instance.RegisterEntity(typeof(ManageAnimalSupplier));
        //        ManageAnimalSupplier ObjMaster;
        //        ObjMaster = ObjectSpace.FindObject<ManageAnimalSupplier>(CriteriaOperator.Parse("SendNo=?", Send_No));
        //        List<ManageAnimalSupplier_Model> list_detail = new List<ManageAnimalSupplier_Model>();

        //        PropertyCollectionSource collectionSource;
        //        ObjectSpace.CommitChanges();
        //        if (collectionSource.MasterObject != null)
        //        {
        //            ManageAnimalSupplier Owner = (ManageAnimalSupplier)collectionSource.MasterObject;
        //            Organization ObjOwnerOrganization = ObjectSpace.FindObject<Organization>(CriteriaOperator.Parse("Oid=?", Owner.OrganizationOid.MasterOrganization.Oid));
        //            DLDArea ObjDLDArea = ObjectSpace.FindObject<DLDArea>(CriteriaOperator.Parse("DLDAreaName=? ", ObjOwnerOrganization.OrganizeNameTH));
        //            if (ObjDLDArea != null)
        //            {
        //                IList<Province> ObjProvinceLists = ObjectSpace.GetObjects<Province>(CriteriaOperator.Parse("DLDZone=? and OrganizationOid=? ", ObjDLDArea.Oid, Owner.OrganizationOid.Oid));
        //                foreach (Province row in ObjProvinceLists)
        //                {
        //                    ManageSubAnimalSupplier objManageSubAnimalSupplier;
        //                    For Each item As AfterFinanceYearAnimalSupplier In Owner.AfterFinanceYearAnimalSuppliers
        //                   AnimalSupplieType ObjAnimalSupplieType = ObjectSpace.FindObject<AnimalSupplieType>(CriteriaOperator.Parse("SupplietypeName='หญ้าแห้ง'"));
        //                    Unit ObjUnitOid = ObjectSpace.FindObject<Unit>(CriteriaOperator.Parse("UnitName='กิโลกรัม'"));
        //                    XafTypesInfo.Instance.RegisterEntity(typeof(ManageSubAnimalSupplier));
        //                    objManageSubAnimalSupplier = ObjectSpace.CreateObject<ManageSubAnimalSupplier>();
        //                    objManageSubAnimalSupplier.ProvinceOid = row;
        //                    objManageSubAnimalSupplier.AnimalSupplieTypeOid = ObjAnimalSupplieType;
        //                    objManageSubAnimalSupplier.ProvinceQTY = 0;
        //                    objManageSubAnimalSupplier.UnitOid = ObjUnitOid;
        //                    Owner.ManageSubAnimalSuppliers.Add(objManageSubAnimalSupplier);

        //                }

        //            }
        //        }
        //        ObjectSpace.CommitChanges();
        //    }
        //    catch (Exception ex)
        //    { //Error case เกิดข้อผิดพลาด
        //        UserError err = new UserError();
        //        err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
        //        err.message = ex.Message;
        //        Return resual
        //        return BadRequest(ex.Message);
        //    }
        //}
        [AllowAnonymous]
        [HttpPost]
        [Route("SupplierUseAnimalProductDetail/Quota")]
        public IHttpActionResult SupplierUseAnimalProductDetail()
        {
            try
            {
                string QuotaType = HttpContext.Current.Request.Form["QuotaType"].ToString();
                string BudgetSource = HttpContext.Current.Request.Form["BudgetSource"].ToString();

                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                XafTypesInfo.Instance.RegisterEntity(typeof(SupplierUseAnimalProductDetail));

                IList<SupplierUseAnimalProductDetail> collection = ObjectSpace.GetObjects<SupplierUseAnimalProductDetail>(CriteriaOperator.Parse(" BudgetSourceOid.Oid = '" + BudgetSource + "' and GCRecord is null and QuotaTypeOid.Oid = ?", QuotaType));
                List<SupplierUseAnimalProductDetail_Model> list_detail = new List<SupplierUseAnimalProductDetail_Model>();
                if (collection.Count > 0)
                {
                    //  SupplierUseAnimalProductDetail ObjMaster;

                    foreach (SupplierUseAnimalProductDetail row in collection)
                    {
                        SupplierUseAnimalProductDetail_Model Model = new SupplierUseAnimalProductDetail_Model();
                        Model.BudgetSource = row.BudgetSourceOid.BudgetName;
                        Model.AnimalSupplie = row.AnimalSupplieOid.AnimalSupplieName;
                        Model.QuotaType = row.QuotaTypeOid.QuotaName;
                        Model.AnimalSupplieType = row.AnimalSupplieTypeOid.SupplietypeName;
                        Model.AnimalSeed = "";
                        Model.StockUsed = row.StockUsed;
                        Model.StockLimit = row.StockLimit;
                        Model.QuotaQTY = row.QuotaQTY;
                        Model.Amount = row.Amount;
                        list_detail.Add(Model);
                    }
                    return Ok(list_detail);
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

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("SupplierUseAnimalApprove")]
        //public IHttpActionResult AnimalUseApprove_Execute()
        //{
        //    try
        //    {
        //        XPCollection<StockAnimalUseInfo> objStockAnimalUseInfo = new XPCollection<StockAnimalUseInfo>();
        //        XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
        //        IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
        //        SupplierUseAnimalProduct ObjMaster;
        //        ObjMaster = ObjectSpace.FindObject<SupplierUseAnimalProduct>(CriteriaOperator.Parse("UseNo=?", UseNo));

        //        foreach (SupplierUseAnimalProductDetail row in ObjMaster.SupplierUseAnimalProductDetails)
        //        {
        //            var objOrganizationOid = row.SupplierUseAnimalProductOid.OrganizationOid;
        //            var objAnimalSupplieOid = row.AnimalSupplieOid;
        //            var objAnimalSupplieTypeOid = row.AnimalSupplieTypeOid;
        //            var objQuotaType = row.QuotaTypeOid;
        //            var objManageSubAnimalSupplierOid = row.ManageSubAnimalSupplierOid;
        //            var ObjAnimalSeedOid = row.AnimalSeedOid;

        //            IList<StockAnimalUseInfo> objStockAnimalUseInfo = ObjectSpace.GetObjects<StockAnimalUseInfo>(CriteriaOperator.Parse("OrganizationOid=? and AnimalSupplieOid=? and AnimalSupplieTypeOid=? and QuotaType=? and   AnimalSeedOid=?", objOrganizationOid.Oid, objAnimalSupplieOid.Oid, objAnimalSupplieTypeOid.Oid, objQuotaType, ObjAnimalSeedOid.Oid));
        //            if (objStockAnimalUseInfo.Count != 0)
        //            {
        //                StockAnimalUseInfo StockAnimalUseInfoNew;
        //                XafTypesInfo.Instance.RegisterEntity(typeof(StockAnimalUseInfo));
        //                StockAnimalUseInfoNew = ObjectSpace.CreateObject<StockAnimalUseInfo>();
        //                StockAnimalUseInfoNew.OrganizationOid = objOrganizationOid;
        //                StockAnimalUseInfoNew.TransactionDate = DateTime.Now;
        //                StockAnimalUseInfoNew.AnimalSupplieOid = objAnimalSupplieOid;
        //                StockAnimalUseInfoNew.AnimalSupplieTypeOid = objAnimalSupplieTypeOid;
        //                StockAnimalUseInfoNew.QuotaTypeOid = objQuotaType;
        //                StockAnimalUseInfoNew.ManageSubAnimalSupplierOid = objManageSubAnimalSupplierOid;
        //                StockAnimalUseInfoNew.AnimalSeedOid = ObjAnimalSeedOid;
        //                StockAnimalUseInfoNew.BudgetSourceOid = row.BudgetSourceOid;
        //                StockAnimalUseInfoNew.Weight = row.Weight;
        //                StockAnimalUseInfoNew.Remark = "อนุมัติใช้เสบียงสัตว์";
        //                ObjectSpace.CommitChanges();
        //            }
        //            else
        //            {
        //               ObjectSpace.CommitChanges();
        //                StockAnimalInfo objStockAnimalInfo = ObjectSpace.CreateObject<StockAnimalInfo>();
        //                objStockAnimalInfo.AnimalProductNumber = ObjMaster.UseNo;
        //                objStockAnimalInfo.AnimalSupplieOid = row.AnimalSupplieOid;
        //                objStockAnimalInfo.FinanceYearOid = ObjMaster.FinanceYearOid;
        //                objStockAnimalInfo.BudgetSourceOid = row.BudgetSourceOid;
        //                objStockAnimalInfo.OrganizationOid = ObjMaster.OrganizationOid;
        //                objStockAnimalInfo.AnimalSupplieTypeOid = row.AnimalSupplieTypeOid;
        //                objStockAnimalInfo.AnimalSeedOid = row.AnimalSeedOid;
        //                objStockAnimalInfo.Weight = 0 - row.Weight;
        //                objStockAnimalInfo.Remark = "ยอดใช้เสบียงสัตว์";
        //                ObjectSpace.CommitChanges();
        //            }

        //            return Ok(true);
        //        }
        //        return BadRequest();

        //    }
        //    catch (Exception ex)
        //    { //Error case เกิดข้อผิดพลาด
        //        UserError err = new UserError();
        //        err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
        //        err.message = ex.Message;
        //        //  Return resual
        //        return BadRequest(ex.Message);
        //    }

        //}
    }
}