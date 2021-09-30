using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Model.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //list of books and authors
        public List<Book_Author> Book_Authors { get; set; }
    }
}
