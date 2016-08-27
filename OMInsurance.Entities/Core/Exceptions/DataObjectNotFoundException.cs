using System;
using System.Runtime.Serialization;

namespace OMInsurance.Entities.Core.Exeptions
{
    [DataContract]
    public class DataObjectNotFoundException : ApplicationException
    {
        public DataObjectNotFoundException()
        {
        }

        public DataObjectNotFoundException(string message)
            : base(message)
        {
        }

        public DataObjectNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
