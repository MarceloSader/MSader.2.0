using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MSader.BLL;
using MSader.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LinkWise.Controllers
{
    public class AccountController : Controller
    {
        public string status = "OK";
        public string message = "Operação concluída com sucesso";

        [HttpPost]
        public async Task<IActionResult> Logon(string dse, string cdc)
        {
            bool stAutenticated = false;

            using (PessoaBLL oBLL = new PessoaBLL())
            {
                PessoaDTO pessoa = oBLL.GetPessoa(dse, cdc);

                if (pessoa != null)
                {
                    stAutenticated = true;

                }
            }

            if (stAutenticated)
            {

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, dse)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return Ok(new { st = status, msg = message});

            }
            else
            {
                return Unauthorized(new { st = "ERRO", msg = "Usuário ou senha inválidos."});
            }
        }

        [HttpPost]
        public async Task<IActionResult> OnPostLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("Account", "Login");
        }
    }
}
