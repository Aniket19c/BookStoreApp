using BookStore.Controllers;
using BookStore.Models.DTO.User;
using Business.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repository.DTO;
using Repository.Helper.CustomExceptions;
using RepositoryLayer.DTO;
using System.Security.Claims;

namespace BookStoreApp.Tests
{
    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IUserBL> _mockUserBL;
        private UserController _userController;

        [SetUp]
        public void Setup()
        {
            _mockUserBL = new Mock<IUserBL>();
            _userController = new UserController(_mockUserBL.Object);
        }

        [Test]
        public async Task RegisterUser_ShouldReturnOk_WhenSuccessful()
        {
            var request = new UserRequestDto
            {
                FirstName = "Annie",
                LastName = "Doe",
                Email = "annie@example.com",
                Password = "Test123@"
            };

            var expected = new ResponseDto<UserResponseDto>
            {
                success = true,
                message = "Registered",
                data = new UserResponseDto
                {
                    UserId = 1,
                    FirstName = "Annie",
                    LastName = "Doe",
                    Email = "annie@example.com"
                }
            };

            _mockUserBL.Setup(x => x.RegisterUserAsync(request))
                       .ReturnsAsync(expected);

            var result = await _userController.RegisterUser(request);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var ok = result as OkObjectResult;
            Assert.IsTrue(((ResponseDto<UserResponseDto>)ok.Value).success);
        }

        [Test]
        public async Task DeleteUser_ShouldReturnOk_WhenUserDeleted()
        {
            var email = "annie@example.com";
            var expected = new ResponseDto<string>
            {
                success = true,
                message = "User deleted"
            };

            _mockUserBL.Setup(x => x.DeleteUserAsync(email))
                       .ReturnsAsync(expected);

            var result = await _userController.DeleteUser(email);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetAllUsers_ShouldReturnOk_WhenSuccessful()
        {
            var expected = new ResponseDto<List<UserResponseDto>>
            {
                success = true,
                message = "Users fetched",
                data = new List<UserResponseDto>()
            };

            _mockUserBL.Setup(x => x.GetAllUsersAsync())
                       .ReturnsAsync(expected);

            var result = await _userController.GetAllUsers();

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task UserLoginAsync_ShouldReturnOk_WhenValidLogin()
        {
            var login = new LoginDto
            {
                email = "annie@example.com",
                password = "Test@123"
            };

            var expected = new ResponseDto<LoginResponseDto>
            {
                success = true,
                message = "Login successful",
                data = new LoginResponseDto { Token = "dummyToken" }
            };

            _mockUserBL.Setup(x => x.UserLoginAsync(login))
                       .ReturnsAsync(expected);

            var result = await _userController.UserLoginAsync(login);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task UserLoginAsync_ShouldReturnUnauthorized_WhenInvalidCredentials()
        {
            var login = new LoginDto
            {
                email = "wrong@example.com",
                password = "wrong"
            };

            _mockUserBL.Setup(x => x.UserLoginAsync(login))
                       .ThrowsAsync(new InvalidCredentialsException());

            var result = await _userController.UserLoginAsync(login);

            Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
        }

        [Test]
        public async Task ForgetPassword_ShouldReturnOk_WhenSuccessful()
        {
            var email = "annie@example.com";

            var expected = new ResponseDto<string>
            {
                success = true,
                message = "OTP sent"
            };

            _mockUserBL.Setup(x => x.ForgetPasswordAsync(email))
                       .ReturnsAsync(expected);

            var result = await _userController.ForgetPassword(email);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task ResetPassword_ShouldReturnOk_WhenPasswordReset()
        {
            var dto = new ResetPasswordDto
            {
                OldPassword = "Test123@",
                NewPassword = "New@123",
                ConfirmPassword = "New@123"
            };

            var expected = new ResponseDto<string>
            {
                success = true,
                message = "Password reset"
            };

            _mockUserBL.Setup(x => x.ResetPasswordAsync(dto, dto.OldPassword))
                       .ReturnsAsync(expected);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, dto.OldPassword) }));

            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var result = await _userController.ResetPassword(dto);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task ResetPassword_ShouldReturnBadRequest_WhenPasswordsMismatch()
        {
            var dto = new ResetPasswordDto
            {
                OldPassword = "Test123@",
                NewPassword = "New@123",
                ConfirmPassword = "Mismatch"
            };

            _mockUserBL.Setup(x => x.ResetPasswordAsync(dto, dto.OldPassword))
                       .ThrowsAsync(new PasswordMismatchException());

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, dto.OldPassword) }));

            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var result = await _userController.ResetPassword(dto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
    }

}
