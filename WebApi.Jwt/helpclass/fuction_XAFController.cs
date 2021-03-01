using System.Web.Http;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base.Security;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp;
using WebApi.Jwt.Models;
using DevExpress.Data.Filtering;
using System.Data.SqlClient;
using System.Configuration;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Base;
using System.Net;
using System.Net.Http;
using System.Xml;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System;
using DevExpress.ExpressApp.Model;
using System.Security.Cryptography;
using DevExpress.Persistent.Validation;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using DevExpress.ExpressApp.Security.Strategy;
using nutrition.Module.EmployeeAsUserExample.Module.BusinessObjects;
using System.Collections.Generic;
using WebApi.Jwt.Controllers;
using nutrition.Module;
namespace WebApi.Jwt.helpclass
{
    public class fuction_XAFController : ApiController
    {
        //    public HttpResponseMessage FarmerProduction_XAF()
        //    {
        //        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
        //        try
        //        {
        //            XpoTypesInfoHelper.GetTypesInfo();
        //            XafTypesInfo.Instance.RegisterEntity(typeof(CustomerType));

        //            XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
        //            IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
        //         //   string register_farmerXAF = new register_farmer();
        //            IList<CustomerType> collection = ObjectSpace.GetObjects<CustomerType>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1", null));
        //            if (collection.Count > 0)
        //            {
        //                List<user.FarmerProductionModel> list = new List<user.FarmerProductionModel>();
        //                foreach (FarmerProduction row in collection)
        //                {
        //                    user.FarmerProductionModel productionModel = new user.FarmerProductionModel();
        //                    productionModel.Oid = row.Oid;
        //                    productionModel.Production = row.AnimalSeedOid.SeedName;
        //                    list.Add(productionModel);
        //                }
        //                return Request.CreateResponse(HttpStatusCode.OK, list);
        //            }
        //            {
        //                UserError err = new UserError();
        //                err.code = ""; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
        //                err.message = "No data";
        //                //  Return resual
        //                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
        //            }
        //        }
        //        catch (Exception ex)
        //        { //Error case เกิดข้อผิดพลาด
        //            UserError err = new UserError();
        //            err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
        //            err.message = ex.Message;
        //            //  Return resual
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, err);
        //        }
        //    }
        //}
    }
}
