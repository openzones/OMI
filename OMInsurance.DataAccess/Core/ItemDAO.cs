using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using OMInsurance.DataAccess.Core.Helpers;

namespace OMInsurance.DataAccess.Core
{
    /// <summary>
    /// Base class for all data access objects that operate with entities of some type.
    /// </summary>
    public abstract class ItemDao
    {
        #region Fields

        private string _databaseAlias;
        private IDatabaseErrorHandler _errorHandler;

        #endregion

        #region Properties

        /// <summary>
        /// Gets alias of the database associated with this data access object.
        /// </summary>
        protected string DatabaseAlias
        {
            get
            {
                return _databaseAlias;
            }
        }

        /// <summary>
        /// Gets the connection with the database associated with this
        /// data access object.
        /// </summary>
        protected SqlConnection GetConnection()
        {
            return DbHelper.GetConnection(_databaseAlias);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates new instance of type <see cref="ItemDao"/>.
        /// </summary>
        /// <param name="databaseAlias">Alias of the database that should be used
        /// by this data access object.</param>
        protected ItemDao(string databaseAlias)
        {
            _databaseAlias = databaseAlias;
            _errorHandler = null;
        }

        /// <summary>
        /// Creates new instance of type <see cref="ItemDao"/>.
        /// </summary>
        /// <param name="databaseAlias">Alias of the database that should be used
        /// by this data access object.</param>
        /// <param name="errorHandler">Object that can transform SqlException to
        /// meaningful back-end exception.</param>
        protected ItemDao(string databaseAlias, IDatabaseErrorHandler errorHandler)
        {
            _databaseAlias = databaseAlias;
            _errorHandler = errorHandler;
        }

        #endregion

        /// <summary>
        /// Executes stored procedure that returns a data reader and passes this
        /// data reader to specified handler.
        /// </summary>
        /// <param name="procedureName">Name of stored procedure to execute.</param>
        /// <param name="commandParameters">Parameters of stored procedure.</param>
        /// <param name="dataReaderHandler">Delegate that accepts the data reader
        /// and reads results from it.</param>
        protected void Execute_Reader(
            string procedureName,
            List<SqlParameter> commandParameters,
            Action<DataReaderAdapter> dataReaderHandler)
        {
            if (procedureName == null)
            {
                throw new ArgumentNullException("procedureName");
            }

            try
            {
                using (DataReaderAdapter reader = DbHelper.ExecuteReaderEx(DatabaseAlias, procedureName, commandParameters))
                {
                    dataReaderHandler(reader);
                }
            }
            catch (SqlException e)
            {
                ThrowRecognisedException(e, procedureName, commandParameters);
                throw;
            }
        }

        /// <summary>
        /// Create and Read single object from database.
        /// </summary>
        /// <param name="procedureName">Stored procedure name.</param>
        /// <param name="commandParameters">Sql parameters.</param>
        /// <returns>Instance of object T.</returns>
        protected T Execute_Get<T>(IMaterializer<T> materializer, string procedureName, List<SqlParameter> commandParameters)
            where T : class
        {
            if (procedureName == null)
            {
                throw new ArgumentNullException("procedureName");
            }

            T result = null;
            try
            {
                using (DataReaderAdapter reader = DbHelper.ExecuteReaderEx(DatabaseAlias, procedureName, commandParameters))
                {
                    result = materializer.Materialize(reader);
                }
            }
            catch (SqlException e)
            {
                ThrowRecognisedException(e, procedureName, commandParameters);
                throw;
            }

            return result;
        }

        /// <summary>
        /// Create and Read list of object from database.
        /// </summary>
        /// <param name="procedureName">Stored procedure name.</param>
        /// <param name="commandParameters">Sql parameters.</param>
        /// <returns>List of objects T.</returns>
        protected List<T> Execute_GetList<T>(IMaterializer<T> materializer, string procedureName, List<SqlParameter> commandParameters)
            where T : class
        {
            if (procedureName == null)
            {
                throw new ArgumentNullException("procedureName");
            }

            List<T> result;
            try
            {
                using (DataReaderAdapter reader = DbHelper.ExecuteReaderEx(DatabaseAlias, procedureName, commandParameters))
                {
                    result = materializer.Materialize_List(reader);
                }
            }
            catch (SqlException e)
            {
                ThrowRecognisedException(e, procedureName, commandParameters);
                throw;
            }

            return result;
        }

        /// <summary>
        /// Create and Read object from database.
        /// </summary>
        /// <param name="query">SQL query to execute.</param>
        /// <returns>Object T.</returns>
        protected T Execute_Query_Get<T>(IMaterializer<T> materializer, string query, List<SqlParameter> commandParameters)
            where T : class
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            T result;
            try
            {
                using (DataReaderAdapter reader = DbHelper.ExecuteQueryReaderEx(DatabaseAlias, query, commandParameters))
                {
                    result = materializer.Materialize(reader);
                }
            }
            catch (SqlException e)
            {
                ThrowRecognisedException(e, "Custom SQL query", new List<SqlParameter>());
                throw;
            }

            return result;
        }

