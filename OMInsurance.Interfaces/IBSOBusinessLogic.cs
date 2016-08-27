using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.Collections.Generic;
using OMInsurance.Entities.Searching;
using OMInsurance.Entities.Sorting;
using System;

namespace OMInsurance.Interfaces
{
    public interface IBSOBusinessLogic
    {
        List<BSOHistoryItem> BSO_GetListHistory(DateTime StatusDateFrom, DateTime StatusDateTo, List<long> BSO_IDs);
        List<BSOSumStatus> BSO_GetSumAllStatus(DateTime data);
        long BSO_Save(BSO.SaveData data);
        void BSO_Set(BSO.SaveData data);
        BSO BSO_GetByID(long bso_id);
        BSO BSO_GetByNumber(string temporaryPolicyNumber);
        List<BSOStatusRef> BSO_GetListStatus();
        DataPage<BSOInfo> BSO_Find(
            BSOSearchCriteria criteria,
            List<SortCriteria<BSOSortField>> sortCriteria,
            PageRequest pageRequest);
    }
}
