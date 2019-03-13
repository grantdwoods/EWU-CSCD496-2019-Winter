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
        public void CanAddUser()
        {
            GuidUserInfo(out string first, out string last);

            UsersPage usersPage = CreateUser(first, last);

            List<string> users = usersPage.UserNames.ToList();

            Assert.IsTrue(users.Contains($"{first} {last}"));
        }

        [TestMethod]
        public void CanAddUserOptionalLastName()
        {
            GuidUserInfo(out string first, out string last);

            UsersPage usersPage = CreateUser(first, "");

            List<string> users = usersPage.UserNames.ToList();

            Assert.IsTrue(users.Contains(first));
        }

        [TestMethod]
        public void CanNotAddEmptyFirstNameErrorAppears()
        {
            GuidUserInfo(out string first, out string last);
            UsersPage usersPage = CreateUsersPage();
            usersPage.AddUser.Click();
            var addPage = new AddUserPage(Driver);

            Assert.IsFalse(addPage.ErrorElement.Displayed);
            addPage.LastNameTextBox.SendKeys(first);
            addPage.SubmitButton.Click();

            
            List<string> users = usersPage.UserNames.ToList();
            
            Assert.IsFalse(users.Contains(last));
            Assert.IsTrue(addPage.ErrorElement.Displayed);
        }

        [TestMethod]
        public void CanNavigateToEditUserPage()
        {
            GuidUserInfo(out string first, out string last);
            UsersPage usersPage = CreateUser(first, last);

            IWebElement editLink = usersPage.GetEditLink($"{first} {last}");
            string linkText = editLink.GetAttribute("href");
            string userID = (linkText.Substring(linkText.LastIndexOf("/") + 1));
            var editPage = new EditUserPage(Driver);

            editLink.Click();

            Assert.AreEqual<string>(userID, editPage.CurrentUserID);
            Assert.AreEqual<string>(first, editPage.FirstNameTextBox.GetAttribute("value"));
            Assert.AreEqual<string>(last, editPage.LastNameTextBox.GetAttribute("value"));
        }

        [TestMethod]
        public void CanEditUser()
        {
            GuidUserInfo(out string first, out string last);
            UsersPage usersPage = CreateUser(first, last);

            usersPage.GetEditLink($"{first} {last}").Click();
            EditUserPage editPage = new EditUserPage(Driver);
            editPage.FirstNameTextBox.Clear();
            editPage.LastNameTextBox.Clear();

            GuidUserInfo(out string newFirst, out string newLast);
            editPage.FirstNameTextBox.SendKeys(newFirst);
            editPage.LastNameTextBox.SendKeys(newLast);
            editPage.SubmitButton.Click();

            List<string> users = usersPage.UserNames.ToList();

            Assert.IsTrue(users.Contains($"{newFirst} {newLast}"));
            Assert.IsFalse(users.Contains($"{first} {last}"));
        }

        [TestMethod]
        public void CanNotCangeToEmptyFirstNameErrorAppears()
        {
            GuidUserInfo(out string first, out string last);
            UsersPage usersPage = CreateUser(first, last);
            usersPage.GetEditLink($"{first} {last}").Click();
            EditUserPage editPage = new EditUserPage(Driver);
            editPage.FirstNameTextBox.Clear();

            Assert.IsFalse(editPage.ErrorElement.Displayed);
            editPage.SubmitButton.Click();

            List<string> users = usersPage.UserNames.ToList();

            Assert.IsFalse(users.Contains($"{first} {last}"));
            Assert.IsTrue(editPage.ErrorElement.Displayed);
        }

        [TestMethod]
        public void CanDeleteUser()
        {
            GuidUserInfo(out string first, out string last);
            UsersPage usersPage = CreateUser(first, last);

            string userFull = $"{first} {last}";
            usersPage.GetDeleteLink(userFull).Click();
            Driver.SwitchTo().Alert().Accept();

            var users = usersPage.UserNames.ToList();

            Assert.IsFalse(users.Contains(userFull));
        }

        private void GuidUserInfo(out string userFirst, out string userLast)
        {
            string guid = Guid.NewGuid().ToString("N");
            userFirst = "UserFirst" + guid;
            userLast = "UserLast" + guid;
        }
        private UsersPage CreateUser(string userFirst, string userLast)
        {
            var usersPage = CreateUsersPage();
            usersPage.AddUser.Click();
            var addpage = new AddUserPage(Driver);
            addpage.FirstNameTextBox.SendKeys(userFirst);

            if (!string.IsNullOrEmpty(userLast))
            {
                addpage.LastNameTextBox.SendKeys(userLast);
            }
            addpage.SubmitButton.Click();
            return usersPage;
        }

        private UsersPage CreateUsersPage()
        {
            var rootUri = new Uri(RootUrl);
            Driver.Navigate().GoToUrl(new Uri(rootUri, UsersPage.Slug));
            return new UsersPage(Driver);
        }
    }
}
