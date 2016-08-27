using OMInsurance.WebApps.Models;

namespace OMInsurance.WebApps.Validation
{
    public class PolicyInfoValidator : BaseValidator<PolicyInfoClientVisitSaveModel>
    {
        public override void Validate(
           PolicyInfoClientVisitSaveModel model,
           ModelValidationContext context)
        {
            ValidateInternalFields(model, context);
        }

        public override bool IsValid
        {
            get
            {
                return base.IsValid;
            }
        }

        private void ValidateInternalFields(
            PolicyInfoClientVisitSaveModel model,
            ModelValidationContext context)
        {
        }
    }
}