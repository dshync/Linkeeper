using AutoMapper;
using Linkeeper.Domain.ApiIdentity;
using Linkeeper.DTOs.ApiIdentity;
using Linkeeper.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    [AllowAnonymous]
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
		public async Task<ActionResult<AuthenticationResultDTO>> RegisterAsync([FromBody]UserRegistrationRequestDTO requestDTO)
        {
            var request = _mapper.Map<UserRegistrationRequest>(requestDTO);
            IdentityUser potentialUser = await _userManager.FindByEmailAsync(request.Email);

            if(potentialUser != null)
            {
                var result = new AuthFailed { Success = false, Errors = new[] { "User with this email already exists" } };
                return BadRequest(_mapper.Map<AuthenticationResultDTO>(result));
            }

            potentialUser = new IdentityUser { Email = request.Email, UserName = request.Email };

            var createdUser = await _userManager.CreateAsync(potentialUser, request.Password);

            if (!createdUser.Succeeded)
            {
                var result = new AuthFailed { Success = false, Errors = createdUser.Errors.Select(x => x.Description).ToArray() };
                return BadRequest(_mapper.Map<AuthenticationResultDTO>(result));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwt.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity( new []
                {
                    new Claim(JwtRegisteredClaimNames.Sub, potentialUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, potentialUser.Email),
                    new Claim("id", potentialUser.Id)
                }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var success = new AuthSuccess 
            { 
                Success = true, 
                Token = tokenHandler.WriteToken(token) 
            };

			return Ok(_mapper.Map<AuthenticationResultDTO>(success));
        }
    }
}
