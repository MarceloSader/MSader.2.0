using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using MSader.BLL;
using MSader.DTO;

namespace LinkWise.Controllers
{
    public class BlogController : Controller
    {
        #region Propriedades

        string msgRetorno = string.Empty;
        string strStatus = "ERRO";

        #endregion

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
                ViewBag.HomeBlog = oBLL.GetHomeBlog(this.GetUrlBase(_httpContextAccessor), ConstDTO.Blogs.Linkwise.ID);
            }

            return View("Home");
        }

        // Endpoints de Comentários

        public IActionResult GetPostComments(int p)
        {
            List<PostCommentDTO> postComments = new List<PostCommentDTO>();

            using (BlogBLL oBLL = new BlogBLL())
            {
                postComments = oBLL.GetPostComments(p, this.GetUrlBase(_httpContextAccessor), ConstantsDTO.NR_POST_COMMENTS);
            }

            return Ok(new { PostComments = postComments });
        }

        /// <summary>
        /// Adiciona um comentário a partir do formulário.
        /// </summary>
        /// <param name="ipcp">IDPostCommentParent - Usado quando o visitante faz um replay de um comentário existente.</param>
        /// <param name="ipos">IDPost - ID do post</param>
        /// <param name="cvis">CDVisitante - Código do visitante, gerado no navegador e armazenado em local storage</param>
        /// <param name="dcom">DSComment - Texto do comentário.</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddPostComment(int ipcp, int ipos, string cvis, string nvis, string dema, string dcom, string tken)
        {
            string nrIP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (ValidarTokenCaptcha(tken))
            {
                List<PostCommentDTO> postComments = new List<PostCommentDTO>();

                PostCommentDTO postComment = new PostCommentDTO(ipcp, ipos, dcom, nrIP);

                VisitanteDTO visitante = new VisitanteDTO(cvis, nvis, dema, nrIP);

                using (BlogBLL oBLL = new BlogBLL())
                {
                    postComment.IDPostComment = oBLL.AddPostComment(postComment, visitante);
                }

                return Json(new { st = "OK", PostComment = postComment });
            }
            else
            {
                return Json(new { st = "ERRO", msg = "Solicitação recusada"});

            }
        }

        /// <summary>
        /// Adiciona um comentário a partir com base na Inteligência Artificial
        /// </summary>
        /// <param name="ipos">IDPost - ID do Post</param>
        /// <param name="idai">IDPessoa - ID da pessoa caracterizada como visitante, mas que neste caso é o personagem AI que irá gerar o comentário</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddPostCommentByAI(int ipos, int idai)
        {
            string nrIP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            List<PostCommentDTO> postComments = new List<PostCommentDTO>();

            PostCommentDTO postComment = new PostCommentDTO(ipos, idai, nrIP);

            VisitanteDTO visitante = new VisitanteDTO(idai);

            using (BlogBLL oBLL = new BlogBLL())
            {
                postComment.IDPostComment = oBLL.AddPostCommentByAI(postComment, visitante);
            }

            return Json(new { st = "OK", PostComment = postComment });
        }

        #region Captcha

        [HttpPost]
        public ActionResult GetToken()
        {

            CaptchaTokenDTO captcha = null;

            string nrIP = "";

            try
            {

                if (_httpContextAccessor.HttpContext != null)
                {
                    nrIP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                }

                // Instancia o captcha e gera um token para gravar na sessão.
                captcha = new CaptchaTokenDTO(nrIP);

                HttpContext.Session.SetString("CaptchaToken", captcha.TokenSaved.CDToken);

                strStatus = "OK";
            }
            catch (Exception ex)
            {
                msgRetorno = ex.Message;
            }

            return Json(new { msg = msgRetorno, tk = captcha.TokenSaved.CDToken, st = strStatus });
        }

        [HttpPost]
        public ActionResult CheckToken(string tkn)
        {

            try
            {
                var token = HttpContext.Session.GetString("CaptchaToken");

                string nrIP = "";

                if (_httpContextAccessor.HttpContext != null)
                {
                    nrIP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                }

                CaptchaTokenDTO captcha = new CaptchaTokenDTO(nrIP, tkn, token);

                if (!captcha.STValid)
                {
                    msgRetorno = "ERRO";
                }
            }
            catch (Exception ex)
            {
                strStatus = "ERRO";
                msgRetorno = ex.Message;
            }

            return Json(new { msg = msgRetorno, st = strStatus });
        }

        private bool ValidarTokenCaptcha(string tkn)
        {

            var token = HttpContext.Session.GetString("CaptchaToken");

            string nrIP = "";

            if (_httpContextAccessor.HttpContext != null)
            {
                nrIP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }

            CaptchaTokenDTO captcha = new CaptchaTokenDTO(nrIP, tkn, token);

            return captcha.STValid;
        }

        #endregion

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
