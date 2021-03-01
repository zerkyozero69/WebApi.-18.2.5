using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using Microsoft.ApplicationBlocks.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using nutrition.Module;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using WebApi.Jwt.Models;
using static WebApi.Jwt.Models.Farmerinfo;
using static WebApi.Jwt.Models.Models_Masters.FarmerProduct_model;

namespace WebApi.Jwt.Controllers
{
    public class RegisterFarmerController : ApiController
    {
        // string connectionString =
        private string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString;

        // string sc2 = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        //string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        //    ConfigurationManager.ConnectionStrings["scc"].ConnectionString
        /// <summary>
        /// ลงทะเบียนเกษตรกร
        /// </summary>
        /// <returns></returns>
        //  [JwtAuthentication]
        [AllowAnonymous]
        [HttpPost]
        [Route("RegisterFarmer")]

        #region ใช้เพิ่มข้อมูล แก้ไข ข้อมูลเกษตรกร

        public HttpResponseMessage RegisterFarmer()
        {
            _Registerfarmer Registerfarmer = new _Registerfarmer();
            try
            {
                string requestString = Request.Content.ReadAsStringAsync().Result;
                JObject jObject = (JObject)JsonConvert.DeserializeObject(requestString);
                string TempForageType = string.Empty;

                if (jObject != null)
                {


                    Registerfarmer.OrganizationOid = jObject.SelectToken("OrganizationOid").Value<string>();
                    Registerfarmer.CitizenID = jObject.SelectToken("CitizenID").Value<Int64>();
                    if (jObject.SelectToken("CitizenID") != null)
                    {
                        RegisterFarmerController best = new RegisterFarmerController();
                        if (best.CheckCitizenID(jObject.SelectToken("CitizenID").ToString()) == false)
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "หมายเลขบัตรประชาชนไม่ถูกต้อง กรุณาตรวจสอบ");
                        }
                        int intCitizenID;

                        if (int.TryParse(jObject.SelectToken("CitizenID").ToString(), out intCitizenID))
                        {
                            Registerfarmer.CitizenID = intCitizenID;
                        }
                
                        // Registerfarmer.BirthDate =jObject.SelectToken("BirthDate").Value<DateTime>().ToString();"dd-MM-yyyy", new CultureInfo("us-US")
                        if (jObject.SelectToken("BirthDate").ToString() != null)
                        {
                            Registerfarmer.BirthDate = jObject.SelectToken("BirthDate").Value<string>();
                        }

                        Registerfarmer.TitleOid = jObject.SelectToken("TitleOid").Value<string>();
                        Registerfarmer.FirstNameTH = jObject.SelectToken("FirstNameTH").Value<string>();
                        Registerfarmer.LastNameTH = jObject.SelectToken("LastNameTH").Value<string>();

                        Registerfarmer.GenderOid = jObject.SelectToken("GenderOid").Value<string>();
                        Registerfarmer.Tel = jObject.SelectToken("Tel").Value<string>();

                        if (jObject.SelectToken("Email") == null)
                        {
                            Registerfarmer.Email = string.Empty;
                        }
                        else
                        {
                            Registerfarmer.Email = jObject.SelectToken("Email").Value<string>();
                        }

                        Registerfarmer.Address = jObject.SelectToken("Address").Value<string>();

                        if (jObject.SelectToken("Moo") == null)
                        {
                            Registerfarmer.Moo = string.Empty;
                        }
                        else
                        {
                            Registerfarmer.Moo = jObject.SelectToken("Moo").Value<string>();
                        }

                        if (jObject.SelectToken("Soi") == null)
                        {
                            Registerfarmer.Soi = string.Empty;
                        }
                        else
                        {
                            Registerfarmer.Soi = jObject.SelectToken("Soi").Value<string>();
                        }

                        if (jObject.SelectToken("Road") == null)
                        {
                            Registerfarmer.Road = string.Empty;
                        }
                        else
                        {
                            Registerfarmer.Road = jObject.SelectToken("Road").Value<string>();
                        }

                        Registerfarmer.ProvinceOid = jObject.SelectToken("provinceOid").Value<string>();
                        Registerfarmer.DistrictOid = jObject.SelectToken("districtOid").Value<string>();
                        Registerfarmer.SubDistrictOid = jObject.SelectToken("SubDistrictOid").Value<string>();
                        Registerfarmer.ZipCode = jObject.SelectToken("zipCode").Value<string>();
                        //Registerfarmer.ForageTypeOid = TempForageType;
                        if ("ForageTypeName1" != null || "ForageTypeName2" != null || "ForageTypeName3" != null || "ForageTypeName4" != null)
                        {
                            JObject jObject_Forage = JObject.Parse(jObject.ToString());

                            JArray Arr_Forage = (JArray)jObject_Forage["ForageTypes"];

                            IList<ForageTypeModel> ForageT = Arr_Forage.ToObject<IList<ForageTypeModel>>();
                            foreach (ForageTypeModel row in ForageT)
                            {
                                if (TempForageType == string.Empty)
                                {
                                    TempForageType = row.Oid;
                                }
                                else
                                {
                                    TempForageType = TempForageType + ',' + row.Oid;
                                }
                            }
                        }


                        if (jObject.SelectToken("latitude") != null || jObject.SelectToken("latitude").ToString() != string.Empty)

                        {
                            Registerfarmer.Latitude = jObject.SelectToken("latitude").Value<float>();
                        }

                        if (jObject.SelectToken("longitude") != null || jObject.SelectToken("longitude").ToString() != string.Empty)
                        {
                            Registerfarmer.Longitude = jObject.SelectToken("longitude").Value<float>();
                        }

                        Registerfarmer.Register_Type = 1;
                    }
                }
                DataSet ds;
                SqlParameter[] prm = new SqlParameter[21];
                if (prm != null)
                {
                    prm[0] = new SqlParameter("@OrganizationOid", Registerfarmer.OrganizationOid); ///ต้องระบุชื่อศูนย์ที่จะสมัคร ถึงขึ้นที่หน้าเว็บ
                    prm[1] = new SqlParameter("@Citizen_ID", Registerfarmer.CitizenID);
                    prm[2] = new SqlParameter("@TitleOid", Registerfarmer.TitleOid);
                    prm[3] = new SqlParameter("@FirstName_TH", Registerfarmer.FirstNameTH);
                    prm[4] = new SqlParameter("@LastName_TH", Registerfarmer.LastNameTH);
                    prm[5] = new SqlParameter("@Birthdate", Registerfarmer.BirthDate);
                    prm[6] = new SqlParameter("@Gender", Registerfarmer.GenderOid);
                    prm[7] = new SqlParameter("@Tel", Registerfarmer.Tel);
                    prm[8] = new SqlParameter("@Email", Registerfarmer.Email);
                    prm[9] = new SqlParameter("@Address_No", Registerfarmer.Address);
                    prm[10] = new SqlParameter("@Address_moo", Registerfarmer.Moo);
                    prm[11] = new SqlParameter("@Address_Soi", Registerfarmer.Soi);
                    prm[12] = new SqlParameter("@Address_Road", Registerfarmer.Road);
                    prm[13] = new SqlParameter("@Address_provinces", Registerfarmer.ProvinceOid);
                    prm[14] = new SqlParameter("@Address_districts", Registerfarmer.DistrictOid);
                    prm[15] = new SqlParameter("@Address_subdistricts", Registerfarmer.SubDistrictOid);
                    prm[16] = new SqlParameter("@ZipCode", Registerfarmer.ZipCode);
                    prm[17] = new SqlParameter("@ForageTypeOid", TempForageType); // Registerfarmer.ForageTypeOid
                    prm[18] = new SqlParameter("@Latitude", Registerfarmer.Latitude);
                    prm[19] = new SqlParameter("@Longitude", Registerfarmer.Longitude);
                    prm[20] = new SqlParameter("@Register_Type", Registerfarmer.Register_Type);


                    SqlHelper.ExecuteNonQuery(scc, CommandType.StoredProcedure, "spt_Moblieinsert_RegisterFarmer", prm);
                    DataTable dt = new DataTable();

                    string Register_Type = "";

                    Farmer_Status Farmer = new Farmer_Status();
                    Register_Type = TempForageType;
                    Farmer.Message = "ลงทะเบียนสำเร็จ ";

                    //return Request.CreateResponse(HttpStatusCode.OK);
                    return Request.CreateResponse(true);
                }
                //else if (ds.Tables[0].Rows[0]["pStatus"].ToString() == "99")
                //{
                //    UserError err = new UserError();
                //    err.code = "2"; // กรอกข้อมูลซ้ำ
                //    err.message = "บุคคลนี้เคยลงทะเบียนไปแล้ว โปรดตรวจสอบ ";
                //    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                //    return Request.CreateResponse(false);
                //}
                else
                {
                    UserError err = new UserError();
                    err.code = "3"; // กรอกข้อมูลไม่ครบ
                    err.message = "กรอกข้อมูลไม่ครบ ";
                    //return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    return Request.CreateResponse(false);
                }

            }
            catch (Exception ex)
            {
                //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.status = "ผิดพลาด";
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }

