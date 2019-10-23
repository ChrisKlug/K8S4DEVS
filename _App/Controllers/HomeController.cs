using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using WebApplication.Models;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _environment;
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _config;
        private readonly IHealthService _healthService;

        public HomeController(IHostingEnvironment environment, IDistributedCache cache, IConfiguration config, IHealthService healthService)
        {
            _healthService = healthService;
            _config = config;
            _environment = environment;
            _cache = cache;
        }

        public IActionResult Index()
        {
            return View(new HomeViewModel
            {
                MachineName = Environment.MachineName,
            });
        }

        public IActionResult Redis()
        {
            return View(new RedisViewModel
            {
                SupportsRedis = !(_cache is MemoryDistributedCache)
            });
        }

        public IActionResult Configuration()
        {
            return View(new ConfigurationViewModel
            {
                ConfigValues = _config.AsEnumerable().ToDictionary(x => x.Key, x => x.Value)
            });
        }

        public IActionResult Health()
        {
            if (Request.Method == "POST")
            {
                _healthService.SetUnhealthy();
                return RedirectToAction("Health");
            }

            return View(new HealthViewModel
            {
                IsHealthy = _healthService.IsHealthy,
                Pings = _healthService.LastPings
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
