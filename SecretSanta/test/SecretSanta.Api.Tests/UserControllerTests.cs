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
    public class UserControllerTests
    {
        protected AutoMocker Mocker { get; private set; }
        [TestInitialize]
        public void CreateMocker()
        {
            Mocker = new AutoMocker();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserController_RequiresUserService()
        {
            new UserController(null);
        }

        [TestMethod]
        public void AddUser_ValidInput_Returns201WithUrlAndObjectCreated()
        {
            var mockUserService = Mocker.GetMock<IUserService>();
            DTO.User user = new DTO.User {
                FirstName = "Grant", LastName = "Woods", Id = 1};

            var controller = new UserController(mockUserService.Object);
            var result = (CreatedResult)controller.PostUser(user);

            Assert.AreEqual<int?>(201, result.StatusCode);
        }
    }
}