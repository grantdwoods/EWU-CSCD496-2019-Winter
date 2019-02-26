using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Web.ApiModels;

namespace SecretSanta.Web.Controllers
{
    public class GroupsController : Controller
    {
        private IHttpClientFactory ClientFactory { get; }
        public GroupsController(IHttpClientFactory clientFactory)
        {
            ClientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            IActionResult result = View();

            using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
            {
                var secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                ViewBag.Groups = await secretSantaClient.GetGroupsAsync();
            }

            return result;
        }

        [HttpGet]
        public async Task<IActionResult> Members(int groupId)
        {
            IActionResult result = View();

            using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
            {
                try
                {
                    var secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                    ViewBag.Group = await secretSantaClient.GetGroupAsync(groupId);

                }
                catch (SwaggerException se)
                {
                    ViewBag.ErrorMessage = se.Message;
                }
            }
            return result;
        }
    }
}