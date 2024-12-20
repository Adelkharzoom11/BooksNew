using BooksLib.Data.Dtos.UserDto.ForCreation;
using BooksLib.Data.Dtos.UserDto.ForResponse;
using BooksLib.Data.Interfaces;
using BooksLib.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksLib.Data.Services
{
    public class UserService : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly JWTService _jWTService;

        public UserService(UserManager<User> userManager , JWTService jWTService)
        {
            _userManager = userManager;
            _jWTService = jWTService;
        }


        public async Task<UsersResponseDto> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            UsersResponseDto result = new UsersResponseDto
            {
                IsSuccess = true,
                StatusCode = 200,
                UsersResponse = new List<UserResponseDto>()
            };

            if (!users.Any())
            {
                result.Message = "List of users is empty";
                result.StatusCode = 204; // Not Content
                return result;
            }

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                string firstRole = roles.FirstOrDefault() ?? Constant.UserRole;

                if (!firstRole.Contains(Constant.UserRole))
                {
                    await _userManager.AddToRoleAsync(user, Constant.UserRole);
                }

                var userInfo = GenerateUserInfoObject(user, firstRole);
                result.UsersResponse.Add(userInfo);
            }
            result.Message = "ok";
            return result;
        }

        public async Task<UsersResponseDto> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return new UsersResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "User Not Found",
                    UsersResponse = new List<UserResponseDto>()
                };
            }

            var roles = await _userManager.GetRolesAsync(user);
            string role = roles.FirstOrDefault() ?? Constant.UserRole;

            if (!role.Contains(Constant.UserRole))
            {
                await _userManager.AddToRoleAsync(user, Constant.UserRole);
            }

            var userInfo = GenerateUserInfoObject(user, role);

            return new UsersResponseDto
            {
                Message = "Ok",
                IsSuccess = true,
                StatusCode = 200,
                UsersResponse = new List<UserResponseDto> { userInfo }
            };
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
            {
                return new LoginResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "User Not Found",
                };
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result)
            {
                return new LoginResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    Message = "Invalid email or password"
                };
            }

            var roles = await _userManager.GetRolesAsync(user);
            string role = roles.FirstOrDefault() ?? Constant.UserRole;
            var userInfo = GenerateUserInfoObject(user, role);
            string Token = await _jWTService.CreateJWT(user);

            return new LoginResponseDto
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Login Successfuly.",
                NewToken = Token,
                UserResponseDto = userInfo
            };
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterUserDto registerUserDto)
        {
            var isExistsUser = await _userManager.Users.AnyAsync(u => u.Email == registerUserDto.Email);

            if (isExistsUser)
            {
                return new RegisterResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 409,
                    Message = "Email Already Exists"
                };
            }

            var newUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                Email = registerUserDto.Email,
                UserName = registerUserDto.Email
            };

            var isCreated = await _userManager.CreateAsync(newUser, registerUserDto.Password);

            if (!isCreated.Succeeded)
            {
                var errorMessages = string.Join(" | ", isCreated.Errors.Select(err => err.Description));
                return new RegisterResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = $"User creation failed: {errorMessages}"
                };
            }

            await _userManager.AddToRoleAsync(newUser, Constant.UserRole);

            return new RegisterResponseDto
            {
                IsSuccess = true,
                StatusCode = 201,
                Message = "User Created Successfuly."
            };
        }









        private UserResponseDto GenerateUserInfoObject(User user, string Role)
        {
            var checkAdmin = Role.Contains("admin");
            bool IsAdminOrUser;

            if (checkAdmin)
            {
                IsAdminOrUser = true;
            }
            else
            {
                IsAdminOrUser = false;
            }

            return new UserResponseDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsAdmin = IsAdminOrUser
            };
        }

    }
}
