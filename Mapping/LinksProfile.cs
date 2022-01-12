using AutoMapper;
using Linkeeper.DTOs;
using Linkeeper.Models;

namespace Linkeeper.Mapping
{
    public class LinksProfile : Profile
    {
        public LinksProfile()
        {
            CreateMap<Link, LinkReadDTO>();
            CreateMap<LinkAddDTO, Link>();
            CreateMap<LinkUpdateDTO, Link>();
        }
    }
}
