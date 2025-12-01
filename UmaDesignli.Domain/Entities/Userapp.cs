using System.ComponentModel;

namespace UmaDesignli.Domain.Entities
{
    /// <summary>
    /// Userapp DTO - Simple class for login authentication as per challenge requirements
    /// Contains only username and password fields
    /// </summary>
    [Description("Userapp")]
    public sealed class Userapp
    {
        /// <summary>
        /// Username for authentication
        /// </summary>
        /// <example>jperez</example>
        public required string Username { get; set; }

        /// <summary>
        /// Password for authentication
        /// </summary>
        /// <example>password@123</example>
        public required string Password { get; set; }
    }
}
