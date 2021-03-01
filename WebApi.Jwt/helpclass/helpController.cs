using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using nutrition.Module;
using nutrition.Module.EmployeeAsUserExample.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Jwt.Controllers;
using WebApi.Jwt.Models;

namespace WebApi.Jwt.helpclass
{
    public class helpController : ApiController
    {
        private string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        /// <summary>
        /// ฟังชั่นเช็ค user password ให้ถูกต้องตามรหัส xaf ที่สมัครไว้
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public WebApi.Jwt.Models.user.User_info CheckLogin_XAF(string Username, string Password) // value1 = Username, value2 = Password จาก class อื่น
        {
            user.User_info objUser_info = new user.User_info();
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(UserInfo));
                XafTypesInfo.Instance.RegisterEntity(typeof(RoleInfo));
                WebApi.Jwt.Models.user.member_info_Shot user2 = new WebApi.Jwt.Models.user.member_info_Shot();
                //XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc);
                using (XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc))
                {
                    using (IObjectSpace ObjectSpace = directProvider.CreateObjectSpace())
                    {
                        UserInfo User;
                        nutrition.Module.Organization DLD;
                        User = ObjectSpace.FindObject<UserInfo>(new BinaryOperator("UserName", Username));
                        // UserInfo = ObjectSpace.FindObject<RoleInfo>(new BinaryOperator("Name", Username));
                        PasswordCryptographer.EnableRfc2898 = true;
                        PasswordCryptographer.SupportLegacySha512 = false;
                        if (User.ComparePassword(Password) == true)
                        {
                            objUser_info.User_Name = User.UserName;
                            objUser_info.DisplayName = User.DisplayName;
                            objUser_info.OrganizationOid = User.Organization.Oid;
                            objUser_info.OrganizationNameTH = User.Organization.OrganizeNameTH;
                            objUser_info.SubOrganizeName = User.Organization.SubOrganizeName;
                            objUser_info.Tel = User.Organization.Tel;
                            objUser_info.Email = User.Organization.Email;
                            objUser_info.Address = User.Organization.Address;
                            objUser_info.Moo = User.Organization.Moo;
                            objUser_info.Soi = User.Organization.Soi;
                            objUser_info.Road = User.Organization.Road;
                            if (objUser_info.ProvinceNameTH == "")
                            {
                                objUser_info.ProvinceNameTH = "ไม่มีข้อมูลศูนย์";
                            }
                            else if (objUser_info.ProvinceNameTH != "")
                            {
                                objUser_info.ProvinceNameTH = User.Organization.ProvinceOid.ProvinceNameTH;
                            }
                            if (objUser_info.DistrictNameTH == "")
                            {
                                objUser_info.DistrictNameTH = "ไม่มีข้อมูลศูนย์";
                            }
                            else if (objUser_info.DistrictNameTH != "")
                            {
                                objUser_info.DistrictNameTH = User.Organization.DistrictOid.DistrictNameTH;
                            }
                            if (objUser_info.SubDistrictNameTH == "")
                            {
                                objUser_info.SubDistrictNameTH = "ไม่มีข้อมูลศูนย์";
                            }
                            else if (objUser_info.SubDistrictNameTH != "")
                            {
                                objUser_info.SubDistrictNameTH = User.Organization.SubDistrictOid.SubDistrictNameTH;
                            }

                            string TempSubDistrict, TempDistrict;
                            if (User.Organization.ProvinceOid.ProvinceNameTH.Contains("กรุงเทพ"))
                            { TempSubDistrict = "แขวง"; }
                            else
                            { TempSubDistrict = "ตำบล"; };

                            if (User.Organization.ProvinceOid.ProvinceNameTH.Contains("กรุงเทพ"))
                            { TempDistrict = "เขต"; }
                            else { TempDistrict = "อำเภอ"; };

                            objUser_info.FullAddress = User.Organization.Address + " หมู่ที่" + " " + checknull(User.Organization.Moo) + " ถนน" + checknull(User.Organization.Road) + " " +
                            TempSubDistrict + User.Organization.SubDistrictOid.SubDistrictNameTH + " " + TempDistrict + User.Organization.DistrictOid.DistrictNameTH + " " +
                            "จังหวัด" + User.Organization.ProvinceOid.ProvinceNameTH + " " + User.Organization.DistrictOid.PostCode;

                            DLD = ObjectSpace.FindObject<nutrition.Module.Organization>(new BinaryOperator("Oid", User.Organization.MasterOrganization));

                            if (DLD == null)
                            {
                                objUser_info.DLDName = "ไม่มีเขต";
                            }
                            else if (DLD != null)
                            {
                                objUser_info.DLDName = DLD.OrganizeNameTH;
                            }
                            objUser_info.DLDZone = User.Organization.ProvinceOid.DLDZone.Oid.ToString();
                            objUser_info.Latitude = User.Organization.Latitude;
                            objUser_info.Longitude = User.Organization.Longitude;
                            TokenController token = new TokenController();
                            objUser_info.Description = "ระบบ Login";
                            objUser_info.Token_key = token.Get(Username, Password);
                            objUser_info.Status = 1;
                            objUser_info.Message = "เข้าสู่ระบบสำเร็จ";
                            string AcName = "";
                            foreach (RoleInfo row2 in User.UserRoles)
                            {
                                switch (row2.Name)
                                {
                                    case "Approver":
                                        if (AcName == "")
                                        {
                                            AcName = "Approve";
                                        }
                                        else
                                        {
                                            AcName = AcName + "," + "Approve";
                                        }
                                        break;

                                    case "Operator":
                                        if (AcName == "")
                                        {
                                            AcName = "Edit";
                                        }
                                        else if (AcName.Contains("Edit") != true)
                                        {
                                            AcName = AcName + "," + "Edit";
                                        }
                                        break;

                                    case "Administrator":
                                        if (AcName == "")
                                        {
                                            AcName = "EditAdmin";
                                        }
                                        else if (AcName.Contains("EditAdmin") != true)
                                        {
                                            AcName = AcName + "," + "EditAdmin";
                                        }
                                        //else
                                        //{
                                        //    AcName = AcName + "," + "Edit";

                                        //}
                                        break;

                                    case "EditAdmin":
                                        if (AcName == "")
                                        {
                                            AcName = "EditAdmin";
                                        }
                                        else if (AcName.Contains("EditAdmin") != true)
                                        {
                                            AcName = AcName + "," + "EditAdmin";
                                        }
                                        break;


                                    default:
                                        if (AcName == "")
                                        {
                                            AcName = "ReadOnly";
                                        }
                                        else
                                        {
                                            if (AcName.Contains("ReadOnly") == false)
                                            {
                                                AcName = AcName + "," + "ReadOnly";
                                            }
                                        }
                                        break;
                                }
                            }

                            objUser_info.ActionName = AcName;

                            //List<WebApi.Jwt.Models.user.Roles_info> objListRoles_info = new List<WebApi.Jwt.Models.user.Roles_info>();

                            //if (AcName.Contains("Edit") == true || AcName.Contains("Administrator") == true )
                            //{
                            //    objUser_info.ActionName = "Edit"+ "Administrator";
                            //}
                            //else
                            //{
                            //    objUser_info.ActionName = AcName;
                            //}
                        }
                        else if (User.ComparePassword(Password) == false)
                        {
                            objUser_info.User_Name = User.UserName;
                            objUser_info.DisplayName = User.DisplayName;
                            objUser_info.OrganizationNameTH = User.Organization.OrganizeNameTH;
                            objUser_info.Tel = User.Organization.Tel;
                            objUser_info.Status = 0;
                            objUser_info.Message = "เข้าสู่ระบบไม่สำเร็จ";
                        }
                    }

                    directProvider.Dispose();
                }
                //IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
            }
            catch (Exception ex)
            {
                objUser_info.Status = 6;
                objUser_info.Message = ex.Message;
            }

            return objUser_info;
        }

        public string checknull(object val)
        {
            string ret = "-";
            try
            {
                if (val != null || val.ToString() != string.Empty)
                {
                    ret = val.ToString();
                };
            }
            catch (Exception ex)
            {
                ret = "-";
            }
            return ret;
        }

        public WebApi.Jwt.Models.user.get_role_byuser get_Roles(string Username)
        {
            user.get_role_byuser roles = new user.get_role_byuser();
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(UserInfo));
                XafTypesInfo.Instance.RegisterEntity(typeof(RoleInfo));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                UserInfo User;
                User = ObjectSpace.FindObject<UserInfo>(new BinaryOperator("UserName", Username));
                //nutrition.Module.CtlShowDispyName ctlshow;
                //ctlshow = ObjectSpace.FindObject<CtlShowDispyName>(new BinaryOperator("Oid", Username));

                if (User.UserName != null)
                {
                    roles.User_Name = User.UserName;
                    roles.Display_name = User.DisplayName;
                    //  roles.Role_name = User.UserRoles;

                    //{
                    List<user.Roles_info> get_Role_Byusers = new List<user.Roles_info>();

                    foreach (RoleInfo row in User.UserRoles)
                    {
                        user.Roles_info Userget = new user.Roles_info();
                        Userget.Role_display = row.DisplayName.ToString();
                        Userget.Role_Name = row.Name;
                        get_Role_Byusers.Add(Userget);
                    }
                    roles.objRoles_info = get_Role_Byusers;

                    roles.Status = 1;
                    roles.Message = "แสดงรายชื่อ User";
                }
                else
                {
                    roles.Status = 0;
                    roles.Message = "ไม่แสดงรายชื่อ User";
                }
            }
            catch (Exception ex)
            {
                roles.Status = 6;
                roles.Message = ex.Message;
            }
            return roles;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("FarmerProduction_XAF")]
        public HttpResponseMessage FarmerProduction_XAF()
        {
            try
            {
                XpoTypesInfoHelper.GetTypesInfo();
                XafTypesInfo.Instance.RegisterEntity(typeof(FarmerProduction));

                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<FarmerProduction> collection = ObjectSpace.GetObjects<FarmerProduction>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1", null));
                if (collection != null)
                {
                    List<user.FarmerProductionModel> list = new List<user.FarmerProductionModel>();
                    foreach (FarmerProduction row in collection)
                    {
                        user.FarmerProductionModel productionModel = new user.FarmerProductionModel();
                        productionModel.Oid = row.Oid;
                        productionModel.Production = row.AnimalSeedOid.SeedName;
                        list.Add(productionModel);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, list);
                }
                else
                {
                    UserError err = new UserError();
                    err.code = ""; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }
    //private CookieContainer cookie = new CookieContainer();

    //protected override WebRequest GetWebRequest(Uri address)
    //{
    //    WebRequest request = base.GetWebRequest(address);
    //    if (request is HttpWebRequest)
    //    {
    //        (request as HttpWebRequest).CookieContainer = cookie;
    //    }
    //    return request;
    //}

        public static string GetClientIp(HttpRequestMessage request)
        {
            var cookies = new CookieContainer();
            string ip = string.Empty;
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                HttpContextBase context = ((HttpContextBase)request.Properties["MS_HttpContext"]);
            
                if (context.Request.ServerVariables["HTTP_VIA"] != null)
                    ip = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                else
                    ip = context.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            return ip;
        }

    }
}

