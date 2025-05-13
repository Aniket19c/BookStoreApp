using BookStore.Controllers;
using Business.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.DTOs;
using Model.Entities;
using Moq;
using NUnit.Framework;
using RepositoryLayer.DTO;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookStore.Tests.Controllers
{
    [TestFixture]
    public class AddressControllerTests
    {
        private Mock<IAddressBL> _mockAddressBL;
        private Mock<ILogger<AddressController>> _mockLogger;
        private AddressController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockAddressBL = new Mock<IAddressBL>();
            _mockLogger = new Mock<ILogger<AddressController>>();
            _controller = new AddressController(_mockAddressBL.Object, _mockLogger.Object);

            // Mock authenticated user with UserId = 1
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("UserId", "1")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        [Test]
        public async Task AddAddress_ReturnsOk_WhenAddressAdded()
        {
            var dto = new AddressDto
            {
                AddressLine = "123 Street",
                City = "TestCity",
                State = "TestState",
                Type = Model.Enums.AddressTypes.HOME,
                Name = "Annie",
                MobileNumber = 1234567890
            };

            _mockAddressBL.Setup(x => x.AddAddress(It.IsAny<AddressEntity>())).ReturnsAsync(true);

            var result = await _controller.AddAddress(dto);

            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.That((result as OkObjectResult).Value, Is.EqualTo("Address added successfully"));
        }

        [Test]
        public async Task AddAddress_ReturnsBadRequest_WhenAddFails()
        {
            _mockAddressBL.Setup(x => x.AddAddress(It.IsAny<AddressEntity>())).ReturnsAsync(false);

            var dto = new AddressDto();
            var result = await _controller.AddAddress(dto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.That((result as BadRequestObjectResult).Value, Is.EqualTo("Failed to add address"));
        }

        [Test]
        public async Task DeleteAddress_ReturnsOk_WhenAddressDeleted()
        {
            _mockAddressBL.Setup(x => x.DeleteAddress(1)).ReturnsAsync(true);

            var result = await _controller.DeleteAddress(1);

            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.That((result as OkObjectResult).Value, Is.EqualTo("Address deleted successfully"));
        }

        [Test]
        public async Task DeleteAddress_ReturnsNotFound_WhenAddressNotFound()
        {
            _mockAddressBL.Setup(x => x.DeleteAddress(1)).ReturnsAsync(false);

            var result = await _controller.DeleteAddress(1);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            Assert.That((result as NotFoundObjectResult).Value, Is.EqualTo("Address not found"));
        }

        [Test]
        public async Task GetAllAddresses_ReturnsOk_WithList()
        {
            var list = new List<AddressEntity> { new AddressEntity { AddressId = 1, UserId = 1 } };
            _mockAddressBL.Setup(x => x.GetAllAddresses(1)).ReturnsAsync(list);

            var result = await _controller.GetAllAddresses();

            Assert.IsInstanceOf<OkObjectResult>(result);
            var response = result as OkObjectResult;
            Assert.IsInstanceOf<List<AddressEntity>>(response.Value);
        }

        [Test]
        public async Task UpdateAddress_ReturnsOk_WhenUpdateSuccessful()
        {
            var dto = new AddressRequestDto
            {
                AddressId = 1,
                AddressLine = "New Line",
                City = "New City",
                State = "New State",
                Type = Model.Enums.AddressTypes.WORK,
                Name = "Annie",
                MobileNumber = 9876543210
            };

            _mockAddressBL.Setup(x => x.UpdateAddress(It.IsAny<AddressEntity>())).ReturnsAsync(true);

            var result = await _controller.UpdateAddress(dto);

            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.That((result as OkObjectResult).Value, Is.EqualTo("Address updated successfully"));
        }

        [Test]
        public async Task UpdateAddress_ReturnsNotFound_WhenUpdateFails()
        {
            var dto = new AddressRequestDto { AddressId = 999 };
            _mockAddressBL.Setup(x => x.UpdateAddress(It.IsAny<AddressEntity>())).ReturnsAsync(false);

            var result = await _controller.UpdateAddress(dto);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            Assert.That((result as NotFoundObjectResult).Value, Is.EqualTo("Address not found"));
        }
    }
}
