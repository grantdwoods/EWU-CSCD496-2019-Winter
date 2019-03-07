using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftsController : ControllerBase
    {
        private IGiftService GiftService { get; }
        private IMapper Mapper { get; }
        private ILogger<GiftsController> Logger {get;}
        public GiftsController(IGiftService giftService, IMapper mapper, ILogger<GiftsController> logger)
        {
            Logger = logger;
            GiftService = giftService;
            Mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GiftViewModel>> GetGift(int id)
        {
            Logger.LogDebug($"Acessing GiftService.GetGift({id})");
            var gift = await GiftService.GetGift(id);

            if (gift == null)
            {
                Logger.LogInformation($"Get gift 404 No data for {id}.");
                return NotFound();
            }
            Logger.LogInformation($"Found Gift {id}.");
            return Ok(Mapper.Map<GiftViewModel>(gift));
        }

        [HttpPost]
        public async Task<ActionResult<GiftViewModel>> CreateGift(GiftInputViewModel viewModel)
        {
            Logger.LogDebug($"Acessing Gift Service, Add Gift {viewModel.Title} to User {viewModel.UserId}");
            var createdGift = await GiftService.AddGift(Mapper.Map<Gift>(viewModel));

            Logger.LogInformation($"Returning new Gift Id:{createdGift.Id} location.");
            return CreatedAtAction(nameof(GetGift), new { id = createdGift.Id }, Mapper.Map<GiftViewModel>(createdGift));
        }

        // GET api/Gift/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ICollection<GiftViewModel>>> GetGiftsForUser(int userId)
        {
            if (userId <= 0)
            {
                return NotFound();
            }
            List<Gift> databaseUsers = await GiftService.GetGiftsForUser(userId);

            return Ok(databaseUsers.Select(x => Mapper.Map<GiftViewModel>(x)).ToList());
        }
    }
}
