using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using Microsoft.ApplicationBlocks.Data;
using nutrition.Module;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Jwt.Models;

namespace WebApi.Jwt.Controllers
{
    public class UserService_Controller : ApiController
    {
        private string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        /// <summary>
        /// ใช้ในการเรียกหน่วยงานที่ขอรับบริการ
        /// </summary>
        /// <param name=</param>
        /// <returns></returns>
        [AllowAnonymous]
        //[JwtAuthentication] /ถ้าใช้โทเคนต้องครอบ
        // [HttpPost]
        [HttpPost]
        [Route("SeachCustomer/info")]
        public HttpResponseMessage OrgeCustomer()
        {
            try

            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(OrgeService));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                List<OrgeService_info> list = new List<OrgeService_info>();
                List<OrgeServiceDetail_Model> list_detail = new List<OrgeServiceDetail_Model>();
                IList<OrgeService> collection = ObjectSpace.GetObjects<OrgeService>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1 ", null));
                string TempSubDistrict = " ", TempDistrict = " ";
                if (collection.Count > 0)
                {
                    foreach (OrgeService row in collection)
                    {
                        OrgeService_info Customer_Info = new OrgeService_info();

                        //Customer_Info.OrganizationOid = row.OrganizationOid.OrganizeNameTH;

                        Customer_Info.OrgeServiceOid = row.Oid.ToString();
                        Customer_Info.OrgeServiceName = row.OrgeServiceName;

                        if (row.Tel != null)
                        {
                            Customer_Info.Tel = row.Tel;
                        }

                        if (row.Email != null)
                        {
                            Customer_Info.Email = row.Email;
                        }

                        if (row.Address != "")
                        {
                            Customer_Info.Address = row.Address;
                        }

                        if (row.Moo != "")
                        {
                            Customer_Info.Moo = row.Moo;
                        }

                        if (row.Soi != "")
                        {
                            Customer_Info.Soi = row.Soi;
                        }

                        if (row.Road != "")
                        {
                            Customer_Info.Road = row.Road;
                        }

                        if (row.ProvinceOid != null)
                        {
                            Customer_Info.ProvinceOid = row.ProvinceOid.Oid.ToString();
                            Customer_Info.ProvinceName = row.ProvinceOid.ProvinceNameTH;
                            if (row.ProvinceOid.ProvinceNameTH.Contains("กรุงเทพ"))
                            { TempSubDistrict = "แขวง"; }
                            else
                            { TempSubDistrict = "ตำบล"; };
                            if (row.ProvinceOid.ProvinceNameTH.Contains("กรุงเทพ"))
                            { TempDistrict = "เขต"; }
                            else { TempDistrict = "อำเภอ"; };
                        }

                        if (row.DistrictOid != null)
                        {
                            Customer_Info.DistrictOid = row.DistrictOid.Oid.ToString();
                            Customer_Info.DistrictName = row.DistrictOid.DistrictNameTH;
                        }

                        if (row.SubDistrictOid != null)
                        {
                            Customer_Info.SubDistrictOid = row.SubDistrictOid.Oid.ToString();
                            Customer_Info.SubDistrictName = row.SubDistrictOid.SubDistrictNameTH;
                        }

                        if (row.ZipCode != null)
                        {
                            Customer_Info.ZipCode = row.ZipCode;
                        }

                        //if (Customer_Info.Address != null && Customer_Info.Moo != null && Customer_Info.Road != null)

                        Customer_Info.FullAddress = "เลขที่" + " " + Customer_Info.Address + " หมู่ที่" + " " + checknull(Customer_Info.Moo) + " ถนน" + " " + checknull(Customer_Info.Road) + " " + TempSubDistrict
                         + " " + Customer_Info.SubDistrictName + " " + TempDistrict + " " + Customer_Info.DistrictName + " " +
                        "จังหวัด" + " " + Customer_Info.ProvinceName + " รหัสไปรษณีย์ " + Customer_Info.ZipCode;

                        list.Add(Customer_Info);
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "NoData");
                }

                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }

