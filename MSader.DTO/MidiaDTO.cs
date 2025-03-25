using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSader.DTO
{
    public class MidiaDTO : TipoMidiaDTO
    {
        #region Propriedades 
        public int IDMidia { get; set; }

        public string? NMTitulo { get; set; }

        public string? CDEmbedded { get; set; }

        public string? DSLegenda { get; set; }

        #endregion

        #region Construtores 

        public MidiaDTO()
        { }

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
