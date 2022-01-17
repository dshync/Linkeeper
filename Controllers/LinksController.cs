using Linkeeper.Data;
using Linkeeper.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Linkeeper.Controllers
{
	[Authorize]
	public class LinksController : Controller
	{
		private readonly ILinkeeperRepo _repository;
		private readonly UserManager<IdentityUser> _userManager;

        public LinksController(ILinkeeperRepo repository, UserManager<IdentityUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        [HttpGet("/add")]
		public IActionResult Add()
		{
			return View("Edit");
		}

		[HttpPost("/add")]
		public async Task<IActionResult> AddAsync(Link lnk)
		{
			IdentityUser user = await _userManager.GetUserAsync(User);
			lnk.User = user;

			_repository.AddLink(lnk);
			_repository.SaveChanges();

			return RedirectToAction("Index", "Home");
		}

		[HttpGet("/edit/{id}")]
		public IActionResult Edit(int id)
		{
			Link link = _repository.GetLinkById(id);

			if (link == null)
				return RedirectToAction("Index", "Home");

			return View(link);
		}

		[HttpPost("/edit")]
		public async Task<IActionResult> EditAsync(Link lnk)
		{
			IdentityUser user = await _userManager.GetUserAsync(User);
			lnk.User = user;

			_repository.UpdateLink(lnk);
			_repository.SaveChanges();

			return RedirectToAction("Index", "Home");
		}

		[HttpGet("/delete/{id}")]
		public IActionResult Delete(int id)
		{
			Link link = _repository.GetLinkById(id);

			if (link == null)
				return RedirectToAction("Index", "Home");

			_repository.DeleteLink(link);
			_repository.SaveChanges();

			return RedirectToAction("Index", "Home");
			//return RedirectToRoute("default", new { controller = "Home", action = "Index" });
		}

	}
}
