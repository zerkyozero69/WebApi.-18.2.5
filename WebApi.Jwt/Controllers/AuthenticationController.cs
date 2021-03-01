using Microsoft.ApplicationBlocks.Data;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web;
using System.Web.Http;
using WebApi.Jwt.Filters;
using WebApi.Jwt.helpclass;
using WebApi.Jwt.Models;
using static WebApi.Jwt.helpclass.helpController;
using static WebApi.Jwt.Models.user;

namespace WebApi.Jwt.Controllers
{
    public class AuthenticationController : ApiController
    {
        private string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        /// <summary>
        /// ฟังชั่น login ผ่าน xaf แล้ว เจน token ให้ไปใช้กับเซอร์วิสอื่น
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        //[JwtAuthentication] /ถ้าใช้โทเคนต้องครอบ
        // [HttpPost] หน้าโมบาย
        [HttpPost]
        [Route("Login")]
        public HttpResponseMessage LoginAuthen() // ByVal UserName As String, ByVal password As String :
        {
            //  HttpResponseMessage
            // IHttpActionResult
            User_info user = new User_info();
            Roles_info rolename = new Roles_info();
            try
            {
                object Token_key = "";
                Login login = new Login();
                if (HttpContext.Current.Request.Form["UserName"].ToString() != null)
                {
                    login.Username = HttpContext.Current.Request.Form["UserName"].ToString();
                }
                if (HttpContext.Current.Request.Form["Password"].ToString() != null)
                {
                    login.Password = HttpContext.Current.Request.Form["Password"].ToString();
                }

                helpController result = new helpController();
                //login.resultLogin = result.CheckLogin_XAF(login.Username, login.Password);
                //if (login.resultLogin != null)
                //{
                //    TokenController token = new TokenController();
                //    Token_key = token.Get(login.Username, login.Password);
                //}
                // XpoTypesInfoHelper.GetXpoTypeInfoSource();
                user = result.CheckLogin_XAF(login.Username, login.Password);
                SqlParameter[] prm = new SqlParameter[9]; /// parameter นับได้เท่าไร ใส่เท่านั้น c#
                user.Description = "ระบบ login";
                prm[0] = new SqlParameter("@Username", user.User_Name); ///แต่ array ต้องนับจาก 0
                prm[1] = new SqlParameter("@DisplayName", user.DisplayName);
                prm[2] = new SqlParameter("@Organization", user.OrganizationNameTH);
                prm[3] = new SqlParameter("@Tel", user.Tel);
                prm[4] = new SqlParameter("@Email", user.Email);
                prm[5] = new SqlParameter("@LogID", "2");
                prm[6] = new SqlParameter("@IPAddress", GetClientIp(Request));
                prm[7] = new SqlParameter("@Description", user.Message);

                if (user.Status == 1)
                {
                    user.Message = "เข้าสู่ระบบสำเร็จ";
                    prm[8] = new SqlParameter("@EventName", user.Description);
                    SqlHelper.ExecuteNonQuery(scc, CommandType.StoredProcedure, "insert_EventLog", prm);
                    return Request.CreateResponse(HttpStatusCode.OK, user);
                }
                else if (user.Status == 0 || user.Status == 6)
                {
                    user.Message = "เข้าสู่ระบบไม่สำเร็จ";
                    prm[8] = new SqlParameter("@EventName", user.Description);
                    SqlHelper.ExecuteNonQuery(scc, CommandType.StoredProcedure, "insert_EventLog", prm);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, user);
                }
            }
            catch (Exception ex)             {
                //Error case เกิดข้อผิดพลาด

                user.Status = 6;
                user.Message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, user);
            }
            finally
            {
                Dispose();
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, user);
        }

        [JwtAuthentication]
        [Route("Logout")]
        [AcceptVerbs("POST")]
        public HttpResponseMessage Logout()
        {
            try
            {
                UserError err = new UserError();
                err.code = "0"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = "Logout Success";
                // Return resual
                return Request.CreateResponse(HttpStatusCode.OK, err);
            }
            catch (Exception ex)
            {
                // error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                // Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }

        /// <summary>
        /// ทำการรีเซ็จพาสเวริค
        /// </summary>
        /// <returns></returns>
        //  [JwtAuthentication]
        [AllowAnonymous]
        [Route("Reset/Password")]
        [AcceptVerbs("POST")]
        public HttpResponseMessage Reset_Password()
        {
            Reset_Password reset = new Reset_Password();
            try
            {
                if (HttpContext.Current.Request.Form["Username"].ToString() != null)
                {
                    reset.Username = HttpContext.Current.Request.Form["Username"].ToString();
                }
                if (HttpContext.Current.Request.Form["Password"].ToString() != null)
                {
                    reset.Password = HttpContext.Current.Request.Form["Password"].ToString();
                }
                var fromAddress = new MailAddress("from@gmail.com", "From Name");
                var toAddress = new MailAddress("to@example.com", "To Name");
                const string fromPassword = "fromPassword";
                const string subject = "Subject";
                const string body = "Body";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
                return Request.CreateResponse(HttpStatusCode.OK, "ส่ง Email แก้ไขพาสเวริค");
            }
            catch (Exception ex)
            {
                // error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                // Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }
    }
}