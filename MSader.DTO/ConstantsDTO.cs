

using System.Data.SqlTypes;

namespace MSader.DTO
{
    public static class ConstantsDTO
    {
        public const string OPEN_AI_API_KEY = "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";

        public const string AZURE_OPEN_AI_API_KEY = "2uHvncjbR2OicrSqDm3gSpE9mLAFLYshJiInTy6WOiucpWxKvP3GJQQJ99BDACYeBjFXJ3w3AAABACOGrDpA";

        public const int NR_POSTS = 500;

        public const int NR_POST_COMMENTS = 30;

        public const int FOTO_MAX_LENGTH = 10240000;

        public const string PATH_FOTOS = "midia/posts/";

        public const string PATH_AVATARS = "image/avatars/pessoas";

        public const string CONN_STRING = "Server=tcp:sql-msader-prd-01.database.windows.net,1433;Initial Catalog=sqldb-msader-prd-01;Persist Security Info=False;User ID=msader-operator;Password=CeHAd?ad8U;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
    }

    /// <summary>
    /// Constantes usadas no projeto
    /// </summary>
    public static class ConstDTO
    {
        public static class Blogs
        {
            public static class Linkwise
            {
                public static int ID = 1;
                public static string ControllerName = "Blog";
            }

            public static class ReservaCognitiva
            {
                public static int ID = 2;
                public static string ControllerName = "ReservaCognitiva";
            }
        }

        public static class TipoPrompt
        {
            public static class ComputerVision
            {
                public static int ID = 1;
            }
            public static class NaturalLanguage
            {
                public static int ID = 2;
            }
            public static class KnowledgeMining
            {
                public static int ID = 3;
            }
            public static class DocumentInteligence
            {
                public static int ID = 4;
            }
        }

        public static class TipoPost
        {
            public static class Texto
            {
                public static int ID = 1;
            }
            public static class Foto
            {
                public static int ID = 2;
            }
            public static class Video
            {
                public static int ID = 3;
            }
            public static class Audio
            {
                public static int ID = 4;
            }
        }

        public static class Autor
        {
            public static class Marcelo
            {
                public static int ID = 1;
            }
        }

        public static class ViesCategoria
        {
            public static class Politico
            {
                public static int ID = 1;
            }

            public static class Economico
            {
                public static int ID = 2;
            }

            public static class Cultural_Social
            {
                public static int ID = 3;
            }

            public static class Filosofico_Etico
            {
                public static int ID = 4;
            }

            public static class Cientifico_Tecnologico
            {
                public static int ID = 5;
            }

            public static class Neutro
            {
                public static int IDVies = 78;
            }

        }

        /// <summary>
        /// Obtém a data e hora mínima do banco de dados SL Server. 1 de janeiro de 1753, 01/01/1753
        /// </summary>
        /// <returns>Objeto DateTime com a data e hora local brasileira.</returns>
        public static DateTime GetSQLDateTimeMinValue()
        {
            return (DateTime)SqlDateTime.MinValue;
        }

        public static class Perfis
        {
            public static class Admin
            {
                public static int ID = 1;
                public static string Nome = "Admin";
            }

            public static class Convidado
            {
                public static int ID = 2;
                public static string Nome = "Convidado";
            }

            public static class AtorIA
            {
                public static int ID = 3;
                public static string Nome = "AtorIA";
            }
        }
    }

}
