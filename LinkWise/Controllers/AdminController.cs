using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSader.DTO;
using MSader.BLL;
using LinkWise.Helpers;
using System.Text.Json;
using System.Text;
using System.Diagnostics;

namespace LinkWise.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        string msgReturn = "";

        string stStatus = "";


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

            ViewBag.SimNao = ListHelper.GetListSimNao();

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
        public IActionResult Posts(int idb)
        {
            ViewBag.Menu = new MenuAdminDTO("Blogs");

            ViewBag.TiposMidia = ListHelper.GetListTipoMidia();

            ViewBag.IDBlog = idb;

            return View("Posts");
        }

        [HttpGet]
        public IActionResult GetPosts(int idb)
        {
            List<PostDTO> posts = new List<PostDTO>();

            using (BlogBLL oBLL = new BlogBLL())
            {
                posts = oBLL.GetPosts(this.GetUrlBase(_httpContextAccessor), ConstantsDTO.NR_POSTS, idb);
            }

            return Ok(new { Posts = posts });
        }

        [HttpGet]
        public IActionResult GetPost(int idp)
        {
            var post = new PostDTO();

            using (BlogBLL oBLL = new BlogBLL())
            {
                if (idp == 0)
                {
                    post.SetNewPost();
                }
                else
                {
                    post = oBLL.GetPostAdmin(this.GetUrlBase(_httpContextAccessor), idp);
                }
            }

            return Ok(new { Post = post });
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

        [HttpGet]
        public IActionResult Post(int idp = 0)
        {
            ViewBag.Menu = new MenuAdminDTO("Blogs");

            ViewBag.TiposPost = ListHelper.GetListTipoPost();

            ViewBag.SimNao = ListHelper.GetListSimNao();

            ViewBag.Pessoas = ListHelper.GetListPessoas();

            ViewBag.Blogs = ListHelper.GetListBlogs();

            ViewBag.IDPost = idp;


            return View("Post");
        }

        // CONTENT GENERATOR

        [HttpPost]
        public IActionResult GetPrompt(int idp)
        {

            PromptPostGeneratorDTO prompt = new PromptPostGeneratorDTO();

            using (PromptBLL oBLL = new PromptBLL())
            {
                prompt = oBLL.GetPromptRequest(idp);
            }

            return Json(prompt);
        }

        [HttpPost]
        public async Task<IActionResult> RunContentGeneratorOpenAI(int idp, int idtpr, string dst, string dsu, string nmer, string nmv, string nmt, string dso, string dscon, string dsp, string dscom, int nrm, double vrt)
        {

            try
            {
                var prompt = new PromptPostGeneratorDTO(idp, idtpr, dst, dsu, nmer, nmv, nmt, dso, dscon, dsp, dscom, nrm, vrt);

                // Aqui você pode montar o prompt final com base no DTO
                var promptMontado = MontarPromptPostGenerator(prompt);

                var oHelper = new AIHelper();

                // Chamada do método que envia o prompt para a OpenAI
                var resposta = await oHelper.ObterRespostaDaOpenAIAsync(promptMontado, prompt.NRMaxTokens, prompt.VRTemperature);

                var post = JsonSerializer.Deserialize<dynamic>(resposta, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Ok(new { res = post });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // Ou use um logger
                return StatusCode(500, new { error = ex.Message });
            }

        }

        [HttpPost]
        public IActionResult SavePrompt(int idp, int idt, int ide, int idv, string nmt, string dso, string dsc, string dsp, int nrm, double vrt)
        {
            PromptPostGeneratorDTO prompt = new PromptPostGeneratorDTO(idp, idt, ide, idv, nmt, dso, dsc, dsp, nrm, vrt);

            using (PromptBLL oBLL = new PromptBLL())
            {
                oBLL.SavePromptRequest(prompt);
            }
            return Json(new { st = "OK" });
        }

        [HttpPost]
        public IActionResult SavePost(int idau, int idbl, int idtp, string dsan, string dstp, string dsst, string dste, string dsta, bool star)
        {
            int idPost = 0;

            PostDTO post = new PostDTO(idau, idbl, idtp, dsan, dstp, dsst, dste, dsta, star);

            PostBlogDTO postBlog = new PostBlogDTO(idbl);

            using (BlogBLL oBLL = new BlogBLL())
            {
                idPost = oBLL.AddPost(post, postBlog);
            }

            return Json(new { st = "OK", IDPost = idPost });
        }

        [HttpPost]
        public IActionResult SavePostTwo(int idpo, int idbl, int idau, int idtp, string dsan, string dstp, string dsst, string dste, string dsta, int star, int stpa, string dtcr, string dtpu)
        {

            PostDTO post = new PostDTO(idpo, idbl, idau, idtp, dsan, dstp, dsst, dste, dsta, star, stpa, dtcr, dtpu);

            PostBlogDTO postBlog = new PostBlogDTO(idbl);

            using (BlogBLL oBLL = new BlogBLL())
            {
                if (post.IDPost == 0)
                {
                    post.IDPost = oBLL.AddPost(post, postBlog);
                }
                else
                {
                    oBLL.UpdPost(post);
                }
            }

            return Json(new { st = "OK", IDPost = post.IDPost });
        }

        [HttpPost]
        public IActionResult SetMidiaMain(int idpm, int idm, int idp)
        {
            using (BlogBLL oBLL = new BlogBLL())
            {
                oBLL.SetMidiaMain(idpm, idm, idp);
            }
            return Ok(new { st = "OK" });
        }

        [HttpPost]
        public IActionResult DelMidia(int idpm, int idm)
        {
            using (BlogBLL oBLL = new BlogBLL())
            {
                oBLL.DelMidia(idpm, idm);
            }
            return Ok(new { st = "OK" });
        }

        [HttpPost]
        public IActionResult SetPostMidiaOrdem(int idpm, int nro)
        {
            using (BlogBLL oBLL = new BlogBLL())
            {
                oBLL.SetPostMidiaOrdem(idpm, nro);
            }
            return Ok(new { st = "OK" });
        }

        // IMAGENS

        [HttpPost]
        public async Task<IActionResult> SaveImagemPost(List<IFormFile> files, int idp, string nmt, string dsl, string cde, [FromServices] IWebHostEnvironment env)
        {
            MidiaDTO midia = null;
            
            string midiaFileName = "";

            try
            {
                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        string[] fileParts = file.FileName.Split('.');

                        if (file == null || file.Length == 0)
                        {
                            msgReturn = "Problemas com o arquivo enviado.";
                        }
                        else if (file.Length >= ConstantsDTO.FOTO_MAX_LENGTH)
                        {
                            msgReturn = $"Arquivo excede o tamanho máximo permitido ({ConstantsDTO.FOTO_MAX_LENGTH:N} bytes);";
                        }
                        else
                        {
                            using (var ms = new MemoryStream())
                            {
                                await file.CopyToAsync(ms);
                                byte[] fileBytes = ms.ToArray();

                                using (BlogBLL oBLL = new BlogBLL())
                                {
                                    midiaFileName = oBLL.GetMidiaFileName(idp, file.FileName);
                                }

                                midia = new MidiaDTO(midiaFileName, fileBytes, fileParts.Last(), nmt, dsl, cde);

                                // Define o nome da pasta
                                midia.SetFolderName(idp);

                                if (midia.IGArquivo == null || midia.IGArquivo.Length == 0)
                                {
                                    Console.WriteLine("Arquivo vazio. Nada será gravado.");
                                }

                                if (!string.IsNullOrWhiteSpace(midia.FolderName) && midia.NMFileName != null && midia.IGArquivo != null)
                                {
                                    string folderPath = Path.Combine(env.WebRootPath, midia.FolderName);

                                    // Cria a pasta se ela não existir
                                    if (!Directory.Exists(folderPath))
                                    {
                                        Directory.CreateDirectory(folderPath);
                                    }

                                    // Define o caminho completo do arquivo com nome
                                    string filePath = Path.Combine(folderPath, midia.NMFileName); // ou use midia.FileName se você tiver isso

                                    try
                                    {
                                        // Salva o arquivo físico no disco
                                        await System.IO.File.WriteAllBytesAsync(filePath, midia.IGArquivo); // midia.Dados == byte[]


                                        if (System.IO.File.Exists(filePath))
                                        {
                                            Debug.WriteLine("Arquivo gravado com sucesso!");
                                        }
                                        else
                                        {
                                            Debug.WriteLine("Atenção: arquivo NÃO foi encontrado após tentativa de gravação.");
                                        }

                                        // Atualiza o caminho no DTO, se necessário
                                        midia.FullPath = filePath;
                                    }
                                    catch (Exception ex)
                                    {
                                        msgReturn = "Erro ao salvar o arquivo físico: " + ex.Message;
                                    }

                                }
                            }

                            using (BlogBLL oBLL = new BlogBLL())
                            {
                                oBLL.AddMidiaPost(midia, idp);
                            }

                            stStatus = "OK";
                        }
                    }
                }
                else
                {
                    msgReturn = "Nenhum arquivo enviado!";
                }
            }
            catch (Exception ex)
            {
                msgReturn = "Erro ao enviar arquivos. Por favor, tente novamente mais tarde.";
            }

            return Json(new { msg = msgReturn, st = stStatus });
        }

        [HttpPost]
        public IActionResult UpdMidia(int idm, string nmt, string dsl, string cde)
        {
            using (BlogBLL oBLL = new BlogBLL())
            {
                oBLL.UpdMidia(idm, nmt, dsl, cde);
            }
            return Ok(new { st = "OK" });
        }

        // HELPERS
        private string MontarPromptPostGenerator(PromptPostGeneratorDTO prompt)
        {

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Contexto: {prompt.DSContexto}");
            sb.AppendLine($"Tema/Assunto: {prompt.DSTema}");
            sb.AppendLine($"Fonte: {prompt.DSUrlSource}");
            sb.AppendLine($"Objetivo: {prompt.DSObjetivo}");
            sb.AppendLine("Tarefa: ");
            sb.AppendLine($"{prompt.DSPrompt}");
            sb.AppendLine("");
            sb.AppendLine($"Observações Complementares: {prompt.DSComplemento}");
            sb.AppendLine("");
            sb.AppendLine("Formato de saída:");
            sb.AppendLine("Gerar um único json estruturado da seguinte maneira:");
            sb.AppendLine("");
            sb.AppendLine("{");
            sb.AppendLine("\"DSAncoraPost\": \"\", ");
            sb.AppendLine("\"DSTituloPost\": \"\", ");
            sb.AppendLine("\"DSSubTituloPost\": \"\", ");
            sb.AppendLine("\"DSTextoPost\": \"\", ");
            sb.AppendLine("\"DSTags\": \"\"");
            sb.AppendLine("}");

            return sb.ToString();
        }
    }

    //public IActionResult GetTextFromSpeechAudio()
    //{
    //    string transcribed = "";

    //    long size = files.Sum(f => f.Length);

    //    foreach (var formFile in files)
    //    {
    //        if (formFile.Length > 0)
    //        {
    //            var filePath = Path.GetTempFileName();

    //            using (var stream = System.IO.File.Create(filePath))
    //            {
    //                await formFile.CopyToAsync(stream);
    //            }
    //        }
    //    }

    //    Process uploaded files
    //    Don't rely on or trust the FileName property without validation.

    //    return Ok(new { count = files.Count, size });
    //}
}

