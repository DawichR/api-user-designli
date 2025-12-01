using Microsoft.Extensions.Logging;
using UmaDesignli.Domain.Entities;

namespace UmaDesignli.Infrastructure.Persistence.Seeds
{
    public class UserSeeding
    {
        public static Task<List<Userapp>> Seed(ILogger logger, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Initialize seeding information for USers");
            try
            {
                var employees = new List<Userapp>
                {
                    new Userapp
                    {
                        Username = "jperez",
                        Name = "Juan",
                        LastName = "Pérez",
                        Email = "juan.perez@designli.co",
                        Password = "password@123",
                    },
                    new Userapp
                    {
                        Username = "mgarcia",
                        Name = "María",
                        LastName = "García",
                        Email = "maria.garcia@designli.co",
                        Password = "password@123",

                    },
                    new Userapp
                    {
                        Username = "crodriguez",
                        Name = "Carlos",
                        LastName = "Rodríguez",
                        Email = "carlos.rodriguez@designli.co",
                        Password = "password@123",

                    }
                };

                logger.LogInformation($"Successfully created {employees.Count} employees for seeding");
                return Task.FromResult(employees);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"The seeder method for {nameof(UserSeeding)} has an error.");
                return Task.FromResult(new List<Userapp>());
            }
        }
    }
}
