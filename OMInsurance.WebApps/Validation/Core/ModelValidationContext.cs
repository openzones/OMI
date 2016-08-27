using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;

namespace OMInsurance.WebApps.Validation
{
    public class ModelValidationContext
    {
        public User currenUser { get; set; }
        public List<ReferenceItem> listReferenceItem { get; set; }
    }
}