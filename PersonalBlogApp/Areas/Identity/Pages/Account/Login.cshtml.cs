// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PersonalBlogApp.Models;

namespace PersonalBlogApp.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
                            [Required]
            [EmailAddress]
            public string Email { get; set; }

                            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

                            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        } 

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            //Thiết lập trang mặc định trả về sau khi đăng nhập thành công là trang chủ nếu url trống 
            returnUrl ??= Url.Content("~/");
            // Nạp lại ds đăng nhập ngoài để chuẩn bị render lại nếu đăng nhập thất bại 
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();


            //kiểm tra tính hợp lệ dữ liệu nhâp(như email đúng định dạng không, mật khẩu có trống không)
            if (ModelState.IsValid)
            {
                //Hàm cốt lõi- gọi Identity kiểm tra tài khoản, mật khẩu dưới dtb 
                //lockoutFailure: true nghĩa là kích hoạt khóa tài khoản tạm thời nếu gõ sai mật khẩu liên tiếp 5 lần 
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                

                //TH1: đăng nhập thành công 
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    //Hàm localRedirect sẽ gửi mã chuyển hướng HTTP 302 về Browser 
                    // Browser lập tức chuyển sang trang được lưu trong returnurl
                    // (VD: Trang chủ hoặc trang mà user đang cố vào trước đó)
                    return LocalRedirect(returnUrl);
                }

                //TH2: Yêu cầu xác thực 2 yêu tố( 2FA)
                if (result.RequiresTwoFactor)
                {
                    //Hệ thống chuyển hướng browser(redirect) sang trang nhập mã 2FA(Loginwith2fa.cshtml) kèm theo tham số ReturnUrl và RememberMe
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                //TH3: Tài khoản đã bị khóa tạm thời do nhập sai mật khẩu quá 5 lần
                if (result.IsLockedOut)
                {   
                    _logger.LogWarning("User account locked out.");
                    // chuyển hướng trình duyệt sang trang tbao tài khoản bị khóa tạm thời(lockout.cshtml)
                    return RedirectToPage("./Lockout");
                }
                else

                //TH4: sai mật khẩu hoặc tài khoản không tồn tại
                {
                    //nạp tbao lỗi "Invaild login attempt vào model Error 
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    //Trả về hàm Page(), hệ thống sẽ render lại chính trang đăng nhập login.cshtml 
                    // kèm theo thông báo lỗi màu đỏ trên dỏm để người dùng nhập lại 
                    return Page();
                }
            }
            //Nếu dữ liệu đầu vào sai định dạng(VD: trống email)    

            // trả về hàm Page() render lại chính trang đăng nhập hiện tại để hiển thị lỗi validaton 
            return Page();
        }
    }
}
