using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _UserService { get; }
        private IMapper _Mapper { get; }
        private ILogger _Logger { get; }

        public UsersController(IUserService userService, IMapper mapper, ILogger<UsersController> logger)
        {
            _UserService = userService;
            _Mapper = mapper;
            _Logger = logger;
        }

        // GET api/User
        [HttpGet]
        public async Task<ActionResult<ICollection<UserViewModel>>> GetAllUsers()
        {
            var users = await _UserService.FetchAll();
            return Ok(users.Select(x => _Mapper.Map<UserViewModel>(x)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserViewModel>> GetUser(int id)
        {
            var fetchedUser = await _UserService.GetById(id);
            if (fetchedUser == null)
            {
                return NotFound();
            }

            return Ok(_Mapper.Map<UserViewModel>(fetchedUser));
        }

        // POST api/User
        [HttpPost]
        public async Task<ActionResult<UserViewModel>> CreateUser(UserInputViewModel viewModel)
        {
            if (User == null)
            {
                return BadRequest();
            }

            var createdUser = await _UserService.AddUser(_Mapper.Map<User>(viewModel));

            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, _Mapper.Map<UserViewModel>(createdUser));
        }

        // PUT api/User/5
        [HttpPut]
        public async Task<ActionResult> UpdateUser(int id, UserInputViewModel viewModel)
        {
            if (viewModel == null)
            {
                return BadRequest();
            }
            var fetchedUser = await _UserService.GetById(id);
            if (fetchedUser == null)
            {
                return NotFound();
            }

            _Mapper.Map(viewModel, fetchedUser);
            await _UserService.UpdateUser(fetchedUser);
            return NoContent();
        }

        // DELETE api/User/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            if (id <= 0)
            {
                return BadRequest("A User id must be specified");
            }

            if (await _UserService.DeleteUser(id))
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
