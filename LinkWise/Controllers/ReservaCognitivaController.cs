using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSader.BLL;
using MSader.DTO;

namespace LinkWise.Controllers
{

    [Authorize(Roles = "Convidado")]
    public class ReservaCognitivaController : Controller
    {

        #region Propriedades

        string msgRetorno = string.Empty;
        string strStatus = "ERRO";

        #endregion

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReservaCognitivaController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize(Roles = "Convidado")]
        /// <summary>
        /// Blog Pessoal, para posts com acesso restrito
        /// </summary>
        [Authorize]
        public IActionResult Home()
        {

            ViewBag.Menu = new MenuPublicDTO("Blog");

            using (BlogBLL oBLL = new BlogBLL())
            {
                ViewBag.HomeBlog = oBLL.GetHomeBlog(this.GetUrlBase(_httpContextAccessor), ConstDTO.Blogs.ReservaCognitiva.ID);
            }

            return View("Home");
        }

        [Authorize(Roles = "Convidado")]
        /// <summary>
        /// Abre um registro de post
        /// </summary>
        /// <param name="p">IDPost - ID do post.</param>
        /// <param name="b">IDBlog - ID do blog.</param>
        /// <param name="f">FromPost - ID do post que estava aberto no momento que cliclou para ler este post.</param>
        [Authorize]
        public IActionResult Post(int p, int b, int? f = null)
        {
            PostDTO post = new PostDTO();

            string urlBase = this.GetUrlBase(_httpContextAccessor);

            string nrIP = "";

            using (BlogBLL oBLL = new BlogBLL())
            {
                post = oBLL.GetPost(urlBase, p, b, 1);

                if (_httpContextAccessor.HttpContext != null)
                {
                    nrIP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                }

                oBLL.AddPostView(p, nrIP);
            }

            ViewBag.Menu = new MenuPublicDTO("Blog");

            ViewBag.Post = post;

            return View("Post");
        }

        #region Tools

        private string GetUrlBase(IHttpContextAccessor httpContextAccessor)
        {
            var context = httpContextAccessor.HttpContext;

            if (context == null) return string.Empty;

            var request = context.Request;

            var urlBase = $"{request.Scheme}://{request.Host}";

            return urlBase;
        }

        #endregion
    }
}
