using Microsoft.Data.SqlClient;
using MSader.DTO;
using Dapper;


namespace MSader.DAL
{
    public class BlogDAL : BaseDAL
    {
        public List<PostDTO> GetLastPosts()
        {
            List<PostDTO> posts = new List<PostDTO>();

            var connectionDB = new SqlConnection("");

            connectionDB.Open();

            posts = connectionDB.Query<PostDTO>("SELECT * FROM Post").ToList();

            connectionDB.Close();

            return posts;
        }
    }
}


