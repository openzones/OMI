using OMInsurance.Entities.Post;
using System.Collections.Generic;

namespace OMInsurance.Interfaces
{
    public interface IPostBusinessLogic
    {
        List<Post> Post_All(bool? withContent = null);
        Post Post_GetByID(long ID);
        long Post_Save(Post post);
    }
}
