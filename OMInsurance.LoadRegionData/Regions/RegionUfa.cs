using System;
using System.Collections.Generic;
using System.Linq;
using NPOI.SS.UserModel;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;

namespace OMInsurance.LoadRegionData
{
    public class RegionUfa : LoadRegion
    {
        private long RegionId = (long)ListRegionId.Ufa;

        public RegionUfa(string path)
        {
            Path = path;
        }

        protected override List<PolicyFromRegion> Process(ISheet table, DataPreLoad dataPreLoad)
        {
            List<PolicyFromRegion> listPolicy = new List<PolicyFromRegion>();

            for (int i = 1; i <= dataPreLoad.CountRow ; i++)
            {
                IRow row = table.GetRow(i);
                if(row != null)
                {
                    PolicyFromRegion policy = new PolicyFromRegion();
                    policy.RegionId = this.RegionId;
                    policy.SaveDate = DateTime.Now;

                    ICell[] cells = new ICell[dataPreLoad.CountField];
                    for(int j = 0; j < cells.Count(); j++)
                    {
                        cells[j] = row.GetCell(j);
                    }

                    if (cells[0] != null) policy.TemporaryPolicyNumber = cells[0].ToString();
                    if (cells[1] != null) policy.UnifiedPolicyNumber = cells[1].ToString();
                    //if (cells[2] != null) policy.UnifiedPolicyNumber = cells[2].StringCellValue;
                    if (cells[3] != null) policy.PolicyStatus.Id = GetLong(cells[3]);
                    if (cells[4] != null) policy.PolicyStatus.Name = cells[4].ToString();
                    if (cells[5] != null) policy.ClientVisitDate =  GetDateTime(cells[5]);
                    if (cells[6] != null) policy.ApplicationMethod = cells[6].ToString();
                    if (cells[7] != null) policy.DeliveryCenter.Name = cells[7].ToString();
                    if (cells[8] != null) policy.DeliveryCenter.Code = cells[8].ToString();
                    if (cells[9] != null) policy.Lastname = cells[9].ToString();
                    if (cells[10] != null) policy.Firstname = cells[10].ToString();
                    if (cells[11] != null) policy.Secondname = cells[11].ToString();
                    if (cells[12] != null) policy.Sex = GetSex(cells[12].ToString());
                    if (cells[13] != null) policy.Birthday = GetDateTime(cells[13]);
                    if (cells[14] != null) policy.Category = cells[14].ToString();
                    if (cells[15] != null) policy.Citizenship = cells[15].ToString();
                    if (cells[16] != null) policy.IssueDate = GetDateTime(cells[16]);
                    if (cells[17] != null) policy.Phone = GetPhone(cells[17].ToString());
                    if (cells[18] != null) policy.BlankNumber = cells[18].ToString();

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
