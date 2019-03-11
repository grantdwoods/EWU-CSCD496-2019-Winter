using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SecretSanta.Web.UITests
{
    [TestClass]
    public class GroupPageTests
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
        public void CanGetToGroupsPage()
        {
            //Arrange
            Driver.Navigate().GoToUrl(RootUrl);

            //Act
            var homePage = new HomePage(Driver);
            homePage.GroupsLink.Click();

            //Assert
            Assert.IsTrue(Driver.Url.EndsWith(GroupsPage.Slug));
        }

        [TestMethod]
        public void CanNavigateToAddGroupsPage()
        {
            //Arrange
            var rootUri = new Uri(RootUrl);
            Driver.Navigate().GoToUrl(new Uri(rootUri, GroupsPage.Slug));
            var page = new GroupsPage(Driver);

            //Act
            page.AddGroup.Click();

            //Assert
            Assert.IsTrue(Driver.Url.EndsWith(AddGroupsPage.Slug));
        }

        [TestMethod]
        public void CanAddGroups()
        {
            //Arrange /Act
            string groupName = "Group Name" + Guid.NewGuid().ToString("N");
            GroupsPage page = CreateGroup(groupName);
            
            //Assert
            Assert.IsTrue(Driver.Url.EndsWith(GroupsPage.Slug));
            List<string> groupNames = page.GroupNames;
            Assert.IsTrue(groupNames.Contains(groupName));
        }

        [TestMethod]
        public void CanDeleteGroup()
        {
            //Arrange
            string groupName = "Group Name" + Guid.NewGuid().ToString("N");
            GroupsPage page = CreateGroup(groupName);

            //Act
            IWebElement deleteLink = page.GetDeleteLink(groupName);
            deleteLink.Click();
            Driver.SwitchTo().Alert().Accept();

            //Assert
            List<string> groupNames = page.GroupNames;
            Assert.IsFalse(groupNames.Contains(groupName));
        }

        private GroupsPage CreateGroup(string groupName)
        {
            var rootUri = new Uri(RootUrl);
            Driver.Navigate().GoToUrl(new Uri(rootUri, GroupsPage.Slug));
            var page = new GroupsPage(Driver);
            page.AddGroup.Click();

            var addGroupPage = new AddGroupsPage(Driver);
            
            addGroupPage.GroupNameTextBox.SendKeys(groupName);
            addGroupPage.SubmitButton.Click();
            return page;
        }
    }
}
