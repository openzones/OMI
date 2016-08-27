using OMInsurance.DataAccess.DAO;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.Entities.Core.Exeptions;
using OMInsurance.Entities.Searching;
using OMInsurance.Entities.Sorting;
using OMInsurance.Entities.Check;
using OMInsurance.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace OMInsurance.BusinessLogic
{
    public class CheckBusinessLogic : ICheckBusinessLogic
    {
        public List<CheckClient> Check_Client(CheckClientSearchCriteria criteria)
        {
            return CheckDao.Instance.Check_Client(criteria);
        }

        public List<FundFileHistory> FundFileHistory_Find(CheckFileHistorySearchCriteria criteria)
        {
            return CheckDao.Instance.FundFileHistory_Find(criteria);
        }

        public List<ClientPretension> ClientPretension_Find(CheckPretensionSearchCriteria criteria)
        {
            return CheckDao.Instance.ClientPretension_Find(criteria);
        }

    }
}
