using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SecretSanta.Web.UITests.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SecretSanta.Web.UITests
{
    [TestClass]
    public class NavigationTests : BasePageTest
    {
        [TestInitialize]
        public void Init()
        {
            Driver = new ChromeDriver(Path.GetFullPath("."));
        }

        [TestCleanup]
        public void Cleanup()
        {
            Driver.Quit();
            Driver.Dispose();
        }
        [TestMethod]
        public void FromHomePage_ToUsersPage_ByUsersLink()
        {
            Driver.Navigate().GoToUrl(RootUrl);

            var homePage = new HomePage(Driver);
            homePage.UsersLink.Click();

            Assert.IsTrue(Driver.Url.EndsWith(UsersPage.Slug));
        }

        [TestMethod]
        public void FromHomePage_ToGroupsPage_BySecretSantaLink()
        {
            Driver.Navigate().GoToUrl(RootUrl);

            var homePage = new HomePage(Driver);
            homePage.GroupsLink.Click();

            Assert.IsTrue(Driver.Url.EndsWith(GroupsPage.Slug));
        }

        [TestMethod]
        public void FromUsersPage_ToHomePage_BySecretSantaLink()
        {
            Driver.Navigate().GoToUrl($"{RootUrl}{UsersPage.Slug}");
            UsersPage usersPage = new UsersPage(Driver);

            usersPage.HomePageLink.Click();

            Assert.AreEqual<string>(RootUrl, Driver.Url);
        }

        [TestMethod]
        public void FromUsersPage_ToAddUserPage_ByAddUserLink()
        {
            var usersPage = CreateUsersPage();

            usersPage.AddUser.Click();

            Assert.IsTrue(Driver.Url.EndsWith(AddUserPage.Slug));
        }

        [TestMethod]
        public void FromUsersPage_ToGroupsPage_ByGroupsLink()
        {
            Driver.Navigate().GoToUrl($"{RootUrl}{UsersPage.Slug}");
            UsersPage usersPage = new UsersPage(Driver);

            usersPage.GroupsPageLink.Click();

            Assert.AreEqual<string>($"{RootUrl}{GroupsPage.Slug}", Driver.Url);
        }

        [TestMethod]
        public void FromUsersPage_ToGiftsPage_ByGiftsLink()
        {
            Driver.Navigate().GoToUrl($"{RootUrl}{UsersPage.Slug}");
            UsersPage usersPage = new UsersPage(Driver);

            usersPage.GiftsPageLink.Click();

            Assert.AreEqual<string>($"{RootUrl}{GiftsPage.Slug}", Driver.Url);
        }

        [TestMethod]
        public void FromAddUserPage_ToHomePage_BySecretSantaLink()
        {
            Driver.Navigate().GoToUrl($"{RootUrl}{AddUserPage.Slug}");
            AddUserPage addUserPage = new AddUserPage(Driver);

            addUserPage.HomePageLink.Click();

            Assert.AreEqual<string>(RootUrl, Driver.Url);
        }

        [TestMethod]
        public void FromAddUserPage_ToUsersPage_ByCancelLink()
        {
            Driver.Navigate().GoToUrl($"{RootUrl}{AddUserPage.Slug}");
            AddUserPage addUserPage = new AddUserPage(Driver);

            addUserPage.CancelLink.Click();

            Assert.AreEqual<string>($"{RootUrl}{UsersPage.Slug}", Driver.Url);
        }

        [TestMethod]
        public void FromGroupPage_ToAddGroupsPage_ByAddGroupLink()
        {
            var page = CreateGroupsPage();

            page.AddGroup.Click();

            Assert.IsTrue(Driver.Url.EndsWith(AddGroupsPage.Slug));
        }
    }
}
