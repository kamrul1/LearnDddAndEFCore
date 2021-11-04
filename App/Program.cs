using System.IO;
using System.Linq;
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


            using (var context = new SchoolContext(connectionString, true))
            {
                //Student student = context.Students.Find(1L);

                Student student = context.Students
                    .Include(x => x.FavoriteCourse)
                    .SingleOrDefault(x => x.Id == 1);
            }
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
