using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.WebApps.Models;
using OMInsurance.WebApps.Security;
using OMInsurance.Interfaces;
using OMInsurance.BusinessLogic;

namespace OMInsurance.WebApps.Controllers
{
    [AuthorizeUser]
    [AuthorizeUser(Roles = "Administrator")]
    public class ReferenceController : OMInsuranceController
    {
        IReferenceBusinessLogic referenceBusinessLogic;

        public ReferenceController()
        {
            referenceBusinessLogic = new ReferenceBusinessLogic();
        }

        public ActionResult Index()
        {
            ReferenceChoiceModel model = new ReferenceChoiceModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ReferenceChoiceModel model)
        {
            ReferenceUniversalItemModel.ReferenceDisplayName = model.ReferencesDisplayName.Where(a => a.Value == model.ReferenceId.ToString()).Select(b => b.Text).FirstOrDefault();
            model.ReferenceName = model.References.Where(a => a.Value == model.ReferenceId.ToString()).Select(b => b.Text).FirstOrDefault();
            return RedirectToAction(model.ReferenceName);
        }

        //Delete
        public ActionResult Delete(long? id, string refname)
        {
            if (id != null && !string.IsNullOrEmpty(refname))
            {
                referenceBusinessLogic.DeleteReferenceItem((long)id, refname);
                return RedirectToAction(refname);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


        /// <summary>
        ///  ClientPretensionExpert
        /// </summary>
        /// <returns></returns>
        public ActionResult ClientPretensionExpert()
        {
            List<ReferenceUniversalItem> reference = referenceBusinessLogic.GetUniversalList(Constants.ClientPretensionExpert);
            ReferenceUniversalItemModel.FillListReferenceUniversal(reference);
            return View(ReferenceUniversalItemModel.ListReferenceUniversalItem);
        }
        [HttpGet]
        public ActionResult ClientPretensionExpertEdit(long? id)
        {
            if (id == null)
            {
                ReferenceUniversalItemModel refItem = new ReferenceUniversalItemModel();
                refItem.ReferenceName = Constants.ClientPretensionExpert;
                return View(refItem);
            }
            else
            {
                return View(ReferenceUniversalItemModel.ListReferenceUniversalItem.Find(a => a.Id == id));
            }
        }
        [HttpPost]
        public ActionResult ClientPretensionExpertEdit(long? id, ReferenceUniversalItem model)
        {
            if (ReferenceUniversalItemModel.ListReferenceUniversalItem.Where(a => a.Id == id).Count() > 0)
            {
                //Update
                referenceBusinessLogic.SaveUniversalReferenceItem(model, model.ReferenceName, false);
            }
            else
            {
                //Insert
                referenceBusinessLogic.SaveUniversalReferenceItem(model, model.ReferenceName, true);
            }
            return RedirectToAction(model.ReferenceName);
        }

        /// <summary>
        ///  Defect
        /// </summary>
        /// <returns></returns>
        public ActionResult Defect()
        {
            List<ReferenceUniversalItem> reference = referenceBusinessLogic.GetUniversalList(Constants.Defect);
            ReferenceUniversalItemModel.FillListReferenceUniversal(reference);
            return View(ReferenceUniversalItemModel.ListReferenceUniversalItem);
        }
        [HttpGet]
        public ActionResult DefectEdit(long? id)
        {
            if (id == null)
            {
                ReferenceUniversalItemModel refItem = new ReferenceUniversalItemModel();
                refItem.ReferenceName = Constants.Defect;
                return View(refItem);
            }
            else
            {
                return View(ReferenceUniversalItemModel.ListReferenceUniversalItem.Find(a => a.Id == id));
            }
        }
        [HttpPost]
        public ActionResult DefectEdit(long? id, ReferenceUniversalItem model)
        {
            if (ReferenceUniversalItemModel.ListReferenceUniversalItem.Where(a => a.Id == id).Count() > 0)
            {
                //Update
                referenceBusinessLogic.SaveUniversalReferenceItem(model, model.ReferenceName, false);
            }
            else
            {
                //Insert
                referenceBusinessLogic.SaveUniversalReferenceItem(model, model.ReferenceName, true);
            }
            return RedirectToAction(model.ReferenceName);
        }


        /// <summary>
        ///  DeliveryCenterRef
        /// </summary>
        /// <returns></returns>
        public ActionResult DeliveryCenterRef()
        {
            List<ReferenceUniversalItem> reference = referenceBusinessLogic.GetUniversalList(Constants.DeliveryCenterRef);
            ReferenceUniversalItemModel.FillListReferenceUniversal(reference);
            return View(ReferenceUniversalItemModel.ListReferenceUniversalItem);
        }
        [HttpGet]
        public ActionResult DeliveryCenterRefEdit(long? id)
        {
            if (id == null)
            {
                ReferenceUniversalItemModel refItem = new ReferenceUniversalItemModel();
                refItem.ReferenceName = Constants.DeliveryCenterRef;
                return View(refItem);
            }
            else
            {
                return View(ReferenceUniversalItemModel.ListReferenceUniversalItem.Find(a => a.Id == id));
            }
        }
        [HttpPost]
        public ActionResult DeliveryCenterRefEdit(long? id, ReferenceUniversalItem model)
        {
            if (ReferenceUniversalItemModel.ListReferenceUniversalItem.Where(a => a.Id == id).Count() > 0)
            {
                //Update
                referenceBusinessLogic.SaveUniversalReferenceItem(model, model.ReferenceName, false);
            }
            else
            {
                //Insert
                referenceBusinessLogic.SaveUniversalReferenceItem(model, model.ReferenceName, true);
            }
            return RedirectToAction(model.ReferenceName);
        }

        /// <summary>
        /// DeliveryPointRef
        /// </summary>
        /// <returns></returns>
        public ActionResult DeliveryPointRef()
        {
            List<ReferenceUniversalItem> reference = referenceBusinessLogic.GetUniversalList(Constants.DeliveryPointRef);
            ReferenceUniversalItemModel.FillListReferenceUniversal(reference);
            return View(ReferenceUniversalItemModel.ListReferenceUniversalItem);
        }
        [HttpGet]
        public ActionResult DeliveryPointRefEdit(long? id)
        {
            if (id == null)
            {
                ReferenceUniversalItemModel refItem = new ReferenceUniversalItemModel();
                refItem.FillReferenceForView();
                refItem.ReferenceName = Constants.DeliveryPointRef;
                return View(refItem);
            }
            else
            {
                ReferenceUniversalItemModel refItem = ReferenceUniversalItemModel.ListReferenceUniversalItem.Find(a => a.Id == id);
                refItem.FillReferenceForView();
                return View(refItem);
            }
        }
        [HttpPost]
        public ActionResult DeliveryPointRefEdit(long? id, ReferenceUniversalItem model)
        {
            if (ReferenceUniversalItemModel.ListReferenceUniversalItem.Where(a => a.Id == id).Count() > 0)
            {
                //Update
                referenceBusinessLogic.SaveUniversalReferenceItem(model, model.ReferenceName, false);
            }
            else
            {
                //Insert
                model.Id = (long)(ReferenceUniversalItemModel.ListReferenceUniversalItem.Max(a => a.Id)) + 1;
                referenceBusinessLogic.SaveUniversalReferenceItem(model, model.ReferenceName, true);
            }
            return RedirectToAction(model.ReferenceName);
        }

        /// <summary>
        /// ReferenceRef
        /// </summary>
        /// <returns></returns>
        public ActionResult ReferenceRef()
        {
            List<ReferenceUniversalItem> reference = referenceBusinessLogic.GetUniversalList(Constants.ReferenceRef);
            ReferenceUniversalItemModel.FillListReferenceUniversal(reference);
            return View(ReferenceUniversalItemModel.ListReferenceUniversalItem);
        }
        [HttpGet]
        public ActionResult ReferenceRefEdit(long? id)
        {
            if (id == null)
            {
                ReferenceUniversalItemModel refItem = new ReferenceUniversalItemModel();
                refItem.ReferenceName = Constants.ReferenceRef;
                return View(refItem);
            }
            else
            {
                return View(ReferenceUniversalItemModel.ListReferenceUniversalItem.Find(a => a.Id == id));
            }
        }
        [HttpPost]
        public ActionResult ReferenceRefEdit(long? id, ReferenceUniversalItem model)
        {
            if (ReferenceUniversalItemModel.ListReferenceUniversalItem.Where(a => a.Id == id).Count() > 0)
            {
                //Update
                referenceBusinessLogic.SaveUniversalReferenceItem(model, model.ReferenceName, false);
            }
            else
            {
                //Insert
                referenceBusinessLogic.SaveUniversalReferenceItem(model, model.ReferenceName, true);
            }
            return RedirectToAction(model.ReferenceName);
        }

        /// <summary>
        /// ClientCategoryRef
        /// </summary>
        /// <returns></returns>
        public ActionResult ClientCategoryRef()
        {
            List<ReferenceUniversalItem> reference = referenceBusinessLogic.GetUniversalList(Constants.ClientCategoryRef);
            ReferenceUniversalItemModel.FillListReferenceUniversal(reference);
            return View(ReferenceUniversalItemModel.ListReferenceUniversalItem);
        }
        [HttpGet]
        public ActionResult ClientCategoryRefEdit(long? id)
        {
            if (id == null)
            {
                ReferenceUniversalItemModel refItem = new ReferenceUniversalItemModel();
                refItem.ReferenceName = Constants.ClientCategoryRef;
                return View(refItem);
            }
            else
            {
                return View(ReferenceUniversalItemModel.ListReferenceUniversalItem.Find(a => a.Id == id));
            }
        }
        [HttpPost]
        public ActionResult ClientCategoryRefEdit(long? id, ReferenceUniversalItem model)
        {
            if (ReferenceUniversalItemModel.ListReferenceUniversalItem.Where(a => a.Id == id).Count() > 0)
            {
                //Update
                referenceBusinessLogic.SaveUniversalReferenceItem(model, model.ReferenceName, false);
            }
            else
            {
                //Insert
                referenceBusinessLogic.SaveUniversalReferenceItem(model, model.ReferenceName, true);
            }
            return RedirectToAction(model.ReferenceName);
        }

        /// <summary>
        /// DocumentTypeRef
        /// </summary>
        /// <returns></returns>
        public ActionResult DocumentTypeRef()
        {
            List<ReferenceUniversalItem> reference = referenceBusinessLogic.GetUniversalList(Constants.DocumentTypeRef);
            ReferenceUniversalItemModel.FillListReferenceUniversal(reference);
            return View(ReferenceUniversalItemModel.ListReferenceUniversalItem);
        }
        [HttpGet]
        public ActionResult DocumentTypeRefEdit(long? id)
        {
            if (id == null)
            {
                ReferenceUniversalItemModel refItem = new ReferenceUniversalItemModel();
                refItem.ReferenceName = Constants.DocumentTypeRef;
                return View(refItem);
            }
            else
            {
                return View(ReferenceUniversalItemModel.ListReferenceUniversalItem.Find(a => a.Id == id));
            }
        }
        [HttpPost]
        public ActionResult DocumentTypeRefEdit(long? id, ReferenceUniversalItem model)
        {
            if (ReferenceUniversalItemModel.ListReferenceUniversalItem.Where(a => a.Id == id).Count() > 0)
            {
                //Update
                referenceBusinessLogic.SaveUniversalReferenceItem(model, model.ReferenceName, false);
            }
            else
            {
                //Insert
                model.Id = (long)ReferenceUniversalItemModel.ListReferenceUniversalItem.Max(a => a.Id) + 1;
                referenceBusinessLogic.SaveUniversalReferenceItem(model, model.ReferenceName, true);
            }
            return RedirectToAction(model.ReferenceName);
        }

        /// <summary>
        /// ScenarioRef
        /// </summary>
        /// <returns></returns>
        public ActionResult ScenarioRef()
        {
            List<ReferenceUniversalItem> reference = referenceBusinessLogic.GetUniversalList(Constants.ScenarioRef);
            ReferenceUniversalItemModel.FillListReferenceUniversal(reference);
            return View(ReferenceUniversalItemModel.ListReferenceUniversalItem);
        }
        [HttpGet]
        public ActionResult ScenarioRefEdit(long? id)
        {
            if (id == null)
            {
                ReferenceUniversalItemModel refItem = new ReferenceUniversalItemModel();
                refItem.ReferenceName = Constants.ScenarioRef;
                return View(refItem);
            }
            else
            {
                return View(ReferenceUniversalItemModel.ListReferenceUniversalItem.Find(a => a.Id == id));
            }
        }
        [HttpPost]
        public ActionResult ScenarioRefEdit(long? id, ReferenceUniversalItem model)
        {
            if (ReferenceUniversalItemModel.ListReferenceUniversalItem.Where(a => a.Id == id).Count() > 0)
            {
                //Update
                referenceBusinessLogic.SaveUniversalReferenceItem(model, model.ReferenceName, false);
            }
            else
            {
                //Insert
                referenceBusinessLogic.SaveUniversalReferenceItem(model, model.ReferenceName, true);
            }
            return RedirectToAction(model.ReferenceName);
        }

        /// <summary>
        /// MedicalCenterRef
        /// </summary>
        /// <returns></returns>
        public ActionResult MedicalCenterRef()
        {
            List<ReferenceUniversalItem> reference = referenceBusinessLogic.GetUniversalList(Constants.MedicalCenterRef);
            ReferenceUniversalItemModel.FillListReferenceUniversal(reference);
            return View(ReferenceUniversalItemModel.ListReferenceUniversalItem);
        }
        [HttpGet]
        public ActionResult MedicalCenterRefEdit(long? id)
        {
            if (id == null)
            {
                ReferenceUniversalItemModel refItem = new ReferenceUniversalItemModel();
                refItem.ReferenceName = Constants.MedicalCenterRef;
                return View(refItem);
            }
            else
            {
                return View(ReferenceUniversalItemModel.ListReferenceUniversalItem.Find(a => a.Id == id));
            }
        }
        [HttpPost]
        public ActionResult MedicalCenterRefEdit(long? id, ReferenceUniversalItem model)
        {
            if (ReferenceUniversalItemModel.ListReferenceUniversalItem.Where(a => a.Id == id).Count() > 0)
            {
                //Update
                referenceBusinessLogic.SaveUniversalReferenceItem(model, model.ReferenceName, false);
            }
            else
            {
                //Insert
                referenceBusinessLogic.SaveUniversalReferenceItem(model, model.ReferenceName, true);
            }
            return RedirectToAction(model.ReferenceName);
        }

    }
}