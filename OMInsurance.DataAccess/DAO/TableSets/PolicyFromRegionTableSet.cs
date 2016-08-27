using OMInsurance.Entities;
using System;
using System.Collections.Generic;
using System.Data;


namespace OMInsurance.DataAccess.DAO
{
    public class PolicyFromRegionTableSet
    {
        private readonly DataTable policyFromRegionTable;

        public DataTable PolicyFromRegionTable
        {
            get { return policyFromRegionTable; }
        }

        public PolicyFromRegionTableSet(IEnumerable<PolicyFromRegion> listPolicy)
        {
            policyFromRegionTable = new DataTable()
            {
                Columns =
                {
                    new DataColumn("RegionId", typeof(long)),
                    new DataColumn("TemporaryPolicyNumber", typeof(string)),
                    new DataColumn("UnifiedPolicyNumber", typeof(string)),
                    new DataColumn("PolicySeries", typeof(string)),
                    new DataColumn("PolicyNumber", typeof(string)),
                    new DataColumn("StatusId", typeof(long)),
                    new DataColumn("StatusName", typeof(string)),
                    new DataColumn("ClientVisitDate", typeof(DateTime)),
                    new DataColumn("ApplicationMethod", typeof(string)),
                    new DataColumn("DeliveryCenterId", typeof(long)),
                    new DataColumn("DeliveryCenterCode", typeof(string)),
                    new DataColumn("DeliveryCenterName", typeof(string)),
                    new DataColumn("Lastname", typeof(string)),
                    new DataColumn("Firstname", typeof(string)),
                    new DataColumn("Secondname", typeof(string)),
                    new DataColumn("Sex char(1)", typeof(char)),
                    new DataColumn("Phone", typeof(string)),
                    new DataColumn("Birthday", typeof(DateTime)),
                    new DataColumn("Category", typeof(string)),
                    new DataColumn("Citizenship", typeof(string)),
                    new DataColumn("DeliveryPointId", typeof(long)),
                    new DataColumn("DeliveryPointCode", typeof(string)),
                    new DataColumn("DeliveryPointName", typeof(string)),
                    new DataColumn("IssueDate", typeof(DateTime)),
                    new DataColumn("StatusDate", typeof(DateTime)),
                    new DataColumn("NomernikStatus", typeof(string)),
                    new DataColumn("LPU", typeof(string)),
                    new DataColumn("AttachmentDate", typeof(DateTime)),
                    new DataColumn("AttachmentMethod", typeof(string)),
                    new DataColumn("BlankNumber", typeof(string)),
                    new DataColumn("SaveDate", typeof(DateTime))
                }
            };
            FillTable(listPolicy);
        }

        private void FillTable(IEnumerable<PolicyFromRegion> listPolicy)
        {
            foreach (PolicyFromRegion item in listPolicy)
            {
                policyFromRegionTable.Rows.Add(
                    item.RegionId,
                    item.TemporaryPolicyNumber,
                    item.UnifiedPolicyNumber,
                    item.PolicySeries,
                    item.PolicyNumber,
                    item.PolicyStatus.Id,
                    item.PolicyStatus.Name,
                    item.ClientVisitDate,
                    item.ApplicationMethod,
                    item.DeliveryCenter.Id,
                    item.DeliveryCenter.Code,
                    item.DeliveryCenter.Name,
                    item.Lastname,
                    item.Firstname,
                    item.Secondname,
                    item.Sex,
                    item.Phone,
                    item.Birthday,
                    item.Category,
                    item.Citizenship,
                    item.DeliveryPoint.Id,
                    item.DeliveryPoint.Code,
                    item.DeliveryPoint.Name,
                    item.IssueDate,
                    item.StatusDate,
                    item.NomernikStatus,
                    item.LPU,
                    item.AttachmentDate,
                    item.AttachmentMethod,
                    item.BlankNumber,
                    item.SaveDate
                    );
            }
        }
    }
}
