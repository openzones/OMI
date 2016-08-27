using OMInsurance.DataAccess.Core;
using OMInsurance.DataAccess.Core.Helpers;
using OMInsurance.DataAccess.Materializers;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.Entities.Searching;
using OMInsurance.Entities.Sorting;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace OMInsurance.DataAccess.DAO
{
    public class ClientDao : ItemDao
    {
        private static ClientDao _instance = new ClientDao();

        private ClientDao()
            : base(DatabaseAliases.OMInsurance, new DatabaseErrorHandler())
        {
        }

        public static ClientDao Instance
        {
            get
            {
                return _instance;
            }
        }

        public DataPage<ClientBaseInfo> Client_Find(
            ClientSearchCriteria criteria,
            List<SortCriteria<ClientSortField>> sortCriteria,
            PageRequest pageRequest)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@Firstname", SqlDbType.NVarChar, criteria.Firstname);
            parameters.AddInputParameter("@Secondname", SqlDbType.NVarChar, criteria.Secondname);
            parameters.AddInputParameter("@Lastname", SqlDbType.NVarChar, criteria.Lastname);

            parameters.AddInputParameter("@Birthday", SqlDbType.Date, criteria.Birthday);
            parameters.AddInputParameter("@TemporaryPolicyDateFrom", SqlDbType.Date, criteria.TemporaryPolicyDateFrom);
            parameters.AddInputParameter("@TemporaryPolicyDateTo", SqlDbType.Date, criteria.TemporaryPolicyDateTo);
            parameters.AddInputParameter("@TemporaryPolicyNumber", SqlDbType.NVarChar, criteria.TemporaryPolicyNumber);
            parameters.AddInputParameter("@PolicySeries", SqlDbType.NVarChar, criteria.PolicySeries);
            parameters.AddInputParameter("@PolicyNumber", SqlDbType.NVarChar, criteria.PolicyNumber);
            parameters.AddInputParameter("@PolicyDateFrom", SqlDbType.Date, criteria.PolicyDateFrom);
            parameters.AddInputParameter("@PolicyDateTo", SqlDbType.Date, criteria.PolicyDateTo);
            parameters.AddInputParameter("@UnifiedPolicyNumber", SqlDbType.NVarChar, criteria.UnifiedPolicyNumber);

            SqlParameter totalCountParameter = parameters.AddOutputParameter("@total_count", SqlDbType.Int);
            parameters.AddInputParameter("@sort_criteria", SqlDbType.Structured, DaoHelper.GetSortFieldsTable(sortCriteria));

            parameters.AddInputParameter("@Page_size", SqlDbType.Int, pageRequest.PageSize);
            parameters.AddInputParameter("@Page_number", SqlDbType.Int, pageRequest.PageNumber);

            List<ClientBaseInfo> clients = Execute_GetList(ClientBaseInfoMaterializer.Instance, "Client_Find", parameters);
            return DaoHelper.GetDataPage(clients, totalCountParameter, pageRequest);
        }

        public ClientVisitSaveResult ClientVisit_Save(ClientVisit.SaveData saveData, long? currentUserId, DateTime? saveDate = null)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            SaveClientVisitTableSet clientVisitTableSet = new SaveClientVisitTableSet(saveData, saveData.ClientId);

            parameters.AddInputParameter("@UserId", SqlDbType.BigInt, currentUserId);
            parameters.AddInputParameter("@RegistratorId", SqlDbType.BigInt, saveData.RegistratorId);
            parameters.AddInputParameter("@DeliveryCenterId", SqlDbType.BigInt, saveData.DeliveryCenterId);
            parameters.AddInputParameter("@TemporaryPolicyDate", SqlDbType.DateTime, saveData.TemporaryPolicyDate);
            parameters.AddInputParameter("@TemporaryPolicyExpirationDate", SqlDbType.DateTime, saveData.TemporaryPolicyExpirationDate);
            parameters.AddInputParameter("@TemporaryPolicyNumber", SqlDbType.NVarChar, saveData.TemporaryPolicyNumber);
            parameters.AddInputParameter("@ScenarioId", SqlDbType.BigInt, saveData.ScenarioId);
            parameters.AddInputParameter("@GOZNAKTypeId", SqlDbType.BigInt, saveData.GOZNAKTypeId);
            parameters.AddInputParameter("@IssueDate", SqlDbType.DateTime, saveData.IssueDate);
            parameters.AddInputParameter("@IsActual", SqlDbType.Bit, saveData.IsActual);
            parameters.AddInputParameter("@InfoSource", SqlDbType.NVarChar, saveData.InfoSource);
            parameters.AddInputParameter("@DeregistrationDate", SqlDbType.DateTime, saveData.DeregistrationDate);
            parameters.AddInputParameter("@ArchivationDate", SqlDbType.DateTime, saveData.ArchivationDate);
            parameters.AddInputParameter("@AttachmentDate", SqlDbType.DateTime, saveData.AttachmentDate);
            parameters.AddInputParameter("@AttachmentTypeId", SqlDbType.BigInt, saveData.AttachmentTypeId);
            parameters.AddInputParameter("@MedicalCentreId", SqlDbType.BigInt, saveData.MedicalCentreId);
            parameters.AddInputParameter("@CarrierId", SqlDbType.BigInt, saveData.CarrierId);
            parameters.AddInputParameter("@ApplicationMethodId", SqlDbType.BigInt, saveData.ApplicationMethodId);
            parameters.AddInputParameter("@Comment", SqlDbType.NVarChar, saveData.Comment);
            parameters.AddInputParameter("@GOZNAKDate", SqlDbType.Date, saveData.GOZNAKDate);
            parameters.AddInputParameter("@ClientCategoryId", SqlDbType.BigInt, saveData.ClientCategoryId);
            parameters.AddInputParameter("@DeliveryPointId", SqlDbType.BigInt, saveData.DeliveryPointId);
            parameters.AddInputParameter("@ClientAcquisitionEmployee", SqlDbType.NVarChar, saveData.ClientAcquisitionEmployee);
            parameters.AddInputParameter("@ClientContacts", SqlDbType.NVarChar, saveData.ClientContacts);
            parameters.AddInputParameter("@Email", SqlDbType.NVarChar, saveData.Email);
            parameters.AddInputParameter("@UralsibCard", SqlDbType.Bit, saveData.UralsibCard);
            parameters.AddInputParameter("@Blanc", SqlDbType.NVarChar, saveData.Blanc);
            parameters.AddInputParameter("@N_KOR", SqlDbType.NVarChar, saveData.N_KOR);
            parameters.AddInputParameter("@DATA_FOND", SqlDbType.Date, saveData.DATA_FOND);
            parameters.AddInputParameter("@NZ_GOZNAK", SqlDbType.NVarChar, saveData.NZ_GOZNAK);
            parameters.AddInputParameter("@Dat_U", SqlDbType.Date, saveData.Dat_U);
            parameters.AddInputParameter("@Dat_S", SqlDbType.Date, saveData.Dat_S);
            parameters.AddInputParameter("@SignatureFileName", SqlDbType.NVarChar, saveData.SignatureFileName);
            parameters.AddInputParameter("@PhotoFileName", SqlDbType.NVarChar, saveData.PhotoFileName);
            parameters.AddInputParameter("@Phone", SqlDbType.NVarChar, saveData.Phone);
            parameters.AddInputParameter("@PartyNumber", SqlDbType.NVarChar, saveData.PolicyPartyNumber);
            parameters.AddInputParameter("@FundResponseApplyingMessage", SqlDbType.NVarChar, saveData.FundResponseApplyingMessage);
            parameters.AddInputParameter("@IsReadyToFundSubmitRequest", SqlDbType.Bit, saveData.IsReadyToFundSubmitRequest);
            parameters.AddInputParameter("@IsDifficultCase", SqlDbType.Bit, saveData.IsDifficultCase);

            parameters.AddInputParameter("@OldClientVersion", SqlDbType.Structured, clientVisitTableSet.OldClientVersionDataTable);
            parameters.AddInputParameter("@NewClientVersion", SqlDbType.Structured, clientVisitTableSet.NewClientVersionDataTable);
            parameters.AddInputParameter("@OldDocument", SqlDbType.Structured, clientVisitTableSet.OldDocumentDataTable);
            parameters.AddInputParameter("@NewDocument", SqlDbType.Structured, clientVisitTableSet.NewDocumentDataTable);
            parameters.AddInputParameter("@OldForeignDocument", SqlDbType.Structured, clientVisitTableSet.OldForeignDocument);
            parameters.AddInputParameter("@NewForeignDocument", SqlDbType.Structured, clientVisitTableSet.NewForeignDocument);
            parameters.AddInputParameter("@LivingAddress", SqlDbType.Structured, clientVisitTableSet.LivingAddressDataTable);
            parameters.AddInputParameter("@RegistrationAddress", SqlDbType.Structured, clientVisitTableSet.RegisterAddressDataTable);
            parameters.AddInputParameter("@RegistrationAddressDate", SqlDbType.DateTime, saveData.RegistrationAddressDate);
            parameters.AddInputParameter("@OldPolicyInfo", SqlDbType.Structured, clientVisitTableSet.OldPolicyInfoDataTable);
            parameters.AddInputParameter("@NewPolicyInfo", SqlDbType.Structured, clientVisitTableSet.NewPolicyInfoDataTable);
            parameters.AddInputParameter("@Representative", SqlDbType.Structured, clientVisitTableSet.RepresentativeDataTable);
            parameters.AddInputParameter("@StatusID", SqlDbType.BigInt, saveData.Status);
            parameters.AddInputParameter("@StatusDate", SqlDbType.DateTime, saveData.StatusDate);
            parameters.AddInputParameter("@OldSystemID", SqlDbType.BigInt, saveData.OldSystemId);
            parameters.AddInputParameter("@SaveDate", SqlDbType.DateTime, saveDate ?? DateTime.Now);

            SqlParameter clientVisitID = parameters.AddInputOutputParameter("@ClientVisitID", SqlDbType.BigInt, saveData.Id);
            SqlParameter clientVersionID = parameters.AddOutputParameter("@NewClientVersionID", SqlDbType.BigInt);
            SqlParameter clientID = parameters.AddInputOutputParameter("@ClientID", SqlDbType.BigInt, saveData.ClientId);
            SqlParameter VisitGroupId = parameters.AddInputOutputParameter("@VisitGroupId", SqlDbType.BigInt, saveData.VisitGroupId);


            Execute_StoredProcedure("ClientVisit_Save", parameters);
            ClientVisitSaveResult result = new ClientVisitSaveResult();
            result.ClientID = (long)clientID.Value;
            result.NewClientVersionID = (long)clientVersionID.Value;
            result.ClientVisitID = (long)clientVisitID.Value;
            result.VisitGroupId = (long)VisitGroupId.Value;
            return result;
        }

        public ClientVisit ClientVisit_Get(long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ClientVisitID", SqlDbType.BigInt, id);
            ClientVisit client = Execute_Get(ClientVisitMaterializer.Instance, "ClientVisit_Get", parameters);
            return client;
        }

        public ClientVisit ClientVisit_GetLastClientVisitInGroup(long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ClientVisitGroupID", SqlDbType.BigInt, id);
            ClientVisit client = Execute_Get(ClientVisitMaterializer.Instance, "ClientVisit_GetLastClientVisitInGroup", parameters);
            return client;
        }

        public ClientVisit ClientVisit_GetFirstClientVisitInGroup(long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ClientVisitGroupID", SqlDbType.BigInt, id);
            ClientVisit client = Execute_Get(ClientVisitMaterializer.Instance, "ClientVisit_GetFirstClientVisitInGroup", parameters);
            return client;
        }

        public List<ClientVisitHistoryItem> ClientVisitHistory_Get(long clientVisitGroupId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ClientVisitGroupID", SqlDbType.BigInt, clientVisitGroupId);
            List<ClientVisitHistoryItem> clientVisitHistoryItems = Execute_GetList(ClientVisitHistoryItemMaterializer.Instance, "ClientVisitHistory_Get", parameters);
            return clientVisitHistoryItems;
        }

        public void ClientVisit_SetStatus(long userId, long id, long status, bool isActualClientVisit, DateTime? statusDate = null)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ClientVisitID", SqlDbType.BigInt, id);
            parameters.AddInputParameter("@UserId", SqlDbType.BigInt, userId);
            parameters.AddInputParameter("@Statusid", SqlDbType.BigInt, status);
            parameters.AddInputParameter("@IsActualClientVisit", SqlDbType.Bit, status);
            parameters.AddInputParameter("@StatusDate", SqlDbType.DateTime, statusDate ?? DateTime.Now);
            Execute_StoredProcedure("ClientVisit_SetStatus", parameters);
        }

        public Client Client_Get(long id, bool onlyActualClientVisits)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ClientId", SqlDbType.BigInt, id);
            parameters.AddInputParameter("@OnlyActualClientVisits", SqlDbType.Bit, onlyActualClientVisits);
            Client client = Execute_Get(ClientMaterializer.Instance, "Client_Get", parameters);
            return client;
        }

        public ClientVersion ClientVersion_Get(long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ClientVersionId", SqlDbType.BigInt, id);
            ClientVersion client = Execute_Get(ClientVersionMaterializer.Instance, "ClientVersion_Get", parameters);
            return client;
        }

        public DataPage<ClientVisitInfo> ClientVisit_Find(
            ClientVisitSearchCriteria criteria,
            List<SortCriteria<ClientVisitSortField>> sortCriteria,
            PageRequest pageRequest)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.AddInputParameter("@ID", SqlDbType.BigInt, criteria.Id);
            parameters.AddInputParameter("@UnifiedPolicyNumber", SqlDbType.NVarChar, criteria.UnifiedPolicyNumber);
            parameters.AddInputParameter("@SNILS", SqlDbType.NVarChar, criteria.SNILS);
            parameters.AddInputParameter("@DocumentTypeId", SqlDbType.BigInt, criteria.DocumentTypeId);
            parameters.AddInputParameter("@DocumentNumber", SqlDbType.NVarChar, criteria.DocumentNumber);
            parameters.AddInputParameter("@DocumentSeries", SqlDbType.NVarChar, criteria.DocumentSeries);

            parameters.AddInputParameter("@TemporaryPolicyNumber", SqlDbType.NVarChar, criteria.TemporaryPolicyNumber);
            parameters.AddInputParameter("@TemporaryPolicyDateFrom", SqlDbType.DateTime, criteria.TemporaryPolicyDateFrom);
            parameters.AddInputParameter("@TemporaryPolicyDateTo", SqlDbType.DateTime, criteria.TemporaryPolicyDateTo);
            parameters.AddInputParameter("@PolicyNumber", SqlDbType.NVarChar, criteria.PolicyNumber);
            parameters.AddInputParameter("@PolicySeries", SqlDbType.NVarChar, criteria.PolicySeries);
            parameters.AddInputParameter("@PolicyDateFrom", SqlDbType.DateTime, criteria.PolicyDateFrom);
            parameters.AddInputParameter("@PolicyDateTo", SqlDbType.DateTime, criteria.PolicyDateTo);
            parameters.AddInputParameter("@UpdateDateFrom", SqlDbType.DateTime, criteria.UpdateDateFrom);
            parameters.AddInputParameter("@UpdateDateTo", SqlDbType.DateTime, criteria.UpdateDateTo);
            parameters.AddInputParameter("@StatusDateFrom", SqlDbType.DateTime, criteria.StatusDateFrom);
            parameters.AddInputParameter("@StatusDateTo", SqlDbType.DateTime, criteria.StetusDateTo);

            parameters.AddInputParameter("@Firstname", SqlDbType.NVarChar, criteria.Firstname);
            parameters.AddInputParameter("@Secondname", SqlDbType.NVarChar, criteria.Secondname);
            parameters.AddInputParameter("@Lastname", SqlDbType.NVarChar, criteria.Lastname);

            parameters.AddInputParameter("@UserId", SqlDbType.BigInt, criteria.UserId);

            parameters.AddInputParameter("@Birthday", SqlDbType.Date, criteria.Birthday);
            parameters.AddInputParameter("@PartyNumber", SqlDbType.NVarChar, criteria.PartyNumber);
            parameters.AddInputParameter("@DeliveryCenterIds", SqlDbType.Structured, DaoHelper.GetObjectIds(criteria.DeliveryCenterIds));
            parameters.AddInputParameter("@DeliveryPointIds", SqlDbType.Structured, DaoHelper.GetObjectIds(criteria.DeliveryPointIds));
            parameters.AddInputParameter("@StatusIds", SqlDbType.Structured, DaoHelper.GetObjectIds(criteria.StatusIds));
            parameters.AddInputParameter("@ScenarioIds", SqlDbType.Structured, DaoHelper.GetObjectIds(criteria.ScenarioIds));
            parameters.AddInputParameter("@IsTemporaryPolicyNumberNotEmpty", SqlDbType.Bit, criteria.IsTemporaryPolicyNumberNotEmpty);
            parameters.AddInputParameter("@IsActualInVisitGroup", SqlDbType.Bit, criteria.IsActualInVisitGroup);
            parameters.AddInputParameter("@IsReadyToFundSubmitRequest", SqlDbType.Bit, criteria.IsReadyToFundSubmitRequest);
            parameters.AddInputParameter("@IsDifficultCase", SqlDbType.Bit, criteria.IsDifficultCase);

            SqlParameter totalCountParameter = parameters.AddOutputParameter("@total_count", SqlDbType.Int);
            parameters.AddInputParameter("@sort_criteria", SqlDbType.Structured, DaoHelper.GetSortFieldsTable(sortCriteria));

            parameters.AddInputParameter("@Page_size", SqlDbType.Int, pageRequest.PageSize);
            parameters.AddInputParameter("@Page_number", SqlDbType.Int, pageRequest.PageNumber);

            List<ClientVisitInfo> clients = Execute_GetList(ClientVisitInfoMaterializer.Instance, "ClientVisit_Find", parameters);
            return DaoHelper.GetDataPage(clients, totalCountParameter, pageRequest);
        }

        public long Clients_Merge(long sourceClientId, long destinationClientId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.AddInputParameter("@SourceClientId", SqlDbType.BigInt, sourceClientId);
            parameters.AddInputParameter("@DestinationClientId", SqlDbType.BigInt, destinationClientId);
            SqlParameter resultClientId = parameters.AddOutputParameter("@ResultClientId", SqlDbType.BigInt);

            Execute_StoredProcedure("Clients_Merge", parameters);
            return (long)resultClientId.Value;
        }

        public long Clients_Split(long visitGroupId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.AddInputParameter("@VisitGroupId", SqlDbType.BigInt, visitGroupId);
            SqlParameter resultClientId = parameters.AddOutputParameter("@ResultClientId", SqlDbType.BigInt);

            Execute_StoredProcedure("Clients_Split", parameters);
            return (long)resultClientId.Value;
        }

        public ClientPretension ClientPretension_Generation(long ClientId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ClientID", SqlDbType.BigInt, ClientId);
            List<ClientPretension> result = Execute_GetList(ClientPretensionGenerationMaterializer.Instance, "ClientPretension_Generation", parameters);
            return result.OrderByDescending(a => a.Id).FirstOrDefault();
        }

        public void ClientPretension_Save(ClientPretension pretension)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@Generator", SqlDbType.BigInt, pretension.Generator);
            parameters.AddInputParameter("@ClientID", SqlDbType.BigInt, pretension.ClientID);
            parameters.AddInputParameter("@LPU_ID", SqlDbType.BigInt, pretension.LPU_ID);
            parameters.AddInputParameter("@DATE_IN", SqlDbType.Date, pretension.DATE_IN);
            parameters.AddInputParameter("@M_nakt", SqlDbType.NVarChar, pretension.M_nakt);
            parameters.AddInputParameter("@M_dakt", SqlDbType.Date, pretension.M_dakt);
            parameters.AddInputParameter("@M_expert_Id", SqlDbType.BigInt, pretension.M_expert_Id);
            parameters.AddInputParameter("@MedicalCenterId", SqlDbType.BigInt, pretension.MedicalCenterId);
            parameters.AddInputParameter("@M_period", SqlDbType.NVarChar, pretension.M_period);
            parameters.AddInputParameter("@M_snpol", SqlDbType.NVarChar, pretension.M_snpol);
            parameters.AddInputParameter("@M_fd", SqlDbType.NVarChar, pretension.M_fd);
            parameters.AddInputParameter("@M_nd1", SqlDbType.NVarChar, pretension.M_nd1);
            parameters.AddInputParameter("@M_nd2", SqlDbType.NVarChar, pretension.M_nd2);
            parameters.AddInputParameter("@IsConfirm", SqlDbType.Bit, pretension.IsConfirm);
            parameters.AddInputParameter("@M_osn230_Id", SqlDbType.BigInt, pretension.M_osn230_Id);
            parameters.AddInputParameter("@M_straf", SqlDbType.Real, pretension.M_straf);
            parameters.AddInputParameter("@PeriodFrom", SqlDbType.DateTime, pretension.PeriodFrom);
            parameters.AddInputParameter("@PeriodTo", SqlDbType.DateTime, pretension.PeriodTo);
            parameters.AddInputParameter("@Coefficient", SqlDbType.Real, pretension.Coefficient);
            parameters.AddInputParameter("@UserId", SqlDbType.BigInt, pretension.UserId);
            parameters.AddInputParameter("@CreateDate", SqlDbType.DateTime, pretension.CreateDate);
            parameters.AddInputParameter("@UpdateUserId", SqlDbType.BigInt, pretension.UpdateUserId);
            parameters.AddInputParameter("@UpdateDate", SqlDbType.DateTime, pretension.UpdateDate);

            parameters.AddInputParameter("@FileNameLPU", SqlDbType.NVarChar, pretension.FileNameLPU);
            parameters.AddInputParameter("@FileUrlLPU", SqlDbType.NVarChar, pretension.FileUrlLPU);
            parameters.AddInputParameter("@FileName2", SqlDbType.NVarChar, pretension.FileName2);
            parameters.AddInputParameter("@FileUrl2", SqlDbType.NVarChar, pretension.FileUrl2);

            Execute_StoredProcedure("ClientPretension_Save", parameters);
        }

        public List<ClientPretension> ClientPretension_Get(long ClientId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ClientId", SqlDbType.BigInt, ClientId);
            List<ClientPretension> result = Execute_GetList(ClientPretensionMaterializer.Instance, "ClientPretension_Get", parameters);
            return result;
        }
    }
}
