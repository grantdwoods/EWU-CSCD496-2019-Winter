using System.Collections.Generic;
using System.Linq;
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
    public class GiftController : ControllerBase
    {
        private IGiftService GiftService { get; }
        private IMapper Mapper { get; }

        public GiftController(IGiftService giftService, IMapper mapper)
        {
            Mapper = mapper;
            GiftService = giftService;
        }

        // GET api/Gift/5
        [HttpGet("{userId}")]
        public ActionResult<List<GiftViewModel>> GetGiftForUser(int userId)
        {
            if (userId <= 0)
            {
                return NotFound();
            }
            List<Gift> userGifts = GiftService.GetGiftsForUser(userId);
            var users = Mapper.Map <List<GiftViewModel>>(userGifts);

            return userGifts.Select(x => GiftViewModel.ToViewModel(x)).ToList();
        }
    }
}
