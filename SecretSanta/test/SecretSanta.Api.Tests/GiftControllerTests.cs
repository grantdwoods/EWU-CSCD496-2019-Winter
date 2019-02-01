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

            var result = (OkObjectResult)controller.GetGiftsForUser(4);

            var resultGifts = (List<DTO.Gift>)result.Value;
            DTO.Gift resultGift = resultGifts.Single();

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

            var result = (NotFoundResult)controller.GetGiftsForUser(-1);

            Assert.AreEqual<int?>(404, result.StatusCode);
            MockGiftService.Verify(x => x.GetGiftsForUser(-1), Times.Never());
        }

        [TestMethod]
        public void PostGiftToUser_ValidInput_Returns201ResultWithUrlAndDTO()
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
        public void PostGiftToUser_NullGift_Returns400()
        {
            var controller = new GiftController(MockGiftService.Object);

            var result = (BadRequestResult)controller.PostGiftToUser(2, null);

            Assert.AreEqual<int?>(400, result.StatusCode);
            MockGiftService
                .Verify(x => x.AddGiftToUser(It.IsAny<int>(),It.IsAny<Gift>()), Times.Never);
        }

        [TestMethod]
        public void PostGiftToUser_UserIDlessThanOrEqualToZero_Returns400()
        {
            var gift = Mocker.CreateInstance<DTO.Gift>();

            var controller = new GiftController(MockGiftService.Object);
      
            var result = (BadRequestResult)controller.PostGiftToUser(0, gift);

            Assert.AreEqual<int?>(400, result.StatusCode);
            MockGiftService
                .Verify(x => x.AddGiftToUser(It.IsAny<int>(), It.IsAny<Gift>()), Times.Never);
        }

        [TestMethod]
        public void PutGiftForUser_ValidInput_Returns201()
        {
            var gift = Mocker.CreateInstance<DTO.Gift>();
            var domainGift = Mocker.CreateInstance<Gift>();

            MockGiftService.Setup(x => x.UpdateGiftForUser(It.IsAny<int>(), It.IsAny<Gift>()))
                .Returns(domainGift).Verifiable();
            var controller = new GiftController(MockGiftService.Object);

            var results = (OkObjectResult)controller.PutUserGift(2, gift);

            Assert.AreEqual<int?>(200, results.StatusCode);
            Assert.AreEqual<string>("Gift updated!", results.Value.ToString());
            MockGiftService.VerifyAll();
        }

        [TestMethod]
        public void PutGiftForUser_NullGift_Returns400()
        {
            var controller = new GiftController(MockGiftService.Object);

            var results = (BadRequestResult)controller.PutUserGift(2, null);

            Assert.AreEqual<int?>(400, results.StatusCode);
            MockGiftService.Verify(x => x.UpdateGiftForUser(2, null), Times.Never);
        }

        [TestMethod]
        public void DeleteGift_Returns200()
        {
            DTO.Gift gift = Mocker.CreateInstance<DTO.Gift>();

            MockGiftService.Setup(x => x.RemoveGift(It.IsAny<Gift>())).Verifiable();
            var controller = new GiftController(MockGiftService.Object);

            var results = (OkObjectResult)controller.DeleteGift(gift);

            Assert.AreEqual<int?>(200, results.StatusCode);
            MockGiftService.VerifyAll();
        }

        [TestMethod]
        public void DeleteGift_NullGift_Returns400()
        {
            var controller = new GiftController(MockGiftService.Object);

            var results = (BadRequestResult)controller.DeleteGift(null);

            Assert.AreEqual<int?>(400, results.StatusCode);
            MockGiftService.Verify(x => x.RemoveGift(It.IsAny<Gift>()), Times.Never);
        }
    }
}
