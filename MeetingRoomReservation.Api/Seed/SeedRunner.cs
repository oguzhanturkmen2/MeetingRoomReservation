public class SeedRunner
{
    private readonly IEnumerable<IDataSeeder> _seeders;

    public SeedRunner(IEnumerable<IDataSeeder> seeders)
    {
        _seeders = seeders;
    }

    public async Task RunAsync()
    {
        var orderedSeeders = _seeders
            .OrderBy(x => x.Order);

        foreach (var seeder in orderedSeeders)
        {
            await seeder.SeedAsync();
        }
    }
}
