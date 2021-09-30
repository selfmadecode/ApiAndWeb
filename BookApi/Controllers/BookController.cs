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
    public class BookController : ControllerBase
    {
        private readonly IBook _book;

        public BookController(IBook book)
        {
            _book = book;
        }

        /// <summary>
        /// Add's a book
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddBook([FromBody]BookVM book)
        {
            try
            {
                var newBook = _book.AddBookWithPublisherAndAuthors(book);

                return CreatedAtRoute("GetBook", new { version=HttpContext.GetRequestedApiVersion().ToString(), bookId = newBook.Id }, newBook);
                //return CreatedAtAction("GetBookById", new { bookId = newBook.Id }, newBook);
                //return Created(nameof(AddBook), newBook);
            }
            catch (Exception ex)
            {
                //return StatusCode(500, "failed!");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get's a list of books
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
        /// get a book by id
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>

        [HttpGet("{bookId}", Name = "GetBook")]
        public IActionResult GetBookById(int bookId)
        {
            try
            {
                var book = _book.GetBookById(bookId);

                if (book == null)
                    return NotFound();

                return Ok(book);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update an existing
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

        /// <summary>
        /// Delete an existing book
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpDelete("{bookId}")]
        public IActionResult DeleteBookById(int bookId)
        {
            try
            {
                _book.DeleteBookById(bookId);

                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
