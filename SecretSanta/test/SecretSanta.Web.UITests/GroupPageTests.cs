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
    public class GroupPageTests : BasePageTest
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
        public void CanAddGroups()
        {
            string groupName = "Group Name" + Guid.NewGuid().ToString("N");
            GroupsPage page = CreateGroup(groupName);

            Assert.IsTrue(Driver.Url.EndsWith(GroupsPage.Slug));
            List<string> groupNames = page.GroupNames;
            Assert.IsTrue(groupNames.Contains(groupName));
        }

        [TestMethod]
        public void CanDeleteGroup()
        {
            string groupName = "Group Name" + Guid.NewGuid().ToString("N");
            GroupsPage page = CreateGroup(groupName);

            IWebElement deleteLink = page.GetDeleteLink(groupName);
            deleteLink.Click();
            Driver.SwitchTo().Alert().Accept();

            List<string> groupNames = page.GroupNames;
            Assert.IsFalse(groupNames.Contains(groupName));
        }
    }
}
