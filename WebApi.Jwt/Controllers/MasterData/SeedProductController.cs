using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using nutrition.Module;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Jwt.Models;
using WebApi.Jwt.Models.Models_Masters;

namespace WebApi.Jwt.Controllers.MasterData
{
    public class SeedProductController : ApiController
    {/// <summary>
     /// เรียกข้อมูล การผลิตของเกษตรกร
     /// </summary>
        private string scc = ConfigurationManager.ConnectionStrings["scc"].ConnectionString.ToString();

        [AllowAnonymous]
        [HttpPost]
        [Route("AnimalSupplie_Info")]
        ///พันธุ์พืช อาหารสัตว์
        public HttpResponseMessage loadAnimalSupplie()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(AnimalSupplie));
                List<AnimalSupplie_Model> list = new List<AnimalSupplie_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<AnimalSupplie> collection = ObjectSpace.GetObjects<AnimalSupplie>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1 ", null));
                foreach (AnimalSupplie row in collection)
                {
                    AnimalSupplie_Model model = new AnimalSupplie_Model();
                    model.AnimalSupplieOid = row.Oid.ToString();
                    model.AnimalSupplieName = row.AnimalSupplieName;
                    model.IsActive = row.IsActive;
                    list.Add(model);
                }
                return Request.CreateResponse(HttpStatusCode.OK, list);
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

