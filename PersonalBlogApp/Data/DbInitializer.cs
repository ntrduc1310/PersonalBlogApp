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

            // Apply any pending migrations
            if (context.Database.GetPendingMigrations().Any())
            {
                await context.Database.MigrateAsync();
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
                if (!result.Succeeded)
                {
                    throw new Exception($"Could not seed admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
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
