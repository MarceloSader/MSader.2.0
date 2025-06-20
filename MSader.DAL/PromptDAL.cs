using Dapper;
using Microsoft.Data.SqlClient;
using MSader.DTO;

namespace MSader.DAL
{
    public class PromptDAL : BaseDAL
    {
        // DIRETRIZES

        public List<FormatoSaidaDTO> GetFormatosSaida()
        {
            List<FormatoSaidaDTO> formatos = new List<FormatoSaidaDTO>();

            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
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

        public FormatoSaidaDTO GetFormatoSaida(int idFormatoSaida)
        {
            FormatoSaidaDTO formato = new FormatoSaidaDTO();

            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
            {
                string query = @$"
                SELECT 
                     r.IDFormatoSaida
                    ,r.NMFormatoSaida
                    ,r.DSDiretriz
                    ,r.STFormatoSaidaActive

                FROM       PR_FormatoSaida r
                WHERE r.IDFormatoSaida = {idFormatoSaida}
                ORDER BY r.NMFormatoSaida
                ";

                formato = connectionDB.Query<FormatoSaidaDTO>(query).FirstOrDefault();
            }

            return formato;
        }

        public List<EstiloRespostaDTO> GetEstilosResposta()
        {
            List<EstiloRespostaDTO> estilos = new List<EstiloRespostaDTO>();

            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
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

        public EstiloRespostaDTO GetEstiloResposta(int idEstiloResposta)
        {
            EstiloRespostaDTO estilo = new EstiloRespostaDTO();

            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
            {
                string query = @$"
                SELECT 
                     r.IDEstiloResposta
                    ,r.NMEstiloResposta
                    ,r.DSDiretriz
                    ,r.STEstiloRespostaActive

                FROM       PR_EstiloResposta r
                WHERE    r.IDEstiloResposta = {idEstiloResposta}
                ORDER BY r.NMEstiloResposta
                ";

                estilo = connectionDB.Query<EstiloRespostaDTO>(query).FirstOrDefault();
            }

            return estilo;
        }

        public List<ViesDTO> GetVieses(int idViesCategoria)
        {
            List<ViesDTO> vieses = new List<ViesDTO>();

            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
            {
                string query = @$"
                SELECT 
                     r.IDVies
                    ,r.NMVies

                FROM       PR_Vies r
                WHERE STViesActive = 1 AND IDViesCategoria = {idViesCategoria}
                ORDER BY r.NMVies
                ";

                vieses = connectionDB.Query<ViesDTO>(query).ToList();
            }

            return vieses;
        }

        public ViesDTO GetVies(int idVies)
        {
            ViesDTO vies = new ViesDTO();

            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
            {
                string query = @$"
                SELECT 
                     r.IDVies
                    ,r.NMVies
                    ,r.DSVies
                    ,r.DSDiretriz

                FROM       PR_Vies r
                WHERE IDVies = {idVies}
                ORDER BY r.NMVies
                ";

                vies = connectionDB.Query<ViesDTO>(query).FirstOrDefault();
            }

            return vies;
        }


        // PROMPT

        public List<PromptPostGeneratorDTO> GetPromptsRequest(int idTipoPrompt)
        {
            List<PromptPostGeneratorDTO> prompts = new List<PromptPostGeneratorDTO>();

            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
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

            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
            {
                string query = @$"
                SELECT 
                     r.IDPrompt
                    ,r.IDTipoPrompt
                    ,a.NMTipoPrompt
                    ,r.NMTitulo
                    ,r.DSObjetivo
                    ,r.DSPrompt
                    ,r.STPromptActive

                FROM       PR_Prompt r
                INNER JOIN PR_TipoPrompt     a ON r.IDTipoPrompt = a.IDTipoPrompt 
                WHERE r.IDPrompt = {idPrompt}
                ";

                post = connectionDB.Query<PromptPostGeneratorDTO>(query).First();
            }

            return post;
        }

        public void AddPromptRequest(PromptPostGeneratorDTO prompt)
        {
            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
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
            using (var connectionDB = new SqlConnection(ConstantsDTO.CONN_STRING))
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


    }
}
