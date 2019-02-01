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
            _UserService = userService;
        }

        [HttpPost("{user}")]
        public ActionResult PostUser(DTO.User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            User domainUser = DTO.User.DtoToDomain(user);
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

            User domainUser = DTO.User.DtoToDomain(user);
            _UserService.UpdateUser(domainUser);
            return Ok("User updated!");
        }

        [HttpDelete("{user}")]
        public ActionResult DeleteUser(DTO.User user)
        {
            if(user == null)
            {
                return BadRequest();
            }

            User domainUser = DTO.User.DtoToDomain(user);
            _UserService.DeleteUser(domainUser);

            return Ok();
        }
    }
}