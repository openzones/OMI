using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.Entities.Searching;
using OMInsurance.Entities.Sorting;
using System;
using System.Collections.Generic;

namespace OMInsurance.Interfaces
{
    public interface IClientBusinessLogic
    {
        DataPage<ClientBaseInfo> Client_Find(
             ClientSearchCriteria criteria,
             List<SortCriteria<ClientSortField>> sortCriteria,
             PageRequest pageRequest);
        ClientVersion ClientVersion_Get(long id);
        ClientVisitSaveResult ClientVisit_Save(User user, ClientVisit.SaveData saveData, DateTime? saveDate = null);
        Client Client_Get(User user, long id);
        ClientVisit ClientVisit_Get(long id);

        DataPage<ClientVisitInfo> ClientVisit_Find(
            ClientVisitSearchCriteria criteria, 
            List<SortCriteria<ClientVisitSortField>> list, 
            PageRequest pageRequest);

        byte[] FileReport_Get(ClientVisitSearchCriteria criteria, List<SortCriteria<ClientVisitSortField>> list, PageRequest pageRequest);
        void ClientVisit_SetStatus(User user, long id, long status, bool isActualClientVisit);
        void ClientVisit_SetStatus(User user, long id, long status, bool isActualClientVisit, DateTime? statusDate);
        List<ClientVisit.UpdateResultData> ClientVisit_UpdateFundDbf(User user, string filename);
        List<ClientVisit.UpdateResultData> ClientVisit_SetStatusesFromDbf(User CurrentUser, DateTime statusdate, string path);
        List<ClientVisit> ClientVisit_GetFromDBF(string path, List<string> parseErrors);
        List<ClientVisitHistoryItem> ClientVisitHistory_Get(long clientVisitGroupId);
        ClientVisit FindLastClientVisit(ClientVisit cv);
        void RegionPolicyData_Save(List<PolicyFromRegion> listPolicy);
        long Clients_Merge(long SourceClientId, long DestinationClientId, User currentUser);
        long Clients_Split(long visitGroupId);
        ClientPretension ClientPretension_Generation(long ClientId);
        void ClientPretension_Save(ClientPretension pretension);
        List<ClientPretension> ClientPretension_Get(long ClientId);
        byte[] PretensionGetLPU(ClientPretension pretension);
        byte[] PretensionGetFile(ClientPretension pretension);
        byte[] PretensionGetLast(ClientPretension pretension, Client client);

    }
}
