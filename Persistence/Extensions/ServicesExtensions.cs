using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Repository;

namespace Persistence.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {   var connectionString = configuration.GetConnectionString("PermissionConnection");
            services.AddDbContext<ApplicationDbContext>(
                    options => options.UseSqlServer(connectionString,
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                ));
            services.AddTransient(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
            services.AddTransient(typeof(IReadRepositoryAsync<>), typeof(ReadRepositoryAsync<>));
        }
    }
}
