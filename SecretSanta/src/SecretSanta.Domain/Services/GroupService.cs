using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretSanta.Domain.Services
{
    public class GroupService
    {
        private ApplicationDbContext Context { get; set; }
        public GroupService(ApplicationDbContext context)
        {
            this.Context = context;
        }

        public Group AddGroup(Group group)
        {
            Context.Groups.Add(group);
            Context.SaveChanges();

            return group;
        }

        public void AddUserToGroup(int groupId, UserGroup userGroup)
        {
            var group = Context.Groups
                .Include(g => g.UserGroups)
                .SingleOrDefault(g => g.Id == groupId);

            group.UserGroups.Add(userGroup);

            Context.SaveChanges();
        }

        public void RemoveUserFromGroup(int groupId, int userId)
        {
            var group = Context.Groups
                .Include(g => g.UserGroups)
                .SingleOrDefault(g => g.Id == groupId);

            var userGroup = group.UserGroups.SingleOrDefault(ug => ug.UserId == userId);
            group.UserGroups.Remove(userGroup);

            Context.SaveChanges();
        }
    }
}