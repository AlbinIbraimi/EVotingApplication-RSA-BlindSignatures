using EVotingApplication.Data;
using EVotingApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EVotingApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        { 
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            //testing
            var credentials = _context.VotersCredentials.FirstOrDefault(m => m.VoterId.Equals(Startup.LogedInUserId));
            if(credentials != null)
            {
                ViewBag.username = credentials.username;
                ViewBag.password = credentials.password;
                ViewBag.voterId = Startup.LogedInUserId;
            }
            return View(_context.Candidates.ToList());
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
