using UmaDesignli.Domain.Entities;

namespace UmaDesignli.Application.Interfaces
{
    /// <summary>
    /// Token provider interface
    /// </summary>
    public interface ITokenProvider
    {
        /// <summary>
        /// Creates a JWT token for the user
        /// </summary>
        /// <param name="user">User entity</param>
        /// <returns>JWT token string</returns>
        string Create(Userapp user);
    }
}
