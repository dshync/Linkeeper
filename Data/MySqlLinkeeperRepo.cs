using Linkeeper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Linkeeper.Data
{
    public class MySqlLinkeeperRepo : ILinkeeperRepo
    {
        private readonly LinkeeperContext _context;

        public MySqlLinkeeperRepo(LinkeeperContext context)
        {
            _context = context;
        }

        public void AddLink(Link link)
        {
            if (link == null) 
                throw new ArgumentNullException(nameof(link));

            _context.Add(link);
        }

        public IEnumerable<Link> GetAllLinks()
        {
            return _context.Links.ToList();
        }

        public Link GetLinkById(int id)
        {
            return _context.Links.FirstOrDefault(p => p.Id == id);
        }

        public void UpdateLink(Link link)
        {
            //nothing
        }

        public void DeleteLink(Link link)
        {
            if (link == null)
                throw new ArgumentNullException(nameof(link));

            _context.Remove(link);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
