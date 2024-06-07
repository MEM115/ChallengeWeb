using ChallengeWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ChallengeWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration config;
        RestClient client;

        public AuthController(IConfiguration config)
        {
            this.config = config;
            client = new RestClient(config.GetConnectionString("DefaultConnection"));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            RestRequest request = new RestRequest("auth/login", Method.Post);
            request.AddBody(model, ContentType.Json);
            RestResponse response = client.Execute(request);

            if (response.IsSuccessStatusCode)
            {
                JObject obj = JObject.Parse(response.Content);

                HttpContext.Session.SetString("JwtToken", obj.Value<string>("token"));
                HttpContext.Session.SetString("UserCard", model.CardNumber);

                return RedirectToAction("Balance", "User");
            }
            else
            {
                if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ModelState.AddModelError(string.Empty, "Credenciales inválidas. Por favor, verifica tu número de tarjeta y PIN.");
                }
                else if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ModelState.AddModelError(string.Empty, "Su tarjeta ha sido bloqueada.");
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
