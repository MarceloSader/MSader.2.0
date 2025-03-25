using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSader.DTO
{
    public class PessoaDTO
    {
        #region Propriedades 
        
        public int IDPessoa { get; set; }
        
        public string? CDChave { get; set; }
        
        public string? DSEmail { get; set; }
        
        public string? NMPessoa { get; set; }

        public bool STPessoaAtivo { get; set; }

        #endregion

        #region Construtores 

        public PessoaDTO()
        { }
        
        #endregion
        
        #region Métodos 
        #endregion
    }
}
