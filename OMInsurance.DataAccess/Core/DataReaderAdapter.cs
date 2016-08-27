using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace OMInsurance.DataAccess.Core
{
    /// <summary>
    /// Wrapper for SqlDataReader. It increases performace when reading column values by
    /// column name (example: reader.GetValue("column1")).
    /// </summary>
    public sealed class DataReaderAdapter : IDisposable
    {
        private SqlDataReader _dataReader;
        private Dictionary<string, int> _rowSchema;

        public DataReaderAdapter(SqlDataReader dataReader)
        {
            if (dataReader == null)
            {
                throw new ArgumentNullException("dataReader");
            }

            _dataReader = dataReader;

            _rowSchema = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                if (_rowSchema.ContainsKey(dataReader.GetName(i)))
                {
                    _rowSchema.Add(dataReader.GetName(i), i);
                }
            }
        }

        public T GetValue<T>(string field)
        {
            return GetValue<T>(field, default(T));
        }

        public T GetValue<T>(string field, T defaultValue)
        {
            int index = GetOrdinal(field);
            return GetValue<T>(index, defaultValue);
        }

        public T GetValue<T>(int index)
        {
            return GetValue<T>(index, default(T));
        }

        public T GetValue<T>(int index, T defaultValue)
        {
            return _dataReader.IsDBNull(index) ? defaultValue : (T)_dataReader.GetValue(index);
        }

        public object GetValue(int index)
        {
            if (_dataReader.IsDBNull(index))
            {
                return null;
            }
            else
            {
                return _dataReader.GetValue(index);
            }
        }

        public string GetName(int index)
        {
            return _dataReader.GetName(index);
        }

        public int GetFieldCount()
        {
            return _dataReader.FieldCount;
        }

        public bool IsNull(string field)
        {
            return _dataReader.IsDBNull(GetOrdinal(field));
        }

        public bool IsNull(int field)
        {
            return _dataReader.IsDBNull(field);
        }

        public bool IsNotNull(string field)
        {
            return !IsNull(field);
        }

        public bool IsNotNull(int field)
        {
            return !IsNull(field);
        }

        public bool GetBoolean(string field)
        {
            bool? nullableValue = GetBooleanNull(field);
            return nullableValue.Value;
        }

        public bool? GetBooleanNull(string field)
        {
            int index = GetOrdinal(field);
            return _dataReader.IsDBNull(index) ? (bool?)null : _dataReader.GetBoolean(index);
        }

        public byte GetByte(string field)
        {
            byte? nullableValue = GetByteNull(field);
            return nullableValue.Value;
        }

        public byte? GetByteNull(string field)
        {
            int index = GetOrdinal(field);
            return _dataReader.IsDBNull(index) ? (byte?)null : _dataReader.GetByte(index);
        }

        public long GetBytes(string field, long fieldOffset, byte[] buffer, int bufferOffset, int length)
        {
            int index = GetOrdinal(field);
            return _dataReader.GetBytes(index, fieldOffset, buffer, bufferOffset, length);
        }

        public byte[] GetBytes(string field, int length)
        {
            byte[] buffer = new byte[length];

            long actualLength = GetBytes(field, 0, buffer, 0, length);
            if (actualLength != length)
            {
                throw new ArgumentException(string.Format("Number of bytes '{0}' specified for field '{1}' is different from actual data size '{2}'.", length, field, actualLength));
            }

            return buffer;
        }

        public char GetChar(string field)
        {
            char? nullableValue = GetCharNull(field);
            return nullableValue.Value;
        }

        public char? GetCharNull(string field)
        {
            int index = GetOrdinal(field);
            return _dataReader.IsDBNull(index) ? (char?)null : _dataReader.GetChar(index);
        }

        public long GetChars(string field, long fieldOffset, char[] buffer, int bufferOffset, int length)
        {
            return _dataReader.GetChars(GetOrdinal(field), fieldOffset, buffer, bufferOffset, length);
        }

        public DateTime GetDateTime(string field)
        {
            DateTime? nullableValue = GetDateTimeNull(field);
            return nullableValue.Value;
        }

        public DateTime? GetDateTimeNull(string field)
        {
            int index = GetOrdinal(field);

            if (_dataReader.IsDBNull(index))
            {
                return null;
            }
            else
            {
                DateTime dateTime = _dataReader.GetDateTime(index);
                return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            }
        }

        public DateTimeOffset GetDateTimeOffset(string field)
        {
            DateTimeOffset? nullableValue = GetDateTimeOffset(field);
            return nullableValue.Value;
        }

        public DateTimeOffset? GetDateTimeOffsetNull(string field)
        {
            int index = GetOrdinal(field);
            return _dataReader.IsDBNull(index) ? (DateTimeOffset?)null : _dataReader.GetDateTimeOffset(index);
        }

        public decimal GetDecimal(string field)
        {
            decimal? nullableValue = GetDecimalNull(field);
            return nullableValue.Value;
        }

        public decimal? GetDecimalNull(string field)
        {
            int index = GetOrdinal(field);
            return _dataReader.IsDBNull(index) ? (decimal?)null : _dataReader.GetDecimal(index);
        }

        public double GetDouble(string field)
        {
            double? nullableValue = GetDoubleNull(field);
            return nullableValue.Value;
        }

        public double? GetDoubleNull(string field)
        {
            int index = GetOrdinal(field);
            return _dataReader.IsDBNull(index) ? (double?)null : _dataReader.GetDouble(index);
        }

        public float GetFloat(string field)
        {
            float? nullableValue = GetFloatNull(field);
            return nullableValue.Value;
        }

        public float? GetFloatNull(string field)
        {
            int index = GetOrdinal(field);
            return _dataReader.IsDBNull(index) ? (float?)null : _dataReader.GetFloat(index);
        }

        public Guid GetGuid(string field)
        {
            Guid? nullableValue = GetGuidNull(field);
            return nullableValue.Value;
        }

        public Guid? GetGuidNull(string field)
        {
            int index = GetOrdinal(field);
            return _dataReader.IsDBNull(index) ? (Guid?)null : _dataReader.GetGuid(index);
        }

        public short GetInt16(string field)
        {
            short? nullableValue = GetInt16Null(field);
            return nullableValue.Value;
        }

        public short? GetInt16Null(string field)
        {
            int index = GetOrdinal(field);
            return _dataReader.IsDBNull(index) ? (short?)null : _dataReader.GetInt16(index);
        }

        public int GetInt32(string field)
        {
            int? nullableValue = GetInt32Null(field);
            return nullableValue.Value;
        }

        public int? GetInt32Null(string field)
        {
            int index = GetOrdinal(field);
            return _dataReader.IsDBNull(index) ? (int?)null : _dataReader.GetInt32(index);
        }

        public long GetInt64(string field)
        {
            long? nullableValue = GetInt64Null(field);
            return nullableValue.Value;
        }

        public long? GetInt64Null(string field)
        {
            int index = GetOrdinal(field);
            if (_dataReader.IsDBNull(index))
            {
                return null;
            }
            return _dataReader.GetInt64(index);
        }

        public T GetEnum<T>(string field)
            where T : struct
        {
            T? nullableValue = GetEnumNull<T>(field);
            return nullableValue.Value;
        }

        public T? GetEnumNull<T>(string field)
            where T : struct
        {
            int index = GetOrdinal(field);

            if (_dataReader.IsDBNull(index))
            {
                return null;
            }
            else
            {
                string valueAsString = _dataReader.GetString(index);
                return (T?)Enum.Parse(typeof(T), valueAsString);
            }
        }

        public string GetString(string field)
        {
            int index = GetOrdinal(field);
            return _dataReader.IsDBNull(index) ? null : _dataReader.GetString(index);
        }

        public bool NextResult()
        {
            _rowSchema.Clear();
            return _dataReader.NextResult();
        }

        public bool Read()
        {
            return _dataReader.Read();
        }

        #region Helper methods

        public bool HasField(string field)
        {
            int index;
            return _rowSchema.TryGetValue(field, out index);
        }

        public int GetOrdinal(string field)
        {
            int index;
            if (_rowSchema.TryGetValue(field, out index))
            {
                return index;
            }
            else
            {
                try
                {
                    index = _dataReader.GetOrdinal(field);
                }
                catch (IndexOutOfRangeException e)
                {
                    throw new ArgumentException(string.Format("Invalid column name '{0}'.", field), "field", e);
                }
                _rowSchema.Add(field, index);
                return index;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _dataReader.Dispose();
        }

        #endregion
    }
}