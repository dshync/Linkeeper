using Linkeeper.Data;
using Linkeeper.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Linkeeper.Controllers
{ 
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILinkeeperRepo _repository;

        public HomeController(ILogger<HomeController> logger, ILinkeeperRepo repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IActionResult Index()
        {
            List<Link> links = _repository.GetAllLinks().ToList();
            return View(links);
        }

        [Route("/edit/{id}")]
        public IActionResult Edit(int id)
        {
            //TODO
            return RedirectToAction(nameof(Index));
        }

        [Route("/delete/{id}")]
        public IActionResult Delete(int id)
        {
            Link link = _repository.GetLinkById(id);

            if (link == null)
                return RedirectToAction(nameof(Index));

            _repository.DeleteLink(link);
            _repository.SaveChanges();

            return RedirectToAction(nameof(Index));
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
