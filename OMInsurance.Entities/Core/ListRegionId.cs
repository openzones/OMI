using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.Entities.Core
{
    public enum ListRegionId : long
    {
        Default = 0, //значит используется стандартный формат загрузки

        //данные RegionId должны жестко соответсвовать ID в справочнике UralsibRegionsRef
        Moscow = 1,
        MoscowObl = 2,
        Ufa = 3,
        Samara = 4
    }
}
