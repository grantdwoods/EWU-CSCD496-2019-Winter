using OpenQA.Selenium;
using SecretSanta.Web.UITests.Pages;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Web.UITests
{
    public class BasePageTest
    {
        protected IWebDriver Driver { get; set; }
        protected const string RootUrl = "https://localhost:44314/";
    
        protected GroupsPage CreateGroup(string groupName)
        {
            var page = CreateGroupsPage();
            page.AddGroup.Click();

            var addGroupPage = new AddGroupsPage(Driver);

            addGroupPage.GroupNameTextBox.SendKeys(groupName);
            addGroupPage.SubmitButton.Click();
            return page;
        }

        protected GroupsPage CreateGroupsPage()
        {
            var rootUri = new Uri(RootUrl);
            Driver.Navigate().GoToUrl(new Uri(rootUri, GroupsPage.Slug));
            return new GroupsPage(Driver);
        }

        protected UsersPage CreateUsersPage()
        {
            var rootUri = new Uri(RootUrl);
            Driver.Navigate().GoToUrl(new Uri(rootUri, UsersPage.Slug));
            return new UsersPage(Driver);
        }
    }
}
