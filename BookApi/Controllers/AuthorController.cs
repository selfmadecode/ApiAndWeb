using BookApi.Model.Interfaces;
using BookApi.Model.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthor _author;

        public AuthorController(IAuthor author)
        {
            _author = author;
        }

        [HttpPost]
        public IActionResult AddAuthor(AuthorVM author)
        {
            var newAuthor = _author.AddAuthor(author);
            return Created(nameof(AddAuthor), newAuthor);
        }
        [HttpGet("{bookId}")]
        public IActionResult GetAuthorWithBook(int bookId)
        {
            var authorAndBook = _author.GetAuthorWithBook(bookId);

            if(authorAndBook == null) return NotFound();

            return Ok(authorAndBook);
        }
    }
}
