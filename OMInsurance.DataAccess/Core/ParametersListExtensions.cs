using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using OMInsurance.DataAccess.Core.Helpers;

namespace OMInsurance.DataAccess.Core
{
    public static class ParametersListExtensions
    {
        public static SqlParameter AddParameter(this List<SqlParameter> parameters, string parameterName, SqlDbType parameterType, object parameterValue, ParameterDirection parameterDirection)
        {
            SqlParameter parameter = DbHelper.CreateParameter(parameterName, parameterType, parameterValue, parameterDirection);
            parameters.Add(parameter);
            return parameter;
        }

        public static SqlParameter AddInputParameter(this List<SqlParameter> parameters, string parameterName, SqlDbType parameterType, object parameterValue)
        {
            SqlParameter parameter = DbHelper.CreateInputParameter(parameterName, parameterType, parameterValue);
            parameters.Add(parameter);
            return parameter;
        }

        public static SqlParameter AddOutputParameter(this List<SqlParameter> parameters, string parameterName, SqlDbType parameterType)
        {
            SqlParameter parameter = DbHelper.CreateOutputParameter(parameterName, parameterType);
            parameters.Add(parameter);
            return parameter;
        }

        public static SqlParameter AddOutputParameter(this List<SqlParameter> parameters, string parameterName, SqlDbType parameterType, int size)
        {
            SqlParameter parameter = DbHelper.CreateOutputParameter(parameterName, parameterType, size);
            parameters.Add(parameter);
            return parameter;
        }

        public static SqlParameter AddInputOutputParameter(this List<SqlParameter> parameters, string parameterName, SqlDbType parameterType, object parameterValue)
        {
            SqlParameter parameter = DbHelper.CreateInputOutputParameter(parameterName, parameterType, parameterValue);
            parameters.Add(parameter);
            return parameter;
        }
    }
}