///เก็บไว้
///
//public user.get_role_byuser Userget_ByRole(string DisplayName)
//{
//    user.get_role_byuser user = new user.get_role_byuser();
//    try
//    {
//        XpoTypesInfoHelper.GetXpoTypeInfoSource();
//        XafTypesInfo.Instance.RegisterEntity(typeof(UserInfo));
//        XafTypesInfo.Instance.RegisterEntity(typeof(RoleInfo));
//        XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
//        IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
//        RoleInfo User;
//        User = ObjectSpace.FindObject <RoleInfo>(new BinaryOperator("DisplayName", DisplayName))
//          if (User.DisplayName != null)
//        {
//            user.User_Name = User.DisplayName;
//            roles.Display_name = User.DisplayName;
//            //  roles.Role_name = User.UserRoles;

//            //{
//            List<user.Roles_info> get_Role_Byusers = new List<user.Roles_info>();

//            foreach (RoleInfo row in User.UserRoles)
//            {
//                user.Roles_info Userget = new user.Roles_info();
//                Userget.Role_display = row.DisplayName;
//                Userget.Role_Name = row.Name;
//                get_Role_Byusers.Add(Userget);
//            }
//            roles.objRoles_info = get_Role_Byusers;

//            roles.Status = 1;
//            roles.Message = "แสดงรายชื่อ User";
//        }
//        else
//        {
//            roles.Status = 0;
//            roles.Message = "ไม่แสดงรายชื่อ User";
//        }

