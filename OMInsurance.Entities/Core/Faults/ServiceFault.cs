using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.Entities.Core.Faults
{
    [KnownType(typeof(DataObjectNotFoundFault))]
    [DataContract(Name = "ServiceFault")]
    public abstract class ServiceFault
    {
        /// <summary>
        /// Message that describes the server error.
        /// </summary>
        [DataMember(Name = "ErrorMessage", IsRequired = false, EmitDefaultValue = false)]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Stack trace of the error on the server side.
        /// </summary>
        [DataMember(Name = "ServerStackTrace", IsRequired = false, EmitDefaultValue = false)]
        public string ServerStackTrace { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        protected ServiceFault()
        {
        }

        /// <summary>
        /// Fills fault with properties from specified exception.
        /// </summary>
        /// <param name="exception">Exception that should be used to fill the fault.</param>
        public void ReadErrorInfo(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }

            this.ErrorMessage = exception.Message;
            this.ServerStackTrace = exception.StackTrace;
        }
    }
}
