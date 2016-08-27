using Microsoft.VisualStudio.TestTools.UnitTesting;
using OMInsurance.BusinessLogic;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.Entities.Searching;
using OMInsurance.Entities.Sorting;
using OMInsurance.Tests.Generation;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.Tests
{
    [TestClass]
    public class ClientVisitTests
    {
        [TestMethod]
        public void ClientVisitCreate_NewUser()
        {
            Client.CreateData clientCreateData = ClientGenerator.GetClientCreateData("Иван", "Иванович", "Иванов", "1");
            ClientBusinessLogic bll = new ClientBusinessLogic();
            ClientVisit.SaveData data = ClientGenerator.GetClientVisitSaveDataForNewUser();
            long clientId = bll.ClientVisit_Save(new User() { Roles = new List<Role>() { Role.Administrator } }, data).ClientID;
            Client client = bll.Client_Get(new User() { Roles = new List<Role>() { Role.Administrator } }, clientId);
            long visitId = client.Visits.OrderByDescending(item => item.StatusDate).FirstOrDefault().Id;

            ClientVisit visit = bll.ClientVisit_Get(visitId);
            Assert.IsNotNull(visit);
            Assert.IsNotNull(visit.LivingAddress);
            Assert.IsNotNull(visit.RegistrationAddress);
            Assert.IsNotNull(visit.OldClientInfo);
            Assert.IsNotNull(visit.NewClientInfo);
            Assert.IsNotNull(visit.OldDocument);
            Assert.IsNotNull(visit.NewDocument);
            Assert.IsNotNull(visit.NewPolicy);
            Assert.IsNotNull(visit.OldPolicy);
        }

        [TestMethod]
        public void ClientVisit_Find()
        {
            Client.CreateData clientCreateData = ClientGenerator.GetClientCreateData("Иван", "Иванович", "Иванов", "1");
            ClientBusinessLogic bll = new ClientBusinessLogic();
            ClientVisit.SaveData data = ClientGenerator.GetClientVisitSaveDataForNewUser();
            long clientId = bll.ClientVisit_Save(new User() { Roles = new List<Role>() { Role.Administrator } }, data).ClientID;
            Client client = bll.Client_Get(new User() { Roles = new List<Role>() { Role.Administrator } }, clientId);
            long visitId = client.Visits.OrderByDescending(item => item.StatusDate).FirstOrDefault().Id;
            ClientVisit visit = bll.ClientVisit_Get(visitId);
            ClientVisitSearchCriteria criteria = new ClientVisitSearchCriteria();
            criteria.Firstname = data.NewClientInfo.Firstname;
            criteria.Secondname = data.NewClientInfo.Secondname;
            criteria.Lastname = data.NewClientInfo.Lastname;
            criteria.Birthday = data.NewClientInfo.Birthday;
            criteria.DeliveryCenterIds = new List<long>();
            if (data.DeliveryCenterId.HasValue)
            {
                criteria.DeliveryCenterIds.Add(data.DeliveryCenterId.Value);
            }
            criteria.Id = visitId;
            criteria.PolicyDateTo = visit.IssueDate;
            criteria.PolicyNumber = visit.NewPolicy.Number;
            criteria.PolicySeries = visit.NewPolicy.Series;
            criteria.StatusIds = new List<long>();
            if (data.Status.HasValue)
            {
                criteria.StatusIds.Add(data.Status.Value);
            }
            criteria.TemporaryPolicyDateTo = visit.TemporaryPolicyDate;
            criteria.TemporaryPolicyNumber = visit.TemporaryPolicyNumber;
            DataPage<ClientVisitInfo> list = bll.ClientVisit_Find(
                criteria,
                new List<SortCriteria<ClientVisitSortField>>(),
                new PageRequest() { PageNumber = 1, PageSize = 10 });
            Assert.AreEqual(list.Count, 1);
        }
    }
}
