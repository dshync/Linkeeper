using Linkeeper.Models;
using System;
using System.Collections.Generic;

namespace Linkeeper.Data
{
    public class MockLinkeeperRepo : ILinkeeperRepo
    {
        public void AddLink(Link link)
        {
            throw new NotImplementedException();
        }

        public void DeleteLink(Link link)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Link> GetAllLinks()
        {
            return new List<Link>
            {
                new Link { Id = 0, Address = "https://github.com", Representation = "GitHub" },
                new Link { Id = 1, Address = "https://www.deepl.com", Representation = "Translator" },
                new Link { Id = 2, Address = "https://colab.research.google.com", Representation = "Colab" }
            };
        }

        public Link GetLinkById(int id)
        {
            return new Link { Id = 0, Address = "https://github.com", Representation = "GitHub" };
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void UpdateLink(Link link)
        {
            throw new NotImplementedException();
        }
    }
}
