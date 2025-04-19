using Microsoft.Data.SqlClient;
using MSader.DAL;
using MSader.DTO;
using System.Runtime.Intrinsics.Arm;

namespace MSader.BLL
{
    public class BlogBLL : BaseBLL
    {
        public HomeBlogDTO GetHomeBlog(string urlBase, int idBlog)
        {
            using (BlogDAL oDAL = new BlogDAL())
            {
                HomeBlogDTO homeBlog = new HomeBlogDTO();

                homeBlog.IDBlog = idBlog;

                homeBlog.Posts = oDAL.GetHomePosts(idBlog);

                homeBlog.PostsCarousel = oDAL.GetHomePostsCarousel(idBlog);

                homeBlog.SetLinks(urlBase, idBlog, 1);

                return homeBlog;
            }
        }

        public PostDTO GetPost(string urlBase, int idPost, int idBlog)
        {
            using (BlogDAL oDAL = new BlogDAL())
            {

                PostDTO post = oDAL.GetPost(idPost, idBlog);

                post.SetMidia(urlBase, 1);

                post.SetUrlPost(urlBase, idBlog);

                return post;
            }
        }

        public void AddPostView(int idPost, string nrIP)
        {
            using (BlogDAL oDAL = new BlogDAL())
            {
                oDAL.AddPostView(idPost, nrIP);
            }
        }
    }
}
