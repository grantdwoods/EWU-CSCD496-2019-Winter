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

        private User CreateUser(string firstName = "G", string lastName = "W", int id = default(int))
        {
            return new User { FirstName = firstName, LastName = lastName, Id = id};
        }
        [TestMethod]
        public void AddUser()
        {
            UserService userService;

            User user = CreateUser();

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                userService = new UserService(context);

                userService.AddUser(user);
            }

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                userService = new UserService(context);

                user = userService.Find(1);

                Assert.AreEqual("G", user.FirstName);
            }
        }

        [TestMethod]
        public void UpdateUser()
        {
            UserService userService;

            User user = CreateUser();

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                userService = new UserService(context);

                userService.UpdateUser(user);
            }

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                userService = new UserService(context);

                user = userService.Find(1);
                user.FirstName = "Grant";
                user.LastName = "Woods";

                userService.UpdateUser(user);
            }

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                userService = new UserService(context);

                user = userService.Find(1);

                Assert.AreEqual("Grant", user.FirstName);
            }
        }
    }
}
