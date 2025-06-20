using Microsoft.Data.SqlClient;
using MSader.DTO;
using Dapper;
using System.Data;
using System;
using Microsoft.Identity.Client;
using System.Collections.Generic;


namespace MSader.DAL
{
    public class BlogDAL : BaseDAL
    {

        #region SAVING

        public void AddPostView(int idPost, string nrIP)
        {
            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
            {
                var PostViewDTO = new PostViewDTO() { };

                string sqlCommand = @$"INSERT PostView (IDPost, DTView, NRIP) VALUES(@IDPost, @DTView, @NRIP)";

                var postView = new { IDPost = idPost, DTView = DateTime.Now, NRIP = nrIP };

                var rowsAffected = connectionDB.Execute(sqlCommand, postView);
            }
        }

        public void AddPostAction(PostActionDTO postAction)
        {
            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
            {
                string sqlCommand = @$"INSERT PostAction (IDPostFrom, IDPostTo, IDProduct, IDCampaign, STWentToStore, STWentToPost, DTAction) VALUES(@IDPostFrom, @IDPostTo, @IDProduct, @IDCampaign, @STWentToStore, @STWentToPost, @DTAction)";

                var _postAction = new {IDPostFrom = postAction.IDPostFrom, IDPostTo = postAction.IDPostTo, IDProduct = postAction.IDProduct, IDCampaign = postAction.IDCampaign, STWentToStore = postAction.STWentToStore, STWentToPost = postAction.STWentToPost, DTAction = postAction.DTAction };

                var rowsAffected = connectionDB.Execute(sqlCommand, _postAction);
            }
        }

        public int AddPost(PostDTO post)
        {
            int idPost = 0;

            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
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
                        ,STAcessoRestrito
                    )
                    VALUES
                    (
                         {post.IDPessoa}
                        ,{post.IDTipoPost}
                        ,'{post.DSAncoraPost}'
                        ,'{post.DSTituloPost}'
                        ,'{post.DSSubTituloPost}'
                        ,'{post.DSTextoPost}'
                        ,'{post.DSTags}'
                        ,{post.STPostAtivoSql}
                        ,'{post.DTCriacaoPost.ToString("yyyy-MM-dd HH:mm:ss")}'
                        ,'{post.DTPublicacaoPost.ToString("yyyy-MM-dd HH:mm:ss")}'
                        ,{post.STAcessoRestritoSql}
                    );
                    SELECT CAST(SCOPE_IDENTITY() AS INT);
                ";

                idPost = connectionDB.ExecuteScalar<int>(query, post);
            }

