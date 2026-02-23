public interface IDataSeeder
{
    int Order { get; }     // execution sırası
    Task SeedAsync();
}
