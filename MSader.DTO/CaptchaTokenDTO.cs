using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace MSader.DTO
{
    public class CaptchaTokenDTO
    {
        #region Properties

        /// <summary>
        /// Tempo em segundos definido para expiração do token.
        /// </summary>
        public const int TIME_EXPIRATION = 30;

        /// <summary>
        /// Tempo percorrido entre a validação do recaptcha e o envio do formulário.
        /// </summary>
        public int NRTimeLife { get; set; }

        /// <summary>
        /// Booleano que indica que o envio de e-mail é válido ou não.
        /// </summary>
        public bool STValid { get; set; }

        /// <summary>
        /// Objeto token enviado pelo client.
        /// </summary>
        public oToken? TokenSent { get; set; }

        /// <summary>
        /// Objeto token gerado, salvo na sessão e enviado para o cliente.
        /// </summary>
        public oToken? TokenSaved { get; set; }

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor do token usado na geração.
        /// </summary>
        /// <param name="strIP">IP de origem do acesso.</param>
        public CaptchaTokenDTO(string strIP)
        {
            TokenSaved = new oToken();

            TokenSaved.DTSaved = DateTime.UtcNow;

            TokenSaved.SetJSonDate();

            // Obtem o IP com apenas números
            TokenSaved.CDIp = Regex.Replace(strIP, @"[^\d]", "");

            // Obtém um código numérico para o token
            TokenSaved.x = GetRandom(100000, 999999);

            // Convert o código em base hexadecimal
            TokenSaved.xHex = TokenSaved.x.ToString("X");

            // Monta o token com vários elementos usados apenas para confundir uma possível tentative de robô.
            TokenSaved.CDToken = string.Format("{0}-{1}-{2}-{3}", TokenSaved.CDIp, TokenSaved.xHex, TokenSaved.JSonDTSaved, Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Construtor usado para validar o token.
        /// </summary>
        /// <param name="strIP">Token encviado juntamente com o formulário.</param>
        /// <param name="tokenSent">Token enviado no formulário.</param>
        /// <param name="tokenSaved">Token recuperado da sessão.</param>
        public CaptchaTokenDTO(string strIPRequest, string cdTokenSent, object tokenSaved)
        {
            try
            {
                STValid = true;

                if (cdTokenSent != null && tokenSaved != null)
                {
                    TokenSent = new oToken(cdTokenSent);
                    TokenSaved = new oToken(tokenSaved.ToString());

                    strIPRequest = Regex.Replace(strIPRequest, @"[^\d]", "");

                    NRTimeLife = Convert.ToInt32((DateTime.Now - TokenSaved.DTSaved).TotalSeconds);

                    if (strIPRequest != TokenSent.CDIp)
                    {
                        // ID da requisiçcão é diferente do IP enviado no token.
                        // A pessoa que validou o capctcha não é a mesma que enviou o formulário
                        STValid = false;
                    }
                    else if (NRTimeLife > TIME_EXPIRATION)
                    {
                        // Tempo entre a validação e o envio de formulário é maior que o permitdo.
                        STValid = false;
                    }
                    else if (TokenSent.xHex != TokenSaved.xHex)
                    {
                        // Os códigos são diferentes
                        // O código da sessão é diferente do código enviado.
                        STValid = false;
                    }
                }
                else
                {
                    // Ou o token enviado é nulo ou a sessão expirou
                    STValid = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region Métodos

        /// <summary>
        /// Obtém um número randomico.
        /// </summary>
        /// <returns>String com a senha temporária já criptografada.</returns>
        public int GetRandom(int min, int max)
        {
            System.Random rand = new Random();
            return rand.Next(min, max);
        }


        #endregion

    }

    public class oToken
    {

        #region Properties

        /// <summary>
        /// Data em que a sessão fora gravada, ou seja, quando o cliente clicou no botão que indica que não é um robô.
        /// </summary>
        public DateTime DTSaved { get; set; }

        public string? JSonDTSaved { get; set; }

        /// <summary>
        /// IP do usuário.
        /// </summary>
        public string? CDIp { get; set; }

        /// <summary>
        /// Variável usada na construção do código numérico do token.
        /// </summary>
        public int x { get; set; }

        /// <summary>
        /// Código númerico do token convertido em Hexadecimal
        /// </summary>
        public string? xHex { get; set; }

        /// <summary>
        /// Token completo no formato string
        /// </summary>
        public string? CDToken { get; set; }

        #endregion

        #region Constructors

        public oToken()
        { }

        public oToken(string cdToken)
        {

            string[] arToken = cdToken.Split('-');

            CDIp = arToken[0];
            xHex = arToken[1];

            JSonDTSaved = arToken[2];

            SetCSharpDate();
        }

        #endregion

        public void SetJSonDate()
        {
            JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
            };

            JSonDTSaved = JsonConvert.SerializeObject(DTSaved, microsoftDateFormatSettings);

            JSonDTSaved = Regex.Replace(JSonDTSaved, @"[^\d\-]", "");
        }

        public void SetCSharpDate()
        {
            string sa = @"""" + "/Date(" + JSonDTSaved + ")/" + @"""";
            DTSaved = JsonConvert.DeserializeObject<DateTime>(sa);
        }
    }
}


