using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using OMInsurance.Entities.Post;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.DataAccess.Materializers
{
    public class PostMaterializer : IMaterializer<Post>
    {
        private static readonly PostMaterializer _instance = new PostMaterializer();

        public static PostMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public Post Materialize(DataReaderAdapter dataReader)
        {
            return Materialize_List(dataReader).FirstOrDefault();
        }

        public List<Post> Materialize_List(DataReaderAdapter dataReader)
        {
            List<Post> items = new List<Post>();

            while (dataReader.Read())
            {
                Post obj = ReadItemFields(dataReader);
                items.Add(obj);
            }

            return items;
        }

        public Post ReadItemFields(DataReaderAdapter dataReader, Post item = null)
        {
            if (item == null)
            {
                item = new Post();
            }

            item.Id = dataReader.GetInt64("ID");
            item.Title = dataReader.GetString("Title");
            item.Content = dataReader.GetString("Content");
            item.CreateDate = dataReader.GetDateTime("CreateDate");
            item.CreateUser = new User() {
                Id = dataReader.GetInt64("CreateUserID"),
                Lastname = dataReader.GetString("CreateLastname"),
                Firstname = dataReader.GetString("CreateFirstname"),
                Secondname = dataReader.GetString("CreateSecondname"),
            };

            item.UpdateDate = dataReader.GetDateTimeNull("UpdateDate");
            item.UpdateUser = new User()
            {
                Id = dataReader.GetInt64Null("UpdateUserID") ?? 0,
                Lastname = dataReader.GetString("UpdateLastname"),
                Firstname = dataReader.GetString("UpdateFirstname"),
                Secondname = dataReader.GetString("UpdateSecondname"),
            };

            item.Disabled = dataReader.GetBooleanNull("Disabled") ?? false;

            return item;
        }
    }
}
