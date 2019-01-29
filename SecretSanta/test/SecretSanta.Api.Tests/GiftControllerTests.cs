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
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GiftController_RequiresGiftService()
        {
            new GiftController(null);
        }

        [TestMethod]
        public void GetGiftsForUser_ReturnsUserGiftList()
        {
            var mocker = new AutoMocker();
            
            List<Gift> gifts = new List<Gift>();
            var gift = Mock.Of<Gift>(g =>
            g.UserId == 4 &&
            g.Title == "A GIFT" &&
            g.Id == 1 &&
            g.Url == "www.theonlygift.com" &&
            g.OrderOfImportance == 9000);
            gifts.Add(gift);

            var mockGiftService = mocker.GetMock<IGiftService>();
            mockGiftService.Setup(x => x.GetGiftsForUser(4)).Returns(gifts).Verifiable();
            var controller = new GiftController(mockGiftService.Object);

            ActionResult<List<DTO.Gift>> result = controller.GetGiftsForUser(4);

            DTO.Gift resultGift = result.Value.Single();

            Assert.AreEqual<int>(gift.UserId, resultGift.UserID);
            Assert.AreEqual<int>(gift.Id, resultGift.Id);
            Assert.AreEqual<string>(gift.Title, resultGift.Title);
            Assert.AreEqual<string>(gift.Description, resultGift.Description);
            Assert.AreEqual<string>(gift.Url, resultGift.Url);
            Assert.AreEqual<int>(gift.OrderOfImportance, resultGift.OrderOfImportance);
            mockGiftService.VerifyAll();
        }

        [TestMethod]
        public void GetGiftForUser_RequiresPositiveUserId()
        {
            var mocker = new AutoMocker();
            var mockGiftService = mocker.GetMock<IGiftService>();
            var controller = new GiftController(mockGiftService.Object);

            ActionResult<List<DTO.Gift>> result = controller.GetGiftsForUser(-1);

            Assert.IsTrue(result.Result is NotFoundResult);

            mockGiftService.Verify(x => x.GetGiftsForUser(-1), Times.Never());
        }

        [TestMethod]
        public void AddGiftToUser_ValidInput_Returns201ResultWithUrlAndDTO()
        {
            var mocker = new AutoMocker();
            var gift = mocker.CreateInstance<Gift>();

            var mockGiftService = mocker.GetMock<IGiftService>();
            mockGiftService.Setup(x => x.AddGiftToUser(It.IsAny<int>(), It.IsAny<Gift>()))
                .Callback((int userid, Gift giftIn) => { giftIn.UserId = userid;})
                .Returns(gift).Verifiable();

            var controller = new GiftController(mockGiftService.Object);
            var result = (CreatedResult)controller.PostGiftToUser(2, gift);
            
            var returendGift = (DTO.Gift)result.Value;

            Assert.AreEqual<int?>(201, result.StatusCode);
            Assert.AreEqual<int>(2, returendGift.UserID);
            Assert.AreEqual<string>($"api/gift/{returendGift.UserID}", result.Location);
            mockGiftService.VerifyAll();
        }

        [TestMethod]
        public void AddGiftToUser_NullGift_Returns400()
        {
            var mocker = new AutoMocker();
            Gift gift = null;

            var mockGiftService = mocker.GetMock<IGiftService>();
            mockGiftService.Setup(x => x.AddGiftToUser(It.IsAny<int>(), It.IsAny<Gift>()));

            var controller = new GiftController(mockGiftService.Object);
            var result = (BadRequestResult)controller.PostGiftToUser(2, gift);

            Assert.AreEqual<int?>(400, result.StatusCode);
            mockGiftService
                .Verify(x => x.AddGiftToUser(It.IsAny<int>(),It.IsAny<Gift>()), Times.Never);
        }

        [TestMethod]
        public void AddGiftToUser_UserIDlessThanOrEqualToZero_Returns400()
        {
            var mocker = new AutoMocker();
            var gift = mocker.CreateInstance<Gift>();

            var mockGiftService = mocker.GetMock<IGiftService>();
            mockGiftService.Setup(x => x.AddGiftToUser(It.IsAny<int>(), It.IsAny<Gift>()));

            var controller = new GiftController(mockGiftService.Object);
            var result = (BadRequestResult)controller.PostGiftToUser(0, gift);

            Assert.AreEqual<int?>(400, result.StatusCode);
            mockGiftService
                .Verify(x => x.AddGiftToUser(It.IsAny<int>(), It.IsAny<Gift>()), Times.Never);
        }
    }
}