        /////// <summary>
        /////// แก้ไขข้อมูล
        /////// </summary>
        /////// <returns></returns>
        //[AllowAnonymous]
        //[HttpPost]
        //[Route("Farmer/Update")]
        //public HttpResponseMessage UpdateRegisterFarmer()
        //{
        //    _Registerfarmer Updatefarmer = new _Registerfarmer();

        //    try
        //    {
        //        string requestString = Request.Content.ReadAsStringAsync().Result;
        //        JObject jObject = (JObject)JsonConvert.DeserializeObject(requestString);
        //        string TempForageType = string.Empty;
        //        if (jObject != null)
        //        {
        //            Updatefarmer.OrganizationOid = jObject.SelectToken("OrganizationOid").Value<string>();
        //            Updatefarmer.CitizenID = jObject.SelectToken("CitizenID").Value<Int64>();
        //            if (jObject.SelectToken("CitizenID") != null)
        //            {
        //                int intCitizenID;
        //                if (int.TryParse(jObject.SelectToken("CitizenID").ToString(), out intCitizenID))
        //                {
        //                    Updatefarmer.CitizenID = intCitizenID;
        //                }
        //                Updatefarmer.TitleOid = jObject.SelectToken("TitleOid").Value<string>();
        //                Updatefarmer.FirstNameTH = jObject.SelectToken("FirstNameTH").Value<string>();
        //                Updatefarmer.LastNameTH = jObject.SelectToken("LastNameTH").Value<string>();
        //                Updatefarmer.BirthDate = jObject.SelectToken("BirthDate").Value<string>();

        //                Updatefarmer.GenderOid = jObject.SelectToken("GenderOid").Value<string>();
        //                Updatefarmer.Tel = jObject.SelectToken("Tel").Value<string>();

        //                if (jObject.SelectToken("Email") != null)

        //                {
        //                    Updatefarmer.Email = jObject.SelectToken("Email").Value<string>();
        //                }

        //                Updatefarmer.Address = jObject.SelectToken("Address").Value<string>();

        //                if (jObject.SelectToken("Moo") != null)

        //                {
        //                    Updatefarmer.Moo = jObject.SelectToken("Moo").Value<string>();
        //                }

        //                if (jObject.SelectToken("Soi") != null)

        //                {
        //                    Updatefarmer.Soi = jObject.SelectToken("Soi").Value<string>();
        //                }

        //                if (jObject.SelectToken("Road") != null)

        //                {
        //                    Updatefarmer.Road = jObject.SelectToken("Road").Value<string>();
        //                }

        //                Updatefarmer.ProvinceOid = jObject.SelectToken("ProvinceOid").Value<string>();
        //                Updatefarmer.DistrictOid = jObject.SelectToken("DistrictOid").Value<string>();
        //                Updatefarmer.SubDistrictOid = jObject.SelectToken("SubDistrictOid").Value<string>();
        //                Updatefarmer.ZipCode = jObject.SelectToken("ZipCode").Value<string>();
        //                //Registerfarmer.ForageTypeOid = TempForageType;

