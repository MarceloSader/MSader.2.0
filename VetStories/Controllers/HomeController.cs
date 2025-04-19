using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VetStories.Models;
using MSader.BLL;
using MSader.DTO;

namespace VetStories.Controllers
{
    public class HomeController : Controller
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            HomeBlogDTO homeBlog = new HomeBlogDTO();

            string urlBase = this.GetUrlBase(_httpContextAccessor);

            using (BlogBLL oBLL = new BlogBLL())
            {
                homeBlog = oBLL.GetHomeBlog(urlBase, 1);
            }
                
            ViewBag.HomeBlog = homeBlog;

            return View("Index");
        }

        public IActionResult Post(int p, int b)
        {
            PostDTO post = new PostDTO();

            string urlBase = this.GetUrlBase(_httpContextAccessor);

            string nrIP = "";

            using (BlogBLL oBLL = new BlogBLL())
            {
                post = oBLL.GetPost(urlBase, p, b);

                if (_httpContextAccessor.HttpContext != null)
                {
                    nrIP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                }

                oBLL.AddPostView(p, nrIP);
            }

            ViewBag.Post = post;

            return View("Post");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string GetUrlBase(IHttpContextAccessor httpContextAccessor)
        {
            var context = httpContextAccessor.HttpContext;

            if (context == null) return string.Empty;

            var request = context.Request;

            var urlBase = $"{request.Scheme}://{request.Host}";

            return urlBase;
        }
    }
}
