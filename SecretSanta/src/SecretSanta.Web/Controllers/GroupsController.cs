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


        [HttpGet]
        public async Task<IActionResult> AddMembers(int groupId, string groupName)
        {
            ViewBag.GroupId = groupId;
            ViewBag.GroupName = groupName;

            using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
            {
                var secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                ViewBag.Users = await secretSantaClient.GetAllUsersAsync();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddMembers(int groupId, int userId)
        {
            IActionResult result = View();

            using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
            {
                try
                {
                    var secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                    await secretSantaClient.AddUserToGroupAsync(groupId, userId);
                    result = RedirectToAction(nameof(AddMembers));
                    ViewBag.SucessMessage = "User added to group!";
                }
                catch (SwaggerException se)
                {
                    ViewBag.ErrorMessage = se.Message;
                }
            }
            return result;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(GroupInputViewModel group)
        {
            IActionResult result = View();
            if (string.IsNullOrEmpty(group.Name))
            {
                ModelState.AddModelError("Name", "Group name is required!");
            }
            using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
            {
                try
                {
                    var secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                    await secretSantaClient.CreateGroupAsync(group);
                    result = RedirectToAction(nameof(Index));
                }
                catch (SwaggerException se)
                {
                    ViewBag.ErrorMessage = se.Message;
                }
            }
            return result;
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int groupId)
        {
            IActionResult result = View();

            using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
            {
                try
                {
                    var secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                    await secretSantaClient.DeleteGroupAsync(groupId);
                    result = RedirectToAction(nameof(Index));
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