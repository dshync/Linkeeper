using AutoMapper;
using Linkeeper.Data;
using Linkeeper.DTOs;
using Linkeeper.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Linkeeper.Controllers
{
    [Route("api/links")]
    [ApiController]
    public class LinksApiController : ControllerBase
    {
        private readonly ILinkeeperRepo _repository;
        private readonly IMapper _mapper;

        public LinksApiController(ILinkeeperRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Link>> GetAllLinks()
        {
            var links = _repository.GetAllLinks();
            return Ok(_mapper.Map<IEnumerable<LinkReadDTO>>(links));
        }

        [HttpGet("{id}", Name = "GetLinkById")]
        public ActionResult<Link> GetLinkById(int id)
        {
            Link link = _repository.GetLinkById(id);
            if(link != null)
            {
                return Ok(_mapper.Map<LinkReadDTO>(link));
            }
            return NotFound();
        }

        [HttpPost]
        public ActionResult<LinkReadDTO> AddLink(LinkAddDTO linkAddDTO)
        {
            Link link = _mapper.Map<Link>(linkAddDTO);
            _repository.AddLink(link);
            bool added = _repository.SaveChanges();

            if (added)
            {
                LinkReadDTO linkReadDTO = _mapper.Map<LinkReadDTO>(link);
                return CreatedAtRoute(nameof(GetLinkById), new { id = linkReadDTO.Id }, linkReadDTO);
            }

            return BadRequest();
        }
    }
}
