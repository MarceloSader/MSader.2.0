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

        public DateTime DTCadastroBlog { get; set; }

        public bool STBlogAtivo { get; set; }

        public List<PostBlogDTO>? Posts { get; set; }

        #endregion

        #region Construtores 

        public BlogDTO()
        { }

        #endregion

        #region Métodos 
        #endregion
    }

    public class HomeBlogDTO
    {
        #region Propriedades 

        public int IDBlog { get; set; }

        public List<PostBlogDTO>? Posts { get; set; }

        public List<PostBlogDTO>? PostsCarousel { get; set; }

        #endregion

        #region Construtores 

        public HomeBlogDTO()
        { }

        #endregion

        #region Métodos 

        public void SetLinks(string urlBase, int idBlog, int nrOrdem)
        {
            foreach (var post in Posts ?? Enumerable.Empty<PostBlogDTO>())
            {
                post.SetMidia(urlBase, nrOrdem);
                post.SetUrlPost(urlBase, idBlog);
            }

            foreach (var post in PostsCarousel ?? Enumerable.Empty<PostBlogDTO>())
            {
                post.SetMidia(urlBase, nrOrdem);
                post.SetUrlPost(urlBase, idBlog);
            }
        }

        #endregion
    }

    public class PostDTO : TipoPostDTO
    {
        #region Propriedades 

        public int IDPessoa { get; set; }

        public string? NMPessoa { get; set; }

        public int IDPost { get; set; }

        public string? DSAncoraPost { get; set; }

        public string? DSSubTituloPost { get; set; }

        public string? DSTags { get; set; }

        public string? DSTituloPost { get; set; }

        public string? DSTextoPost { get; set; }

        public string? DSUrlPost { get; set; }

        public bool STPostAtivo { get; set; }

        public DateTime DTCriacaoPost { get; set; }

        public DateTime DTPublicacaoPost { get; set; }

        public int NRPostViews { get; set; }

        public List<MidiaDTO>? Midias { get; set; }

        #endregion

        #region Construtores 

        public PostDTO()
        { }

        #endregion

        #region Métodos 

        #region Métodos 

        public void SetMidia(string urlBase, int nrOrdem)
        {
            Midias = new List<MidiaDTO>();

            Midias.Add(new MidiaDTO(urlBase, IDPost, nrOrdem));
        }

        public void SetUrlPost(string urlBase, int idBlog)
        {
            DSUrlPost = $"{urlBase}/home/post?p={IDPost}&b={idBlog}";
        }

        #endregion

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
}
