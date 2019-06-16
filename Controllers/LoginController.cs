using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizappNet.Models;
using QuizappNet.HttpValues.HttpExceptions;
using QuizappNet.Values;
using QuizappNet.Services.Interfaces;
using QuizappNet.HttpValues;

namespace QuizappNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController: ControllerBase {
        private readonly ILoginService _loginService;

        public LoginController (ILoginService loginService){   
            _loginService = loginService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(User userModel)
        {
            if (!ModelState.IsValid) 
                return new InvalidObjectHttpException().ToJson();
                
            var user = _loginService.Get(userModel.Name).Value;

            if ( user == null || user.Password != userModel.Password)
                return new LoginHttpException().ToJson();

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Name) };

            var groups = (await _loginService.Groups(userModel)).Value;

            if ( !_loginService.CheckGroups(groups) )
                return new InvalidObjectHttpException().ToJson();

            foreach (var group in groups)
                claims.Add(new Claim(ClaimTypes.Role, group.Name));
            
            var props = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(5),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);
            return new HttpOk().ToJson();
        }


        [HttpPost("logout")]
        [AllowAnonymous]
        public ActionResult Logout()
        {
            this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return new HttpOk().ToJson();
        }

        [HttpPost("register")]
        [Authorize(Roles = GroupNames.SuperUsers + "," + GroupNames.Admins)]
        public async Task<ActionResult> Register(User newUser)
        {
            if (!ModelState.IsValid) 
                return new InvalidObjectHttpException().ToJson();
              
            return await _loginService.Create(newUser);
        }
    }
}