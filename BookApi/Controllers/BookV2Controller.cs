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
    [ApiVersion("2.0")]
    [ApiController]
    public class BookV2Controller : ControllerBase
    {
        private readonly IBook _book;

        public BookV2Controller(IBook book)
        {
            _book = book;
        }

        /// <summary>
        /// Get's a list of books with BookAPI Version 2
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                var book = _book.GetBooks();
                return Ok(book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }        

        /// <summary>
        /// Update an existing with BookAPI version 2
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="book"></param>
        /// <returns></returns>

        [HttpPut("{bookId}")]
        public IActionResult UpdateBookById(int bookId, [FromBody]BookVM book)
        {
            var updatedBook = _book.UpdateBook(bookId, book);
            return Created(nameof(UpdateBookById), updatedBook);
        }        
    }
}
