using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;

namespace SecretSanta.Domain.Services
{
    public class GroupService : IGroupService
    {
        private ApplicationDbContext DbContext { get; }

        public GroupService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Group AddGroup(Group group)
        {
            DbContext.Groups.Add(group);
            DbContext.SaveChanges();
            return group;
        }

        public Group GetById(int id)
        {
            return DbContext.Groups.Find(id);
        }

        public Group UpdateGroup(Group group)
        {
            DbContext.Groups.Update(group);
            DbContext.SaveChanges();
            return group;
        }

        public List<Group> FetchAll()
        {
            return DbContext.Groups.ToList();
        }

        public List<User> GetUsers(int groupId)
        {
            return DbContext.Groups
                .Where(x => x.Id == groupId)
                .SelectMany(x => x.GroupUsers)
                .Select(x => x.User)
                .ToList();
        }

        public async Task<bool> AddUserToGroup(int groupId, int userId)
        {
            Group foundGroup = await DbContext.Groups
                .Include(x => x.GroupUsers)
                .FirstOrDefaultAsync(x => x.Id == groupId);
            if (foundGroup == null) return false;

            User foundUser = await DbContext.Users.FindAsync(userId);
            if (foundUser == null) return false;

            var groupUser = new GroupUser { GroupId = foundGroup.Id, UserId = foundUser.Id };
            if (foundGroup.GroupUsers == null)
            {
                foundGroup.GroupUsers = new List<GroupUser>();
            }
            foundGroup.GroupUsers.Add(groupUser);
            await DbContext.SaveChangesAsync();

            return true;
        }

        public bool RemoveUserFromGroup(int groupId, int userId)
        {
            Group foundGroup = DbContext.Groups
                .Include(x => x.GroupUsers)
                .FirstOrDefault(x => x.Id == groupId);

            GroupUser mapping = foundGroup?.GroupUsers.FirstOrDefault(x => x.UserId == userId);

            if (mapping == null) return false;

            foundGroup.GroupUsers.Remove(mapping);
            DbContext.SaveChanges();

            return true;
        }

        public bool DeleteGroup(int groupId)
        {
            Group foundGroup = DbContext.Groups.Find(groupId);

            if (foundGroup != null)
            {
                DbContext.Groups.Remove(foundGroup);
                DbContext.SaveChanges();
                return true;
            }

            return false;
        }
    }
}