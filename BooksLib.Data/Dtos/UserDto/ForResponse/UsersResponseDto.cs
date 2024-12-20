using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksLib.Data.Dtos.UserDto.ForResponse
{
    public class UsersResponseDto
    {
        public UsersResponseDto()
        {
            UsersResponse = new List<UserResponseDto>();
        }
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<UserResponseDto> UsersResponse { get; set; }
    }
}
