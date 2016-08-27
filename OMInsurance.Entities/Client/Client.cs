using OMInsurance.Entities.Core;
using OMInsurance.Entities.SMS;
using System;
using System.Collections.Generic;

namespace OMInsurance.Entities
{
    public class Client : DataObject
    {
        #region Constructors

        public Client()
        {
            Versions = new List<ClientVersion>();
            Visits = new List<ClientVisitInfo>();
            ListSms = new List<SmsBase>();
        }

        #endregion

        #region Properties

        public List<ClientVersion> Versions { get; set; }
        public List<ClientVisitInfo> Visits { get; set; }
        public ClientVersion ActualVersion { get; set; }
        public List<SmsBase> ListSms { get; set; }

        #endregion

        public class CreateData
        {
        }
	}
}
