using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using MSader.DAL;
using MSader.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSader.BLL
{
    public class PromptBLL : BaseBLL
    {
        public List<FormatoSaidaDTO> GetFormatosSaida()
        {
            List<FormatoSaidaDTO> formatos = new List<FormatoSaidaDTO>();

            using (PromptDAL oDAL = new PromptDAL())
            {
                formatos = oDAL.GetFormatosSaida();
            }

            return formatos;
        }

        public List<EstiloRespostaDTO> GetEstilosResposta()
        {
            List<EstiloRespostaDTO> estilos = new List<EstiloRespostaDTO>();

            using (PromptDAL oDAL = new PromptDAL())
            {
                estilos = oDAL.GetEstilosResposta();
            }

            return estilos;
        }

        public List<ViesDTO> GetVieses()
        {
            List<ViesDTO> vieses = new List<ViesDTO>();

            using (PromptDAL oDAL = new PromptDAL())
            {
                vieses = oDAL.GetVieses();
            }

            return vieses;
        }

        public PromptPostGeneratorDTO GetPromptRequest(int idPrompt)
        {
            PromptPostGeneratorDTO post = new PromptPostGeneratorDTO();

            using (PromptDAL oDAL = new PromptDAL())
            {
                post = oDAL.GetPromptRequest(idPrompt);
            }

            return post;
        }

        public void SavePromptRequest(PromptPostGeneratorDTO prompt)
        {
            using (PromptDAL oDAL = new PromptDAL())
            {
                if (prompt.IDPrompt == 0)
                {
                    oDAL.AddPromptRequest(prompt);
                }
                else
                {
                    oDAL.UpdPromptRequest(prompt);
                }
            }
        }

        public List<PromptPostGeneratorDTO> GetPromptsRequest(int idTipoPrompt)
        {
            List<PromptPostGeneratorDTO> prompts = new List<PromptPostGeneratorDTO>();

            using (PromptDAL oDAL = new PromptDAL())
            {
                prompts = oDAL.GetPromptsRequest(idTipoPrompt);
            }

            return prompts;
        }
    }
}
