
using System.Security.Cryptography;

namespace MSader.DTO
{
    public class PromptPostGeneratorDTO
    {
        #region Properties

        /// <summary>
        /// ID do Prompt.
        /// </summary>
        public int IDPrompt { get; set; }

        /// <summary>
        /// ID do Tipo do Prompt
        /// </summary>
        public int IDTipoPrompt { get; set; }

        /// <summary>
        /// ID do Tipo do Prompt
        /// </summary>
        public string? NMTipoPrompt { get; set; }

        /// <summary>
        /// Descrição do tema principal ou assunto do post a ser gerado.
        /// </summary>
        public string? DSTema { get; set; }

        /// <summary>
        /// URL da fonte onde será feita a consulta para coleta de informações para gerar o post.
        /// </summary>
        public string? DSUrlSource { get; set; }

        /// <summary>
        /// Um nome ou descrição curta para identificar o prompt.
        /// </summary>
        public string? NMTitulo { get; set; }

        /// <summary>
        /// Informações complementares enviadas pelo usuário no momento de submeter o prompt.
        /// </summary>
        public string? DSComplemento { get; set; }

        /// <summary>
        /// O que se espera que o conteúdo gerado faça ou comunique.
        /// </summary>
        public string? DSObjetivo { get; set; }

        /// <summary>
        /// Informações adicionais que ajudam a dar mais precisão à resposta da IA.
        /// </summary>
        public string? DSContexto { get; set; }

        /// <summary>
        /// Estilo da resposta esperada, como "formal", "técnico", "simplificado", etc.
        /// </summary>
        public int IDEstiloResposta { get; set; }

        /// <summary>
        /// Estilo da resposta esperada, como "formal", "técnico", "simplificado", etc.
        /// </summary>
        public string? NMEstiloResposta { get; set; }

        /// <summary>
        /// ID do Viés da pesquisa.
        /// </summary>
        public int IDVies { get; set; }

        /// <summary>
        /// Nome do Viés da pesquisa.
        /// </summary>
        public string? NMVies { get; set; }

        /// <summary>
        /// O conteúdo base do prompt que será enviado para a IA.
        /// </summary>
        public string? DSPrompt { get; set; }

        /// <summary>
        /// Número máximo de tokens (limite de tamanho da resposta). Default: 1000.
        /// </summary>
        public int NRMaxTokens { get; set; }

        /// <summary>
        /// Grau de criatividade da resposta. Varia de 0 (mais objetiva) até 1 (mais criativa). Default: 0.7.
        /// </summary>
        public double VRTemperature { get; set; }

        #endregion

        #region Constructors

        #endregion

        #region methods

        public PromptPostGeneratorDTO()
        {

        }

        public PromptPostGeneratorDTO(int idp, int idtp, string dst, string dsu, string nmer, string nmv, string nmt, string dso, string dscon, string dsp, string dscom, int nrm, double vrt)
        {
            IDPrompt = idp;
            IDTipoPrompt = idtp;
            DSTema = dst;
            DSUrlSource = dsu;
            NMEstiloResposta = nmer;
            NMVies = nmv;
            NMTitulo = nmt;
            DSObjetivo = dso;
            DSContexto = dscon;
            DSPrompt = dsp;
            DSComplemento = dscom;
            NRMaxTokens = nrm;
            VRTemperature = vrt;
        }

        public PromptPostGeneratorDTO(int idp, int idt, int ide, int idv, string nmt, string dso, string dsc, string dsp, int nrm, double vrt)
        {
            IDPrompt = idp;
            IDTipoPrompt = idt;
            IDEstiloResposta = ide;
            IDVies = idv;
            NMTitulo = nmt;
            DSObjetivo = dso;
            DSContexto = dsc;
            DSPrompt = dsp;
            NRMaxTokens = nrm;
            VRTemperature = vrt;
        }
        #endregion
    }

    /// <summary>
    /// Formato desejado da resposta, como "SQL Script", "Texto descritivo", "Markdown", etc.
    /// </summary>
    public class FormatoSaidaDTO {

        /// <summary>
        /// ID do Formato de saída da resposta esperadano banco de dados.
        /// </summary>
        public int IDFormatoSaida { get; set; }

        /// <summary>
        /// Nome do Formato de saída da resposta esperadano banco de dados.
        /// </summary>
        public string? NMFormatoSaida { get; set; }

        /// <summary>
        /// Diretriz a ser adicionada ao prompt
        /// </summary>
        public string? DSDiretriz { get; set; }
    }

    /// <summary>
    /// Estilo da resposta esperada, como "formal", "técnico", "simplificado", etc.
    /// </summary>
    public class EstiloRespostaDTO
    {

        /// <summary>
        /// ID do Estilo da resposta esperadano banco de dados.
        /// </summary>
        public int IDEstiloResposta { get; set; }

        /// <summary>
        /// Nome do Estilo da resposta esperadano banco de dados.
        /// </summary>
        public string? NMEstiloResposta { get; set; }

        /// <summary>
        /// Diretriz a ser adicionada ao prompt
        /// </summary>
        public string? DSDiretriz { get; set; }

        public EstiloRespostaDTO()
        {

        }
    }

    public class ViesDTO
    {
        #region Propriedades 
        
        public int IDVies { get; set; }

        public string? NMVies { get; set; }

        /// <summary>
        /// Descrição do Vies
        /// </summary>
        public string? DSVies { get; set; }

        /// <summary>
        /// Diretriz a ser adicionada ao prompt
        /// </summary>
        public string? DSDiretriz { get; set; }

        public bool STViesActive { get; set; }

        #endregion

        #region Construtores 

        public ViesDTO()
        { }

        #endregion
        
        #region Métodos 
        
        #endregion
    
    }
}
