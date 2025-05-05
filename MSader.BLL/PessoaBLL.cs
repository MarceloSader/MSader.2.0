using MSader.DAL;
using MSader.DTO;

namespace MSader.BLL
{
    public class PessoaBLL : BaseBLL
    {
        public PessoaDTO GetPessoa(int idPessoa)
        {
            PessoaDTO pessoa = new PessoaDTO();

            using (PessoaDAL oDAL = new PessoaDAL())
            {
                pessoa = oDAL.GetPessoa(idPessoa);
            }

            return pessoa;
        }

        public PessoaDTO GetPessoa(string dsEmail, string cdChave)
        {
            PessoaDTO pessoa = new PessoaDTO();

            using (PessoaDAL oDAL = new PessoaDAL())
            {
                pessoa = oDAL.GetPessoa(dsEmail, cdChave);
            }

            return pessoa;
        }
    }
}
