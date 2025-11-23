using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Backend.Controllers;

namespace Backend.Tests
{
    public class AuthControllerTests
    {
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _controller = new AuthController();
        }

        [Fact]
        public void Register_ShouldSucceed_WhenUsernameIsNew()
        {
            var user = new User { Username = "newuser", PasswordHash = "123456" };

            var result = _controller.Register(user);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void Register_ShouldFail_WhenUsernameExists()
        {
            var user = new User { Username = "duplicate", PasswordHash = "123456" };
            _controller.Register(user);

            var result = _controller.Register(user);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void Login_ShouldReturnToken_WhenCredentialsAreValid()
        {
            var plainPassword = "123456";
            var user = new User { Username = "loginuser", PasswordHash = plainPassword };
            _controller.Register(user);

            var loginAttempt = new User { Username = "loginuser", PasswordHash = plainPassword };
            var result = _controller.Login(loginAttempt) as OkObjectResult;

            result.Should().NotBeNull("Login should succeed with correct credentials");
            var token = result.Value?.GetType().GetProperty("token")?.GetValue(result.Value)?.ToString();
            token.Should().NotBeNullOrEmpty("Token should be returned");
        }

        [Fact]
        public void Login_ShouldFail_WhenPasswordIsWrong()
        {
            var user = new User { Username = "wrongpass", PasswordHash = "123456" };
            _controller.Register(user);

            var loginAttempt = new User { Username = "wrongpass", PasswordHash = "wrong" };
            var result = _controller.Login(loginAttempt);

            result.Should().BeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public void Login_ShouldFail_WhenUserDoesNotExist()
        {
            var loginAttempt = new User { Username = "nouser", PasswordHash = "123456" };
            var result = _controller.Login(loginAttempt);

            result.Should().BeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public void Me_ShouldReturnUsername_WhenAuthorized()
        {
            var user = new User { Username = "meuser", PasswordHash = "123456" };
            _controller.Register(user);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.ControllerContext.HttpContext.User = new System.Security.Claims.ClaimsPrincipal(
                new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, "meuser")
                }, "mock")
            );

            var result = _controller.Me() as OkObjectResult;

            result.Should().NotBeNull();
            var username = result.Value?.GetType().GetProperty("username")?.GetValue(result.Value)?.ToString();
            username.Should().Be("meuser");
        }
    }
}
