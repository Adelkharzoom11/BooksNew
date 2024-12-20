using BooksLib.Data.Dtos.UserDto.ForCreation;
using BooksLib.Data.Dtos.UserDto.ForResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksLib.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<RegisterResponseDto> RegisterAsync(RegisterUserDto registerUserDto);
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
        Task<UsersResponseDto> GetAllUsersAsync();
        Task<UsersResponseDto> GetUserByEmailAsync(string email);
    }
}
