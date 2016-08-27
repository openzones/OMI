using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System;
using System.Collections.Generic;

namespace OMInsurance.DataAccess.Materializers
{
    public class FundResponseMaterializer : IMaterializer<FundResponse>
    {
        private static readonly FundResponseMaterializer _instance = new FundResponseMaterializer();

        public static FundResponseMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }
        public FundResponse Materialize(DataReaderAdapter dataReader)
        { 
            if (dataReader.Read())
            {
                string type = dataReader.GetString("FundResponseType");
                if (type.Equals(S5FundResponse.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return S5FundResponseMaterializer.Instance.ReadItemFields(dataReader);
                }
                if (type.Equals(S6FundResponse.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return S6FundResponseMaterializer.Instance.ReadItemFields(dataReader);
                }
                if (type.Equals(S9FundResponse.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return S9FundResponseMaterializer.Instance.ReadItemFields(dataReader);
                }
                if (type.Equals(SnilsFundResponse.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return SnilsFundResponseMaterializer.Instance.ReadItemFields(dataReader);
                }
                if (type.Equals(SvdFundResponse.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return SvdFundResponseMaterializer.Instance.ReadItemFields(dataReader);
                }
                if (type.Equals(FundErrorResponse.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return FundErrorResponseMaterializer.Instance.ReadItemFields(dataReader);
                }
                if (type.Equals(GoznakResponse.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return GoznakResponseMaterializer.Instance.ReadItemFields(dataReader);
                }
            }
            return null;
        }

        public List<FundResponse> Materialize_List(DataReaderAdapter dataReader)
        {
            List<FundResponse> items = new List<FundResponse>();

            items.AddRange(S5FundResponseMaterializer.Instance.Materialize_List(dataReader));
            dataReader.NextResult();
            items.AddRange(S6FundResponseMaterializer.Instance.Materialize_List(dataReader));
            dataReader.NextResult();
            items.AddRange(S9FundResponseMaterializer.Instance.Materialize_List(dataReader));
            dataReader.NextResult();
            items.AddRange(SnilsFundResponseMaterializer.Instance.Materialize_List(dataReader));
            dataReader.NextResult();
            items.AddRange(SvdFundResponseMaterializer.Instance.Materialize_List(dataReader));
            dataReader.NextResult();
            items.AddRange(FundErrorResponseMaterializer.Instance.Materialize_List(dataReader));
            dataReader.NextResult();
            items.AddRange(GoznakResponseMaterializer.Instance.Materialize_List(dataReader));
            return items;
        }
    }

}
