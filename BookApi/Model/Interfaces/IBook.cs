using BookApi.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Model.Interfaces
{
    public interface IBook
    {
        BookWithAuthorsVM AddBookWithPublisherAndAuthors(BookVM book);
        IEnumerable<Book> GetBooks();
        BookWithAuthorsVM GetBookById(int id);
        Book UpdateBook(int id, BookVM book);
        void DeleteBookById(int id);

    }
}
