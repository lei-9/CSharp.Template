using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CSharp.CodeGenerator.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CSharp.CodeGenerator.Models;
using Microsoft.Extensions.Options;

namespace CSharp.CodeGenerator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var service = new GeneratorService();
            var result = service.GetAllTable();
            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(IEnumerable<string> tableIds)
        {
            var service = new GeneratorService();
            service.Create(tableIds.ToList());
            return Ok();
        }

        // public IActionResult TableList()
        // {
        //     var service = new GeneratorService();
        //     var result=service.GetAllTable();
        //     return View(result);
        // }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}