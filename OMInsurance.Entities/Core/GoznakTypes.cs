using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.Entities.Core
{
    public class GoznakTypes
    {
        public static ReferenceItem DontSent;
        public static ReferenceItem PrintPolicyFirstTime;
        public static ReferenceItem PrintPolicyRepeatedly;
        public static ReferenceItem DigitalPolicyFirstTime;
        public static ReferenceItem DigitalPolicyRepeatedly;

        static GoznakTypes()
        {
            DontSent = new ReferenceItem() { Id = 1, Code = "0", Name = "в ГОЗНАК на печать не посылать" };
            PrintPolicyFirstTime = new ReferenceItem() { Id = 2, Code = "1", Name = "Отправить в ГОЗНАК с признаком печати \"впервые\"" };
            PrintPolicyRepeatedly = new ReferenceItem() { Id = 3, Code = "2", Name = "Отправить в ГОЗНАК с признаком печати \"повторно\"" };
            DigitalPolicyFirstTime = new ReferenceItem() { Id = 4, Code = "3", Name = "электронный полис с признаком печати \"впервые\"" };
            DigitalPolicyRepeatedly = new ReferenceItem() { Id = 5, Code = "4", Name = "электронный полис с признаком печати \"повторно\"" };
        }

        public bool Equals(ReferenceItem x, ReferenceItem y)
        {
            return x.Code == y.Code && x.Id == y.Id;
        }

        public int GetHashCode(ReferenceItem obj)
        {
            if (string.IsNullOrEmpty(obj.Code))
            {
                return obj.Id.GetHashCode();
            };
            return obj.Id.GetHashCode() ^ (obj.Code.GetHashCode());
        }
    }
}
