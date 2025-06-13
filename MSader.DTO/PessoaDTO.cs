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

        public DateTime DTCreatePessoa { get; set; }

        public DateTimeDTO? DTCreatePessoaTwo { get; set; }

        #endregion

        #region Construtores 

        public PessoaDTO()
        { }

        #endregion

        #region Métodos 
        #endregion
    }

    public class VisitanteDTO : PessoaDTO
    {
        #region Propriedades 

        public int IDVisitante { get; set; }

        public string? CDVisitante { get; set; }

        public DateTime DTCreateVisitante { get; set; }

        public DateTimeDTO? DTCreateVisitanteTwo { get; set; }

        public string? NRIP { get; set; }

        public bool STVisitanteAtivo { get; set; }

        public BoolDTO? STVisitanteAtivoTwo { get; set; }

        #endregion

        #region Construtores 

        public VisitanteDTO()
        { }

        public VisitanteDTO(int idPessoa)
        {
            IDPessoa = idPessoa;
        }

        public VisitanteDTO(string? cdVisitante, string? nmPessoa, string? dsEmail, string? nrIP)
        {

            CDVisitante = cdVisitante;
            NMPessoa = nmPessoa;
            DSEmail = dsEmail;
            DTCreateVisitante = DateTime.Now;
            DTCreateVisitanteTwo = new DateTimeDTO(DTCreateVisitante);
            DTCreatePessoa = DateTime.Now;
            DTCreatePessoaTwo = new DateTimeDTO(DTCreateVisitante);
            NRIP = nrIP;
            STVisitanteAtivo = true;
            STVisitanteAtivoTwo = new BoolDTO(STVisitanteAtivo);
        }

        #endregion

        #region Métodos 
        #endregion
    }
}
