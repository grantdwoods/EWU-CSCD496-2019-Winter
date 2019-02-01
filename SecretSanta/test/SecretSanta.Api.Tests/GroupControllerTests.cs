using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Domain.Models;
using System.Collections.Generic;
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
        public void PostGroup_ValidGroup_Returns201WithUrlAndObject()
        {
            DTO.Group group = new DTO.Group { Name = "The Group" };

            MockGroupService.Setup(x => x.AddGroup(It.IsAny<Group>()))
                .Callback((Group g) => g.Id = 1);
            var controller = new GroupController(MockGroupService.Object);
            
            var result = (CreatedResult)controller.PostGroup(group);

            var returnedGroup = (DTO.Group)result.Value;

            Assert.AreEqual<int?>(201, result.StatusCode);
            Assert.AreNotEqual<int>(0, returnedGroup.Id);
            MockGroupService.VerifyAll();
        }

        [TestMethod]
        public void PostGroup_NullGroup_Returns400()
        {
            var controller = new GroupController(MockGroupService.Object);

            var result = (BadRequestResult)controller.PostGroup(null);

            Assert.AreEqual<int?>(400, result.StatusCode);
            MockGroupService.Verify(x => x.AddGroup(It.IsAny<Group>()), Times.Never);
        }

        [TestMethod]
        public void PostUserToGroup_GroupAndUserIdPositive_Returns201WithUrlAndObject()
        {
            MockGroupService.Setup(x => x.AddUserToGroup(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new Group { Id = 1}).Verifiable();
            var controller = new GroupController(MockGroupService.Object);

            var result = (CreatedResult)controller.PostUserToGroup(1,1);

            var returnedGroup = (DTO.Group)result.Value;

            Assert.AreEqual<int?>(201, result.StatusCode);
            Assert.AreEqual<int>(1, returnedGroup.Id);
            Assert.AreEqual<string>($"api/Group/{returnedGroup.Id}", result.Location);
            MockGroupService.VerifyAll();
        }

        [TestMethod]
        [DataRow(1,0)]
        [DataRow(0,1)]
        [DataRow(0,0)]
        [DataRow(-1,1)]
        public void PostUserToGroup_GroupOrUserIdLessThanOne_Return400(int groupId, int userId)
        {
            var controller = new GroupController(MockGroupService.Object);

            var result = (BadRequestResult)controller.PostUserToGroup(groupId, userId);

            Assert.AreEqual<int?>(400, result.StatusCode);
            MockGroupService.Verify(x => 
                x.AddUserToGroup(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void GetUsersInGroup_GroupIdPositive_Returns200WithListofUsers()
        {
            var users = new List<User>
            {
                new User { FirstName = "Grant" },
                new User { LastName = "Woods" }
            };

            MockGroupService.Setup(x => x.GetUsers(1)).Returns(users).Verifiable();
            var controller = new GroupController(MockGroupService.Object);

            var results = (OkObjectResult)controller.GetUsersInGroup(1);

            var returnedUsers = (List<DTO.User>)results.Value;

            MockGroupService.VerifyAll();
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void GetUsersInGroup_GroupIdLessthanOne_Return404(int groupId)
        {
            var controller = new GroupController(MockGroupService.Object);

            var result = (NotFoundResult)controller.GetUsersInGroup(groupId);

            Assert.AreEqual<int?>(404, result.StatusCode);
            MockGroupService.Verify(x => x.GetUsers(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void PutGroup_ValidGroup_Returns200WithUpdatedObject()
        {
            DTO.Group group = new DTO.Group { Name = "Testing", Id = 1 };

            MockGroupService.Setup(x => x.UpdateGroup(It.IsAny<Group>()))
                .Callback((Group g) => { g.Name = "UPDATED"; });
            var controller = new GroupController(MockGroupService.Object);

            var results = (OkObjectResult) controller.PutGroup(group);

            var updatedGroup = (DTO.Group)results.Value;

            Assert.AreEqual<int?>(200, results.StatusCode);
            Assert.AreEqual<string>("UPDATED", updatedGroup.Name);
            Assert.AreEqual<int>(1, updatedGroup.Id);
            MockGroupService.VerifyAll();
        }

        [TestMethod]
        public void PutGroup_NullGroup_Returns400()
        {
            var controller = new GroupController(MockGroupService.Object);

            var result = (BadRequestResult)controller.PutGroup(null);

            Assert.AreEqual<int?>(400, result.StatusCode);
            MockGroupService.Verify(x => x.UpdateGroup(It.IsAny<Group>()), Times.Never);
        }

        [TestMethod]
        public void DeleteGroup_Returns200WithMessage()
        {
            DTO.Group group = Mocker.CreateInstance<DTO.Group>();
            MockGroupService.Setup(x => x.DeleteGroup(It.IsAny<Group>()));
            var controller = new GroupController(MockGroupService.Object);
            var result = (OkObjectResult) controller.DeleteGroup(group);

            Assert.AreEqual<int?>(200, result.StatusCode);
            MockGroupService.VerifyAll();
        }

        [TestMethod]
        public void DeleteGroup_NullGroup_Returns400()
        {
            var controller = new GroupController(MockGroupService.Object);

            var result = (BadRequestResult)controller.DeleteGroup(null);

            Assert.AreEqual<int?>(400, result.StatusCode);
            MockGroupService.Verify(x => x.DeleteGroup(It.IsAny<Group>()), Times.Never);
        }

        [TestMethod]
        public void GetAllGroups_Returns200WithListOfGroups()
        {
            List<Group> groups = new List<Group>
            { new Group { Name = "Group 1" },
              new Group { Name = "Group 2" } };

            MockGroupService.Setup(x => x.FetchAll()).Returns(groups).Verifiable();

            var controller = new GroupController(MockGroupService.Object);
            var result = (OkObjectResult)controller.GetAllGroups();

            var returnedGroups = (List<DTO.Group>)result.Value;

            Assert.AreEqual<string>("Group 1", returnedGroups[0].Name);
            Assert.AreEqual<string>("Group 2", returnedGroups[1].Name);
            MockGroupService.VerifyAll();
        }
    }
}