        /// <summary>
        /// Execute query
        /// </summary>
        protected void Execute_Query(string query, List<SqlParameter> commandParameters)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            try
            {
                DbHelper.ExecuteQuery(DatabaseAlias, query, commandParameters);
            }
            catch (SqlException e)
            {
                ThrowRecognisedException(e, "Custom SQL query", new List<SqlParameter>());
                throw;
            }
        }

        /// <summary>
        /// This method for safety close connection after execute stored procedure.
        /// </summary>
        /// <param name="procedureName">Stored procedure name.</param>
        /// <param name="commandParameters">Sql parameters.</param>
        /// <returns>Execution result.</returns>
        protected int Execute_StoredProcedure(string procedureName, List<SqlParameter> commandParameters)
        {
            int result;

            try
            {
                result = DbHelper.ExecuteProcedure(DatabaseAlias, procedureName, commandParameters);
            }
            catch (SqlException e)
            {
                ThrowRecognisedException(e, procedureName, commandParameters);
                throw;
            }

            return result;
        }

        /// <summary>
        /// Execute scalar procedure.
        /// </summary>
        /// <typeparam name="T">Type of result value.</typeparam>
        /// <param name="procedureName">Stored procedure name.</param>
        /// <param name="commandParameters">Sql parameters.</param>
        /// <returns>Scalar value, result of execution.</returns>
        protected T Execute_ScalarStoredProcedure<T>(string procedureName, List<SqlParameter> commandParameters)
        {
            try
            {
                return DbHelper.ExecuteScalarProcedure<T>(DatabaseAlias, procedureName, commandParameters);
            }
            catch (SqlException e)
            {
                ThrowRecognisedException(e, procedureName, commandParameters);
                throw;
            }
        }

        /// <summary>
        /// Passes specified SqlException to error handler, that tries to
        /// recognise and convert the exception to meaningful back-end exception.
        /// If exception wasn't recognised, then this method silently returns,
        /// otherwise it throws recognised exception.
        /// </summary>
        /// <param name="sqlException">Instance of SqlException.</param>
        /// <param name="procedureName">Name of stored procedure.</param>
        /// <param name="commandParameters">Procedure parameters.</param>
        private void ThrowRecognisedException(
            SqlException sqlException,
            string procedureName,
            List<SqlParameter> commandParameters)
        {
            if (_errorHandler != null)
            {
                Exception transformedException = _errorHandler.TransformException(
                    sqlException, procedureName, commandParameters);

                // If handler recognised the exception (i.e. returned not-null
                // transformed exception), then we throw this transformed exception
                if (transformedException != null)
                {
                    throw transformedException;
                }
            }
        }
    }
}