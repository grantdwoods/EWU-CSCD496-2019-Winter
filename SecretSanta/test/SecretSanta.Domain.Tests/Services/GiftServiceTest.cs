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
    public class GiftServiceTests
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
        private Gift CreateGift()
        {
            User user = new User { FirstName = "G", LastName = "W" };
            Gift gift = new Gift
            {
                User = user,
                Importance = 5,
                Description = "A really cool thing!",
                Url = "shophere.com"
            };
            return gift;
        }
        [TestMethod]
        public void AddGift()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                GiftService giftService = new GiftService(context);
                Gift gift = CreateGift();
                Gift userGift = giftService.AddGift(gift);
                Assert.AreNotEqual(0,userGift.Id);
            }
        }
        [TestMethod]
        public void FindGift()
        {
            GiftService giftService;
            Gift gift = CreateGift();

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                giftService = new GiftService(context);

                giftService.AddGift(gift);
            }

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                giftService = new GiftService(context);

                Gift foundGift = giftService.Find(1);

                Assert.AreEqual("A really cool thing!", foundGift.Description);
                Assert.AreEqual(5, foundGift.Importance);
            }
        }

        [TestMethod]
        public void UpdateGift()
        {
            GiftService giftService;
            Gift gift = CreateGift();

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                giftService = new GiftService(context);

                giftService.AddGift(gift);
            }

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                giftService = new GiftService(context);

                Gift userGift = giftService.Find(1);

                userGift.Description = "This isnt very cool anymore...";
                userGift.Importance = 1;
                giftService.UpdateGift(userGift);
            }

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                giftService = new GiftService(context);
                Gift userGift = giftService.Find(1);

                Assert.AreEqual("This isnt very cool anymore...", userGift.Description);
                Assert.AreEqual(1, userGift.Importance);
            }
        }
        [TestMethod]
        public void RemoveGift()
        {
            GiftService giftService;
            Gift gift = CreateGift();

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                giftService = new GiftService(context);

                giftService.AddGift(gift);
            }

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                giftService = new GiftService(context);

                giftService.RemoveGift(gift);

                Assert.IsNull(giftService.Find(gift.Id));
            }
        }

        [TestMethod]
        public void FetchAllUserGifts()
        {
            GiftService giftService;
            Gift firstGift = CreateGift();
            Gift secondGift = CreateGift();
            
            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                giftService = new GiftService(context);

                giftService.AddGift(firstGift);
                giftService.AddGift(secondGift);
            }

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                giftService = new GiftService(context);

                List<Gift> userGifts = giftService.FetchAllUserGifts(1);
                foreach(Gift g in userGifts)
                {
                    Assert.AreEqual("G", g.User.FirstName);
                }
            }
        }
    }
}

