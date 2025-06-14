using Dapper;
using Microsoft.Data.SqlClient;
using MSader.DAL;
using MSader.DTO;
using System.Collections.Generic;
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

                foreach (PostDTO post in homeBlog.Posts)
                {
                    if (post.Midias != null)
                    {
                        foreach (MidiaDTO midia in post.Midias)
                        {
                            midia.DSUrlMidia = $"{urlBase}/{ConstantsDTO.PATH_FOTOS}{post.IDPost}/{midia.NMFileName}";
                        }
                    }
                }

                homeBlog.PostsCarousel = oDAL.GetHomePostsCarousel(idBlog);

                foreach (PostDTO post in homeBlog.PostsCarousel)
                {
                    if (post.Midias != null)
                    {
                        foreach (MidiaDTO midia in post.Midias)
                        {
                            midia.DSUrlMidia = $"{urlBase}/{ConstantsDTO.PATH_FOTOS}{post.IDPost}/{midia.NMFileName}";
                        }
                    }
                }

                homeBlog.SetLinks(urlBase, idBlog, 1);

                return homeBlog;
            }
        }

        public PostDTO GetPost(string urlBase, int idPost, int idBlog, int stAcessoRestrito)
        {
            using (BlogDAL oDAL = new BlogDAL())
            {

                PostDTO post = oDAL.GetPost(idPost, idBlog, stAcessoRestrito);

                if (post.Midias != null)
                {
                    foreach (MidiaDTO midia in post.Midias)
                    {
                        midia.DSUrlMidia = $"{urlBase}/{ConstantsDTO.PATH_FOTOS}{post.IDPost}/{midia.NMFileName}";
                    }
                }

                post.PostsLinked = oDAL.GetPostLinked(idPost);

                post.SetUrlPost(urlBase, idBlog);

                if (post.PostsLinked != null)
                {
                    foreach (var postLinked in post.PostsLinked)
                    {
                        postLinked.SetUrlPost(urlBase, idBlog);

                        if (postLinked.Midias != null)
                        {
                            foreach (MidiaDTO midia in postLinked.Midias)
                            {
                                midia.DSUrlMidia = $"{urlBase}/{ConstantsDTO.PATH_FOTOS}{postLinked.IDPost}/{midia.NMFileName}";
                            }
                        }
                    }
                }



                return post;
            }
        }

        public List<PostCommentDTO> GetPostComments(int idPost, string urlBase, int nrPostComments)
        {
            List<PostCommentDTO> postComments = new List<PostCommentDTO>();

            List<PostCommentDTO> postCommentsFull = new List<PostCommentDTO>();

            List<PostCommentDTO> postCommentsParents = new List<PostCommentDTO>();

            using (BlogDAL oDAL = new BlogDAL())
            {
                postCommentsFull = oDAL.GetPostComments(idPost, nrPostComments);

                if (postCommentsFull != null && postCommentsFull.Count > 0)
                {
                    foreach (var postCommentFull in postCommentsFull)
                    {
                        postCommentFull.DTCommentTwo = new DateTimeDTO(postCommentFull.DTComment);

                        if (postCommentFull.IDPessoa == 2 || postCommentFull.IDPessoa == 3)
                        {
                            postCommentFull.DSUrlAvatar = $"{urlBase}/{ConstantsDTO.PATH_AVATARS}/{postCommentFull.IDPessoa}.png";
                        }
                        else
                        {
                            postCommentFull.DSUrlAvatar = $"{urlBase}/{ConstantsDTO.PATH_AVATARS}/0.png";
                        }
                    }

                    postCommentsParents = postCommentsFull.Where(r => r.IDPostCommentParent == 0).ToList();
                }
                
                if (postCommentsParents != null && postCommentsParents.Count > 0)
                {
                    foreach (var postComment in postCommentsParents)
                    {
                        postComment.PostCommentsChildren = postCommentsFull.Where(r => r.IDPostCommentParent == postComment.IDPostComment).ToList();

                        postComments.Add(postComment);
                    }
                }
            }

            return postComments;
        }

        public PostDTO GetPostAdmin(string urlBase, int idPost)
        {
            using (BlogDAL oDAL = new BlogDAL())
            {
                PostDTO post = new PostDTO();

                if (idPost == 0)
                {
                    post.SetNewPost();

                    post.IDPost = oDAL.AddPost(post);
                }

                post = oDAL.GetPostAdmin(idPost);

                post.SetDetails();

                post.PostsLinked = oDAL.GetPostLinked(idPost);

                if (post.Midias != null)
                {
                    foreach (MidiaDTO midia in post.Midias)
                    {
                        midia.DSUrlMidia = $"{urlBase}/{ConstantsDTO.PATH_FOTOS}{post.IDPost}/{midia.NMFileName}";
                    }
                }

                return post;
            }
        }

        public List<PostDTO> GetPosts(string urlBase, int nrPosts, int idBlog)
        {
            List<PostDTO> posts = new List<PostDTO>();

            using (BlogDAL oDAL = new BlogDAL())
            {
                posts = oDAL.GetPosts(nrPosts, idBlog);

                if (posts != null && posts.Count > 0)
                {
                    foreach(var post in posts)
                    {
                        post.DTCriacaoPostTwo = new DateTimeDTO(post.DTCriacaoPost);
                        post.DTPublicacaoPostTwo = new DateTimeDTO(post.DTPublicacaoPost);
                        post.STAcessoRestritoTwo = new BoolDTO(post.STAcessoRestrito);
                        post.STPostAtivoTwo = new BoolDTO(post.STPostAtivo);
                    }
                }
            }

            return posts;
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

        public int AddPost(PostDTO post, PostBlogDTO postBlog)
        {
            if (post != null && post.DSTextoPost != null)
            {
                post.DSTextoPost = post.DSTextoPost.Replace("'", "\"");

                using (BlogDAL oDAL = new BlogDAL())
                {
                    post.IDPost = oDAL.AddPost(post);

                    oDAL.AddPostBlog(postBlog, post.IDPost);
                }
            }

            return post.IDPost;
        }

        public int AddPostComment(PostCommentDTO postComment, VisitanteDTO visitante)   
        {
            
            using (BlogDAL oBlogDAL = new BlogDAL())
            using (PessoaDAL oPessoaDAL = new PessoaDAL())
            {
                visitante.IDVisitante = oPessoaDAL.GetIDVisitante(visitante.CDVisitante);

                if (visitante.IDVisitante == 0)
                {
                    visitante.IDVisitante = oPessoaDAL.AddVisitante(visitante);
                }

                visitante.IDVisitante = visitante.IDVisitante;

                if (postComment.IDPostCommentParent == 0)
                {
                    postComment.IDPostCommentParent = null;
                }

                postComment.IDPostComment = oBlogDAL.AddPostComment(postComment, visitante);
            }

            return postComment.IDPostComment;
        }

        public int AddPostCommentByAI(PostCommentDTO postComment, VisitanteDTO visitante)
        {

            using (BlogDAL oBlogDAL = new BlogDAL())
            using (PessoaDAL oPessoaDAL = new PessoaDAL())
            {
                visitante.IDVisitante = oPessoaDAL.GetIDVisitante(visitante.CDVisitante);

                if (visitante.IDVisitante == 0)
                {
                    visitante.IDVisitante = oPessoaDAL.AddVisitante(visitante);
                }

                visitante.IDVisitante = visitante.IDVisitante;

                postComment.IDPostComment = oBlogDAL.AddPostComment(postComment, visitante);
            }

            return postComment.IDPostComment;
        }

        public void UpdPost(PostDTO post)
        {

            if (post != null && post.DSTextoPost != null)
            {
                post.DSTextoPost = post.DSTextoPost.Replace("'", "\"");

                using (BlogDAL oDAL = new BlogDAL())
                {
                    oDAL.UpdPost(post);
                }
            }
        }

        public void SetMidiaMain(int idPostMidia, int idMidia, int idPost)
        {

            using (BlogDAL oDAL = new BlogDAL())
            {
                oDAL.SetMidiaMain(idPostMidia, idMidia, idPost);
            }
        }

        public void DelMidia(int idPostMidia, int idMidia)
        {

            using (BlogDAL oDAL = new BlogDAL())
            {
                oDAL.DelMidia(idPostMidia, idMidia);
            }
        }

        public void SetPostMidiaOrdem(int idPostMidia, int nrOrdem)
        {

            using (BlogDAL oDAL = new BlogDAL())
            {
                oDAL.SetPostMidiaOrdem(idPostMidia, nrOrdem);
            }
        }

        public void UpdMidia(int idMidia, string nmTitulo, string dsLegenda, string cdEmbedded)
        {

            using (BlogDAL oDAL = new BlogDAL())
            {
                oDAL.UpdMidia(idMidia, nmTitulo, dsLegenda, cdEmbedded);
            }
        }

        public void AddMidiaPost(MidiaDTO midia, int idPost)
        {

            if (midia != null)
            {
                using (BlogDAL oDAL = new BlogDAL())
                {
                    int totalMidiasPost = 0;

                    totalMidiasPost = oDAL.GetTotalMidiasPost(idPost);

                    midia.NROrdem = totalMidiasPost + 1;

                    oDAL.AddMidiaPost(midia, idPost);
                }
            }
        }

        public string GetMidiaFileName(int idPost, string fileName)
        {
            int totalMidiasPost = 0;

            using (BlogDAL oDAL = new BlogDAL())
            {
                totalMidiasPost = oDAL.GetTotalMidiasPost(idPost);
            }

            totalMidiasPost = totalMidiasPost + 1;

            return $"{idPost}-{totalMidiasPost}_{fileName}";
        }


    }
}
