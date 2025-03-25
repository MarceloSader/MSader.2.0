using System;
using System.Collections.Generic;

namespace MSader.DTO
{
    public class BlogDTO
    {
        #region Propriedades 
        
        public int IDBlog { get; set; }

        public string? NMBlog { get; set; }
        
        public string? NMAliasBlog { get; set; }
        
        public string? DSBlog { get; set; }
        
        public string? DSClassIcon { get; set; }
        
        public string? DSUrlBlog { get; set; }

        public int NROrdemBlog { get; set; }

        public DateTime DTCadastroBlog { get; set; }

        public bool STBlogAtivo { get; set; }

        public List<PostBlogDTO>? Posts  { get; set; }

        #endregion
       
        #region Construtores 
        
        public BlogDTO()
        { }
        
        #endregion
        
        #region Métodos 
        #endregion
    }

    public class PostDTO : TipoPostDTO
    {
        #region Propriedades 

        public int IDPessoa { get; set; }

        public int IDPost { get; set; }

        public string? DSAncoraPost { get; set; }

        public string? DSSubTituloPost { get; set; }

        public string? DSTags { get; set; }

        public string? DSTituloPost { get; set; }

        public string? DSTextoPost { get; set; }

        public bool STPostAtivo { get; set; }

        public DateTime DTCriacaoPost { get; set; }

        public DateTime DTPublicacaoPost { get; set; }

        public List<MidiaDTO>? Midias { get; set; }

        #endregion

        #region Construtores 

        public PostDTO()
        { }

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

        #region Métodos 
        #endregion
    }

    public class PostBlogDTO
    {
        #region Propriedades 
        public int IDBlog { get; set; }

        public int IDPost { get; set; }

        public int IDPostBlog { get; set; }

        #endregion

        #region Construtores 

        public PostBlogDTO()
        { }

        #endregion

        #region Métodos 
        #endregion
    }
}
