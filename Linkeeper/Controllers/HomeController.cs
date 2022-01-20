using Linkeeper.Data;
using Linkeeper.Models;
using Microsoft.AspNetCore.Identity;
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
		private readonly UserManager<IdentityUser> _userManager;

		public HomeController(UserManager<IdentityUser> userManager, ILinkeeperRepo repository, ILogger<HomeController> logger)
		{
			_logger = logger;
			_repository = repository;
			_userManager = userManager;
		}

		public async Task<IActionResult> IndexAsync()
		{
			IdentityUser user = await _userManager.GetUserAsync(User);
			List<Link> lnks = user != null ? _repository.GetAllUserLinks(user).ToList() : new List<Link>();

#if DEBUG
			List<Link> links = _repository.GetAllLinks().ToList();
			lnks.Add(new Link { Representation = "DIVIDER", Address = "DIVIDER", Id=22323 });
			lnks.AddRange(links);
#endif
			return View(lnks);
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
