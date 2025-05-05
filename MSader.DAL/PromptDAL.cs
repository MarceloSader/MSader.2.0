using Dapper;
using Microsoft.Data.SqlClient;
using MSader.DTO;

namespace MSader.DAL
{
    public class PromptDAL : BaseDAL
    {
        public List<FormatoSaidaDTO> GetFormatosSaida()
        {
            List<FormatoSaidaDTO> formatos = new List<FormatoSaidaDTO>();

            using (var connectionDB = new SqlConnection("Server=tcp:sql-msader-prd-01.database.windows.net,1433;Initial Catalog=sqldb-msader-prd-01;Persist Security Info=False;User ID=msader-operator;Password=CeHAd?ad8U;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                string query = @$"
                SELECT 
                     r.IDFormatoSaida
                    ,r.NMFormatoSaida
                    ,r.STFormatoSaidaActive

                FROM       PR_FormatoSaida r
                ORDER BY r.NMFormatoSaida
                ";

                formatos = connectionDB.Query<FormatoSaidaDTO>(query).ToList();
            }

            return formatos;
        }

        public List<EstiloRespostaDTO> GetEstilosResposta()
        {
            List<EstiloRespostaDTO> estilos = new List<EstiloRespostaDTO>();

            using (var connectionDB = new SqlConnection("Server=tcp:sql-msader-prd-01.database.windows.net,1433;Initial Catalog=sqldb-msader-prd-01;Persist Security Info=False;User ID=msader-operator;Password=CeHAd?ad8U;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                string query = @$"
                SELECT 
                     r.IDEstiloResposta
                    ,r.NMEstiloResposta
                    ,r.STEstiloRespostaActive

                FROM       PR_EstiloResposta r
                ORDER BY r.NMEstiloResposta
                ";

                estilos = connectionDB.Query<EstiloRespostaDTO>(query).ToList();
            }

            return estilos;
        }

        public List<PromptPostGeneratorDTO> GetPromptsRequest(int idTipoPrompt)
        {
            List<PromptPostGeneratorDTO> prompts = new List<PromptPostGeneratorDTO>();

            using (var connectionDB = new SqlConnection("Server=tcp:sql-msader-prd-01.database.windows.net,1433;Initial Catalog=sqldb-msader-prd-01;Persist Security Info=False;User ID=msader-operator;Password=CeHAd?ad8U;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                string query = @$"
                SELECT 
                     r.IDPrompt
                    ,r.NMTitulo

                FROM       PR_Prompt r
                WHERE r.IDTipoPrompt = {idTipoPrompt} AND r.STPromptActive = 1
                ";

                prompts = connectionDB.Query<PromptPostGeneratorDTO>(query).ToList();
            }

            return prompts;
        }

        public PromptPostGeneratorDTO GetPromptRequest(int idPrompt)
        {
            PromptPostGeneratorDTO post = new PromptPostGeneratorDTO();

            using (var connectionDB = new SqlConnection("Server=tcp:sql-msader-prd-01.database.windows.net,1433;Initial Catalog=sqldb-msader-prd-01;Persist Security Info=False;User ID=msader-operator;Password=CeHAd?ad8U;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                string query = @$"
                SELECT 
                     r.IDPrompt
                    ,r.IDTipoPrompt
                    ,a.NMTipoPrompt
                    ,r.NMTitulo
                    ,r.DSObjetivo
                    ,r.DSContexto
                    ,r.IDEstiloResposta
                    ,c.NMEstiloResposta
                    ,r.IDVies
                    ,d.NMVies
                    ,r.DSPrompt
                    ,r.NRMaxTokens
                    ,r.VRTemperature
                    ,r.STPromptActive

                FROM       PR_Prompt r
                INNER JOIN PR_TipoPrompt     a ON r.IDTipoPrompt = a.IDTipoPrompt 
                INNER JOIN PR_EstiloResposta c ON r.IDEstiloResposta = c.IDEstiloResposta 
                INNER JOIN PR_Vies           d ON r.IDVies = d.IDVies 
                WHERE r.IDPrompt = {idPrompt}
                ";

                post = connectionDB.Query<PromptPostGeneratorDTO>(query).First();
            }

            return post;
        }

        public void AddPromptRequest(PromptPostGeneratorDTO prompt)
        {
            using (var connectionDB = new SqlConnection("Server=tcp:sql-msader-prd-01.database.windows.net,1433;Initial Catalog=sqldb-msader-prd-01;Persist Security Info=False;User ID=msader-operator;Password=CeHAd?ad8U;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                string query = @$"
                    INSERT INTO PR_Prompt
                    (
                         IDTipoPrompt
                        ,NMTitulo
                        ,DSObjetivo
                        ,DSContexto
                        ,IDEstiloResposta
                        ,IDVies
                        ,DSPrompt
                        ,NRMaxTokens
                        ,VRTemperature
                        ,STPromptActive
                    )
                    VALUES
                    (
                         {prompt.IDTipoPrompt}
                        ,'{prompt.NMTitulo}'
                        ,'{prompt.DSObjetivo}'
                        ,'{prompt.DSContexto}'
                        ,{prompt.IDEstiloResposta}
                        ,{prompt.IDVies}
                        ,'{prompt.DSPrompt}'
                        ,{prompt.NRMaxTokens}
                        ,{prompt.VRTemperature}
                        ,1
                    )
                ";

                connectionDB.Execute(query, prompt);
            }
        }

        public void UpdPromptRequest(PromptPostGeneratorDTO prompt)
        {
            using (var connectionDB = new SqlConnection("Server=tcp:sql-msader-prd-01.database.windows.net,1433;Initial Catalog=sqldb-msader-prd-01;Persist Security Info=False;User ID=msader-operator;Password=CeHAd?ad8U;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                string query = @$"
                    UPDATE PR_Prompt SET
                         IDTipoPrompt = {prompt.IDTipoPrompt}
                        ,NMTitulo = '{prompt.NMTitulo}'
                        ,DSObjetivo = '{prompt.DSObjetivo}'
                        ,DSContexto = '{prompt.DSContexto}'
                        ,IDEstiloResposta = {prompt.IDEstiloResposta}
                        ,IDVies = {prompt.IDVies}
                        ,DSPrompt = '{prompt.DSPrompt}'
                        ,VRTemperature = {prompt.VRTemperature.ToString().Replace(".","").Replace(",",".")}
                        ,NRMaxTokens = {prompt.NRMaxTokens}

                    WHERE IDPrompt = {prompt.IDPrompt}
                ";

                connectionDB.Execute(query, prompt);
            }
        }

        public List<ViesDTO> GetVieses()
        {
            List<ViesDTO> vieses = new List<ViesDTO>();

            using (var connectionDB = new SqlConnection("Server=tcp:sql-msader-prd-01.database.windows.net,1433;Initial Catalog=sqldb-msader-prd-01;Persist Security Info=False;User ID=msader-operator;Password=CeHAd?ad8U;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                string query = @$"
                SELECT 
                     r.IDVies
                    ,r.NMVies
                    ,r.STViesActive

                FROM       PR_Vies r
                ORDER BY r.NMVies
                ";

                vieses = connectionDB.Query<ViesDTO>(query).ToList();
            }

            return vieses;
        }
    }
}
