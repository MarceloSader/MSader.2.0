using Dapper;
using Microsoft.Data.SqlClient;
using MSader.DTO;

namespace MSader.DAL
{
    public class PessoaDAL : BaseDAL
    {
        public PessoaDTO GetPessoa(int idPessoa)
        {
            PessoaDTO pessoa = new PessoaDTO();

            using (var connectionDB = new SqlConnection("Server=tcp:sql-msader-prd-01.database.windows.net,1433;Initial Catalog=sqldb-msader-prd-01;Persist Security Info=False;User ID=msader-operator;Password=CeHAd?ad8U;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                string query = @$"
                SELECT 
                     r.IDPessoa
                    ,r.NMPessoa
                    ,r.DSEmail
                    
                FROM       Pessoa r
                WHERE r.IDPessoa = {idPessoa}
                ";

                pessoa = connectionDB.Query<PessoaDTO>(query).First();
            }

            return pessoa;
        }

        public PessoaDTO GetPessoa(string dsEmail, string cdChave)
        {
            PessoaDTO? pessoa = new PessoaDTO();

            using (var connectionDB = new SqlConnection("Server=tcp:sql-msader-prd-01.database.windows.net,1433;Initial Catalog=sqldb-msader-prd-01;Persist Security Info=False;User ID=msader-operator;Password=CeHAd?ad8U;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                string query = @$"
                SELECT 
                     r.IDPessoa
                    ,r.NMPessoa
                    ,r.DSEmail
                    
                FROM       Pessoa r
                WHERE r.DSEmail = '{dsEmail}' AND  r.CDChave = '{cdChave}';
                ";

                pessoa = connectionDB.Query<PessoaDTO>(query).FirstOrDefault();
            }

            return pessoa;
        }
    }
}
