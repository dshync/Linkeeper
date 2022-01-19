using AutoMapper;
using Linkeeper.Domain.ApiIdentity;
using Linkeeper.DTOs.ApiIdentity;
using Linkeeper.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Linkeeper.Controllers.API
{
	[ApiController]
	[Route("api/identity")]
	public class IdentityApiController : ControllerBase
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IMapper _mapper;
		private readonly JwtSettings _jwt;

		public IdentityApiController(UserManager<IdentityUser> userManager, IMapper mapper, JwtSettings jwt)
		{
			_userManager = userManager;
			_mapper = mapper;
			_jwt = jwt;
		}

		[HttpPost("register")]
		public async Task<ActionResult<AuthenticationResultDTO>> RegisterAsync([FromBody]UserRegistrationRequestDTO registerRequestDTO)
		{

			var registerRequest = _mapper.Map<UserRegistrationRequest>(registerRequestDTO);
			IdentityUser potentialUser = await _userManager.FindByEmailAsync(registerRequest.Email);

			if(potentialUser != null)
			{
				var result = new AuthFailed { Success = false, Errors = new[] { "User with this email already exists" } };
				return BadRequest(_mapper.Map<AuthenticationResultDTO>(result));
			}

			potentialUser = new IdentityUser { Email = registerRequest.Email, UserName = registerRequest.Email };

			var createdUser = await _userManager.CreateAsync(potentialUser, registerRequest.Password);

			if (!createdUser.Succeeded)
			{
				var result = new AuthFailed { Success = false, Errors = createdUser.Errors.Select(x => x.Description).ToArray() };
				return BadRequest(_mapper.Map<AuthenticationResultDTO>(result));
			}

			var success = new AuthSuccess
			{
				Success = true,
				Token = GenerateToken(potentialUser)
			};

			return Ok(_mapper.Map<AuthenticationResultDTO>(success));
		}

		[HttpPost("login")]
		public async Task<ActionResult<AuthenticationResultDTO>> LoginAsync([FromBody] UserLoginRequestDTO loginRequestDTO)
		{
			var loginRequest = _mapper.Map<UserRegistrationRequest>(loginRequestDTO);
			IdentityUser user = await _userManager.FindByEmailAsync(loginRequest.Email);

			if (user == null)
			{
				var result = new AuthFailed { Success = false, Errors = new[] { "User does not exists" } };
				return BadRequest(_mapper.Map<AuthenticationResultDTO>(result));
			};

			bool passwordValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);

			if (!passwordValid)
			{
				var result = new AuthFailed { Success = false, Errors = new[] { "Password is wrong" } };
				return BadRequest(_mapper.Map<AuthenticationResultDTO>(result));
			};

			var success = new AuthSuccess
			{
				Success = true,
				Token = GenerateToken(user)
			};

			return Ok(_mapper.Map<AuthenticationResultDTO>(success));
		}

		protected string GenerateToken(IdentityUser user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_jwt.Secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim(JwtRegisteredClaimNames.Sub, user.Email),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
					new Claim(JwtRegisteredClaimNames.Email, user.Email),
					new Claim("Id", user.Id)
				}),
				Expires = DateTime.UtcNow.AddHours(4),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}
	}
}
