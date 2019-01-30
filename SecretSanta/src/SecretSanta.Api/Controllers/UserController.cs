using System;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _UserService;

        public UserController(IUserService userService)
        {
            _UserService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpPost("{user}")]
        public ActionResult PostUser(DTO.User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            User domainUser = DtoToDomain(user);
            _UserService.AddUser(domainUser);
            DTO.User returnUser = new DTO.User(domainUser);

            return Created($"api/User/{returnUser.Id}", returnUser);
        }

        [HttpPut("{user}")]
        public ActionResult PutUser(DTO.User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            User domainUser = DtoToDomain(user);
            _UserService.UpdateUser(domainUser);
            return Ok("User updated!");
        }
        private User DtoToDomain(DTO.User user)
        {
            User domainGift = new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            return domainGift;
        }
        [HttpDelete("{user}")]
        public ActionResult DeleteUser(DTO.User user)
        {
            if(user == null)
            {
                return BadRequest();
            }

            User domainUser = DtoToDomain(user);
            _UserService.DeleteUser(domainUser);

            return Ok();
        }
    }
}