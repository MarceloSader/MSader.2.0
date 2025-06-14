using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MSader.BLL;
using MSader.DTO;
using System.Security.Claims;


namespace LinkWise.Controllers
{
    public class AccountController : Controller
    {
        public string status = "OK";
        public string message = "Operação concluída com sucesso";

        public IActionResult Login(string ReturnUrl = "")
        {
            ViewBag.Menu = new MenuPublicDTO("Login");

            ViewBag.UrlToGo = ReturnUrl;

            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Logon(string dse, string cdc, string url)
        {
            bool stAutenticated = false;

            PessoaDTO pessoa = new PessoaDTO();

            using (PessoaBLL oBLL = new PessoaBLL())
            {
                pessoa = oBLL.GetPessoa(dse, cdc);

                if (pessoa != null)
                {
                    stAutenticated = true;

                }
            }

            if (stAutenticated && pessoa != null)
            {

                string role = pessoa.GetRole();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, dse),
                    new Claim(ClaimTypes.Role, role) // <- define o papel
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return Ok(new { st = status, msg = message, url = url });

            }
            else
            {
                return Unauthorized(new { st = "ERRO", msg = "Usuário ou senha inválidos." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logoff()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }

    }
}
