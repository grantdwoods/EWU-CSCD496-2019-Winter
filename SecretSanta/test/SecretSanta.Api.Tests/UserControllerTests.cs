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
        protected Mock<IUserService> MockUserService { get; private set; }
        [TestInitialize]
        public void SetUpMock()
        {
            Mocker = new AutoMocker();
            MockUserService = Mocker.GetMock<IUserService>();
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
            DTO.User user = new DTO.User {
                FirstName = "Grant", LastName = "Woods"};
            MockUserService.Setup(x => x.AddUser(It.IsAny<User>()))
                .Callback((User u) => { u.Id = 1; });

            var controller = new UserController(MockUserService.Object);
            var result = (CreatedResult)controller.PostUser(user);

            DTO.User returnedUser = (DTO.User)result.Value;
            Assert.AreEqual<int?>(201, result.StatusCode);
            Assert.AreNotEqual<int>(0, returnedUser.Id);
            Assert.AreEqual<string>($"api/User/{returnedUser.Id}", result.Location);
            Assert.AreEqual<string>("Grant", returnedUser.FirstName);
            Assert.AreEqual<string>("Woods", returnedUser.LastName);

            MockUserService.VerifyAll();
        }

        [TestMethod]
        public void PostUser_NullUser_Returns400()
        {
            MockUserService.Setup(x => x.DeleteUser(It.IsAny<User>())).Verifiable();

            var controller = new UserController(MockUserService.Object);
            var result = (BadRequestResult)controller.PostUser(null);

            Assert.AreEqual<int?>(400, result.StatusCode);
            MockUserService.Verify(x => x.AddUser(It.IsAny<User>()), Times.Never);
        }

        [TestMethod]
        public void PutUser_ValidUser_Rurns200()
        {
            MockUserService.Setup(x => x.UpdateUser(It.IsAny<User>()));
            DTO.User user = new DTO.User{
                FirstName = "Grant",
                LastName = "Woods"
            };

            var controller = new UserController(MockUserService.Object);
            OkObjectResult result = (OkObjectResult)controller.PutUser(user);

            Assert.AreEqual<int?>(200, result.StatusCode);
        }
    }
}