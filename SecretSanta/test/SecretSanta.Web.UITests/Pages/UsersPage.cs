using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SecretSanta.Web.UITests.Pages
{
    public class UsersPage
    {
        public const string Slug = "Users";
        public const string UserLinksText = " Edit Delete";
        IWebDriver Driver { get; }
        public IWebElement AddUser => Driver.FindElement(By.LinkText("Add User"));
        public IWebElement HomePageLink => Driver.FindElement(By.LinkText("Secret Santa"));
        public UsersPage(IWebDriver driver)
        {
            Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }
        public IEnumerable<string> UserNames
        {
            get
            {
                var userListItems = Driver.FindElements(By.CssSelector("h1.title+ul>li"));
                return userListItems.Select(x => 
                    x.Text.Substring(0,x.Text.Length - UserLinksText.Length)).ToList();
            }
        }
        public IWebElement GetEditLink(string userName)
        {
            ReadOnlyCollection<IWebElement> userElements = 
                Driver.FindElements(By.CssSelector("h1.title+ul>li"));

            var userElement = userElements.Single(x => x.Text.StartsWith(userName));

            return userElement.FindElement(By.CssSelector("a.button"));
        }
        public IWebElement GetDeleteLink(string userName)
        {
            ReadOnlyCollection<IWebElement> deleteLinks =
                Driver.FindElements(By.CssSelector("h1.title+ul>li>a.is-danger"));

            return deleteLinks.Single(x => x.GetAttribute("onclick").EndsWith($"{userName}?')"));
        }
    }
}
