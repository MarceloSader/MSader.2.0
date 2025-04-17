using Microsoft.Data.SqlClient;
using MSader.DTO;
using Dapper;


namespace MSader.DAL
{
    public class BlogDAL : BaseDAL
    {
        public List<PostBlogDTO> GetHomePosts(int idBlog)
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
                    AND a.STHomeBlog = 1
                ORDER BY a.NROrdemPost
                ";

            posts = connectionDB.Query<PostBlogDTO>(query).ToList();

            connectionDB.Close();

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
                        r.IDPost = {idPost}
                    AND a.IDBlog = {idBlog}
                ORDER BY a.NROrdemPost
                ";

            PostDTO post = connectionDB.Query<PostDTO>(query).First();

            connectionDB.Close();

            return post;
        }
    }
}


