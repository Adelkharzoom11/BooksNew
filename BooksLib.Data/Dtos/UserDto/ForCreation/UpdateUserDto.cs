using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksLib.Data.Dtos.UserDto.ForCreation
{
    public class UpdateUserDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
