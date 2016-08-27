using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OMInsurance.Entities.Post;
using System.ComponentModel;
using OMInsurance.Entities;

namespace OMInsurance.WebApps.Models
{
    public class PostModel
    {
        public PostModel()
        {
            //this.Content = string.Empty;
            //this.PostId = PostId;
            CreateUser = new UserModel();
            UpdateUser = new UserModel();
        }

        public long? PostId { get; set; }

        [DisplayName("Заголовок")]
        public string Title { get; set; }

        [UIHint("tinymce_jquery_full"), AllowHtml]
        [DisplayName("Контент")]
        public string Content { get; set; }

        [DisplayName("Создан")]
        public DateTime? CreateDate { get; set; }

        [DisplayName("Обновлен")]
        public DateTime? UpdateDate { get; set; }

        [DisplayName("Автор")]
        public string CreateUserName { get; set; }

        [DisplayName("Обновил")]
        public string UpdateUserName { get; set; }

        public bool Disabled { get; set; }


        public UserModel CreateUser { get; set; }
        public UserModel UpdateUser { get; set; }

        public PostModel(Post post)
        {
            this.PostId = post.Id;
            this.Title = post.Title;
            this.Content = post.Content;
            this.CreateDate = post.CreateDate;
            this.CreateUser = new UserModel(post.CreateUser);
            this.CreateUserName = post.CreateUser.Fullname;
            this.UpdateDate = post.UpdateDate;
            this.UpdateUser = new UserModel(post.UpdateUser);
            this.UpdateUserName = post.UpdateUser.Fullname;

        }

        public Post GetPost(User user = null)
        {
            Post post = new Post();
            post.Id = this.PostId ?? 0;
            post.Title = this.Title;
            post.Content = this.Content;
            post.CreateDate = this.CreateDate;
            post.CreateUser = new User() { Id = this.CreateUser.Id ?? 0 };
            post.UpdateDate = this.UpdateDate;
            post.UpdateUser = new User() { Id = this.UpdateUser.Id ?? 0 };
            post.Disabled = this.Disabled;
            return post;
        }
    }
}
