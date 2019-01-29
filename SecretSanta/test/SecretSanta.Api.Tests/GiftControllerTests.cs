using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using Moq.AutoMock;
using Moq;
using SecretSanta.Domain.Services;

namespace SecretSanta.Api.Tests
{
    [TestClass]
    public class GiftControllerTests
    {
        protected AutoMocker Mocker { get; private set; }
        protected Mock<IGiftService> MockGiftService { get; private set; }

        [TestInitialize]
        public void SetUpMock()
        {
            Mocker = new AutoMocker();
            MockGiftService = Mocker.GetMock<IGiftService>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GiftController_RequiresGiftService()
        {
            new GiftController(null);
        }

        [TestMethod]
        public void GetGiftsForUser_ReturnsUserGiftList()
        {
            List<Gift> gifts = new List<Gift>();
            var gift = Mock.Of<Gift>(g =>
            g.UserId == 4 &&
            g.Title == "A GIFT" &&
            g.Id == 1 &&
            g.Url == "www.theonlygift.com" &&
            g.OrderOfImportance == 9000);
            gifts.Add(gift);

            MockGiftService.Setup(x => x.GetGiftsForUser(4)).Returns(gifts).Verifiable();

            var controller = new GiftController(MockGiftService.Object);

            ActionResult<List<DTO.Gift>> result = controller.GetGiftsForUser(4);

            DTO.Gift resultGift = result.Value.Single();

            Assert.AreEqual<int>(gift.UserId, resultGift.UserId);
            Assert.AreEqual<int>(gift.Id, resultGift.Id);
            Assert.AreEqual<string>(gift.Title, resultGift.Title);
            Assert.AreEqual<string>(gift.Description, resultGift.Description);
            Assert.AreEqual<string>(gift.Url, resultGift.Url);
            Assert.AreEqual<int>(gift.OrderOfImportance, resultGift.OrderOfImportance);
            MockGiftService.VerifyAll();
        }

        [TestMethod]
        public void GetGiftForUser_RequiresPositiveUserId()
        {
            var controller = new GiftController(MockGiftService.Object);

            ActionResult<List<DTO.Gift>> result = controller.GetGiftsForUser(-1);

            Assert.IsTrue(result.Result is NotFoundResult);

            MockGiftService.Verify(x => x.GetGiftsForUser(-1), Times.Never());
        }

        [TestMethod]
        public void AddGiftToUser_ValidInput_Returns201ResultWithUrlAndDTO()
        {
            var gift = Mocker.CreateInstance<DTO.Gift>();

            MockGiftService.Setup(x => x.AddGiftToUser(It.IsAny<int>(), It.IsAny<Gift>()))
                .Callback((int userid, Gift giftIn) => { giftIn.UserId = userid; });

            var controller = new GiftController(MockGiftService.Object);
            var result = (CreatedResult)controller.PostGiftToUser(2, gift);
            
            var returendGift = (DTO.Gift)result.Value;

            Assert.AreEqual<int?>(201, result.StatusCode);
            Assert.AreEqual<int>(2, returendGift.UserId);
            Assert.AreEqual<string>($"api/Gift/{returendGift.UserId}", result.Location);
            MockGiftService.VerifyAll();
        }

        [TestMethod]
        public void AddGiftToUser_NullGift_Returns400()
        {
            DTO.Gift gift = null;

            MockGiftService.Setup(x => x.AddGiftToUser(It.IsAny<int>(), It.IsAny<Gift>()));

            var controller = new GiftController(MockGiftService.Object);
            var result = (BadRequestResult)controller.PostGiftToUser(2, gift);

            Assert.AreEqual<int?>(400, result.StatusCode);
            MockGiftService
                .Verify(x => x.AddGiftToUser(It.IsAny<int>(),It.IsAny<Gift>()), Times.Never);
        }

        [TestMethod]
        public void AddGiftToUser_UserIDlessThanOrEqualToZero_Returns400()
        {
            var gift = Mocker.CreateInstance<DTO.Gift>();

            MockGiftService.Setup(x => x.AddGiftToUser(It.IsAny<int>(), It.IsAny<Gift>()));

            var controller = new GiftController(MockGiftService.Object);
            var result = (BadRequestResult)controller.PostGiftToUser(0, gift);

            Assert.AreEqual<int?>(400, result.StatusCode);
            MockGiftService
                .Verify(x => x.AddGiftToUser(It.IsAny<int>(), It.IsAny<Gift>()), Times.Never);
        }

        [TestMethod]
        public void UpdateGiftForUser_ValidInput_Returns201()
        {
            var gift = Mocker.CreateInstance<DTO.Gift>();
            var domainGift = Mocker.CreateInstance<Gift>();


            MockGiftService.Setup(x => x.UpdateGiftForUser(It.IsAny<int>(), It.IsAny<Gift>()))
                .Returns(domainGift).Verifiable();

            var controller = new GiftController(MockGiftService.Object);

            OkObjectResult results = (OkObjectResult)controller.PutUserGift(2, gift);

            Assert.AreEqual<int?>(200, results.StatusCode);
            Assert.AreEqual<string>("Gift updated!", results.Value.ToString());
            MockGiftService.VerifyAll();
        }

        [TestMethod]
        public void UpdateGiftForUser_NullGift_Returns400()
        {
            DTO.Gift gift = null;
            Gift domainGift = null;
            MockGiftService = Mocker.GetMock<IGiftService>();

            MockGiftService.Setup(x => x.UpdateGiftForUser(2, domainGift)).Verifiable();

            var controller = new GiftController(MockGiftService.Object);

            BadRequestResult results = (BadRequestResult)controller.PutUserGift(2, gift);

            Assert.AreEqual<int?>(400, results.StatusCode);
            MockGiftService.Verify(x => x.UpdateGiftForUser(2, domainGift), Times.Never);
        }

        [TestMethod]
        public void RemoveGift_Returns200()
        {
            DTO.Gift gift = Mocker.CreateInstance<DTO.Gift>();

            MockGiftService.Setup(x => x.RemoveGift(It.IsAny<Gift>())).Verifiable();

            var controller = new GiftController(MockGiftService.Object);

            OkObjectResult results = (OkObjectResult)controller.DeleteGift(gift);

            Assert.AreEqual<int?>(200, results.StatusCode);
            MockGiftService.VerifyAll();
        }

        private Gift DtoToDomain(DTO.Gift gift)
        {
            Gift domainGift = new Gift
            {
                Id = gift.Id,
                Title = gift.Title,
                Description = gift.Description,
                Url = gift.Url,
                UserId = gift.UserId,
                OrderOfImportance = gift.OrderOfImportance
            };
            return domainGift;
        }

    }
}
