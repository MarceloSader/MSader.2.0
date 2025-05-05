using Microsoft.Data.SqlClient;
using MSader.DTO;
using Dapper;
using System.Data;
using System;
using Microsoft.Identity.Client;


namespace MSader.DAL
{
    public class BlogDAL : BaseDAL
    {
        #region SAVING

        public void AddPostView(int idPost, string nrIP)
        {
            using (var connectionDB = new SqlConnection("Server=tcp:sql-msader-prd-01.database.windows.net,1433;Initial Catalog=sqldb-msader-prd-01;Persist Security Info=False;User ID=msader-operator;Password=CeHAd?ad8U;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                var PostViewDTO = new PostViewDTO() { };

                string sqlCommand = @$"INSERT PostView (IDPost, DTView, NRIP) VALUES(@IDPost, @DTView, @NRIP)";

                var postView = new { IDPost = idPost, DTView = DateTime.Now, NRIP = nrIP };

                var rowsAffected = connectionDB.Execute(sqlCommand, postView);
            }
        }

        public void AddPostAction(PostActionDTO postAction)
        {
            using (var connectionDB = new SqlConnection("Server=tcp:sql-msader-prd-01.database.windows.net,1433;Initial Catalog=sqldb-msader-prd-01;Persist Security Info=False;User ID=msader-operator;Password=CeHAd?ad8U;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                string sqlCommand = @$"INSERT PostAction (IDPostFrom, IDPostTo, IDProduct, IDCampaign, STWentToStore, STWentToPost, DTAction) VALUES(@IDPostFrom, @IDPostTo, @IDProduct, @IDCampaign, @STWentToStore, @STWentToPost, @DTAction)";

                var _postAction = new {IDPostFrom = postAction.IDPostFrom, IDPostTo = postAction.IDPostTo, IDProduct = postAction.IDProduct, IDCampaign = postAction.IDCampaign, STWentToStore = postAction.STWentToStore, STWentToPost = postAction.STWentToPost, DTAction = postAction.DTAction };

                var rowsAffected = connectionDB.Execute(sqlCommand, _postAction);
            }
        }

        public void AddPost(PostDTO post)
        {
            using (var connectionDB = new SqlConnection("Server=tcp:sql-msader-prd-01.database.windows.net,1433;Initial Catalog=sqldb-msader-prd-01;Persist Security Info=False;User ID=msader-operator;Password=CeHAd?ad8U;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                string query = @$"
                    INSERT INTO Post
                    (
                         IDPessoa
                        ,IDTipoPost
                        ,DSAncoraPost
                        ,DSTituloPost
                        ,DSSubTituloPost
                        ,DSTextoPost
                        ,DSTags
                        ,STPostAtivo
                        ,DTCriacaoPost
                        ,DTPublicacaoPost
                    )
                    VALUES
                    (
                         {post.IDPessoa}
                        ,'{post.IDTipoPost}'
                        ,'{post.DSAncoraPost}'
                        ,'{post.DSTituloPost}'
                        ,'{post.DSSubTituloPost}'
                        ,'{post.DSTextoPost}'
                        ,'{post.DSTags}'
                        ,{post.STPostAtivoSql}
                        ,'{post.DTCriacaoPost.ToString("yyyy-MM-dd HH:mm:ss")}'
                        ,'{post.DTPublicacaoPost.ToString("yyyy-MM-dd HH:mm:ss")}'
                    )
                ";

                connectionDB.Execute(query, post);
            }
        }


        #endregion

        #region GETTING

        public List<BlogDTO> GetBlogs()
        {
            List<BlogDTO> posts = new List<BlogDTO>();

            using (var connectionDB = new SqlConnection("Server=tcp:sql-msader-prd-01.database.windows.net,1433;Initial Catalog=sqldb-msader-prd-01;Persist Security Info=False;User ID=msader-operator;Password=CeHAd?ad8U;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                string query = @$"
                SELECT 
                      r.IDBlog
                    , r.NMBlog
                    , r.NMAliasBlog
                    , r.DSBlog
                    , r.DTCadastroBlog
                    , r.DSUrlBlog
                    , r.NROrdemBlog
                    , r.STBlogAtivo
                    , r.DSClassIcon
                FROM       Blog r
                 ORDER BY r.NMBlog
                ";

                posts = connectionDB.Query<BlogDTO>(query).ToList();
            }

            return posts;
        }

        public List<PostBlogDTO> GetHomePosts(int idBlog)
        {
            List<PostBlogDTO> posts = new List<PostBlogDTO>();

            using (var connectionDB = new SqlConnection("Server=tcp:sql-msader-prd-01.database.windows.net,1433;Initial Catalog=sqldb-msader-prd-01;Persist Security Info=False;User ID=msader-operator;Password=CeHAd?ad8U;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                string query = @$"
                SELECT 
                     r.IDPessoa
                    ,b.NMPessoa
                    ,r.IDPost
                    ,r.DSAncoraPost
                    ,r.DSSubTituloPost
                    ,r.DSTags
                    ,r.DSTituloPost
                    ,r.DSTextoPost
                    ,r.STPostAtivo
                    ,r.DTCriacaoPost
                    ,r.DTPublicacaoPost
                    ,ISNULL(v.PostViews, 0) AS NRPostViews
                FROM       Post r
                INNER JOIN PostBlog a ON r.IDPost    = a.IDPost
                INNER JOIN Pessoa   b ON r.IDPessoa  = b.IDPessoa
                LEFT JOIN (
                    SELECT IDPost, COUNT(IDPostView) AS PostViews
                    FROM PostView
                    GROUP BY IDPost
                    ) v ON r.IDPost = v.IDPost
                WHERE 
                        a.IDBlog = {idBlog}
                    AND a.STHomeBlog = 1
                ORDER BY a.NROrdemPost
                ";

                posts = connectionDB.Query<PostBlogDTO>(query).ToList();
            }

            return posts;
        }

        public List<PostBlogDTO> GetHomePostsCarousel(int idBlog)
        {
            List<PostBlogDTO> posts = [];

            using (var connectionDB = new SqlConnection("Server=tcp:sql-msader-prd-01.database.windows.net,1433;Initial Catalog=sqldb-msader-prd-01;Persist Security Info=False;User ID=msader-operator;Password=CeHAd?ad8U;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {

                string query = @$"
                SELECT 
                     r.IDPessoa
                    ,b.NMPessoa
                    ,r.IDPost
                    ,r.DSAncoraPost
                    ,r.DSSubTituloPost
                    ,r.DSTags
                    ,r.DSTituloPost
                    ,r.DSTextoPost
                    ,r.STPostAtivo
                    ,r.DTCriacaoPost
                    ,r.DTPublicacaoPost
                    ,ISNULL(v.PostViews, 0) AS NRPostViews
                FROM       Post r
                INNER JOIN PostBlog a ON r.IDPost    = a.IDPost
                INNER JOIN Pessoa   b ON r.IDPessoa  = b.IDPessoa
                LEFT JOIN (
                    SELECT IDPost, COUNT(IDPostView) AS PostViews
                    FROM PostView
                    GROUP BY IDPost
                    ) v ON r.IDPost = v.IDPost
                WHERE 
                        a.IDBlog = {idBlog}
                    AND a.STHomeBlogCarousel = 1
                ORDER BY a.NROrdemPostCarousel
                ";

                posts = connectionDB.Query<PostBlogDTO>(query).ToList();
            }

            return posts;
        }

        public PostDTO GetPost(int idPost, int idBlog, int stAcessoRestrito)
        {
            PostDTO post = new PostDTO();

            using (var connectionDB = new SqlConnection("Server=tcp:sql-msader-prd-01.database.windows.net,1433;Initial Catalog=sqldb-msader-prd-01;Persist Security Info=False;User ID=msader-operator;Password=CeHAd?ad8U;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                string queryGetPost = @$"
                SELECT 
                     r.IDPessoa
                    ,b.NMPessoa
                    ,r.IDPost
                    ,r.DSAncoraPost
                    ,r.DSSubTituloPost
                    ,r.DSTags
                    ,r.DSTituloPost
                    ,r.DSTextoPost
                    ,r.STPostAtivo
                    ,r.DTCriacaoPost
                    ,r.DTPublicacaoPost
                    ,ISNULL(v.PostViews, 0) AS NRPostViews
                FROM       Post r
                INNER JOIN PostBlog a ON r.IDPost    = a.IDPost
                INNER JOIN Pessoa   b ON r.IDPessoa  = b.IDPessoa
                LEFT JOIN (
                    SELECT IDPost, COUNT(IDPostView) AS PostViews
                    FROM PostView
                    GROUP BY IDPost
                    ) v ON r.IDPost = v.IDPost
                WHERE 
                        r.IDPost = {idPost}
                    AND a.IDBlog = {idBlog}
                    AND r.STAcessoRestrito = {stAcessoRestrito}
                ORDER BY a.NROrdemPost
                ";

                string queryGetPostViews = @$"SELECT COUNT(*) FROM PostView WHERE IDPost = {idPost}";

                post = connectionDB.Query<PostDTO>(queryGetPost).First();

                post.NRPostViews = connectionDB.Query<int>(queryGetPostViews).First();
            }

            return post;
        }

        public List<PostBlogDTO> GetPostLinked(int idPost)
        {
            List<PostBlogDTO> posts = [];

            using (var connectionDB = new SqlConnection("Server=tcp:sql-msader-prd-01.database.windows.net,1433;Initial Catalog=sqldb-msader-prd-01;Persist Security Info=False;User ID=msader-operator;Password=CeHAd?ad8U;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {

                string query = @$"
                SELECT 
                     r.IDPessoa
                    ,b.NMPessoa
                    ,r.IDPost
                    ,r.DSAncoraPost
                    ,r.DSSubTituloPost
                    ,r.DSTags
                    ,r.DSTituloPost
                    ,r.DSTextoPost
                    ,r.STPostAtivo
                    ,r.DTCriacaoPost
                    ,r.DTPublicacaoPost
                    ,ISNULL(v.PostViews, 0) AS NRPostViews
                FROM       Post r
                INNER JOIN PostBlog     a ON r.IDPost    = a.IDPost
                INNER JOIN Pessoa       b ON r.IDPessoa  = b.IDPessoa
                LEFT JOIN (
                    SELECT IDPost, COUNT(IDPostView) AS PostViews
                    FROM PostView
                    GROUP BY IDPost
                    ) v ON r.IDPost = v.IDPost
                WHERE r.IDPost in (SELECT IDPostSecundario FROM PostLinked WHERE IDPostPrincipal = {idPost})
				ORDER BY a.NROrdemPost
                ";

                posts = connectionDB.Query<PostBlogDTO>(query).ToList();

                connectionDB.Close();

            }

            return posts;
        }

        public List<TipoPostDTO> GetTiposPost()
        {
            List<TipoPostDTO> tipos = [];

            using (var connectionDB = new SqlConnection("Server=tcp:sql-msader-prd-01.database.windows.net,1433;Initial Catalog=sqldb-msader-prd-01;Persist Security Info=False;User ID=msader-operator;Password=CeHAd?ad8U;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                string query = @$"
                SELECT 
                     r.IDTipoPost
                    ,r.NMTipoPost

                FROM       TipoPost r
                ORDER BY r.NMTipoPost
                ";

                tipos = connectionDB.Query<TipoPostDTO>(query).ToList();
            }

            return tipos;
        }

        public TipoPostDTO GetTipoPost(int idTipoPost)
        {
            TipoPostDTO tipoPost = new TipoPostDTO();

            using (var connectionDB = new SqlConnection("Server=tcp:sql-msader-prd-01.database.windows.net,1433;Initial Catalog=sqldb-msader-prd-01;Persist Security Info=False;User ID=msader-operator;Password=CeHAd?ad8U;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                string query = @$"
                SELECT 
                     r.IDTipoPost
                    ,b.NMTipoPost
                    ,r.STTipoPostAtivo
                FROM       TipoPost r
                WHERE   r.IDTipoPost = {idTipoPost}
                ";

                tipoPost = connectionDB.Query<TipoPostDTO>(query).First();
            }

            return tipoPost;
        }

        public List<PessoaDTO> GetPessoas()
        {
            List<PessoaDTO> pessoas = [];

            using (var connectionDB = new SqlConnection("Server=tcp:sql-msader-prd-01.database.windows.net,1433;Initial Catalog=sqldb-msader-prd-01;Persist Security Info=False;User ID=msader-operator;Password=CeHAd?ad8U;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                string query = @$"
                SELECT 
                     r.IDPessoa
                    ,r.NMPessoa

                FROM       Pessoa r
                ORDER BY r.NMPessoa
                ";

                pessoas = connectionDB.Query<PessoaDTO>(query).ToList();
            }

            return pessoas;
        }

        #endregion

    }
}


