using BooksLib.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BooksLib.Data.Services
{
    public class ContextSeedService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ContextSeedService(AppDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitializeContextAsync()
        {
            // Apply pending migrations
            if ((await _context.Database.GetPendingMigrationsAsync().ConfigureAwait(false)).Any())
            {
                await _context.Database.MigrateAsync().ConfigureAwait(false);
            }

            // Seed roles
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = Constant.AdminRole }).ConfigureAwait(false);
                await _roleManager.CreateAsync(new IdentityRole { Name = Constant.UserRole }).ConfigureAwait(false);
            }

            // Seed admin user
            if (!await _userManager.Users.AnyAsync().ConfigureAwait(false))
            {
                var admin = new User
                {
                    FirstName = Constant.AdminRole,
                    LastName = Constant.AdminRole,
                    Email = Constant.AdminEmail,
                    UserName = Constant.AdminEmail,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(admin, Constant.AdminPassWord).ConfigureAwait(false);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, Constant.AdminRole).ConfigureAwait(false);
                    await _userManager.AddClaimsAsync(admin, new[]
                    {
                        new Claim(ClaimTypes.Email, admin.Email),
                        new Claim(ClaimTypes.Surname, admin.LastName)
                    }).ConfigureAwait(false);

                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                else
                {
                    Console.WriteLine("Failed to create admin user. Errors:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"- {error.Description}");
                    }
                }
            }
        }
    }
}
