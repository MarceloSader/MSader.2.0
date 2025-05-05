using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSader.BLL;
using MSader.DTO;

namespace LinkWise.Controllers
{
    public class BlogController : Controller
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public BlogController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Abre um registro de post
        /// </summary>
        /// <param name="p">IDPost - ID do post.</param>
        /// <param name="b">IDBlog - ID do blog.</param>
        /// <param name="f">FromPost - ID do post que estava aberto no momento que cliclou para ler este post.</param>
        /// <returns></returns>
        public IActionResult Post(int p, int b, int? f = null)
        {
            PostDTO post = new PostDTO();

            string urlBase = this.GetUrlBase(_httpContextAccessor);

            string nrIP = "";

            using (BlogBLL oBLL = new BlogBLL())
            {
                post = oBLL.GetPost(urlBase, p, b, 0);

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

        /// <summary>
        /// Registra a ação de ir para a loja a partir de um post 
        /// </summary>
        public IActionResult FromPostToStore(int idPost, int? idCampaign = null, int? idProduct = null)
        {
            PostActionDTO postAction = new PostActionDTO(idPost, idCampaign, idProduct);

            using (BlogBLL oBLL = new BlogBLL())
            {
                oBLL.AddPostAction(postAction);
            }

            return Ok();
        }

        private string GetUrlBase(IHttpContextAccessor httpContextAccessor)
        {
            var context = httpContextAccessor.HttpContext;

            if (context == null) return string.Empty;

            var request = context.Request;

            var urlBase = $"{request.Scheme}://{request.Host}";

            return urlBase;
        }

        // Endpoints dos Blogs

        /// <summary>
        /// Linkwise Blog
        /// </summary>
        /// <returns></returns>
        public IActionResult Home()
        {
            ViewBag.Menu = new MenuPublicDTO("Blog");

            using (BlogBLL oBLL = new BlogBLL())
            {
                ViewBag.HomeBlog = oBLL.GetHomeBlog(this.GetUrlBase(_httpContextAccessor), ConstDTO.Blogs.LinkwiseBlog.ID);
            }

            return View("Home");
        }

        /// <summary>
        /// Blog Pessoal, para posts com acesso restrito
        /// </summary>
        [Authorize]   
        public IActionResult ReservaCognitiva()
        {

            ViewBag.Menu = new MenuPublicDTO("Blog");

            using (BlogBLL oBLL = new BlogBLL())
            {
                ViewBag.HomeBlog = oBLL.GetHomeBlog(this.GetUrlBase(_httpContextAccessor), ConstDTO.Blogs.ReservaCognitiva.ID);
            }

            return View("Home");
        }

        /// <summary>
        /// Abre um registro de post
        /// </summary>
        /// <param name="p">IDPost - ID do post.</param>
        /// <param name="b">IDBlog - ID do blog.</param>
        /// <param name="f">FromPost - ID do post que estava aberto no momento que cliclou para ler este post.</param>
        [Authorize]
        public IActionResult PostReservaCognitiva(int p, int b, int? f = null)
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
    }
}