//    }
//}

//    public class CustomAuthentication :
//    {
//        private CustomLogonParameters customLogonParameters;
//        public CustomAuthentication()
//        {
//            customLogonParameters = new CustomLogonParameters();
//        }
//        public override void Logoff()
//        {
//            base.Logoff();
//            customLogonParameters = new CustomLogonParameters();
//        }
//        public override void ClearSecuredLogonParameters()
//        {
//            customLogonParameters.Password = "";
//            base.ClearSecuredLogonParameters();
//        }
//        public override object Authenticate(IObjectSpace objectSpace)
//        {
//            Employee employee = objectSpace.FindObject<Employee>(
//                new BinaryOperator("UserName", customLogonParameters.UserName));

//            if (employee == null)
//                throw new ArgumentNullException("Employee");

//            if (!employee.ComparePassword(customLogonParameters.Password))
//                throw new AuthenticationException(
//                    employee.UserName, "Password mismatch.");

//            return employee;
//        }

//        public override void SetLogonParameters(object logonParameters)
//        {
//            this.customLogonParameters = (CustomLogonParameters)logonParameters;
//        }

//        public override IList<Type> GetBusinessClasses()
//        {
//            return new Type[] { typeof(CustomLogonParameters) };
//        }
//        public override bool AskLogonParametersViaUI
//        {
//            get { return true; }
//        }
//        public override object LogonParameters
//        {
//            get { return customLogonParameters; }
//        }
//        public override bool IsLogoffEnabled
//        {
//            get { return true; }
//        }
//    }
//}

