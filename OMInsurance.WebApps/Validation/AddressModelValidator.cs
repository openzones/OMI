using OMInsurance.WebApps.Models;

namespace OMInsurance.WebApps.Validation
{
    public class AddressModelValidator : BaseValidator<AddressModel>
    {
        public override void Validate(
           AddressModel address,
           ModelValidationContext context)
        {
            ValidateInternalFields(address, context);
        }

        public override bool IsValid
        {
            get
            {
                return base.IsValid;
            }
        }

        private void ValidateInternalFields(
            AddressModel address,
            ModelValidationContext context)
        {
            if (address.AddressType == Models.Core.AddressType.Living)
            {
                if (string.IsNullOrEmpty(address.City)
                    && string.IsNullOrEmpty(address.Region)
                    && string.IsNullOrEmpty(address.Area)
                    && string.IsNullOrEmpty(address.Locality) 
                    && string.IsNullOrEmpty(address.Street))
                {
                    Messages.Add("Хотя бы одно поле не должно быть пустым (Город, Район, Регион, Населеный пункт, Улица)");
                }
            }
        }
    }
}