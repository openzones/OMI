namespace OMInsurance.Entities.Core
{
	public class PageRequest
	{
		public const int FirstPage = 1;
		public const int DefaultPageSize = 25;

		public readonly static PageRequest SinglePage = new PageRequest(1, int.MaxValue);

		#region Properties

		public int PageNumber
		{
			get;
			set;
		}

		public int PageSize
		{
			get;
			set;
		}

		#endregion

		public PageRequest()
		{
			PageNumber = 1;
			PageSize = DefaultPageSize;
		}

		public PageRequest(int pageNumber, int pageSize)
		{
			PageNumber = pageNumber;
			PageSize = pageSize;
		}
	}
}
