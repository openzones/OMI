using OMInsurance.Entities.Core;
using System.ComponentModel;

namespace OMInsurance.Entities
{
    /// <summary>
    /// Справочник статусов БСО
    /// </summary>
    public class BSOStatusRef : DataObject
    {
        [DisplayName("Статус")]
        public string Name { get; set; }
    }
}
