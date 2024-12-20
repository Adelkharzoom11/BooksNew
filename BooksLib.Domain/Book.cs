using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksLib.Domain
{
    public class Book
    {

        public Book()
        {
            Authers = new List<BookAuther>();
        }

        public required string Id { get; set; }
        public required string Title { get; set; }
        public string Description { get; set; }
        public required int totalPages { get; set; }
        public required string BookUrl { get; set; }
        public required string CoverImagePath { get; set; }
        public ICollection<BookAuther> Authers { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public User User { get; set; }

    }
}
