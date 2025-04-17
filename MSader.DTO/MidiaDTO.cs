using System;

namespace MSader.DTO
{
    public class MidiaDTO : TipoMidiaDTO
    {
        #region Propriedades 

        public int IDMidia { get; set; }

        public string? NMTitulo { get; set; }

        public string? CDEmbedded { get; set; }

        public string? DSLegenda { get; set; }

        public string? DSUrlMidia { get; set; }

        public int NROrdemPost { get; set; }

        #endregion

        #region Construtores 

        public MidiaDTO()
        { }

        public MidiaDTO(string urlBase, int idPost, int nrOrdemPost)
        {
            DSUrlMidia = $"{urlBase}/midia/posts/{idPost}/{idPost}-{nrOrdemPost}.png";

            NROrdemPost = nrOrdemPost ;
        }

        #endregion

            #region Métodos 
            #endregion
    }

    public class TipoMidiaDTO
    {
        #region Propriedades 
        public int IDTipoMidia { get; set; }
        
        public string? NMTipoMidia { get; set; }

        public string? CDExtencaoArquivo { get; set; }

        #endregion
        
        #region Construtores 
        
        public TipoMidiaDTO()
        { }
        
        #endregion
        
        #region Métodos 
        #endregion
    }
}
