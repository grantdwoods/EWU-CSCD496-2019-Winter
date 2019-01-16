using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Services
{
    class GroupService
    {
        private ApplicationDbContext DbContext { get; }
        public GroupService(ApplicationDbContext context)
        {
            DbContext = context;
        }

        public void AddUser(int id, User user)
        {
           
        }

        public void AddGroup(Group group)
        {

        }

        public void RemoveUser()
        {

        }

        //public Group Find(int id)
        //{
        //    return DbContext.Groups.Find(id);
        //}
    }
}
