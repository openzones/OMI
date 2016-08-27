using OMInsurance.Entities;
using System;
using System.Data;

namespace OMInsurance.DataAccess.DAO
{
    internal sealed class SaveClientVisitTableSet
    {
        private readonly DataTable oldClientVersionDataTable;
        public DataTable OldClientVersionDataTable
        {
            get { return oldClientVersionDataTable; }
        }
        private readonly DataTable newClientVersionDataTable;
        public DataTable NewClientVersionDataTable
        {
            get { return newClientVersionDataTable; }
        }

        private readonly DataTable oldDocumentDataTable;
        public DataTable OldDocumentDataTable
        {
            get { return oldDocumentDataTable; }
        }
        private readonly DataTable newDocumentDataTable;
        public DataTable NewDocumentDataTable
        {
            get { return newDocumentDataTable; }
        }
        private readonly DataTable newForeignDocument;
        public DataTable NewForeignDocument
        {
            get { return newForeignDocument; }
        }

        private readonly DataTable oldForeignDocument;
        public DataTable OldForeignDocument
        {
            get { return oldForeignDocument; }
        }

        private readonly DataTable livingAddressDataTable;
        public DataTable LivingAddressDataTable
        {
            get { return livingAddressDataTable; }
        }
        private readonly DataTable registrationAddressDataTable;
        public DataTable RegisterAddressDataTable
        {
            get { return registrationAddressDataTable; }
        }

        private readonly DataTable oldPolicyInfoDataTable;
        public DataTable OldPolicyInfoDataTable
        {
            get { return oldPolicyInfoDataTable; }
        }
        private readonly DataTable newPolicyInfoDataTable;
        public DataTable NewPolicyInfoDataTable
        {
            get { return newPolicyInfoDataTable; }
        }

        private readonly DataTable representativeDataTable;
        public DataTable RepresentativeDataTable
        {
            get { return representativeDataTable; }
        }

        public SaveClientVisitTableSet(ClientVisit.SaveData data, long? clientId)
        {
            oldClientVersionDataTable = GetClientVersionTableDeclaration();
            newClientVersionDataTable = GetClientVersionTableDeclaration();
            oldDocumentDataTable = GetDocumentTableDeclaration();
            newDocumentDataTable = GetDocumentTableDeclaration();
            newForeignDocument = GetDocumentTableDeclaration();
            oldForeignDocument = GetDocumentTableDeclaration();
            livingAddressDataTable = GetAddressTableDeclaration();
            registrationAddressDataTable = GetAddressTableDeclaration();
            oldPolicyInfoDataTable = GetPolicyInfoTableDeclaration();
            newPolicyInfoDataTable = GetPolicyInfoTableDeclaration();
            representativeDataTable = GetRepresentativeTableDeclaration();

            AddRowToClientVersionTable(oldClientVersionDataTable, clientId, data.OldClientInfo);
            AddRowToClientVersionTable(newClientVersionDataTable, clientId, data.NewClientInfo);
            AddRowToDocumentTable(oldDocumentDataTable, data.OldDocument);
            AddRowToDocumentTable(newDocumentDataTable, data.NewDocument);
            AddRowToDocumentTable(newForeignDocument, data.NewForeignDocument);
            AddRowToDocumentTable(oldForeignDocument, data.OldForeignDocument);
            AddRowToAddressTable(livingAddressDataTable, data.LivingAddress);
            AddRowToAddressTable(registrationAddressDataTable, data.RegistrationAddress);
            AddRowToPolicyInfoTable(oldPolicyInfoDataTable, data.OldPolicy);
            AddRowToPolicyInfoTable(newPolicyInfoDataTable, data.NewPolicy);
            AddRowToRepresentativeTable(representativeDataTable, data.Representative);
        }

        private DataTable GetRepresentativeTableDeclaration()
        {
            return new DataTable()
            {
                Columns =
                    {
                        new DataColumn("Id", typeof(long)),
                        new DataColumn("RepresentativeTypeId", typeof(long)),
                        new DataColumn("DocumentTypeId", typeof(long)),
                        new DataColumn("Firstname", typeof(string)),
                        new DataColumn("Secondname", typeof(string)),
                        new DataColumn("Lastname", typeof(string)),
                        new DataColumn("Birthday", typeof(DateTime)),
                        new DataColumn("Series", typeof(string)),
                        new DataColumn("Number", typeof(string)),
                        new DataColumn("IssueDate", typeof(DateTime)),
                        new DataColumn("IssueDepartment", typeof(string))
                    }
            };
        }

        public static DataTable GetPolicyInfoTableDeclaration()
        {
            return new DataTable()
            {
                Columns =
                    {
                        new DataColumn("Id", typeof(long)),
                        new DataColumn("PolicyTypeId", typeof(long)),
                        new DataColumn("Series", typeof(string)),
                        new DataColumn("Number", typeof(string)),
                        new DataColumn("UnifiedPolicyNumber", typeof(string)),
                        new DataColumn("StartDate", typeof(DateTime)),
                        new DataColumn("EndDate", typeof(DateTime)),
                        new DataColumn("OGRN", typeof(string)),
                        new DataColumn("OKATO", typeof(string)),
                        new DataColumn("SmoId", typeof(long))
                    }
            };
        }

