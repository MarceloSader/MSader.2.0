using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.SqlServer.Server;
using MSader.BLL;
using MSader.DTO;

namespace MSader.Helpers
{
    public class ListHelper
    {
        /// <summary>
        /// Obém uma collection do tipo selectlist para opções de formatos de saída de um prompt.
        /// </summary>
        /// <returns>SelectList com as opções de formato de saída.</returns>
        public static SelectList GetListFormatosSaida()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem() { Text = ":: SELECIONE ::", Value = "0", Selected = false });

            List<FormatoSaidaDTO> formatos = [];

            using (PromptBLL oBLL = new PromptBLL())
            {
                formatos = oBLL.GetFormatosSaida();

                try
                {
                    foreach (FormatoSaidaDTO formato in formatos)
                    {
                        items.Add(new SelectListItem() { Text = formato.NMFormatoSaida, Value = formato.IDFormatoSaida.ToString(), Selected = false });
                    }

                    return new SelectList(items, "Value", "Text");
                }
                catch 
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Obém uma collection do tipo selectlist para opções de estilos de saída de um prompt.
        /// </summary>
        /// <returns>SelectList com as opções de estilos de saída.</returns>
        public static SelectList GetListEstilosResposta()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem() { Text = ":: SELECIONE ::", Value = "0", Selected = false });

            List<EstiloRespostaDTO> formatos = [];

            using (PromptBLL oBLL = new PromptBLL())
            {
                formatos = oBLL.GetEstilosResposta();

                try
                {
                    foreach (EstiloRespostaDTO formato in formatos)
                    {
                        items.Add(new SelectListItem() { Text = formato.NMEstiloResposta, Value = formato.IDEstiloResposta.ToString(), Selected = false });
                    }

                    return new SelectList(items, "Value", "Text");
                }
                catch
                {
                    throw;
                }
            }
        }

        public static SelectList GetListTipoPost()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem() { Text = ":: SELECIONE ::", Value = "0", Selected = false });

            List<TipoPostDTO> tipos = [];

            using (BlogBLL oBLL = new BlogBLL())
            {
                tipos = oBLL.GetTiposPost();

                try
                {
                    foreach (TipoPostDTO tipo in tipos)
                    {
                        items.Add(new SelectListItem() { Text = tipo.NMTipoPost, Value = tipo.IDTipoPost.ToString(), Selected = false });
                    }

                    return new SelectList(items, "Value", "Text");
                }
                catch
                {
                    throw;
                }
            }
        }

        public static SelectList GetListPessoas()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem() { Text = ":: SELECIONE ::", Value = "0", Selected = false });

            List<PessoaDTO> pessoas = [];

            using (BlogBLL oBLL = new BlogBLL())
            {
                pessoas = oBLL.GetPessoas();

                try
                {
                    foreach (PessoaDTO pessoa in pessoas)
                    {
                        items.Add(new SelectListItem() { Text = pessoa.NMPessoa, Value = pessoa.IDPessoa.ToString(), Selected = false });
                    }

                    return new SelectList(items, "Value", "Text");
                }
                catch
                {
                    throw;
                }
            }
        }

        public static SelectList GetListVieses()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem() { Text = ":: SELECIONE ::", Value = "0", Selected = false });

            List<ViesDTO> vieses = [];

            using (PromptBLL oBLL = new PromptBLL())
            {
                vieses = oBLL.GetVieses();

                try
                {
                    foreach (ViesDTO vies in vieses)
                    {
                        items.Add(new SelectListItem() { Text = vies.NMVies, Value = vies.IDVies.ToString(), Selected = false });
                    }

                    return new SelectList(items, "Value", "Text");
                }
                catch
                {
                    throw;
                }
            }
        }

        public static SelectList GetListPrompts(int idTipoPrompt)
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem() { Text = ":: SELECIONE ::", Value = "0", Selected = false });

            List<PromptPostGeneratorDTO> prompts = [];

            using (PromptBLL oBLL = new PromptBLL())
            {
                prompts = oBLL.GetPromptsRequest(idTipoPrompt);

                try
                {
                    foreach (PromptPostGeneratorDTO prompt in prompts)
                    {
                        items.Add(new SelectListItem() { Text = prompt.NMTitulo, Value = prompt.IDPrompt.ToString(), Selected = false });
                    }

                    return new SelectList(items, "Value", "Text");
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
