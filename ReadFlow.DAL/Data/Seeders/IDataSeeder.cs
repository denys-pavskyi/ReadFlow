namespace ReadFlow.DAL.Data.Seeders;

public interface IDataSeeder
{
    Task SeedAsync(AppDbContext context);
}
