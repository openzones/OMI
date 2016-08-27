using OMInsurance.Entities;
using System;
using System.Collections.Generic;
namespace OMInsurance.Interfaces
{
    public interface IDbfProcessingBusinessLogic
    {
        List<ClientVisit.UpdateResultData> UploadMFCClientVisits(User user, string zipFilepath);
        List<ClientVisit.UpdateResultData> UploadMFCClientVisitsExtradition(User user, string zipFilepath);
        List<ClientVisit.UpdateResultData> UploadMFCClientVisitsOldData(User user, string zipFilepath);
    }
}
