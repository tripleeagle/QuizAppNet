using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizappNet.Models;
using QuizappNet.HttpValues.HttpExceptions;
using QuizappNet.Values;
using QuizappNet.Services.Interfaces;
using QuizappNet.HttpValues;

namespace QuizappNet.Services
{
    public class LoginService : ILoginService
    {
        private readonly QuizAppContext _db;

        public LoginService (QuizAppContext db){
            _db = db;
        }

        public async Task<ActionResult> Create(User newUser) {
            User user = await _db.Users.FirstOrDefaultAsync(u => u.Name == newUser.Name);

            if ( user != null || !CheckGroups( (await Groups(newUser)).Value ))
                return new ExistingLoginHttpException(user.Name).ToJson();

            if ( newUser.groupsLinks != null ){
                foreach ( var groupLink in newUser.groupsLinks ){
                    groupLink.User = newUser;
                    groupLink.Group = _db.Groups.Find(groupLink.GroupId);
                }
            }

            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();
              
            return new HttpOk().ToJson();
        }

        public ActionResult<User> Get(string username) {
            return _db.Users.FirstOrDefault( u => u.Name == username);
        }

        public async Task<ActionResult<List<Group>>> Groups(User user) {
            List<Group> groups = new List<Group>();
            var userOld = Get(user.Name).Value;
            if ( userOld == null )
                return groups;

            List<GroupUser> groupUsers = await _db.GroupUsers.Where(g => g.UserId == userOld.Id).ToListAsync();            
            foreach ( GroupUser groupUser in groupUsers )
                groups.Add( await _db.Groups.FindAsync(groupUser.GroupId) );
            return groups;
        }

        public bool CheckGroups(List<Group> groups) {
            foreach ( Group group in groups ){
                bool exists = false;
                foreach ( string definedGroupName in GroupNames.GroupNameList ){
                    if ( group.Name == definedGroupName )
                        exists = true;
                }
                if ( !exists )
                    return false;
            }
            return true;
        }
    }
}