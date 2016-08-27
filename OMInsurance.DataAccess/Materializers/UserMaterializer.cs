using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.DataAccess.Materializers
{
    public class UserMaterializer : IMaterializer<User>
    {
        private static readonly UserMaterializer instance = new UserMaterializer();

        public static UserMaterializer Instance
        {
            get
            {
                return instance;
            }
        }
        public User Materialize(DataReaderAdapter dataReader)
        {
            return Materialize_List(dataReader).FirstOrDefault();
        }

        public List<User> Materialize_List(DataReaderAdapter dataReader)
        {
            List<User> items = new List<User>();
            Dictionary<long, User> usersById = new Dictionary<long, User>();

            while (dataReader.Read())
            {
                User obj = ReadItemFields(dataReader);
                usersById.Add(obj.Id, obj);
                items.Add(obj);
            }

            dataReader.NextResult();

            while (dataReader.Read())
            {
                long userId = dataReader.GetInt64("UserID");
                Role obj = RoleMaterializer.Instance.ReadItemFields(dataReader);
                usersById[userId].Roles.Add(obj);
            }

            return items;
        }

        public User ReadItemFields(DataReaderAdapter dataReader, User item = null)
        {
            if (item == null)
            {
                item = new User();
            }
            item.Id = dataReader.GetInt64("ID");
            item.Login = dataReader.GetString("Login");
            item.PasswordHash = dataReader.GetString("PasswordHash");
            item.Department = ReferencesMaterializer.Instance.ReadItemFields(dataReader, "DepartmentID", "DepartmentCode", "DepartmentName");
            item.DepartmentDisplayName = dataReader.GetString("DepartmentDisplayName");
            item.DeliveryPoint = ReferencesMaterializer.Instance.ReadItemFields(dataReader, "DeliveryPointID", "DeliveryPointCode", "DeliveryPointName");
            item.Firstname = dataReader.GetString("Firstname");
            item.Secondname = dataReader.GetString("Secondname");
            item.Lastname = dataReader.GetString("Lastname");
            item.IsBlocked = dataReader.GetBooleanNull("IsBlocked");
            item.CreateDate = dataReader.GetDateTime("CreateDate");
            item.UpdateDate = dataReader.GetDateTime("UpdateDate");
            item.Position = dataReader.GetString("Position");
            item.Email = dataReader.GetString("Email");
            item.Phone = dataReader.GetString("Phone");

            return item;
        }
    }
}
