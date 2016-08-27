using OMInsurance.Configuration;
using OMInsurance.DataAccess.Core;
using OMInsurance.DataAccess.Core.Helpers;
using OMInsurance.DataAccess.Materializers;
using OMInsurance.DBUtils;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.Entities.Searching;
using OMInsurance.Entities.SMS;
using OMInsurance.Entities.Sorting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace OMInsurance.DataAccess.DAO
{
    public class ClientVisitReportDao : ItemDao
    {
        private static ClientVisitReportDao _instance = new ClientVisitReportDao();
        Dictionary<string, ReferenceItem> statusesByCode = ReferencesDao.Instance.GetList(Constants.ClientVisitStatusRef)
                                                                .ToDictionary(item => item.Code.Trim(), item => item);
        Dictionary<string, ReferenceItem> nameTypesByCode = ReferencesDao.Instance.GetList(Constants.CodFioClassifier)
                                                            .ToDictionary(item => item.Code.Trim(), item => item);
        Dictionary<string, ReferenceItem> medCentersByCode = ReferencesDao.Instance.GetList(Constants.MedicalCenterRef)
                                                            .ToDictionary(item => item.Code.Trim(), item => item);
        Dictionary<string, ReferenceItem> categoryByCode = ReferencesDao.Instance.GetList(Constants.ClientCategoryRef)
                                                            .ToDictionary(item => item.Code.Trim(), item => item);
        Dictionary<string, ReferenceItem> doctypeByCode = ReferencesDao.Instance.GetList(Constants.DocumentTypeRef)
                                                            .ToDictionary(item => item.Code.Trim(), item => item);
        Dictionary<string, ReferenceItem> citizenshipByCode = ReferencesDao.Instance.GetList(Constants.CitizenshipRef)
                                                            .ToDictionary(item => item.Code.Trim(), item => item);
        Dictionary<string, ReferenceItem> carriersByCode = ReferencesDao.Instance.GetList(Constants.CarriersRef)
                                                            .ToDictionary(item => item.Code.Trim(), item => item);
        Dictionary<string, ReferenceItem> reprTypeByCode = ReferencesDao.Instance.GetList(Constants.RepresentativeTypeRef)
                                                            .ToDictionary(item => item.Code.Trim(), item => item);
        Dictionary<string, ReferenceItem> applicationMethodsByCode = ReferencesDao.Instance.GetList(Constants.ApplicationMethodRef)
                                                            .ToDictionary(item => item.Code.Trim(), item => item);
        Dictionary<string, ReferenceItem> goznakTypeByCode = ReferencesDao.Instance.GetList(Constants.GOZNAKTypeRef)
                                                            .ToDictionary(item => item.Code.Trim(), item => item);
        Dictionary<string, ReferenceItem> policyTypeByCode = ReferencesDao.Instance.GetList(Constants.PolicyTypeRef)
                                                            .ToDictionary(item => item.Code, item => item);
        Dictionary<string, ReferenceItem> deliveryCenterByCode = ReferencesDao.Instance.GetList(Constants.DeliveryCenterRef)
                                                            .ToDictionary(item => item.Code.Trim(), item => item);
        Dictionary<string, ReferenceItem> scenarioByCode = ReferencesDao.Instance.GetList(Constants.ScenarioRef)
                                                            .ToDictionary(item => item.Code.Trim(), item => item);
        Dictionary<string, ReferenceItem> usibCategoryRef = ReferencesDao.Instance.GetList(Constants.UralsibClientCategoryRef)
                                                            .ToDictionary(item => item.Code.Trim(), item => item);
        Dictionary<long, ReferenceUniversalItem> deliveryPointById = ReferencesDao.Instance.GetUniversalList(Constants.DeliveryPointRef)
                                                            .ToDictionary(item => item.Id, item => item);

        private ClientVisitReportDao()
            : base(DatabaseAliases.OMInsurance, new DatabaseErrorHandler())
        {
        }

        public static ClientVisitReportDao Instance
        {
            get
            {
                return _instance;
            }
        }

        public byte[] GetClientVisitsReport(
             ClientVisitSearchCriteria criteria,
             List<SortCriteria<ClientVisitSortField>> sortCriteria,
             PageRequest pageRequest)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ID", SqlDbType.BigInt, criteria.Id);
            parameters.AddInputParameter("@TemporaryPolicyNumber", SqlDbType.NVarChar, criteria.TemporaryPolicyNumber);
            parameters.AddInputParameter("@TemporaryPolicyDateFrom", SqlDbType.DateTime, criteria.TemporaryPolicyDateFrom);
            parameters.AddInputParameter("@TemporaryPolicyDateTo", SqlDbType.DateTime, criteria.TemporaryPolicyDateTo);
            parameters.AddInputParameter("@PolicyNumber", SqlDbType.NVarChar, criteria.PolicyNumber);
            parameters.AddInputParameter("@PolicySeries", SqlDbType.NVarChar, criteria.PolicySeries);
            parameters.AddInputParameter("@PolicyDateFrom", SqlDbType.DateTime, criteria.PolicyDateFrom);
            parameters.AddInputParameter("@PolicyDateTo", SqlDbType.DateTime, criteria.PolicyDateTo);
            parameters.AddInputParameter("@UpdateDateFrom", SqlDbType.DateTime, criteria.UpdateDateFrom);
            parameters.AddInputParameter("@UpdateDateTo", SqlDbType.DateTime, criteria.UpdateDateTo);

            parameters.AddInputParameter("@Firstname", SqlDbType.NVarChar, criteria.Firstname);
            parameters.AddInputParameter("@Secondname", SqlDbType.NVarChar, criteria.Secondname);
            parameters.AddInputParameter("@Lastname", SqlDbType.NVarChar, criteria.Lastname);

            parameters.AddInputParameter("@Birthday", SqlDbType.Date, criteria.Birthday);
            parameters.AddInputParameter("@DeliveryCenterIds", SqlDbType.Structured, DaoHelper.GetObjectIds(criteria.DeliveryCenterIds));
            parameters.AddInputParameter("@DeliveryPointIds", SqlDbType.Structured, DaoHelper.GetObjectIds(criteria.DeliveryPointIds));
            parameters.AddInputParameter("@ScenarioIds", SqlDbType.Structured, DaoHelper.GetObjectIds(criteria.ScenarioIds));
            parameters.AddInputParameter("@StatusIds", SqlDbType.Structured, DaoHelper.GetObjectIds(criteria.StatusIds));

            SqlParameter totalCountParameter = parameters.AddOutputParameter("@total_count", SqlDbType.Int);
            parameters.AddInputParameter("@sort_criteria", SqlDbType.Structured, DaoHelper.GetSortFieldsTable(sortCriteria));

            parameters.AddInputParameter("@Page_size", SqlDbType.Int, pageRequest.PageSize);
            parameters.AddInputParameter("@Page_number", SqlDbType.Int, pageRequest.PageNumber);

            SqlDataReader reader = DbHelper.ExecuteReader(DatabaseAlias, "ClientVisit_BuildImportDBF", parameters);

            ReferenceItem deliveryCenter = ReferencesDao.Instance.GetList(Constants.DeliveryCenterRef).FirstOrDefault(item => item.Id == criteria.DeliveryCenterIds.FirstOrDefault());
            DBFProcessor pr = new DBFProcessor();
            string filepath = string.Format("e{0}_{1}_{2}.dbf", deliveryCenter != null ? deliveryCenter.Code : Guid.NewGuid().ToString(),
                criteria.UpdateDateFrom.HasValue ?
                criteria.UpdateDateFrom.Value.ToString("ddMMyy") :
                string.Empty,
                criteria.UpdateDateTo.HasValue ?
                criteria.UpdateDateTo.Value.ToString("ddMMyy") :
                string.Empty);
            List<long> ids = new List<long>();
            List<FileWrapper> files = pr.SaveToUralsibDBF(reader, filepath, ids);
            string zipFileName = filepath.Replace("dbf", "zip");
            return ZipHelper.ZipFiles(files);
        }

        #region Parse data

        public List<ClientVisit.UpdateData> GetUpdateDataFromDbf(string filename)
        {
            DBFProcessor pr = new DBFProcessor();
            List<ClientVisit.UpdateData> result = new List<ClientVisit.UpdateData>();
            try
            {
                DataTable table = DBFProcessor.GetDataTable(filename, string.Format("select * from \"{0}\";", filename));
                foreach (DataRow row in table.Rows)
                {
                    ClientVisit.UpdateData data = new ClientVisit.UpdateData();
                    data.UnifiedPolicyNumber = (string)row["ENP"];
                    data.Lastname = (string)row["FAM"];
                    data.Firstname = (string)row["IM"];
                    data.Secondname = (string)row["OT"];
                    DateTime bday;
                    if (DateTime.TryParseExact((string)row["DR"], "yyyyMMdd",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None, out bday))
                    {
                        data.Birthday = bday;
                    }
                    DateTime dat_u;
                    if (DateTime.TryParseExact((string)row["Dat_U"], "yyyyMMdd",
                       CultureInfo.DefaultThreadCurrentCulture,
                       DateTimeStyles.None, out dat_u))
                    {
                        data.Dat_U = dat_u;
                    }
                    DateTime dat_s;
                    if (DateTime.TryParseExact((string)row["Dat_S"], "yyyyMMdd",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None, out dat_s))
                    {
                        data.Dat_S = dat_s;
                    }

                    data.DATA_FOND = (DateTime?)row["DATA_FOND"];
                    data.PolicyPartyNumber = (string)row["PART"];
                    data.NZ_GOZNAK = ((decimal)row["NZ"]).ToString();
                    data.N_KOR = ((decimal)row["N_KOR"]).ToString();
                    data.Blanc = (string)row["Blanc"];
                    data.TemporaryPolicyNumber = (string)row["VS"];
                    data.OGRN = (string)row["OGRN"];
                    data.Sex = (int)((decimal)row["W"]);
                    data.UnifiedPolicyNumber = (string)row["ENP"];
                    result.Add(data);
                }
            }
            finally
            {
                File.Delete(filename);
            }
            return result;
        }

        /// <summary>
        /// Returns list of identifiers from  
        /// </summary>
        /// <param name="filename">Name of the file</param>
        /// <param name="idName">Name of the column that contains identifier</param>
        /// <returns>List of identifiers</returns>
        public List<long> GetClientVisitIdsFromDbf(string filename, string idName)
        {
            DBFProcessor pr = new DBFProcessor();
            string directory = Path.Combine(ConfiguraionProvider.FileStorageFolder, Path.GetFileNameWithoutExtension(filename));
            ZipHelper.UnZipFiles(filename, directory);
            string unzippedDbfFile = Path.Combine(directory, Directory.GetFiles(directory).FirstOrDefault(item => item.EndsWith(".dbf")));
            HashSet<long> result = new HashSet<long>();
            try
            {
                DataTable table = DBFProcessor.GetDataTable(unzippedDbfFile, string.Format("select * from \"{0}\";", unzippedDbfFile));
                foreach (DataRow row in table.Rows)
                {
                    if (table.Columns.Contains(idName))
                    {
                        string stringValue = "";
                        if ((stringValue = (row[idName] as string)) != null)
                        {
                            result.Add(int.Parse(stringValue.Trim()));
                        }
                        else
                        {
                            result.Add((int)row[idName]);
                        }
                    }
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
            return result.ToList();
        }

        public List<ClientVisit> GetDataToCreateClientVisitfromDBF(string filename, List<string> parseErrors)
        {
            DBFProcessor pr = new DBFProcessor();
            string directory = Path.Combine(ConfiguraionProvider.FileStorageFolder, Path.GetFileNameWithoutExtension(filename));
            ZipHelper.UnZipFiles(filename, directory);
            string unzippedDbfFile = Path.Combine(directory, Directory.GetFiles(directory).FirstOrDefault(item => item.EndsWith(".dbf")));
            List<ClientVisit> result = new List<ClientVisit>();
            try
            {
                DataTable table = DBFProcessor.GetDataTable(unzippedDbfFile, string.Format("select * from \"{0}\";", unzippedDbfFile));
                foreach (DataRow row in table.Rows)
                {
                    ClientVisit clientVisit = null;
                    try
                    {
                        clientVisit = FillClientVisitFromDbfRow(row);
                    }
                    catch (InvalidDataException ex)
                    {
                        string lastname = GetString(row, "FAM");
                        string firstname = GetString(row, "IM");
                        string secondname = GetString(row, "OT");
                        int id = (int)row["RECID_PV"];
                        parseErrors.Add(string.Format("Ошибка при загрузке пользователя {0} {1} {2} RECID_PV = {3}. {4}",
                            lastname,
                            firstname,
                            secondname,
                            id,
                            ex.Message));
                    }
                    catch (Exception ex)
                    {
                        string lastname = GetString(row, "FAM");
                        string firstname = GetString(row, "IM");
                        string secondname = GetString(row, "OT");
                        int id = (int)row["RECID_PV"];
                        parseErrors.Add(string.Format("Ошибка при загрузке пользователя {0} {1} {2} RECID_PV = {3}",
                            lastname,
                            firstname,
                            secondname,
                            id));
                    }
                    if (clientVisit != null)
                    {
                        string sourceSignatureFilename = Path.Combine(directory, string.Format("s" + new String('0', 6 - clientVisit.Id.ToString().Length) + "{0}.jpg", clientVisit.Id));
                        string sourcePhotoFilename = Path.Combine(directory, string.Format("f" + new String('0', 6 - clientVisit.Id.ToString().Length) + "{0}.jpg", clientVisit.Id));

                        if (File.Exists(sourceSignatureFilename))
                        {
                            string fname = Guid.NewGuid().ToString();
                            string destinationSignatureFilename = Path.Combine(ConfiguraionProvider.FileStorageFolder, fname);
                            File.Copy(sourceSignatureFilename, destinationSignatureFilename);
                            clientVisit.SignatureFileName = Path.GetFileNameWithoutExtension(fname);
                        }
                        if (File.Exists(sourcePhotoFilename))
                        {
                            string fname = Guid.NewGuid().ToString();
                            string destinationPhotoFilename = Path.Combine(ConfiguraionProvider.FileStorageFolder, fname);
                            File.Copy(sourcePhotoFilename, destinationPhotoFilename);
                            clientVisit.PhotoFileName = Path.GetFileNameWithoutExtension(fname);
                        }
                        result.Add(clientVisit);
                    }
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
            return result;
        }

        private ClientVisit FillClientVisitFromDbfRow(DataRow row)
        {
            ClientVisit clientVisit = new ClientVisit();
            clientVisit.Id = (int)row["RECID_PV"];
            clientVisit.PolicyPartyNumber = GetString(row, "NZ");
            string statusCode = GetIntString(row, "STATUS");
            if (statusesByCode.ContainsKey(statusCode))
            {
                clientVisit.Status = statusesByCode[statusCode];
            }
            else
            {
                throw new InvalidDataException("Указан неверный статус");
            }
            clientVisit.StatusDate = (DateTime)row["DP"];
            string sn_card = GetString(row, "SN_CARD");
            if (!string.IsNullOrEmpty(sn_card) && sn_card.Contains(' '))
            {
                clientVisit.NewPolicy.Series = sn_card.Split(' ')[0];
                clientVisit.NewPolicy.Number = sn_card.Split(' ')[1];
            }
            clientVisit.TemporaryPolicyNumber = GetString(row, "VS");
            clientVisit.NewPolicy.UnifiedPolicyNumber = GetString(row, "ENP");
            clientVisit.NewClientInfo.Lastname = GetString(row, "FAM");
            ReferenceItem lastnameType;
            if (nameTypesByCode.TryGetValue(GetString(row, "D_TYPE1"), out lastnameType))
            {
                clientVisit.NewClientInfo.LastnameType = lastnameType;
            }
            else
            {
                clientVisit.NewClientInfo.LastnameType = nameTypesByCode["1"];
            }

            clientVisit.NewClientInfo.Firstname = GetString(row, "IM");
            ReferenceItem firstnameType;
            if (nameTypesByCode.TryGetValue(GetString(row, "D_TYPE2"), out firstnameType))
            {
                clientVisit.NewClientInfo.FirstnameType = firstnameType;
            }
            else
            {
                clientVisit.NewClientInfo.FirstnameType = nameTypesByCode["1"];
            }

            clientVisit.NewClientInfo.Secondname = GetString(row, "OT");
            ReferenceItem secondnameType;
            if (nameTypesByCode.TryGetValue(GetString(row, "D_TYPE3"), out secondnameType))
            {
                clientVisit.NewClientInfo.SecondnameType = secondnameType;
            }
            else
            {
                clientVisit.NewClientInfo.SecondnameType = nameTypesByCode["1"];
            }
            clientVisit.NewClientInfo.Birthday = GetDateFromRow(row, "DR");
            clientVisit.NewClientInfo.Sex = GetIntString(row, "W");

            if (medCentersByCode.ContainsKey(GetString(row, "MCOD", string.Empty)))
            {
                clientVisit.MedicalCentre = medCentersByCode[GetString(row, "MCOD")];
            }
            else
            {
                clientVisit.MedicalCentre = null;
            }
            ReferenceItem category;
            if (categoryByCode.TryGetValue(GetIntString(row, "KL"), out category))
            {
                clientVisit.NewClientInfo.Category = category;
            }
            else
            {
                clientVisit.NewClientInfo.Category = categoryByCode["00"];
            }

            string unformattedPhone = GetString(row, "CONT");
            string formattedPhone = FormatPhone(unformattedPhone);

            if (!string.IsNullOrEmpty(formattedPhone))
            {
                clientVisit.Comment = string.Format("Телефон из старой системы: {0}", unformattedPhone);
            }

            ReferenceItem doctype;
            string c_doc = GetIntString(row, "C_DOC");
            if (doctypeByCode.TryGetValue(c_doc, out doctype))
            {
                clientVisit.NewDocument.DocumentType = doctype;
            }
            else
            {
                clientVisit.NewDocument.DocumentType = doctypeByCode["14"];
            }
            clientVisit.NewDocument.Series = GetString(row, "S_DOC");
            clientVisit.NewDocument.Number = GetString(row, "N_DOC");
            clientVisit.NewDocument.IssueDate = GetDateFromRow(row, "D_DOC");
            clientVisit.NewDocument.IssueDepartment = GetString(row, "PODR_DOC");
            clientVisit.NewClientInfo.SNILS = GetString(row, "SS");

            ReferenceItem citizenship;
            string gr = GetString(row, "GR");
            if (citizenshipByCode.TryGetValue(gr, out citizenship))
            {
                clientVisit.NewClientInfo.Citizenship = citizenship;
            }
            else
            {
                clientVisit.NewClientInfo.Citizenship = citizenshipByCode["RUS"];
            }
            clientVisit.NewClientInfo.Birthplace = GetString(row, "MR");
            clientVisit.RegistrationAddressDate = GetDateFromRow(row, "DAT_REG");
            string carrierCode = GetIntString(row, "FORM");
            if (carriersByCode.ContainsKey(carrierCode))
            {
                clientVisit.CarrierId = carriersByCode[carrierCode].Id;
            }
            else
            {
                clientVisit.CarrierId = 1;
            }

            string reprTypeCode = GetString(row, "PREDST");
            if (reprTypeByCode.ContainsKey(reprTypeCode))
            {
                clientVisit.Representative.RepresentativeTypeId = reprTypeByCode[reprTypeCode].Id;
            }
            else
            {
                clientVisit.Representative.RepresentativeTypeId = 1;
            }
            string appl = GetIntString(row, "SPOS");
            if (applicationMethodsByCode.ContainsKey(appl))
            {
                clientVisit.ApplicationMethodId = applicationMethodsByCode[appl].Id;
            }
            else
            {
                if (clientVisit.Representative.RepresentativeTypeId == 1)
                {
                    clientVisit.ApplicationMethodId = 1;
                }
                else
                {
                    clientVisit.ApplicationMethodId = 2;
                }
            }

            string d_type4 = GetIntString(row, "D_TYPE4");
            if (goznakTypeByCode.ContainsKey(d_type4))
            {
                clientVisit.GOZNAKType = goznakTypeByCode[d_type4];
            }
            else
            {
                clientVisit.GOZNAKType = goznakTypeByCode["0"];
            }
            clientVisit.Comment = GetString(row, "COMMENT") + " " + clientVisit.Comment;

            string p_doc = GetIntString(row, "P_DOC");
            if (policyTypeByCode.ContainsKey(p_doc))
            {
                clientVisit.NewPolicy.PolicyType = policyTypeByCode[p_doc];
            }
            clientVisit.TemporaryPolicyDate = GetDateFromRow(row, "VS_DATA");
            clientVisit.CreateDate = clientVisit.TemporaryPolicyDate ?? clientVisit.StatusDate;
            clientVisit.GOZNAKDate = GetDateFromRow(row, "GZ_DATA");
            string pv = GetString(row, "PV");
            if (deliveryCenterByCode.ContainsKey(pv))
            {
                clientVisit.DeliveryCenter = new DeliveryCenter() { Id = deliveryCenterByCode[pv].Id };
            }
            else
            {
                throw new InvalidDataException("Не указан номер пункта выдачи");
            }

            clientVisit.OldDocument.Series = GetString(row, "OS_DOC");
            clientVisit.OldDocument.Number = GetString(row, "ON_DOC");
            if (row.Table.Columns.Contains("OD_DOC"))
            {
                clientVisit.OldDocument.IssueDate = GetDateFromRow(row, "OD_DOC");
            }

            ReferenceItem odoctype;
            string oc_doc = GetIntString(row, "OC_DOC");
            if (doctypeByCode.TryGetValue(oc_doc, out odoctype))
            {
                clientVisit.OldDocument.DocumentType = odoctype;
            }
            decimal ow = (decimal)row["OW"];
            clientVisit.OldClientInfo.Sex = ow != 0 ? ow.ToString() : string.Empty;
            clientVisit.OldClientInfo.Birthday = GetDateFromRow(row, "ODR");
            clientVisit.OldClientInfo.Lastname = GetString(row, "OFAM");
            clientVisit.OldClientInfo.Firstname = GetString(row, "OIM");
            clientVisit.OldClientInfo.Secondname = GetString(row, "OOT");

            string scn = GetString(row, "SCN");
            if (scenarioByCode.ContainsKey(scn))
            {
                clientVisit.Scenario = scenarioByCode[scn];
            }
            if (GetString(row, "JT") == "r")
            {
                clientVisit.Scenario = ClientVisitScenaries.PolicyExtradition;
            }
            clientVisit.NewDocument.ExpirationDate = GetDateFromRow(row, "E_DOC");
            string usibCategory = GetString(row, "KTG");
            if (usibCategoryRef.ContainsKey(usibCategory))
            {
                clientVisit.ClientCategoryId = usibCategoryRef[usibCategory].Id;
            }

            // for the old format
            if (row.Table.Columns.Contains("X_DOC"))
            {
                string x_doc = GetIntString(row, "X_DOC");
                if (x_doc == "1")
                {
                    clientVisit.NewDocument.IsIssueCase = true;
                }
            }

            clientVisit.LivingAddress.TerritoryCode = "45";
            clientVisit.LivingAddress.RegionCode = "000";
            clientVisit.LivingAddress.Region = "г Москва";
            clientVisit.LivingAddress.RegionId = "0c5b2444-70a0-4932-980c-b4dc0d3f02b5";
            clientVisit.LivingAddress.StreetCode = GetIntString(row, "Ul");
            clientVisit.LivingAddress.House = GetString(row, "D");
            clientVisit.LivingAddress.Housing = GetString(row, "KOR");
            clientVisit.LivingAddress.Building = GetString(row, "STR");
            clientVisit.LivingAddress.Appartment = GetString(row, "KV");
            string c_okato = GetString(row, "C_OKATO");
            if (!string.IsNullOrEmpty(c_okato) && c_okato.Length == 5)
            {
                clientVisit.RegistrationAddress.TerritoryCode = c_okato.Substring(0, 2);
                clientVisit.RegistrationAddress.RegionCode = c_okato.Substring(2, 3);
            }
            clientVisit.RegistrationAddress.Area = GetString(row, "RA_NAME");
            clientVisit.RegistrationAddress.City = GetString(row, "NP_NAME");
            clientVisit.RegistrationAddress.Street = GetString(row, "UL_NAME");
            clientVisit.RegistrationAddress.House = GetString(row, "DOM2");
            clientVisit.RegistrationAddress.Housing = GetString(row, "KOR2");
            clientVisit.RegistrationAddress.Building = GetString(row, "STR2");
            clientVisit.RegistrationAddress.Appartment = GetString(row, "KV2");

            ReferenceItem forDocumentType;
            string c_perm = GetIntString(row, "C_PERM");
            if (doctypeByCode.TryGetValue(c_perm, out forDocumentType))
            {
                clientVisit.NewForeignDocument.DocumentType = forDocumentType;
            }
            else
            {
                clientVisit.NewForeignDocument.DocumentType = null;
            }

            clientVisit.NewForeignDocument.Series = GetString(row, "S_PERM");
            clientVisit.NewForeignDocument.Number = GetString(row, "N_PERM");
            clientVisit.NewForeignDocument.IssueDate = GetDateFromRow(row, "D_PERM");
            clientVisit.Representative.Lastname = GetString(row, "PRFAM");
            clientVisit.Representative.Firstname = GetString(row, "PRIM");
            clientVisit.Representative.Secondname = GetString(row, "PROT");

            ReferenceItem predDocumentType;
            string prc_doc = GetIntString(row, "PRC_DOC");
            if (doctypeByCode.TryGetValue(prc_doc, out predDocumentType))
            {
                clientVisit.Representative.DocumentType = predDocumentType;
            }
            else
            {
                clientVisit.Representative.DocumentType = null;
            }
            clientVisit.Representative.Series = GetString(row, "PRS_DOC");
            clientVisit.Representative.Number = GetString(row, "PRN_DOC");
            clientVisit.Representative.IssueDate = GetDateFromRow(row, "PRD_DOC");
            FillPolicyDataFromDbfRow(row, clientVisit);
            return clientVisit;
        }

        private static string FormatPhone(string unformattedPhone)
        {
            string formattedPhone = unformattedPhone;
            if (string.IsNullOrEmpty(unformattedPhone))
            {
                return "(000)000-00-00";
            }
            string phone = string.Concat(unformattedPhone.Where(c => char.IsDigit(c)));
            if (phone.Length == 10)
            {
                formattedPhone = string.Format("({0}){1}-{2}-{3}",
                       phone.Substring(0, 3),
                       phone.Substring(3, 3),
                       phone.Substring(6, 2),
                       phone.Substring(8, 2));
            }
            else if (phone.Length == 11 && (phone[0] == '8' || phone[0] == '7'))
            {
                formattedPhone = string.Format("({0}){1}-{2}-{3}",
                    phone.Substring(1, 3),
                    phone.Substring(4, 3),
                    phone.Substring(7, 2),
                    phone.Substring(9, 2));
            }
            else if (phone.Length == 12 && phone[0] == '+' && phone[1] == '7')
            {
                formattedPhone = string.Format("({0}){1}-{2}-{3}",
                    phone.Substring(2, 3),
                    phone.Substring(5, 3),
                    phone.Substring(8, 2),
                    phone.Substring(10, 2));
            }
            else if (phone.Length == 7)
            {
                formattedPhone = string.Format("(000){0}-{1}-{2}", phone.Substring(0, 3), phone.Substring(3, 2), phone.Substring(5, 2));
            }
            return formattedPhone;
        }

        private static void FillPolicyDataFromDbfRow(DataRow row, ClientVisit clientVisit)
        {
            long kmsType = 1;
            long enpType = 3;
            string enp = GetString(row, "ENP");
            string sn_card = GetString(row, "SN_CARD");
            string sCard = null;
            string nCard = null;
            if (!string.IsNullOrEmpty(sn_card) && sn_card.Contains(' '))
            {
                sCard = sn_card.Split(' ')[0];
                nCard = sn_card.Split(' ')[1];
            }
            string uralsibOKATO = "45000";
            string uralsibOGRN = "1025002690877";
            string oldOKATO = GetString(row, "OKATO_OLD");
            DateTime? oldDP = GetDateFromRow(row, "DP_OLD");
            string ogrnOld2 = GetString(row, "OGRN_OLD2");
            string nz = clientVisit.PolicyPartyNumber;
            string ogrnOld = GetString(row, "OGRN_OLD");

            if (clientVisit.StatusDate < new DateTime(2011, 05, 1) ||
                clientVisit.StatusDate > new DateTime(2011, 04, 30)
                && !string.IsNullOrEmpty(nz) && nz.All(c => char.IsDigit(c)))
            {
                clientVisit.NewPolicy.UnifiedPolicyNumber = enp;
                clientVisit.NewPolicy.Series = sCard;
                clientVisit.NewPolicy.Number = nCard;
                clientVisit.NewPolicy.PolicyType = new ReferenceItem() { Id = kmsType };
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
                if (!string.IsNullOrWhiteSpace(nz)
                    && nz.Any(c => char.IsLetter(c)))
                {
                    clientVisit.NewPolicy.UnifiedPolicyNumber = enp;
                    clientVisit.NewPolicy.Series = sCard;
                    clientVisit.NewPolicy.Number = nCard;
                    clientVisit.NewPolicy.PolicyType = new ReferenceItem() { Id = enpType };
                    clientVisit.NewPolicy.OKATO = uralsibOKATO;
                    clientVisit.NewPolicy.OGRN = uralsibOGRN;
                    clientVisit.NewPolicy.StartDate = clientVisit.TemporaryPolicyDate;
                    clientVisit.NewPolicy.EndDate = GetDateFromRow(row, "DT");
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
                            {
                                clientVisit.OldPolicy.PolicyType = new ReferenceItem() { Id = kmsType };
                            }
                            else
                            {
                                clientVisit.OldPolicy.PolicyType = new ReferenceItem() { Id = enpType };
                            }
                        }
                    }
                }
                else if (string.IsNullOrWhiteSpace(nz)
                        && string.IsNullOrWhiteSpace(clientVisit.TemporaryPolicyNumber)
                        && clientVisit.Status.Id != ClientVisitStatuses.ErrorEntry.Id &&
                            clientVisit.Status.Id != ClientVisitStatuses.PolicyMadeByAnotherCompany.Id)
                {
                    clientVisit.NewPolicy.UnifiedPolicyNumber = enp;
                    clientVisit.NewPolicy.Series = sCard;
                    clientVisit.NewPolicy.Number = nCard;
                    clientVisit.NewPolicy.PolicyType = new ReferenceItem() { Id = enpType };
                    clientVisit.NewPolicy.OKATO = uralsibOKATO;
                    clientVisit.NewPolicy.OGRN = uralsibOGRN;
                    clientVisit.NewPolicy.StartDate = clientVisit.TemporaryPolicyDate;
                    clientVisit.NewPolicy.EndDate = GetDateFromRow(row, "DT");
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
                            clientVisit.OldPolicy.PolicyType = new ReferenceItem() { Id = enpType };
                        }
                        else
                        {
                            clientVisit.OldPolicy.PolicyType = new ReferenceItem() { Id = kmsType };
                        }
                    }
                }
                else if (string.IsNullOrWhiteSpace(clientVisit.TemporaryPolicyNumber) &&
                  string.IsNullOrWhiteSpace(nz) &&
                  (clientVisit.Status.Id == ClientVisitStatuses.ErrorEntry.Id ||
                   clientVisit.Status.Id == ClientVisitStatuses.PolicyMadeByAnotherCompany.Id)
                   || !string.IsNullOrWhiteSpace(clientVisit.TemporaryPolicyNumber) &&
                      string.IsNullOrWhiteSpace(nz))
                {
                    clientVisit.NewPolicy.UnifiedPolicyNumber = enp;
                    clientVisit.NewPolicy.Series = sCard;
                    clientVisit.NewPolicy.Number = nCard;
                    clientVisit.NewPolicy.PolicyType = new ReferenceItem() { Id = enpType };
                    clientVisit.NewPolicy.OKATO = string.Empty;
                    clientVisit.NewPolicy.OGRN = string.Empty;
                    if (!string.IsNullOrWhiteSpace(ogrnOld))
                    {
                        clientVisit.OldPolicy.UnifiedPolicyNumber = enp;
                        clientVisit.OldPolicy.OKATO = oldOKATO;
                        clientVisit.OldPolicy.OGRN = ogrnOld;
                        clientVisit.OldPolicy.StartDate = oldDP;
                        if (!clientVisit.OldPolicy.StartDate.HasValue ||
                            clientVisit.OldPolicy.StartDate.Value > new DateTime(2011, 5, 1))
                        {
                            clientVisit.OldPolicy.PolicyType = new ReferenceItem() { Id = enpType };
                        }
                        else
                        {
                            clientVisit.OldPolicy.PolicyType = new ReferenceItem() { Id = kmsType };
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
                    if (clientVisit.Status.Id == ClientVisitStatuses.PolicyIssued.Id
                        || clientVisit.Status.Id == ClientVisitStatuses.ReregistrationDone.Id
                        || clientVisit.Status.Id == ClientVisitStatuses.SentToGoznak.Id)
                    {
                        clientVisit.NewPolicy.UnifiedPolicyNumber = enp;
                        clientVisit.NewPolicy.Series = sCard;
                        clientVisit.NewPolicy.Number = nCard;
                        clientVisit.NewPolicy.PolicyType = new ReferenceItem() { Id = enpType };
                        clientVisit.NewPolicy.StartDate = clientVisit.TemporaryPolicyDate;
                        clientVisit.NewPolicy.EndDate = GetDateFromRow(row, "DT");
                        clientVisit.NewPolicy.OKATO = uralsibOKATO;
                        clientVisit.NewPolicy.OGRN = uralsibOGRN;
                    }
                    else
                    {
                        clientVisit.NewPolicy.UnifiedPolicyNumber = enp;
                        clientVisit.NewPolicy.Series = sCard;
                        clientVisit.NewPolicy.Number = nCard;
                        clientVisit.NewPolicy.PolicyType = new ReferenceItem() { Id = enpType };
                        clientVisit.NewPolicy.StartDate = null;
                        clientVisit.NewPolicy.EndDate = null;
                        clientVisit.NewPolicy.OKATO = string.Empty;
                        clientVisit.NewPolicy.OGRN = string.Empty;
                    }

                    clientVisit.OldPolicy.UnifiedPolicyNumber = (string)row["ENP2"];
                    clientVisit.OldPolicy.Series = GetString(row, "S_CARD2");
                    clientVisit.OldPolicy.Number = GetString(row, "N_CARD2");
                    clientVisit.OldPolicy.StartDate = GetDateFromRow(row, "DP_OLD2");
                    clientVisit.OldPolicy.OKATO = GetString(row, "OKATO_old2");
                    clientVisit.OldPolicy.OGRN = ogrnOld2;
                    if (!clientVisit.OldPolicy.StartDate.HasValue ||
                            clientVisit.OldPolicy.StartDate.Value > new DateTime(2011, 5, 1))
                    {
                        clientVisit.OldPolicy.PolicyType = new ReferenceItem() { Id = enpType };
                    }
                    else
                    {
                        clientVisit.OldPolicy.PolicyType = new ReferenceItem() { Id = kmsType };
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(nz))
                    {
                        if (nz[0] <= 'B' && int.Parse(nz.Substring(1)) < 76)
                        {
                            clientVisit.NewPolicy.UnifiedPolicyNumber = enp;
                            clientVisit.NewPolicy.Series = sCard;
                            clientVisit.NewPolicy.Number = nCard;
                            clientVisit.NewPolicy.PolicyType = new ReferenceItem() { Id = enpType };
                            clientVisit.NewPolicy.StartDate = clientVisit.TemporaryPolicyDate;
                            clientVisit.NewPolicy.EndDate = GetDateFromRow(row, "DT");
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
                                    clientVisit.OldPolicy.PolicyType = new ReferenceItem() { Id = enpType };
                                }
                                else
                                {
                                    clientVisit.OldPolicy.PolicyType = new ReferenceItem() { Id = kmsType };
                                }
                            }
                        }
                        else
                        {
                            clientVisit.NewPolicy.UnifiedPolicyNumber = enp;
                            clientVisit.NewPolicy.Series = sCard;
                            clientVisit.NewPolicy.Number = nCard;
                            clientVisit.NewPolicy.PolicyType = new ReferenceItem() { Id = enpType };
                            clientVisit.NewPolicy.StartDate = clientVisit.TemporaryPolicyDate;
                            clientVisit.NewPolicy.EndDate = GetDateFromRow(row, "DT");
                            clientVisit.NewPolicy.OKATO = uralsibOKATO;
                            clientVisit.NewPolicy.OGRN = uralsibOGRN;
                        }
                    }
                    else
                    {
                        if (clientVisit.Status.Id != ClientVisitStatuses.ReregistrationDone.Id)
                        {
                            clientVisit.NewPolicy.UnifiedPolicyNumber = enp;
                            clientVisit.NewPolicy.Series = sCard;
                            clientVisit.NewPolicy.Number = nCard;
                            clientVisit.NewPolicy.PolicyType = new ReferenceItem() { Id = enpType };
                            clientVisit.NewPolicy.StartDate = null;
                            clientVisit.NewPolicy.EndDate = null;
                            if (!string.IsNullOrWhiteSpace(ogrnOld))
                            {
                                clientVisit.OldPolicy.OGRN = ogrnOld;
                                clientVisit.OldPolicy.StartDate = oldDP;
                                if (!clientVisit.OldPolicy.StartDate.HasValue ||
                                    clientVisit.OldPolicy.StartDate.Value > new DateTime(2011, 5, 1))
                                {
                                    clientVisit.OldPolicy.PolicyType = new ReferenceItem() { Id = enpType };
                                }
                                else
                                {
                                    clientVisit.OldPolicy.PolicyType = new ReferenceItem() { Id = kmsType };
                                }
                            }
                        }
                        else
                        {
                            clientVisit.NewPolicy.UnifiedPolicyNumber = enp;
                            clientVisit.NewPolicy.Series = sCard;
                            clientVisit.NewPolicy.Number = nCard;
                            clientVisit.NewPolicy.PolicyType = new ReferenceItem() { Id = enpType };
                            clientVisit.NewPolicy.StartDate = clientVisit.TemporaryPolicyDate;
                            clientVisit.NewPolicy.EndDate = GetDateFromRow(row, "DT");
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
                                    clientVisit.OldPolicy.PolicyType = new ReferenceItem() { Id = enpType };
                                }
                                else
                                {
                                    clientVisit.OldPolicy.PolicyType = new ReferenceItem() { Id = kmsType };
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

        #endregion

        private static DateTime? GetDateFromRow(DataRow row, string field)
        {
            DateTime? date = (DateTime?)row[field];
            if (date.HasValue && date.Value.Year == 1899 && date.Value.Month == 12 && date.Value.Day == 30)
            {
                return null;
            }
            return date;
        }

        private static string GetString(DataRow row, string fieldName, string defaultValue = null)
        {
            if (DBNull.Value == row[fieldName])
            {
                return defaultValue;
            }
            string stringValue = (string)row[fieldName];
            return stringValue.Trim();
        }

        private static string ConvertToDos(string str)
        {
            if (str == null) return null;
            Encoding windows = Encoding.GetEncoding(1251);
            Encoding dos = Encoding.GetEncoding(866);
            return dos.GetString(windows.GetBytes(str));
        }

        private static string GetConvertedString(DataRow row, string fieldName, string defaultValue = null)
        {
            if (DBNull.Value == row[fieldName])
            {
                return defaultValue;
            }
            string stringValue = (string)row[fieldName];
            return ConvertToDos(stringValue).Trim();
        }

        private static string GetIntString(DataRow row, string fieldName, string defaultValue = null)
        {
            if (row[fieldName] != DBNull.Value)
            {
                int value = (int)((decimal)row[fieldName]);
                return value.ToString();
            }
            return defaultValue;
        }

        private static string GetLongString(DataRow row, string fieldName, string defaultValue = null)
        {
            if (row[fieldName] != DBNull.Value)
            {
                long value = (long)((decimal)row[fieldName]);
                return value.ToString();
            }
            return defaultValue;
        }

        public void BuildClientVisitDataFromMFC(
            Dictionary<int, ClientVisit.SaveData> clientVisitByRecid,
            List<string> parseErrors,
            string persFile)
        {
            System.IO.FileInfo persFileInfo = new System.IO.FileInfo(persFile);
            DataTable table = DBFProcessor.GetDataTable(persFile, string.Format("select * from \"{0}\";", persFileInfo.FullName));
            foreach (DataRow row in table.Rows)
            {
                int id = 0;
                string recid = (string)row["RECID"];
                if (!int.TryParse(recid, out id))
                {
                    continue;
                }
                try
                {
                    if (clientVisitByRecid.ContainsKey(id))
                    {
                        ClientVisit.SaveData oldData = ParseFromMFCDbf(row, parseErrors);
                        clientVisitByRecid[id].OldPolicy = oldData.NewPolicy;
                        clientVisitByRecid[id].OldClientInfo = oldData.NewClientInfo;
                    }
                    else
                    {
                        ClientVisit.SaveData data = ParseFromMFCDbf(row, parseErrors);
                        clientVisitByRecid.Add(id, data);
                    }
                }
                catch (Exception ex)
                {
                    string lastname = string.Empty;
                    string firstname = string.Empty;
                    string secondname = string.Empty;
                    try
                    {
                        lastname = GetConvertedString(row, "FAM");
                        firstname = GetConvertedString(row, "IM");
                        secondname = GetConvertedString(row, "OT");
                    }
                    catch { }
                    recid = (string)row["RECID"];
                    parseErrors.Add(string.Format("Ошибка при загрузке пользователя {0} {1} {2} RECID = {3}: {4}",
                        lastname,
                        firstname,
                        secondname,
                        recid,
                        ex.Message));
                }
            }
        }

        public void BuildDocumentsFromMFC(Dictionary<int, ClientVisit.SaveData> clientVisitByRecid,
            List<string> errors,
            string docFile)
        {
            System.IO.FileInfo docFileInfo = new System.IO.FileInfo(docFile);
            DataTable table = DBFProcessor.GetDataTable(docFile, string.Format("select * from \"{0}\";", docFileInfo.FullName));
            foreach (DataRow row in table.Rows)
            {
                int id = 0;
                string recid = (string)row["RECID"];
                if (!int.TryParse(recid, out id))
                {
                    continue;
                }
                try
                {
                    ClientVisit.SaveData data;
                    if (clientVisitByRecid.TryGetValue(id, out data))
                    {
                        ParseDocumentData(data, row);
                    }
                }
                catch (Exception ex)
                {
                    string lastname = GetConvertedString(row, "FAM");
                    string firstname = GetConvertedString(row, "IM");
                    string secondname = GetConvertedString(row, "OT");
                    recid = (string)row["RECID"];
                    errors.Add(string.Format("Ошибка при загрузке пользователя {0} {1} {2} RECID = {3}: {4}",
                        lastname,
                        firstname,
                        secondname,
                        recid,
                        ex.Message));
                }
            }
        }

        public void BuildAddressedFromMFC(Dictionary<int, ClientVisit.SaveData> clientVisitByRecid,
            List<string> errors,
            string addrFile)
        {
            System.IO.FileInfo addrFileInfo = new System.IO.FileInfo(addrFile);
            DataTable table = DBFProcessor.GetDataTable(addrFile, string.Format("select * from \"{0}\";", addrFileInfo.FullName));
            foreach (DataRow row in table.Rows)
            {
                int id = 0;
                string recid = (string)row["RECID"];
                if (!int.TryParse(recid, out id))
                {
                    continue;
                }

                try
                {
                    ClientVisit.SaveData data;
                    if (clientVisitByRecid.TryGetValue(id, out data))
                    {
                        ParseAddressData(data, row);
                    }
                }
                catch (Exception ex)
                {
                    string lastname = GetConvertedString(row, "FAM");
                    string firstname = GetConvertedString(row, "IM");
                    string secondname = GetConvertedString(row, "OT");
                    recid = (string)row["RECID"];
                    errors.Add(string.Format("Ошибка при загрузке пользователя {0} {1} {2} RECID = {3}: {4}",
                        lastname,
                        firstname,
                        secondname,
                        recid,
                        ex.Message));
                }
            }
        }


        public void BuildClientVisitFromMFC_OldData(
            Dictionary<long, ClientVisit.SaveData> clientVisitByRecid,
            List<string> parseErrors,
            string persFile)
        {
            System.IO.FileInfo persFileInfo = new System.IO.FileInfo(persFile);
            DataTable table = DBFProcessor.GetDataTable(persFile, string.Format("select * from \"{0}\";", persFileInfo.FullName));
            foreach (DataRow row in table.Rows)
            {
                long id = 0;
                string recid = Path.GetFileNameWithoutExtension(persFile).Replace("fioP","")+ (string)row["RECID"];
                //string recid = (string)row["RECID"];
                if (!long.TryParse(recid, out id))
                {
                    continue;
                }
                try
                {
                    if (clientVisitByRecid.ContainsKey(id))
                    {
                        ClientVisit.SaveData oldData = ParseFromMFCDbfOldData(row, parseErrors);
                        clientVisitByRecid[id].OldPolicy = oldData.NewPolicy;
                        clientVisitByRecid[id].OldClientInfo = oldData.NewClientInfo;
                    }
                    else
                    {
                        ClientVisit.SaveData data = ParseFromMFCDbfOldData(row, parseErrors);
                        clientVisitByRecid.Add(id, data);
                    }
                }
                catch (Exception ex)
                {
                    string lastname = string.Empty;
                    string firstname = string.Empty;
                    string secondname = string.Empty;
                    try
                    {
                        lastname = GetConvertedString(row, "FAM");
                        firstname = GetConvertedString(row, "IM");
                        secondname = GetConvertedString(row, "OT");
                    }
                    catch { }
                    parseErrors.Add(string.Format("Ошибка при загрузке пользователя {0} {1} {2} RECID_oldData = {3}: {4}",
                        lastname,
                        firstname,
                        secondname,
                        recid,
                        ex.Message));
                }
            }
        }

        public void BuildDocumentsFromMFC_OldData(Dictionary<long, ClientVisit.SaveData> clientVisitByRecid,
            List<string> errors,
            string dulFile)
        {
            System.IO.FileInfo docFileInfo = new System.IO.FileInfo(dulFile);
            DataTable table = DBFProcessor.GetDataTable(dulFile, string.Format("select * from \"{0}\";", docFileInfo.FullName));
            foreach (DataRow row in table.Rows)
            {
                long id = 0;
                string recid = Path.GetFileNameWithoutExtension(dulFile).Replace("dulP", "").Replace("docP", "") + (string)row["RECID"];
                //string recid = (string)row["RECID"];
                if (!long.TryParse(recid, out id))
                {
                    continue;
                }
                try
                {
                    ClientVisit.SaveData data;
                    if (clientVisitByRecid.TryGetValue(id, out data))
                    {
                        ParseDocumentDataOldData(data, row);
                    }
                }
                catch (Exception ex)
                {
                    string lastname = GetConvertedString(row, "FAM");
                    string firstname = GetConvertedString(row, "IM");
                    string secondname = GetConvertedString(row, "OT");
                    errors.Add(string.Format("Ошибка при загрузке пользователя {0} {1} {2} RECID_oldData = {3}: {4}",
                        lastname,
                        firstname,
                        secondname,
                        recid,
                        ex.Message));
                }
            }
        }

        public void BuildAddressedFromMFC_OldData(Dictionary<long, ClientVisit.SaveData> clientVisitByRecid,
            List<string> errors,
            string addrFile)
        {
            System.IO.FileInfo addrFileInfo = new System.IO.FileInfo(addrFile);
            DataTable table = DBFProcessor.GetDataTable(addrFile, string.Format("select * from \"{0}\";", addrFileInfo.FullName));
            foreach (DataRow row in table.Rows)
            {
                long id = 0;
                string recid = Path.GetFileNameWithoutExtension(addrFile).Replace("adrP", "") + (string)row["RECID"];
                //string recid = (string)row["RECID"];
                if (!long.TryParse(recid, out id))
                {
                    continue;
                }

                try
                {
                    ClientVisit.SaveData data;
                    if (clientVisitByRecid.TryGetValue(id, out data))
                    {
                        ParseAddressDataOldData(data, row);
                    }
                }
                catch (Exception ex)
                {
                    string lastname = GetConvertedString(row, "FAM");
                    string firstname = GetConvertedString(row, "IM");
                    string secondname = GetConvertedString(row, "OT");
                    errors.Add(string.Format("Ошибка при загрузке пользователя {0} {1} {2} RECID_oldData = {3}: {4}",
                        lastname,
                        firstname,
                        secondname,
                        recid,
                        ex.Message));
                }
            }
        }

        private void ParseAddressData(ClientVisit.SaveData data, DataRow row)
        {
            data.LivingAddress.TerritoryCode = "45";
            data.LivingAddress.RegionCode = "000";
            data.LivingAddress.Region = "г Москва";
            data.LivingAddress.RegionId = "0c5b2444-70a0-4932-980c-b4dc0d3f02b5";
            data.LivingAddress.StreetCode = GetIntString(row, "ADR_MSK");
            data.LivingAddress.Street = GetConvertedString(row, "ADR_UL");
            data.LivingAddress.House = GetConvertedString(row, "ADR_DOM");
            data.LivingAddress.Housing = GetConvertedString(row, "ADR_KOR");
            data.LivingAddress.Building = GetConvertedString(row, "ADR_STR");
            data.LivingAddress.Appartment = GetConvertedString(row, "ADR_KVR");
            data.RegistrationAddressDate = GetDateFromRow(row, "DAT_REG");
            data.LivingAddress.FullAddressString = GetConvertedString(row, "ADR_ADR");
        }

        private void ParseAddressDataOldData(ClientVisit.SaveData data, DataRow row)
        {
            data.LivingAddress.TerritoryCode = "45";
            data.LivingAddress.RegionCode = "000";
            data.LivingAddress.Region = "г Москва";
            data.LivingAddress.RegionId = "0c5b2444-70a0-4932-980c-b4dc0d3f02b5";
            //data.LivingAddress.StreetCode = GetIntString(row, "ADR_MSK");
            data.LivingAddress.Street = GetConvertedString(row, "ADR_UL");
            data.LivingAddress.House = GetConvertedString(row, "ADR_DOM");
            data.LivingAddress.Housing = GetConvertedString(row, "ADR_KOR");
            data.LivingAddress.Building = GetConvertedString(row, "ADR_STR");
            data.LivingAddress.Appartment = GetConvertedString(row, "ADR_KVR");
            data.RegistrationAddressDate = GetDateFromRow(row, "DAT_REG");
            data.LivingAddress.FullAddressString = GetConvertedString(row, "ADR_ADR");
        }

        private ClientVisit.SaveData ParseFromMFCDbf(DataRow row, List<string> parseErrors)
        {
            ClientVisit.SaveData data = new ClientVisit.SaveData();
            string scn = GetConvertedString(row, "SCENARIO");
            if (scenarioByCode.ContainsKey(scn))
            {
                data.ScenarioId = scenarioByCode[scn].Id;
            }
            string pv = GetConvertedString(row, "PV");
            if (deliveryCenterByCode.ContainsKey(pv))
            {
                data.DeliveryCenterId = deliveryCenterByCode[pv].Id;
            }
            else
            {
                throw new InvalidDataException("Не указан номер пункта выдачи");
            }

            var deliveryPoint = deliveryPointById.Values.FirstOrDefault(item => item.DeliveryCenterId == data.DeliveryCenterId);
            if (deliveryPoint != null && !data.DeliveryPointId.HasValue)
            {
                data.DeliveryPointId = deliveryPoint.Id;
            }
            data.StatusDate = (DateTime)row["DP"];
            data.TemporaryPolicyDate = (DateTime)row["DP"];

            string unformattedPhone = GetConvertedString(row, "CONT");
            data.ClientContacts = unformattedPhone;
            string formattedPhone = FormatPhone(unformattedPhone);

            if (!string.IsNullOrEmpty(formattedPhone))
            {
                data.Comment = string.Format("Телефон из старой системы: {0}", unformattedPhone);
            }
            string carrierCode = GetIntString(row, "FORM");
            if (carriersByCode.ContainsKey(carrierCode))
            {
                data.CarrierId = carriersByCode[carrierCode].Id;
            }
            else
            {
                data.CarrierId = 1;
            }
            string reprTypeCode = GetIntString(row, "PREDST");
            if (reprTypeByCode.ContainsKey(reprTypeCode))
            {
                data.Representative.RepresentativeTypeId = reprTypeByCode[reprTypeCode].Id;
            }
            else
            {
                data.Representative.RepresentativeTypeId = 1;
            }
            string appl = GetIntString(row, "SPOS");
            if (applicationMethodsByCode.ContainsKey(appl))
            {
                data.ApplicationMethodId = applicationMethodsByCode[appl].Id;
            }
            else
            {
                if (data.Representative.RepresentativeTypeId == 1)
                {
                    data.ApplicationMethodId = 1;
                }
                else
                {
                    data.ApplicationMethodId = 2;
                }
            }

            string D_GZK = GetIntString(row, "D_GZK");
            if (goznakTypeByCode.ContainsKey(D_GZK))
            {
                data.GOZNAKTypeId = goznakTypeByCode[D_GZK].Id;
            }
            else
            {
                data.GOZNAKTypeId = goznakTypeByCode["0"].Id;
            }
            ParseClientData(data, row);
            ParsePolicyData(data, row);
            return data;
        }

        private ClientVisit.SaveData ParseFromMFCDbfOldData(DataRow row, List<string> parseErrors)
        {
            ClientVisit.SaveData data = new ClientVisit.SaveData();
            string scn = GetConvertedString(row, "JT");
            if(scn.Trim() == "7")
            {
                scn = "CP";
            }
            else if(scn.Trim().ToLower() == "z")
            {
                scn = "PI";
            }
            else if (scn.Trim().ToLower() == "1")
            {
                scn = "NB";
            }
            else if (scn.Trim().ToLower() == "3")
            {
                scn = "DP";
            }
            else if (scn.Trim().ToLower() == "4")
            {
                scn = "CR";
            }
            else if (scn.Trim().ToLower() == "b")
            {
                scn = "PRI";
            }
            else if (scn.Trim().ToLower() == "d")
            {
                scn = "CLR";
            }
            else if (scn.Trim().ToLower() == "v")
            {
                scn = "";
            }
            if (scenarioByCode.ContainsKey(scn))
            {
                data.ScenarioId = scenarioByCode[scn].Id;
            }
            string pv = GetConvertedString(row, "PV");
            if (deliveryCenterByCode.ContainsKey(pv))
            {
                data.DeliveryCenterId = deliveryCenterByCode[pv].Id;
            }
            else
            {
                throw new InvalidDataException("Не указан номер пункта выдачи");
            }

            var deliveryPoint = deliveryPointById.Values.FirstOrDefault(item => item.DeliveryCenterId == data.DeliveryCenterId);
            if (deliveryPoint != null && !data.DeliveryPointId.HasValue)
            {
                data.DeliveryPointId = deliveryPoint.Id;
            }
            data.StatusDate = (DateTime)row["DP"];
            data.TemporaryPolicyDate = (DateTime)row["DP"];

            string unformattedPhone = GetConvertedString(row, "CONT");
            data.ClientContacts = unformattedPhone;
            string formattedPhone = FormatPhone(unformattedPhone);

            if (!string.IsNullOrEmpty(formattedPhone))
            {
                data.Comment = string.Format("Телефон из старой системы: {0}", unformattedPhone);
            }
            string carrierCode = GetIntString(row, "FORM");
            if (carriersByCode.ContainsKey(carrierCode))
            {
                data.CarrierId = carriersByCode[carrierCode].Id;
            }
            else
            {
                data.CarrierId = 1;
            }
            string reprTypeCode = GetIntString(row, "PREDST");
            if (reprTypeByCode.ContainsKey(reprTypeCode))
            {
                data.Representative.RepresentativeTypeId = reprTypeByCode[reprTypeCode].Id;
            }
            else
            {
                data.Representative.RepresentativeTypeId = 1;
            }
            string appl = GetIntString(row, "SPOS");
            if (applicationMethodsByCode.ContainsKey(appl))
            {
                data.ApplicationMethodId = applicationMethodsByCode[appl].Id;
            }
            else
            {
                if (data.Representative.RepresentativeTypeId == 1)
                {
                    data.ApplicationMethodId = 1;
                }
                else
                {
                    data.ApplicationMethodId = 2;
                }
            }

            string D_GZK = GetIntString(row, "D_TYPE4");
            if (goznakTypeByCode.ContainsKey(D_GZK))
            {
                data.GOZNAKTypeId = goznakTypeByCode[D_GZK].Id;
            }
            else
            {
                data.GOZNAKTypeId = goznakTypeByCode["0"].Id;
            }

            if (medCentersByCode.ContainsKey(GetIntString(row, "LPU_ID", string.Empty)))
            {
                data.MedicalCentreId = medCentersByCode[GetIntString(row, "LPU_ID")].Id;
            }

            data.LivingAddress.StreetCode = GetIntString(row, "UL");

            ParseClientDataOldData(data, row);
            ParsePolicyDataOldData(data, row);
            return data;
        }
        private void ParseClientData(ClientVisit.SaveData data, DataRow row)
        {
            ReferenceItem category;
            if (categoryByCode.TryGetValue(GetConvertedString(row, "KL"), out category))
            {
                data.NewClientInfo.Category = category.Id;
            }
            else
            {
                data.NewClientInfo.Category = categoryByCode["00"].Id;
            }
            data.NewClientInfo.Lastname = GetConvertedString(row, "FAM");
            ReferenceItem lastnameType;
            if (nameTypesByCode.TryGetValue(GetConvertedString(row, "D_FAM"), out lastnameType))
            {
                data.NewClientInfo.LastnameTypeId = lastnameType.Id;
            }
            else
            {
                data.NewClientInfo.LastnameTypeId = nameTypesByCode["1"].Id;
            }

            data.NewClientInfo.Firstname = GetConvertedString(row, "IM");
            ReferenceItem firstnameType;
            if (nameTypesByCode.TryGetValue(GetConvertedString(row, "D_IM"), out firstnameType))
            {
                data.NewClientInfo.FirstnameTypeId = firstnameType.Id;
            }
            else
            {
                data.NewClientInfo.FirstnameTypeId = nameTypesByCode["1"].Id;
            }

            data.NewClientInfo.Secondname = GetConvertedString(row, "OT");
            ReferenceItem secondnameType;
            if (nameTypesByCode.TryGetValue(GetConvertedString(row, "D_OT"), out secondnameType))
            {
                data.NewClientInfo.SecondnameTypeId = secondnameType.Id;
            }
            else
            {
                data.NewClientInfo.SecondnameTypeId = nameTypesByCode["1"].Id;
            }
            string birthday = GetString(row, "DR");
            data.NewClientInfo.Birthday =
                new DateTime(int.Parse(birthday.Substring(0, 4)), int.Parse(birthday.Substring(4, 2)), int.Parse(birthday.Substring(6, 2)));
            data.NewClientInfo.Sex = GetIntString(row, "W")[0];
            ReferenceItem citizenship;
            string gr = GetConvertedString(row, "GR");
            if (citizenshipByCode.TryGetValue(gr, out citizenship))
            {
                data.NewClientInfo.Citizenship = citizenship.Id;
            }
            else
            {
                data.NewClientInfo.Citizenship = citizenshipByCode["RUS"].Id;
            }
            data.NewClientInfo.Birthplace = GetConvertedString(row, "MR");
            data.NewClientInfo.SNILS = GetConvertedString(row, "SNILS");
        }

        private void ParseClientDataOldData(ClientVisit.SaveData data, DataRow row)
        {
            ReferenceItem category;
            if (categoryByCode.TryGetValue(GetConvertedString(row, "KL"), out category))
            {
                data.NewClientInfo.Category = category.Id;
            }
            else
            {
                data.NewClientInfo.Category = categoryByCode["00"].Id;
            }
            data.NewClientInfo.Lastname = GetConvertedString(row, "FAM");
            ReferenceItem lastnameType;
            if (nameTypesByCode.TryGetValue(GetConvertedString(row, "D_TYPE1"), out lastnameType))
            {
                data.NewClientInfo.LastnameTypeId = lastnameType.Id;
            }
            else
            {
                data.NewClientInfo.LastnameTypeId = nameTypesByCode["1"].Id;
            }

            data.NewClientInfo.Firstname = GetConvertedString(row, "IM");
            ReferenceItem firstnameType;
            if (nameTypesByCode.TryGetValue(GetConvertedString(row, "D_TYPE2"), out firstnameType))
            {
                data.NewClientInfo.FirstnameTypeId = firstnameType.Id;
            }
            else
            {
                data.NewClientInfo.FirstnameTypeId = nameTypesByCode["1"].Id;
            }

            data.NewClientInfo.Secondname = GetConvertedString(row, "OT");
            ReferenceItem secondnameType;
            if (nameTypesByCode.TryGetValue(GetConvertedString(row, "D_TYPE3"), out secondnameType))
            {
                data.NewClientInfo.SecondnameTypeId = secondnameType.Id;
            }
            else
            {
                data.NewClientInfo.SecondnameTypeId = nameTypesByCode["1"].Id;
            }
            string birthday = GetString(row, "DR");
            data.NewClientInfo.Birthday =
                new DateTime(int.Parse(birthday.Substring(0, 4)), int.Parse(birthday.Substring(4, 2)), int.Parse(birthday.Substring(6, 2)));
            data.NewClientInfo.Sex = GetIntString(row, "W")[0];
            ReferenceItem citizenship;
            string gr = GetConvertedString(row, "GR");
            if (citizenshipByCode.TryGetValue(gr, out citizenship))
            {
                data.NewClientInfo.Citizenship = citizenship.Id;
            }
            else
            {
                data.NewClientInfo.Citizenship = citizenshipByCode["RUS"].Id;
            }
            data.NewClientInfo.Birthplace = GetConvertedString(row, "MR");
            data.NewClientInfo.SNILS = GetConvertedString(row, "SS");
        }

        private void ParsePolicyData(ClientVisit.SaveData data, DataRow row)
        {
            data.NewPolicy.UnifiedPolicyNumber = GetConvertedString(row, "ENP");
            string VID_DOCU = GetConvertedString(row, "VID_DOCU");
            if (policyTypeByCode.ContainsKey(VID_DOCU))
            {
                data.NewPolicy.PolicyTypeId = policyTypeByCode[VID_DOCU].Id;
            }
            data.NewPolicy.Series = GetConvertedString(row, "SER_DOCU");
            if (data.NewPolicy.PolicyTypeId == PolicyTypeRef.TemporaryPolicy.Id)
            {
                data.TemporaryPolicyNumber = GetConvertedString(row, "NOM_DOCU");
            }
            data.NewPolicy.Number = GetConvertedString(row, "NOM_DOCU");
            data.NewPolicy.OGRN = GetConvertedString(row, "OGRN");
            data.NewPolicy.OKATO = GetConvertedString(row, "OKATO");
            data.NewPolicy.EndDate = GetDateFromRow(row, "DT");
        }

        private void ParsePolicyDataOldData(ClientVisit.SaveData data, DataRow row)
        {
            data.NewPolicy.UnifiedPolicyNumber = GetConvertedString(row, "ENP");
            string VID_DOCU = GetIntString(row, "P_DOC");
            if (VID_DOCU == "0" || string.IsNullOrEmpty(VID_DOCU)) VID_DOCU = "В";
            if (VID_DOCU == "1") VID_DOCU = "С";
            if (VID_DOCU == "3") VID_DOCU = "П";
            if (policyTypeByCode.ContainsKey(VID_DOCU))
            {
                data.NewPolicy.PolicyTypeId = policyTypeByCode[VID_DOCU].Id;
            }
            data.NewPolicy.Series = GetConvertedString(row, "S_CARD");
            //берем БСО не зависимо от типа полиса
            //if (data.NewPolicy.PolicyTypeId == PolicyTypeRef.TemporaryPolicy.Id)
            {
                data.TemporaryPolicyNumber = GetConvertedString(row, "VS");
            }
            data.NewPolicy.Number = GetLongString(row, "N_CARD") == "0" ? string.Empty : GetLongString(row, "N_CARD");
            data.NewPolicy.OGRN = GetConvertedString(row, "OGRN_OLD");
            data.NewPolicy.OKATO = GetConvertedString(row, "OKATO_OLD");
            data.NewPolicy.EndDate = GetDateFromRow(row, "DT")  >  new DateTime(2099, 12, 31)? new DateTime(2099, 12, 31): GetDateFromRow(row, "DT");
        }

        public void ParseDocumentData(ClientVisit.SaveData data, DataRow row)
        {
            ReferenceItem doctype;
            string c_doc = GetIntString(row, "C_DOC");
            if (doctypeByCode.TryGetValue(c_doc, out doctype))
            {
                data.NewDocument.DocumentTypeId = doctype.Id;
            }
            else
            {
                data.NewDocument.DocumentTypeId = doctypeByCode["14"].Id;
            }
            data.NewDocument.Series = GetConvertedString(row, "S_DOC");
            data.NewDocument.Number = GetConvertedString(row, "N_DOC");
            data.NewDocument.IssueDate = GetDateFromRow(row, "D_DOC");
            data.NewDocument.ExpirationDate = GetDateFromRow(row, "E_DOC");
        }

        public void ParseDocumentDataOldData(ClientVisit.SaveData data, DataRow row)
        {
            ReferenceItem doctype;
            string c_doc = GetIntString(row, "C_DOC");
            if (doctypeByCode.TryGetValue(c_doc, out doctype))
            {
                data.NewDocument.DocumentTypeId = doctype.Id;
            }
            else
            {
                data.NewDocument.DocumentTypeId = doctypeByCode["14"].Id;
            }
            data.NewDocument.Series = GetConvertedString(row, "S_DOC");
            data.NewDocument.Number = GetConvertedString(row, "N_DOC");
            data.NewDocument.IssueDate = GetDateFromRow(row, "D_DOC");
            //data.NewDocument.ExpirationDate = GetDateFromRow(row, "E_DOC");
        }
    }
}
