using Linkeeper.Models;
using System;
using System.Collections.Generic;

namespace Linkeeper.Data
{
    public class MockLinkeeperRepo : ILinkeeperRepo
    {
        public MockLinkeeperRepo()
        {
            links = new List<Link>() {
                new Link { Id = 0, Address = "https://github.com", Representation = "GitHub" },
                new Link { Id = 1, Address = "https://www.deepl.com", Representation = "Translator" },
                new Link { Id = 2, Address = "https://colab.research.google.com", Representation = "Colab" }
            };
        }

        public void AddLink(Link link)
        {
            links.Add(link);
        }

        public void DeleteLink(Link link)
        {
            links.Remove(link);
        }

        public IEnumerable<Link> GetAllLinks()
        {
            return links;
        }

        public Link GetLinkById(int id)
        {
            return links.Find(lnk => lnk.Id == id);
        }

        public bool SaveChanges()
        {
            return true;
        }

        public void UpdateLink(Link link)
        {
            int index = links.FindIndex(lnk => lnk.Id == link.Id);
            links[index].Address = link.Address;
            links[index].Representation = link.Representation;
        }

        private List<Link> links;
    }
}
