using OMInsurance.WebApps.Models;

namespace OMInsurance.WebApps.Validation
{
    public class RepresentativeEditModelValidator : BaseValidator<RepresentativeEditModel>
    {
        public override void Validate(RepresentativeEditModel entity, ModelValidationContext context)
        {
        }

        public override bool IsValid
        {
            get
            {
                return base.IsValid;
            }
        }

        private void ValidateInternalFields(
            RepresentativeEditModel entity,
            ModelValidationContext context)
        {
        }
    }
}