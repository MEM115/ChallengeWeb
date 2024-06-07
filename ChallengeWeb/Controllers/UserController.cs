using ChallengeWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Numerics;
using System.Reflection;
using System.Text.Json.Serialization;

namespace ChallengeWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration config;
        RestClient client;

        public UserController(IConfiguration config)
        {
            this.config = config;
            client = new RestClient(config.GetConnectionString("DefaultConnection"));
        }
        [HttpGet]
        public IActionResult Balance()
        {
            String? token = HttpContext.Session.GetString("JwtToken");
            if (token == null) { return RedirectToAction("Login", "Auth"); }
            RestRequest request = new RestRequest("user/balance", Method.Get);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("CardNumber", HttpContext.Session.GetString("UserCard"));
            RestResponse response = client.Execute(request);
            if (response.IsSuccessStatusCode)
            {
                BalanceViewModel model = JsonConvert.DeserializeObject<BalanceViewModel>(response.Content);
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
        [HttpGet]
        public IActionResult Operations(int page = 0)
        {
            String? token = HttpContext.Session.GetString("JwtToken");
            if (token == null) { return RedirectToAction("Login", "Auth"); }
            
            RestRequest request = new RestRequest("user/operations", Method.Get);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("CardNumber", HttpContext.Session.GetString("UserCard"));
            request.AddParameter("Page", page);
            request.AddParameter("Results", 10);
            RestResponse response = client.Execute(request);
            if (response.IsSuccessStatusCode)
            {
                OperationViewModel model = JsonConvert.DeserializeObject<OperationViewModel>(response.Content);
                ViewBag.Page = page;
                ViewBag.HasPreviousPage = page > 0;
                ViewBag.HasNextPage = model.Total > (page + 1) * 10;
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
        [HttpGet]
        public IActionResult Withdraw()
        {
            String? token = HttpContext.Session.GetString("JwtToken");
            if (token == null) { return RedirectToAction("Login", "Auth"); }
            return View();
        }
        [HttpPost]
        public IActionResult Withdraw(WithdrawViewModel model)
        {
            String? token = HttpContext.Session.GetString("JwtToken");
            if (token == null) { return RedirectToAction("Login", "Auth"); }
            RestRequest request = new RestRequest("user/withdraw", Method.Post);
            request.AddHeader("Authorization", "Bearer " + token);
            model.cardNumber = HttpContext.Session.GetString("UserCard");
            request.AddBody(model, ContentType.Json);
            RestResponse response = client.Execute(request);
            if (response.IsSuccessStatusCode)
            {
                WithdrawResponseViewModel modelresponse = JsonConvert.DeserializeObject<WithdrawResponseViewModel>(response.Content);
                return View("WithdrawConfirmation",modelresponse);
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    if (response.Content.Replace("\"","") == "-1")
                    {
                        ModelState.AddModelError(string.Empty, "Ha ingresado un valor mayor al saldo.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ha ingresado un valor erroneo.");
                    }
                }
                else if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ha sucedido un error.");
                }
                return View(model);
            }
        }
    }
}
