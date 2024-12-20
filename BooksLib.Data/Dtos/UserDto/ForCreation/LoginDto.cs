using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksLib.Data.Dtos.UserDto.ForCreation
{
    public class LoginDto
    {

        [EmailAddress]
        public required string Email { set; get; }
        public required string Password { set; get; }
    }
}
