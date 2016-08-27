using System.Collections.Generic;

namespace OMInsurance.Entities.Core
{
    public class Role : DataObject
    {
        public static Role Administrator = new Role() { Id = 1, Name = "Administrator", Description = "Администратор", Weight = 0 };
        public static Role Registrator = new Role() { Id = 2, Name = "Registrator", Description = "Регистратор", Weight = 100};
        public static Role OperatorSG = new Role() { Id = 3, Name = "OperatorSG", Description = "Оператор СГ", Weight = 30};
        public static Role AdministratorBSO = new Role() { Id = 4, Name = "AdministratorBSO", Description = "Администратор БСО", Weight = 10};
        public static Role ResponsibleBSO = new Role() { Id = 5, Name = "ResponsibleBSO", Description = "Ответственный БСО", Weight = 20};
        public static List<Role> Roles = new List<Role>() { Administrator, Registrator, OperatorSG, AdministratorBSO, ResponsibleBSO };
        public string Name { get; set; }
        public string Description { get; set; }

        //Вес роли пользователя, чем меньше тем большие прав имеет пользователь
        //Необходимо, т.к. пользователь может быть Администратором и Регистратором - значит он Администратор.
        public long Weight { get; set; }
        

        public override bool Equals(object obj)
        {
            Role role = (Role)obj;
            return role.Id == this.Id && role.Name == Name;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Name.GetHashCode();
        }


        //определяет реальную Роль пользователя. См. переменную Weight
        public static Role GetRealRole(User user)
        {
            //по умолчанию обычный Регистратор
            Role role = Role.Registrator;
            long min = Role.Registrator.Weight;

            min = GetRealWeight(user);

            foreach (var r in Roles)
            {
                if (min == r.Weight) role = r;
            }

            return role;
        }

        //определяет реальный Вес пользователя
        private static long GetRealWeight(User user)
        {
            long min = Role.Registrator.Weight;

            foreach(var r in user.Roles)
            {
                if (r.Weight < min) min = r.Weight;
            }
            return min;
        }

        public static User FillWeightRoles(User user)
        {
            foreach(var roleInUser in user.Roles)
            {
                foreach(var roleInList in Roles)
                {
                    if (roleInUser.Id == roleInList.Id) roleInUser.Weight = roleInList.Weight;
                }

            }
            return user;
        }



    }
}
