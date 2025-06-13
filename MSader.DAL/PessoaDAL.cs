using Dapper;
using Microsoft.Data.SqlClient;
using MSader.DTO;

namespace MSader.DAL
{
    public class PessoaDAL : BaseDAL
    {

        public int AddVisitante(VisitanteDTO visitante)
        {
            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
            {
                DateTime now = DateTime.Now;

                // Inserção em Pessoa
                string queryPessoa = @"
                    INSERT INTO Pessoa
                    (
                        NMPessoa,
                        DSEmail,
                        STPessoaAtivo,
                        DTCreatePessoa
                    )
                    VALUES
                    (
                        @NMPessoa,
                        @DSEmail,
                        @STPessoaAtivo,
                        @DTCreatePessoa
                    );
                    SELECT CAST(SCOPE_IDENTITY() AS INT);
                ";

                var parametrosPessoa = new
                {
                    NMPessoa = visitante.NMPessoa,
                    DSEmail = visitante.DSEmail,
                    STPessoaAtivo = 1,
                    DTCreatePessoa = visitante.DTCreatePessoaTwo.DSDateTimeSql
                };

                visitante.IDPessoa = connectionDB.ExecuteScalar<int>(queryPessoa, parametrosPessoa);

                // Inserção em Visitante
                string queryVisitante = @"
                    INSERT INTO Visitante
                    (
                        IDPessoa,
                        CDVisitante,
                        NRIP,
                        STVisitanteAtivo,
                        DTCreateVisitante
                    )
                    VALUES
                    (
                        @IDPessoa,
                        @CDVisitante,
                        @NRIP,
                        @STVisitanteAtivo,
                        @DTCreateVisitante
                    );
                    SELECT CAST(SCOPE_IDENTITY() AS INT);
                ";

                var parametrosVisitante = new
                {
                    IDPessoa = visitante.IDPessoa,
                    CDVisitante = visitante.CDVisitante,
                    NRIP = visitante.NRIP,
                    STVisitanteAtivo = visitante.STVisitanteAtivoTwo.CDBoolSql,
                    DTCreateVisitante = visitante.DTCreatePessoaTwo.DSDateTimeSql
                };

                visitante.IDVisitante = connectionDB.ExecuteScalar<int>(queryVisitante, parametrosVisitante);
            }

            return visitante.IDVisitante;
        }


        public int GetIDVisitante(string? cdVisitante)
        {
            int idVisitante = 0;

            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
            {
                string query = @$"
                SELECT 
                     r.IDVisitante
                    
                FROM       Visitante r
                WHERE r.CDVisitante = '{cdVisitante}';
                ";

                idVisitante = connectionDB.Query<int>(query).FirstOrDefault();
            }

            return idVisitante;
        }

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
