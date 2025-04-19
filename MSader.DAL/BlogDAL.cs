using Microsoft.Data.SqlClient;
using MSader.DTO;
using Dapper;
using System.Data;


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

        #endregion

        #region GETTING

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
            List<PostBlogDTO> posts = new List<PostBlogDTO>();

            var connectionDB = new SqlConnection("Server=tcp:sql-msader-prd-01.database.windows.net,1433;Initial Catalog=sqldb-msader-prd-01;Persist Security Info=False;User ID=msader-operator;Password=CeHAd?ad8U;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            
            connectionDB.Open();

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

            connectionDB.Close();

            return posts;
        }

        public PostDTO GetPost(int idPost, int idBlog)
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
                ORDER BY a.NROrdemPost
                ";

                string queryGetPostViews = @$"SELECT COUNT(*) FROM PostView WHERE IDPost = {idPost}";

                post = connectionDB.Query<PostDTO>(queryGetPost).First();

                post.NRPostViews = connectionDB.Query<int>(queryGetPostViews).First();
            }

            return post;
        }

        #endregion

    }
}


