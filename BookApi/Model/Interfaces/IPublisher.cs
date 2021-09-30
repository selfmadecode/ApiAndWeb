using BookApi.Model.Entities;
using BookApi.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Model.Interfaces
{
    public interface IPublisher
    {
        Publisher AddPublisher(PublisherVM publisher);
        Publisher GetPublisherById(int id);

        PublisherWithBookVM GetPublisherWithBook(int id);

        IEnumerable<Publisher> GetAllPublsihers(string orderBy, string searchParam);
    }
}
