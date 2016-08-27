using OMInsurance.Entities.Core.Exeptions;
using System.Collections.Generic;

namespace OMInsurance.WebApps.Validation
{
    public abstract class BaseValidator<T>
    {
        public BaseValidator()
        {
            Messages = new List<string>();
            MessagesNotCritical = new List<string>();
        }
        
        public List<string> Messages { get; set; }
        public List<string> MessagesNotCritical { get; set; }

        public virtual bool IsValid
        {
            get 
            {
                return Messages.Count == 0;
            }
        }

        public virtual bool IsValidNotCritical
        {
            get
            {
                return MessagesNotCritical.Count == 0;
            }
        }

        public abstract void Validate(T entity, ModelValidationContext context);
    }
}