        [AllowAnonymous]
        [HttpPost]
        [Route("Seedlevel")]
        /// ระดับชั้นพันธุ์
        public HttpResponseMessage loadSeedlevel()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(AnimalSeedLevel));
                List<AnimalSeedLevel_Model> list = new List<AnimalSeedLevel_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<AnimalSeedLevel> collection = ObjectSpace.GetObjects<AnimalSeedLevel>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1 ", null));
                foreach (AnimalSeedLevel row in collection)
                {
                    AnimalSeedLevel_Model model = new AnimalSeedLevel_Model();
                    model.Oid = row.Oid.ToString();
                    model.SeedLevelCode = row.SeedLevelCode;
                    model.SeedLevelName = row.SeedLevelName;
                    model.SortID = row.SortID;
                    model.IsActive = row.IsActive;
                    list.Add(model);
                }
                return Request.CreateResponse(HttpStatusCode.OK, list);
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

        [AllowAnonymous]
        [HttpPost]
        [Route("SeedType")]
        /// ประเภทเมล็ด
        public HttpResponseMessage loadSeedType()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(SeedType));
                List<SeedType_Model> list = new List<SeedType_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                //   Activity ActivityOid = ObjectSpace.FindObject<Activity>(CriteriaOperator.Parse("GCRecord is null and IsActive = 1 and ActivityName ='เพื่อช่วยเหลือภัยพิบัติ'  ", null));
                ForageType objseedtype = ObjectSpace.FindObject<ForageType>(CriteriaOperator.Parse("IsActive = 1 and  ForageTypeName = 'เสบียงสัตว์ ' ", null));
                IList<SeedType> collection = ObjectSpace.GetObjects<SeedType>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1  and [ForageTypeOid] ='" + objseedtype.Oid + "'  ", null));
                foreach (SeedType row in collection)
                {
                    SeedType_Model model = new SeedType_Model();
                    model.SeedTypeOid = row.Oid.ToString();
                    model.SeedTypeName = row.SeedTypeName;
                    model.IsActive = row.IsActive;
                    list.Add(model);
                }
                return Request.CreateResponse(HttpStatusCode.OK, list);
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

        [AllowAnonymous]
        [HttpPost]
        [Route("Harvesting")]
        /// วิธีเก็บเกี่ยว
        public HttpResponseMessage loadHarvesting(string ForageTypeOid) // เมล็ดพัรธุ์ | เสบียงสัตว์ | ค่าว่าง
        {
            try

            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(Harvest));
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<Harvest> collection = ObjectSpace.GetObjects<Harvest>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1", null));

                List<PlantModel> list = new List<PlantModel>();
                foreach (Harvest row in collection)
                {
                    PlantModel plant = new PlantModel();
                    plant.Oid = row.Oid;
                    plant.ForageTypeOid = row.HarvestName;
                    list.Add(plant);
                }
                return Request.CreateResponse(HttpStatusCode.OK, list);
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

        /// <summary>
        /// ค้นหาจำนวนคงเหลือ
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("AnimalSupplieType_Stock")]
        /// ประเภทอาหารสัตว์
        public HttpResponseMessage loadAnimalsuppliesStock()
        {
            string SeedTypeOid = null;
            string managesuboid = null;
            try
            {
                string animalsupplieoid = HttpContext.Current.Request.Form["animalsupplieoid"].ToString();
                string orgOid = HttpContext.Current.Request.Form["orgoid"].ToString();

                if (HttpContext.Current.Request.Form["managesuboid"].ToString() != null)
                {
                    managesuboid = HttpContext.Current.Request.Form["seedTypeOid"].ToString();
                }

                if (HttpContext.Current.Request.Form["seedTypeOid"].ToString() != null)
                {
                    SeedTypeOid = HttpContext.Current.Request.Form["seedTypeOid"].ToString();
                }

                string budgetsourceoid = HttpContext.Current.Request.Form["budgetsourceoid"].ToString();
                //   string animalsupplieroid = HttpContext.Current.Request.Form["animalsupplieroid"].ToString();
                // string animalsuppliertypeoid = HttpContext.Current.Request.Form["animalsuppliertypeoid"].ToString();
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(AnimalSupplieType));
                List<AnimalSupplieType_Model> list = new List<AnimalSupplieType_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<AnimalSupplieType> collection = ObjectSpace.GetObjects<AnimalSupplieType>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1 and [AnimalSupplie.oid] = ? ", animalsupplieoid));
                foreach (AnimalSupplieType row in collection)
                {
                    AnimalSupplieType_Model model = new AnimalSupplieType_Model();
                    model.animalsupplieoid = animalsupplieoid;
                    model.AnimalSupplieTypeOid = row.Oid.ToString();
                    model.SupplietypeName = row.SupplietypeName;
                    model.AnimalSupplieName = row.AnimalSupplie.AnimalSupplieName;
                    model.SalePrice = row.SalePrice;
                    model.IsActive = row.IsActive;
                    list.Add(model);
                }
                return Request.CreateResponse(HttpStatusCode.OK, list);
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

        [AllowAnonymous]
        [HttpPost]
        [Route("AnimalType")]
        /// ระดับชั้นพันธุ์
        public HttpResponseMessage AnimalType()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(AnimalType));
                List<AnimalType_Model> list = new List<AnimalType_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<AnimalType> collection = ObjectSpace.GetObjects<AnimalType>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1 ", null));
                foreach (AnimalType row in collection)
                {
                    AnimalType_Model model = new AnimalType_Model();
                    model.Oid = row.Oid.ToString();
                    model.AnimalCode = row.AnimalCode;
                    model.AnimalName = row.AnimalName;
                    model.IsActive = row.IsActive;
                    list.Add(model);
                }
                return Request.CreateResponse(HttpStatusCode.OK, list);
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

        [AllowAnonymous]
        [HttpPost]
        [Route("Animal_seed")]
        /// ชื่อพันธุ์พืชอาหารสัตว์
        public HttpResponseMessage AnimalSeed()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(AnimalSeed));
                List<AnimalSeed_Model> list = new List<AnimalSeed_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<AnimalSeed> collection = ObjectSpace.GetObjects<AnimalSeed>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1 ", null));
                foreach (AnimalSeed row in collection)
                {
                    AnimalSeed_Model model = new AnimalSeed_Model();
                    model.Oid = row.Oid.ToString();
                    model.SeedCode = row.SeedCode;
                    model.SeedName = row.SeedName;
                    model.SeedNameCommon = row.SeedNameCommon;
                    model.SeedNameScience = row.SeedNameScience;
                    model.IsActive = row.IsActive;
                    list.Add(model);
                }
                return Request.CreateResponse(HttpStatusCode.OK, list);
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

        [AllowAnonymous]
        [HttpPost]
        [Route("ForageType")]
        /// ชื่อพันธุ์พืชอาหารสัตว์
        public HttpResponseMessage AnimalForageType()
        {
            try
            {
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(ForageType));
                List<ForageType_Model> list = new List<ForageType_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<ForageType> collection = ObjectSpace.GetObjects<ForageType>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1 ", null));
                foreach (ForageType row in collection)
                {
                    ForageType_Model model = new ForageType_Model();
                    model.Oid = row.Oid.ToString();
                    model.ForageTypeName = row.ForageTypeName;
                    model.IsActive = row.IsActive;
                    list.Add(model);
                }
                return Request.CreateResponse(HttpStatusCode.OK, list);
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

        /// <summary>
        /// ค้นหาจำนวนคงเหลือ
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("AnimalSupplieType")]
        /// ประเภทอาหารสัตว์
        public HttpResponseMessage loadAnimalsupplies()
        {
            try
            {
                string animalsupplieroid = HttpContext.Current.Request.Form["animalsupplieroid"].ToString();
                // string animalsuppliertypeoid = HttpContext.Current.Request.Form["animalsuppliertypeoid"].ToString();
                XpoTypesInfoHelper.GetXpoTypeInfoSource();
                XafTypesInfo.Instance.RegisterEntity(typeof(AnimalSupplieType));
                List<AnimalSupplieType_Model> list = new List<AnimalSupplieType_Model>();
                XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(scc, null);
                IObjectSpace ObjectSpace = directProvider.CreateObjectSpace();
                IList<AnimalSupplieType> collection = ObjectSpace.GetObjects<AnimalSupplieType>(CriteriaOperator.Parse(" GCRecord is null and IsActive = 1 and [AnimalSupplie.oid] = ? ", animalsupplieroid));
                foreach (AnimalSupplieType row in collection)
                {
                    AnimalSupplieType_Model model = new AnimalSupplieType_Model();
                    model.animalsupplieoid = row.AnimalSupplie.Oid.ToString();
                    model.AnimalSupplieTypeOid = row.Oid.ToString();
                    model.SupplietypeName = row.SupplietypeName;
                    model.AnimalSupplieName = row.AnimalSupplie.AnimalSupplieName;
                    model.SalePrice = row.SalePrice;
                    model.IsActive = row.IsActive;
                    list.Add(model);
                }
                return Request.CreateResponse(HttpStatusCode.OK, list);
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
    }
}