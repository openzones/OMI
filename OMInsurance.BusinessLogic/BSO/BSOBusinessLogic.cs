using OMInsurance.DataAccess.DAO;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.Interfaces;
using System.Collections.Generic;
using OMInsurance.Entities.Searching;
using OMInsurance.Entities.Sorting;
using System;

namespace OMInsurance.BusinessLogic
{
    public class BSOBusinessLogic : IBSOBusinessLogic
    {

        public List<BSOHistoryItem> BSO_GetListHistory(DateTime StatusDateFrom, DateTime StatusDateTo, List<long> BSO_IDs)
        {
            return BSODao.Instance.BSO_GetListHistory(StatusDateFrom, StatusDateTo, BSO_IDs);
        }

        public List<BSOSumStatus> BSO_GetSumAllStatus(DateTime data)
        {
            return BSODao.Instance.BSO_GetSumAllStatus(data);
        }

        public void BSO_Set(BSO.SaveData data)
        {
            BSODao.Instance.BSO_Set(data);
        }

        public long BSO_Save(BSO.SaveData data)
        {
            return BSODao.Instance.BSO_Save(data);
        }

        public BSO BSO_GetByID(long bso_id)
        {
            return BSODao.Instance.BSO_GetByID(bso_id);
        }

        public BSO BSO_GetByNumber(string temporaryPolicyNumber)
        {
            return BSODao.Instance.BSO_GetByNumber(temporaryPolicyNumber);
        }

        public List<BSOStatusRef> BSO_GetListStatus()
        {
            return BSODao.Instance.BSO_GetListStatus();
        }

        public DataPage<BSOInfo> BSO_Find(
            BSOSearchCriteria criteria,
            List<SortCriteria<BSOSortField>> sortCriteria,
            PageRequest pageRequest)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException("criteria can't be null");
            }

            if (sortCriteria == null)
            {
                throw new ArgumentNullException("sortCriteria can't be null");
            }

            if (pageRequest == null)
            {
                throw new ArgumentNullException("pageRequest can't be null");
            }
            return BSODao.Instance.BSO_Find(criteria, sortCriteria, pageRequest);
        }
    }
}
