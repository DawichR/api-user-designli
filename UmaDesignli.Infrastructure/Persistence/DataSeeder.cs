using Microsoft.Extensions.Logging;
using UmaDesignli.Application.Interfaces;
using UmaDesignli.Application.Interfaces.Repositories;
using UmaDesignli.Domain.Entities;
using UmaDesignli.Infrastructure.Persistence.Seeds;

namespace UmaDesignli.Infrastructure.Persistence
{
    /// <summary>
    /// Data seeder - Initializes in-memory database with test users
    /// </summary>
    public class DataSeeder : IDataSeeder
    {
        private readonly ILogger<DataSeeder> _logger;
        private readonly IRepository<User> _userRepository;

        public DataSeeder(ILogger<DataSeeder> logger, IRepository<User> userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Seed generic method for Seeding information around the app.
        /// </summary>
        /// <param name="cancellationToken">cancelation tokens</param>
        /// <returns>Success of seeding data</returns>
        /// <exception cref="Exception">exception</exception>
        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                // Get seed users
                var users = await UserSeeding.Seed(_logger, cancellationToken);
                // Add to repository
                await _userRepository.AddRangeAsync(users);

                _logger.LogInformation($"Successfully seeded {users.Count} users to the application.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The seeder method has an error.");
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
