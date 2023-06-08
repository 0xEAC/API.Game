using API.Game.Backend.Database;
using Microsoft.EntityFrameworkCore.Design;

namespace WebsiteIsala
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseConnector>
    {
        public DatabaseConnector CreateDbContext(string[] args)
        {
            return new DatabaseConnector(DatabaseConnector.CreateOptions());
        }
    }
}
