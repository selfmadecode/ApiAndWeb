using AutoMapper;
using BookApi.Model.Entities;
using BookApi.Model.Interfaces;
using BookApi.Model.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Model.Services
{
    public class BookServices : IBook
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BookServices(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public BookWithAuthorsVM AddBookWithPublisherAndAuthors(BookVM book)
        {
            // use dbContext transactions
            var newBook = _mapper.Map<Book>(book);


            using (IDbContextTransaction dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Add(newBook);
                    _context.SaveChanges();

                    foreach (var Id in book.AuthorIds)
                    {
                        var book_authors = new Book_Author
                        {
                            BookId = newBook.Id,
                            AuthorId = Id,

                        };
                        _context.Books_Authors.Add(book_authors);
                    }
                    _context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }


            var bookWithAuthors = _context.Books.Where(b => b.Id == newBook.Id)
               .Select(book => new BookWithAuthorsVM
               {
                   Id = book.Id,
                   Genre = book.Genre,
                   CoverUrl = book.CoverUrl,
                   IsRead = book.IsRead,
                   DateRead = book.DateRead,
                   Description = book.Description,
                   Rate = book.Rate,
                   Title = book.Title,
                   PublisherName = book.Publisher.Name,
                   AuthorsName = book.Book_Authors.Select(n => n.Author.Name).ToList()
               }).FirstOrDefault();

            return bookWithAuthors;
        }

        public void DeleteBookById(int id)
        {
            var bookToBeDeleted = _context.Books.FirstOrDefault(b => b.Id == id);

            if (bookToBeDeleted != null)
            {
                _context.Books.Remove(bookToBeDeleted);
                _context.SaveChanges();
            }
        }

        public BookWithAuthorsVM GetBookById(int id)
        {

            //var bookWithAuthor = _context.Books.Where(b => b.Id == id)
            //    .Include(n => n.Book_Authors)
            //    .Include(p => p.Publisher)
            //    .FirstOrDefault();

            //var bookAndAuthor = _mapper.Map<BookWithAuthorsVM>(bookWithAuthor);

            //return bookAndAuthor;

            var bookWithAuthors = _context.Books.Where(b => b.Id == id)
                .Select(book => new BookWithAuthorsVM
                {
                    Genre = book.Genre,
                    CoverUrl = book.CoverUrl,
                    IsRead = book.IsRead,
                    DateRead = book.DateRead,
                    Description = book.Description,
                    Rate = book.Rate,
                    Title = book.Title,
                    PublisherName = book.Publisher.Name,
                    AuthorsName = book.Book_Authors.Select(n => n.Author.Name).ToList()
                }).FirstOrDefault();

            if (bookWithAuthors == null) return null;

            return bookWithAuthors;
        }

        public IEnumerable<Book> GetBooks()
            => _context.Books.ToList();

        public Book UpdateBook(int id, BookVM book)
        {
            var oldBook = _context.Books.FirstOrDefault(b => b.Id == id);

            if(oldBook != null)
            {
                _mapper.Map<BookVM, Book>(book, oldBook);
                _context.SaveChanges();
            }

            return oldBook;
        }
    }
}
