using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksLib.Data.Dtos.BookDto.ForCreation
{
    public class AddUpdateBook
    {
        public AddUpdateBook()
        {
            AuthersIds = new List<string>();
        }

        [StringLength(200 , ErrorMessage = "The book title must be less than 200 characters.")]
        public required string Title { get; set; }
        public string Description { get; set; }

        [Range(1, int.MaxValue , ErrorMessage = "The number of pages must be greater than zero")]
        public required int totalPages { get; set; }

        public required IFormFile BookFile { get; set; }
        public required IFormFile CoverImage { get; set; }

        public required ICollection<string> AuthersIds { get; set; }

    }
}
