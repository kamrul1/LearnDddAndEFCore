using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = GetConnectionString();
            ILoggerFactory loggerFatory = CreateLoggerFactory();


            var optionsBuilder = new DbContextOptionsBuilder<SchoolContext>();
            optionsBuilder
                .UseSqlServer(connectionString)
                .UseLoggerFactory(loggerFatory)
                .EnableSensitiveDataLogging();

            using (var context = new SchoolContext(optionsBuilder.Options))
            {
                Student student = context.Students.Find(1L);

                student.Name += "2";
                student.Email += "2";

                context.SaveChanges();
            }
        }

        private static ILoggerFactory CreateLoggerFactory()
        {
            return LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter((category, level) =>
                        category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                    .AddConsole();

            });
        }

        private static string GetConnectionString()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return configuration["ConnectionString"];

        }
    }
}
