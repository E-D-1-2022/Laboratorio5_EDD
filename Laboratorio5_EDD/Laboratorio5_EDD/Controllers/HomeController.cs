using Laboratorio5_EDD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CustomStructures.Tree;

namespace Laboratorio5_EDD.Controllers
{
    public class Cliente {
    
}
    public class HomeController : Controller
    {
        public static int comp(Cliente cl1, Cliente cl2) {
            return 1;
        
        }
        Btree<Cliente> Btree = new Btree<Cliente>(2,comp);
        
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Cliente clien = new Cliente();
            Btree.add(clien);
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
