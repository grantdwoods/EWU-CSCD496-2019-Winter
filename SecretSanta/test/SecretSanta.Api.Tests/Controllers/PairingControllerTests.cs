using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.AutoMock;
using SecretSanta.Api.Controllers;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;
using SecretSanta.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class PairingControllerTests
    {
        private AutoMocker Mocker { get; set; }
        private Mock<IPairingService> MockPairingService { get; set; }
        private Mock<IMapper> MockMapper { get; set; }

        [TestInitialize]
        public void SetProperties()
        {
            Mocker = new AutoMocker();
            MockPairingService = Mocker.GetMock<IPairingService>();
            MockMapper = Mocker.GetMock<IMapper>();
        }
        
        private void SetUpMockMapper()
        {
            MockMapper.Setup(x => x.Map<PairingViewModel>(It.IsAny<Pairing>()))
            .Returns((Pairing p) => {
                    return new PairingViewModel
                    {
                        SantaId = p.SantaId,
                        RecipientId = p.RecipientId,
                         GroupId = p.GroupId
                    };
            });
        }
        private Task<List<Pairing>> CreatePairings(List<int> userIds, int groupId)
        {
            var pairings = new List<Pairing>();

            for (int i = 0; i < userIds.Count - 1; i++)
            {
                pairings.Add(new Pairing
                {
                    SantaId = userIds[i],
                    RecipientId = userIds[i + 1],
                    GroupId = groupId
                });
            }

            pairings.Add(new Pairing
            {
                SantaId = userIds.Last(),
                RecipientId = userIds.First(),
                GroupId = groupId
            });

            return Task.FromResult(pairings);
        }

        [TestMethod]
        public async Task PostPairing_ValidGroupNumber_ReturnsCreated()
        {
            List<int> userIds = new List<int> { 1, 2, 3, 4, 5 };
            int groupId = 1;

            Task<List<Pairing>> pairings = CreatePairings(userIds, groupId);

            MockPairingService.Setup(x => x.GeneratePairings(groupId)).Returns(pairings);
            SetUpMockMapper();

            var controller = Mocker.CreateInstance<PairingController>();

            CreatedResult result = await controller.Post(groupId) as CreatedResult;

            List<PairingViewModel> resultPairings = result.Value as List<PairingViewModel>;
            var firstPairing = resultPairings.First();
            var lastPairing = resultPairings.Last();

            Assert.AreEqual<int>(groupId, firstPairing.GroupId);
            Assert.AreEqual<int>(1, firstPairing.SantaId);
            Assert.AreEqual<int>(2, firstPairing.RecipientId);
            Assert.AreEqual<int>(5, lastPairing.SantaId);
            Assert.AreEqual<int>(1, lastPairing.RecipientId);
            Mocker.VerifyAll();
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public async Task PostPairing_RequiresPositiveId_ReturnsBadRequestResult(int groupId)
        {
            var controller = Mocker.CreateInstance<PairingController>();
            MockPairingService = new Mock<IPairingService>(MockBehavior.Strict);

            BadRequestObjectResult result = await controller.Post(groupId) as BadRequestObjectResult;

            Assert.IsNotNull(result);
        }
    }
}
