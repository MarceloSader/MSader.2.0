using System.Text;

namespace MSader.Helpers
{
    public static class NavigationHelper
    {
        public static string BuildMenuHtml(string selectedMenuOption, string selectedSubMenuOption, IHttpContextAccessor httpContextAccessor)
        {
            var sb = new StringBuilder();

            string baseUrl = GetUrlBase(httpContextAccessor);

            bool isAISelected = selectedMenuOption == "Inteligência Artificial" ||
                                new[] {"Computer Vision", "Natural Language", "Knowledge Minings", "Document Intelligence"}.Contains(selectedSubMenuOption);

            bool isBlogSelected = selectedMenuOption == "Blog" ||
                                  new[] { "Reserva Cognitiva", "Tecnologia", "Mundo Animal", "Ciência" }.Contains(selectedSubMenuOption);

            bool isConteudoSelected = selectedMenuOption == "Conteudo" ||
                                      new[] { "Destra", "Medicina Veterinária", "Tecnologia" }.Contains(selectedSubMenuOption);

            sb.AppendLine("<ul class=\"nav nav-pills\" id=\"mainNav\">");

            sb.AppendLine(BuildMenuItem("Home", $"{baseUrl}/Home/Index", selectedMenuOption));
            sb.AppendLine(BuildMenuItem("Jornada", $"{baseUrl}/Home/Jornada", selectedMenuOption));
            sb.AppendLine(BuildMenuItem("Ferramentas", $"{baseUrl}/Home/Tools", selectedMenuOption));

            // Inteligência Artificial
            sb.AppendLine($"    <li class=\"dropdown{(isAISelected ? " open" : "")}\">");
            sb.AppendLine($"        <a class=\"dropdown-item dropdown-toggle{(isAISelected ? " active" : "")}\" href=\"#\">");
            sb.AppendLine("            Inteligência Artificial");
            sb.AppendLine("        </a>");
            sb.AppendLine("        <ul class=\"dropdown-menu\">");

            sb.AppendLine(BuildSubItem("Computer Vision", $"{baseUrl}/AITools/AIVisual", selectedSubMenuOption));
            sb.AppendLine(BuildSubItem("Natural Language", $"{baseUrl}/AITools/AINaturalLanguage", selectedSubMenuOption));
            sb.AppendLine(BuildSubItem("Knowledge Minings", $"{baseUrl}/AITools/AI", selectedSubMenuOption));
            sb.AppendLine(BuildSubItem("Document Intelligence", $"{baseUrl}/AITools/AI", selectedSubMenuOption));
            sb.AppendLine("        </ul>");
            sb.AppendLine("    </li>");

            // Blog
            sb.AppendLine($"    <li class=\"dropdown{(isBlogSelected ? " open" : "")}\">");
            sb.AppendLine($"        <a class=\"dropdown-item dropdown-toggle{(isBlogSelected ? " active" : "")}\" href=\"#\">");
            sb.AppendLine("            Blog");
            sb.AppendLine("        </a>");
            sb.AppendLine("        <ul class=\"dropdown-menu\">");
            sb.AppendLine(BuildSubItem("Reserva Cognitiva", "#", selectedSubMenuOption));
            sb.AppendLine(BuildSubItem("Tecnologia", "#", selectedSubMenuOption));
            sb.AppendLine(BuildSubItem("Mundo Animal", "#", selectedSubMenuOption));
            sb.AppendLine(BuildSubItem("Ciência", "#", selectedSubMenuOption));
            sb.AppendLine("        </ul>");
            sb.AppendLine("    </li>");

            // Conteúdo
            sb.AppendLine($"    <li class=\"dropdown{(isConteudoSelected ? " open" : "")}\">");
            sb.AppendLine($"        <a class=\"dropdown-item dropdown-toggle{(isConteudoSelected ? " active" : "")}\" href=\"#\">");
            sb.AppendLine("            Conteúdo");
            sb.AppendLine("        </a>");
            sb.AppendLine("        <ul class=\"dropdown-menu\">");
            sb.AppendLine(BuildSubItem("Destra", $"{baseUrl}/Conteudo/Destra", selectedSubMenuOption));
            sb.AppendLine(BuildSubItem("Medicina Veterinária", $"{baseUrl}/Conteudo/MedicinaVeterinaria", selectedSubMenuOption));
            sb.AppendLine(BuildSubItem("Tecnologia", $"{baseUrl}/Conteudo/Tecnologia", selectedSubMenuOption));
            sb.AppendLine("        </ul>");
            sb.AppendLine("    </li>");

            sb.AppendLine("</ul>");

            return sb.ToString();
        }


        private static string BuildMenuItem(string label, string href, string selectedOption)
        {
            string liClass = "dropdown" + (label == selectedOption ? " open" : "");
            string aClass = (label == selectedOption ? "active" : "");

            var sb = new StringBuilder();
            sb.AppendLine($"    <li class=\"{liClass}\">");
            sb.AppendLine($"        <a class=\"{aClass}\" href=\"{href}\">");
            sb.AppendLine($"            {label}");
            sb.AppendLine("        </a>");
            sb.AppendLine("    </li>");

            return sb.ToString();
        }

        private static string BuildSubItem(string label, string href, string selectedOption)
        {
            string aClass = "dropdown-item" + (label == selectedOption ? " active" : "");
            return $"            <li><a class=\"{aClass}\" href=\"{href}\">{label}</a></li>";
        }

        /// <summary>
        /// Obtém a url base da requisição.
        /// </summary>
        /// <returns>String da URL base da requisição.</returns>
        public static string GetUrlBase(IHttpContextAccessor httpContextAccessor)
        {
            var context = httpContextAccessor.HttpContext;

            if (context == null) return string.Empty;

            var request = context.Request;

            var urlBase = $"{request.Scheme}://{request.Host}";

            return urlBase;
        }
    }



}
