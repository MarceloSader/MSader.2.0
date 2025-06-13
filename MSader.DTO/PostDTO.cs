
using System.Diagnostics.Contracts;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography;
using System.Globalization;

namespace MSader.DTO
{
    public class PostMinDTO : TipoPostDTO
    {

        public int IDPost { get; set; }

        public int IDBlog { get; set; }

        public string? DSTituloPost { get; set; }

        public string? DSAncoraPost { get; set; }

        public string? DSSubTituloPost { get; set; }

        public string? DSTextoPost { get; set; }

        public string? DSTags { get; set; }
    }

    public class PostDTO : PostMinDTO
    {
        #region Propriedades 

        public int IDPessoa { get; set; }

        public string? NMPessoa { get; set; }
        
        public string? DSUrlPost { get; set; }

        public bool STPostAtivo { get; set; }

        public int STPostAtivoSql { get; set; }

        public BoolDTO? STPostAtivoTwo { get; set; }

        public DateTime DTCriacaoPost { get; set; }

        public DateTime DTPublicacaoPost { get; set; }

        public DateTimeDTO? DTCriacaoPostTwo { get; set; }

        public DateTimeDTO? DTPublicacaoPostTwo { get; set; }

        public int NRPostViews { get; set; }

        public List<MidiaDTO>? Midias { get; set; }

        public List<PostBlogDTO>? PostsLinked { get; set; }

        public bool? STAcessoRestrito { get; set; }

        public int? STAcessoRestritoSql { get; set; }

        public BoolDTO? STAcessoRestritoTwo { get; set; }

        #endregion

        #region Construtores 

        public PostDTO()
        { }

        public PostDTO(int idau, int idbl, int idtp, string dsan, string dstp, string dsst, string dste, string dsta, bool? star)
        {
            IDPessoa = idau;
            IDBlog = idbl;
            IDTipoPost = idtp;
            DSAncoraPost = dsan;
            DSTituloPost = dstp;
            DSSubTituloPost = dsst;
            DSTextoPost = dste;
            DSTags = dsta;
            STPostAtivo = true;
            STPostAtivoSql = 1;
            DTCriacaoPost = DateTime.Now;
            DTPublicacaoPost = DateTime.Now;
            STAcessoRestrito = star;
            STAcessoRestritoTwo = new BoolDTO(star);
            STAcessoRestritoSql = 0;
            if (star != null && star == true)
            {
                STAcessoRestritoSql = 1;
            }

        }

