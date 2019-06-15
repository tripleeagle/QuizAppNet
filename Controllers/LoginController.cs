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
using QuizappNet.Utils;
using QuizappNet.Utils.Models;

namespace QuizappNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController: ControllerBase {
        private readonly QuizAppContext _context;
        IUserService userService;
         public LoginController (QuizAppContext context){   
            _context = context;
           this.userService = new UserService(_context);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(User userModel)
        {
            var user = _context.Users.FirstOrDefault( u => u.Name == userModel.Name);

            if ( user == null || user.Password != userModel.Password)
                return new LoginHttpException().ToJson();

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Name) };

            var groups = _context.GroupUsers.Where(g => g.UserId == user.Id).ToList();

            foreach (var groupUser in groups){
                var group = await _context.Groups.FindAsync(groupUser.GroupId);
                claims.Add(new Claim(group.Name, groupUser.GroupId.ToString()));
            }

            var isAdmin = groups.Any(_ => _.Group.Name == GroupNames.Admins);
            var isSuperUser = groups.Any(_ => _.Group.Name == GroupNames.SuperUsers);
        
            if(isAdmin)
                claims.Add(new Claim(ClaimTypes.Role, GroupNames.Admins));

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
            return Ok();
        }

        [HttpPost("logout")]
        [AllowAnonymous]
        public ActionResult Logout()
        {
            this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        [HttpPost("register")]
        [Authorize(Roles = GroupNames.SuperUsers)]
        public async Task<ActionResult> Register(User newUser)
        {
            if (!ModelState.IsValid) 
                return new InvalidObjectHttpException().ToJson();
            
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Name == newUser.Name);

            if ( user != null )
                return new ExistingLoginHttpException(user.Name).ToJson();

            if ( newUser.groupsLinks != null ){
                foreach ( var groupLink in newUser.groupsLinks ){
                    groupLink.User = newUser;
                    groupLink.Group = _context.Groups.Find(groupLink.GroupId);
                }
            }

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
              
            return Ok();
        }
    }
}