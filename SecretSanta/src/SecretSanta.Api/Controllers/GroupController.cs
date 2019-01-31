using Microsoft.AspNetCore.Mvc;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _GroupService;

        public GroupController(IGroupService groupService)
        {
            _GroupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
        }

        [HttpPost("{group}")]
        public ActionResult PostGroup(DTO.Group group)
        {
            if(group == null)
            {
                return BadRequest();
            }

            Group domainGroup = DTO.Group.DtoToDomain(group);
            _GroupService.AddGroup(domainGroup);
            DTO.Group returnedGroup = new DTO.Group(domainGroup);
            return Created("", returnedGroup);
        }

        [HttpPost("{groupId}, {userId}")]
        public ActionResult PostUserToGroup(int groupId, int userId)
        {
            Group returnedGroup = _GroupService.AddUserToGroup(groupId, userId);
            return Created($"api/Group/{returnedGroup.Id}", new DTO.Group(returnedGroup));
        }

        [HttpGet("{groupId}")]
        public ActionResult GetUsersInGroup(int groupId)
        {
            List<User> domainUsers =_GroupService.GetUsers(groupId);
            var dtoUsers = domainUsers.Select(x => new DTO.User(x)).ToList();
            return Ok(dtoUsers);
        }
    }
}
