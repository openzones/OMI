using OMInsurance.Entities.Core;
using OMInsurance.WebApps.Models;
using System;
using System.Linq;
using OMInsurance.Entities;

namespace OMInsurance.WebApps.Validation
{
    public class BSOSearchCriteriaModelValidator : BaseValidator<BSOSearchCriteriaModel>
    {
        public BSOSearchCriteriaModelValidator()
        {
        }

        public override void Validate(
           BSOSearchCriteriaModel bsoSearchCriteriaModel,
           ModelValidationContext context)
        {
            ValidateInternalFields(bsoSearchCriteriaModel, context);
        }

        public override bool IsValid
        {
            get
            {
                return base.IsValid && isValid;
            }
        }

        bool isValid;

        private void ValidateInternalFields(
               BSOSearchCriteriaModel bsoSearchCriteriaModel,
            ModelValidationContext context)
        {
            isValid = true;
            ValidateChangeStatusBSO(bsoSearchCriteriaModel, context);
            ValidateChangeOther(bsoSearchCriteriaModel);
            ValidateChangeStatusOnDelivery(bsoSearchCriteriaModel);
        }

        private void ValidateChangeStatusBSO(BSOSearchCriteriaModel bsoSearchCriteriaModel, ModelValidationContext context)
        {
            string Message = BSOStatusValidator.Validator((long)bsoSearchCriteriaModel.StatusId, (long)bsoSearchCriteriaModel.CurrentStatusId, context.currenUser);
            if (!string.IsNullOrEmpty(Message)) { isValid = false; this.Messages.Add(Message); }
        }

        private void ValidateChangeOther(BSOSearchCriteriaModel bsoSearchCriteriaModel)
        {
            if (bsoSearchCriteriaModel.PolicyPartyNumber == bsoSearchCriteriaModel.PolicyPartyNumber &&
                //bsoSearchCriteriaModel.DeliveryCenterId == bsoSearchCriteriaModel.DeliveryCenterId &&
                bsoSearchCriteriaModel.CurrentStatusId == bsoSearchCriteriaModel.StatusId &&
                bsoSearchCriteriaModel.NewDeliveryPointId == (bsoSearchCriteriaModel.DeliveryPointIds.Count == 1 ? bsoSearchCriteriaModel.DeliveryPointIds.FirstOrDefault() : 0) &&
                bsoSearchCriteriaModel.NewResponsibleID == bsoSearchCriteriaModel.ResponsibleID)
            {
                isValid = false;
                this.Messages.Add("Не было изменений по существу.");
            }

            if (bsoSearchCriteriaModel.CurrentStatusId == (long)ListBSOStatusID.OnResponsible && bsoSearchCriteriaModel.NewResponsibleID == null)
            {
                isValid = false;
                this.Messages.Add("При статусе [На ответственном] необходимо выбрать ответственного.");
            }

            if (bsoSearchCriteriaModel.CurrentStatusId == (long)ListBSOStatusID.FailOnResponsible && bsoSearchCriteriaModel.NewResponsibleID == null)
            {
                isValid = false;
                this.Messages.Add("При статусе [Испорчен, на ответственном] необходимо выбрать ответственного.");
            }
        }

        private void ValidateChangeStatusOnDelivery (BSOSearchCriteriaModel obj)
        {
            if(obj.CurrentStatusId == (long)ListBSOStatusID.OnDelivery  && obj.NewDeliveryPointId == null)
            {
                isValid = false;
                this.Messages.Add("Вы поставили статус [На точке], но не указали точку выдачи.");
            }
        }
    }
}