using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace OMInsurance.WebApps.Models
{
    public class ClientModel 
    {
        public ClientModel()
        {
        }

        public ClientModel(Client client, User currentUser) : base()
        {
            IsMergeOperationsAvailable = currentUser.Roles.Exists(role => role.Id == Role.Administrator.Id || role.Id == Role.OperatorSG.Id);
            IsSplitOperationsAvailable = currentUser.Roles.Exists(role => role.Id == Role.Administrator.Id || role.Id == Role.OperatorSG.Id);
            if (client != null)
            {
                this.Id = client.Id;
                this.Versions = new HashSet<ClientVersion>(client.Versions, new ClientVersionModelComparer())
                    .Where(v => !string.IsNullOrEmpty(v.Firstname))
                    .Select(v => new ClientVersionModel(v)).OrderBy(v => v.ID);

                this.Visits = new Dictionary<VisitGroupModel, List<ClientVisitInfoModel>>();
                Dictionary<VisitGroupModel, List<ClientVisitInfoModel>> VisitsNotSorted = new Dictionary<VisitGroupModel, List<ClientVisitInfoModel>>();
                Dictionary<long, List<ClientVisitInfoModel>> visitsByGroupId = new Dictionary<long, List<ClientVisitInfoModel>>();
                foreach (var visit in client.Visits)
                {
                    if (!visitsByGroupId.ContainsKey(visit.VisitGroupId))
                    {
                        visitsByGroupId.Add(visit.VisitGroupId, new List<ClientVisitInfoModel>());
                    }
                    visitsByGroupId[visit.VisitGroupId].Add(new ClientVisitInfoModel(visit));
                }
                foreach (var groupId in visitsByGroupId.Keys)
                {
                    VisitGroupModel groupModel = new VisitGroupModel();
                    var clientVisitList = visitsByGroupId[groupId].OrderBy(item => item.Id).ToList();
                    ClientVisitInfoModel firstClientVisit = clientVisitList.FirstOrDefault();
                    ClientVisitInfoModel lastClientVisit = clientVisitList.LastOrDefault();
                    groupModel.Id = groupId;
                    groupModel.Status = lastClientVisit.Status.Name;
                    groupModel.StatusDate = lastClientVisit.StatusDate;
                    groupModel.ClientFullname = string.Format("{0} {1} {2}", 
                        lastClientVisit.Lastname ?? string.Empty, 
                        lastClientVisit.Firstname ?? string.Empty, 
                        lastClientVisit.Secondname ?? string.Empty);
                    groupModel.TemporaryPolicyDate = firstClientVisit.TemporaryPolicyDate;
                    VisitsNotSorted.Add(groupModel, clientVisitList);
                }
                //сортируем по ID обращения
                var temp = VisitsNotSorted.OrderBy(a => a.Key.Id);
                foreach (var elem in temp)
                {
                    this.Visits.Add(elem.Key, VisitsNotSorted[elem.Key]);
                }

                this.ActualVersion = new ClientVersionEditModel(client.ActualVersion, EntityType.General);

                this.listSmsModel = new List<ClientSmsModel>();
                foreach(var sms in client.ListSms)
                {
                    this.listSmsModel.Add(new ClientSmsModel(sms));
                }
            }
        }

        [DisplayName("Id")]
        public long Id { get; set; }

        [DisplayName("Версии")]
        public IEnumerable<ClientVersionModel> Versions { get; set; }

        public bool IsMergeOperationsAvailable { get; set; }
        public bool IsSplitOperationsAvailable { get; set; }

        [DisplayName("Обращения")]
        public Dictionary<VisitGroupModel, List<ClientVisitInfoModel>> Visits { get; set; }

        public ClientVersionEditModel ActualVersion { get; set; }

        public List<ClientSmsModel> listSmsModel { get; set; }
    }
}