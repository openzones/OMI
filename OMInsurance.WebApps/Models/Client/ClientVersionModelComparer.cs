using OMInsurance.Entities;
using System.Collections.Generic;

namespace OMInsurance.WebApps.Models
{
    public class ClientVersionModelComparer : IEqualityComparer<ClientVersion>
    {
        public bool Equals(ClientVersion x, ClientVersion y)
        {
            return x.Firstname == y.Firstname
                && x.Lastname == y.Lastname
                && x.Secondname == y.Secondname
                && x.Birthday == y.Birthday
                && x.Sex == y.Sex
                && x.SNILS == y.SNILS;
        }

        public int GetHashCode(ClientVersion obj)
        {
            return (obj.Firstname ?? string.Empty).GetHashCode()
                   ^ (obj.Lastname ?? string.Empty).GetHashCode()
                   ^ (obj.Secondname ?? string.Empty).GetHashCode()
                   ^ (obj.Birthday != null ? obj.Birthday.ToString() : string.Empty).GetHashCode()
                   ^ (obj.Sex ?? string.Empty).GetHashCode()
                   ^ (obj.SNILS ?? string.Empty).GetHashCode();
        }
    }
}