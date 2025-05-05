using Dapper;
using Microsoft.Data.SqlClient;
using MSader.DAL;
using MSader.DTO;
using System.Runtime.Intrinsics.Arm;

namespace MSader.BLL
{
    public class BlogBLL : BaseBLL
    {
        public List<BlogDTO> GetBlogs()
        {
            List<BlogDTO> blogs = new List<BlogDTO>();

            using (BlogDAL oDAL = new BlogDAL())
            {
                blogs = oDAL.GetBlogs();
            }

            return blogs;
        }

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

        public PostDTO GetPost(string urlBase, int idPost, int idBlog, int stAcessoRestrito)
        {
            using (BlogDAL oDAL = new BlogDAL())
            {

                PostDTO post = oDAL.GetPost(idPost, idBlog, stAcessoRestrito);

                post.PostsLinked = oDAL.GetPostLinked(idPost);

                post.SetMidia(urlBase, 1);

                post.SetUrlPost(urlBase, idBlog);

                if (post.PostsLinked != null)
                {
                    foreach (var poostsLinked in post.PostsLinked)
                    {
                        poostsLinked.SetMidia(urlBase, 1);

                        poostsLinked.SetUrlPost(urlBase, idBlog);
                    }
                }

                return post;
            }
        }

        public List<TipoPostDTO> GetTiposPost()
        {
            List<TipoPostDTO> tiposPost = new List<TipoPostDTO>();

            using (BlogDAL oDAL = new BlogDAL())
            {
                tiposPost = oDAL.GetTiposPost();
            }

            return tiposPost;
        }

        public TipoPostDTO GetTipoPost(int idTipoPost)
        {
            TipoPostDTO tipoPost = new TipoPostDTO();

            using (BlogDAL oDAL = new BlogDAL())
            {
                tipoPost = oDAL.GetTipoPost(idTipoPost);
            }

            return tipoPost;
        }

        public List<PessoaDTO> GetPessoas()
        {
            List<PessoaDTO> pessoas = new List<PessoaDTO>();

            using (BlogDAL oDAL = new BlogDAL())
            {
                pessoas = oDAL.GetPessoas();
            }

            return pessoas;
        }

        public void AddPostView(int idPost, string nrIP)
        {
            using (BlogDAL oDAL = new BlogDAL())
            {
                oDAL.AddPostView(idPost, nrIP);
            }
        }

        public void AddPostAction(PostActionDTO postAction)
        {
            using (BlogDAL oDAL = new BlogDAL())
            {
                oDAL.AddPostAction(postAction);
            }
        }

        public void AddPost(PostDTO post)
        {
            using (BlogDAL oDAL = new BlogDAL())
            {
                oDAL.AddPost(post);
            }
        }
    }
}
