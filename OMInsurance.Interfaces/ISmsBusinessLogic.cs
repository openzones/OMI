using OMInsurance.Entities.SMS;
using System.Collections.Generic;

namespace OMInsurance.Interfaces
{
    public interface ISmsBusinessLogic
    {
        /// <summary>
        /// получаем всю таблицу SMSBase по заданным параметрам
        /// </summary>
        /// <param name="smsGet"></param>
        /// <returns></returns>
        List<SmsBase> SMSBase_GetAll(SmsBase.SmsBaseGet smsGet);

        /// <summary>
        /// получаем шаблон смс
        /// </summary>
        /// <returns></returns>
        SmsTemplate SmsTemplate_Get();

        /// <summary>
        /// устанавливаем шаблон смс
        /// </summary>
        /// <param name="smsTemplate"></param>
        void SmsTemplate_Set(SmsTemplate smsTemplate);

        void SMSBaseSet(SmsBase.SmsBaseSet data);

        /// <summary>
        /// Retuns list of messages that should be sent
        /// </summary>
        /// <returns>List of SmsMessages</returns>
        List<SMSMessage> GetList(long? StatusIdInside = null);

        /// <summary>
        /// Sets messages results to spacified messages
        /// </summary>
        void SetMessageResult(IEnumerable<SmsResult> results);

        /// <summary>
        /// Sets message results to spacified message
        /// </summary>
        void SetMessageResult(SmsResult result);
    }
}