            return idPost;
        }

        public void AddPostBlog(PostBlogDTO postBlog, int idPost)
        {
            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
            {
                string query = @$"
                    INSERT INTO PostBlog
                    (
                         IDPost
                        ,IDBlog
                        ,STHomeBlog
                        ,STHomeBlogCarousel
                        ,NROrdemPost
                        ,NROrdemPostCarousel
                    )
                    VALUES
                    (
                         {idPost}
                        ,{postBlog.IDBlog}
                        ,'{postBlog.STHomeBlogTwo.CDBoolSql}'
                        ,'{postBlog.STHomeBlogCarouselTwo.CDBoolSql}'
                        ,'{postBlog.NROrdemPost}'
                        ,'{postBlog.NROrdemPostCarousel}'
                    );
                ";

                connectionDB.ExecuteScalar<int>(query, postBlog);
            }
        }

        public void UpdPost(PostDTO post)
        {

            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
            {
                string queryUpdateMidia = "UPDATE Post SET IDPessoa = @idPessoa, IDTipoPost = @idTipoPost, DSAncoraPost = @dsAncoraPost, DSTituloPost = @dsTituloPost, DSSubTituloPost = @dsSubTituloPost , DSTextoPost = @dsTextoPost , DSTags = @dsTags , STPostAtivo = @stPostAtivoSql , DTCriacaoPost = @dtCriacaoPost , DTPublicacaoPost = @dtPublicacaoPost , STAcessoRestrito = @stAcessoRestritoSql WHERE IDPost = @idPost";

                connectionDB.Execute(queryUpdateMidia, new { post.IDPost, post.IDPessoa, post.IDTipoPost, post.DSAncoraPost, post.DSTituloPost, post.DSSubTituloPost, post.DSTextoPost, post.DSTags, post.STPostAtivoSql, post.DTCriacaoPost, post.DTPublicacaoPost, post.STAcessoRestritoSql });
            }
        }

        public void SetMidiaMain(int idPostMidia, int idMidia, int idPost)
        {
            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
            {
                string querySetAll = "UPDATE PostMidia SET STMidiaMain = 0 WHERE IDPost = @idPost";
                connectionDB.Execute(querySetAll, new { idPost });

                string querySetMidiaMain = "UPDATE PostMidia SET STMidiaMain = 1 WHERE IDPostMidia = @idPostMidia";
                connectionDB.Execute(querySetMidiaMain, new { idPostMidia });
            }
        }

        public void DelMidia(int idPostMidia, int idMidia)
        {
            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
            {
                string queryDeletePostMidia = "DELETE FROM PostMidia WHERE IDPostMidia = @idPostMidia";
                connectionDB.Execute(queryDeletePostMidia, new { idPostMidia });

                string queryDeleteMidia = "DELETE FROM Midia WHERE IDMidia = @idMidia";
                connectionDB.Execute(queryDeleteMidia, new { idMidia });
            }
        }

        public void SetPostMidiaOrdem(int idPostMidia, int nrOrdem)
        {
            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
            {
                string queryUpdatePostMidiaOrdem = "UPDATE PostMidia SET NROrdem = @nrOrdem WHERE IDPostMidia = @idPostMidia";

                connectionDB.Execute(queryUpdatePostMidiaOrdem, new { idPostMidia, nrOrdem });
            }
        }

        public void UpdMidia(int idMidia, string nmTitulo, string dsLegenda, string cdEmbedded)
        {
            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
            {
                string queryUpdateMidia = "UPDATE Midia SET NMTitulo = @nmTitulo, DSLegenda = @dsLegenda, CDEmbedded = @cdEmbedded WHERE IDMidia = @idMidia";

                connectionDB.Execute(queryUpdateMidia, new { idMidia, nmTitulo, dsLegenda, cdEmbedded });
            }
        }

        public void AddMidiaPost(MidiaDTO midia, int idPost)
        {

            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
            {
                string queryMidia = @$"
                    INSERT INTO Midia
                    (
                         IDTipoMidia
                        ,NMTitulo
                        ,DSLegenda
                        ,CDEmbedded
                        ,NMFileName
                    )
                    VALUES
                    (
                         {midia.IDTipoMidia}
                        ,'{midia.NMTitulo}'
                        ,'{midia.DSLegenda}'
                        ,'{midia.CDEmbedded}'
                        ,'{midia.NMFileName}'
                    );
                    SELECT CAST(SCOPE_IDENTITY() AS INT);
                ";


                int idMidia = connectionDB.ExecuteScalar<int>(queryMidia, midia);

                string queryPostMidia = @$"
                    INSERT INTO PostMidia
                    (
                         IDPost
                        ,IDMidia
                        ,NROrdem
                        ,STMidiaMain
                    )
                    VALUES
                    (
                         {idPost}
                        ,'{idMidia}'
                        ,'{midia.NROrdem}'
                        ,'{midia.STMidiaMain}'
                    )
                ";

                connectionDB.ExecuteScalar<int>(queryPostMidia, midia);

            }
        }

        public int AddPostComment(PostCommentDTO postComment, VisitanteDTO visitante)
        {
            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
            {
                string query = @"
                    INSERT INTO PostComment
                    (
                        IDPostCommentParent,
                        IDPost,
                        IDVisitante,
                        DSComment,
                        DTComment,
                        STPostCommentAtivo,
                        NRIP
                    )
                    VALUES
                    (
                        @IDPostCommentParent,
                        @IDPost,
                        @IDVisitante,
                        @DSComment,
                        @DTComment,
                        @STPostCommentAtivo,
                        @NRIP
                    );
                    SELECT CAST(SCOPE_IDENTITY() AS INT);
                ";

                var parametros = new
                {
                    IDPostCommentParent = (object?)postComment.IDPostCommentParent ?? DBNull.Value,
                    IDPost = postComment.IDPost,
                    IDVisitante = visitante.IDVisitante,
                    DSComment = postComment.DSComment,
                    DTComment = postComment.DTCommentTwo.DSDateTimeSql,
                    STPostCommentAtivo = postComment.STPostCommentAtivoTwo.CDBoolSql,
                    NRIP = postComment.NRIP
                };

                postComment.IDPostComment = connectionDB.ExecuteScalar<int>(query, parametros);
            }

            return postComment.IDPostComment;
        }


        #endregion

        #region GETTING

        public List<BlogDTO> GetBlogs()
        {
            List<BlogDTO> posts = new List<BlogDTO>();

            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
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

            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
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
                    ,r.STAcessoRestrito
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

                if (posts != null)
                {
                    foreach (PostDTO post in posts)
                    {
                        string queryGetMidiasPost = "SELECT r.IDMidia ,r.NROrdem ,r.STMidiaMain ,a.IDTipoMidia ,a.NMTitulo ,a.DSLegenda ,a.CDEmbedded, a.NMFileName FROM PostMidia r INNER JOIN Midia a ON r.IDMidia = a.IDMidia WHERE r.IDPost = @idPost ORDER BY r.NROrdem";
                        post.Midias = connectionDB.Query<MidiaDTO>(queryGetMidiasPost, new { post.IDPost }).ToList();
                    }
                }

            }

            return posts;
        }

        public List<PostBlogDTO> GetHomePostsCarousel(int idBlog)
        {

            List<PostBlogDTO> posts = [];

            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
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

                if (posts != null)
                {
                    foreach (PostDTO post in posts)
                    {
                        string queryGetMidiasPost = "SELECT r.IDMidia ,r.NROrdem ,r.STMidiaMain ,a.IDTipoMidia ,a.NMTitulo ,a.DSLegenda ,a.CDEmbedded, a.NMFileName FROM PostMidia r INNER JOIN Midia a ON r.IDMidia = a.IDMidia WHERE r.IDPost = @idPost ORDER BY r.NROrdem";
                        post.Midias = connectionDB.Query<MidiaDTO>(queryGetMidiasPost, new { post.IDPost }).ToList();
                    }
                }
            }

            return posts;
        }

        public PostDTO GetPost(int idPost, int idBlog, int stAcessoRestrito)
        {
            PostDTO post = new PostDTO();

            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
            {
                string queryGetPost = @$"
                SELECT 
                     r.IDPost
                    ,a.IDBlog
                    ,r.IDPessoa
                    ,b.NMPessoa
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

                string queryGetMidiasPost = @$"
                SELECT 
                     r.IDMidia
                    ,r.NROrdem
                    ,r.STMidiaMain
                    ,a.IDTipoMidia
                    ,a.NMTitulo
                    ,a.DSLegenda
                    ,a.CDEmbedded
                    ,a.NMFileName
                FROM       PostMidia r
                INNER JOIN Midia     a ON r.IDMidia = a.IDMidia
                WHERE 
                        r.IDPost = {idPost}
                ORDER BY r.NROrdem
                ";

                post = connectionDB.Query<PostDTO>(queryGetPost).First();

                post.NRPostViews = connectionDB.Query<int>(queryGetPostViews).First();

                post.Midias = connectionDB.Query<MidiaDTO>(queryGetMidiasPost).ToList();
            }

            return post;
        }

        public int GetTotalMidiasPost(int idPost)
        {
            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
            {
                string query = @"SELECT COUNT(*) FROM PostMidia WHERE IDPost = @IdPost";

                return connectionDB.QuerySingle<int>(query, new { IdPost = idPost });
            }
        }

        public List<PostDTO> GetPosts(int nrPosts, int idBlog)
        {
            List<PostDTO> posts = new List<PostDTO>();

            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
            {
                string query = @$"
                SELECT top {nrPosts}
                      r.IDPost
                    , r.DSTituloPost
                    , r.DSSubTituloPost
                    , r.DSAncoraPost
                    , r.DSTags
                    , r.DTCriacaoPost
                    , r.DTPublicacaoPost
                    , r.STPostAtivo
                    , r.STAcessoRestrito
                FROM       Post     r
                INNER JOIN PostBlog a ON r.IDPost = a.IDPost
                WHERE a.IDBlog = {idBlog}
                 ORDER BY r.DTCriacaoPost DESC
                ";

                posts = connectionDB.Query<PostDTO>(query).ToList();
            }

            return posts;
        }

        public List<PostCommentDTO> GetPostComments(int idPost, int nrComments)
        {
            List<PostCommentDTO> postComments = new List<PostCommentDTO>();

            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
            {
                string query = @$"
                    SELECT top {nrComments}
                          r.IDPostComment
                        , IsNull(r.IDPostCommentParent, 0) AS IDPostCommentParent
                        , r.IDPost
                        , b.IDPessoa
                        , b.NMPessoa
                        , r.DSComment
                        , r.DTComment
                    FROM       PostComment r
                    INNER JOIN Visitante   a ON r.IDVisitante = a.IDVisitante
                    INNER JOIN Pessoa      b ON a.IDPessoa    = b.IDPessoa
                    WHERE r.STPostCommentAtivo = 1
                    ORDER BY r.DTComment DESC
                ";

                postComments = connectionDB.Query<PostCommentDTO>(query).ToList();
            }

            return postComments;
        }

        public PostDTO GetPostAdmin(int idPost)
        {
            PostDTO post = new PostDTO();

            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
            {
                string queryGetPost = @$"
                SELECT 
                     r.IDPost
                    ,a.IDBlog
                    ,r.IDPessoa
                    ,b.NMPessoa
                    ,r.IDTipoPost
                    ,r.DSAncoraPost
                    ,r.DSSubTituloPost
                    ,r.DSTags
                    ,r.DSTituloPost
                    ,r.DSTextoPost
                    ,r.STPostAtivo
                    ,r.DTCriacaoPost
                    ,r.DTPublicacaoPost
                    ,r.STPostAtivo
                    ,r.STAcessoRestrito
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
                ORDER BY a.NROrdemPost
                ";

                string queryGetPostViews = @$"SELECT COUNT(*) FROM PostView WHERE IDPost = {idPost}";

                string queryGetMidiasPost = @$"
                SELECT 
                     r.IDMidia
                    ,r.IDPostMidia
                    ,r.NROrdem
                    ,r.STMidiaMain
                    ,a.NMFileName
                    ,a.IDTipoMidia
                    ,a.NMTitulo
                    ,a.DSLegenda
                    ,a.CDEmbedded
                FROM       PostMidia r
                INNER JOIN Midia     a ON r.IDMidia = a.IDMidia
                WHERE 
                        r.IDPost = {idPost}
                ORDER BY r.NROrdem
                ";

                post = connectionDB.Query<PostDTO>(queryGetPost).First();

                post.NRPostViews = connectionDB.Query<int>(queryGetPostViews).First();

                post.Midias = connectionDB.Query<MidiaDTO>(queryGetMidiasPost).ToList();
            }

            return post;
        }

        public List<PostBlogDTO> GetPostLinked(int idPost)
        {
            List<PostBlogDTO> posts = [];

            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
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

                if (posts != null)
                {
                    foreach (PostDTO post in posts)
                    {
                        string queryGetMidiasPost = "SELECT r.IDMidia ,r.NROrdem ,r.STMidiaMain ,a.IDTipoMidia ,a.NMTitulo ,a.DSLegenda ,a.CDEmbedded, a.NMFileName FROM PostMidia r INNER JOIN Midia a ON r.IDMidia = a.IDMidia WHERE r.IDPost = @idPost ORDER BY r.NROrdem";
                        post.Midias = connectionDB.Query<MidiaDTO>(queryGetMidiasPost, new { post.IDPost }).ToList();
                    }
                }

                connectionDB.Close();

            }

            return posts;
        }

        public List<TipoPostDTO> GetTiposPost()
        {
            List<TipoPostDTO> tipos = [];

            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
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

            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
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

            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
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

        public List<PostCommentDTO> GetPostComments(int idPost)
        {
            List<PostCommentDTO> postComments = new List<PostCommentDTO>();

            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
            {
                string query = @$"
                SELECT 
                      r.IDPostComment
                    , r.IDPostCommentParent
                    , r.IDPost
                    , b.NMPessoa
                    , r.IDVisitante
                    , r.DSComment
                    , r.DTComment
                    , r.STPostCommentAtivo
                FROM       PostComment r
                    INNER JOIN Visitante a ON r.IDVisitante = a.IDVisitante
                    INNER JOIN Pessoa    b ON a.IDPessoa    = b.IDPessoa
                 ORDER BY r.DTComment DESC
                ";

                postComments = connectionDB.Query<PostCommentDTO>(query).ToList();
            }

            return postComments;
        }

        #endregion

    }
}


