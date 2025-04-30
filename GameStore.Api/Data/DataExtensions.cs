using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data
{
    public static class DataExtensions
    {
        public static void MigrateDb(this WebApplication app) // Method to automate add data migration to DB when project starts
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
            dbContext.Database.Migrate();
        }

    }
}
