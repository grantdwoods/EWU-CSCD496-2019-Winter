using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SecretSanta.Web.UITests
{
    public class GroupsPage
    {
        public const string Slug = "Groups";
        public const string GroupLinksText = " Edit Delete";

        public IWebDriver Driver { get; }

        public IWebElement AddGroup => Driver.FindElement(By.LinkText("Add Group"));

        public AddGroupsPage AddGroupsPage => new AddGroupsPage(Driver);

        public List<string> GroupNames
        {
            get
            {
                var elements = Driver.FindElements(By.CssSelector("h1+ul>li"));

                return elements
                    .Select(x =>
                    {
                        var text = x.Text;
                        if (text.EndsWith(GroupLinksText))
                        {
                            text = text.Substring(0, text.Length - GroupLinksText.Length);
                        }
                        return text;
                    })
                    .ToList();
            }
        }

        public IWebElement GetDeleteLink(string groupName)
        {
            ReadOnlyCollection<IWebElement> deleteLinks =
                Driver.FindElements(By.CssSelector("a.is-danger"));

            return deleteLinks.Single(x => x.GetAttribute("onclick").EndsWith($"{groupName}?')"));
        } 

        public GroupsPage(IWebDriver driver)
        {
            Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }
    }
}
