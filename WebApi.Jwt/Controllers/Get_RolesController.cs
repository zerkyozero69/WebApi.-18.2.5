using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Jwt.helpclass;
using WebApi.Jwt.Models;
using static WebApi.Jwt.Models.user;

namespace WebApi.Jwt.Controllers
{
    public class Get_RolesController : ApiController
    {
        private string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        /// <summary>
        /// เช็ค User ในระบบ
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        [AllowAnonymous]
        // [JwtAuthentication]
        [HttpPost]
        [Route("GetRoles_User")]
        public HttpResponseMessage GetRoles_byUser(string Username) ///ใส่หรือไม่ใส่ username ก็ได้ ทำงานได้ 2 แบบ แสดงชื่อทั้งหมดกับตาม user ที่ใส่
        {
            get_role_byuser objUser_info = new get_role_byuser();
            try

            {
                //
                DataSet ds;
                //ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MoblieGetRoles_ByUser", new SqlParameter("@Oid", Oid)); ///อย่าลืมเปลี่ยน คอนเนคชั่นสติง
                //DataTable dt = new DataTable();
                //dt = ds.Tables[0];
                helpController result = new helpController();
                objUser_info = result.get_Roles(Username);

                if (objUser_info.Status != 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, objUser_info);
                }
                {
                    objUser_info.Status = 2;
                    objUser_info.Message = "ใส่ Username ผิด";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, objUser_info);
                }
            }

            //else {
            //    objUser_info.status = "0";
            //}
            //return  Request.CreateResponse(HttpStatusCode.BadRequest, "ไม่เจอ User");
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
                Dispose();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="OidRole"></param>
        /// <returns></returns>
        [AllowAnonymous]
        // [JwtAuthentication]
        [HttpPost]
        [Route("GetUser_Role")]
        public HttpResponseMessage GetUser_byRole(string OidRole)
        {
            get_role_byuser getrole = new get_role_byuser();
            try
            {
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(scc, CommandType.StoredProcedure, "spt_MoblieGetUsers_ByRole", new SqlParameter("@oid", OidRole)); ///อย่าลืมเปลี่ยน คอนเนคชั่นสติง
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
                return Request.CreateResponse(HttpStatusCode.OK, rows);
            }
            catch (Exception ex)
            { //Error case เกิดข้อผิดพลาด
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ

                err.message = ex.Message;
                //  Return resual
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            finally
            {
                Dispose();
            }
        }
    }
}