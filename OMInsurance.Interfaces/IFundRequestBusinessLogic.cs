using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.Entities.Searching;
using OMInsurance.Entities.Sorting;
using System;
using System.Collections.Generic;

namespace OMInsurance.Interfaces
{
    public interface IFundRequestBusinessLogic
    {
        List<FundResponse.UploadReportData> UploadFundResponse(User user, string zipPath, bool runScenario);
        List<FundResponse.UploadReportData> UploadFundResponseUnion(User user, string zipPath, bool runScenario);
        List<FundResponse.UploadReportData> UploadSubmitFundResponse(User user, string zipPath);
        List<FundResponsePackage> FundResponsePackage_GetListByClientVisitId(long clientVisitId);
        void ApplyResponse(User user, FundResponseCopyFields fundResponseCopyFields);
        byte[] GetFullRequestPackage(DataPage<ClientVisitInfo> clientVisitInfo);
        byte[] FundSubmitRequest_Get(DataPage<ClientVisitInfo> clientVisitInfo, string filename);
        string FundSubmitRequest_GetName(DateTime date);
        List<FundResponse.UploadReportData> Upload_SecondStepReconciliationPack(User user, string zipPath);
        void ClientVisit_SetReadyToFundSubmitRequest(User user, long id, bool isReady, string message);
        void ClientVisits_SetDifficultCase(long p1, bool isDifficult);
        void FundFileHistory_Save(List<FundFileHistory> list);
        List<FundFileHistory> FundFileHistory_Get(long VisitGroupID);
    }
}
