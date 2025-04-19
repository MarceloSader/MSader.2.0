using System;
using System.Collections.Generic;

namespace MSader.DTO
{
    public class BlogDTO
    {
        #region Propriedades 

        public int IDBlog { get; set; }

        public string? NMBlog { get; set; }

        public string? NMAliasBlog { get; set; }

        public string? DSBlog { get; set; }

        public string? DSClassIcon { get; set; }

        public string? DSUrlBlog { get; set; }

        public DateTime DTCadastroBlog { get; set; }

        public bool STBlogAtivo { get; set; }

        public List<PostBlogDTO>? Posts { get; set; }

        #endregion

        #region Construtores 

        public BlogDTO()
        { }

        #endregion

        #region Métodos 
        #endregion
    }

    public class HomeBlogDTO
    {
        #region Propriedades 

        public int IDBlog { get; set; }

        public List<PostBlogDTO>? Posts { get; set; }

        public List<PostBlogDTO>? PostsCarousel { get; set; }

        #endregion

        #region Construtores 

        public HomeBlogDTO()
        { }

        #endregion

        #region Métodos 

        public void SetLinks(string urlBase, int idBlog, int nrOrdem)
        {
            foreach (var post in Posts ?? Enumerable.Empty<PostBlogDTO>())
            {
                post.SetMidia(urlBase, nrOrdem);
                post.SetUrlPost(urlBase, idBlog);
            }

            foreach (var post in PostsCarousel ?? Enumerable.Empty<PostBlogDTO>())
            {
                post.SetMidia(urlBase, nrOrdem);
                post.SetUrlPost(urlBase, idBlog);
            }
        }

        #endregion
    }

}
