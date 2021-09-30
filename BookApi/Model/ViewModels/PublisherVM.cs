using BookApi.Model.Entities;
using BookApi.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Model.ViewModels
{
    public class PublisherVM 
    {
        [Required]
        public string Name { get; set; }

    }

    public class PublisherWithBookVM
    {
        public string Name { get; set; }
        public IEnumerable<string> BooksPublished { get; set; }

    }
}
