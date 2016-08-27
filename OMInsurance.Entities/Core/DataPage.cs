using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.Entities.Core
{
	public class DataPage<T> : IEnumerable<T> where T : class
	{
		#region Properties

		protected List<T> _data;

		public List<T> Data
		{
			get
			{
				return _data;
			}
			private set
			{
				_data = value ?? new List<T>();
			}
		}

		public int PageNumber { get; set; }
		public int PageCount { get; set; }
		public int TotalCount { get; set; }
		public virtual int Count
		{
			get
			{
				return _data.Count;
			}
		}

		public T this[int index]
		{
			get
			{
				return _data[index];
			}
		}

		#endregion

		#region Constructors

		public DataPage()
			: this(new List<T>(), 0, 0, 0)
		{
		}

		public DataPage(IEnumerable<T> data)
			: this(data, 1, 1, data.Count())
		{
		}

		public DataPage(IEnumerable<T> data, int pageNumber, int pageCount, int totalCount)
		{
			_data = new List<T>(data);
			PageNumber = pageNumber;
			PageCount = pageCount;
			TotalCount = totalCount;
		}

		#endregion

		public static DataPage<T> Empty
		{
			get
			{
				return new DataPage<T>();
			}
		}

		#region IEnumerable<T> members

		public IEnumerator<T> GetEnumerator()
		{
			return Data.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Data.GetEnumerator();
		}

		#endregion
	}
}
