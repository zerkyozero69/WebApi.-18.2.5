namespace WebApi.Jwt.Controllers.MasterData
{
    //public class SubActivityLevelController : ApiController
    //     string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
    //{
    //    public HttpResponseMessage SubActivity_info()
    //    {
    //        try
    //        {
    //            XpoTypesInfoHelper.GetXpoTypeInfoSource();
    //            XafTypesInfo.Instance.RegisterEntity(typeof(Activator));
    //            XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
    //            IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
    //            List<activity_Model> list = new List<activity_Model>();
    //            List<activityDetails_Model> detail = new List<activityDetails_Model>();
    //            IList<Activity> collection = ObjectSpace.GetObjects<Activity>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1 and ObjectTypeOid = '" + ObjectTypeOid + "' ", null));

    //        }
    //        catch (Exception ex)
    //        { //Error case เกิดข้อผิดพลาด
    //            UserError err = new UserError();
    //            err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
    //            err.message = ex.Message;
    //            //  Return resual
    //            return Request.CreateResponse(HttpStatusCode.BadRequest, err);
    //        }
}