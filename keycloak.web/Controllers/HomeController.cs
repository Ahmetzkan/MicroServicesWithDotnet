using keycloak.web.Models;
using keycloak.web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Diagnostics;

namespace keycloak.web.Controllers
{
    public class HomeController(ILogger<HomeController> logger, WeatherService weatherService) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secured()
        {
            var claims = User.Claims;


            var token = HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken).Result;


            Response.Cookies.Append("key", "value");


            var refreshToken = HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken).Result;
            return View();
        }


        public IActionResult WeatherForecastPage()
        {
            var response = weatherService.GetWeatherForecast();
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}