using Microsoft.Extensions.Logging;
using UmaDesignli.Domain.Entities;

namespace UmaDesignli.Infrastructure.Persistence.Seeds
{
    /// <summary>
    /// User seeding - Creates test users for in-memory database
    /// </summary>
    public class UserSeeding
    {
        public static Task<List<User>> Seed(ILogger logger, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Initialize seeding information for Users");
            try
            {
                var users = new List<User>
                {
                    new User
                    {
                        Username = "jperez",
                        Name = "Juan",
                        LastName = "Pérez",
                        Email = "juan.perez@designli.co",
                        Password = "password@123",
                    },
                    new User
                    {
                        Username = "mgarcia",
                        Name = "María",
                        LastName = "García",
                        Email = "maria.garcia@designli.co",
                        Password = "password@123",

                    },
                    new User
                    {
                        Username = "crodriguez",
                        Name = "Carlos",
                        LastName = "Rodríguez",
                        Email = "carlos.rodriguez@designli.co",
                        Password = "password@123",

                    }
                };

                logger.LogInformation($"Successfully created {users.Count} users for seeding");
                return Task.FromResult(users);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"The seeder method for {nameof(UserSeeding)} has an error.");
                return Task.FromResult(new List<User>());
            }
        }
    }
}
