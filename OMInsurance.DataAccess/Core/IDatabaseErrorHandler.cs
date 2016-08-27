using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace OMInsurance.DataAccess.Core
{
    /// <summary>
    /// Provides method that accepts SqlException and converts it into some
    /// business-logic exception.
    /// </summary>
    public interface IDatabaseErrorHandler
    {
        /// <summary>
        /// Transforms specified SqlException instance into some business-logic
        /// exception, whenever specified exception may be recognised.
        /// </summary>
        /// <param name="sqlException">SqlException to be recognised and converted.</param>
        /// <param name="procedureName">Name of stored procedure that has thrown
        /// the exception.</param>
        /// <param name="commandParameters">List of parameters that were passed
        /// to stored procedure.</param>
        /// <returns>Exception to which original exception was transformed. If
        /// specified exception wasn't recognised, then returns null.</returns>
        Exception TransformException(SqlException sqlException, string procedureName, List<SqlParameter> commandParameters);
    }
}
