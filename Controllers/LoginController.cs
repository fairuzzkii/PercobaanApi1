using Microsoft.AspNetCore.Mvc;
using PercobaanApi1.Models;

namespace PercobaanApi1.Controllers
{
    public class LoginController : Controller
    {
        private string __constr;
        private IConfiguration __config;

        public LoginController(IConfiguration configuration)
        {
            __config = configuration;
            __constr = configuration.GetConnectionString("WebApiDatabase");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("api/Login")]
        public IActionResult LoginUser(string namaUser, string password)
        {
            LoginContext context = new LoginContext(this.__constr);
            List<Login> ListLogin = context.Autentifikasi(namaUser, password, __config);

            if (ListLogin.Count == 0)
            {
                return Unauthorized(new { message = "Username atau password salah!" });
            }

            return Ok(ListLogin);
        }
    }
}
