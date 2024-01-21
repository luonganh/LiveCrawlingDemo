using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LiveCrawlingDemo
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<Lottery> Lotteries { get; set; }

        /// <summary>
        /// IDesignTimeDbContextFactory
        /// using Microsoft.EntityFrameworkCore.Design;
        /// </summary>
        public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
        {
            public AppDbContext CreateDbContext(string[] args)
            {
                /// Iconfiguration
                /// ConfigurationBuilder
                /// using Microsoft.Extensions.Configuration;
                IConfiguration configuration = new ConfigurationBuilder()

                    /// SetBasePath
                    /// using Microsoft.Extensions.Configuration.FileExtensions
                    /// using Microsoft.Extensions.Configuration.Json                   
                    /// Directory
                    /// using Sytem.IO
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)                    
                    .Build();

                /// DbContextOptionsBuilder
                /// using Microsoft.EntityFrameworkCore;
                var builder = new DbContextOptionsBuilder<AppDbContext>();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                builder.UseSqlServer(connectionString, o => o.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name));
                return new AppDbContext(builder.Options);
            }
        }
    }
}