//       string Userinfo = "";

//       UserInfo = ObjectSpace.FindObject<UserInfo>(new BinaryOperator("StoredPassword", Password));

//public class RestorePasswordParameters : LogonActionParametersBase
//{
//    public override void ExecuteBusinessLogic(IObjectSpace objectSpace)
//    {
//        if (string.IsNullOrEmpty(Email))
//        {
//            throw new ArgumentException("Email address is not specified!");
//        }
//        IAuthenticationStandardUser user = objectSpace.FindObject(SecurityExtensionsModule.SecuritySystemUserType, CriteriaOperator.Parse("Email = ?", Email)) as IAuthenticationStandardUser;
//        IAuthenticationStandardUser user = objectSpace.FindObject(SecurityExtensionsModule.SecuritySystemUserType, CriteriaOperator.Parse("Email = ?", Email)) as IAuthenticationStandardUser;
//        if (user == null)
//        {
//            throw new ArgumentException("Cannot find registered users by the provided email address!");
//        }
//        byte[] randomBytes = new byte[6];
//        new RNGCryptoServiceProvider().GetBytes(randomBytes);
//        string password = Convert.ToBase64String(randomBytes);
//        Dennis: Resets the old password and generates a new one.
//        user.SetPassword(password);
//        user.ChangePasswordOnFirstLogon = true;
//        objectSpace.CommitChanges();
//        EmailLoginInformation(Email, password);
//    }
//    public static void EmailLoginInformation(string email, string password)
//    {
//        Dennis:
//        if (success)
//        {
//            Send an email with the login details here. Refer to http://msdn.microsoft.com/en-us/library/system.net.mail.mailmessage.aspx for more details.
//        }
//        else
//        {
//            throw new Exception("Failed!");
//        }
//    }
//}