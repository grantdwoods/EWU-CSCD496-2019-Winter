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
    public class GroupControllerTests
    {
        protected AutoMocker Mocker { get; private set; }
        protected Mock<IGroupService> MockGroupService { get; private set; }

        [TestInitialize]
        public void SetUpMock()
        {
            Mocker = new AutoMocker();
            MockGroupService = Mocker.GetMock<IGroupService>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GiftController_RequiresGiftService()
        {
            new GroupController(null);
        }

        [TestMethod]
        public void PostGroup_ValidGroup_Returns201WithUrlAndObject()
        {
            DTO.Group group = new DTO.Group { Name = "The Group" };

            MockGroupService.Setup(x => x.AddGroup(It.IsAny<Group>()))
                .Callback((Group g) => g.Id = 1);
            var controller = new GroupController(MockGroupService.Object);
            
            CreatedResult result = (CreatedResult)controller.PostGroup(group);

            DTO.Group returnedGroup = (DTO.Group)result.Value;

            Assert.AreEqual<int?>(201, result.StatusCode);
            Assert.AreNotEqual<int>(0, returnedGroup.Id);
            MockGroupService.Verify();
        }

        [TestMethod]
        public void PostUserToGroup_ValidUser_Returns201WithUrlAndObject()
        {
            MockGroupService.Setup(x => x.AddUserToGroup(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new Group { Id = 1}).Verifiable();
            var controller = new GroupController(MockGroupService.Object);

            CreatedResult result = (CreatedResult)controller.PostUserToGroup(1,1);

            DTO.Group returnedGroup = (DTO.Group)result.Value;

            Assert.AreEqual<int?>(201, result.StatusCode);
            Assert.AreEqual<int>(1, returnedGroup.Id);
            MockGroupService.VerifyAll();
        }
    }
}
