using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UmaDesignli.Domain.Entities
{
    /// <summary>
    /// Userapp Class
    /// </summary>
    [Description("Userapp")]
    public sealed class Userapp
    {
        /// <summary>
        /// Username Identifier
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        /// <example>Juan</example>
        public required string Username { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        /// <example>Fernandez</example>
        public required string Password { get; set; }


        /// <summary>
        /// Email
        /// </summary>
        /// <example>j.felix@designli.co</example>
        [EmailAddress]
        public string Email { get; set; }
        
        /// <summary>
        /// Name
        /// </summary>
        /// <example>Juan</example>
        public string Name { get; set; }

        /// <summary>
        /// Lastname
        /// </summary>
        /// <example>Felix</example>
        public string LastName { get; set; }

    }
}
