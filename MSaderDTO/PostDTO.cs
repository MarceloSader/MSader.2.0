using System;


namespace MSader.MSaderDTO
{
    public class PostDTO
    {
        #region Propriedades 

        public int IDPessoa { get; set; }

        public string NMPessoa { get; set; }

        public string CDChave { get; set; }

        public string DSEmail { get; set; }

        public bool STPessoaAtivo { get; set; }

        #endregion
        #region Construtores 

        public PostDTO()
        {
            IDPessoa = 0;
        }

        #endregion

        #region Métodos 

        #endregion
    }

    public class TipoPostDTO
    {
    }
}
