using BookApi.Model.Entities;
using BookApi.Model.Interfaces;
using BookApi.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Model.Services
{
    public class AuthorServices : IAuthor
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthorServices(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Author AddAuthor(AuthorVM author)
        {
            var newAuthor = new Author
            {
                Name = author.Name
            };

            _dbContext.Authors.Add(newAuthor);
            _dbContext.SaveChanges();
            return newAuthor;
        }

        public AuthorWithBookVM GetAuthorWithBook(int id)
        {
            // use automapper
            return _dbContext.Authors.Where(n => n.Id == id)
                .Select(book => new AuthorWithBookVM
                {
                    Name = book.Name,
                    AllBook = book.Book_Authors.Select(b => b.Book.Title).ToList()
                }).FirstOrDefault();            
        }
    }
}
