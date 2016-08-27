using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel;
using System.Linq;

namespace OMInsurance.WebApps.Models
{
    public class UploadRegionExcelModel
    {
        public UploadRegionExcelModel()
        {
            Regions = ReferencesProvider.GetReferences(Constants.UralsibRegionsRef, null, true);
            Messages = new List<string>();
            //disabled Moscow
            Regions[Regions.FindIndex(a => a.Value == ((long)ListRegionId.Moscow).ToString())].Disabled = true;

        }

        [DisplayName("Регион")]
        public long RegionId { get; set; }
        public List<SelectListItem> Regions { get; set; }

        public string FilePath { get; set; }

        [DisplayName("Имя файла")]
        public string OriginalFileName { get; set; }

        [DisplayName("Статус загрузки")]
        public bool StatusLoad { get; set; }

        [DisplayName("Размер файла")]
        public string FileSizeToString { get; set; }

        [DisplayName("Всего записей")]
        public long? CountRow { get; set; }

        [DisplayName("Всего полей")]
        public long? CountField { get; set; }

        [DisplayName("Автоопределение региона")]
        public string AutoRegion { get; set; }

        public List<string> Messages { get; set; }
    }
}
