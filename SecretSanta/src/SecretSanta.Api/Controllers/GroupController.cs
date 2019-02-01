using Microsoft.AspNetCore.Mvc;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;
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
            _GroupService = groupService;
        }

        [HttpGet("{groupId}")]
        public ActionResult GetUsersInGroup(int groupId)
        {
            if(groupId <= 0)
            {
                return NotFound();
            }

            List<User> domainUsers = _GroupService.GetUsers(groupId);
            var dtoUsers = domainUsers.Select(x => new DTO.User(x)).ToList();

            return Ok(dtoUsers);
        }

        [HttpGet("groups/")]
        public ActionResult GetAllGroups()
        {
            List<Group> domainGroups = _GroupService.FetchAll();
            var dtoGroups = domainGroups.Select(x => new DTO.Group(x)).ToList();

            return Ok(dtoGroups);
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

        [HttpPost("userGroup/{groupId, userId}")]
        public ActionResult PostUserToGroup(int groupId, int userId)
        {
            if(groupId <=0 || userId <= 0)
            {
                return BadRequest();
            }

            Group returnedGroup = _GroupService.AddUserToGroup(groupId, userId);
            return Created($"api/Group/{returnedGroup.Id}", new DTO.Group(returnedGroup));
        }

        [HttpPut("{group}")]
        public ActionResult PutGroup(DTO.Group group)
        {
            if(group == null)
            {
                return BadRequest();
            }

            Group domainGift = DTO.Group.DtoToDomain(group);
            _GroupService.UpdateGroup(domainGift);
            return Ok(new DTO.Group(domainGift));
        }

        [HttpDelete("{group}")]
        public ActionResult DeleteGroup(DTO.Group group)
        {
            if(group == null)
            {
                return BadRequest();
            }

            Group domainGroup = DTO.Group.DtoToDomain(group);
            _GroupService.DeleteGroup(domainGroup);

            return Ok("Group removed.");
        }
    }
}
