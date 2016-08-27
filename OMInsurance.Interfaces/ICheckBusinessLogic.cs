using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.Entities.Searching;
using OMInsurance.Entities.Check;
using OMInsurance.Entities.Sorting;
using System;
using System.Collections.Generic;

namespace OMInsurance.Interfaces
{
    public interface ICheckBusinessLogic
    {
        List<CheckClient> Check_Client(CheckClientSearchCriteria criteria);
        List<FundFileHistory> FundFileHistory_Find(CheckFileHistorySearchCriteria criteria);
        List<ClientPretension> ClientPretension_Find(CheckPretensionSearchCriteria criteria);
    }
}
