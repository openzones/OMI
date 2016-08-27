using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.Migration
{
    public class ClientVisitSaveDataMaterializer : IMaterializer<ClientVisit.SaveData>
    {
        private static readonly ClientVisitSaveDataMaterializer _instance = new ClientVisitSaveDataMaterializer();

        public static ClientVisitSaveDataMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }
        public ClientVisit.SaveData Materialize(DataReaderAdapter dataReader)
        {
            return Materialize_List(dataReader).FirstOrDefault();
        }

        public List<ClientVisit.SaveData> Materialize_List(DataReaderAdapter dataReader)
        {
            List<ClientVisit.SaveData> items = new List<ClientVisit.SaveData>();

            while (dataReader.Read())
            {
                ClientVisit.SaveData obj = ReadItemFields(dataReader);
                items.Add(obj);
            }

            return items;
        }

        public ClientVisit.SaveData ReadItemFields(DataReaderAdapter dataReader,
            ClientVisit.SaveData clientVisit = null)
        {
            if (clientVisit == null)
                clientVisit = new ClientVisit.SaveData();

            clientVisit.DeliveryCenterId = dataReader.GetInt64("DeliveryCenterID");
            clientVisit.MedicalCentreId = dataReader.GetInt64Null("MedicalCenterID");
            clientVisit.PolicyPartyNumber = dataReader.GetString("PolicyParty");
            clientVisit.Status = dataReader.GetInt64Null("Status") ?? 12;
            clientVisit.StatusDate = dataReader.GetDateTimeNull("DP");
            clientVisit.TemporaryPolicyNumber = dataReader.GetString("TemporaryPolicyNumber");
            clientVisit.TemporaryPolicyDate = dataReader.GetDateTimeNull("TemporaryPolicyDate");
            
            string scnCode = dataReader.GetString("SCN");
            string JT = dataReader.GetString("JT");
            if (JT == "r" || scnCode == "POK")
            {
                clientVisit.Status = ClientVisitStatuses.PolicyIssued.Id;
                clientVisit.IssueDate = clientVisit.StatusDate;
            }

            FillPolicyData(dataReader, clientVisit);

            clientVisit.NewClientInfo.Lastname = dataReader.GetString("NewClientLastname");
            long? NewClientLastnameTypeId = dataReader.GetInt64Null("NewClientLastnameTypeId");
            if (!NewClientLastnameTypeId.HasValue)
            {
                NewClientLastnameTypeId = 1;
            }
            clientVisit.NewClientInfo.LastnameTypeId = NewClientLastnameTypeId.Value;
            clientVisit.NewClientInfo.Firstname = dataReader.GetString("NewClientFirstname");
            long? NewClientFirstnameTypeId = dataReader.GetInt64Null("NewClientFirstnameTypeId");
            if (!NewClientFirstnameTypeId.HasValue)
            {
                NewClientFirstnameTypeId = 1;
            }
            clientVisit.NewClientInfo.FirstnameTypeId = NewClientFirstnameTypeId.Value;
            clientVisit.NewClientInfo.Secondname = dataReader.GetString("NewClientSecondname");
            long? NewClientSecondnameTypeId = dataReader.GetInt64Null("NewClientSecondnameTypeId");
            if (!NewClientSecondnameTypeId.HasValue)
            {
                NewClientSecondnameTypeId = 1;
            }
            clientVisit.NewClientInfo.SecondnameTypeId = NewClientSecondnameTypeId.Value;
            clientVisit.NewClientInfo.Birthday = dataReader.GetDateTimeNull("NewBirthDate");
            clientVisit.NewClientInfo.Sex = dataReader.GetInt64("Sex").ToString()[0];
            clientVisit.NewClientInfo.Category = dataReader.GetInt64Null("ClientCategoryId") ?? 1;

            string unformattedPhone = dataReader.GetString("Phone") ?? string.Empty;
            string phone = string.Concat(unformattedPhone.Where(c => char.IsDigit(c)));
            clientVisit.Phone = dataReader.GetString("Phone");
            if (phone.Length == 10)
            {
                clientVisit.Phone = string.Format("({0}){1}-{2}-{3}",
                       phone.Substring(0, 3),
                       phone.Substring(3, 3),
                       phone.Substring(6, 2),
                       phone.Substring(8, 2));
            }
            else if (phone.Length == 11 && (phone[0] == '8' || phone[0] == '7'))
            {
                clientVisit.Phone = string.Format("({0}){1}-{2}-{3}",
                    phone.Substring(1, 3),
                    phone.Substring(4, 3),
                    phone.Substring(7, 2),
                    phone.Substring(9, 2));
            }
            else if (phone.Length == 12 && phone[0] == '+' && phone[1] == '7')
            {
                clientVisit.Phone = string.Format("({0}){1}-{2}-{3}",
                    phone.Substring(2, 3),
                    phone.Substring(5, 3),
                    phone.Substring(8, 2),
                    phone.Substring(10, 2));
            }
            else if (phone.Length == 7)
            {
                clientVisit.Phone = string.Format("(000){0}-{1}-{2}", phone.Substring(0, 3), phone.Substring(3, 2), phone.Substring(5, 2));
            }
            if (!string.IsNullOrEmpty(unformattedPhone))
            {
                clientVisit.Comment = clientVisit.Comment + string.Format(" Телефон из старой системы: {0}", unformattedPhone);
            }
            clientVisit.NewDocument.DocumentTypeId = dataReader.GetInt64Null("NewDocumentTypeID");
            clientVisit.NewDocument.Series = dataReader.GetString("NewDocumentSeries");
            clientVisit.NewDocument.Number = dataReader.GetString("NewDocumentNumber");
            clientVisit.NewDocument.IssueDate = dataReader.GetDateTimeNull("NewDocumentIssueDate");
            clientVisit.NewDocument.IssueDepartment = dataReader.GetString("DocumentIssueDepartment");
            clientVisit.NewClientInfo.SNILS = dataReader.GetString("NewSNILS");
            clientVisit.NewClientInfo.Citizenship = dataReader.GetInt64("Citizenship");
            clientVisit.RegistrationAddressDate = dataReader.GetDateTimeNull("AddressRegistrationDate");
            clientVisit.CarrierId = dataReader.GetInt64Null("CarrierID");
            clientVisit.Representative.RepresentativeTypeId = 1;
            if (dataReader.GetInt32Null("RepresentativeTypeCode").HasValue)
            {
                clientVisit.Representative.RepresentativeTypeId = dataReader.GetInt32Null("RepresentativeTypeCode") + 1;
            }
            clientVisit.ApplicationMethodId = dataReader.GetInt64Null("ApplicationMethodID");
            clientVisit.GOZNAKTypeId = dataReader.GetInt64Null("GoznakTypeID");
            clientVisit.GOZNAKDate = dataReader.GetDateTimeNull("GOZNAKDate");
            clientVisit.RegistratorId = 1;
            clientVisit.NewDocument.ExpirationDate = dataReader.GetDateTimeNull("NewDocumentExpirationDate");
            clientVisit.ClientCategoryId = dataReader.GetInt64Null("UralsibClientCategory");
            clientVisit.NewDocument.IsIssueCase = false;
            clientVisit.OldSystemId = dataReader.GetInt64Null("OldSystemId");
            clientVisit.UniqueId = dataReader.GetInt64Null("UniqueId");
            clientVisit.LivingAddress.TerritoryCode = "45";
            clientVisit.LivingAddress.RegionCode = "000";
            clientVisit.LivingAddress.Region = "г Москва";
            clientVisit.LivingAddress.RegionId = "0c5b2444-70a0-4932-980c-b4dc0d3f02b5";
            long? livingStreetCode = dataReader.GetInt64Null("LivingStreetCode");
            clientVisit.LivingAddress.StreetCode = livingStreetCode.HasValue ? livingStreetCode.ToString() : null;
            clientVisit.LivingAddress.Street = dataReader.GetString("LivingStreetname");
            clientVisit.LivingAddress.House = dataReader.GetString("LivingHouse");
            clientVisit.LivingAddress.Housing = dataReader.GetString("LivingHousing");
            clientVisit.LivingAddress.Building = dataReader.GetString("LivingBuilding");
            clientVisit.LivingAddress.Appartment = dataReader.GetString("LivingAppartment");
            clientVisit.LivingAddress.TerritoryCode = "45";
            clientVisit.LivingAddress.RegionCode = "000";
            clientVisit.RegistrationAddress.Region = dataReader.GetString("RegistrationRegionName");
            clientVisit.RegistrationAddress.Area = dataReader.GetString("RegistrationAreaName");
            clientVisit.RegistrationAddress.City = dataReader.GetString("RegistrationCityName");
            clientVisit.RegistrationAddress.Street = dataReader.GetString("RegistrationStreetName");
            clientVisit.RegistrationAddress.House = dataReader.GetString("RegistrationHouse");
            clientVisit.RegistrationAddress.Housing = dataReader.GetString("RegistrationHousing");
            clientVisit.RegistrationAddress.Building = dataReader.GetString("RegistrationBuilding");
            clientVisit.RegistrationAddress.Appartment = dataReader.GetString("RegistrationAppartment");
            string C_OKATO = dataReader.GetString("C_OKATO");
            if (!string.IsNullOrEmpty(C_OKATO) && C_OKATO.Length == 5)
            {
                clientVisit.RegistrationAddress.TerritoryCode = C_OKATO.Substring(0, 2);
                clientVisit.RegistrationAddress.RegionCode = C_OKATO.Substring(2, 3);
            }

            clientVisit.NewForeignDocument.DocumentTypeId = dataReader.GetInt64Null("ForeignDocumentType");
            clientVisit.NewForeignDocument.Series = dataReader.GetString("ForeignDocumentSeries");
            clientVisit.NewForeignDocument.Number = dataReader.GetString("ForeignDocumentNumber");
            clientVisit.NewForeignDocument.IssueDate = dataReader.GetDateTimeNull("ForeignDocumentIssueDate");
            clientVisit.OldClientInfo.Lastname = dataReader.GetString("OldLastname");
            clientVisit.OldClientInfo.Firstname = dataReader.GetString("OldFirstname");
            clientVisit.OldClientInfo.Secondname = dataReader.GetString("OldSecondname");
            clientVisit.OldClientInfo.Birthday = dataReader.GetDateTimeNull("OldBirthdate");
            clientVisit.OldClientInfo.Sex = dataReader.GetInt64Null("OldSex").HasValue ? dataReader.GetInt64Null("OldSex").ToString()[0] : new char?();
            clientVisit.OldClientInfo.LastnameTypeId = 1;
            clientVisit.OldClientInfo.FirstnameTypeId = 1;
            clientVisit.OldClientInfo.SecondnameTypeId = 1;
            clientVisit.OldDocument.DocumentTypeId = dataReader.GetInt64Null("OldDocumentId");
            clientVisit.OldDocument.Series = dataReader.GetString("OldDocumentSeries");
            clientVisit.OldDocument.Number = dataReader.GetString("OldDocumentNumber");
            clientVisit.OldDocument.IssueDate = dataReader.GetDateTimeNull("OldDocumentDate");
            clientVisit.ScenarioId = dataReader.GetInt64Null("ScenarioID");
            clientVisit.Representative.Lastname = dataReader.GetString("RepresentativeLastname");
            clientVisit.Representative.Firstname = dataReader.GetString("RepresentativeFirstname");
            clientVisit.Representative.Secondname = dataReader.GetString("RepresentativeSecondname");
            clientVisit.Representative.Series = dataReader.GetString("RepresentativeSeries");
            clientVisit.Representative.Number = dataReader.GetString("RepresentativeNumber");
            clientVisit.Representative.IssueDate = dataReader.GetDateTimeNull("RepresentativeIssueDate");
            clientVisit.Representative.Birthday = dataReader.GetDateTimeNull("RepresentativeBirthDate");
            clientVisit.NewClientInfo.Birthplace = dataReader.GetString("NewBirthplace");
            clientVisit.Comment = clientVisit.Comment + " " + dataReader.GetString("comment");

            return clientVisit;
        }

        private static void FillPolicyData(DataReaderAdapter dataReader, ClientVisit.SaveData clientVisit)
        {
            long kmsType = 1;
            long enpType = 3;
            string enp = dataReader.GetString("ENP");
            string sCard = dataReader.GetString("S_CARD");
            string nCard = dataReader.GetString("N_CARD");
            string uralsibOKATO = "45000";
            string uralsibOGRN = "1025002690877";
            string oldOKATO = dataReader.GetString("OKATO_OLD");
            DateTime? oldDP = dataReader.GetDateTimeNull("DP_OLD");
            string ogrnOld2 = dataReader.GetString("OGRN_OLD2");
            string NZ = clientVisit.PolicyPartyNumber;
            string ogrnOld = dataReader.GetString("OGRN_OLD");

            if (clientVisit.StatusDate < new DateTime(2011, 05, 1) ||
                clientVisit.StatusDate > new DateTime(2011, 04, 30)
                && !string.IsNullOrEmpty(NZ) && NZ.All(c => char.IsDigit(c)))
            {
                clientVisit.NewPolicy.UnifiedPolicyNumber = enp;
                clientVisit.NewPolicy.Series = sCard;
                clientVisit.NewPolicy.Number = nCard;
                clientVisit.NewPolicy.PolicyTypeId = kmsType;
                clientVisit.NewPolicy.StartDate = null;
            }
            else if (clientVisit.StatusDate > new DateTime(2011, 04, 30) &&
                clientVisit.StatusDate < new DateTime(2015, 01, 01) &&
                clientVisit.StatusDate < new DateTime(2015, 1, 1) &&
                clientVisit.StatusDate != new DateTime(2013, 1, 1) &&
                clientVisit.StatusDate != new DateTime(2013, 1, 3) &&
                clientVisit.StatusDate != new DateTime(2013, 1, 4) &&
                clientVisit.StatusDate != new DateTime(2013, 1, 5))
            {
                if (!string.IsNullOrWhiteSpace(NZ)
                    && NZ.Any(c => char.IsLetter(c)))
                {
                    clientVisit.NewPolicy.UnifiedPolicyNumber = enp;
                    clientVisit.NewPolicy.Series = sCard;
                    clientVisit.NewPolicy.Number = nCard;
                    clientVisit.NewPolicy.PolicyTypeId = enpType;
                    clientVisit.NewPolicy.OKATO = uralsibOKATO;
                    clientVisit.NewPolicy.OGRN = uralsibOGRN;
                    clientVisit.NewPolicy.StartDate = clientVisit.TemporaryPolicyDate;
                    clientVisit.NewPolicy.EndDate = dataReader.GetDateTimeNull("DT");
                    if (!string.IsNullOrWhiteSpace(ogrnOld))
                    {
                        clientVisit.OldPolicy.UnifiedPolicyNumber = enp;
                        clientVisit.OldPolicy.OKATO = oldOKATO;
                        if (clientVisit.OldPolicy.OKATO == uralsibOKATO)
                        {
                            clientVisit.OldPolicy.Series = sCard;
                            clientVisit.OldPolicy.Number = nCard;
                        }
                        clientVisit.OldPolicy.OGRN = ogrnOld;
                        clientVisit.OldPolicy.StartDate = oldDP;
                        if (clientVisit.OldPolicy.StartDate.HasValue)
                        {
                            if (clientVisit.OldPolicy.StartDate.Value < new DateTime(2011, 5, 1))
                                clientVisit.OldPolicy.PolicyTypeId = kmsType;
                            else
                                clientVisit.OldPolicy.PolicyTypeId = enpType;
                        }
                    }
                }
                else if (string.IsNullOrWhiteSpace(NZ)
                        && string.IsNullOrWhiteSpace(clientVisit.TemporaryPolicyNumber)
                        && clientVisit.Status != ClientVisitStatuses.ErrorEntry.Id &&
                            clientVisit.Status != ClientVisitStatuses.PolicyMadeByAnotherCompany.Id)
                {
                    clientVisit.NewPolicy.UnifiedPolicyNumber = enp;
                    clientVisit.NewPolicy.Series = sCard;
                    clientVisit.NewPolicy.Number = nCard;
                    clientVisit.NewPolicy.PolicyTypeId = enpType;
                    clientVisit.NewPolicy.OKATO = uralsibOKATO;
                    clientVisit.NewPolicy.OGRN = uralsibOGRN;
                    clientVisit.NewPolicy.StartDate = clientVisit.TemporaryPolicyDate;
                    clientVisit.NewPolicy.EndDate = dataReader.GetDateTimeNull("DT");
                    if (!string.IsNullOrWhiteSpace(ogrnOld))
                    {
                        clientVisit.OldPolicy.UnifiedPolicyNumber = enp;
                        clientVisit.OldPolicy.OKATO = oldOKATO;
                        if (clientVisit.OldPolicy.OKATO == uralsibOKATO)
                        {
                            clientVisit.OldPolicy.Series = sCard;
                            clientVisit.OldPolicy.Number = nCard;
                        }
                        clientVisit.OldPolicy.OGRN = ogrnOld;
                        clientVisit.OldPolicy.StartDate = oldDP;
                        if (!clientVisit.OldPolicy.StartDate.HasValue ||
                            clientVisit.OldPolicy.StartDate.Value > new DateTime(2011, 5, 1))
                        {
                            clientVisit.OldPolicy.PolicyTypeId = enpType;
                        }
                        else
                        {
                            clientVisit.OldPolicy.PolicyTypeId = kmsType;
                        }
                    }
                }
                else if (string.IsNullOrWhiteSpace(clientVisit.TemporaryPolicyNumber) &&
                  string.IsNullOrWhiteSpace(NZ) &&
                  (clientVisit.Status == ClientVisitStatuses.ErrorEntry.Id ||
                   clientVisit.Status == ClientVisitStatuses.PolicyMadeByAnotherCompany.Id)
                   || !string.IsNullOrWhiteSpace(clientVisit.TemporaryPolicyNumber) &&
                      string.IsNullOrWhiteSpace(NZ))
                {
                    clientVisit.NewPolicy.UnifiedPolicyNumber = enp;
                    clientVisit.NewPolicy.Series = sCard;
                    clientVisit.NewPolicy.Number = nCard;
                    clientVisit.NewPolicy.PolicyTypeId = enpType;
                    clientVisit.NewPolicy.OKATO = "";
                    clientVisit.NewPolicy.OGRN = "";
                    if (!string.IsNullOrWhiteSpace(ogrnOld))
                    {
                        clientVisit.OldPolicy.UnifiedPolicyNumber = enp;
                        clientVisit.OldPolicy.OKATO = oldOKATO;
                        clientVisit.OldPolicy.OGRN = ogrnOld;
                        clientVisit.OldPolicy.StartDate = oldDP;
                        if (!clientVisit.OldPolicy.StartDate.HasValue ||
                            clientVisit.OldPolicy.StartDate.Value > new DateTime(2011, 5, 1))
                        {
                            clientVisit.OldPolicy.PolicyTypeId = enpType;
                        }
                        else
                        {
                            clientVisit.OldPolicy.PolicyTypeId = kmsType;
                        }
                    }
                }
                else
                {
                    clientVisit.Comment = "BAD POLICY DATA";
                }
            }
            else if (clientVisit.StatusDate > new DateTime(2014, 12, 31))
            {
                if (!string.IsNullOrWhiteSpace(ogrnOld2))
                {
                    if (clientVisit.Status == ClientVisitStatuses.PolicyIssued.Id
                        || clientVisit.Status == ClientVisitStatuses.ReregistrationDone.Id
                        || clientVisit.Status == ClientVisitStatuses.SentToGoznak.Id)
                    {
                        clientVisit.NewPolicy.UnifiedPolicyNumber = enp;
                        clientVisit.NewPolicy.Series = sCard;
                        clientVisit.NewPolicy.Number = nCard;
                        clientVisit.NewPolicy.PolicyTypeId = enpType;
                        clientVisit.NewPolicy.StartDate = clientVisit.TemporaryPolicyDate;
                        clientVisit.NewPolicy.EndDate = dataReader.GetDateTimeNull("DT");
                        clientVisit.NewPolicy.OKATO = uralsibOKATO;
                        clientVisit.NewPolicy.OGRN = uralsibOGRN;
                    }
                    else
                    {
                        clientVisit.NewPolicy.UnifiedPolicyNumber = enp;
                        clientVisit.NewPolicy.Series = sCard;
                        clientVisit.NewPolicy.Number = nCard;
                        clientVisit.NewPolicy.PolicyTypeId = enpType;
                        clientVisit.NewPolicy.StartDate = null;
                        clientVisit.NewPolicy.EndDate = null;
                        clientVisit.NewPolicy.OKATO = "";
                        clientVisit.NewPolicy.OGRN = "";
                    }

                    clientVisit.OldPolicy.UnifiedPolicyNumber = dataReader.GetString("ENP2");
                    clientVisit.OldPolicy.Series = dataReader.GetString("S_CARD2");
                    clientVisit.OldPolicy.Number = dataReader.GetString("N_CARD2");
                    clientVisit.OldPolicy.StartDate = dataReader.GetDateTimeNull("DP_OLD2");
                    clientVisit.OldPolicy.OKATO = dataReader.GetString("OKATO_old2");
                    clientVisit.OldPolicy.OGRN = ogrnOld2;
                    if (!clientVisit.OldPolicy.StartDate.HasValue ||
                            clientVisit.OldPolicy.StartDate.Value > new DateTime(2011, 5, 1))
                    {
                        clientVisit.OldPolicy.PolicyTypeId = enpType;
                    }
                    else
                    {
                        clientVisit.OldPolicy.PolicyTypeId = kmsType;
                    }

                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(NZ))
                    {
                        if (NZ[0] <= 'B' && int.Parse(NZ.Substring(1)) < 76)
                        {
                            clientVisit.NewPolicy.UnifiedPolicyNumber = enp;
                            clientVisit.NewPolicy.Series = sCard;
                            clientVisit.NewPolicy.Number = nCard;
                            clientVisit.NewPolicy.PolicyTypeId = enpType;
                            clientVisit.NewPolicy.StartDate = clientVisit.TemporaryPolicyDate;
                            clientVisit.NewPolicy.EndDate = dataReader.GetDateTimeNull("DT");
                            clientVisit.NewPolicy.OKATO = uralsibOKATO;
                            clientVisit.NewPolicy.OGRN = uralsibOGRN;

                            clientVisit.OldPolicy.UnifiedPolicyNumber = enp;
                            clientVisit.OldPolicy.OKATO = oldOKATO;
                            if (clientVisit.OldPolicy.OKATO == uralsibOKATO)
                            {
                                clientVisit.OldPolicy.Series = sCard;
                                clientVisit.OldPolicy.Number = nCard;
                            }
                            if (!string.IsNullOrWhiteSpace(ogrnOld))
                            {
                                clientVisit.OldPolicy.OGRN = ogrnOld;
                                clientVisit.OldPolicy.StartDate = oldDP;
                                if (!clientVisit.OldPolicy.StartDate.HasValue ||
                                    clientVisit.OldPolicy.StartDate.Value > new DateTime(2011, 5, 1))
                                {
                                    clientVisit.OldPolicy.PolicyTypeId = enpType;
                                }
                                else
                                {
                                    clientVisit.OldPolicy.PolicyTypeId = kmsType;
                                }
                            }
                        }
                        else
                        {
                            clientVisit.NewPolicy.UnifiedPolicyNumber = enp;
                            clientVisit.NewPolicy.Series = sCard;
                            clientVisit.NewPolicy.Number = nCard;
                            clientVisit.NewPolicy.PolicyTypeId = enpType;
                            clientVisit.NewPolicy.StartDate = clientVisit.TemporaryPolicyDate;
                            clientVisit.NewPolicy.EndDate = dataReader.GetDateTimeNull("DT");
                            clientVisit.NewPolicy.OKATO = uralsibOKATO;
                            clientVisit.NewPolicy.OGRN = uralsibOGRN;
                        }
                    }
                    else
                    {
                        if (clientVisit.Status != ClientVisitStatuses.ReregistrationDone.Id)
                        {
                            clientVisit.NewPolicy.UnifiedPolicyNumber = enp;
                            clientVisit.NewPolicy.Series = sCard;
                            clientVisit.NewPolicy.Number = nCard;
                            clientVisit.NewPolicy.PolicyTypeId = enpType;
                            clientVisit.NewPolicy.StartDate = null;
                            clientVisit.NewPolicy.EndDate = null;
                            if (!string.IsNullOrWhiteSpace(ogrnOld))
                            {
                                clientVisit.OldPolicy.OGRN = ogrnOld;
                                clientVisit.OldPolicy.StartDate = oldDP;
                                if (!clientVisit.OldPolicy.StartDate.HasValue ||
                                    clientVisit.OldPolicy.StartDate.Value > new DateTime(2011, 5, 1))
                                {
                                    clientVisit.OldPolicy.PolicyTypeId = enpType;
                                }
                                else
                                {
                                    clientVisit.OldPolicy.PolicyTypeId = kmsType;
                                }
                            }
                        }
                        else
                        {
                            clientVisit.NewPolicy.UnifiedPolicyNumber = enp;
                            clientVisit.NewPolicy.Series = sCard;
                            clientVisit.NewPolicy.Number = nCard;
                            clientVisit.NewPolicy.PolicyTypeId = enpType;
                            clientVisit.NewPolicy.StartDate = clientVisit.TemporaryPolicyDate;
                            clientVisit.NewPolicy.EndDate = dataReader.GetDateTimeNull("DT");
                            clientVisit.NewPolicy.OKATO = uralsibOKATO;
                            clientVisit.NewPolicy.OGRN = uralsibOGRN;
                            if (!string.IsNullOrWhiteSpace(ogrnOld))
                            {
                                clientVisit.OldPolicy.OGRN = ogrnOld;
                                clientVisit.OldPolicy.OKATO = oldOKATO;
                                clientVisit.OldPolicy.UnifiedPolicyNumber = enp;
                                clientVisit.OldPolicy.StartDate = oldDP;
                                if (!clientVisit.OldPolicy.StartDate.HasValue ||
                                    clientVisit.OldPolicy.StartDate.Value > new DateTime(2011, 5, 1))
                                {
                                    clientVisit.OldPolicy.PolicyTypeId = enpType;
                                }
                                else
                                {
                                    clientVisit.OldPolicy.PolicyTypeId = kmsType;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                clientVisit.Comment = "BAD POLICY DATA";
            }
        }
    }
}
