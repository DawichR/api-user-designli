using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UmaDesignli.Domain.Entities
{
    /// <summary>
    /// User Entity - Represents a complete user in the system
    /// </summary>
    [Description("User")]
    public sealed class User
    {
        /// <summary>
        /// User Identifier
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Username - unique identifier for login
        /// </summary>
        /// <example>jperez</example>
        public required string Username { get; set; }

        /// <summary>
        /// Password - user's authentication credential
        /// </summary>
        /// <example>password@123</example>
        public required string Password { get; set; }

        /// <summary>
        /// Email address
        /// </summary>
        /// <example>juan.perez@example.com</example>
        [EmailAddress]
        public required string Email { get; set; }
        
        /// <summary>
        /// First name
        /// </summary>
        /// <example>Juan</example>
        public required string Name { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        /// <example>Pérez</example>
        public required string LastName { get; set; }
    }
}