        //                JObject jObject_Forage = JObject.Parse(jObject.ToString());
        //                JArray Arr_Forage = (JArray)jObject_Forage["ForageTypes"];

        //                IList<ForageTypeModel> ForageT = Arr_Forage.ToObject<IList<ForageTypeModel>>();
        //                foreach (ForageTypeModel row in ForageT)
        //                {
        //                    if (TempForageType == string.Empty)
        //                    {
        //                        TempForageType = row.Oid;
        //                    }
        //                    else
        //                    {
        //                        TempForageType = TempForageType + ',' + row.Oid;
        //                    }
        //                }

        //                if (jObject.SelectToken("latitude") == null || jObject.SelectToken("latitude").ToString() == string.Empty)

        //                    Updatefarmer.Latitude = 0;
        //                }
        //                else
        //                {
        //                    Updatefarmer.Latitude = jObject.SelectToken("latitude").Value<float>();
        //                }

        //                if (jObject.SelectToken("longitude") == null || jObject.SelectToken("longitude").ToString() == string.Empty)
        //                {
        //                    Updatefarmer.Longitude = 0;
        //                }
        //                else
        //                {
        //                    Updatefarmer.Longitude = jObject.SelectToken("longitude").Value<float>();
        //                }

        //                Updatefarmer.Register_Type = 2;
        //            }
        //        }

        //        DataSet ds = new DataSet();
        //        SqlParameter[] prm = new SqlParameter[21];
        //        prm[0] = new SqlParameter("@OrganizationOid", Updatefarmer.OrganizationOid);
        //        prm[1] = new SqlParameter("@Citizen_ID", Updatefarmer.CitizenID);
        //        prm[2] = new SqlParameter("@TitleOid", Updatefarmer.TitleOid);
        //        prm[3] = new SqlParameter("@FirstName_TH", Updatefarmer.FirstNameTH);
        //        prm[4] = new SqlParameter("@LastName_TH", Updatefarmer.LastNameTH);
        //        prm[5] = new SqlParameter("@Birthdate", Updatefarmer.BirthDate);
        //        prm[6] = new SqlParameter("@Gender", Updatefarmer.GenderOid);
        //        prm[7] = new SqlParameter("@Tel", Updatefarmer.Tel);
        //        prm[8] = new SqlParameter("@Email", Updatefarmer.Email);
        //        prm[9] = new SqlParameter("@Address_No", Updatefarmer.Address);
        //        prm[10] = new SqlParameter("@Address_moo", Updatefarmer.Moo);
        //        prm[11] = new SqlParameter("@Address_Soi", Updatefarmer.Soi);
        //        prm[12] = new SqlParameter("@Address_Road", Updatefarmer.Road);
        //        prm[13] = new SqlParameter("@Address_provinces", Updatefarmer.ProvinceOid);
        //        prm[14] = new SqlParameter("@Address_districts", Updatefarmer.DistrictOid);
        //        prm[15] = new SqlParameter("@Address_subdistricts", Updatefarmer.SubDistrictOid);
        //        prm[16] = new SqlParameter("@ZipCode", Updatefarmer.ZipCode);
        //        prm[17] = new SqlParameter("@ForageTypeOid", TempForageType);//Updatefarmer.ForageTypeOid);
        //        prm[18] = new SqlParameter("@Latitude", Updatefarmer.Latitude);
        //        prm[19] = new SqlParameter("@Longitude", Updatefarmer.Longitude);
        //        prm[20] = new SqlParameter("@Register_Type", Updatefarmer.Register_Type);
        //        ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_Moblieinsert_RegisterFarmer", prm);
        //        if (prm[1].Value != null)
        //        {
        //            if (ds.Tables[0].Rows[0]["pStatus"].ToString() != "0")
        //            {
        //                //return Request.CreateResponse(HttpStatusCode.OK, "[" + Updatefarmer.@FirstNameTH + "  " + Updatefarmer.@LastNameTH + "] " + "แก้ไขข้อมูลสำเร็จแล้ว");
        //                return Request.CreateResponse(true);
        //            }
        //            else if (ds.Tables[0].Rows[0]["pStatus"].ToString() == "0")
        //            {
        //                //return Request.CreateResponse(HttpStatusCode.BadRequest, "กรอกข้อมูลผิด");
        //                return Request.CreateResponse(false);
        //            }
        //            {
        //                //return Request.CreateResponse(HttpStatusCode.BadRequest, "ไม่มีข้อมูล");
        //                return Request.CreateResponse(false);
        //            }
        //        }
        //        {
        //            //return Request.CreateResponse(HttpStatusCode.BadRequest, "กรุณาใส่เลขบัตร");
        //            return Request.CreateResponse(false);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //Error case เกิดข้อผิดพลาด
        //        UserError err = new UserError();
        //        err.status = "ผิดพลาด";
        //        err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

        //        err.message = ex.Message;
        //        //  Return resual
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
        //    }
        //}

