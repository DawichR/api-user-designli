using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UmaDesignli.Api.Controllers.Access;
using UmaDesignli.Application.Commands.Access;
using UmaDesignli.Domain.Entities;
using Xunit;

namespace UmaDesignli.UnitTest.Controllers
{
    /// <summary>
    /// Unit tests for AccessController
    /// Tests the authentication endpoints including login functionality
    /// </summary>
    public class AccessControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AccessController _controller;

        public AccessControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new AccessController(_mediatorMock.Object);
        }

        /// <summary>
        /// Test: Login with valid credentials should return 200 OK with token
        /// </summary>
        [Fact]
        public async Task Login_WithValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var userapp = new Userapp { Username = "testuser", Password = "password123" };
            var expectedResult = new LoginResult("mock-jwt-token", "testuser");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Login(userapp);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<LoginResult>(okResult.Value);
            Assert.Equal("mock-jwt-token", returnValue.Token);
            Assert.Equal("testuser", returnValue.Username);
        }

        /// <summary>
        /// Test: Login endpoint should call mediator with correct command
        /// </summary>
        [Fact]
        public async Task Login_ShouldCallMediator_WithCorrectCommand()
        {
            // Arrange
            var userapp = new Userapp { Username = "testuser", Password = "password123" };
            var expectedResult = new LoginResult("token", "testuser");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            await _controller.Login(userapp);

            // Assert
            _mediatorMock.Verify(
                m => m.Send(
                    It.Is<LoginCommand>(c => c.Username == "testuser" && c.Password == "password123"),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        /// <summary>
        /// Test: Login with different usernames should handle each case correctly
        /// </summary>
        [Theory]
        [InlineData("user1", "pass1")]
        [InlineData("user2", "pass2")]
        [InlineData("admin", "admin123")]
        public async Task Login_WithDifferentCredentials_ReturnsExpectedResult(string username, string password)
        {
            // Arrange
            var userapp = new Userapp { Username = username, Password = password };
            var expectedResult = new LoginResult($"token-for-{username}", username);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Login(userapp);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<LoginResult>(okResult.Value);
            Assert.Equal(username, returnValue.Username);
            Assert.Contains(username, returnValue.Token);
        }
    }
}
