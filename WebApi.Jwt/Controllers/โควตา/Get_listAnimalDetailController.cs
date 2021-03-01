using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
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
using WebApi.Jwt.Models.Models_Masters;

namespace WebApi.Jwt.Controllers.MasterData
{
    public class Get_listAnimalDetailController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
        [AllowAnonymous]
        [HttpPost]
        [Route("AnimalSupplieTypeList")]
        public HttpResponseMessage AnimalSupplieType_list()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(AnimalSupplieType));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<AnimalSupplieType> collection = ObjectSpace.GetObjects<AnimalSupplieType>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1", null));

                if (collection.Count > 0)
                {
                    List<AnimalSupplieType_Model> list = new List<AnimalSupplieType_Model>();
                    foreach (AnimalSupplieType row in collection)
                    {
                        AnimalSupplieType_Model item = new AnimalSupplieType_Model();
                        item.AnimalSupplieTypeOid = row.Oid.ToString();
                        item.SupplietypeName = row.SupplietypeName;
                        item.AnimalSupplieName = row.AnimalSupplie.AnimalSupplieName;
                        item.SalePrice = row.SalePrice;
                        list.Add(item);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, list);
                }
                else
                {
                    UserError err = new UserError();
                    err.code = "-1"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                    err.message = "ไม่พบข้อมูล";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
            }
            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);


            }


        }
        /// <summary>
        /// list โควตาจัดสรร
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("QuotaType_list")]
        public HttpResponseMessage QuotaType_list()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(QuotaType));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<QuotaType> collection = ObjectSpace.GetObjects<QuotaType>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1", null));

                if (collection.Count > 0)
                {
                    List<QuotaType_Model> list = new List<QuotaType_Model>();
                    foreach (QuotaType row in collection)
                    {
                        QuotaType_Model item = new QuotaType_Model();
                        item.QuotaTypeOid = row.Oid.ToString();
                        item.QuotaName = row.QuotaName;

                        list.Add(item);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, list);
                }
                else
                {
                    UserError err = new UserError();
                    err.code = "-1"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                    err.message = "ไม่พบข้อมูล";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
            }
            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }
        #region แก้โควตา หญ้าแห้ง
        ///// <summary>
        ///// /// ชุดโควต้า
        ///// </summary>
        ///// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("ManageSubAnimalSupplierList")]
        public HttpResponseMessage ManageSubAnimalSupplierList()
        {

            List<AnimalProductDetail> animalDatail = new List<AnimalProductDetail>();
            AnimalProductDetail animal = new AnimalProductDetail();
            try
            {
                //    string QuotaName = HttpContext.Current.Request.Form["QuotaName"].ToString();
                string OrganizationOid = HttpContext.Current.Request.Form["OrganizationOid"].ToString();
                //string AnimalSupplieOid = HttpContext.Current.Request.Form["AnimalSupplieOid"].ToString();
                //string AnimalSupplieTypeOid = HttpContext.Current.Request.Form["AnimalSupplieTypeOid"].ToString(); 
                string QuotaTypeOid = HttpContext.Current.Request.Form["QuotaTypeOid"].ToString();

                List<QuotaType_Model> listquo = new List<QuotaType_Model>();
                QuotaType_Model quota = new QuotaType_Model();
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(QuotaType));
                XafTypesInfo.Instance.RegisterEntity(typeof(ManageAnimalSupplier));
                XafTypesInfo.Instance.RegisterEntity(typeof(ManageSubAnimalSupplier));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                List<ManageAnimalSupplier_Model2> list = new List<ManageAnimalSupplier_Model2>();
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                QuotaType quotaType;
                //  var ManageSubAnimalSupplierOid = null;
                quotaType = ObjectSpace.FindObject<QuotaType>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1 and Oid  = '" + QuotaTypeOid + "' ", null));
               
                    ManageAnimalSupplier ObjManageAnimalSupplier = ObjectSpace.FindObject<ManageAnimalSupplier>(CriteriaOperator.Parse("[OrganizationOid]=? and Status=1", OrganizationOid));
                    if (ObjManageAnimalSupplier != null)
                    {
                        switch (quotaType.QuotaName)
                        {
                            case "โควตาสำนัก":
                                animal.QuotaName = "โควตาสำนัก"; //ผิด
                            animal.AnimalSupplieTypeOid = ObjectSpace.FindObject<AnimalSupplieType>(CriteriaOperator.Parse("[SupplietypeName]='หญ้าแห้ง'", null)).Oid.ToString();
                            animal.AnimalSupplieTypeName = ObjectSpace.FindObject<AnimalSupplieType>(CriteriaOperator.Parse("[SupplietypeName]='หญ้าแห้ง'", null)).SupplietypeName;

                            break;

                            case "โควตาศูนย์ฯ":
                                animal.QuotaName = "โควตาศูนย์ฯ";
                                animal.AnimalSupplieTypeOid = ObjectSpace.FindObject<AnimalSupplieType>(CriteriaOperator.Parse("[SupplietypeName]='หญ้าแห้ง'", null)).Oid.ToString();
                                animal.AnimalSupplieTypeName = ObjectSpace.FindObject<AnimalSupplieType>(CriteriaOperator.Parse("[SupplietypeName]='หญ้าแห้ง'", null)).SupplietypeName;
                                animal.QuotaQTY = ObjManageAnimalSupplier.CenterQTY;

                                break;
                            case "โควตาปศุสัตว์เขต":
                                animal.QuotaName = "โควตาปศุสัตว์เขต";
                                animal.AnimalSupplieTypeOid = ObjectSpace.FindObject<AnimalSupplieType>(CriteriaOperator.Parse("[SupplietypeName]='หญ้าแห้ง'", null)).Oid.ToString();
                                animal.AnimalSupplieTypeName = ObjectSpace.FindObject<AnimalSupplieType>(CriteriaOperator.Parse("[SupplietypeName]='หญ้าแห้ง'", null)).SupplietypeName;
                                animal.QuotaQTY = ObjManageAnimalSupplier.ZoneQTY;
                                break;
                            //case "โควตาปศุสัตว์จังหวัด":
                            //    animal.QuotaName = "โควตาปศุสัตว์จังหวัด";
                            //    animal.AnimalSupplieTypeOid = ObjectSpace.FindObject<AnimalSupplieType>(CriteriaOperator.Parse("[SupplietypeName]='หญ้าแห้ง'", null)).Oid.ToString();
                            //    animal.AnimalSupplieTypeName = ObjectSpace.FindObject<AnimalSupplieType>(CriteriaOperator.Parse("[SupplietypeName]='หญ้าแห้ง'", null)).SupplietypeName;
                            //    animal.QuotaQTY = ObjManageAnimalSupplier.ZoneQTY;
                            //    break;
                            case "โควตาอื่นๆ":
                                animal.QuotaName = "โควตาอื่นๆ";
                                animal.AnimalSupplieTypeOid = ObjectSpace.FindObject<AnimalSupplieType>(CriteriaOperator.Parse("[SupplietypeName]='หญ้าแห้ง'", null)).Oid.ToString();
                                animal.AnimalSupplieTypeName = ObjectSpace.FindObject<AnimalSupplieType>(CriteriaOperator.Parse("[SupplietypeName]='หญ้าแห้ง'", null)).SupplietypeName;
                                animal.QuotaQTY = ObjManageAnimalSupplier.OtherQTY;
                                break;

                            case "โควตาปศุสัตว์จังหวัด":
                                if (ObjManageAnimalSupplier != null)
                                {
                                animal.AnimalSupplieTypeOid = ObjectSpace.FindObject<AnimalSupplieType>(CriteriaOperator.Parse("[SupplietypeName]='หญ้าแห้ง'", null)).Oid.ToString();
                                animal.AnimalSupplieTypeName = ObjectSpace.FindObject<AnimalSupplieType>(CriteriaOperator.Parse("[SupplietypeName]='หญ้าแห้ง'", null)).SupplietypeName;
                                animal.manageSubAnimalSupplierOid = ObjManageAnimalSupplier.Oid.ToString();
                                    List<ManageSubAnimalSupplier_Province> Detail2 = new List<ManageSubAnimalSupplier_Province>();
                                    IList<ManageSubAnimalSupplier> objmanageSubAnimalSupplier = ObjectSpace.GetObjects<ManageSubAnimalSupplier>(CriteriaOperator.Parse("[ManageAnimalSupplierOid]= '" + ObjManageAnimalSupplier.Oid + "' ", null));
                                    if (objmanageSubAnimalSupplier.Count > 0)
                                    {
                                        foreach (ManageSubAnimalSupplier row2 in objmanageSubAnimalSupplier)
                                        {
                                            ManageSubAnimalSupplier_Province subanimal = new ManageSubAnimalSupplier_Province();
                                            subanimal.ManageSubAnimalSupplierOid = row2.ManageAnimalSupplierOid.Oid.ToString();
                                            subanimal.ProvinceOid = row2.ProvinceOid.Oid.ToString();
                                            subanimal.ProvinceName = row2.ProvinceOid.ProvinceNameTH;
                                            Detail2.Add(subanimal);
                                        }
                                    animal.QuotaName = quotaType.QuotaName;
                                        animal.ObjProvinceName = Detail2;                                   
                                    }                   
                            }
                            break;

                    }
                    directProvider.Dispose();
                    ObjectSpace.Dispose();
                    return Request.CreateResponse(HttpStatusCode.OK, animal);

                   
                }
                    else
                    {
                        UserError err2 = new UserError();
                        err2.code = "3"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                        err2.message = "ไม่พบข้อมูล";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err2);

                    }

                
           
                   
                    }
                 
            
            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }

        }







        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("ManageSubAnimalSupplierList_Quantity")]   ///เรียกโควต้า แห้ง   หมัก สด tmr 
        public HttpResponseMessage ManageAnimalSupplierList_Quantity()
        {
            ManageQuantity item = new ManageQuantity();
            object orgOid = "";
            try
            {
                string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
                if (HttpContext.Current.Request.Form["orgoid"] != null)
                {
                    orgOid = HttpContext.Current.Request.Form["orgoid"].ToString();

            }
                object managesuboid = HttpContext.Current.Request.Form["managesuboid"].ToString();
                object quotatypeoid = HttpContext.Current.Request.Form["quotatypeoid"].ToString();
                object budgetsourceoid = HttpContext.Current.Request.Form["budgetsourceoid"].ToString();
                object animalsupplieroid = HttpContext.Current.Request.Form["animalsupplieroid"].ToString();
                object animalsuppliertypeoid = HttpContext.Current.Request.Form["animalsuppliertypeoid"].ToString();
                object seedtypeoid = HttpContext.Current.Request.Form["seedtypeoid"].ToString();
                ///โควตา
                //ManageSubAnimalSupplierOid, string QuotaTypeOid, string BudgetSourceOid
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(ManageSubAnimalSupplier));
                XafTypesInfo.Instance.RegisterEntity(typeof(QuotaType));
                XafTypesInfo.Instance.RegisterEntity(typeof(ManageAnimalSupplier));
                XafTypesInfo.Instance.RegisterEntity(typeof(SeedType));
                XafTypesInfo.Instance.RegisterEntity(typeof(AnimalSupplieType));
                XafTypesInfo.Instance.RegisterEntity(typeof(AnimalSupplie));
                List<ManageAnimalSupplier_Model2> list = new List<ManageAnimalSupplier_Model2>();
                List<ManageQuantity> listQuantity = new List<ManageQuantity>();
                List<objManageAnimalSupplier> listdetail = new List<objManageAnimalSupplier>();
                ManageQuantity listQuantity2 = new ManageQuantity();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                double balanceQTY = 0.0;
                double amount = 0.0;
                double objstocklimit = 0.0;

                AnimalSupplie objAnimalSupplie = ObjectSpace.FindObject<AnimalSupplie>(CriteriaOperator.Parse("[oid] =? ", animalsupplieroid));
                if (objAnimalSupplie.AnimalSupplieName == "แห้ง")
                {

                    QuotaType objQuotaType = ObjectSpace.FindObject<QuotaType>(CriteriaOperator.Parse("[Oid]=?", quotatypeoid));
                    if (objQuotaType != null)
                    {
                        if (objQuotaType.QuotaName != "โควตาปศุสัตว์จังหวัด")
                        {
                            ManageAnimalSupplier objManageAnimalSupplier = ObjectSpace.FindObject<ManageAnimalSupplier>(CriteriaOperator.Parse("[OrganizationOid] =? and Status = 1", orgOid));
                            if (objManageAnimalSupplier != null)
                            {
                                if (objQuotaType.QuotaName == "โควตาปศุสัตว์เขต")
                                {
                                    listQuantity2.QuotaName = objQuotaType.QuotaName;
                                    listQuantity2.QuotaQTY = objManageAnimalSupplier.ZoneQTY;
                                }
                                else if (objQuotaType.QuotaName == "โควตาอื่นๆ")
                                {
                                    listQuantity2.QuotaName = objQuotaType.QuotaName;
                                    listQuantity2.QuotaQTY = objManageAnimalSupplier.OtherQTY;
                                }
                                else if (objQuotaType.QuotaName == "โควตาศูนย์ฯ")
                                {
                                    listQuantity2.QuotaName = objQuotaType.QuotaName;
                                    listQuantity2.QuotaQTY = objManageAnimalSupplier.CenterQTY;
                                }
                                else if (objQuotaType.QuotaName == "โควตาสำนัก")
                                {
                                    listQuantity2.QuotaName = objQuotaType.QuotaName;
                                    SeedType objSeedType = ObjectSpace.FindObject<SeedType>(CriteriaOperator.Parse("[Oid]=?", seedtypeoid));
                                    AnimalSupplieType objAnimalSupplieType = ObjectSpace.FindObject<AnimalSupplieType>(CriteriaOperator.Parse("[Oid]=?", animalsuppliertypeoid));
                                    if (objSeedType != null && objAnimalSupplieType != null)
                                    {
                                        if (objSeedType.SeedTypeName == "GAP" && objAnimalSupplieType.SupplietypeName == "หญ้าแห้ง")
                                        {
                                            listQuantity2.QuotaQTY = objManageAnimalSupplier.OfficeGAPQTY;
                                        }
                                        else if (objSeedType.SeedTypeName.ToLower() == "normal" && objAnimalSupplieType.SupplietypeName == "หญ้าแห้ง")
                                        {
                                            listQuantity2.QuotaQTY = objManageAnimalSupplier.OfficeQTY;
                                        }
                                        else if (objSeedType.SeedTypeName.ToLower() == "normal" && objAnimalSupplieType.SupplietypeName == "ถั่วแห้ง")
                                        {
                                            listQuantity2.QuotaQTY = objManageAnimalSupplier.OfficeBeanQTY;
                                        }
                                        else if (objSeedType.SeedTypeName.ToLower() == "oganic")
                                        {
                                            listQuantity2.QuotaQTY = 0;
                                        }
                                    }
                                }
                            }
                        }
                        else //โควตาจังหวัด
                        {
                            listQuantity2.QuotaName = objQuotaType.QuotaName;
                            ManageSubAnimalSupplier objManageSubAnimalSupplier = ObjectSpace.FindObject<ManageSubAnimalSupplier>(CriteriaOperator.Parse("[ManageSubAnimalSupplier.ProvinceOid] =?", managesuboid));
                            if (objManageSubAnimalSupplier != null)
                            {

                                listQuantity2.QuotaQTY = objManageSubAnimalSupplier.ProvinceQTY;

                                listQuantity2.Provincename = objManageSubAnimalSupplier.ProvinceOid.ProvinceNameTH;

                            }
                        }

                        //Get StockUsed
                        DataSet Ds = null;
                        if (managesuboid != null && managesuboid != "")
                        {
                            Ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "GetStockUsed_QuotaHay_Province"
                         , new SqlParameter("@OrganizationOid", orgOid)
                         , new SqlParameter("@AnimalSupplieTypeOid", animalsuppliertypeoid)
                         , new SqlParameter("@QuotaTypeOid", quotatypeoid)
                         , new SqlParameter("@BudgetSourceOid", budgetsourceoid)
                         , new SqlParameter("@AnimalSupplieOid", animalsupplieroid)
                         , new SqlParameter("@ManageSubAnimalSupplierOid", managesuboid.ToString())
                         , new SqlParameter("@SeedTypeOid", seedtypeoid));
                        }
                        else
                        {
                            Ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "GetStockUsed_QuotaHay"
                         , new SqlParameter("@OrganizationOid", orgOid.ToString())
                         , new SqlParameter("@AnimalSupplieTypeOid", animalsuppliertypeoid.ToString())
                         , new SqlParameter("@QuotaTypeOid", quotatypeoid.ToString())
                         , new SqlParameter("@BudgetSourceOid", budgetsourceoid.ToString())
                         , new SqlParameter("@AnimalSupplieOid", animalsupplieroid.ToString())
                         , new SqlParameter("@SeedTypeOid", seedtypeoid));
                        }
                        if (Ds.Tables[0].Rows.Count > 0)
                        {
                            //listQuantity2.ba = (double)Ds.Tables[0].Rows[0]["StockUsed"];
                            listQuantity2.balancQuotaQTY = listQuantity2.QuotaQTY - (double)Ds.Tables[0].Rows[0]["StockUsed"];
                            listQuantity2.StockUsed = (double)Ds.Tables[0].Rows[0]["StockUsed"];

                        }
                        else
                        {
                            //listQuantity2.QuotaQTY = 0;
                            listQuantity2.balanceQTY = 0;
                            listQuantity2.balancQuotaQTY = 0;
                        }

                        Ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "sp_StockAnimalInfo"
                        , new SqlParameter("@OrganizationOid", orgOid)
                        , new SqlParameter("@AnimalSupplieTypeOid", animalsuppliertypeoid)
                        , new SqlParameter("@BudgetSourceOid", budgetsourceoid)
                        , new SqlParameter("@AnimalSupplieOid", animalsupplieroid)
                        , new SqlParameter("@SeedTypeOid", seedtypeoid));
                        if (Ds.Tables[0].Rows.Count > 0)
                        {
                            listQuantity2.balanceQTY = (double)Ds.Tables[0].Rows[0]["Total_StockAnimalInfo"];
                        }
                        else
                        {
                            listQuantity2.balanceQTY = 0;
                        }

                    }
                    else //QuotaType = null
                    {
                        listQuantity2.QuotaQTY = 0; //ปริมาณตามแผนจัดสรร
                        listQuantity2.balanceQTY = 0; //จำนวนคงเหลือตามจริง
                        listQuantity2.balancQuotaQTY = 0; //ปริมาณคงเหลือตามแผนจัดสรร
                    }

                    listQuantity.Add(listQuantity2);
                }
                else
                {
                    // string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
                    orgOid = HttpContext.Current.Request.Form["orgoid"].ToString();
                    //   string quotatypeoid = HttpContext.Current.Request.Form["quotatypeoid"].ToString();
                    budgetsourceoid = HttpContext.Current.Request.Form["budgetsourceoid"].ToString();
                    animalsupplieroid = HttpContext.Current.Request.Form["animalsupplieroid"].ToString();
                    animalsuppliertypeoid = HttpContext.Current.Request.Form["animalsuppliertypeoid"].ToString();
                    var stocklimit = 0.0;
                    if (HttpContext.Current.Request.Form["seedtypeoid"].ToString() != null && HttpContext.Current.Request.Form["seedtypeoid"].ToString() != "")
                    {
                        seedtypeoid = HttpContext.Current.Request.Form["seedtypeoid"].ToString();
                        DataSet Ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "sp_StockAnimalInfo"
                         , new SqlParameter("@OrganizationOid", orgOid)
                         , new SqlParameter("@AnimalSupplieTypeOid", animalsuppliertypeoid)
                         , new SqlParameter("@BudgetSourceOid", budgetsourceoid)
                         , new SqlParameter("@AnimalSupplieOid", animalsupplieroid)
                         , new SqlParameter("@SeedTypeOid", seedtypeoid));
                        if (Ds.Tables[0].Rows.Count > 0)
                        {
                            balanceQTY = (double)Ds.Tables[0].Rows[0]["Total_StockAnimalInfo"];
                        }
                        else
                        {
                            balanceQTY = 0;
                        }

                        Ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "sp_StockAnimalInfo_TMR"
                         , new SqlParameter("@OrganizationOid", orgOid)
                         , new SqlParameter("@AnimalSupplieTypeOid", animalsuppliertypeoid)
                         , new SqlParameter("@BudgetSourceOid", budgetsourceoid)
                         , new SqlParameter("@AnimalSupplieOid", animalsupplieroid)
                         , new SqlParameter("@SeedTypeOid", seedtypeoid));
                        if (Ds.Tables[0].Rows.Count > 0)
                        {
                            stocklimit = (double)Ds.Tables[0].Rows[0]["Total_Current"];
                        }
                        else
                        {
                            stocklimit = 0;
                        }
                    }
                    else
                    {
                        DataSet Ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "sp_StockAnimalInfo"
                         , new SqlParameter("@OrganizationOid", orgOid)
                         , new SqlParameter("@AnimalSupplieTypeOid", animalsuppliertypeoid)
                         , new SqlParameter("@BudgetSourceOid", budgetsourceoid)
                         , new SqlParameter("@AnimalSupplieOid", animalsupplieroid)
                         , new SqlParameter("@SeedTypeOid", null));
                        if (Ds.Tables[0].Rows.Count > 0)
                        {
                            balanceQTY = (double)Ds.Tables[0].Rows[0]["Total_StockAnimalInfo"];
                        }
                        else
                        {
                            balanceQTY = 0;
                        }

                        Ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "sp_StockAnimalInfo_TMR"
                         , new SqlParameter("@OrganizationOid", orgOid)
                         , new SqlParameter("@AnimalSupplieTypeOid", animalsuppliertypeoid)
                         , new SqlParameter("@BudgetSourceOid", budgetsourceoid)
                         , new SqlParameter("@AnimalSupplieOid", animalsupplieroid)
                         , new SqlParameter("@SeedTypeOid", null));
                        if (Ds.Tables[0].Rows.Count > 0)
                        {
                            stocklimit = (double)Ds.Tables[0].Rows[0]["Total_Current"];
                        }
                        else
                        {
                            stocklimit = 0;
                        }
                    }


                    listQuantity2.QuotaQTY = 0;
                    listQuantity2.balanceQTY = balanceQTY;
                    listQuantity2.balancQuotaQTY = 0;
                    listQuantity2.StockUsed = stocklimit;
                    listQuantity.Add(listQuantity2);
                }

                UserError err = new UserError();
                err.code = ""; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = "OK";
                return Request.CreateResponse(HttpStatusCode.OK, listQuantity);

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
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("StockAnimalInfoList_Quantity")]   ///หาโควต้า TMR / สด / หมัก
        public HttpResponseMessage StockAnimalInfoList_Quantity()
        {
            ManageQuantity item = new ManageQuantity();
            try
            {

                string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
                string orgOid = HttpContext.Current.Request.Form["orgoid"].ToString();
                //   string quotatypeoid = HttpContext.Current.Request.Form["quotatypeoid"].ToString();
                string budgetsourceoid = HttpContext.Current.Request.Form["budgetsourceoid"].ToString();
                string animalsupplieroid = HttpContext.Current.Request.Form["animalsupplieroid"].ToString();
                string animalsuppliertypeoid = HttpContext.Current.Request.Form["animalsuppliertypeoid"].ToString();

                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                Get_listAnimalDetailController getlist = new Get_listAnimalDetailController();
                var stockused = 0.0;
                var stocklimit = 0.0;
                string seedtypeoid = null;

                if (HttpContext.Current.Request.Form["seedtypeoid"].ToString() != null && HttpContext.Current.Request.Form["seedtypeoid"].ToString() != "")
                {
                    seedtypeoid = HttpContext.Current.Request.Form["seedtypeoid"].ToString();
                    DataSet Ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "sp_StockAnimalInfo"
                     , new SqlParameter("@OrganizationOid", orgOid)
                     , new SqlParameter("@AnimalSupplieTypeOid", animalsuppliertypeoid)
                     , new SqlParameter("@BudgetSourceOid", budgetsourceoid)
                     , new SqlParameter("@AnimalSupplieOid", animalsupplieroid)
                     , new SqlParameter("@SeedTypeOid", seedtypeoid));
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        stockused = (double)Ds.Tables[0].Rows[0]["Total_StockAnimalInfo"];
                    }
                    else
                    {
                        stockused = 0;
                    }

                    Ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "sp_StockAnimalInfo_TMR"
                     , new SqlParameter("@OrganizationOid", orgOid)
                     , new SqlParameter("@AnimalSupplieTypeOid", animalsuppliertypeoid)
                     , new SqlParameter("@BudgetSourceOid", budgetsourceoid)
                     , new SqlParameter("@AnimalSupplieOid", animalsupplieroid)
                     , new SqlParameter("@SeedTypeOid", seedtypeoid));
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        stocklimit = (double)Ds.Tables[0].Rows[0]["Total_Current"];
                    }
                    else
                    {
                        stocklimit = 0;
                    }
                }
                else
                {
                    DataSet Ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "sp_StockAnimalInfo"
                     , new SqlParameter("@OrganizationOid", orgOid)
                     , new SqlParameter("@AnimalSupplieTypeOid", animalsuppliertypeoid)
                     , new SqlParameter("@BudgetSourceOid", budgetsourceoid)
                     , new SqlParameter("@AnimalSupplieOid", animalsupplieroid)
                     , new SqlParameter("@SeedTypeOid", null));
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        stockused = (double)Ds.Tables[0].Rows[0]["Total_StockAnimalInfo"];
                    }
                    else
                    {
                        stockused = 0;
                    }

                    Ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "sp_StockAnimalInfo_TMR"
                     , new SqlParameter("@OrganizationOid", orgOid)
                     , new SqlParameter("@AnimalSupplieTypeOid", animalsuppliertypeoid)
                     , new SqlParameter("@BudgetSourceOid", budgetsourceoid)
                     , new SqlParameter("@AnimalSupplieOid", animalsupplieroid)
                     , new SqlParameter("@SeedTypeOid", null));
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        stocklimit = (double)Ds.Tables[0].Rows[0]["Total_Current"];
                    }
                    else
                    {
                        stocklimit = 0;
                    }
                }


                //item.ProvinceQTY = 0;
                //item.Current_ProvinceQTY = stockused;
                //item.StockLimit = stocklimit;

                UserError err = new UserError();
                err.code = ""; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = "OK";
                return Request.CreateResponse(HttpStatusCode.OK, item);

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
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]/// เลือกโควต้าจังหวัด
        [HttpPost]
        [Route("QuotaProvince_info")]

        public HttpResponseMessage quata_typemanage()
        {
            try
            {
                string ManageAnimalSupplier = HttpContext.Current.Request.Form["manageanimalsupplieroid"].ToString();
                //      string animalSupplieOid = HttpContext.Current.Request.Form["animalSupplieOid"].ToString();
                string quotatypeoid = HttpContext.Current.Request.Form["quotatypeoid"].ToString();
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(ManageAnimalSupplier));
                XafTypesInfo.Instance.RegisterEntity(typeof(QuotaType));
                XafTypesInfo.Instance.RegisterEntity(typeof(ManageSubAnimalSupplier));
                XafTypesInfo.Instance.RegisterEntity(typeof(AnimalSupplie));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                List<objManageAnimalSupplier> listdetail = new List<objManageAnimalSupplier>();
                ManageAnimalSupplier_Modelinfo2 item = new ManageAnimalSupplier_Modelinfo2();
                QuotaType objQuotaType = ObjectSpace.FindObject<QuotaType>(CriteriaOperator.Parse(" GCRecord is null and  oid = ? ", quotatypeoid));
                IList<ManageSubAnimalSupplier> objmanageSubAnimalSupplier = ObjectSpace.GetObjects<ManageSubAnimalSupplier>(CriteriaOperator.Parse("ProvinceOid.OrganizationOid= '" + ManageAnimalSupplier + "' ", null));
                ManageSubAnimalSupplier manageanimalsupplie = ObjectSpace.FindObject<ManageSubAnimalSupplier>(CriteriaOperator.Parse(" GCRecord is null and  [ProvinceOid.OrganizationOid]= ? ", ManageAnimalSupplier));
                if (objQuotaType.QuotaName == "โควตาปศุสัตว์จังหวัด")
                {
                    QuotaType_Model itemquota = new QuotaType_Model();
                    item.QuotaName = objQuotaType.QuotaName;
                    //item.OrganizationOid = manageanimalsupplie.ManageAnimalSupplierOid.OrganizationOid.Oid.ToString();
                    //item.Organizationname = manageanimalsupplie.ManageAnimalSupplierOid.OrganizationOid.SubOrganizeName;

                    foreach (ManageSubAnimalSupplier row in objmanageSubAnimalSupplier)
                    {
                        objManageAnimalSupplier detail = new objManageAnimalSupplier();
                        detail.ProvinceOid = row.ProvinceOid.Oid.ToString();
                        detail.Provincename = row.ProvinceOid.ProvinceNameTH;
                        listdetail.Add(detail);
                    }
                    item.detail = listdetail;
                }
                else
                {
                    item.QuotaName = objQuotaType.QuotaName;
                }

                return Request.CreateResponse(HttpStatusCode.OK, item);
            }

            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);

            }
        }


        public static double GetStockUsed(string OrganizationOid, string QuotaTypeOid, string BudgetSourceOid, string AnimalSupplieTypeOid, string AnimalSupplieOid)
        {
            double StockUsed = 0;
            string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
            DataSet Ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "SP_GetStockUsed"
                , new SqlParameter("@OrganizationOid", OrganizationOid)
                , new SqlParameter("@AnimalSupplieTypeOid", AnimalSupplieTypeOid)
                , new SqlParameter("@QuotaTypeOid", QuotaTypeOid)
                , new SqlParameter("@BudgetSourceOid", BudgetSourceOid)
                , new SqlParameter("@AnimalSupplieOid", AnimalSupplieOid));
            if (Ds.Tables[0].Rows.Count > 0)
            {
                StockUsed = Convert.ToDouble(Ds.Tables["Total_Current"].ToString());
            }
            else
            {
                StockUsed = 0;
            }
            return StockUsed;
        }
    }

    public class GetBlance
    {
        public GetBlance()
        {
        }

        public double Total { get; set; }
        public string Name { get; set; }
    }
}
#endregion





