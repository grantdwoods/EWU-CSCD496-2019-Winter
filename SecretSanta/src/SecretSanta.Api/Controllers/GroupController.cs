using Microsoft.AspNetCore.Mvc;
using SecretSanta.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            Domain.Models.Group domainGroup = DTO.Group.DtoToDomain(group);
            _GroupService.AddGroup(domainGroup);
            DTO.Group returnedGroup = new DTO.Group(domainGroup);
            return Created("", returnedGroup);
        }
    }
}
