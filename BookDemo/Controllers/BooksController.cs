using BookDemo.Data;
using BookDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = ApplicationContext.Books;
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetBook([FromRoute(Name = "id")] int id)
        {
            var book = ApplicationContext.Books.FirstOrDefault(a => a.Id.Equals(id));

            if (book == null)
                return NotFound();

            return Ok(book);
        }

        [HttpPost]
        public IActionResult CreateOneBook([FromBody] Book book)
        {
            try
            {
                if (book is null)
                    return BadRequest();

                ApplicationContext.Books.Add(book);
                return StatusCode(201, book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateBook([FromRoute(Name = "id")] int id,
            [FromBody] Book book)
        {
            var entity = ApplicationContext.Books.FirstOrDefault(a => a.Id.Equals(id));

            if (entity is null)
                return NotFound();

            if (id != book.Id)
                return BadRequest();

            ApplicationContext.Books.Remove(entity);
            book.Id = entity.Id;
            ApplicationContext.Books.Add(book);
            return Ok(book);

        }

        [HttpDelete]
        public IActionResult DeleteAllBooks()
        {
            ApplicationContext.Books.Clear();
            return NoContent();
        }

        [HttpDelete("id:int")]
        public IActionResult DeleteBook([FromRoute(Name = "id")] int id)
        {
            var entity = ApplicationContext.Books.FirstOrDefault(b => b.Id.Equals(id));
            
            if (entity is null) 
                return NotFound(new
                {
                    statusCode=404,
                    message=$"Book with id:{id} could not found."
                });

            ApplicationContext.Books.Remove(entity);
            return NoContent();

        }
    }
}
