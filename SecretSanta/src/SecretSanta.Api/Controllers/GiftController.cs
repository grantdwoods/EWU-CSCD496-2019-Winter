﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : ControllerBase
    {
        private readonly IGiftService _GiftService;

        public GiftController(IGiftService giftService)
        {
            _GiftService = giftService ?? throw new ArgumentNullException(nameof(giftService));
        }

        // GET api/Gift/5
        [HttpGet("{userId}")]
        public ActionResult<List<DTO.Gift>> GetGiftsForUser(int userId)
        {
            if (userId <= 0)
            {
                return NotFound();
            }
            List<Gift> databaseUsers = _GiftService.GetGiftsForUser(userId);

            return Ok(databaseUsers.Select(x => new DTO.Gift(x)).ToList());
        }

        [HttpPost("{userId, gift}")]
        public ActionResult PostGiftToUser(int userId, DTO.Gift gift)
        {
            if (gift == null || userId <= 0)
            {
                return BadRequest();
            }

            Gift domainGift = DTO.Gift.DtoToDomain(gift);
            _GiftService.AddGiftToUser(userId, domainGift);

            DTO.Gift returnGift = new DTO.Gift(domainGift);

            return Created($"api/Gift/{returnGift.UserId}", returnGift);
        }

        [HttpPut("{userId, gift}")]
        public ActionResult PutUserGift(int userId, DTO.Gift gift)
        {
            if (gift == null || userId <= 0)
            {
                return BadRequest();
            }

            Gift domainGift = DTO.Gift.DtoToDomain(gift);
            Gift updatedGift = _GiftService.UpdateGiftForUser(userId, domainGift);

            if (GiftsAreEqual(updatedGift, gift))
            {
                return Ok("Gift updated!");
            }
            return BadRequest();
        }

        private bool GiftsAreEqual(Gift updatedGift, DTO.Gift gift)
        {
            return (updatedGift.Description == gift.Description &&
                    updatedGift.Id == gift.Id &&
                    updatedGift.OrderOfImportance == gift.OrderOfImportance &&
                    updatedGift.Url == gift.Url &&
                    updatedGift.UserId == gift.UserId);
        }

        [HttpDelete("{gift}")]
        public ActionResult DeleteGift(DTO.Gift gift)
        {
            if(gift == null)
            {
                return BadRequest();
            }

            Gift domainGift = DTO.Gift.DtoToDomain(gift);
            _GiftService.RemoveGift(domainGift);
            return Ok("Gift removed!");
        }
    }
}
