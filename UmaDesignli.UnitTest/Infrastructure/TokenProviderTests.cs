using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UmaDesignli.Domain.Entities;
using UmaDesignli.Infrastructure.Token;
using Xunit;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace UmaDesignli.UnitTest.Infrastructure
{
    /// <summary>
    /// Unit tests for TokenProvider
    /// Tests JWT token generation with proper claims and configuration
    /// </summary>
    public class TokenProviderTests
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly TokenProvider _tokenProvider;

        public TokenProviderTests()
        {
            _configurationMock = new Mock<IConfiguration>();

            // Setup configuration values
            _configurationMock.Setup(c => c["Jwt:Secret"])
                .Returns("test-secret-key-that-must-be-at-least-32-characters-long-for-security");
            _configurationMock.Setup(c => c["Jwt:Issuer"])
                .Returns("TestIssuer");
            _configurationMock.Setup(c => c["Jwt:Audience"])
                .Returns("TestAudience");
            _configurationMock.Setup(c => c.GetSection("Jwt:ExpirationInMinutes").Value)
                .Returns("60");

            _tokenProvider = new TokenProvider(_configurationMock.Object);
        }

        /// <summary>
        /// Test: Create should generate a valid JWT token
        /// </summary>
        [Fact]
        public void Create_WithValidUser_ReturnsValidJwtToken()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "testuser",
                Password = "password123",
                Email = "test@example.com",
                Name = "Test",
                LastName = "User"
            };

            // Act
            var token = _tokenProvider.Create(user);

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);
            Assert.Contains(".", token); // JWT format: header.payload.signature
        }

        /// <summary>
        /// Test: Generated token should contain correct user claims
        /// </summary>
        [Fact]
        public void Create_TokenContainsCorrectClaims()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "jperez",
                Password = "password123",
                Email = "juan.perez@example.com",
                Name = "Juan",
                LastName = "Pérez"
            };

            // Act
            var token = _tokenProvider.Create(user);

            // Assert - Decode token to verify claims
            var handler = new JsonWebTokenHandler();
            var jsonToken = handler.ReadJsonWebToken(token);

            Assert.Equal("1", jsonToken.Subject);
            Assert.Equal("juan.perez@example.com", jsonToken.GetClaim(JwtRegisteredClaimNames.Email).Value);
            Assert.Equal("jperez", jsonToken.GetClaim(JwtRegisteredClaimNames.Nickname).Value);
            Assert.Contains("Juan", jsonToken.GetClaim(JwtRegisteredClaimNames.Name).Value);
            Assert.Contains("Pérez", jsonToken.GetClaim(JwtRegisteredClaimNames.Name).Value);
        }

        /// <summary>
        /// Test: Token should have correct issuer and audience
        /// </summary>
        [Fact]
        public void Create_TokenHasCorrectIssuerAndAudience()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "testuser",
                Password = "password123",
                Email = "test@example.com",
                Name = "Test",
                LastName = "User"
            };

            // Act
            var token = _tokenProvider.Create(user);

            // Assert
            var handler = new JsonWebTokenHandler();
            var jsonToken = handler.ReadJsonWebToken(token);

            Assert.Equal("TestIssuer", jsonToken.Issuer);
            Assert.Contains("TestAudience", jsonToken.Audiences);
        }

        /// <summary>
        /// Test: Token should have an expiration time
        /// </summary>
        [Fact]
        public void Create_TokenHasExpirationTime()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "testuser",
                Password = "password123",
                Email = "test@example.com",
                Name = "Test",
                LastName = "User"
            };

            // Act
            var token = _tokenProvider.Create(user);

            // Assert
            var handler = new JsonWebTokenHandler();
            var jsonToken = handler.ReadJsonWebToken(token);

            Assert.True(jsonToken.ValidTo > DateTime.UtcNow);
        }

        /// <summary>
        /// Test: Create tokens for different users should generate different tokens
        /// </summary>
        [Theory]
        [InlineData(1, "user1", "user1@test.com")]
        [InlineData(2, "user2", "user2@test.com")]
        [InlineData(3, "user3", "user3@test.com")]
        public void Create_DifferentUsers_GeneratesDifferentTokens(int id, string username, string email)
        {
            // Arrange
            var user = new User
            {
                Id = id,
                Username = username,
                Password = "password123",
                Email = email,
                Name = "Test",
                LastName = "User"
            };

            // Act
            var token = _tokenProvider.Create(user);

            // Assert
            var handler = new JsonWebTokenHandler();
            var jsonToken = handler.ReadJsonWebToken(token);

            Assert.Equal(id.ToString(), jsonToken.Subject);
            Assert.Equal(email, jsonToken.GetClaim(JwtRegisteredClaimNames.Email).Value);
            Assert.Equal(username, jsonToken.GetClaim(JwtRegisteredClaimNames.Nickname).Value);
        }

        /// <summary>
        /// Test: Token should properly format full name with space
        /// </summary>
        [Fact]
        public void Create_FormatsFullNameCorrectly()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "jperez",
                Password = "password123",
                Email = "juan@example.com",
                Name = "Juan",
                LastName = "Pérez"
            };

            // Act
            var token = _tokenProvider.Create(user);

            // Assert
            var handler = new JsonWebTokenHandler();
            var jsonToken = handler.ReadJsonWebToken(token);
            var fullName = jsonToken.GetClaim(JwtRegisteredClaimNames.Name).Value;

            Assert.Equal("Juan Pérez", fullName);
            Assert.Contains(" ", fullName); // Should have space between names
        }
    }
}
