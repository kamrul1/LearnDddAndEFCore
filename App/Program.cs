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


            bool useConsoleLogger = true; //IHostingEnvironment.IsDevelopment();


            using (var context = new SchoolContext(connectionString, useConsoleLogger))
            {
                Student student = context.Students.Find(1L);

                student.Name += "2";
                student.Email += "2";

                context.SaveChanges();
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