        public PostDTO(int idpo, int idbl, int idau, int idtp, string dsan, string dstp, string dsst, string dste, string dsta, int stAcessoRestrito, int stPostAtivo, string dtcr, string dtpu)
        {

            bool star = true;
            bool stpa = true;

            if (stAcessoRestrito == 0)
            {
                star = false;
            }

            if (stPostAtivo == 0)
            {
                stpa = false;
            }

            IDPost = idpo;
            IDBlog = idbl;
            IDPessoa = idau;
            IDTipoPost = idtp;
            DSAncoraPost = dsan;
            DSTituloPost = dstp;
            DSSubTituloPost = dsst;
            DSTextoPost = dste;
            DSTags = dsta;
            STPostAtivoSql = 1;
            DTCriacaoPost = DateTime.ParseExact(dtcr, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DTPublicacaoPost = DateTime.ParseExact(dtpu, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            STAcessoRestrito = star;
            STAcessoRestritoTwo = new BoolDTO(star);
            STAcessoRestritoSql = 0;
            if (star == true)
            {
                STAcessoRestritoSql = 1;
            }
            STPostAtivo = stpa;
            STPostAtivoTwo = new BoolDTO(stpa);
            STPostAtivoSql = 0;
            if (stpa == true)
            {
                STPostAtivoSql = 1;
            }

        }


        #endregion

        #region Métodos 

        public void SetUrlPost(string urlBase, int idBlog)
        {
            DSUrlPost = $"{urlBase}/Blog/Post?p={IDPost}&b={idBlog}";
        }

        public void SetNewPost()
        {
            IDPost = 0; 
            IDPessoa = ConstDTO.Autor.Marcelo.ID;
            IDTipoPost = ConstDTO.TipoPost.Texto.ID;
            DSAncoraPost = "Âncora";
            DSTituloPost = "Título";
            DSSubTituloPost = "Subtítulo";
            DSTextoPost = "Texto";
            DSTags = "Tags";
            STPostAtivo = true;
            STPostAtivoSql = 1;
            STAcessoRestrito = false;
            STAcessoRestritoSql = 0;
            DTCriacaoPost = DateTime.Now;
            DTPublicacaoPost = DateTime.Now;
            DTCriacaoPostTwo = new DateTimeDTO(DateTime.Now);
            DTPublicacaoPostTwo = new DateTimeDTO(DateTime.Now);
        }

        public void SetDetails()
        {
            DTCriacaoPostTwo = new DateTimeDTO(DTCriacaoPost);
            DTPublicacaoPostTwo = new DateTimeDTO(DTPublicacaoPost);
            STAcessoRestritoSql = 0;
            STPostAtivoSql = 0;

            if (STAcessoRestrito == true) STAcessoRestritoSql = 1;
            if (STPostAtivo == true) STPostAtivoSql = 1;

            STAcessoRestritoTwo = new BoolDTO(STAcessoRestrito);
            STPostAtivoTwo = new BoolDTO(STPostAtivo);
        }

        #endregion
    }

    public class TipoPostDTO
    {
        #region Propriedades 

        public int IDTipoPost { get; set; }

        public string? NMTipoPost { get; set; }

        public bool STTipoPostAtivo { get; set; }

        #endregion

        #region Construtores 

        public TipoPostDTO()
        { }

        #endregion

    }

    public class PostBlogDTO : PostDTO
    {
        #region Propriedades 

        public int IDPostBlog { get; set; }

        public bool STHomeBlog { get; set; }

        public bool STHomeBlogCarousel { get; set; }

        public BoolDTO? STHomeBlogTwo  { get; set; }

        public BoolDTO? STHomeBlogCarouselTwo { get; set; }

        public int NROrdemPost { get; set; }

        public int NROrdemPostCarousel { get; set; }


        #endregion

        #region Construtores 

        public PostBlogDTO()
        { }

        public PostBlogDTO(int idBlog)
        {
            IDBlog = idBlog;

            STHomeBlog = true;

            STHomeBlogTwo = new BoolDTO(STHomeBlog);

            STHomeBlogCarousel = false;

            STHomeBlogCarouselTwo = new BoolDTO(STHomeBlogCarousel);

            NROrdemPost = 0;

            NROrdemPostCarousel = 0;
        }

        #endregion

        #region Métodos 

        #endregion
    }

    public class PostViewDTO
    {
        #region Propriedades 

        public int IDPostView { get; set; }

        public int IDPost { get; set; }

        public string? NRIP { get; set; }

        public DateTime DTView { get; set; }

        #endregion

        #region Construtores 

        public PostViewDTO()
        { }

        #endregion

        #region Métodos 

        #endregion
    }

    public class PostVoteDownDTO
    {
        #region Propriedades 

        public int IDPostVoteDown { get; set; }

        public int IDPost { get; set; }

        public string? NRIP { get; set; }

        public DateTime DTVoteDown { get; set; }

        #endregion

        #region Construtores 

        public PostVoteDownDTO()
        { }

        #endregion

        #region Métodos 

        #endregion
    }

    public class PostVoteUpDTO
    {
        #region Propriedades 

        public int IDPostVoteUp { get; set; }

        public int IDPost { get; set; }

        public string? NRIP { get; set; }

        public DateTime DTVoteUp { get; set; }

        #endregion

        #region Construtores 

        public PostVoteUpDTO()
        { }

        #endregion

        #region Métodos 

        #endregion
    }

    public class PostActionDTO
    {
        #region Propriedades 


        public int IDPostAction { get; set; }
        public int IDPostFrom { get; set; }
        public int? IDPostTo { get; set; }
        public int? IDProduct { get; set; }
        public int? IDCampaign { get; set; }
        public bool STWentToPost { get; set; }
        public bool STWentToStore { get; set; }
        public DateTime DTAction { get; set; }

        #endregion

        #region Construtores 

        public PostActionDTO()
        { }

        public PostActionDTO(int idPost, int? idCampaign = null, int? idProduct = null)
        {
            IDPostFrom = idPost;
            IDPostTo = null;
            IDCampaign = idCampaign;
            IDProduct = idProduct;
            DTAction = DateTime.Now;
            STWentToPost = false;
            STWentToStore = false;

            if (idCampaign != null || idProduct != null)
            {
                STWentToStore = true;
            }
        }

        #endregion

        #region Métodos 
        #endregion
    }

    public class PostCommentDTO 
    {
        #region Propriedades 

        public int IDPostComment { get; set; }

        public int? IDPostCommentParent { get; set; }

        public int IDPost { get; set; }

        public int IDPessoa { get; set; }

        public string NMPessoa { get; set; }

        public bool STPostCommentAtivo { get; set; }

        public BoolDTO? STPostCommentAtivoTwo { get; set; }

        public DateTime DTComment { get; set; }

        public DateTimeDTO? DTCommentTwo { get; set; }

        public string? DSComment { get; set; }

        public string? DSUrlAvatar { get; set; }

        public string? NRIP { get; set; }

        public List<PostCommentDTO> PostCommentsChildren { get; set; }

        #endregion

        #region Construtores 

        public PostCommentDTO()
        { }

        public PostCommentDTO(int idPost, int idai, string nrIp)
        {

            IDPostCommentParent = 0;
            IDPost = idPost;
            STPostCommentAtivo = true;
            STPostCommentAtivoTwo = new BoolDTO(STPostCommentAtivo);
            DTComment = DateTime.Now;
            DTCommentTwo = new DateTimeDTO(DTComment);
            DSComment = "A ser preenchido por AI";
            NRIP = nrIp;
        }

        public PostCommentDTO(int idPostCommentParent, int idPost, string dsComment, string nrIP)
        {

            IDPostCommentParent = idPostCommentParent;
            IDPost = idPost;
            STPostCommentAtivo = true;
            STPostCommentAtivoTwo = new BoolDTO(STPostCommentAtivo);
            DTComment = DateTime.Now;
            DTCommentTwo = new DateTimeDTO(DTComment);
            DSComment = dsComment;
            NRIP = nrIP;
        }

        #endregion

        #region Métodos 
        #endregion
    }

}
