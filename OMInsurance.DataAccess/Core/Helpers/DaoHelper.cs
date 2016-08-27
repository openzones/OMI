using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace OMInsurance.DataAccess.Core.Helpers
{
    public static class DaoHelper
    {
        /// <summary>
        /// Creates data page containing specified items.
        /// </summary>
        /// <typeparam name="T">Type of items.</typeparam>
        /// <param name="items">Collection of items that should be displayed at the data page.</param>
        /// <param name="totalCountParameter">Parameter containing total number of items on all pages.</param>
        /// <param name="pageSize">Size of single page.</param>
        /// <returns>Data page containing the items.</returns>
        public static DataPage<T> GetDataPage<T>(IEnumerable<T> items, SqlParameter totalCountParameter, PageRequest pageRequest) where T : class
        {
            int totalCount = (int)totalCountParameter.Value;
            return GetDataPage(items, pageRequest.PageNumber, totalCount, pageRequest.PageSize);
        }

        /// <summary>
        /// Creates data page containing specified items.
        /// </summary>
        /// <typeparam name="T">Type of items.</typeparam>
        /// <param name="items">Collection of items that should be displayed at the data page.</param>
        /// <param name="totalCount">Total number of items on all pages.</param>
        /// <param name="pageSize">Size of single page.</param>
        /// <returns>Data page containing the items.</returns>
        public static DataPage<T> GetDataPage<T>(IEnumerable<T> items, int pageNumber, int totalCount, int pageSize)
        where T : class
        {
            int pagesCount = GetPagesCount(totalCount, pageSize);
            return new DataPage<T>(items, pageNumber, pagesCount, totalCount);
        }

        /// <summary>
        /// Calculates number of data pages for specified total number of items and page size.
        /// </summary>
        /// <param name="totalCount">Total number of items on all pages.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Number of data pages.</returns>
        public static int GetPagesCount(int totalCount, int pageSize)
        {
            if (totalCount < 0)
            {
                throw new ArgumentOutOfRangeException("totalCount", "Number of items must be equal to or greater than zero.");
            }
            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException("pageSize", "Page size must be greater than zero.");
            }

            int itemsCountOnLastPage;
            int fullPagesCount = Math.DivRem(totalCount, pageSize, out itemsCountOnLastPage);

            if (itemsCountOnLastPage > 0)
            {
                return fullPagesCount + 1;
            }
            else
            {
                return fullPagesCount;
            }
        }

        public static DataTable GetObjectIds(IEnumerable<long> objectIds)
        {
            DataTable idsTable = new DataTable("ObjectIds");
            DataColumn idColumn = new DataColumn("entity_id", typeof(long));
            idsTable.Columns.Add(idColumn);

            if (objectIds != null)
            {
                foreach (long objectId in objectIds.Distinct())
                {
                    idsTable.Rows.Add(objectId);
                }
            }

            return idsTable;
        }

        public static DataTable GetObjectIds(params long[] objectIds)
        {
            return GetObjectIds(objectIds as IEnumerable<long>);
        }

        public static DataTable GetNamesTable(IEnumerable<string> items)
        {
            DataTable stringsTable = new DataTable();
            stringsTable.Columns.Add(new DataColumn("name", typeof(string)));

            if (items != null)
            {
                foreach (string item in items)
                {
                    stringsTable.Rows.Add(DaoHelper.TruncateString(item, Constants.NameMaxLength));
                }
            }

            return stringsTable;
        }

        public static DataTable GetLongNamesTable(IEnumerable<string> items)
        {
            DataTable stringsTable = new DataTable();
            stringsTable.Columns.Add(new DataColumn("longname", typeof(string)));

            if (items != null)
            {
                foreach (string item in items)
                {
                    stringsTable.Rows.Add(DaoHelper.TruncateString(item, Constants.LongNameMaxLength));
                }
            }

            return stringsTable;
        }

        // TODO: review name of this method and type of column
        public static DataTable GetStringsTable(IEnumerable<int> items)
        {
            DataTable stringsTable = new DataTable();
            DataColumn stringColumn = new DataColumn("value", typeof(string));
            stringsTable.Columns.Add(stringColumn);

            if (items != null)
            {
                foreach (int item in items)
                {
                    stringsTable.Rows.Add(item.ToString(CultureInfo.InvariantCulture));
                }
            }

            return stringsTable;
        }

        public static DataTable GetEnumsTable<T>(IEnumerable<T> items)
            where T : struct
        {
            return GetNamesTable(items.Select(item => item.ToString()));
        }

        public static DataTable GetDatesTable(IEnumerable<DateTime> dates)
        {
            DataTable datesTable = new DataTable();
            datesTable.Columns.Add(new DataColumn("date", typeof(DateTime)));

            if (dates != null)
            {
                foreach (DateTime date in dates)
                {
                    datesTable.Rows.Add(date);
                }
            }

            return datesTable;
        }

        public static DataTable GetSortFieldsTable<SomeSortField>(IEnumerable<SortCriteria<SomeSortField>> items) where SomeSortField : struct
        {
            DataTable result = new DataTable();

            result.Columns.Add(new DataColumn("sort_field", typeof(string)));
            result.Columns.Add(new DataColumn("sort_order", typeof(string)));

            if (items != null)
            {
                foreach (SortCriteria<SomeSortField> item in items)
                {
                    result.Rows.Add(item.SortField, item.SortOrder.ToString());
                }
            }

            return result;
        }

        public static string TruncateString(string value, int maxLength)
        {
            if (value != null && value.Length > maxLength)
            {
                return value.Substring(0, maxLength);
            }
            else
            {
                return value;
            }
        }
    }
}