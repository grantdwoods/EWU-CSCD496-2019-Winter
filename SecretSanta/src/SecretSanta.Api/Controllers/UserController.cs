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
    public class UserController : ControllerBase
    {
        private IUserService UserService { get; }
        private IMapper Mapper { get; }

        public UserController(IUserService userService, IMapper mapper)
        {
            Mapper = mapper;
            UserService = userService;
        }

        // POST api/<controller>
        [HttpPost]
        public ActionResult<UserViewModel> Post(UserInputViewModel userInputViewModel)
        {
            if (userInputViewModel == null)
            {
                return BadRequest();
            }
            var domainUser = Mapper.Map<User>(userInputViewModel);
            var persistedUser = UserService.AddUser(domainUser);

            return Ok(Mapper.Map<UserViewModel>(persistedUser));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public ActionResult<UserViewModel> Put(int id, UserInputViewModel userViewModel)
        {
            if (userViewModel == null)
            {
                return BadRequest();
            }

            var foundUser = UserService.Find(id);
            if (foundUser == null)
            {
                return NotFound();
            }

            foundUser.FirstName = userViewModel.FirstName;
            foundUser.LastName = userViewModel.LastName;

            var persistedUser = UserService.UpdateUser(foundUser);

            return Ok(UserViewModel.ToViewModel(persistedUser));
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            bool userWasDeleted = UserService.DeleteUser(id);

            return userWasDeleted ? (ActionResult)Ok() : (ActionResult)NotFound();
        }
    }
}
