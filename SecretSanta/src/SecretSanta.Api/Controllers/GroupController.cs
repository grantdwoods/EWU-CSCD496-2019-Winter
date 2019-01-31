using Microsoft.AspNetCore.Mvc;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;
using System;

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

        [HttpPost("{groupId, userId}")]
        public ActionResult PostUserToGroup(int v1, int v2)
        {
            return Created("", new DTO.Group());
        }
    }
}
