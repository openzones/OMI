using OMInsurance.WebApps.Models;

namespace OMInsurance.WebApps.Validation
{
    public class DocumentModelValidator : BaseValidator<DocumentModel>
    {
        public override void Validate(
            DocumentModel document,
            ModelValidationContext context)
        {
            ValidateInternalFields(document, context);
        }

        public override bool IsValid
        {
            get
            {
                return base.IsValid;
            }
        }

        private void ValidateInternalFields(
            DocumentModel document,
            ModelValidationContext context)
        {
        }
    }
}