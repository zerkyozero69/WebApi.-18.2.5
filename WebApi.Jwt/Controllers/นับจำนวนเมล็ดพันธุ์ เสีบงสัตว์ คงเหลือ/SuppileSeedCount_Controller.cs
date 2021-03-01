using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Jwt.Models;

namespace WebApi.Jwt.Controllers
{
    public class SuppileSeedCount_Controller : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
        //public HttpResponseMessage supplie_seedCount()
        //{
        //    try
        //    {
        //        DataSet ds;
        //        ds =SqlHelper.ExecuteDataset(scc);

            
        //    }

        //    catch (Exception ex)
        //    { //Error case เกิดข้อผิดพลาด
        //        UserError err = new UserError();
        //         err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
        //        err.message = ex.Message;
        //        //  Return resual
        //        return BadRequest(ex.Message);
        //            }
        //        }
    }
}
