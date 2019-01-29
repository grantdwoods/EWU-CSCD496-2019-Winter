using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.AutoMock;
using SecretSanta.Api.Controllers;
using SecretSanta.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Api.Tests
{
    class GroupControllerTests
    {
        protected AutoMocker Mocker { get; private set; }
        protected Mock<IGroupService> MockGiftService { get; private set; }

        [TestInitialize]
        public void SetUpMock()
        {
            Mocker = new AutoMocker();
            MockGiftService = Mocker.GetMock<IGroupService>();
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

        }
    }
}
