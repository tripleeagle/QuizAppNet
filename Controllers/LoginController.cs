using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
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
        public async Task<ActionResult> Login(User user)
        {
            if (!ModelState.IsValid)
                return new InvalidObjectHttpException().ToJson();
            
            var userDb = _loginService.Get(user.Name).Value;
            
            if ( userDb == null || user.Password != userDb.Password)
                return new LoginHttpException().ToJson();
            
            var identity = await GetIdentity(userDb);
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            
            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
            return Ok();
        }

        private async Task<ClaimsIdentity> GetIdentity(User user)
        {
            var claims = new List<Claim> { new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name) };
            var groups = await _loginService.Groups(user);
            foreach (var group in groups.Value)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, group.Name));
            }
            return  new ClaimsIdentity(claims, "Token", 
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
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