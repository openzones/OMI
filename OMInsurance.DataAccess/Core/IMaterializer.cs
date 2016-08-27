using System.Collections.Generic;

namespace OMInsurance.DataAccess.Core
{
    /// <summary>
    /// Interface to materialize data objects.
    /// </summary>
    /// <typeparam name="T">Data object type.</typeparam>
    public interface IMaterializer<T>
        where T : class
    {
        /// <summary>
        /// Materializes single object from DataReader.
        /// </summary>
        /// <param name="dataReader">DataReader packed to the helper class DataReaderAdapter.</param>
        /// <returns>Instance of T.</returns>
        /// <remarks>All result sets are read within this method.</remarks>
        T Materialize(DataReaderAdapter dataReader);

        /// <summary>
        /// Materializes list of objects from DataReader.
        /// </summary>
        /// <param name="dataReader">DataReader packed to the helper class DataReaderAdapter.</param>
        /// <returns>List of T instances.</returns>
        /// <remarks>All result sets are read within this method.</remarks>
        List<T> Materialize_List(DataReaderAdapter dataReader);
    }
}