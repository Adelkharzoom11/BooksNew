using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksLib.Domain
{
    public class Auther
    {
        public Auther()
        {
            BookAuthers = new List<BookAuther>();
        }

        public required string Id { get; set; }
        public string Name { get; set; }

        public ICollection<BookAuther> BookAuthers { get; set; }
    }
}
