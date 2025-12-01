using UmaDesignli.Application.Interfaces;

namespace UmaDesignli.Api.Services
{
    internal sealed class AppInitializer : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public AppInitializer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            // Data seeder for Employee
            var dataSeeder = serviceProvider.GetRequiredService<IDataSeeder>();
            await dataSeeder.SeedAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }

}
