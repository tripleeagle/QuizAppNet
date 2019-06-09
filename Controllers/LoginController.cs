using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizappNet.Models;
using QuizappNet.Services;

namespace QuizappNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController: ControllerBase {
        private readonly QuizAppContext _context;
        IUserService userService;
         public LoginController (QuizAppContext context){   
            _context = context;
           this.userService = new UserService();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(User userViewModel)
        {
            var returnUrl = "/";

            var user = this.userService.GetByName(userViewModel.Name);

            if (user.Password != userViewModel.Password)
                return LocalRedirect(returnUrl);

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Name) };
            var groups = user.Groups;

            foreach (var group in groups)
                claims.Add(new Claim(group.Name, group.Id.ToString()));

            var isAdmin = groups.Any(_ => _.Name == GroupNames.Admins);

            if(isAdmin)
                claims.Add(new Claim(ClaimTypes.Role, GroupNames.Admins));

            var isSuperUser = groups.Any(_ => _.Name == GroupNames.SuperUsers);

            if(isSuperUser)
                claims.Add(new Claim(ClaimTypes.Role, GroupNames.SuperUsers));

            var props = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(5),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);

            return LocalRedirect(returnUrl);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var returnUrl = "/";

            return LocalRedirect(returnUrl);
        }

        /* [HttpPost("register")]
        public async Task<bool> Register(User model)
        {
        if (ModelState.IsValid)
            {
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    _context.Users.Add(new User { Email = model.Email, Password = model.Password });
                    await _context.SaveChangesAsync();

                    return true;
                }    
            }
            return false;
        }
 
      */
    }
}