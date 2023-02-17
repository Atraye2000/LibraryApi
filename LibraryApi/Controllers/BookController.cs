
using DocumentFormat.OpenXml.Presentation;
using LibraryApi.Context;
using LibraryApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class BookController : ControllerBase
    {
        private readonly AppDbContext _appContext;

        public BookController(AppDbContext appContext)
        {
            _appContext = appContext;
        }

        [HttpGet]
        public async Task<ActionResult<BookPayload>> GetBook([FromQuery] QueryStringParameters qsParameters)
        {
            IQueryable<Book> returnBooks = _appContext.Books.OrderBy(on => on.Id);
            List<Book> list = await returnBooks.ToListAsync();
            return new BookPayload(list);
        }

        [HttpGet("Id")]
        public async Task<ActionResult<Book>> GetBook(int Id)
        {
            var Book = await _appContext.Books.FindAsync(Id);
            Console.WriteLine(Book);
            if (Book == null)
                return NotFound();
            return Book;
        }

        

        [HttpGet("Title")]
        public async Task<IEnumerable<Book>> Search(string title)
        {
            Console.WriteLine(title);

            IEnumerable<Book> book = await _appContext.Books.Where(b => b.Title == title).ToListAsync();            
            return book;
        }


        /*[HttpGet("{GetBook}/{title}/{author?}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBook(string author , Title? title)
        {
            try
            {
                var result = await _appContext.GetBook(author, title);


                if(result.Any())
                {
                    return Ok(result);
                    
                }
                return NotFound();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database");
            }
        }*/



        [HttpPost("post")]
        public void Post([FromBody] Book book)
        {
            _appContext.Books.Add(book);
            _appContext.SaveChanges();
        }


        //Add Put Request for Update
        [HttpPut("{Id}")]
        public void Put(int Id, [FromBody] Book bookObj)
        {
            var book = _appContext.Books.Find(Id);
            if (bookObj != null)
            {
                book.Author = bookObj.Author;
                book.Title = bookObj.Title;
                book.Status = bookObj.Status;
                _appContext.SaveChanges();

            }

        }

        //Add Delete Request

        [HttpDelete("{Id}")]
        public void Delete(int Id)
        {
            var book = _appContext.Books.Find(Id);
            _appContext.Books.Remove(book);
            _appContext.SaveChanges();
        }
    }
}
