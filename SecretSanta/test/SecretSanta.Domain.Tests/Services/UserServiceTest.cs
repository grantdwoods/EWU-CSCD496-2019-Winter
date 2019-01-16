using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Tests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        private SqliteConnection SqliteConnection { get; set; }
        private DbContextOptions<ApplicationDbContext> Options { get; set; }

        [TestInitialize]
        public void OpenConnection()
        {
            SqliteConnection = new SqliteConnection("DataSource=:memory:");
            SqliteConnection.Open();
            
            Options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(SqliteConnection)
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
        public void AddUser()
        {
            UserService userService;

            User user = new User
            {
                FirstName = "Inigo",
                LastName = "Montoya"
            };

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                userService = new UserService(context);

                userService.AddUser(user);
            }

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                userService = new UserService(context);

                user = userService.Find(1);

                Assert.AreEqual("Inigo", user.FirstName);
            }
        }

        [TestMethod]
        public void UpdateUser()
        {
            UserService userService;

            User user = new User
            {
                FirstName = "Inigo",
                LastName = "Montoya"
            };

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                userService = new UserService(context);

                userService.UpdateUser(user);
            }

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                userService = new UserService(context);

                user = userService.Find(1);
                user.FirstName = "Princess";
                user.LastName = "Buttercup";

                userService.UpdateUser(user);
            }

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                userService = new UserService(context);

                user = userService.Find(1);

                Assert.AreEqual("Princess", user.FirstName);
            }
        }
    }
}
