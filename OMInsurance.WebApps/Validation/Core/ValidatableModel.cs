using System.Collections.Generic;

namespace OMInsurance.WebApps.Validation
{
    public abstract class ValidatableModel<T> where T : ValidatableModel<T>
    {
        protected BaseValidator<T> validator;

        public List<string> Messages
        {
            get
            {
                return validator.Messages;
            }
        }

        public List<string> MessagesNotCritical
        {
            get
            {
                return validator.MessagesNotCritical;
            }
            set
            {
                validator.MessagesNotCritical = value;
            }
        }

        public virtual bool IsValid()
        {
            return validator.IsValid;
        }

        public virtual bool IsValidNotCritical()
        {
            return validator.IsValidNotCritical;
        }

        public virtual void Validate(ModelValidationContext context)
        {
            validator.Validate((T)this, context);
        }
    }
}