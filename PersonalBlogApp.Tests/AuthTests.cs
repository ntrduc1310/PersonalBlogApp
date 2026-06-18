using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PersonalBlogApp.Areas.Identity.Pages.Account;
using PersonalBlogApp.Models;
using System.Security.Claims;
using Xunit;

namespace PersonalBlogApp.Tests
{
    public class AuthTests
    {
        private Mock<UserManager<ApplicationUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            var mgr = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());
            return mgr;
        }

        private Mock<SignInManager<ApplicationUser>> MockSignInManager(UserManager<ApplicationUser> userManager)
        {
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            var options = new Mock<IOptions<IdentityOptions>>();
            var logger = new Mock<ILogger<SignInManager<ApplicationUser>>>();
            var schemes = new Mock<IAuthenticationSchemeProvider>();
            var confirmation = new Mock<IUserConfirmation<ApplicationUser>>();
            return new Mock<SignInManager<ApplicationUser>>(userManager, contextAccessor.Object, claimsFactory.Object, options.Object, logger.Object, schemes.Object, confirmation.Object);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsLocalRedirect()
        {
            // Arrange
            var userManagerMock = MockUserManager();
            var signInManagerMock = MockSignInManager(userManagerMock.Object);
            var loggerMock = new Mock<ILogger<LoginModel>>();

            signInManagerMock.Setup(s => s.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(new List<AuthenticationScheme>());

            signInManagerMock.Setup(s => s.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            var loginModel = new LoginModel(signInManagerMock.Object, loggerMock.Object)
            {
                Input = new LoginModel.InputModel { Email = "test@example.com", Password = "ValidPassword123!" }
            };

            var urlHelperMock = new Mock<IUrlHelper>();
            urlHelperMock.Setup(u => u.Content(It.IsAny<string>())).Returns("/");
            loginModel.Url = urlHelperMock.Object;

            // Act
            var result = await loginModel.OnPostAsync();

            // Assert
            Assert.IsType<LocalRedirectResult>(result);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsPage()
        {
            // Arrange
            var userManagerMock = MockUserManager();
            var signInManagerMock = MockSignInManager(userManagerMock.Object);
            var loggerMock = new Mock<ILogger<LoginModel>>();

            signInManagerMock.Setup(s => s.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(new List<AuthenticationScheme>());

            signInManagerMock.Setup(s => s.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            var loginModel = new LoginModel(signInManagerMock.Object, loggerMock.Object)
            {
                Input = new LoginModel.InputModel { Email = "test@example.com", Password = "WrongPassword" }
            };

            var httpContext = new DefaultHttpContext();
            var modelState = new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary();
            var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            var pageContext = new PageContext(actionContext);
            loginModel.PageContext = pageContext;
            
            var urlHelperMock = new Mock<IUrlHelper>();
            urlHelperMock.Setup(u => u.Content(It.IsAny<string>())).Returns("/");
            loginModel.Url = urlHelperMock.Object;

            // Act
            var result = await loginModel.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.False(loginModel.ModelState.IsValid);
        }

        [Fact]
        public async Task Login_LockedOutAccount_ReturnsRedirectToLockout()
        {
            // Arrange
            var userManagerMock = MockUserManager();
            var signInManagerMock = MockSignInManager(userManagerMock.Object);
            var loggerMock = new Mock<ILogger<LoginModel>>();

            signInManagerMock.Setup(s => s.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(new List<AuthenticationScheme>());

            signInManagerMock.Setup(s => s.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.LockedOut);

            var loginModel = new LoginModel(signInManagerMock.Object, loggerMock.Object)
            {
                Input = new LoginModel.InputModel { Email = "test@example.com", Password = "WrongPassword" }
            };

            var urlHelperMock = new Mock<IUrlHelper>();
            urlHelperMock.Setup(u => u.Content(It.IsAny<string>())).Returns("/");
            loginModel.Url = urlHelperMock.Object;

            // Act
            var result = await loginModel.OnPostAsync();

            // Assert
            var redirectResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("./Lockout", redirectResult.PageName);
        }
    }
}
