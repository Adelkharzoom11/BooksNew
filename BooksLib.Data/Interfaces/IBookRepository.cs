using BooksLib.Data.Dtos.BookDto.ForCreation;
using BooksLib.Data.Dtos.BookDto.ForResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksLib.Data.Interfaces
{
    public interface IBookRepository
    {
        Task<BookResponseDto<BookDto>> AddBookAsync(AddUpdateBook addUpdateBook);
        Task<BookResponseDto<BookDto>> UpdateBookAsync (string id, AddUpdateBook addUpdateBook);
        Task<BookResponseDto<IEnumerable<BookDto>>> GetAllBooksAsync();
        Task<BookResponseDto<BookDto>> GetBookByIdAsync(string id);
        Task<BookResponseDto<BookDto>> DeleteBookAsync (string id);
    }
}
