using OMInsurance.BusinessLogic.Core;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Controllers
{
    /// <summary>
    /// Controller returns FIAS object
    /// </summary>
    [Authorize]
    public class FIASController : Controller
    {
        FiasBusinessLogic bll = new FiasBusinessLogic();

        /// <summary>
        /// Returns list of regions by region name prefix
        /// </summary>
        /// <param name="name">Prefix of a region name</param>
        /// <returns>List of specified regions</returns>
        public JsonResult GetRegions(string name)
        {
            IEnumerable<SelectListItem> items = bll.GetRegions(name).Select(
                item => new SelectListItem()
                {
                    Value = item.Id,
                    Text = item.Name
                });
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Returns list of areas by areas name prefix and region id
        /// </summary>
        /// <param name="regionId">Identifier of a region</param>
        /// <param name="name">Prefix of a area name</param>
        /// <returns>List of areas</returns>
        public JsonResult GetAreas(string regionId, string name)
        {
            regionId = string.IsNullOrEmpty(regionId) ? null : regionId;
            IEnumerable<SelectListItem> items = bll.GetAreas(regionId, name).Select(
                item => new SelectListItem()
                {
                    Value = item.Id,
                    Text = item.Name
                });
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Returns list of cities by name prefix and area id and region id
        /// </summary>
        /// <param name="regionId">Identifier of a region</param>
        /// <param name="regionId">Identifier of an area</param>
        /// <param name="name">Prefix of a city name</param>
        /// <returns>List of cities</returns>
        public JsonResult GetCities(string regionId, string areaId, string name)
        {
            areaId = string.IsNullOrEmpty(areaId) ? null : areaId;
            regionId = string.IsNullOrEmpty(regionId) ? null : regionId;
            IEnumerable<SelectListItem> items = bll.GetCities(regionId, areaId, name).Select(
                item => new SelectListItem()
                {
                    Value = item.Id,
                    Text = item.Name
                });
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Returns list of cities and localities by name prefix and area id and region id
        /// </summary>
        /// <param name="regionId">Identifier of a region</param>
        /// <param name="regionId">Identifier of an area</param>
        /// <param name="name">Prefix of a city name</param>
        /// <returns>List of cities</returns>
        public JsonResult GetLocalitiesAndCities(string regionId, string areaId, string name)
        {
            areaId = string.IsNullOrEmpty(areaId) ? null : areaId;
            regionId = string.IsNullOrEmpty(regionId) ? null : regionId;
            var items = bll.GetCities(regionId, areaId, name);
            items.AddRange(bll.GetLocalities(regionId, areaId, null, name));
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (var item in items)
            {
                list.Add(new SelectListItem()
                {
                    Value = item.Id,
                    Text = item.Name
                });
                list.AddRange(bll.GetLocalities(regionId, areaId, item.Id, name).Select(
                locality => new SelectListItem()
                {
                    Value = locality.Id,
                    Text = locality.Name
                }));
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLocalities(string regionId, string areaId, string name)
        {
            areaId = string.IsNullOrEmpty(areaId) ? null : areaId;
            regionId = string.IsNullOrEmpty(regionId) ? null : regionId;
            var items = bll.GetLocalities(regionId, areaId, null, name);
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (var item in items)
            {
                list.Add(new SelectListItem()
                {
                    Value = item.Id,
                    Text = item.Name
                });
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Returns list of moscow region
        /// </summary>
        /// <param name="regionId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonResult GetStreets(string regionId, string name)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            regionId = string.IsNullOrEmpty(regionId) ? null : regionId;
            if (regionId != null && regionId.Equals(Constants.MoscowRegionFiasId))
            {
                list.AddRange(bll.GetMoscowStreets(name).Select(item => new SelectListItem() { Text = item.Name, Value = item.Code }));
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Returns list of SMO
        /// </summary>
        /// <param name="name">Substring od SMO name</param>
        /// <returns></returns>
        public JsonResult GetSmo(string name)
        {
            List<SMO> list = new List<SMO>();
            list = bll.GetSMOList(name);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}