using OMInsurance.Entities.Core;
using System.Collections.Generic;
using System.ComponentModel;

namespace OMInsurance.WebApps.Models
{
    public abstract class PagedList<T> where T : class 
    {
        public PagedList()
        {
            this.Items = new List<T>();
            this.PageSize = Constants.DefaultPageSize;
            this.PageNumber = Constants.DefaultPageNumber;
        }

        public IEnumerable<T> Items { get; set; }
        
        [DisplayName("Отображать")]
        public int PageSize { get; set; }

        [DisplayName("Номер страницы")]
        public int PageNumber { get; set; }

        [DisplayName("Всего найдено")]
        public int TotalCount { get; set; }
    }
}