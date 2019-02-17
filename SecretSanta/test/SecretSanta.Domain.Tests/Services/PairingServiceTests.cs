using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.AutoMock;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;
using SecretSanta.Domain.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Domain.Tests.Services
{
    [TestClass]
    public class PairingServiceTests : DatabaseServiceTests
    {
        [TestMethod]
        public async Task GeneratePairings_ValidInput_ReturnsList()
        {
            int groupId = 1;
            int[] userIds = { 10, 20, 30 };
            await SetupValidGroupAndUsers(groupId, userIds);

            int[] randIndices = { 0, 1, 0 };

            var mockRandomService = new Mock<IRandomService>();
            //Next gets called x number of Ids
            mockRandomService.Setup(x => x.Next(2))
                .Returns(randIndices[2]);
            mockRandomService.Setup(x => x.Next(1))
                .Returns(randIndices[1]);
            mockRandomService.Setup(x => x.Next(0))
                .Returns(randIndices[0]);
            //Order becomes 10,30,20

            using (var context = new ApplicationDbContext(Options))
            {
                var pairingService = new PairingService(context, mockRandomService.Object);

                List<Pairing> pairings = await pairingService.GeneratePairings(1);
                Assert.AreEqual<int>(10, pairings.First().SantaId);
                Assert.AreEqual<int>(30, pairings.First().RecipientId);
                Assert.AreEqual<int>(20, pairings.Last().SantaId);
                Assert.AreEqual<int>(10, pairings.Last().RecipientId);
            }
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public async Task GeneratePairings_RequiresPostiveId_ReturnsNull(int invalidId)
        {
            int groupId = 20;
            int[] userIds = { 1, 2, 3 };
            await SetupValidGroupAndUsers(groupId, userIds);
            var mockRandomService = new Mock<IRandomService>(MockBehavior.Strict);

            using (var context = new ApplicationDbContext(Options))
            {
                var pairingService = new PairingService(context, mockRandomService.Object);
                List<Pairing> pairings = await pairingService.GeneratePairings(invalidId);
                Assert.IsNull(pairings);
            }
        }
        [TestMethod]
        [DataRow(1, new int[] { 1 })]
        [DataRow(1, null)]
        public async Task GeneratePairings_NotEnoughGroupMembers_ReturnsNull(int groupId, int[] userIds)
        {
            await SetupInvalidGroupAndUsers(groupId, userIds);
            var mockRandomService = new Mock<IRandomService>(MockBehavior.Strict);

            using (var context = new ApplicationDbContext(Options))
            {
                var pairingService = new PairingService(context, mockRandomService.Object);
                List<Pairing> pairings = await pairingService.GeneratePairings(groupId);
                Assert.IsNull(pairings);
            }
        }

        private async Task SetupValidGroupAndUsers(int groupId, int[] userIds)
        {
            List<GroupUser> groupUsers = new List<GroupUser>();
            List<User> users = new List<User>();

            foreach (int i in userIds)
            {
                groupUsers.Add(new GroupUser { UserId = i, GroupId = groupId });
                users.Add(new User { Id = i });
            }

            var group = new Group
            {
                Name = "The Group",
                GroupUsers = groupUsers
            };

            using (var context = new ApplicationDbContext(Options))
            {
                context.Groups.Add(group);
                context.Users.AddRange(users);
                await context.SaveChangesAsync();
            }
        }

        private async Task SetupInvalidGroupAndUsers(int groupId, int[] userIds)
        {
            if(userIds != null)
            {
                await SetupValidGroupAndUsers(groupId, userIds);
            }
            else
            {
                using (var context = new ApplicationDbContext(Options))
                {
                    context.Groups.Add(new Group { Id = groupId, Name ="The Group"});
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
