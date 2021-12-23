using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RedBookWebApplication.Helper;
using RedBookWebApplication.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RedBookWebApplication.Controllers
{
    public class UserController : Controller
    {
        RedBookApi api = new RedBookApi();

        private IConfiguration _config;
     
        public UserController(IConfiguration config)
        {
            
            _config = config;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserModel user)
        {

            HttpClient client = api.Initial();
            HttpResponseMessage res = await client.GetAsync($"api/Login?username={user.UserName}&pass={user.Password}");
          
            var claims = new List<Claim>
            {
                new Claim("token", res.Content.ReadAsStringAsync().Result)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie");

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
      
            return RedirectToAction("Index", "Home");
        }      
    }
}
