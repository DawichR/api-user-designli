using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UmaDesignli.Api.Controllers.Users;
using UmaDesignli.Application.Queries.Access;
using Xunit;

namespace UmaDesignli.UnitTest.Controllers
{
    /// <summary>
    /// Unit tests for UsersController
    /// Tests the user listing functionality with JWT authentication
    /// </summary>
    public class UsersControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new UsersController(_mediatorMock.Object);
        }

        /// <summary>
        /// Test: GetAll should return 200 OK with list of users
        /// </summary>
        [Fact]
        public async Task GetAll_ReturnsOkWithUserList()
        {
            // Arrange
            var expectedUsers = new List<UserResponse>
            {
                new UserResponse(1, "jperez", "juan.perez@example.com", "Juan", "Pérez"),
                new UserResponse(2, "mrodriguez", "maria.rodriguez@example.com", "María", "Rodríguez"),
                new UserResponse(3, "cgomez", "carlos.gomez@example.com", "Carlos", "Gómez")
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedUsers);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<UserResponse>>(okResult.Value);
            Assert.Equal(3, returnValue.Count);
            Assert.Equal("jperez", returnValue[0].Username);
        }

        /// <summary>
        /// Test: GetAll should call mediator with GetAllUsersQuery
        /// </summary>
        [Fact]
        public async Task GetAll_ShouldCallMediator_WithGetAllUsersQuery()
        {
            // Arrange
            var expectedUsers = new List<UserResponse>();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedUsers);

            // Act
            await _controller.GetAll();

            // Assert
            _mediatorMock.Verify(
                m => m.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        /// <summary>
        /// Test: GetAll should return empty list when no users exist
        /// </summary>
        [Fact]
        public async Task GetAll_WithNoUsers_ReturnsEmptyList()
        {
            // Arrange
            var emptyList = new List<UserResponse>();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(emptyList);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<UserResponse>>(okResult.Value);
            Assert.Empty(returnValue);
        }

        /// <summary>
        /// Test: GetAll should return correct user data structure
        /// </summary>
        [Fact]
        public async Task GetAll_ReturnsCorrectUserDataStructure()
        {
            // Arrange
            var expectedUsers = new List<UserResponse>
            {
                new UserResponse(1, "testuser", "test@example.com", "Test", "User")
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedUsers);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<UserResponse>>(okResult.Value);
            var user = returnValue.First();

            Assert.Equal(1, user.Id);
            Assert.Equal("testuser", user.Username);
            Assert.Equal("test@example.com", user.Email);
            Assert.Equal("Test", user.Name);
            Assert.Equal("User", user.LastName);
        }
    }
}
