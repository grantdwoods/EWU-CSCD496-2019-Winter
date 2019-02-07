using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SecretSanta.Api.Controllers;
using SecretSanta.Api.Models;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class GroupControllerTests
    {
        private IMapper _Mapper { get; set; }


        [TestInitialize]
        public void SetMapper()
        {
            _Mapper = Mapper.Instance;
        }
        [TestMethod]
        public void GetAllGroups_ReturnsGroups()
        {
            var group1 = new Group
            {
                Id = 1,
                Name = "Group 1"
            };
            var group2 = new Group
            {
                Id = 2,
                Name = "Group 2"
            };

            var service = new Mock<IGroupService>();
            service.Setup(x => x.FetchAll())
                .Returns(new List<Group> { group1, group2 })
                .Verifiable();


            var controller = new GroupController(service.Object, _Mapper);

            var result = (OkObjectResult)controller.GetAllGroups();

            var groups = (List<GroupViewModel>) result.Value;

            Assert.AreEqual(2, groups.Count);
            AssertAreEqual(groups[0], group1);
            AssertAreEqual(groups[1], group2);
            service.VerifyAll();
        }

        [TestMethod]
        public void PostGroup_RequiresGroup()
        {
            var service = new Mock<IGroupService>(MockBehavior.Strict);
            var controller = new GroupController(service.Object, _Mapper);


            var result = (ActionResult)controller.PostGroup(null);
            
            Assert.IsTrue(result is BadRequestResult);
        }

        [TestMethod]
        public void PostGroup_ReturnsCreatedGroup()
        {
            var group = new GroupInputViewModel
            {
                Name = "Group"
            };
            var service = new Mock<IGroupService>();
            service.Setup(x => x.AddGroup(It.Is<Domain.Models.Group>(g => g.Name == group.Name)))
                .Returns(new Domain.Models.Group
                {
                    Id = 2,
                    Name = group.Name
                })
                .Verifiable();

            var controller = new GroupController(service.Object, _Mapper);

            var result = (CreatedAtActionResult)controller.PostGroup(group);
            var groupResult = (GroupViewModel)result.Value;
            Assert.AreEqual(2, groupResult.Id);
            Assert.AreEqual("Group", groupResult.Name);
            service.VerifyAll();
        }

        [TestMethod]
        public void PutGroup_RequiresGroup()
        {
            var service = new Mock<IGroupService>(MockBehavior.Strict);
            var controller = new GroupController(service.Object, _Mapper);


            var result = controller.PutGroup(-1, null);

            Assert.IsTrue(result is BadRequestResult);
        }

        [TestMethod]
        public void PutGroup_ReturnsUpdatedGroup()
        {
            var group = new GroupInputViewModel
            {
                Name = "Group"
            };
            var service = new Mock<IGroupService>();
            service.Setup(x => x.Find(2)).Returns(new Group { Id = 2, Name = "My Group" }).Verifiable();
            service.Setup(x => x.UpdateGroup(It.Is<Group>(g =>
                    g.Name == group.Name)))
                .Returns(new Domain.Models.Group
                {
                    Id = 2,
                    Name = group.Name
                })
                .Verifiable();

            var controller = new GroupController(service.Object, _Mapper);

            var result = (ActionResult)controller.PutGroup(2, group);

            Assert.IsTrue(result is NoContentResult);
            service.VerifyAll();
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        public void DeleteGroup_RequiresPositiveId(int groupId)
        {
            var service = new Mock<IGroupService>(MockBehavior.Strict);
            var controller = new GroupController(service.Object, _Mapper);

            var result = controller.DeleteGroup(groupId);

            Assert.IsTrue(result is BadRequestObjectResult);
        }

        [TestMethod]
        public void DeleteGroup_ReturnsNotFoundWhenTheGroupFailsToDelete()
        {
            var service = new Mock<IGroupService>();
            service.Setup(x => x.DeleteGroup(2))
                .Returns(false)
                .Verifiable();
            var controller = new GroupController(service.Object, _Mapper);

            var result = controller.DeleteGroup(2);

            Assert.IsTrue(result is NotFoundResult);
        }

        [TestMethod]
        public void DeleteGroup_ReturnsOkWhenGroupIsDeleted()
        {
            var service = new Mock<IGroupService>();
            service.Setup(x => x.DeleteGroup(2))
                .Returns(true)
                .Verifiable();
            var controller = new GroupController(service.Object, _Mapper);

            var result = controller.DeleteGroup(2);

            Assert.IsTrue(result is OkResult);
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        public void PutUserToGroup_RequiresPositiveGroupId(int groupId)
        {
            var service = new Mock<IGroupService>(MockBehavior.Strict);
            var controller = new GroupController(service.Object, _Mapper);

            var result = controller.PutUserToGroup(groupId, 1);

            Assert.IsTrue(result is NotFoundResult);
        }


        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        public void PutUserToGroup_RequiresPositiveUserId(int userId)
        {
            var service = new Mock<IGroupService>(MockBehavior.Strict);
            var controller = new GroupController(service.Object, _Mapper);

            var result = controller.PutUserToGroup(1, userId);

            Assert.IsTrue(result is NotFoundResult);
        }

        [TestMethod]
        public void PutUserToGroup_WhenUserFailsToAddToGroupItReturnsNotFound()
        {
            var service = new Mock<IGroupService>();
            service.Setup(x => x.AddUserToGroup(2, 3))
                .Returns(false)
                .Verifiable();
            var controller = new GroupController(service.Object, _Mapper);

            var result = controller.PutUserToGroup(2, 3);

            Assert.IsTrue(result is NotFoundResult);
        }

        [TestMethod]
        public void PutUserToGroup_ReturnsNoContentWhenUserAddedToGroup()
        {
            var service = new Mock<IGroupService>();
            service.Setup(x => x.AddUserToGroup(2, 3))
                .Returns(true)
                .Verifiable();
            var controller = new GroupController(service.Object, _Mapper);

            var result = controller.PutUserToGroup(2, 3);

            Assert.IsTrue(result is NoContentResult);
        }

        private static void AssertAreEqual(GroupViewModel expected, Group actual)
        {
            if (expected == null && actual == null) return;
            if (expected == null || actual == null) Assert.Fail();

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
        }
    }
}
