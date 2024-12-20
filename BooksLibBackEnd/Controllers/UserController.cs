using BooksLib.Data;
using BooksLib.Data.Dtos.UserDto.ForCreation;
using BooksLib.Data.Dtos.UserDto.ForResponse;
using BooksLib.Data.Interfaces;
using BooksLib.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using Constant = BooksLib.Data.Constant;

namespace BooksLibBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("allusers")]
        [Authorize(Roles = Constant.AdminRole)]
        public async Task<ActionResult<UsersResponseDto>> GetAllUsersAcync()
        {
            var result = await _userRepository.GetAllUsersAsync();

            return result.IsSuccess ? Ok(result) : StatusCode(result.StatusCode , result);
        }

        [HttpGet("get-user-by-email")]
        [Authorize(Roles = Constant.AdminRole)]
        public async Task<ActionResult<UsersResponseDto>> GetUserByEmail(string email)
        {
            var result = await _userRepository.GetUserByEmailAsync(email);

            return result.IsSuccess ? Ok(result) : StatusCode(result.StatusCode, result);
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponseDto>> Register([FromBody] RegisterUserDto registerUserDto)
        {
            var result = await _userRepository.RegisterAsync(registerUserDto);

            return result.IsSuccess ? Ok(result) : StatusCode(result.StatusCode , result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            var result = await _userRepository.LoginAsync(loginDto);

            return result.IsSuccess ? Ok(result) : StatusCode (result.StatusCode , result);
        }
    }
}
