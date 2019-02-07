using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SecretSanta.Api.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests
    {
        private CustomWebApplicationFactory<Startup> Factory { get; set; }
        public UserControllerTests()
        {
            Factory = new CustomWebApplicationFactory<Startup>();
        }

        [TestMethod]
        public async Task AddUser_MissingName_ReturnsBadRequest()
        {
            var client = Factory.CreateClient();
            var userInut = new UserInputViewModel { FirstName = "", LastName = "Woods" };

            var stringContent = new StringContent(JsonConvert.SerializeObject(userInut), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/User", stringContent);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task AddUser_ValidUser_ReturnsCreated()
        {
            var client = Factory.CreateClient();
            var userInut = new UserInputViewModel { FirstName = "Grant", LastName = "Woods" };

            var stringContent = new StringContent(JsonConvert.SerializeObject(userInut), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/User", stringContent);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
