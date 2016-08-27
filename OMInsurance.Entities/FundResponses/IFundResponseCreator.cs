using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace OMInsurance.Entities
{
    public interface IFundResponseCreator
    {
        long Create(S5FundResponse.CreateData data, DateTime date);
        long Create(S6FundResponse.CreateData data, DateTime date);
        long Create(S9FundResponse.CreateData data, DateTime date);
        long Create(SnilsFundResponse.CreateData data, DateTime date);
        long Create(SvdFundResponse.CreateData data, DateTime date);
        long Create(GoznakResponse.CreateData createData, DateTime date);

        long Create(FundErrorResponse.CreateData createData, DateTime date);
    }
}
