
namespace MSader.DTO
{
    public class PostMinDTO : TipoPostDTO
    {
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

        public int IDPost { get; set; }
        
        public string? DSUrlPost { get; set; }

        public bool STPostAtivo { get; set; }

        public int STPostAtivoSql { get; set; }

        public DateTime DTCriacaoPost { get; set; }

        public DateTime DTPublicacaoPost { get; set; }

        public int NRPostViews { get; set; }

        public List<MidiaDTO>? Midias { get; set; }

        public List<PostBlogDTO>? PostsLinked { get; set; }

        #endregion

        #region Construtores 

        public PostDTO()
        { }

        public PostDTO(int idau, int idtp, string dsan, string dstp, string dsst, string dste, string dsta)
        {
            IDPessoa = idau;
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
        }

        public PostDTO(int idpos, int idau, int idtp, string dsan, string dstp, string dsst, string dste, string dsta)
        {
            IDPost = idpos;
            IDPessoa = idau;
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
        }

        #endregion

        #region Métodos 

        public void SetMidia(string urlBase, int nrOrdem)
        {
            Midias = new List<MidiaDTO>();

            Midias.Add(new MidiaDTO(urlBase, IDPost, nrOrdem));
        }

        public void SetUrlPost(string urlBase, int idBlog)
        {
            DSUrlPost = $"{urlBase}/Blog/Post?p={IDPost}&b={idBlog}";
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

        public int IDBlog { get; set; }

        public bool STHomeBlog { get; set; }

        public bool STHomeBlogCarousel { get; set; }

        public int NROrdemPost { get; set; }

        public int NROrdemPostCarousel { get; set; }


        #endregion

        #region Construtores 

        public PostBlogDTO()
        { }

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

}
