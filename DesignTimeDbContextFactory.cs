using System.IO;
using LaptopManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LaptopManagement.Models
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<LaptopManagementContext>
    {
        public LaptopManagementContext CreateDbContext(string[] args)
        {
            // Thiết lập base path dựa trên thư mục chứa appsettings.json
            var basePath = Directory.GetCurrentDirectory();
            var configuration = new ConfigurationBuilder()
                                    .SetBasePath(basePath)
                                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                    .Build();

            var builder = new DbContextOptionsBuilder<LaptopManagementContext>();
            var connectionString = configuration.GetConnectionString("LaptopManagementContext");
            builder.UseSqlServer(connectionString);

            return new LaptopManagementContext(builder.Options);
        }
    }
}