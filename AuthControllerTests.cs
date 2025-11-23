using Xunit;
using FluentAssertions;
using Backend.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Backend.Tests;

    public class AuthControllerTests
    {
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _controller = new AuthController();
        }

        [Fact]
        public void Register_ShouldAddUser_WhenUsernameIsNew()
        {
            var user = new User { Username = "testuser", PasswordHash = "123456" };

            var result = _controller.Register(user);

            result.Should().BeOfType<OkObjectResult>();
            //AuthController.Users.Any(u => u.Username == "testuser").Should().BeTrue();
        }

        [Fact]
        public void Register_ShouldFail_WhenUsernameExists()
        {
            var user = new User { Username = "duplicate", PasswordHash = "123456" };
            _controller.Register(user); // first time

            var result = _controller.Register(user); // second time

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void Login_ShouldReturnToken_WhenCredentialsAreValid()
        {
              var registerUser = new User { Username = "loginuser", PasswordHash = "123456" };
                _controller.Register(registerUser);

                // ตอน login ต้องส่ง plain text password เดิม
                var loginAttempt = new User { Username = "loginuser", PasswordHash = "123456" };
                var result = _controller.Login(loginAttempt) as OkObjectResult;

                result.Should().NotBeNull();
                var token = result.Value?.GetType().GetProperty("token")?.GetValue(result.Value)?.ToString();
                token.Should().NotBeNullOrEmpty();
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
    }
