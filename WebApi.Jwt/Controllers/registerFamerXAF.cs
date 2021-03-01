using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using DevExpress.Xpo.Helpers;
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
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using WebApi.Jwt.Models;
using WebApi.Jwt.Models.Models_Masters;
using static WebApi.Jwt.Models.Farmerinfo;
using static WebApi.Jwt.Models.Models_Masters.FarmerProduct_model;

namespace WebApi.Jwt.Controllers.MasterData
{
    public class RegisterFarmerXAF_Controller : ApiController
    {
        private string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();



        [AllowAnonymous]
        [HttpGet]
        [Route("farmerGET")]
        public IHttpActionResult xafclass()
        {
            //  Farmerinfo.Profile_Farmer farmerinfo = new Farmerinfo.Profile_Farmer();
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(Farmer));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                List<Farmerinfo.Profile_Farmer> ilist = new List<Farmerinfo.Profile_Farmer>();
                IList<Farmer> collection = ObjectSpace.GetObjects<Farmer>(CriteriaOperator.Parse(" GCRecord is null and IsActive = true", null));
                if (collection != null)
                {
                    foreach (Farmer row in collection)
                    {
                        Farmerinfo.Profile_Farmer _farmerinfo = new Farmerinfo.Profile_Farmer();
                        _farmerinfo.Oid = row.Oid;
                        _farmerinfo.CitizenID = row.CitizenID;
                        _farmerinfo.Title = row.TitleOid.TitleName;
                        _farmerinfo.FirstNameTH = row.FirstNameTH;
                        _farmerinfo.LastNameTH = row.LastNameTH;
                        ilist.Add(_farmerinfo);
                    }
                }
                else
                {
                    return BadRequest("Any object");
                }
                return Ok(ilist);
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
        [AllowAnonymous]
        //// [JwtAuthentication]
        //[HttpPost]
        [HttpPost]
        [Route("FarmerCitizenID")]
        public HttpResponseMessage get_CitizenID()
        {
            List<FarmerCitizen> farmerCitizenList = new List<FarmerCitizen>();
            FarmerCitizen farmer_info = new FarmerCitizen();

            try
            {
                string CitizenID = string.Empty;
                if (HttpContext.Current.Request.Form["CitizenID"].ToString() != null)
                {
                    CitizenID = HttpContext.Current.Request.Form["CitizenID"].ToString();
                }
                RegisterFarmerController best = new RegisterFarmerController();
                if (best.CheckCitizenID(CitizenID) == false)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, $"หมายเลขบัตรประชาชนไม่ถูกต้อง กรุณาตรวจสอบ");
                }
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MobileGetCitizenID", new SqlParameter("@CitizenID", CitizenID));
                if (ds.Tables.Count > 0)
                {
                    List<Farmer_Modelinfo> Farmer_Model = new List<Farmer_Modelinfo>();
                    Farmer_Modelinfo Farmer_info = new Farmer_Modelinfo();
                    Farmer_info.Oid = ds.Tables[0].Rows[0]["Oid"].ToString();
                    Farmer_info.OrganizationOid = ds.Tables[0].Rows[0]["OrganizationOid"].ToString();
                    Farmer_info.OrganizeNameTH = ds.Tables[0].Rows[0]["OrganizeNameTH"].ToString();
                    Farmer_info.CitizenID = ds.Tables[0].Rows[0]["CitizenID"].ToString();
                    Farmer_info.TitleOid = ds.Tables[0].Rows[0]["TitleOid"].ToString();
                    Farmer_info.TitleName = ds.Tables[0].Rows[0]["TitleName"].ToString();
                    Farmer_info.GenderName = ds.Tables[0].Rows[0]["GenderName"].ToString();
                    Farmer_info.FirstNameTH = ds.Tables[0].Rows[0]["FirstNameTH"].ToString();
                    Farmer_info.LastNameTH = ds.Tables[0].Rows[0]["LastNameTH"].ToString();
                    Farmer_info.GenderOid = ds.Tables[0].Rows[0]["GenderOid"].ToString();

                    if (ds.Tables[0].Rows[0]["BirthDate"].ToString() != "")
                    {
                        Farmer_info.BirthDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["BirthDate"]).ToString("dd/MM/yyyy");
                    }

                    Farmer_info.Tel = ds.Tables[0].Rows[0]["Tel"].ToString();
                    Farmer_info.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                    Farmer_info.IsActive = ds.Tables[0].Rows[0]["IsActive"].ToString();
                    Farmer_info.DisPlayName = ds.Tables[0].Rows[0]["DisPlayName"].ToString();
                    Farmer_info.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                    Farmer_info.Moo = ds.Tables[0].Rows[0]["Moo"].ToString();
                    Farmer_info.Soi = ds.Tables[0].Rows[0]["Soi"].ToString();
                    Farmer_info.Road = ds.Tables[0].Rows[0]["Road"].ToString();
                    Farmer_info.ProvinceOid = ds.Tables[0].Rows[0]["ProvinceOid"].ToString();
                    Farmer_info.ProvinceNameTH = ds.Tables[0].Rows[0]["ProvinceNameTH"].ToString();
                    Farmer_info.DistrictOid = ds.Tables[0].Rows[0]["DistrictOid"].ToString();
                    Farmer_info.DistrictNameTH = ds.Tables[0].Rows[0]["DistrictNameTH"].ToString();
                    Farmer_info.SubDistrictOid = ds.Tables[0].Rows[0]["SubDistrictOid"].ToString();
                    Farmer_info.SubDistrictNameTH = ds.Tables[0].Rows[0]["SubDistrictNameTH"].ToString();
                    Farmer_info.ZipCode = ds.Tables[0].Rows[0]["ZipCode"].ToString();
                    Farmer_info.FullAddress = ds.Tables[0].Rows[0]["FullAddress"].ToString();
                    Farmer_info.Latitude = ds.Tables[0].Rows[0]["Latitude"].ToString();
                    Farmer_info.Longitude = ds.Tables[0].Rows[0]["Longitude"].ToString();
                    Farmer_info.Rigister_Type = ds.Tables[0].Rows[0]["RegisterType"].ToString();
                    Farmer_info.FarmerGroupsOid = ds.Tables[0].Rows[0]["FarmerGroupsOid"].ToString();
                    Farmer_info.Status = ds.Tables[0].Rows[0]["Status"].ToString();

