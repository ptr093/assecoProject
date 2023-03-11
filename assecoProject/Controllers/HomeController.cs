using assecoProject.Models;
using assecoProject.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace assecoProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPathInfoRepository pathInfoRepository;

        public HomeController(ILogger<HomeController> logger,IPathInfoRepository pathInfoRepository)
        {
            _logger = logger;
            this.pathInfoRepository = pathInfoRepository ?? throw new ArgumentNullException(nameof(pathInfoRepository));
        }

        public IActionResult Index()
        {
            var token = pathInfoRepository.GetAccessToken();
            var xmlFile = pathInfoRepository.GetXmlFile(token.Result);
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