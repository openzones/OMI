using OMInsurance.Entities.Core.Exeptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace OMInsurance.DataAccess.Core
{
    /// <summary>
    /// Handles database exceptions and transforms them into business-logic exceptions.
    /// </summary>
    public sealed class DatabaseErrorHandler : IDatabaseErrorHandler
    {
        #region Fields

        /// <summary>
        /// Error number indicating that database threw exception and stored error number in the error message.
        /// </summary>
        private const int TextFormatErrorNumber = 50000;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DatabaseErrorHandler()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Transforms specified instance of SqlException into some business-logic exception, whenever specified error number may be recognised.
        /// </summary>
        /// <param name="errorNumber">Number of error that should be recognised.</param>
        /// <param name="errorMessage">Error message (without errorNumber)</param>
        /// <param name="sqlException">SqlException to transform.</param>
        /// <param name="procedureName">Name of stored procedure that threw the exception.</param>
        /// <param name="commandParameters">List of parameters that were passed to stored procedure.</param>
        /// <returns>Transformed exception. If specified error number wasn't recognised, then returns null.</returns>
        public Exception TransformException(
            SqlException sqlException,
            string procedureName,
            List<SqlParameter> commandParameters)
        {
            switch (sqlException.Number)
            {
                case 60100:
                    return new DataObjectAlreadyExistsException("Объект с заданными параметрами уже существует", sqlException);
                case 60101:
                    return new DataObjectNotFoundException("Объект с заданными параметрами не найден", sqlException);
                default:
                    // If we've got here, then error number couldn't be recognised, so exception cannot be transformed 
                    return null;
            }
        }

        #endregion
    }
}