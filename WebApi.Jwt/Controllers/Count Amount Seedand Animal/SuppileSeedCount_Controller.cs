using System.Configuration;
using System.Web.Http;

namespace WebApi.Jwt.Controllers
{
    public class SuppileSeedCount_Controller : ApiController
    {
        private string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
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