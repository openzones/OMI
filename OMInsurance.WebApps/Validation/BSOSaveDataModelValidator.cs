using OMInsurance.Entities.Core;
using OMInsurance.WebApps.Models;
using System;
using OMInsurance.Entities;

namespace OMInsurance.WebApps.Validation
{
    public class BSOSaveDataModelValidator : BaseValidator<BSOSaveDataModel>
    {

        public BSOSaveDataModelValidator()
        {
        }

        public override void Validate(
            BSOSaveDataModel bsoSaveDataModel,
            ModelValidationContext context)
        {
            ValidateInternalFields(bsoSaveDataModel, context);
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
            BSOSaveDataModel bsoSaveDataModel,
            ModelValidationContext context)
        {
            isValid = true;
            BusinessLogic.BSOBusinessLogic bsoLogic = new BusinessLogic.BSOBusinessLogic();
            BSO bso = bsoLogic.BSO_GetByNumber(bsoSaveDataModel.TemporaryPolicyNumber);
            ValidateChangeStatusBSO(bsoSaveDataModel, bso, context);
            ValidateChangeOther(bsoSaveDataModel, bso);
            ValidateChangeVisitGroupId(bsoSaveDataModel, bso);

        }

        private void ValidateChangeStatusBSO(BSOSaveDataModel bsoSaveDataModel, BSO bso, ModelValidationContext context)
        {
            string Message = BSOStatusValidator.Validator(bso.Status.Id, (long)bsoSaveDataModel.StatusId, context.currenUser);
            if (!string.IsNullOrEmpty(Message)) { isValid = false; this.Messages.Add(Message); }
        }

        private void ValidateChangeOther(BSOSaveDataModel bsoSaveDataModel, BSO bso)
        {
            if (bso.PolicyPartyNumber == bsoSaveDataModel.PolicyPartyNumber &&
                bso.DeliveryPointId == bsoSaveDataModel.DeliveryPointId &&
                bso.Status.Id == bsoSaveDataModel.StatusId &&
                bso.ResponsibleID == bsoSaveDataModel.ResponsibleID &&
                bso.VisitGroupId == bsoSaveDataModel.VisitGroupId &&
                bso.StatusDate == bsoSaveDataModel.StatusDate)
            {
                isValid = false;
                this.Messages.Add("Не было изменений по существу.");
            }

            if (bsoSaveDataModel.StatusId == (long)ListBSOStatusID.OnResponsible && bsoSaveDataModel.ResponsibleID == null)
            {
                isValid = false;
                this.Messages.Add("При статусе [На ответственном] необходимо выбрать ответственного.");
            }

            if (bsoSaveDataModel.StatusId == (long)ListBSOStatusID.FailOnResponsible && bsoSaveDataModel.ResponsibleID == null)
            {
                isValid = false;
                this.Messages.Add("При статусе [Испорчен, на ответственном] необходимо выбрать ответственного.");
            }

            if (bsoSaveDataModel.StatusId == (long)ListBSOStatusID.OnDelivery && bsoSaveDataModel.DeliveryPointId == null)
            {
                isValid = false;
                this.Messages.Add("Вы поставили статус [На точке], но не указали точку выдачи.");
            }
        }

        private void ValidateChangeVisitGroupId(BSOSaveDataModel bsoSaveDataModel, BSO bso)
        {
            if (bsoSaveDataModel.VisitGroupId != bso.VisitGroupId && bsoSaveDataModel.VisitGroupId != null)
            {
                BusinessLogic.ClientBusinessLogic clientBusinessLogic = new BusinessLogic.ClientBusinessLogic();
                try
                {
                    ClientVisit cv = clientBusinessLogic.ClientVisit_GetFirstClientVisitInGroup((long)bsoSaveDataModel.VisitGroupId);
                }
                catch
                {
                    isValid = false;
                    this.Messages.Add(string.Format("Вы ввели несуществующий идентификатор обращения {0}!", bsoSaveDataModel.VisitGroupId));
                }
            }
        }
    }
}