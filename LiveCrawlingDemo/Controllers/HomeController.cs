using LiveCrawlingDemo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LiveCrawlingDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILotteryService _lotteryService;

        public HomeController(ILogger<HomeController> logger, ILotteryService lotteryService)
        {
            _logger = logger;
            _lotteryService = lotteryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetLatestLotteryResult()
        {
            var model = _lotteryService.GetLatestLotteryResult();
            return new ObjectResult(model);
        }
       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
