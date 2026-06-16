using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalBlogApp.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlogApp.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Apply any pending migrations
            if (context.Database.GetPendingMigrations().Any())
            {
                await context.Database.MigrateAsync();
            }

            // Seed Roles
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Seed Admin User
            string adminEmail = "admin@example.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    AvatarUrl = "https://ui-avatars.com/api/?name=Admin&background=random"
                };

                // P@ssw0rd123 satisfies default Identity requirements
                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    throw new Exception($"Could not seed admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Seed Regular User
            string userEmail = "user@example.com";
            var regularUser = await userManager.FindByEmailAsync(userEmail);

            if (regularUser == null)
            {
                regularUser = new ApplicationUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    EmailConfirmed = true,
                    AvatarUrl = "https://ui-avatars.com/api/?name=User&background=random"
                };

                var result = await userManager.CreateAsync(regularUser, "User@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(regularUser, "User");
                }
                else
                {
                    throw new Exception($"Could not seed user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                if (!await userManager.IsInRoleAsync(regularUser, "User"))
                {
                    await userManager.AddToRoleAsync(regularUser, "User");
                }
            }

            // Seed Sample Blogs
            if (!context.Blogs.Any())
            {
                context.Blogs.AddRange(
                    new Blog
                    {
                        Title = "Khởi đầu dự án Personal Blog",
                        Content = "<p>Chào mừng bạn đến với bài viết đầu tiên. Đây là nội dung mẫu được seed tự động vào database.</p>",
                        Priority = 5,
                        CreatedAt = DateTime.UtcNow,
                        UserId = adminUser.Id
                    },
                    new Blog
                    {
                        Title = "Tìm hiểu về .NET 8 và Razor Pages",
                        Content = "<p>Razor Pages giúp việc xây dựng các trang web trở nên đơn giản và hiệu quả hơn trong .NET 8.</p>",
                        Priority = 3,
                        CreatedAt = DateTime.UtcNow.AddMinutes(-10),
                        UserId = adminUser.Id
                    },
                    new Blog
                    {
                        Title = "Hướng dẫn sử dụng Entity Framework Core",
                        Content = "<p>EF Core là một O/RM mạnh mẽ cho phép làm việc với database thông qua các đối tượng .NET.</p>",
                        Priority = 4,
                        CreatedAt = DateTime.UtcNow.AddMinutes(-20),
                        UserId = adminUser.Id
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
