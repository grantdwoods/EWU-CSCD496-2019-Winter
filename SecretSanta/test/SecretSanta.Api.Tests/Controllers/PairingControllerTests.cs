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
            Mocker.Use(Mapper.Instance);
        }

        [TestMethod]
        [DataRow(new int[] { 1, 2, 3, 4, 5 })]
        [DataRow(new int[] { 45, 68 })]
        public async Task PostPairing_ValidGroupNumber_ReturnsCreated(int[] ids)
        {
            List<int> userIds = new List<int> (ids);
            int groupId = 1;

            List<Pairing> pairings = CreatePairings(userIds, groupId);

            MockPairingService.Setup(x => x.GeneratePairings(groupId))
                .Returns(Task.FromResult(pairings));

            var controller = Mocker.CreateInstance<PairingController>();

            CreatedResult result = await controller.Post(groupId) as CreatedResult;

            List<PairingViewModel> resultPairings = result.Value as List<PairingViewModel>;
            var firstPairing = resultPairings.First();
            var lastPairing = resultPairings.Last();

            Assert.AreEqual<int>(groupId, firstPairing.GroupId);
            Assert.AreEqual<int>(userIds.First(), firstPairing.SantaId);
            Assert.AreEqual<int>(userIds[1], firstPairing.RecipientId);
            Assert.AreEqual<int>(userIds.Last(), lastPairing.SantaId);
            Assert.AreEqual<int>(userIds.First(), lastPairing.RecipientId);
            Mocker.VerifyAll();
        }

        [TestMethod]
        public async Task PostPairing_ValidGroupNumberButServiceReturnsNull_ReturnsBadRequestObject()
        {
            int groupId = 1;
            var controller = Mocker.CreateInstance<PairingController>();

            MockPairingService.Setup(x => x.GeneratePairings(It.IsAny<int>()))
                .Returns(Task.FromResult<List<Pairing>>(null));

            BadRequestObjectResult result = await controller.Post(groupId) as BadRequestObjectResult;

            Assert.IsNotNull(result);
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

        [TestMethod]
        public async Task GetPairingsByGroupId_FoundPairings_ReturnsOKwithList()
        {
            List<int> userIds = new List<int> { 1, 2 };
            int groupId = 1;
            List<Pairing> pairings = CreatePairings(userIds, groupId);

            MockPairingService.Setup(x => x.GetPairingsByGroupId(groupId))
                .Returns(Task.FromResult(pairings));

            var controller = Mocker.CreateInstance<PairingController>();

            OkObjectResult result = await controller.Get(groupId) as OkObjectResult;
            List<PairingViewModel> resultPairings = result.Value as List<PairingViewModel>;

            Assert.IsNotNull(result);
            Assert.AreEqual<int>(1, resultPairings.First().GroupId);
            Assert.AreEqual<int>(1, resultPairings.Last().GroupId);
            Mocker.VerifyAll();
        }

        //Pairings do not need to be "randomized" for controller tests.
        private List<Pairing> CreatePairings(List<int> userIds, int groupId)
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

            return pairings;
        }
    }
}