                    Farmer_info.ForageTypeName1 = null;
                    Farmer_info.ForageTypeName2 = null;
                    Farmer_info.ForageTypeName3 = null;
                    Farmer_info.ForageTypeName4 = null;

                    if (ds.Tables[0].Rows[0]["ForageTypeName"].ToString().Contains("เมล็ด"))
                    {
                        Farmer_info.ForageTypeName1 = "เมล็ดพันธุ์";
                    }

                    if (ds.Tables[0].Rows[0]["ForageTypeName"].ToString().Contains("เสบียง"))
                    {
                        Farmer_info.ForageTypeName2 = "เสบียงสัตว์";
                    }

                    if (ds.Tables[0].Rows[0]["ForageTypeName"].ToString().Contains("ท่อน"))
                    {
                        Farmer_info.ForageTypeName3 = "ท่อนพันธุ์";
                    }
                    Farmer_Model.Add(Farmer_info);
                    return Request.CreateResponse(HttpStatusCode.OK, Farmer_Model);
                    //    return Request.CreateResponse(HttpStatusCode.OK, Farmer_Modelinfo);
                }

                else
                {
                    string param = "username=regislive01&password=password&grant_type=password";//เพื่อทำการขอ access_token 
                    byte[] dataStream = Encoding.UTF8.GetBytes(param);
                    string AuthParam = "regislive:password";
                    string authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(AuthParam));
                    var request = (HttpWebRequest)WebRequest.Create("http://regislives.dld.go.th:9080/regislive_authen/oauth/token");
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = dataStream.Length;
                    request.Headers.Add("Authorization", "Basic " + authInfo);
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(dataStream, 0, dataStream.Length);
                    }
                    var response = (HttpWebResponse)request.GetResponse();
                    string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd().ToString();
                    var jsonResulttodict = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseString);
                    var access_token = jsonResulttodict["access_token"];

                    //    'Get Data By Token
                    request = (HttpWebRequest)WebRequest.Create("http://regislives.dld.go.th:9080/regislive_webservice/farmer/findbyPid?pid=" + CitizenID);
                    request.Method = "GET";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Headers.Add("Authorization", "Bearer " + access_token);
                    response = (HttpWebResponse)request.GetResponse();
                    var xreader = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    if (response.StatusCode.ToString() != "OK")
                    {
                        var farmerResul = JsonConvert.DeserializeObject<Dictionary<string, object>>(xreader);
                        if (xreader != string.Empty)
                        {
                            farmer_info.CitizenID = farmerResul["pid"];
                            farmer_info.titleName = farmerResul["prefixNameTh"];
                            farmer_info.FirstNameTH = farmerResul["firstName"];
                            farmer_info.LastNameTH = farmerResul["lastName"];
                            if (farmerResul["genderName"] == null)
                            {
                                farmer_info.genderName = "";
                            }
                            else
                            {
                                farmer_info.genderName = farmerResul["genderName"];
                            }
                            farmer_info.birthDate = farmerResul["birthDay"];
                            farmer_info.tel = farmerResul["phone"];
                            farmer_info.email = farmerResul["email"];
                            farmer_info.address = farmerResul["homeNo"];
                            farmer_info.moo = farmerResul["moo"];
                            farmer_info.soi = farmerResul["soi"];
                            farmer_info.road = farmerResul["road"];
                            farmer_info.provinceNameTH = farmerResul["provinceName"];
                            farmer_info.districtNameTH = farmerResul["amphurName"];
                            farmer_info.subDistrictNameTH = farmerResul["tambolName"];
                            farmer_info.PostCode = farmerResul["postCode"];
                            farmer_info.latitude = farmerResul["latitude"];
                            farmer_info.longitude = farmerResul["longitude"];
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.NoContent, "ไม่ข้อมูลเลขบัตรนี้ โปรดลงทะเบียน");
                        }
                    }
                    else
                    {

                        
                        return Request.CreateResponse(HttpStatusCode.GatewayTimeout,"ไม่สามารถติดต่อเซิฟเวอร์ได้" );
                    }
                    farmerCitizenList.Add(farmer_info);
                    return Request.CreateResponse(HttpStatusCode.OK, farmerCitizenList);
                

                //}
                //    else
                //    {
                //        UserError err3 = new UserError();
                //        err3.status = "ไม่พบเลขบัตรประชาชน กรุณาลงทะเบียน";
                //        err3.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ


                //        // Return resual
                //        return Request.CreateResponse(HttpStatusCode.NotFound, err3);

                //    }

                //Farmer_Model.Add(Farmer_info);
                //return Request.CreateResponse(HttpStatusCode.OK, Farmer_Model);

                //else
                //{

                //    UserError err = new UserError();
                //    err.status = "ไม่พบเลขบัตรประชาชนในระบบ กรุณาลงทะเบียน";
                //    err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ


                //    // Return resual
                //    //return Request.CreateResponse(HttpStatusCode.NotFound, err);

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

            }
            finally
            {

                SqlConnection.ClearAllPools();
            }

        }

    }
}