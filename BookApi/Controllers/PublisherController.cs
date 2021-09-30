using BookApi.Model.Entities;
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
    public class PublisherController : ControllerBase
    {
        private readonly IPublisher _publisher;

        public PublisherController(IPublisher publisher)
        {
            _publisher = publisher;
        }

        [HttpGet("get-publisher")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Publisher>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult GetPublisher(string orderBy, string searchParam)
        {
            try
            {
                var publisher = _publisher.GetAllPublsihers(orderBy, searchParam);

                if (publisher == null) return NotFound();

                return Ok(publisher);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Publisher))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPublisherById(int id)
        {
            var publisher = _publisher.GetPublisherById(1);
            if (publisher == null)
                return NotFound($"Publisher with id: {id} doesnt exist!");

            return Ok(publisher);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(IEnumerable<Publisher>))]
        [ProducesResponseType(400)]
        public IActionResult AddPublisher(PublisherVM publisher)
        {
            var newPublisher = _publisher.AddPublisher(publisher);
            return Created(nameof(AddPublisher), newPublisher);
        }

        [HttpGet("{publisherId}")]
        [ProducesResponseType(200, Type = typeof(PublisherWithBookVM))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPublisherWithBooks(int publisherId)
        {
            var response = _publisher.GetPublisherWithBook(publisherId);

            if (response == null) return NotFound();

            return Ok(response);
        }
    }
}
