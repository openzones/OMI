using System;
using System.ComponentModel;
using OMInsurance.Entities.Searching;

namespace OMInsurance.WebApps.Models
{
    public class SearchCheckPretensionModel
    {
        public SearchCheckPretensionModel()
        {
            M_daktFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            M_daktTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        }

        [DisplayName("Дата акта")]
        public DateTime? M_daktFrom { get; set; }

        [DisplayName("Дата акта до")]
        public DateTime? M_daktTo { get; set; }

        [DisplayName("Дата создания")]
        public DateTime? CreateDateFrom { get; set; }

        [DisplayName("Дата создания до")]
        public DateTime? CreateDateTo { get; set; }

        [DisplayName("Пользователь")]
        public long? UserId { get; set; }

        public CheckPretensionSearchCriteria GetSearchCriteria()
        {
            CheckPretensionSearchCriteria criteria = new CheckPretensionSearchCriteria()
            {
                M_daktFrom = this.M_daktFrom,
                M_daktTo = this.M_daktTo,
                CreateDateFrom = this.CreateDateFrom,
                CreateDateTo = this.CreateDateTo,
                UserId = this.UserId,
            };
            return criteria;
        }
    }
}