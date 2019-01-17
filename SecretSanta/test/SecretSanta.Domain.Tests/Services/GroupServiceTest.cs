using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Tests.Services
{
    [TestClass]
    public class GroupServiceTests
    {
        ILoggerFactory GetLoggerFactory()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole()
                    .AddFilter(DbLoggerCategory.Database.Command.Name,
                               LogLevel.Information);
            });
            return serviceCollection.BuildServiceProvider().
            GetService<ILoggerFactory>();
        }
        private SqliteConnection SqliteConnection { get; set; }
        private DbContextOptions<ApplicationDbContext> Options { get; set; }

        [TestInitialize]
        public void OpenConnection()
        {
            SqliteConnection = new SqliteConnection("DataSource=:memory:");
            SqliteConnection.Open();

            Options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(SqliteConnection)
                .UseLoggerFactory(GetLoggerFactory())
                .EnableSensitiveDataLogging()
                .Options;

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                context.Database.EnsureCreated();
            }
        }
        [TestCleanup]
        public void CloseConnection()
        {
            SqliteConnection.Close();
        }

        [TestMethod]
        public void AddGroup()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                GroupService groupService = new GroupService(context);

                Group group = new Group { Title = "Grant's Team" };
                Group addedGroup = groupService.AddGroup(group);

                Assert.AreNotEqual(0, addedGroup.Id);
            }
        }

        [TestMethod]
        public void FindGroup()
        {
            GroupService groupService;

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                groupService = new GroupService(context);

                Group group = new Group { Title = "Grant's Team" };
                Group addedGroup = groupService.AddGroup(group);
            }

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                groupService = new GroupService(context);
                Group addedGroup = groupService.Find(1);

                Assert.AreEqual("Grant's Team", addedGroup.Title);
            }
        }
        [TestMethod]
        public void AddUserToGroup()
        {
            GroupService groupService;
            UserService userService;

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                groupService = new GroupService(context);
                userService = new UserService(context);
                User user = new User { FirstName = "Grant", LastName = "Woods" };
                userService.AddUser(user);

                Group group = new Group { Title = "Grant's Team" };
                groupService.AddGroup(group);
            }

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                groupService = new GroupService(context);
                userService = new UserService(context);

                Group group = groupService.Find(1);
                User user = userService.Find(1);

                UserGroup userGroup = new UserGroup { User = user, UserId = user.Id,
                    Group = group, GroupId = group.Id };

                groupService.AddUserToGroup(userGroup);
            }

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                UserGroup userGroup = new UserGroup
                {

                };
            }
        }
    }
}