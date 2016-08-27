namespace OMInsurance.Entities.Core
{
	public sealed class SortCriteria<T> where T : struct
	{
		#region Properties

		public T SortField
		{
			get;
			set;
		}

		public SortOrder SortOrder
		{
			get;
			set;
		}

		#endregion

		#region Constructors
		public SortCriteria()
		{
		}

		public SortCriteria(T sortField, SortOrder sortOrder)
		{
			SortField = sortField;
			SortOrder = sortOrder;
		}

		#endregion
	}
}
