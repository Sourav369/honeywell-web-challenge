using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using honeywell_web_challenge.Models;
using honeywell_web_challenge.Services;

namespace honeywell_web_challenge.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMediaService _mediaService;

        public HomeController(ILogger<HomeController> logger, IMediaService mediaService)
        {
            _logger = logger;
            _mediaService = mediaService;
        }

        // All views from a single URL: / or /Home/Index
        public IActionResult Index()
        {
            var files = _mediaService.GetAllMediaFiles();

            var vm = new HomeViewModel
            {
                MediaFiles = files.ToList(),
                ActiveView = "catalogue" // default to catalogue list view
            };

            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
