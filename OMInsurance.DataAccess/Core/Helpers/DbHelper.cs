using OMInsurance.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace OMInsurance.DataAccess.Core.Helpers
{
    /// <summary>
    /// Database access helper class.
    /// </summary>
    public sealed class DbHelper
    {
        private DbHelper()
        {
        }

        #region Connection management

        /// <summary>
        /// Get the connection string for database.
        /// </summary>
        /// <param name="databaseAlias">Database alias in the configuration file.</param>
        /// <returns>Connection string.</returns>
        public static string GetConnectionString(string databaseAlias)
        {
            string connStr = ConfiguraionProvider.DatabaseConnectionString;

            return connStr;
        }

        /// <summary>
        /// Get the connection for database.
        /// </summary>
        /// <param name="databaseAlias">Database alias in the configuration file.</param>
        /// <returns>Connection.</returns>
        public static SqlConnection GetConnection(string databaseAlias)
        {
            return new SqlConnection(GetConnectionString(databaseAlias));
        }

        /// <summary>
        /// Open the connection.
        /// </summary>
        /// <param name="connection">Connection.</param>
        public static void OpenConnection(SqlConnection connection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        /// <summary>
        /// Close connection.
        /// </summary>
        /// <param name="connection">Connection.</param>
        public static void CloseConnection(SqlConnection connection)
        {
            if (connection != null && connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }
        }

        #endregion

        #region SqlCommand helper methods

        /// <summary>
        /// Create SQL command.
        /// </summary>
        /// <param name="connection">SQL connection.</param>
        /// <param name="procedureName">Name of stored procedure.</param>
        /// <param name="parameters">List of parameters.</param>
        /// <returns>SQL command.</returns>
        public static SqlCommand CreateSqlCommand(SqlConnection connection, string procedureName, List<SqlParameter> parameters)
        {
            SqlCommand cmd = new SqlCommand()
            {
                CommandText = procedureName,
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = connection.ConnectionTimeout
            };

            ApplyParameters(cmd, parameters);

            return cmd;
        }

        /// <summary>
        /// Create and execute sql command without return parameters.
        /// </summary>
        /// <param name="databaseAlias">Database alias in the configuration file.</param>
        /// <param name="query">Query</param>
        /// <param name="parameters">List of parameters.</param>
        public static void ExecuteQuery(string databaseAlias, string query, List<SqlParameter> commandParameters)
        {
            SqlConnection connection = GetConnection(databaseAlias);
            try
            {
                SqlCommand dbCommand = CreateSqlCommand(connection, query, commandParameters);
                OpenConnection(connection);
                dbCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseConnection(connection);
            }
        }

        /// <summary>
        /// Create and execute stored procedure without return parameters.
        /// </summary>
        /// <param name="databaseAlias">Database alias in the configuration file.</param>
        /// <param name="procedureName">Name of stored procedure.</param>
        /// <param name="parameters">List of parameters.</param>
        /// <returns>Return value.</returns>
        public static int ExecuteProcedure(string databaseAlias, string procedureName, List<SqlParameter> parameters)
        {
            return ExecuteProcedure(GetConnection(databaseAlias), procedureName, parameters);
        }

        /// <summary>
        /// Create and execute stored procedure without return parameters.
        /// </summary>
        /// <param name="connection">SQL connection.</param>
        /// <param name="procedureName">Name of stored procedure.</param>
        /// <param name="parameters">List of parameters.</param>
        /// <returns>Return value.</returns>
        public static int ExecuteProcedure(SqlConnection connection, string procedureName, List<SqlParameter> parameters)
        {
            int returnValue = 0;
            try
            {
                SqlCommand dbCommand = CreateSqlCommand(connection, procedureName, parameters);
                OpenConnection(connection);
                returnValue = dbCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseConnection(connection);
            }
            return returnValue;
        }

        /// <summary>
        /// Create and execute scalar stored procedure.
        /// </summary>
        /// <param name="databaseAlias">Database alias in the configuration file.</param>
        /// <param name="procedureName">Name of stored procedure.</param>
        /// <param name="parameters">List of parameters.</param>
        /// <returns>Scalar value.</returns>
        public static T ExecuteScalarProcedure<T>(string databaseAlias, string procedureName, List<SqlParameter> parameters)
        {
            return ExecuteScalarProcedure<T>(GetConnection(databaseAlias), procedureName, parameters);
        }

        /// <summary>
        /// Create and execute scalar stored procedure.
        /// </summary>
        /// <param name="connection">SQL connection.</param>
        /// <param name="procedureName">Name of stored procedure.</param>
        /// <param name="parameters">List of parameters.</param>
        /// <returns>Scalar value.</returns>
        public static T ExecuteScalarProcedure<T>(SqlConnection connection, string procedureName, List<SqlParameter> parameters)
        {
            try
            {
                SqlCommand dbCommand = CreateSqlCommand(connection, procedureName, parameters);
                OpenConnection(connection);

                object result = dbCommand.ExecuteScalar();
                if (result == DBNull.Value)
                {
                    return default(T);
                }
                else
                {
                    return (T)result;
                }
            }
            finally
            {
                CloseConnection(connection);
            }
        }

        /// <summary>
        /// Create and execute procedure .
        /// </summary>
        /// <param name="databaseAlias">Database alias in the configuration file.</param>
        /// <param name="procedureName">Name of stored procedure.</param>
        /// <param name="parameters">List of parameters.</param>
        /// <returns>DataReader.</returns>
        public static SqlDataReader ExecuteReader(string databaseAlias, string procedureName, List<SqlParameter> parameters)
        {
            return ExecuteReader(GetConnection(databaseAlias), procedureName, parameters);
        }

        /// <summary>
        /// Create and execute procedure.
        /// </summary>
        /// <param name="connection">SQL connection.</param>
        /// <param name="procedureName">Name of stored procedure.</param>
        /// <param name="parameters">List of parameters.</param>
        /// <returns>DataReader.</returns>
        public static SqlDataReader ExecuteReader(SqlConnection connection, string procedureName, List<SqlParameter> parameters)
        {
            try
            {
                SqlCommand dbCommand = CreateSqlCommand(connection, procedureName, parameters);
                OpenConnection(connection);
                return dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception)
            {
                CloseConnection(connection);
                throw;
            }
        }

        /// <summary>
        /// Create and execute SQL query.
        /// </summary>
        /// <param name="connection">SQL connection.</param>
        /// <param name="query">SQL query text.</param>
        /// <param name="parameters">List of parameters.</param>
        /// <returns>DataReader.</returns>
        public static SqlDataReader ExecuteQueryReader(SqlConnection connection, string query, List<SqlParameter> parameters)
        {
            try
            {
                SqlCommand dbCommand = new SqlCommand()
                {
                    CommandText = query,
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandTimeout = connection.ConnectionTimeout
                };
                ApplyParameters(dbCommand, parameters);
                OpenConnection(connection);
                return dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception)
            {
                CloseConnection(connection);
                throw;
            }
        }

        /// <summary>
        /// Create and execute procedure.
        /// </summary>
        /// <param name="databaseAlias">Database alias in the configuration file.</param>
        /// <param name="procedureName">Name of stored procedure.</param>
        /// <param name="parameters">List of parameters.</param>
        /// <returns>DataReaderAdapter</returns>
        public static DataReaderAdapter ExecuteReaderEx(string databaseAlias, string procedureName, List<SqlParameter> parameters)
        {
            return ExecuteReaderEx(GetConnection(databaseAlias), procedureName, parameters);
        }

        /// <summary>
        /// Create and execute procedure.
        /// </summary>
        /// <param name="connection">SQL connection.</param>
        /// <param name="procedureName">Name of stored procedure.</param>
        /// <param name="parameters">List of parameters.</param>
        /// <returns>DataReaderAdapter</returns>
        public static DataReaderAdapter ExecuteReaderEx(SqlConnection connection, string procedureName, List<SqlParameter> parameters)
        {
            return new DataReaderAdapter(ExecuteReader(connection, procedureName, parameters));
        }

        /// <summary>
        /// Create and execute procedure.
        /// </summary>
        /// <param name="databaseAlias">Database alias in the configuration file.</param>
        /// <param name="procedureName">Name of stored procedure.</param>
        /// <param name="parameters">List of parameters.</param>
        /// <returns>DataTable</returns>
        public static DataTable ExecuteDataTableEx(string databaseAlias, string procedureName, List<SqlParameter> parameters)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = GetConnection(databaseAlias))
            using (SqlCommand command = new SqlCommand(procedureName, connection))
            {
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                {
                    command.Parameters.AddRange(parameters.ToArray());
                    command.CommandType = CommandType.StoredProcedure;
                    dataAdapter.Fill(table);
                }
            }
            return table;
        }

        /// <summary>
        /// Create and execute SQL query.
        /// </summary>
        /// <param name="databaseAlias">Database alias in the configuration file.</param>
        /// <param name="query">SQL query text.</param>
        /// <param name="parameters">List of parameters.</param>
        /// <returns>DataReaderAdapter</returns>
        public static DataReaderAdapter ExecuteQueryReaderEx(string databaseAlias, string query, List<SqlParameter> parameters)
        {
            return ExecuteQueryReaderEx(GetConnection(databaseAlias), query, parameters);
        }

        /// <summary>
        /// Create and execute SQL query.
        /// </summary>
        /// <param name="connection">SQL connection.</param>
        /// <param name="query">SQL query text.</param>
        /// <param name="parameters">List of parameters.</param>
        /// <returns>DataReaderAdapter</returns>
        private static DataReaderAdapter ExecuteQueryReaderEx(SqlConnection connection, string query, List<SqlParameter> parameters)
        {
            return new DataReaderAdapter(ExecuteQueryReader(connection, query, parameters));
        }

        #endregion

        #region Helper methods for SQL parameters creation

        /// <summary>
        /// Create input parameter.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        /// <param name="parameterType">Parameter type.</param>
        /// <param name="parameterValue">Parameter value.</param>
        /// <returns>Parameter.</returns>
        public static SqlParameter CreateInputParameter(string parameterName, SqlDbType parameterType, object parameterValue)
        {
            return CreateParameter(parameterName, parameterType, parameterValue, ParameterDirection.Input);
        }

        /// <summary>
        /// Create output parameter.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        /// <param name="parameterType">Parameter type.</param>
        /// <returns>Parameter.</returns>
        public static SqlParameter CreateOutputParameter(string parameterName, SqlDbType parameterType)
        {
            return new SqlParameter()
            {
                ParameterName = parameterName,
                SqlDbType = parameterType,
                Direction = ParameterDirection.Output,
            };
        }

        /// <summary>
        /// Create output parameter with limitation on value size.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        /// <param name="parameterType">Parameter type.</param>
        /// <param name="size">Maximal size of parameter value.</param>
        /// <returns>Parameter.</returns>
        public static SqlParameter CreateOutputParameter(string parameterName, SqlDbType parameterType, int size)
        {
            return new SqlParameter()
            {
                ParameterName = parameterName,
                SqlDbType = parameterType,
                Size = size,
                Direction = ParameterDirection.Output,
            };
        }

        /// <summary>
        /// Create input/output parameter.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        /// <param name="parameterType">Parameter type.</param>
        /// <param name="parameterValue">Parameter value.</param>
        /// <returns>Parameter.</returns>
        public static SqlParameter CreateInputOutputParameter(string parameterName, SqlDbType parameterType, object parameterValue)
        {
            return CreateParameter(parameterName, parameterType, parameterValue, ParameterDirection.InputOutput);
        }

        /// <summary>
        /// Create parameter.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        /// <param name="parameterType">Parameter type.</param>
        /// <param name="parameterValue">Parameter value.</param>
        /// <param name="parameterDirection">Parameter direction.</param>
        /// <returns>Parameter.</returns>
        public static SqlParameter CreateParameter(string parameterName, SqlDbType parameterType, object parameterValue,
            ParameterDirection parameterDirection)
        {
            return new SqlParameter()
            {
                ParameterName = parameterName,
                SqlDbType = parameterType,
                Direction = parameterDirection,
                Value = parameterValue
            };
        }

        private static void ApplyParameters(SqlCommand dbCommand, List<SqlParameter> parameters)
        {
            if (parameters != null)
            {
                foreach (SqlParameter param in parameters)
                {
                    if (param.Direction != ParameterDirection.Output && param.Value == null)
                    {
                        param.Value = DBNull.Value;
                    }

                    if (param.Direction != ParameterDirection.Output && param.Value.Equals(string.Empty)
                        && (param.SqlDbType == SqlDbType.NText || param.SqlDbType == SqlDbType.NVarChar
                            || param.SqlDbType == SqlDbType.Text || param.SqlDbType == SqlDbType.VarChar))
                    {
                        param.Value = DBNull.Value;
                    }

                    dbCommand.Parameters.Add(param);
                }
            }
        }

        #endregion

        #region Helper methods for sql parameters reading

        /// <summary>
        /// Extract value from parameters collection.
        /// </summary>
        /// <typeparam name="T">Parameter type.</typeparam>
        /// <param name="parameters">Parameters collection.</param>
        /// <param name="parameterName">Parameter Name.</param>
        /// <returns>Value.</returns>
        public static T GetParameterValue<T>(List<SqlParameter> parameters, string parameterName)
        {
            SqlParameter param = parameters.SingleOrDefault(a => a.ParameterName.Equals(parameterName, StringComparison.Ordinal));
            return GetParameterValue<T>(param);
        }

        /// <summary>
        /// Extract value from command.
        /// </summary>
        /// <typeparam name="T">Parameter type.</typeparam>
        /// <param name="cmd">SQL Command.</cmd>
        /// <param name="parameterName">Parameter Name.</param>
        /// <returns>Value.</returns>
        public static T GetParameterValue<T>(SqlCommand cmd, string parameterName)
        {
            SqlParameter param = cmd.Parameters[parameterName];
            return GetParameterValue<T>(param);
        }

        private static T GetParameterValue<T>(SqlParameter parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException("parameter");
            }
            if (parameter.Value == DBNull.Value)
            {
                return default(T);
            }

            return (T)parameter.Value;
        }

        #endregion
    }
}