        [AllowAnonymous]
        //[JwtAuthentication] /ถ้าใช้โทเคนต้องครอบ
        ///ค้นหาด้วยชื่อ หน่วยงาน
        // [HttpPost]
        [HttpPost]
        [Route("SeachCustomer/ID")]
        public HttpResponseMessage OrgeCustomer_All()
        {
            string OrgeServiceName = string.Empty;
            try

            {
                if (HttpContext.Current.Request.Form["OrgeServiceName"].ToString() != null)
                {
                    OrgeServiceName = HttpContext.Current.Request.Form["OrgeServiceName"].ToString();
                }

                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(OrgeService));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                OrgeService_info Customer_Info = new OrgeService_info();
                List<OrgeService_info> list = new List<OrgeService_info>();
                List<OrgeServiceDetail_Model> list_detail = new List<OrgeServiceDetail_Model>();
                OrgeService OrgeService_;
                OrgeService_ = ObjectSpace.FindObject<OrgeService>(CriteriaOperator.Parse("GCRecord is null and OrgeServiceName = ? ", OrgeServiceName));
                DataSet ds = SqlHelper.ExecuteDataset(scc, CommandType.Text, "select OrgeServiceName from OrgeService where OrgeServiceName = '" + OrgeServiceName + "'");
                if (ds.Tables[0].Rows.Count != 0)
                {
                    //Customer_Info.OrganizationOid = row.OrganizationOid.OrganizeNameTH;
                    //   Customer_Info.OrgeServiceID = row.or
                    Customer_Info.OrgeServiceName = OrgeService_.OrgeServiceName;
                    Customer_Info.Tel = OrgeService_.Tel;
                    if (OrgeService_.Email == null)
                    {
                        Customer_Info.Email = "ไม่พบข้อมูลอีเมล์";
                    }
                    else
                    {
                        Customer_Info.Email = OrgeService_.Email;
                    }
                    if (OrgeService_.Address == null)
                    {
                        Customer_Info.Address = "ไม่พบมูลบ้านเลขที่";
                    }
                    else
                    {
                        Customer_Info.Address = OrgeService_.Address;
                    }

                    if (OrgeService_.Moo == null)
                    {
                        Customer_Info.Moo = "ไม่พบข้อมูลหมู่";
                    }
                    else
                    {
                        Customer_Info.Moo = OrgeService_.Moo;
                    }
                    if (OrgeService_.Soi == null)
                    {
                        Customer_Info.Soi = "ไม่พบข้อมูลซอย";
                    }
                    else
                    {
                        Customer_Info.Soi = OrgeService_.Soi;
                    }

                    if (OrgeService_.Road == null)
                    {
                        Customer_Info.Road = "ไม่พบข้อมูลถนน";
                    }
                    else
                    {
                        Customer_Info.Road = OrgeService_.Road;
                    }
                    if (OrgeService_.ProvinceOid == null)
                    {
                        Customer_Info.ProvinceName = "ไม่พบข้อมูลจังหวัด";
                    }
                    else
                    {
                        Customer_Info.ProvinceName = OrgeService_.ProvinceOid.ProvinceNameTH;
                    }
                    if (OrgeService_.DistrictOid == null)
                    {
                        Customer_Info.DistrictName = "ไม่พบข้อมูลอำเภอ";
                    }
                    else
                    {
                        Customer_Info.DistrictName = OrgeService_.DistrictOid.DistrictNameTH;
                    }
                    if (OrgeService_.SubDistrictOid == null)
                    {
                        Customer_Info.SubDistrictName = "ไม่พบข้อมูลตำบล";
                    }
                    else
                    {
                        Customer_Info.SubDistrictName = OrgeService_.SubDistrictOid.SubDistrictNameTH;
                    }

                    if (OrgeService_.ZipCode == null)
                    {
                        Customer_Info.ZipCode = "ไม่พบข้อมูลรหัสไปรษณีย์";
                    }
                    else
                    {
                        Customer_Info.ZipCode = OrgeService_.ZipCode;
                    }

                    string TempSubDistrict, TempDistrict;
                    if (OrgeService_.ProvinceOid.ProvinceNameTH.Contains("กรุงเทพ"))
                    { TempSubDistrict = "แขวง"; }
                    else
                    { TempSubDistrict = "ตำบล"; };

                    if (OrgeService_.ProvinceOid.ProvinceNameTH.Contains("กรุงเทพ"))
                    { TempDistrict = "เขต"; }
                    else { TempDistrict = "อำเภอ"; };

                    Customer_Info.FullAddress = "เลขที่" + " " + Customer_Info.Address + " หมู่ที่" + " " + checknull(Customer_Info.Moo) + " ถนน" + " " + checknull(Customer_Info.Road) + " " + TempSubDistrict
                     + " " + Customer_Info.SubDistrictName + " " + TempDistrict + " " + Customer_Info.DistrictName + " " +
                    "จังหวัด" + " " + Customer_Info.ProvinceName + " รหัสไปรษณีย์ " + Customer_Info.ZipCode;

                    foreach (OrgeServiceDetail row in OrgeService_.OrgeServiceDetails)
                    {
                        OrgeServiceDetail_Model Model = new OrgeServiceDetail_Model();
                        Model.OrgeServiceOid = row.OrgeServiceOid.OrgeServiceName;
                        Model.ServiceTypeOid = row.ServiceTypeOid.ServiceTypeName;
                        Model.SubServiceTypeOid = row.SubServiceTypeOid.ServiceTypeName;
                        list_detail.Add(Model);
                    }
                    Customer_Info.OrgeServiceDetails = list_detail;
                    return Request.CreateResponse(HttpStatusCode.OK, Customer_Info);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "NoData");
                }
            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }

        [AllowAnonymous]
        //[JwtAuthentication] /ถ้าใช้โทเคนต้องครอบ
        ///ค้นหาชื่อผู้ขอรับบริการ
        // [HttpPost]
        [HttpPost]
        [Route("SeachCusService/List")]
        public HttpResponseMessage GetRegisterCusServiceList()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(RegisterCusService));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                List<RegicusService_Model> list = new List<RegicusService_Model>();
                IList<RegisterCusService> collection = ObjectSpace.GetObjects<RegisterCusService>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1 ", null));
                string TempSubDistrict = " ", TempDistrict = " ";
                if (collection.Count > 0)
                {
                    foreach (RegisterCusService row in collection)
                    {
                        RegicusService_Model item = new RegicusService_Model();
                        if (row.OrganizationOid != null)
                        {
                            item.OrganizationOid = row.OrganizationOid.Oid.ToString();
                        }

                        item.RegicusServiceOid = row.Oid.ToString();
                        item.RegisterDate = row.RegisterDate.ToString();
                        item.CitizenID = row.CitizenID;
                        item.TitleOid = row.TitleOid.Oid.ToString();
                        item.TitleName = row.TitleOid.TitleName;
                        item.FirstNameTH = row.FirstNameTH;
                        item.LastNameTH = row.LastNameTH;
                        if (row.GenderOid != null)
                        {
                            item.GenderOid = row.GenderOid.Oid.ToString();
                            item.Gender = row.GenderOid.GenderName;
                        }

                        if (row.BirthDate != null)
                        {
                            item.BirthDate = row.BirthDate.ToString("dd/MM/yyyy");
                        }
                        if (row.Tel != null)
                        {
                            item.Tel = row.Tel;
                        }
                        if (row.Email != null)
                        {
                            item.Email = row.Email;
                        }
                        if (row.Address != "")
                        {
                            item.Address = row.Address;
                        }

                        if (row.Moo != "")
                        {
                            item.Moo = row.Moo;
                        }

                        if (row.Soi != "")
                        {
                            item.Soi = row.Soi;
                        }

                        if (row.Road != "")
                        {
                            item.Road = row.Road;
                        }

                        if (row.ProvinceOid != null)
                        {
                            item.ProvinceName = row.ProvinceOid.ProvinceNameTH;
                            if (row.ProvinceOid.ProvinceNameTH.Contains("กรุงเทพ"))
                            { TempSubDistrict = "แขวง"; }
                            else
                            { TempSubDistrict = "ตำบล"; };
                            if (row.ProvinceOid.ProvinceNameTH.Contains("กรุงเทพ"))
                            { TempDistrict = "เขต"; }
                            else { TempDistrict = "อำเภอ"; };
                        }

                        if (row.DistrictOid != null)
                        {
                            item.DistrictName = row.DistrictOid.DistrictNameTH;
                        }

                        if (row.SubDistrictOid != null)
                        {
                            item.SubDistrictName = row.SubDistrictOid.SubDistrictNameTH;
                        }

                        if (row.ZipCode != null)
                        {
                            item.ZipCode = row.ZipCode;
                        }

                        item.FullName = item.TitleName + item.FirstNameTH + " " + item.LastNameTH;

                        item.FullAddress = "เลขที่" + " " + item.Address + " หมู่ที่" + " " + checknull(item.Moo) + " ถนน" + " " + checknull(item.Road) + " " + TempSubDistrict
                         + " " + item.SubDistrictName + " " + TempDistrict + " " + item.DistrictName + " " +
                        "จังหวัด" + " " + item.ProvinceName + " รหัสไปรษณีย์ " + item.ZipCode;

                        if (row.Remark != null)
                        {
                            item.Remark = row.Remark;
                        }

                        item.IsActive = row.IsActive;
                        item.ServicesNumber = row.ServicesNumber.ToString();

                        //List<RegisterCusServiceDetail_Model> item2 = new List<RegisterCusServiceDetail_Model>();
                        //foreach (RegisterCusServiceDetail row2 in row.RegisterCusServiceDetails)
                        //{
                        //    RegisterCusServiceDetail_Model d2 = new RegisterCusServiceDetail_Model();
                        //    d2.RegisterCusServiceOid = row2.RegisterCusServiceOid.Oid.ToString();
                        //    d2.RefOid = row2.RefOid;
                        //    d2.ReceiveDate = row2.ReceiveDate.ToString();
                        //    d2.ServiceTypeOid = d2.ServiceTypeOid;
                        //    d2.ServiceTypeName = d2.ServiceTypeName;
                        //    d2.SubServiceTypeOid = d2.SubServiceTypeOid;
                        //    d2.ServiceTypeName = d2.SubServiceTypeName;
                        //    item2.Add(d2);
                        //}
                        //item.Detail = item2;
                        list.Add(item);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, list);
                }
                else
                {
                    UserError err = new UserError();
                    err.status = "false";
                    err.code = "0";
                    err.message = "ไม่มีชื่อผู้ขอรับบริการ โปรดทำการลงทะเบียน";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }

        [AllowAnonymous]
        //[JwtAuthentication] /ถ้าใช้โทเคนต้องครอบ
        ///ค้นหาด้วยเลขบัตร ปชช CitizenID
        // [HttpPost]
        [HttpPost]
        [Route("SeachCusService/CitizenID")]
        public HttpResponseMessage RegisterCusService_ByCitizenID()
        {
            string CitizenID = string.Empty;
            try

            {
                if (HttpContext.Current.Request.Form["CitizenID"].ToString() != null)
                {
                    CitizenID = HttpContext.Current.Request.Form["CitizenID"].ToString();
                }
                RegisterFarmerController best = new RegisterFarmerController();
                if (best.CheckCitizenID(CitizenID) == false)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "หมายเลขบัตรประชาชนไม่ถูกต้อง กรุณาตรวจสอบ");
                }
                if (CitizenID != "")
                {
                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(RegisterCusService));
                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                    RegicusService_Model RegisterCusServicer_Info = new RegicusService_Model();
                    RegisterCusService RegisterCusService_;
                    RegisterCusService_ = ObjectSpace.FindObject<RegisterCusService>(CriteriaOperator.Parse("GCRecord is null and CitizenID = ? ", CitizenID));
                    string TempSubDistrict = "", TempDistrict = "";
                    if (RegisterCusService_ != null)
                    {
                        RegicusService_Model item = new RegicusService_Model();
                        item.RegicusServiceOid = RegisterCusService_.Oid.ToString();
                        item.OrganizationOid = RegisterCusService_.OrganizationOid.Oid.ToString();
                        item.RegisterDate = RegisterCusService_.RegisterDate.ToString("dd/MM/yyyy");
                        item.CitizenID = RegisterCusService_.CitizenID;
                        item.TitleOid = RegisterCusService_.TitleOid.Oid.ToString();
                        item.TitleName = RegisterCusService_.TitleOid.TitleName;
                        item.FirstNameTH = RegisterCusService_.FirstNameTH;
                        item.LastNameTH = RegisterCusService_.LastNameTH;
                        if (RegisterCusService_.GenderOid != null)
                        {
                            item.GenderOid = RegisterCusService_.GenderOid.Oid.ToString();
                            item.Gender = RegisterCusService_.GenderOid.GenderName;
                        }

                        if (RegisterCusService_.BirthDate != null)
                        {
                            item.BirthDate = RegisterCusService_.BirthDate.ToString("dd/MM/yyyy");
                        }
                        if (RegisterCusService_.Tel != null)
                        {
                            item.Tel = RegisterCusService_.Tel;
                        }
                        if (RegisterCusService_.Email != null)
                        {
                            item.Email = RegisterCusService_.Email;
                        }
                        item.FullName = item.TitleName + item.FirstNameTH + " " + item.LastNameTH;

                        if (RegisterCusService_.Address != "")
                        {
                            item.Address = RegisterCusService_.Address;
                        }

                        if (RegisterCusService_.Moo != "")
                        {
                            item.Moo = RegisterCusService_.Moo;
                        }

                        if (RegisterCusService_.Soi != " ")
                        {
                            item.Soi = RegisterCusService_.Soi;
                        }

                        if (RegisterCusService_.Road != "")
                        {
                            item.Road = RegisterCusService_.Road;
                        }

                        if (RegisterCusService_.ProvinceOid != null)
                        {
                            item.ProvinceOid = RegisterCusService_.ProvinceOid.Oid.ToString();
                            item.ProvinceName = RegisterCusService_.ProvinceOid.ProvinceNameTH;
                            if (RegisterCusService_.ProvinceOid.ProvinceNameTH.Contains("กรุงเทพ"))
                            { TempSubDistrict = "แขวง"; }
                            else
                            { TempSubDistrict = "ตำบล"; };
                            if (RegisterCusService_.ProvinceOid.ProvinceNameTH.Contains("กรุงเทพ"))
                            { TempDistrict = "เขต"; }
                            else { TempDistrict = "อำเภอ"; };
                        }

                        if (RegisterCusService_.DistrictOid != null)
                        {
                            item.DistrictOid = RegisterCusService_.DistrictOid.Oid.ToString();
                            item.DistrictName = RegisterCusService_.DistrictOid.DistrictNameTH;
                        }

                        if (RegisterCusService_.SubDistrictOid != null)
                        {
                            item.SubDistrictOid = RegisterCusService_.SubDistrictOid.Oid.ToString();
                            item.SubDistrictName = RegisterCusService_.SubDistrictOid.SubDistrictNameTH;
                        }

                        if (RegisterCusService_.ZipCode != null)
                        {
                            item.ZipCode = RegisterCusService_.ZipCode;
                        }

                        item.FullAddress = RegisterCusService_.FullAddress;

                        if (RegisterCusService_.Remark != null)
                        {
                            item.Remark = RegisterCusService_.Remark;
                        }

                        item.IsActive = RegisterCusService_.IsActive;
                        directProvider.Dispose();

                        return Request.CreateResponse(HttpStatusCode.OK, item);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "NoData");
                    }
                }
                else
                {
                    UserError err = new UserError();
                    err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                    err.message = "กรุณาระบุเลขบัตรประชาชน";
                    //  Return resual
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }

        public string checknull(object val)
        {
            object ret = "-";
            try
            {
                if (val != null || val.ToString() != string.Empty)
                {
                    ret = val;
                };
            }
            catch (Exception)
            {
                ret = "-";
            }
            return ret.ToString();
        }
    }
}