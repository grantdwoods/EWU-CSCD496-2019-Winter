using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Services
{
    public interface IGroupService
    {
        List<Group> FetchAll();
        Group UpdateGroup(Group group);
        Group AddGroup(Group group);
        void DeleteGroup(Group group);
        Group AddUserToGroup(int groupId, int userId);
        List<User> GetUsers(int groupId);
    }
}
