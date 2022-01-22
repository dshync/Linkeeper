using AutoMapper;
using Linkeeper.Data;
using Linkeeper.DTOs;
using Linkeeper.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Linkeeper.Controllers
{
	[ApiController]
	[Route("api/link")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class LinksApiController : ControllerBase
	{
		private readonly ILinkeeperRepo _repository;
		private readonly IMapper _mapper;
		private readonly UserManager<IdentityUser> _userManager;

		public LinksApiController(ILinkeeperRepo repository, IMapper mapper, UserManager<IdentityUser> userManager)
		{
			_repository = repository;
			_mapper = mapper;
			_userManager = userManager;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Link>>> GetAllUserLinksAsync()
		{
			string userId = "";
			foreach (var claim in User.Claims) if (claim.Type == "Id") userId = claim.Value;
			IdentityUser user = await _userManager.FindByIdAsync(userId);

			var links = _repository.GetAllUserLinks(user);
			if (links == null) return Ok();
			return Ok(_mapper.Map<IEnumerable<LinkReadDTO>>(links));
		}

		[HttpGet("{id}", Name = "GetLinkByIdAsync")]
		public async Task<ActionResult<Link>> GetLinkByIdAsync(int id)
		{
			string userId = "";
			foreach (var claim in User.Claims) if (claim.Type == "Id") userId = claim.Value;
			IdentityUser user = await _userManager.FindByIdAsync(userId);

			Link link = _repository.GetLinkById(id);

			if (link != null)
			{
				if (link.User == null) return StatusCode(StatusCodes.Status403Forbidden);
				return Ok(_mapper.Map<LinkReadDTO>(link));
			}
			return NotFound();
		}

		[HttpPost]
		public async Task<ActionResult<LinkReadDTO>> AddLinkAsync(LinkAddDTO linkAddDTO)
		{
			string userId = "";
			foreach (var claim in User.Claims) if (claim.Type == "Id") userId = claim.Value;
			IdentityUser user = await _userManager.FindByIdAsync(userId);

			Link link = _mapper.Map<Link>(linkAddDTO);
			link.User = user;
			_repository.AddLink(link);
			bool added = _repository.SaveChanges();

			if (added)
			{
				LinkReadDTO linkReadDTO = _mapper.Map<LinkReadDTO>(link);
				return CreatedAtRoute(nameof(GetLinkByIdAsync), new { id = linkReadDTO.Id }, linkReadDTO);
			}

			return BadRequest();
		}

		[HttpPut("{id}")]
		public async Task<ActionResult> UpdateLinkAsync(int id, LinkUpdateDTO linkUpdateDTO)
		{
			string userId = "";
			foreach (var claim in User.Claims) if (claim.Type == "Id") userId = claim.Value;
			IdentityUser user = await _userManager.FindByIdAsync(userId);

			Link link = _repository.GetLinkById(id);

			if (link == null)
				return NotFound();

			_mapper.Map(linkUpdateDTO, link);

			if (link.User == null) return StatusCode(StatusCodes.Status403Forbidden);

			_repository.UpdateLink(link);
			bool added = _repository.SaveChanges();

			if (added)
				return NoContent();

			return BadRequest();
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteLinkAsync(int id)
		{
			string userId = "";
			foreach (var claim in User.Claims) if (claim.Type == "Id") userId = claim.Value;
			IdentityUser user = await _userManager.FindByIdAsync(userId);

			Link link = _repository.GetLinkById(id);

			if (link == null)
				return NotFound();

			if (link.User == null) return StatusCode(StatusCodes.Status403Forbidden);

			_repository.DeleteLink(link);
			_repository.SaveChanges();

			return NoContent();
		}
	}
}
