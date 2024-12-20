    using Microsoft.AspNetCore.Identity;


namespace BooksLib.Domain
{
    public class User : IdentityUser
    {
        public User()
        {
            CreatedAt = DateTime.UtcNow;
            Books = new List<Book>();
        }

        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
