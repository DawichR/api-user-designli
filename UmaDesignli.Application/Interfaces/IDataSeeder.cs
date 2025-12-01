namespace UmaDesignli.Application.Interfaces
{
    /// <summary>
    ///  Data seeder for generic seeders.
    /// </summary>
    public interface IDataSeeder
    {
        /// <summary>
        /// Seed methods for generate entities.
        /// </summary>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>seed information</returns>
        Task SeedAsync(CancellationToken cancellationToken = default);
    }
}
