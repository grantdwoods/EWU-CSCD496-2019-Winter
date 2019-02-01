using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecretSanta.Domain.Services
{
    public class GroupService : IGroupService
    {
        private ApplicationDbContext DbContext { get; }

        public GroupService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Group AddGroup(Group @group)
        {
            DbContext.Groups.Add(@group);
            DbContext.SaveChanges();
            return @group;
        }

        public Group UpdateGroup(Group @group)
        {
            DbContext.Groups.Update(@group);
            DbContext.SaveChanges();
            return @group;
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

        public void DeleteGroup(Group @group)
        {
            DbContext.Remove(@group);
            DbContext.SaveChanges();
        }

        public Group AddUserToGroup(int groupId, int userId)
        {
            Group group = DbContext.Groups.Single(x => x.Id == groupId);
            User user = DbContext.Users.Single(x => x.Id == userId);
            GroupUser groupUser = new GroupUser{
            User = user,
            Group = group,
            GroupId = group.Id,
            UserId = user.Id};

            if(group.GroupUsers != null && user.GroupUsers != null)
            {
                group.GroupUsers.Add(groupUser);
                user.GroupUsers.Add(groupUser);
            }
            else
            {
                group.GroupUsers = new List<GroupUser> {groupUser};
                user.GroupUsers = new List<GroupUser> { groupUser };
            }

            DbContext.Groups.Update(group);
            DbContext.Users.Update(user);

            DbContext.SaveChanges();

            return group;
        }
    }
}