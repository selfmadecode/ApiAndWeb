using BookApi.Model.Entities;
using BookApi.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Model.Interfaces
{
    public interface IAuthor
    {
        Author AddAuthor(AuthorVM author);

        AuthorWithBookVM GetAuthorWithBook(int id);
    }
}
