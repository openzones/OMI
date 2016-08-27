using OMInsurance.BusinessLogic;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System;
using System.Linq;
using System.Collections.Generic;

namespace OMInsurance.Tests.Generation
{
    public static class ClientGenerator
    {
        static ReferenceBusinessLogic referenceBll = new ReferenceBusinessLogic();
        public static ClientVisit.SaveData GetClientVisitSaveDataForNewUser()
        {
            ClientVisit.SaveData clientVisit = new ClientVisit.SaveData();
            clientVisit.TemporaryPolicyDate = DateTime.Now.AddYears(-1);
            clientVisit.TemporaryPolicyNumber = null;
            clientVisit.ScenarioId = referenceBll.GetReferencesList(Constants.ScenarioRef).FirstOrDefault().Id;
            clientVisit.DeliveryCenterId = referenceBll.GetReferencesList(Constants.DeliveryCenterRef).FirstOrDefault().Id;
            clientVisit.LivingAddress = AddressGenerator.GetAddressSaveData(null);
            clientVisit.RegistrationAddress = AddressGenerator.GetAddressSaveData(null);
            clientVisit.RegistrationAddressDate = DateTime.Now.AddYears(-2);
            clientVisit.NewClientInfo = GetClientVersionSaveData("Иван", "Иванович", "Иванов", "1");
            clientVisit.OldClientInfo = GetClientVersionSaveData("Петр", "Петрович", "Петров", "1");
            clientVisit.OldDocument = DocumentGenerator.GetDocumentSaveData(null);
            clientVisit.NewDocument = DocumentGenerator.GetDocumentSaveData(null);
            clientVisit.NewPolicy = PolicyGenerator.GetPolicyInfoSaveData(null);
            clientVisit.OldPolicy = PolicyGenerator.GetPolicyInfoSaveData(null);
            clientVisit.NewForeignDocument = DocumentGenerator.GetDocumentSaveData(null);
            clientVisit.OldForeignDocument = DocumentGenerator.GetDocumentSaveData(null);
            clientVisit.RegistratorId = 1;
            clientVisit.Representative = new Representative.SaveData();
            clientVisit.Phone = "(954)223-11-23";
            clientVisit.ArchivationDate = DateTime.Now;
            clientVisit.AttachmentDate = DateTime.Now;
            clientVisit.AttachmentTypeId = null;
            clientVisit.DeregistrationDate = DateTime.Now;
            clientVisit.GOZNAKTypeId = referenceBll.GetReferencesList(Constants.GOZNAKTypeRef).FirstOrDefault().Id;
            clientVisit.DeliveryPointId = referenceBll.GetReferencesList(Constants.DeliveryPointRef).FirstOrDefault().Id;
            clientVisit.CarrierId = referenceBll.GetReferencesList(Constants.CarriersRef).FirstOrDefault().Id;
            clientVisit.ApplicationMethodId = referenceBll.GetReferencesList(Constants.ApplicationMethodRef).FirstOrDefault().Id;
            return clientVisit;
        }

        public static ClientVersion.SaveData GetClientVersionSaveData(
            string firstname, 
            string secondname, 
            string lastname,
            string sex)
        {
            ClientVersion.SaveData data = new ClientVersion.SaveData();
            data.Firstname = firstname;
            data.Secondname = secondname;
            data.Lastname = lastname;
            List<ReferenceItem> refs = referenceBll.GetReferencesList(Constants.CodFioClassifier);
            data.FirstnameTypeId = refs.FirstOrDefault().Id;
            data.SecondnameTypeId = refs.FirstOrDefault().Id;
            data.LastnameTypeId = refs.FirstOrDefault().Id;
            data.Birthday = DateTime.Now.AddYears(-50);
            data.Birthplace = "Birthplace";
            data.SNILS = "123-456-789 64";
            data.Sex = sex[0];
            data.Citizenship = referenceBll.GetReferencesList(Constants.CitizenshipRef).FirstOrDefault().Id;
            data.Category = referenceBll.GetReferencesList(Constants.ClientCategoryRef).FirstOrDefault().Id;
            return data;
        }

        public static Client.CreateData GetClientCreateData(
            string firstname, 
            string secondname, 
            string lastname,
            string sex)
        {
            Client.CreateData client = new Client.CreateData();
            return client;
        }
    }
}
