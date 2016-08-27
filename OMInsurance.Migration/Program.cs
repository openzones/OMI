using OMInsurance.DataAccess.DAO;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.Entities.Searching;
using OMInsurance.Entities.Sorting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;

namespace OMInsurance.Migration
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ClientVisit.SaveData> list = new List<ClientVisit.SaveData>();
            while ((list = ClientDataDao.Instance.GetNextClientData()).Count > 0)
            {
                foreach (var clientVisit in list)
                {
                    Process(clientVisit);
                }
            }
        }

        private static void Process(ClientVisit.SaveData clientVisit)
        {
            bool isSuccess = false;
            try
            {
                ClientVisitSearchCriteria clientVisitSC = new ClientVisitSearchCriteria();
                clientVisitSC.Firstname = clientVisit.NewClientInfo.Firstname;
                clientVisitSC.Secondname = clientVisit.NewClientInfo.Secondname;
                clientVisitSC.Lastname = clientVisit.NewClientInfo.Lastname;
                clientVisitSC.Birthday = clientVisit.NewClientInfo.Birthday;

                DataPage<ClientVisitInfo> foundClients = new DataPage<ClientVisitInfo>();
                if (!string.IsNullOrEmpty(clientVisit.NewClientInfo.SNILS))
                {
                    clientVisitSC.SNILS = clientVisit.NewClientInfo.SNILS;
                    foundClients = ClientDao.Instance.ClientVisit_Find(
                        clientVisitSC,
                        new List<SortCriteria<ClientVisitSortField>>(),
                        new PageRequest() { PageNumber = 1, PageSize = 10 });
                    clientVisitSC.SNILS = null;
                }

                if (clientVisit.NewDocument.DocumentTypeId.HasValue && !string.IsNullOrEmpty(clientVisit.NewDocument.Number))
                {
                    clientVisitSC.DocumentTypeId = clientVisit.NewDocument.DocumentTypeId;
                    clientVisitSC.DocumentSeries = clientVisit.NewDocument.Series;
                    clientVisitSC.DocumentNumber = clientVisit.NewDocument.Number;
                    foundClients = ClientDao.Instance.ClientVisit_Find(clientVisitSC, new List<SortCriteria<ClientVisitSortField>>(), new PageRequest() { PageNumber = 1, PageSize = 10 });
                    clientVisitSC.DocumentTypeId = null;
                    clientVisitSC.DocumentSeries = null;
                    clientVisitSC.DocumentNumber = null;
                }

                if (!string.IsNullOrEmpty(clientVisit.NewPolicy.Series) && !string.IsNullOrEmpty(clientVisit.NewPolicy.Number))
                {
                    clientVisitSC.PolicySeries = clientVisit.NewPolicy.Series;
                    clientVisitSC.PolicyNumber = clientVisit.NewPolicy.Number;
                    foundClients = ClientDao.Instance.ClientVisit_Find(clientVisitSC, new List<SortCriteria<ClientVisitSortField>>(), new PageRequest() { PageNumber = 1, PageSize = 10 });
                    clientVisitSC.PolicySeries = null;
                    clientVisitSC.PolicyNumber = null;
                }

                if (!string.IsNullOrEmpty(clientVisit.NewPolicy.UnifiedPolicyNumber))
                {
                    clientVisitSC.UnifiedPolicyNumber = clientVisit.NewPolicy.UnifiedPolicyNumber;
                    foundClients = ClientDao.Instance.ClientVisit_Find(clientVisitSC, new List<SortCriteria<ClientVisitSortField>>(), new PageRequest() { PageNumber = 1, PageSize = 10 });
                    clientVisitSC.UnifiedPolicyNumber = null;
                }

                if (foundClients.Count != 0)
                {
                    clientVisit.ClientId = foundClients.OrderBy(item => item.StatusDate).LastOrDefault().ClientId;
                }

                if (clientVisit.Status == 0)
                {
                    clientVisit.Status = 12;
                }

                ClientVisitSaveResult result = ClientDao.Instance.ClientVisit_Save(clientVisit, 1, clientVisit.StatusDate ?? new DateTime(1900, 1, 1));
                ClientDao.Instance.ClientVisit_SetStatus(1, result.ClientVisitID, clientVisit.Status.Value, true, clientVisit.StatusDate ?? new DateTime(1900, 1, 1));
                isSuccess = true;
                ClientDataDao.Instance.SetProcessed(clientVisit.UniqueId.Value, isSuccess);
            }
            catch (SqlTypeException ex)
            {
                isSuccess = false;
                ClientDataDao.Instance.SetProcessed(clientVisit.UniqueId.Value, isSuccess);
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("The data for table-valued parameter \"@Representative\""))
                {
                    isSuccess = false;
                    ClientDataDao.Instance.SetProcessed(clientVisit.UniqueId.Value, isSuccess);
                }
                else
                {
                    throw ex;
                }
            }
            catch
            {
                isSuccess = false;
                ClientDataDao.Instance.SetProcessed(clientVisit.UniqueId.Value, isSuccess);
            }
            Console.WriteLine(string.Format("   {0} {1} {2} {3} {4}", clientVisit.OldSystemId.ToString(),
                clientVisit.NewClientInfo.Lastname ?? string.Empty,
                clientVisit.NewClientInfo.Firstname ?? string.Empty,
                clientVisit.NewClientInfo.Secondname ?? string.Empty,
                isSuccess.ToString()));
        }
    }
}
