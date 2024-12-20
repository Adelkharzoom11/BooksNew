using BooksLib.Data.Dtos.AutherDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksLib.Data.Dtos.BookDto.ForCreation
{
    public class BookDto
    {
        public BookDto()
        {
            Authers = new List<AuthorDto>();
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TotalPages { get; set; }

        public string BookURL { get; set; }
        public string CoverImage { get; set; }

        public List<AuthorDto> Authers { get; set; }
    }
}