        public static DataTable GetAddressTableDeclaration()
        {
            return new DataTable()
            {
                Columns =
                    {
                        new DataColumn("Id", typeof(long)),
                        new DataColumn("TerritoryCode", typeof(string)),
                        new DataColumn("RegionCode", typeof(string)),
                        new DataColumn("Region", typeof(string)),
                        new DataColumn("RegionId", typeof(string)),
                        new DataColumn("Area", typeof(string)),
                        new DataColumn("AreaId", typeof(string)),
                        new DataColumn("City", typeof(string)),
                        new DataColumn("CityId", typeof(string)),
                        new DataColumn("Locality", typeof(string)),
                        new DataColumn("LocalityId", typeof(string)),
                        new DataColumn("Street", typeof(string)),
                        new DataColumn("House", typeof(string)),
                        new DataColumn("Housing", typeof(string)),
                        new DataColumn("Building", typeof(string)),
                        new DataColumn("Appartment", typeof(string)),
                        new DataColumn("StreetCode", typeof(string)),
                        new DataColumn("PostIndex", typeof(string)),
                        new DataColumn("FullAddressString", typeof(string))
                    }
            };
        }

        public static DataTable GetDocumentTableDeclaration()
        {
            return new DataTable()
            {
                Columns =
                {
                    new DataColumn("Id", typeof(long)),
                    new DataColumn("DocumentTypeID", typeof(long)),
                    new DataColumn("Series", typeof(string)),
                    new DataColumn("Number", typeof(string)),
                    new DataColumn("IssueDate", typeof(DateTime)),
                    new DataColumn("ExpirationDate", typeof(DateTime)),
                    new DataColumn("IsIssueCase", typeof(bool)),
                    new DataColumn("IssueDepartment", typeof(string))
                }
            };
        }

        public static DataTable GetClientVersionTableDeclaration()
        {
            return new DataTable()
            {
                Columns =
                {
                    new DataColumn("Id", typeof(long)),
                    new DataColumn("Firstname", typeof(string)),
                    new DataColumn("Secondname", typeof(string)),
                    new DataColumn("Lastname", typeof(string)),
                    new DataColumn("FirstnameType", typeof(long)),
                    new DataColumn("SecondnameType", typeof(long)),
                    new DataColumn("LastnameType", typeof(long)),
                    new DataColumn("Birthday", typeof(DateTime)),
                    new DataColumn("Sex", typeof(char)),
                    new DataColumn("SNILS", typeof(string)),
                    new DataColumn("Citizenship", typeof(long)),
                    new DataColumn("Birthplace", typeof(string)),
                    new DataColumn("Category", typeof(long)),
                    new DataColumn("Document", typeof(long))
                }
            };
        }

        private void AddRowToRepresentativeTable(DataTable table, Representative.SaveData saveData)
        {
            table.Rows.Add
                (saveData.Id,
                saveData.RepresentativeTypeId,
                saveData.DocumentTypeId,
                saveData.Firstname,
                saveData.Secondname,
                saveData.Lastname,
                saveData.Birthday,
                saveData.Series,
                saveData.Number,
                saveData.IssueDate,
                saveData.IssueDepartment);
        }

        private static void AddClientVersion(
            DataTable table,
            long? id,
            string firstname,
            string secondname,
            string lastname,
            long? firstnameType,
            long? secondnameType,
            long? lastnameType,
            DateTime? birthday,
            char? sex,
            string snils,
            long? citizenship,
            string birthplace,
            long? category)
        {
            table.Rows.Add(id,
            firstname, secondname, lastname, firstnameType, secondnameType, lastnameType, birthday,
            sex, snils, citizenship, birthplace, category);
        }

        public static void AddRowToClientVersionTable(DataTable table, long? clientId, ClientVersion.SaveData data)
        {
            AddClientVersion(table, data.Id, data.Firstname,
            data.Secondname, data.Lastname, data.FirstnameTypeId, data.SecondnameTypeId, data.LastnameTypeId, data.Birthday,
            data.Sex, data.SNILS, data.Citizenship, data.Birthplace, data.Category);
        }

        public static void AddRowToDocumentTable(DataTable table, Document.SaveData data)
        {
            table.Rows.Add(
                data.Id,
                data.DocumentTypeId,
                data.Series,
                data.Number,
                data.IssueDate,
                data.ExpirationDate,
                data.IsIssueCase,
                data.IssueDepartment);
        }

        public static void AddRowToAddressTable(DataTable table, Address.SaveData data)
        {
            table.Rows.Add(
                data.Id,
                data.TerritoryCode,
                data.RegionCode,
                data.Region,
                data.RegionId,
                data.Area,
                data.AreaId,
                data.City,
                data.CityId,
                data.Locality,
                data.LocalityId,
                data.Street,
                data.House,
                data.Housing,
                data.Building,
                data.Appartment,
                data.StreetCode,
                data.PostIndex,
                data.FullAddressString);
        }

        public static void AddRowToPolicyInfoTable(DataTable table, PolicyInfo.SaveData data)
        {
            table.Rows.Add(
                data.Id,
                data.PolicyTypeId,
                data.Series,
                data.Number,
                data.UnifiedPolicyNumber,
                data.StartDate,
                data.EndDate,
                data.OGRN,
                data.OKATO,
                data.SmoId);
        }
    }
}