        /// <summary>
        /// ค้นหาเรียกรายชื่อเกษตรกร
        /// </summary>
        /// <param name="CitizenID"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("get/profileFarmer")]
        public HttpResponseMessage loadprofileFarmer_ByCitizenID()
        {
            try
            {
                string CitizenID = null;
                if (HttpContext.Current.Request.Form["CitizenID"].ToString() != null)
                {
                    CitizenID = HttpContext.Current.Request.Form["CitizenID"].ToString();
                }

                DataSet ds = new DataSet();

                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MobileGetCitizenID", new SqlParameter("@CitizenID", CitizenID)
                   );

                if (ds.Tables[0].Rows.Count > 0)
                {
                    Profile_Farmer profile = new Profile_Farmer();
                    profile.Oid = ds.Tables[0].Rows[0]["Oid"].ToString();
                    profile.CitizenID = ds.Tables[0].Rows[0]["OrganizeNameTH"].ToString();
                    profile.Title = ds.Tables[0].Rows[0]["CitizenID"].ToString();
                    profile.FirstNameTH = ds.Tables[0].Rows[0]["TitleName"].ToString();
                    profile.LastNameTH = ds.Tables[0].Rows[0]["FirstNameTH"].ToString();
                    profile.Gender = ds.Tables[0].Rows[0]["LastNameTH"].ToString();
                    profile.Gender = ds.Tables[0].Rows[0]["GenderName"].ToString();
                    profile.BirthDate = ds.Tables[0].Rows[0]["BirthDate"].ToString();
                    profile.Tel = ds.Tables[0].Rows[0]["Tel"].ToString();
                    profile.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                    profile.IsActive = ds.Tables[0].Rows[0]["IsActive"].ToString();
                    profile.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                    profile.Moo = ds.Tables[0].Rows[0]["Moo"].ToString();
                    profile.Soi = ds.Tables[0].Rows[0]["Soi"].ToString();
                    profile.Road = ds.Tables[0].Rows[0]["Road"].ToString();
                    profile.Province = ds.Tables[0].Rows[0]["ProvinceNameTH"].ToString();
                    profile.District = ds.Tables[0].Rows[0]["DistrictNameTH"].ToString();
                    profile.SubDistrict = ds.Tables[0].Rows[0]["SubDistrictNameTH"].ToString();
                    profile.ZipCode = ds.Tables[0].Rows[0]["ZipCode"].ToString();
                    return Request.CreateResponse(HttpStatusCode.OK, profile);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "ไม่มีเลขบัตรประชาชน");
                }
            }
            catch (Exception ex)
            {
                //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.status = "ผิดพลาด";
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, err);
            }
            // return Request.CreateResponse(HttpStatusCode.GatewayTimeout, 0);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("TestPostMethod")]
        public HttpResponseMessage TestPostMethod()
        {
            string requestString = Request.Content.ReadAsStringAsync().Result;
            JObject jObject = (JObject)JsonConvert.DeserializeObject(requestString);
            //FirstName
            string Oid = jObject.SelectToken("Oid").Value<string>();
            string firstName = jObject.SelectToken("FirstName").Value<string>();
            //Lastname
            string lastName = jObject.SelectToken("LastName").Value<string>();
            //Age
            int age = jObject.SelectToken("Age").Value<int>();
            //Title
            string title = jObject.SelectToken("Title").Value<string>();
            //if (jObject.SelectToken("Title") != null)
            //{
            //    string titleName = jObject.SelectToken("Title").SelectToken("TitleName").Value<string>();
            //    int titleID = jObject.SelectToken("Title").SelectToken("ID").Value<int>();
            //}
            DataSet ds;
            SqlParameter[] prm = new SqlParameter[4];

            prm[0] = new SqlParameter("@FirstName_TH", firstName);
            prm[1] = new SqlParameter("@LastName_TH", lastName);
            prm[2] = new SqlParameter("@age", age);
            prm[3] = new SqlParameter("@title", title);

            ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "testinsertTBL", prm);
            if (ds.Tables[0].Rows[0]["pMessage"].ToString() == "success")
            {
                return Request.CreateResponse(HttpStatusCode.OK, "พร้อมแล้ว");
            }
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "ว๊ายไม่ลง");
            }
        }

        /// <summary>
        /// ค้นหาเลขบัตรประชาชน จากฐานเรา และฐาน TGS ใช้ตัวนี้
        /// </summary>
        /// <returns></returns>
        //[AllowAnonymous]
        //// [JwtAuthentication]
        //[HttpPost]
        //[Route("FarmerCitizenID")]
        //public HttpResponseMessage get_CitizenID()
        //{
        //    List<FarmerCitizen> farmerCitizenList = new List<FarmerCitizen>();
        //    FarmerCitizen farmer_info = new FarmerCitizen();

        //    try
        //    {
        //        string CitizenID = string.Empty;
        //        if (HttpContext.Current.Request.Form["CitizenID"].ToString() != null)
        //        {
        //            CitizenID = HttpContext.Current.Request.Form["CitizenID"].ToString();
        //        }
        //        DataSet ds = new DataSet();
        //        XpoTypesInfoHelper.GetXpoTypeInfoSource();
        //        XafTypesInfo.Instance.RegisterEntity(typeof(Farmer));
        //        XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
        //        IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
        //        ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MobileGetCitizenID", new SqlParameter("@CitizenID", CitizenID));
        //        if (ds.Tables.Count > 0)
        //        {
        //            List<Farmer_Modelinfo> Farmer_Modelinfo = new List<Farmer_Modelinfo>();
        //            Farmer_Modelinfo Farmer_info = new Farmer_Modelinfo();
        //            Farmer_info.Oid = ds.Tables[0].Rows[0]["Oid"].ToString();
        //            Farmer_info.OrganizationOid = ds.Tables[0].Rows[0]["OrganizationOid"].ToString();
        //            Farmer_info.OrganizeNameTH = ds.Tables[0].Rows[0]["OrganizeNameTH"].ToString();
        //            Farmer_info.CitizenID = ds.Tables[0].Rows[0]["CitizenID"].ToString();
        //            Farmer_info.TitleOid = ds.Tables[0].Rows[0]["TitleOid"].ToString();
        //            Farmer_info.TitleName = ds.Tables[0].Rows[0]["TitleName"].ToString();
        //            Farmer_info.GenderName = ds.Tables[0].Rows[0]["GenderName"].ToString();
        //            Farmer_info.FirstNameTH = ds.Tables[0].Rows[0]["FirstNameTH"].ToString();
        //            Farmer_info.LastNameTH = ds.Tables[0].Rows[0]["LastNameTH"].ToString();
        //            Farmer_info.GenderOid = ds.Tables[0].Rows[0]["GenderOid"].ToString();

        //            if (ds.Tables[0].Rows[0]["BirthDate"].ToString() != "")
        //            {
        //                Farmer_info.BirthDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["BirthDate"]).ToString("dd/MM/yyyy");
        //            }

        //            Farmer_info.Tel = ds.Tables[0].Rows[0]["Tel"].ToString();
        //            Farmer_info.Email = ds.Tables[0].Rows[0]["Email"].ToString();
        //            Farmer_info.IsActive = ds.Tables[0].Rows[0]["IsActive"].ToString();
        //            Farmer_info.DisPlayName = ds.Tables[0].Rows[0]["DisPlayName"].ToString();
        //            Farmer_info.Address = ds.Tables[0].Rows[0]["Address"].ToString();
        //            Farmer_info.Moo = ds.Tables[0].Rows[0]["Moo"].ToString();
        //            Farmer_info.Soi = ds.Tables[0].Rows[0]["Soi"].ToString();
        //            Farmer_info.Road = ds.Tables[0].Rows[0]["Road"].ToString();
        //            Farmer_info.ProvinceOid = ds.Tables[0].Rows[0]["ProvinceOid"].ToString();
        //            Farmer_info.ProvinceNameTH = ds.Tables[0].Rows[0]["ProvinceNameTH"].ToString();
        //            Farmer_info.DistrictOid = ds.Tables[0].Rows[0]["DistrictOid"].ToString();
        //            Farmer_info.DistrictNameTH = ds.Tables[0].Rows[0]["DistrictNameTH"].ToString();
        //            Farmer_info.SubDistrictOid = ds.Tables[0].Rows[0]["SubDistrictOid"].ToString();
        //            Farmer_info.SubDistrictNameTH = ds.Tables[0].Rows[0]["SubDistrictNameTH"].ToString();
        //            Farmer_info.ZipCode = ds.Tables[0].Rows[0]["ZipCode"].ToString();
        //            Farmer_info.FullAddress = ds.Tables[0].Rows[0]["FullAddress"].ToString();
        //            Farmer_info.Latitude = ds.Tables[0].Rows[0]["Latitude"].ToString();
        //            Farmer_info.Longitude = ds.Tables[0].Rows[0]["Longitude"].ToString();
        //            Farmer_info.Rigister_Type = ds.Tables[0].Rows[0]["RegisterType"].ToString();
        //            Farmer_info.FarmerGroupsOid = ds.Tables[0].Rows[0]["FarmerGroupsOid"].ToString();
        //            Farmer_info.Status = ds.Tables[0].Rows[0]["Status"].ToString();
        //            Farmer_info.ForageTypeName1 = null;
        //            Farmer_info.ForageTypeName2 = null;
        //            Farmer_info.ForageTypeName3 = null;
        //            Farmer_info.ForageTypeName4 = null;

        //            if (ds.Tables[0].Rows[0]["ForageTypeName"].ToString().Contains("เมล็ด"))
        //            {
        //                Farmer_info.ForageTypeName1 = "เมล็ดพันธุ์";
        //            }

        //            if (ds.Tables[0].Rows[0]["ForageTypeName"].ToString().Contains("เสบียง"))
        //            {
        //                Farmer_info.ForageTypeName2 = "เสบียงสัตว์";
        //            }

        //            if (ds.Tables[0].Rows[0]["ForageTypeName"].ToString().Contains("ท่อน"))
        //            {
        //                Farmer_info.ForageTypeName3 = "ท่อนพันธุ์";
        //            }
        //            Farmer_Modelinfo.Add(Farmer_info);
        //            return Request.CreateResponse(HttpStatusCode.OK, Farmer_info);
        //            //if (ds.Tables[0].Rows[0]["ForageTypeName"].ToString() != null)
        //            //{
        //            //    //string[] arr = RefNo.Split('|');
        //            //    //string _refno = arr[0]; //เลขที่อ้างอิง
        //            //    //string _org_oid = arr[1]; //oid หน่วยงาน
        //            //    //string _type = arr[2]; //ประเภทส่ง(2)-รับ(1)
        //            //    string[] arr = null;
        //            //    arr = ds.Tables[0].Rows[0]["ForageTypeName"].ToString().Split(',');
        //            //    if (arr.Length == 1)
        //            //    {
        //            //        Farmer_info.ForageTypeName1 = arr[0];
        //            //    }
        //            //    else if (arr.Length == 2)
        //            //    {
        //            //        Farmer_info.ForageTypeName1 = arr[0];
        //            //        Farmer_info.ForageTypeName2 = arr[1];
        //            //    }
        //            //    else if (arr.Length == 3)
        //            //    {
        //            //        Farmer_info.ForageTypeName1 = arr[0];
        //            //        Farmer_info.ForageTypeName2 = arr[1];
        //            //        Farmer_info.ForageTypeName3 = arr[2];
        //            //    }
        //            //    else if (arr.Length == 4)
        //            //    {
        //            //        Farmer_info.ForageTypeName1 = arr[0];
        //            //        Farmer_info.ForageTypeName2 = arr[1];
        //            //        Farmer_info.ForageTypeName3 = arr[2];
        //            //        Farmer_info.ForageTypeName4 = arr[3];
        //            //    }

        //            //}

        //            //    return Request.CreateResponse(HttpStatusCode.OK, Farmer_Modelinfo);
        //        }
        //        else if (ds.Tables.Count == 0)
        //        {
        //            string param = "username=regislive01&password=password&grant_type=password";//เพื่อทำการขอ access_token
        //            byte[] dataStream = Encoding.UTF8.GetBytes(param);
        //            string AuthParam = "regislive:password";
        //            string authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(AuthParam));
        //            var request = (HttpWebRequest)WebRequest.Create("http://regislives.dld.go.th:9080/regislive_authen/oauth/token");
        //            request.Method = "POST";
        //            request.ContentType = "application/x-www-form-urlencoded";
        //            request.ContentLength = dataStream.Length;
        //            request.Headers.Add("Authorization", "Basic " + authInfo);
        //            using (var stream = request.GetRequestStream())
        //            {
        //                stream.Write(dataStream, 0, dataStream.Length);
        //            }
        //            var response = (HttpWebResponse)request.GetResponse();
        //            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd().ToString();
        //            var jsonResulttodict = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseString);
        //            var access_token = jsonResulttodict["access_token"];

        //            //    'Get Data By Token
        //            request = (HttpWebRequest)WebRequest.Create("http://regislives.dld.go.th:9080/regislive_webservice/farmer/findbyPid?pid=" + CitizenID);
        //            request.Method = "GET";
        //            request.ContentType = "application/x-www-form-urlencoded";
        //            request.Headers.Add("Authorization", "Bearer " + access_token);
        //            response = (HttpWebResponse)request.GetResponse();
        //            var xreader = new StreamReader(response.GetResponseStream()).ReadToEnd();
        //            var farmerResul = JsonConvert.DeserializeObject<Dictionary<string, object>>(xreader);
        //            if (xreader != "")
        //            {
        //                farmer_info.CitizenID = farmerResul["pid"];
        //                farmer_info.titleName = farmerResul["prefixNameTh"];
        //                farmer_info.FirstNameTH = farmerResul["firstName"];
        //                farmer_info.LastNameTH = farmerResul["lastName"];
        //                if (farmerResul["genderName"] == null)
        //                {
        //                    farmer_info.genderName = "";
        //                }
        //                else
        //                {
        //                    farmer_info.genderName = farmerResul["genderName"];
        //                }

        //                farmer_info.birthDate = farmerResul["birthDay"];
        //                farmer_info.tel = farmerResul["phone"];
        //                farmer_info.email = farmerResul["email"];
        //                farmer_info.address = farmerResul["homeNo"];
        //                farmer_info.moo = farmerResul["moo"];
        //                farmer_info.soi = farmerResul["soi"];
        //                farmer_info.road = farmerResul["road"];
        //                farmer_info.provinceNameTH = farmerResul["provinceName"];
        //                farmer_info.districtNameTH = farmerResul["amphurName"];
        //                farmer_info.subDistrictNameTH = farmerResul["tambolName"];
        //                farmer_info.PostCode = farmerResul["postCode"];
        //                farmer_info.latitude = farmerResul["latitude"];
        //                farmer_info.longitude = farmerResul["longitude"];

        //                farmerCitizenList.Add(farmer_info);

        //                return Request.CreateResponse(HttpStatusCode.OK, farmerCitizenList);
        //            }
        //            else
        //            {
        //                UserError err3 = new UserError();
        //                err3.status = "ไม่พบเลขบัตรประชาชน กรุณาลงทะเบียน";
        //                err3.code = "-99"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

        //                // Return resual
        //                return Request.CreateResponse(HttpStatusCode.NotFound, err3);
        //            }

        //        }
        //        else
        //        {
        //            UserError err = new UserError();
        //            err.status = "ไม่พบเลขบัตรประชาชนในระบบ กรุณาลงทะเบียน";
        //            err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

        //            //Return resual
        //            return Request.CreateResponse(HttpStatusCode.NotFound, err);
        //        }



        //        //if()

        //    }
        //    catch (Exception ex)
        //    {
        //        //Error case เกิดข้อผิดพลาด
        //        UserError err = new UserError();
        //        err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

        //        err.message = ex.Message;
        //        //  Return resual
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
        //    }
        //    finally
        //    {
        //        SqlConnection.ClearAllPools();
        //    }
        //}

        #region ทดสอบ RAW

        //[AllowAnonymous]
        //[HttpGet]
        //[Route("RAW/TestPostMethod")]
        //public HttpResponseMessage TestgetMethod()
        //{
        //    string requestString = Request.Content.ReadAsStringAsync().Result;
        //    JObject jObject = (JObject)JsonConvert.DeserializeObject(requestString);
        //    //FirstName
        //    string firstName = jObject.SelectToken("FirstName").Value<string>();
        //    //Lastname
        //    string lastName = jObject.SelectToken("LastName").Value<string>();
        //    //Age
        //    int age = jObject.SelectToken("Age").Value<int>();
        //    //Title
        //    object title = jObject.SelectToken("Title").Value<object>();
        //     if (jObject.SelectToken("Title") != null)
        //    {
        //       string titleName = jObject.SelectToken("Title").SelectToken("TitleName").Value<string>();
        //       int titleID = jObject.SelectToken("Title").SelectToken("ID").Value<int>();
        //    }
        //    DataSet ds;
        //    SqlParameter[] prm = new SqlParameter[4];

        //    prm[0] = new SqlParameter("@FirstName_TH", firstName);
        //    prm[1] = new SqlParameter("@LastName_TH", lastName);
        //    prm[2] = new SqlParameter("@age", age);
        //    prm[3] = new SqlParameter("@title", title);

        //    ds = SqlHelper.ExecuteDataset(scc, CommandType.Text, "select * from testTBL where FirstName_TH='" + firstName + "' " );

        //    return Request.CreateResponse(HttpStatusCode.OK, ds.Tables[0]);
        //}
        /// <summary>
        ///  CitizenID ใช้ในการลบ farmer
        /// </summary>
        /// <returns></returns>
        ///

        #endregion ทดสอบ RAW

        [AllowAnonymous]
        [HttpDelete]
        [Route("Delete/FarmerOid")]
        public HttpResponseMessage Delete_Farmer()
        {
            _Delete_Farmer delete_Farmer = new _Delete_Farmer();
            try

            {
                if (HttpContext.Current.Request.Form["Oid"].ToString() != null)
                {
                    delete_Farmer.Oid = HttpContext.Current.Request.Form["Oid"].ToString();
                }
                DataSet ds = new DataSet();

                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MoblieDeleteFarmer_byOid", new SqlParameter("@Oid", delete_Farmer.Oid)
                   );
                if (ds.Tables[0].Rows[0]["pMessage"].ToString() == "ลบข้อมูลสำเร็จ")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, " ลบข้อมูลแล้ว");
                }
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "ไม่เจอเลขบัตรประชาชนหรือไม่มีข้อมูล");
                }
            }
            catch (Exception ex)
            {
                //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.status = "ผิดพลาด";
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, err);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Farmer/byprovince")]
        public HttpResponseMessage GetFarmer_ByProvince()
        {
            string Remark = "";
            string ProvinceOid = "";
            string Firstname = "";
            string LastName = "";
            List<Farmer_Modelinfo_province> Farmer_Modelinfo = new List<Farmer_Modelinfo_province>();

            try
            {
                {
                    if (HttpContext.Current.Request.Form["ProvinceOid"] != null)
                    {
                        ProvinceOid = HttpContext.Current.Request.Form["ProvinceOid"].ToString();
                    }
                    if (HttpContext.Current.Request.Form["Firstname"] != null)
                    {
                        Firstname = HttpContext.Current.Request.Form["Firstname"].ToString();
                    }
                    if (HttpContext.Current.Request.Form["LastName"] != null)
                    {
                        LastName = HttpContext.Current.Request.Form["LastName"].ToString();
                    }

                    XpoTypesInfoHelper.GetXpoTypeInfoSource();
                    XafTypesInfo.Instance.RegisterEntity(typeof(Farmer));
                    XafTypesInfo.Instance.RegisterEntity(typeof(FarmerProduction));

                    XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                    IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                    List<RegicusService_Model> list = new List<RegicusService_Model>();
                    //    IList<Farmer> collction = ObjectSpace.GetObject<Farmer>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1 ",null));
                    if (Firstname =="" && LastName == "" && ProvinceOid != "")
                    {
                        IList<Farmer> collection = ObjectSpace.GetObjects<Farmer>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1 and ProvinceOid  = '" + ProvinceOid + "' ", null));// [OrganizationOid] like '%" + org_oid + "%' and [ActivityOid] = '" + ActivityOid.Oid + "'  ", null));
                        var query = from Q in collection orderby Q.ProvinceOid select Q;
                        foreach (Farmer row in query)
                        {
                            Farmer_Modelinfo_province Farmer_info = new Farmer_Modelinfo_province();
                            Farmer_info.Oid = row.Oid.ToString();
                            if (row.OrganizationOid != null || row.OrganizationOid.ToString() != "")
                            {
                                Farmer_info.OrganizationOid = row.OrganizationOid.ToString();
                                Farmer_info.OrganizeNameTH = row.OrganizationOid.OrganizeNameTH;
                            }

                            Farmer_info.CitizenID = row.CitizenID;
                            Farmer_info.TitleOid = row.TitleOid.Oid.ToString();
                            Farmer_info.TitleName = row.TitleOid.TitleName;
                            Farmer_info.GenderName = row.GenderOid.GenderName;
                            Farmer_info.FirstNameTH = row.FirstNameTH;
                            Farmer_info.LastNameTH = row.LastNameTH;
                            Farmer_info.GenderOid = row.GenderOid.Oid.ToString();

                            if (row.BirthDate.ToString() != " " || row.BirthDate.ToString() != null)
                            {
                                Farmer_info.BirthDate = row.BirthDate.ToString();
                            }
                            if (row.Tel != null)
                            {
                                Farmer_info.Tel = row.Tel;
                            }

                            if (row.Email != null)
                            {
                                Farmer_info.Email = row.Email;
                            }

                            Farmer_info.IsActive = row.IsActive.ToString();
                            Farmer_info.DisPlayName = row.DisPlayName;
                            if (row.Address != null && row.Moo != null && row.Soi != null && row.Road != null || row.Address != "" && row.Moo != "" && row.Soi != "" && row.Road != "")
                            {
                                Farmer_info.Address = row.Address;
                                Farmer_info.Moo = row.Moo;
                                Farmer_info.Soi = row.Soi;
                                Farmer_info.Road = row.Road;
                            }

                            if (row.ProvinceOid != null)
                            {
                                Farmer_info.ProvinceOid = row.ProvinceOid.Oid.ToString();
                                Farmer_info.ProvinceNameTH = row.ProvinceOid.ProvinceNameTH;
                            }
                            if (row.DistrictOid != null)
                            {
                                Farmer_info.DistrictOid = row.DistrictOid.Oid.ToString();
                                Farmer_info.DistrictNameTH = row.DistrictOid.DistrictNameTH;
                            }
                            if (row.SubDistrictOid != null)
                            {
                                Farmer_info.SubDistrictOid = row.SubDistrictOid.Oid.ToString();
                                Farmer_info.SubDistrictNameTH = row.SubDistrictOid.SubDistrictNameTH.ToString();
                            }

                            Farmer_info.ZipCode = row.ZipCode;
                            Farmer_info.FullAddress = row.FullAddress;
                            Farmer_info.Latitude = row.Latitude;
                            Farmer_info.Longitude = row.Longitude;
                            Farmer_info.Rigister_Type = row.RegisterType.ToString();
                            //   Farmer_info.FarmerGroupsOid = row.FarmerGroupsOid.Oid.ToString(); ;
                            Farmer_info.Status = row.Status.ToString();

                            List<ForageType_Name> list_item = new List<ForageType_Name>();
                            ForageType_Name dt = new ForageType_Name();
                            if (row.FarmerProductions.Count > 0)
                            {
                                foreach (FarmerProduction row2 in row.FarmerProductions)
                                {
                                    if (row2.ForageTypeOid.ForageTypeName.Contains("เมล็ดพันธุ์") == true)
                                    {
                                        dt.ForageTypeName1 = row2.ForageTypeOid.ForageTypeName;
                                    }
                                    else if (row2.ForageTypeOid.ForageTypeName.Contains("เสบียงสัตว์") == true)
                                    {
                                        dt.ForageTypeName2 = row2.ForageTypeOid.ForageTypeName;
                                    }
                                    else if (row2.ForageTypeOid.ForageTypeName.Contains("ท่อนพันธุ์") == true)
                                    {
                                        dt.ForageTypeName3 = row2.ForageTypeOid.ForageTypeName;
                                    }
                                    else if (row2.ForageTypeOid.ForageTypeName.Contains("กล้าพันธุ์") == true)
                                    {
                                        dt.ForageTypeName4 = row2.ForageTypeOid.ForageTypeName;
                                    }
                                }
                            }
                            else
                            {
                                dt.ForageTypeName1 = "ไม่มีข้อมูลการผลิต";
                                dt.ForageTypeName2 = "ไม่มีข้อมูลการผลิต";
                                dt.ForageTypeName3 = "ไม่มีข้อมูลการผลิต";
                                dt.ForageTypeName4 = "ไม่มีข้อมูลการผลิต";
                            }
                            list_item.Add(dt);
                            Farmer_info.detail = list_item;

                            Farmer_Modelinfo.Add(Farmer_info);
                        }

                        return Request.CreateResponse(HttpStatusCode.OK, Farmer_Modelinfo);
                    }
                    else
                    {
                        IList<Farmer> collection = ObjectSpace.GetObjects<Farmer>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1 or FirstNameTH like '%" + Firstname + "%' or  LastNameTH like '%" + LastName + "%' ", null));
                        foreach (Farmer row in collection)
                        {
                            Farmer_Modelinfo_province Farmer_info = new Farmer_Modelinfo_province();
                            Farmer_info.Oid = row.Oid.ToString();
                            Farmer_info.OrganizationOid = row.OrganizationOid.ToString();
                            Farmer_info.OrganizeNameTH = row.OrganizationOid.OrganizeNameTH;
                            Farmer_info.CitizenID = row.CitizenID;
                            Farmer_info.TitleOid = row.TitleOid.Oid.ToString();
                            Farmer_info.TitleName = row.TitleOid.TitleName;
                            Farmer_info.GenderName = row.GenderOid.GenderName;
                            Farmer_info.FirstNameTH = row.FirstNameTH;
                            Farmer_info.LastNameTH = row.LastNameTH;
                            Farmer_info.GenderOid = row.GenderOid.Oid.ToString();

                            if (row.BirthDate.ToString() != "")
                            {
                                Farmer_info.BirthDate = row.BirthDate.ToString();
                            }
                            Farmer_info.Tel = row.Tel;
                            Farmer_info.Email = row.Email;
                            Farmer_info.IsActive = row.IsActive.ToString();
                            Farmer_info.DisPlayName = row.DisPlayName;
                            Farmer_info.Address = row.Address;
                            Farmer_info.Moo = row.Moo;
                            Farmer_info.Soi = row.Soi;
                            Farmer_info.Road = row.Road;
                            if (row.ProvinceOid != null)
                            {
                                Farmer_info.ProvinceOid = row.ProvinceOid.Oid.ToString();
                                Farmer_info.ProvinceNameTH = row.ProvinceOid.ProvinceNameTH;
                            }

                            if (row.DistrictOid != null)
                            {
                                Farmer_info.DistrictOid = row.DistrictOid.Oid.ToString();
                                Farmer_info.DistrictNameTH = row.DistrictOid.DistrictNameTH;
                            }
                            if (row.SubDistrictOid != null)
                            {
                                Farmer_info.SubDistrictOid = row.SubDistrictOid.Oid.ToString();
                                Farmer_info.SubDistrictNameTH = row.SubDistrictOid.SubDistrictNameTH.ToString();
                            }

                            Farmer_info.ZipCode = row.ZipCode;
                            Farmer_info.FullAddress = row.FullAddress;
                            Farmer_info.Latitude = row.Latitude;
                            Farmer_info.Longitude = row.Longitude;
                            Farmer_info.Rigister_Type = row.RegisterType.ToString();
                            //   Farmer_info.FarmerGroupsOid = row.FarmerGroupsOid.Oid.ToString(); ;
                            Farmer_info.Status = row.Status.ToString();

                            List<ForageType_Name> list_item = new List<ForageType_Name>();
                            ForageType_Name dt = new ForageType_Name();
                            if (row.FarmerProductions.Count > 0)
                            {
                                foreach (FarmerProduction row2 in row.FarmerProductions)
                                {
                                    if (row2.ForageTypeOid.ForageTypeName.Contains("เมล็ดพันธุ์") == true)
                                    {
                                        dt.ForageTypeName1 = row2.ForageTypeOid.ForageTypeName;
                                    }
                                    else if (row2.ForageTypeOid.ForageTypeName.Contains("TMR") == true)
                                    {
                                        dt.ForageTypeName2 = row2.ForageTypeOid.ForageTypeName;
                                    }
                                    else if (row2.ForageTypeOid.ForageTypeName.Contains("เสบียงสัตว์") == true)
                                    {
                                        dt.ForageTypeName3 = row2.ForageTypeOid.ForageTypeName;
                                    }
                                    else if (row2.ForageTypeOid.ForageTypeName.Contains("กล้าพันธุ์") == true)
                                    {
                                        dt.ForageTypeName4 = row2.ForageTypeOid.ForageTypeName;
                                    }
                                }
                                list_item.Add(dt);
                            }
                           
                            
                          
                            Farmer_info.detail = list_item;
                            Farmer_Modelinfo.Add(Farmer_info);
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, Farmer_Modelinfo);
                    }
                }
            }
            catch (Exception ex)
            {
                //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                err.message = ex.Message;
                //  Return resual

                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                throw;
            }
        }

        public bool CheckCitizenID(string CitizenId)
        {
            if (CitizenId.Length != 13)
            {
                return false;
            }
            string[] strCitizenID = new string[13];
            int strResult = 0;
            string tmpStr = "";
            string tmpCitizenID = CitizenId;
            for (int i = 0; i <= CitizenId.Length - 1; i++)
            {
                tmpStr =tmpCitizenID.Substring(0, 1);
                strCitizenID[i] = tmpStr;
                tmpCitizenID = tmpCitizenID.Remove(0, 1);
                try
                {
                    Convert.ToInt32(strCitizenID[i]);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            int CONS = 13;
            for (int i = 0; i <= 11; i++)
            {
                strResult = strResult + (System.Convert.ToInt32(strCitizenID[i]) * CONS);
                CONS = CONS - 1;
            }
            strResult = strResult % 11;
            strResult = (11 - strResult) % 10;
            if (strResult.ToString() == strCitizenID[12])
                return true;
            else
                return false;
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("farmerGETcheck")]
        public HttpResponseMessage xafclass( string Citizen)
        {
            //  Farmerinfo.Profile_Farmer farmerinfo = new Farmerinfo.Profile_Farmer();
            try
            {
                RegisterFarmerController best = new RegisterFarmerController();
                bool number = best.CheckCitizenID(Citizen);
                string check;
          
           
                if (number == true)
                {
                    check = "ถูกฟอแมต";
                }
                else
                {
                    check = "ไม่ถูกฟอแมต";
                }
                    return Request.CreateResponse(HttpStatusCode.OK, check);
                   
            }
            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                //   return BadRequest(ex.Message);
                return null;
            }
        }


    }
}

#endregion ใช้เพิ่มข้อมูล แก้ไข ข้อมูลเกษตรกร