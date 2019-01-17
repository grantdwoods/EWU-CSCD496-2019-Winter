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
    public class PairingServiceTests
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

        private Pairing CreatePairing()
        {
            Group group = new Group {Title = "Grant's Team", Id = 1};
            User santa = new User { FirstName = "Grant", LastName = "Santa", Id = 1 };
            User recipient = new User { FirstName = "Gug", LastName = "Recipient", Id = 2 };
            Pairing pairing = new Pairing { Group = group, Santa = santa, Recipient = recipient };
            return pairing;
        }
        [TestMethod]
        public void AddPairing()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                PairingService pairingService = new PairingService(context);

                Pairing pairing = CreatePairing();
                Pairing addedPairing = pairingService.AddPairing(pairing);

                Assert.AreNotEqual(0, addedPairing.Id);
            }
        }
        [TestMethod]
        public void FindPairing()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                PairingService pairingService = new PairingService(context);

                Pairing pairing = CreatePairing();
                pairingService.AddPairing(pairing);
            }

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                PairingService pairingService = new PairingService(context);

                Pairing pairing = pairingService.Find(1);

                Assert.AreEqual("Santa", pairing.Santa.LastName);
            }
        }
    }
}