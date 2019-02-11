using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Domain.Services.Interfaces
{
    public interface IGroupService
    {
        List<Group> FetchAll();
        Group GetById(int id);

        Group AddGroup(Group group);

        Group UpdateGroup(Group group);

        bool DeleteGroup(int groupId);

        Task<bool> AddUserToGroup(int groupId, int userId);

        bool RemoveUserFromGroup(int groupId, int userId);
    }
}
