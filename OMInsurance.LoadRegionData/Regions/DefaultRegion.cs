using System;
using System.Collections.Generic;
using System.Linq;
using NPOI.SS.UserModel;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;

namespace OMInsurance.LoadRegionData
{
    public class DefaultRegion : LoadRegion
    {
        private long RegionId;
        public DefaultRegion(string path, long regionId)
        {
            RegionId = regionId;
            Path = path;
        }

        protected override List<PolicyFromRegion> Process(ISheet table, DataPreLoad dataPreLoad)
        {
            List<PolicyFromRegion> listPolicy = new List<PolicyFromRegion>();

            for (int i = 1; i <= dataPreLoad.CountRow; i++)
            {
                IRow row = table.GetRow(i);
                if (row != null)
                {
                    PolicyFromRegion policy = new PolicyFromRegion();
                    policy.RegionId = this.RegionId;
                    policy.SaveDate = DateTime.Now;

                    ICell[] cells = new ICell[dataPreLoad.CountField];
                    for (int j = 0; j < cells.Count(); j++)
                    {
                        cells[j] = row.GetCell(j);
                    }

                    if (cells[0] != null) policy.TemporaryPolicyNumber =    cells[0].ToString();
                    if (cells[1] != null) policy.UnifiedPolicyNumber =      cells[1].ToString();
                    if (cells[2] != null) policy.PolicySeries =             cells[2].ToString();
                    if (cells[3] != null) policy.PolicyNumber =             cells[3].ToString();
                    if (cells[4] != null) policy.PolicyStatus.Id =          GetLong(cells[4]);
                    if (cells[5] != null) policy.PolicyStatus.Name =        cells[5].ToString();
                    if (cells[6] != null) policy.ClientVisitDate =          GetDateTime(cells[6]);
                    if (cells[7] != null) policy.ApplicationMethod =        cells[7].ToString();
                    if (cells[8] != null) policy.DeliveryCenter.Id =        GetLong(cells[8]);
                    if (cells[9] != null) policy.DeliveryCenter.Code =      cells[9].ToString();
                    if (cells[10] != null) policy.DeliveryCenter.Name =     cells[10].ToString();
                    if (cells[11] != null) policy.Lastname =                cells[11].ToString();
                    if (cells[12] != null) policy.Firstname =               cells[12].ToString();
                    if (cells[13] != null) policy.Secondname =              cells[13].ToString();
                    if (cells[14] != null) policy.Sex =                     GetSex(cells[14].ToString());
                    if (cells[15] != null) policy.Phone =                   GetPhone(cells[15].ToString());
                    if (cells[16] != null) policy.Birthday =                GetDateTime(cells[16]);
                    if (cells[17] != null) policy.Category =                cells[17].ToString();
                    if (cells[18] != null) policy.Citizenship =             cells[18].ToString();
                    if (cells[19] != null) policy.DeliveryPoint.Id =        GetLong(cells[19]);
                    if (cells[20] != null) policy.DeliveryPoint.Code =      cells[20].ToString();
                    if (cells[21] != null) policy.DeliveryPoint.Name =      cells[21].ToString();
                    if (cells[22] != null) policy.IssueDate =               GetDateTime(cells[22]);
                    if (cells[23] != null) policy.StatusDate =              GetDateTime(cells[23]);
                    if (cells[24] != null) policy.NomernikStatus =          cells[24].ToString();
                    if (cells[25] != null) policy.LPU =                     cells[25].ToString();
                    if (cells[26] != null) policy.AttachmentDate =          GetDateTime(cells[26]);
                    if (cells[27] != null) policy.AttachmentMethod =        cells[27].ToString();
                    if (cells[28] != null) policy.BlankNumber =             cells[28].ToString();

                    listPolicy.Add(policy);
                }
                else
                {
                    break;
                }
            }

            return listPolicy;
        }
    }
}
