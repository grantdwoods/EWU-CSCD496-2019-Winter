 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private IGroupService GroupService { get; }
        private IMapper _Mapper { get; }

        public GroupController(IGroupService groupService, IMapper mapper)
        {
            GroupService = groupService;
            _Mapper = mapper;
        }

        // GET api/group
        [HttpGet]
        public ActionResult<IEnumerable<GroupViewModel>> GetAllGroups()
        {
            return _Mapper.Map <List<GroupViewModel>>(GroupService.FetchAll());
        }

        // POST api/group
        [HttpPost]
        public ActionResult<GroupViewModel> PostGroup(GroupInputViewModel viewModel)
        {
            if (viewModel == null)
            {
                return BadRequest();
            }

            var group = GroupService.AddGroup(_Mapper.Map<Group>(viewModel));

            return _Mapper.Map<GroupViewModel>(GroupService.AddGroup(_Mapper.Map<Group>(viewModel)));
        }

        // PUT api/group/5
        [HttpPut("{id}")]
        public ActionResult<GroupViewModel> PutGroup(int id, GroupInputViewModel viewModel)
        {
            if (viewModel == null)
            {
                return BadRequest();
            }
            var fetchedGroup = GroupService.Find(id);
            if (fetchedGroup == null)
            {
                return NotFound();
            }

            fetchedGroup.Name = viewModel.Name;

            return NoContent();//_Mapper.Map<GroupViewModel>(GroupService.UpdateGroup(fetchedGroup));
        }

        [HttpPut("{groupId}/{userid}")]
        public ActionResult PutUserToGroup(int groupId, int userId)
        {
            if (groupId <= 0 || userId <=0)
            {
                return NotFound();
            }

            if (!GroupService.AddUserToGroup(groupId, userId))
            {
                return BadRequest();
            }

            return NoContent();
        }

        // DELETE api/group/5
        [HttpDelete("{id}")]
        public ActionResult DeleteGroup(int id)
        {
            if (id <= 0)
            {
                return BadRequest("A group id must be specified");
            }

            if (GroupService.DeleteGroup(id))
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
