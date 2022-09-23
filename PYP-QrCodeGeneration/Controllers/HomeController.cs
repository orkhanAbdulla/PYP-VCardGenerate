using Microsoft.AspNetCore.Mvc;
using PYP_QrCodeGeneration.Models;
using PYP_QrCodeGeneration.Services;
using System.Diagnostics;

namespace PYP_QrCodeGeneration.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVCardService _vCardService;


        public HomeController(ILogger<HomeController> logger, IVCardService vCardService)
        {
            _logger = logger;
            _vCardService = vCardService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _vCardService.GetAll();
            return View(result);
        }
        public async Task<IActionResult> AddVCard()
        {
            await _vCardService.AddVCardAsync();
            return View();
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