public class SeedRunner
{
    private readonly IEnumerable<IDataSeeder> _seeders;

    public SeedRunner(IEnumerable<IDataSeeder> seeders)
    {
        _seeders = seeders;
    }

    public async Task RunAsync()
    {
        foreach (var seeder in _seeders)
        {
            await seeder.SeedAsync();
        }
    }
}
