using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PairingController : ControllerBase
    {
        private IPairingService PairingService { get; }
        private IMapper Mapper { get; }

        public PairingController(IPairingService pairingService, IMapper mapper)
        {
            PairingService = pairingService;
            Mapper = mapper;
        }

        [HttpPost]
        [Produces(typeof(List<PairingViewModel>))]
        public async Task<IActionResult> Post(int groupId)
        {
            List<Pairing> domainPairings = await PairingService.GeneratePairings(groupId);

            if(domainPairings == null)
            {
                return BadRequest("There must be two or more users in a group to generate parings.");
            }
            List<PairingViewModel> viewPairings = domainPairings.Select(x => Mapper.Map<PairingViewModel>(x)).ToList();

            return Created($"Pairing/{groupId}" ,viewPairings);
        }

        [HttpGet("{groupId}")]
        [Produces(typeof(List<Pairing>))]
        public async Task<IActionResult> Get(int groupId)
        {
            List<Pairing> domainPairings = await PairingService.GetPairingsByGroupId(groupId);
            if(domainPairings == null)
            {
                return NotFound();
            }
            List<PairingViewModel> viewPairings = domainPairings.Select(x => Mapper.Map<PairingViewModel>(x)).ToList();

            return Ok(viewPairings);
        }
    }
}
