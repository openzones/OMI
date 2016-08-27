using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.Collections.Generic;

namespace OMInsurance.BusinessLogic.Core
{
    /// <summary>
    /// Class provides an access to FIAS references by criteria
    /// </summary>
    public class FiasBusinessLogic
    {
        public List<FiasEntry> GetRegions(string name)
        {
            List<FiasEntry> items = FiasDao.Instance.Find(null, name, FiasType.Region);
            return items;
        }

        public List<FiasEntry> GetAreas(string regionId, string name)
        {
            List<FiasEntry> items = FiasDao.Instance.Find(regionId, name, FiasType.Area);
            return items;
        }

        public List<FiasEntry> GetCities(string regionId, string areaId, string name)
        {
            List<FiasEntry> items = FiasDao.Instance.Find(areaId ?? regionId, name, FiasType.City);
            return items;
        }

        public List<FiasEntry> GetLocalities(string regionId, string areaId, string cityId, string name)
        {
            List<FiasEntry> items = FiasDao.Instance.Find(cityId ?? areaId ?? regionId, name, FiasType.Locality);
            return items;
        }

        public List<Street> GetMoscowStreets(string name)
        {
            return FiasDao.Instance.FindStreet(name);
        }

        public List<SMO> GetSMOList(string name)
        {
            return FiasDao.Instance.FindSmo(name);
        }
    }
}
