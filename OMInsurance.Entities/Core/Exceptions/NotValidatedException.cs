using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.Entities.Core.Exeptions
{
    public class NotValidatedException : ApplicationException
    {
        public NotValidatedException()
        {
        }

        public NotValidatedException(string message)
            : base(message)
        {
        }

        public NotValidatedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
