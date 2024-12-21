using BooksLib.Data.Dtos.BookDto.ForCreation;
using BooksLib.Data.Interfaces;
using BooksLib.Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BooksLibBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookService;

        public BookController(IBookRepository bookService)
        {
            _bookService = bookService;
        }



        [HttpPost("AddBook")]
        public async Task<IActionResult> AddBookAsync([FromForm] AddUpdateBook bookDto)
        {
            var result = await _bookService.AddBookAsync(bookDto);

            return result.IsSuccess ? Ok(result) : StatusCode(result.StatusCode, result);
        }

    }
}
