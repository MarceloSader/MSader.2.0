

namespace MSader.DTO
{
    public class NavigationDTO
    {



    }

    public class MenuDTO
    {
        public string? Home { get; set; }
        public string? Labs { get; set; }
        public string? Blog { get; set; }
        public string? Login { get; set; }
        public string? Loja { get; set; }
    }

    public class MenuPublicDTO
    {
        public string? HomeActive { get; set; }
        public string? LabsActive { get; set; }
        public string? BlogActive { get; set; }
        public string? LoginActive { get; set; }
        public string? LojaActive { get; set; }

        public MenuPublicDTO(string option)
        {
            HomeActive = "";
            LabsActive = "";
            BlogActive = "";
            LoginActive = "";
            LojaActive = "";

            switch (option)
            {
                case "Home":
                    HomeActive = "active";
                    break;
                case "Labs":
                    LabsActive = "active";
                    break;
                case "Blog":
                    BlogActive = "active";
                    break;
                case "Login":
                    LoginActive = "active";
                    break;
                case "Loja":
                    LojaActive = "active";
                    break;

                default:
                    break;
            }
        }
    }

    public class MenuAdminDTO
    {
        public string? HomeActive { get; set; }
        public string? AIToolsActive { get; set; }
        public string? BlogsActive { get; set; }
        public string? PromptsActive { get; set; }

        public MenuAdminDTO(string option)
        {
            HomeActive = "";
            AIToolsActive = "";
            BlogsActive = "";
            PromptsActive = "";

            switch (option)
            {
                case "Home":
                    HomeActive = "active";
                    break;
                case "AITools":
                    AIToolsActive = "active";
                    break;
                case "Blogs":
                    BlogsActive = "active";
                    break;
                case "Prompts":
                    PromptsActive = "active";
                    break;

                default:
                    break;
            }
        }
    }
}
