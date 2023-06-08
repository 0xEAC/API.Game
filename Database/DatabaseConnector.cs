using Microsoft.EntityFrameworkCore;
using API.Game.Backend.Database.Tables;

namespace API.Game.Backend.Database
{
    public class DatabaseConnector : DbContext
    {

        public static string GetConnectionString()
        {
            return "server=localhost;user=root;password=;database=apigame";
        }

        public static DbContextOptions<DatabaseConnector> CreateOptions()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseConnector>();
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 26));

            optionsBuilder.UseMySql(GetConnectionString(), serverVersion);

            return optionsBuilder.Options;

        }

        public static DatabaseConnector CreateContext()
        {
            var optionsBuilder = CreateOptions();

            return new DatabaseConnector(optionsBuilder);
        }

        public DatabaseConnector(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Animal> Animals { get; set; }
    }
}
