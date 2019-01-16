using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Services
{
    public class UserService
    {
        private ApplicationDbContext DbContext { get; }
        public UserService(ApplicationDbContext context)
        {
            DbContext = context;
        }

        public void AddUser(User user)
        {
            DbContext.Users.Add(user);
            DbContext.SaveChanges();
        }
        public void UpdateUser(User user)
        {
            DbContext.Users.Update(user);
            DbContext.SaveChanges();
        }

        public User Find(int id)
        {
            return DbContext.Users.Find(id);
        }

        public List<User> FetchAll()
        {
            var userTask = DbContext.Users.ToListAsync();
            userTask.Wait();

            return userTask.Result;
        }
    }
}
