﻿using System;
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
            string result5 = Execute(x => x.EditPersonalInfo(1, "Carl 2", "carl@gmail.com", 1));

            //string result4 = Execute(x => x.RegisterStudent("Carl", "carl@gmail.com", 2, Grade.B));

            //string result3 = Execute(x => x.DisenrollStudent(1, 2, Grade.A));

            //string result2 = Execute(x => x.EnrollStudent(1, 2, Grade.A));

            //string result = Execute(x => x.CheckStudentFavoriteCourse(1, 2));

        }

        private static string Execute(Func<StudentController, string> func)
        {
            string connectionString = GetConnectionString();

            using (var context = new SchoolContext(connectionString, true))
            {
                var controller = new StudentController(context);
                return func(controller);
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
