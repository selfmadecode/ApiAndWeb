using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Model.Entities
{
    public class Book_Author
    {

        // Many Authors for a book
        public int Id { get; set; }


        public int BookId { get; set; }
        public Book Book { get; set; }


        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
