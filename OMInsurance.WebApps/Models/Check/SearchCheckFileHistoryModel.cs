using System;
using System.ComponentModel;
using OMInsurance.Entities.Searching;

namespace OMInsurance.WebApps.Models
{
    public class SearchCheckFileHistoryModel
    {
        public SearchCheckFileHistoryModel()
        {
            DateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        }

        [DisplayName("Статус")]
        public long? StatusId { get; set; }

        [DisplayName("Дата загрузки")]
        public DateTime? DateFrom { get; set; }

        [DisplayName("Дата загрузки до")]
        public DateTime? DateTo { get; set; }

        [DisplayName("Пользователь")]
        public long? UserId { get; set; }

        public CheckFileHistorySearchCriteria GetSearchCriteria()
        {
            CheckFileHistorySearchCriteria criteria = new CheckFileHistorySearchCriteria();
            criteria.StatusId = this.StatusId;
            criteria.DateFrom = this.DateFrom;
            if (this.DateTo.HasValue)
            {
                criteria.DateTo = ((DateTime)this.DateTo).AddDays(1);
            }
            criteria.UserId = this.UserId;

            return criteria;
        }
    }
}