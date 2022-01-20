using AutoMapper;
using Linkeeper.Domain.ApiIdentity;
using Linkeeper.DTOs.ApiIdentity;

namespace Linkeeper.Mapping
{
	public class ApiIdentityProfile : Profile
	{
		//used only for API
		public ApiIdentityProfile()
		{
			CreateMap<UserRegistrationRequestDTO, UserRegistrationRequest>();
			CreateMap<UserLoginRequestDTO, UserRegistrationRequest>();
			CreateMap<AuthSuccess, AuthenticationResultDTO>();
			CreateMap<AuthFailed, AuthenticationResultDTO>();
		}
	}
}
