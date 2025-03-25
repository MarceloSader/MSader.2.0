using MSader.DAL;
using MSader.DTO;

namespace MSader.BLL
{
    public class BlogBLL : BaseBLL
    {
        public List<PostDTO> GetLastPosts()
        {
            using (BlogDAL dal = new BlogDAL())
            {
                return dal.GetLastPosts();
            }
        }
    }
}
