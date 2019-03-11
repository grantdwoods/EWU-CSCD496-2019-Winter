using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SecretSanta.Web.UITests.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SecretSanta.Web.UITests
{
    [TestClass]
    public class UsersPageTests
    {
        private const string RootUrl = "https://localhost:44314/";

        private IWebDriver Driver { get; set; }

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
        public void CanNavigateToUsersPage()
        {
            Driver.Navigate().GoToUrl(RootUrl);

            var homePage = new HomePage(Driver);
            homePage.UsersLink.Click();

            Assert.IsTrue(Driver.Url.EndsWith(UsersPage.Slug));
        }

        [TestMethod]
        public void CanNavigateToAddUserPage()
        {
            var page = CreatePage();

            page.AddUser.Click();

            Assert.IsTrue(Driver.Url.EndsWith(AddUserPage.Slug));
        }
        [TestMethod]
        public void CanAddUser()
        {
            var page = CreatePage();
            page.AddUser.Click();

            GuidUserInfo(out string userFirst, out string userLast);

            CreateUser(userFirst, userLast);

            List<string> users = page.UserNames as List<string>;

            Assert.IsTrue(users.Contains($"{userFirst} {userLast}"));
        }

        [TestMethod]
        public void CanNavigateToEditUserPage()
        {
            var page = CreatePage();

            GuidUserInfo(out string first, out string last);
            CreateUser(first, last);

            page.GetEditLink($"{first} {last}").Click();
            Assert.IsTrue(Driver.Url.EndsWith(EditUserPage.Slug));
        }

        private void GuidUserInfo(out string userFirst, out string userLast)
        {
            string guid = Guid.NewGuid().ToString("N");
            userFirst = "UserFirst" + guid;
            userLast = "UserLast" + guid;
        }
        private AddUserPage CreateUser(string userFirst, string userLast)
        {
            var addpage = new AddUserPage(Driver);
            addpage.FirstNameTextBox.SendKeys(userFirst);
            addpage.LastNameTextBox.SendKeys(userLast);
            addpage.SubmitButton.Click();
            return addpage;
        }

        private UsersPage CreatePage()
        {
            var rootUri = new Uri(RootUrl);
            Driver.Navigate().GoToUrl(new Uri(rootUri, UsersPage.Slug));
            return new UsersPage(Driver);
        }
    }
}
