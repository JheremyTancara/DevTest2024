using Microsoft.EntityFrameworkCore;
using Project.DataAccess;

namespace Project.Configurations
{
  public static class DbContextConfig
  {
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
      var connectionString = configuration.GetConnectionString("MySQLConnection");
      services.AddDbContext<AppDataContext>(options =>
        options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 25))));
      return services;
    }
  }
}
