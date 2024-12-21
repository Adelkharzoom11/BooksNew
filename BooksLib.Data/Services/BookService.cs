using BooksLib.Data.Dtos.BookDto.ForCreation;
using BooksLib.Data.Dtos.BookDto.ForResponse;
using BooksLib.Data.Interfaces;
using BooksLib.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksLib.Data.Services
{
    public class BookService : IBookRepository
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookService(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<BookResponseDto<BookDto>> AddBookAsync(AddUpdateBook addUpdateBook)
        {
            EnsureDirectoriesExist();

            if (addUpdateBook.BookFile == null || Path.GetExtension(addUpdateBook.BookFile.FileName).ToLower() != ".pdf")
            {
                return new BookResponseDto<BookDto>
                {
                    StatusCode = 400,
                    Message = "The book must be in PDF format",
                    IsSuccess = false
                };
            }

            var validImageExtensions = new[] { ".png" , ".jpg" };

            if(addUpdateBook.CoverImage == null || !validImageExtensions.Contains(Path.GetExtension(addUpdateBook.CoverImage.FileName).ToLower()))
            {
                return new BookResponseDto<BookDto>
                {
                    StatusCode = 400,
                    Message = "The Image must be in png or jpg format",
                    IsSuccess = false
                };
            }
            // Save Book
            var bookPath = Path.Combine(_webHostEnvironment.WebRootPath, "BooksPdf");
            var bookFileName = Guid.NewGuid() + Path.GetExtension(addUpdateBook.BookFile.FileName);
            var bookFullPath = Path.Combine(bookPath, bookFileName);
            using(var stream = new FileStream(bookFullPath, FileMode.Create))
            {
                await addUpdateBook.BookFile.CopyToAsync(stream);
            }

            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
            var imageFileName = Guid.NewGuid() + Path.GetExtension(addUpdateBook.CoverImage.FileName);
            var imageFullPath = Path.Combine(imagePath, imageFileName);
            using(var stream = new FileStream(imageFullPath, FileMode.Create))
            {
                await addUpdateBook.CoverImage.CopyToAsync(stream);
            }

            var book = new Book
            {
                Id = Guid.NewGuid().ToString(),
                Title = addUpdateBook.Title,
                Description = addUpdateBook.Description,
                totalPages = addUpdateBook.totalPages,
                BookUrl = $"/BooksPdf/{bookFileName}",
                CoverImagePath = $"/Images/{imageFileName}",
            };

            var authers = await _context.Authers.Where(a => addUpdateBook.AuthersIds.Contains(a.Id)).ToListAsync();

            foreach (var auther in authers)
            {
                book.Authers.Add(new BookAuther
                {
                    Id = Guid.NewGuid().ToString(),
                    AutherId = auther.Id,
                    BookId = book.Id,
                });
            }
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            var bookResult = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                TotalPages = book.totalPages,
                BookURL = book.BookUrl,
                CoverImage = book.CoverImagePath,
            };

            return new BookResponseDto<BookDto>
            {
                IsSuccess = true,
                StatusCode = 201,
                Message = "Created Successfuly.",
                Data = bookResult
            };
        }

        public Task<BookResponseDto<BookDto>> DeleteBookAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<BookResponseDto<IEnumerable<BookDto>>> GetAllBooksAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BookResponseDto<BookDto>> GetBookByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<BookResponseDto<BookDto>> UpdateBookAsync(string id, AddUpdateBook addUpdateBook)
        {
            throw new NotImplementedException();
        }



        // Foldes (BookPdf Folder and CoverImages Folder) is Exist
        private void EnsureDirectoriesExist()
        {
            var wwwRootPath = _webHostEnvironment.WebRootPath;

            var BookPdfPath = Path.Combine(wwwRootPath, "BooksPdf");

            var ImagesPath = Path.Combine(wwwRootPath, "Images");

            if(!Directory.Exists(BookPdfPath))
            {
                Directory.CreateDirectory(BookPdfPath);
            }

            if(!Directory.Exists(ImagesPath))
            {
                Directory.CreateDirectory(ImagesPath);
            }
        }

    }
}
