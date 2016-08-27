using OMInsurance.BusinessLogic;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.Tests.Generation
{
    public static class DocumentGenerator
    {
        public static Document.SaveData GetDocumentSaveData(long? id)
        {
            Document.SaveData data = new Document.SaveData();
            data.Id = id;
            data.IsIssueCase = true;
            data.IssueDate = DateTime.Now.AddYears(-3);
            data.ExpirationDate = DateTime.Now.AddYears(1);
            data.IssueDepartment = "УФМС";
            data.Series = "1234";
            data.Number = "121234";
            ReferenceBusinessLogic bll = new ReferenceBusinessLogic();
            //Паспорт гражданина РФ
            data.DocumentTypeId = bll.GetReferencesList(Constants.DocumentTypeRef).Where(a=>a.Code == "14").FirstOrDefault().Id;
            return data;
        }
    }
}
