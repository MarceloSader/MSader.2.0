using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSader.DTO;
using MSader.BLL;
using MSader.Helpers;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Hosting;

namespace LinkWise.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult HomeAdmin()
        {
            ViewBag.Menu = new MenuAdminDTO("Home");

            return View("HomeAdmin");
        }

        [HttpGet]
        public IActionResult AINaturalLanguage()
        {
            ViewBag.Menu = new MenuAdminDTO("AITools");

            ViewBag.EstilosResposta = ListHelper.GetListEstilosResposta();

            ViewBag.TiposPost = ListHelper.GetListTipoPost();

            ViewBag.Pessoas = ListHelper.GetListPessoas();

            ViewBag.Vieses = ListHelper.GetListVieses();

            ViewBag.Prompts = ListHelper.GetListPrompts(ConstDTO.TipoPrompt.NaturalLanguage.ID);

            return View("AINaturalLanguage");
        }

        [HttpGet]
        public IActionResult Blogs()
        {
            ViewBag.Menu = new MenuAdminDTO("Blogs");

            return View("Blogs");
        }

        [HttpGet]
        public IActionResult GetBlogs()
        {
            List<BlogDTO> blogs = new List<BlogDTO>();

            using (BlogBLL oBLL = new BlogBLL())
            {
                blogs = oBLL.GetBlogs();
            }
            return Json(new { Blogs = blogs });
        }

        [HttpGet]
        public IActionResult GetHomeBlog(int idBlog)
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
