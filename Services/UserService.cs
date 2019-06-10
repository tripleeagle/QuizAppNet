using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuizappNet.Models;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace QuizappNet.Services
{
    public class UserService : IUserService
    {
        const string filename = "users.json";
        private QuizAppContext _context;

        public UserService(QuizAppContext context)
        {
            if (File.Exists(filename) && context.Users.Count() == 0)
            {
                var json = File.ReadAllText(filename);
                _context = context;
                _context.Users.AddRange(JsonConvert.DeserializeObject<IList<User>>(json));
                 _context.SaveChanges();
                _context.Groups.Add( this.CreateSuperuserGroup() );
                _context.SaveChanges();
            }
        }

        public User GetByName(string name)
        {
            var q = from x in _context.Users where x.Name == name select x;
            var user = q.AsNoTracking().FirstOrDefault();

            return user;
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Users.Remove(this.GetByName(user.Name));
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        private Group CreateSuperuserGroup (){
            Group group =  new Group { Name = GroupNames.SuperUsers };
            group.usersLinks = new List<GroupUser>();
            GroupUser gu = new GroupUser(){Group = group, User = _context.Users.First()};
            group.usersLinks.Add ( gu);
            return group;
        }
    }
}