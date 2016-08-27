using OMInsurance.DataAccess.Core;
using OMInsurance.DataAccess.Materializers;
using OMInsurance.Entities;
using OMInsurance.Entities.Post;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace OMInsurance.DataAccess.DAO
{
    public class PostDao : ItemDao
    {
        private static PostDao _instance = new PostDao();

        private PostDao()
            : base(DatabaseAliases.OMInsurance, new DatabaseErrorHandler())
        {
        }

        public static PostDao Instance
        {
            get
            {
                return _instance;
            }
        }

        public List<Post> Post_All(bool? withContent = null)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ViewContent", SqlDbType.Bit, withContent);
            List<Post> posts = Execute_GetList(PostMaterializer.Instance, "Post_All", parameters);
            return posts;
        }

        public Post Post_GetByID(long ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@Post_ID", SqlDbType.BigInt, ID);
            Post item = Execute_Get(PostMaterializer.Instance, "Post_GetByID", parameters);
            return item;
        }

        public long Post_Save(Post post)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@Title", SqlDbType.NVarChar, post.Title);
            parameters.AddInputParameter("@Content", SqlDbType.NVarChar, post.Content);
            parameters.AddInputParameter("@CreateDate", SqlDbType.DateTime, post.CreateDate);
            parameters.AddInputParameter("@CreateUserID", SqlDbType.BigInt, post.CreateUser.Id);
            parameters.AddInputParameter("@UpdateDate", SqlDbType.DateTime, post.UpdateDate);
            parameters.AddInputParameter("@UpdateUserID", SqlDbType.BigInt, post.UpdateUser.Id);
            parameters.AddInputParameter("@Disabled", SqlDbType.Bit, post.Disabled);
            SqlParameter Post_ID = parameters.AddInputOutputParameter("@Post_ID", SqlDbType.BigInt, post.Id);
            Execute_StoredProcedure("Post_Save", parameters);
            return (long)Post_ID.Value;
        }
    }
}
