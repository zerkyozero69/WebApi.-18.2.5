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
using System.Globalization;

namespace WebApi.Jwt.Controllers.MasterData
{
    public class CustomerTypeController : ApiController
    {
        string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();
        /// <summary>
        /// ประเภทผู้รับบริการ
        /// </summary>
        /// <returns></returns>
     
        [AllowAnonymous]
        // [JwtAuthentication]
        [HttpGet    ]
        [Route("GetCusService/ID")]
        public IHttpActionResult RegisterCusService()
        {
            string RegisterCusService = null;
            try
            {
                 RegisterCusService =  HttpContext.Current.Request.Form["RegisterCusService"].ToString(); ;
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(RegisterCusService));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();

                IList<RegisterCusService> collection = ObjectSpace.GetObjects<RegisterCusService>(CriteriaOperator.Parse(" IsActive = 1 and GCRecord is null and RegisterCusService ", RegisterCusService));
                List<RegisterCusService_Model> list = new List<RegisterCusService_Model>();
                foreach (RegisterCusService row in collection)
                {
                    RegisterCusService_Model Model = new RegisterCusService_Model();
                    Model.Oid = row.Oid.ToString();
                    Model.Organization = row.OrganizationOid.SubOrganizeName;
                    Model.RegisterDate = row.RegisterDate.ToString("dd-MM-yyyy", new CultureInfo("us-US"));
                    Model.CitizenID = row.CitizenID;
                    Model.Title = row.TitleOid.TitleName;
                    Model.FirstNameTH = row.FirstNameTH;
                    Model.LastNameTH = row.LastNameTH;
                    Model.Gender = row.GenderOid.GenderName;
                    if (row.BirthDate != null)
                    {
                        Model.BirthDate = row.BirthDate.ToString();
                    }
                    if (row.Tel != null)
                    {
                        Model.Tel = row.Tel.ToString();
                    }
                    if (row.Email != null)
                    {
                        Model.Email = row.Email.ToString();
                    }

                    if (row.Address != null)
                    {
                        Model.Address = row.Address.ToString();
                    }

                    if (row.Moo != null)
                    {
                        Model.Moo = row.Moo.ToString();
                    }
                    if (row.Soi != null)
                    {
                        Model.Soi = row.Soi.ToString();
                    }

                    if (row.Road != null)
                    {
                        Model.Road = row.Road.ToString();
                    }

                    Model.Province = row.ProvinceOid.ProvinceNameTH;
                    if (row.DistrictOid != null)
                    {
                        Model.District = row.DistrictOid.DistrictNameTH;
                    }
                    if (row.SubDistrictOid != null)
                    {

                        Model.SubDistrict = row.SubDistrictOid.SubDistrictNameTH;
                    }
                    if (row.ZipCode != null)
                    {

                        Model.ZipCode = row.ZipCode;
                    }

                    list.Add(Model);

                }
                return Ok(list);

                
            }
            catch (Exception ex)
            {
                UserError err = new UserError();
                err.code = "6"; // error จากสาเหตุอื่นๆ จะมีรายละเอียดจาก system แจ้งกลับ
                err.message = ex.Message;
                //  Return resual
                return BadRequest(ex.Message);
            }
        }


}
        
        
}
