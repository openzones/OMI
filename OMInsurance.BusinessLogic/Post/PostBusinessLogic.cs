using OMInsurance.DataAccess.DAO;
using OMInsurance.Entities.Post;
using OMInsurance.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace OMInsurance.BusinessLogic
{
    public class PostBusinessLogic : IPostBusinessLogic
    {
        public List<Post> Post_All(bool? withContent = null)
        {
            return PostDao.Instance.Post_All(withContent);
        }

        public Post Post_GetByID(long ID)
        {
            return PostDao.Instance.Post_GetByID(ID);
        }
        public long Post_Save(Post post)
        {
            return PostDao.Instance.Post_Save(post);
        }
    }
}
