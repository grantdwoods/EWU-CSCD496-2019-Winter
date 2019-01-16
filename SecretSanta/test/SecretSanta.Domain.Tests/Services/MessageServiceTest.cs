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
    public class MessageServiceTests
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
        
        private Message CreateMessage()
        {
            User toUser = new User { FirstName = "Grant", LastName = "Woods" };
            User fromUser = new User { FirstName = "Tnarg", LastName = "Sdoow" };
            Message message = new Message
            {
                ToUser = toUser,
                FromUser = fromUser,
                Note = "Hello, you."
            };

            return message;
        }
        [TestMethod]
        public void AddMessage()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                MessageService messageService = new MessageService(context);
                Message message = CreateMessage();

                Message messageAdded = messageService.AddMessage(message);

                Assert.AreNotEqual(0, messageAdded.Id);
            }
        }
        [TestMethod]
        public void FindMessage()
        {
            MessageService messageService;
            Message message = CreateMessage();
            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                messageService = new MessageService(context);

                messageService.AddMessage(message);
            }

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                messageService = new MessageService(context);

                Message foundMessage = messageService.Find(1);

                Assert.AreEqual("Hello, you.", foundMessage.Note);
                Assert.AreEqual("Grant", foundMessage.ToUser.FirstName);
                Assert.AreEqual("Sdoow", foundMessage.FromUser.LastName);
            }
        }
    }
}