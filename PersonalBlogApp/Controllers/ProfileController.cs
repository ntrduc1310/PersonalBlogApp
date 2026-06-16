using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalBlogApp.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlogApp.Controllers
{
    /// <summary>
    /// Controller handling user profile viewing and custom avatar file uploads.
    /// Access is restricted to authenticated users.
    /// </summary>
    [Authorize]
    [Route("profile")]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        /// Initializes a new instance of the ProfileController.
        /// </summary>
        public ProfileController(UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Renders the profile page for the currently logged-in user.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Không tìm thấy người dùng.");
            }

            return View(user);
        }

        /// <summary>
        /// Handles custom avatar image uploads. Validates file size (max 10MB) and file extension,
        /// deletes any previous avatar, saves the new image to server storage, and updates user database record.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadAvatar(IFormFile? avatarFile)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Không tìm thấy người dùng.");
            }

            if (avatarFile == null || avatarFile.Length == 0)
            {
                ModelState.AddModelError("avatarFile", "Vui lòng chọn một file ảnh để tải lên.");
                return View("Index", user);
            }

            // 1. Validate File Size: Max 10MB (10 * 1024 * 1024 bytes)
            const long maxFileSize = 10 * 1024 * 1024;
            if (avatarFile.Length > maxFileSize)
            {
                ModelState.AddModelError("avatarFile", "Dung lượng file tối đa là 10MB.");
                return View("Index", user);
            }

            // 2. Validate File Extension
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(avatarFile.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("avatarFile", "Chỉ chấp nhận các file ảnh định dạng .jpg, .jpeg, .png, hoặc .gif.");
                return View("Index", user);
            }

            try
            {
                // 3. Setup directory
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "avatars");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // 4. Generate unique filename (UserId + unique GUID)
                var uniqueFileName = $"{user.Id}_{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // 5. Delete previous custom avatar if it exists
                if (!string.IsNullOrEmpty(user.AvatarUrl) && user.AvatarUrl.StartsWith("/avatars/"))
                {
                    var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, user.AvatarUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // 6. Resize image to 200x200 px and save the new file using SixLabors.ImageSharp
                using (var image = Image.Load(avatarFile.OpenReadStream()))
                {
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(200, 200),
                        Mode = ResizeMode.Crop
                    }));
                    await image.SaveAsync(filePath);
                }

                // 7. Update User AvatarUrl in DB
                user.AvatarUrl = $"/avatars/{uniqueFileName}";
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Cập nhật ảnh đại diện thành công!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("avatarFile", "Có lỗi xảy ra khi cập nhật thông tin tài khoản.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("avatarFile", $"Lỗi hệ thống khi lưu file: {ex.Message}");
            }

            return View("Index", user);
        }
    }
}
