using System;
using Serilog;
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
        private IGiftService _GiftService { get; }
        private IMapper _Mapper { get; }
        private ILogger<GiftsController> _Logger {get;}
        public GiftsController(IGiftService giftService, IMapper mapper, ILogger<GiftsController> logger)
        {
            _Logger = logger;
            _GiftService = giftService;
            _Mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GiftViewModel>> GetGift(int id)
        {
            var gift = await _GiftService.GetGift(id);

            if (gift == null)
            {
                _Logger.LogInformation($" Get gift 404 No data for {id}.");
                return NotFound();
            }

            return Ok(_Mapper.Map<GiftViewModel>(gift));
        }

        [HttpPost]
        public async Task<ActionResult<GiftViewModel>> CreateGift(GiftInputViewModel viewModel)
        {
            var createdGift = await _GiftService.AddGift(_Mapper.Map<Gift>(viewModel));

            return CreatedAtAction(nameof(GetGift), new { id = createdGift.Id }, _Mapper.Map<GiftViewModel>(createdGift));
        }

        // GET api/Gift/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ICollection<GiftViewModel>>> GetGiftsForUser(int userId)
        {
            if (userId <= 0)
            {
                return NotFound();
            }
            List<Gift> databaseUsers = await _GiftService.GetGiftsForUser(userId);

            return Ok(databaseUsers.Select(x => _Mapper.Map<GiftViewModel>(x)).ToList());
        }
    }
}
