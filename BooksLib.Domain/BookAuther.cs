using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksLib.Domain
{
    public class BookAuther
    {
        public required string Id { get; set; }


        public string BookId { get; set; }
        public Book Book { get; set; }

        public string AutherId { get; set; }
        public Auther Auther { get; set; }
    }
}
