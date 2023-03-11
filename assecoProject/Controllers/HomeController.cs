using assecoProject.Models;
using assecoProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace assecoProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPathInfoRepository pathInfoRepository;
        private readonly IConfiguration configuration;

        public HomeController(ILogger<HomeController> logger,IPathInfoRepository pathInfoRepository, IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            _logger = logger;
            this.pathInfoRepository = pathInfoRepository ?? throw new ArgumentNullException(nameof(pathInfoRepository));
            this.configuration = configuration;
        }

        public IActionResult Index()
        {
            var token = pathInfoRepository.GetAccessToken(configuration.GetValue<string>("AssecoConfig:ClientId"), configuration.GetValue<string>("AssecoConfig:ClientSecret"), configuration.GetValue<string>("AssecoConfig:AuthToken"));
            var xmlFile = pathInfoRepository.GetXmlFile(token.Result, configuration.GetValue<string>("AssecoConfig:wadlEndpoint"));
            var lista = pathInfoRepository.ParseXmlToList(xmlFile.Result);
         

            return View(lista);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}