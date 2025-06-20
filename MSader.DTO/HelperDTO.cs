using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSader.DTO
{
    /// <summary>
    /// Valor de uma data, no formato datetime e string.
    /// </summary>
    public class DateTimeDTO
    {
        #region Properties

        /// <summary>
        /// Valor no formato DateTime.
        /// </summary>
        public DateTime DTDateTime { get; set; }

        /// <summary>
        /// Valor no formato texto (dd/MM/yyyy)
        /// </summary>
        public string? DSDate { get; set; }

        /// <summary>
        /// Valor no formato texto (dd/MM)
        /// </summary>
        public string? DSDateShort { get; set; }

        /// <summary>
        /// Valor no formato texto. (HH:mm)
        /// </summary>
        public string? DSTime { get; set; }

        /// <summary>
        /// Valor no formato texto extendido. (HH:mm:ss)
        /// </summary>
        public string? DSTimeExt { get; set; }

        /// <summary>
        /// Valor no formato texto (dd/MM/yyyy HH:mm).
        /// </summary>
        public string? DSDateTime { get; set; }

        /// <summary>
        /// Valor no formato texto (yyyy-MM-dd HH:mm).
        /// </summary>
        public string? DSDateTimeSql { get; set; }

        /// <summary>
        /// Inteiro usado para ordenar colunas com base em datas na camada de apresentação.
        /// </summary>
        public string? NROrder { get; set; }

        #endregion

        #region Constructors

        public DateTimeDTO()
        {

        }

        public DateTimeDTO(DateTime date)
        {
            DTDateTime = date;

            if (date != ConstDTO.GetSQLDateTimeMinValue())
            {
                DSDate = date.ToString("dd/MM/yyyy");
                DSDateShort = date.ToString("dd/MM");
                DSTime = date.ToString("HH:mm");
                DSTimeExt = date.ToString("HH:mm:ss");
                DSDateTime = date.ToString("dd/MM/yyyy HH:mm");
                DSDateTimeSql = date.ToString("yyyy-MM-dd HH:mm");
            }
            else
            {
                DSDate = "";
                DSDateShort = "";
                DSTime = "";
                DSTimeExt = "";
                DSDateTime = "";
                DSDateTimeSql = "";
            }
        }

        public DateTimeDTO(string strdate)
        {

            if (!string.IsNullOrEmpty(strdate))
            {
                DateTime date = DateTime.MinValue;

                if (strdate.Length == 10)
                {
                    date = DateTime.ParseExact(strdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else if (strdate.Length == 19)
                {
                    date = DateTime.ParseExact(strdate, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
                }
                else
                {
                    date = DateTime.Parse(strdate);
                }

                DTDateTime = date;

                if (date != ConstDTO.GetSQLDateTimeMinValue())
                {
                    DSDate = date.ToString("dd/MM/yyyy");
                    DSDateShort = date.ToString("dd/MM");
                    DSTime = date.ToString("HH:mm");
                    DSTimeExt = date.ToString("HH:mm:ss");
                    DSDateTime = date.ToString("dd/MM/yyyy HH:mm");
                }
                else
                {
                    DSDate = "";
                    DSDateShort = "";
                    DSTime = "";
                    DSTimeExt = "";
                    DSDateTime = "";
                }
            }
            else
            {
                DTDateTime = ConstDTO.GetSQLDateTimeMinValue();
                DSDateShort = "";
                DSDate = "";
                DSTime = "";
                DSTimeExt = "";
                DSDateTime = "";
            }

        }

        /// <summary>
        /// Este construtor foi criado para permitir instanciar este objeto a partir de uma DateTime null.
        /// </summary>
        /// <param name="date">Data no formato DateTime.</param>
        /// <param name="dateNull">Flag que serve como diferenciador o construtor que aceita apenas a DateTime não nulo.</param>
        /// <remarks>Nõ é possivel definir o contrutor com o parâmetro dateTime nullable, pois geraria cnfliqo com o contrutor que tem como parâmetro um string, que também é nullable.</remarks>
        public DateTimeDTO(DateTime? date, bool dateNull)
        {
            if (date != null)
            {
                DTDateTime = Convert.ToDateTime(date);
                DSDate = DTDateTime.ToString("dd/MM/yyyy");
                DSDateShort = DTDateTime.ToString("dd/MM");
                DSTime = DTDateTime.ToString("HH:mm");
                DSDateTime = DTDateTime.ToString("dd/MM/yyyy HH:mm");
            }
            else
            {
                DTDateTime = ConstDTO.GetSQLDateTimeMinValue();
                DSDate = "";
                DSDateShort = DTDateTime.ToString("dd/MM");
                DSTime = "";
                DSDateTime = "";
            }
        }

        #endregion
    }

    public class BoolDTO
    {
        #region Properties

        public bool? STStatusAtivo { get; set; }

        public string? DSStatusAtivo { get; set; }

        public int CDBoolSql { get; set; }

        #endregion

        #region Constructors

        public BoolDTO(bool? stAtivo) 
        { 
            STStatusAtivo = stAtivo;

            DSStatusAtivo = "NÃO";

            CDBoolSql = 0;

            if (STStatusAtivo != null && STStatusAtivo == true) 
            { 
                DSStatusAtivo = "SIM";

                CDBoolSql = 1;
            }
        }

        #endregion
    }

    public class ScrapingDTO
    {
        #region Properties

        public string? DSTextScraped { get; set; }

        #endregion
    }
}
