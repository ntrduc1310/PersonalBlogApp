using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalBlogApp.Data;
using PersonalBlogApp.Models;
using PersonalBlogApp.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlogApp.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;


        public UsersController(
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        // GET: /users
        [HttpGet("")]
        public async Task<IActionResult> Index(string search)
        {
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim();
                query = query.Where(u => (u.Email != null && u.Email.Contains(search)) || (u.UserName != null && u.UserName.Contains(search)));
            }

            var users = await query.ToListAsync();
            var userList = new List<UserItemVM>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new UserItemVM
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    Role = roles.FirstOrDefault() ?? "User",
                    IsActive = user.IsActive
                });
            }

            ViewData["CurrentFilter"] = search;
            return View(userList);
        }

        // GET: /users/{id}/edit
        [HttpGet("{id}/edit")]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var model = new UserEditVM
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                Role = roles.FirstOrDefault() ?? "User",
                IsActive = user.IsActive
            };

            return View(model);
        }

        // POST: /users/{id}/edit
        [HttpPost("{id}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserEditVM model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Expert protection: Admin cannot deactivate or demote themselves
            var currentUserId = _userManager.GetUserId(User);
            if (user.Id == currentUserId)
            {
                if (!model.IsActive)
                {
                    ModelState.AddModelError(string.Empty, "Bạn không thể tự vô hiệu hóa tài khoản Admin đang sử dụng.");
                    return View(model);
                }
                if (model.Role != "Admin")
                {
                    ModelState.AddModelError(string.Empty, "Bạn không thể tự hạ cấp vai trò Admin của chính mình.");
                    return View(model);
                }
            }

            // Update user properties
            user.IsActive = model.IsActive;
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            // Update user role
            var currentRoles = await _userManager.GetRolesAsync(user);
            var selectedRole = model.Role;

            if (!currentRoles.Contains(selectedRole))
            {
                // Remove old roles
                if (currentRoles.Any())
                {
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                }

                // Add new role
                if (await _roleManager.RoleExistsAsync(selectedRole))
                {
                    await _userManager.AddToRoleAsync(user, selectedRole);
                }
            }

            TempData["SuccessMessage"] = $"Cập nhật người dùng {user.Email} thành công!";
            return RedirectToAction(nameof(Index));
        }

        // GET: /users/{id}/delete
        [HttpGet("{id}/delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Prevent self-deletion
            var currentUserId = _userManager.GetUserId(User);
            if (user.Id == currentUserId)
            {
                TempData["ErrorMessage"] = "Bạn không thể tự xóa tài khoản Admin của chính mình.";
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        // POST: /users/{id}/delete
        [HttpPost("{id}/delete")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var currentUserId = _userManager.GetUserId(User);
            if (user.Id == currentUserId)
            {
                TempData["ErrorMessage"] = "Bạn không thể tự xóa tài khoản Admin của chính mình.";
                return RedirectToAction(nameof(Index));
            }

            // Expert implementation: Pre-emptively delete comments to avoid Restrict delete behavior FK error
            var userComments = _context.Comments.Where(c => c.UserId == user.Id);
            _context.Comments.RemoveRange(userComments);

            // Fetch user blogs to make sure their blogs and comments are deleted
            var userBlogs = _context.Blogs.Where(b => b.UserId == user.Id);
            _context.Blogs.RemoveRange(userBlogs);

            await _context.SaveChangesAsync();

            // Perform core user deletion
            var deleteResult = await _userManager.DeleteAsync(user);
            if (deleteResult.Succeeded)
            {
                TempData["SuccessMessage"] = $"Xóa tài khoản {user.Email} thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = $"Lỗi khi xóa người dùng: {string.Join(", ", deleteResult.Errors.Select(e => e.Description))}";
            }

            return RedirectToAction(nameof(Index));
        }
     }
}
