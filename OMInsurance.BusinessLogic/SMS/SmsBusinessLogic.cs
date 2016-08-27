using OMInsurance.DataAccess.DAO;
using OMInsurance.Entities.SMS;
using OMInsurance.Interfaces;
using System.Collections.Generic;

namespace OMInsurance.BusinessLogic
{
    public class SmsBusinessLogic : ISmsBusinessLogic
    {

        public List<SmsBase> SMSBase_GetAll(SmsBase.SmsBaseGet smsGet)
        {
            return SmsMessageDao.Instance.SMSBase_GetAll(smsGet);
        }

        public SmsTemplate SmsTemplate_Get()
        {
            return SmsMessageDao.Instance.SmsTemplate_Get();
        }

        public void SmsTemplate_Set(SmsTemplate smsTemplate)
        {
            SmsMessageDao.Instance.SmsTemplate_Set(smsTemplate);
        }


        public void SMSBaseSet(SmsBase.SmsBaseSet data)
        {
            SmsMessageDao.Instance.SMSBaseSet(data);
        }

        /// <summary>
        /// Retuns list of messages that should be sent
        /// </summary>
        /// <returns>List of SmsMessages</returns>
        public List<SMSMessage> GetList(long? StatusIdInside = null)
        {
            return SmsMessageDao.Instance.GetList(StatusIdInside);
        }

        /// <summary>
        /// Sets messages results to spacified messages
        /// </summary>
        public void SetMessageResult(IEnumerable<SmsResult> results)
        {
            SmsMessageDao.Instance.SetMessageResult(results);
        }

        /// <summary>
        /// Sets message result to spacified message
        /// </summary>
        public void SetMessageResult(SmsResult result)
        {
            SmsMessageDao.Instance.SetMessageResult(result);
        }
    }
}
