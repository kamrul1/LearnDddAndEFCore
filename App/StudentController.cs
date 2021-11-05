using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public sealed class StudentController
    {
        private readonly SchoolContext context;
        private readonly StudentRepository studentRepository;

        public StudentController(SchoolContext context)
        {
            this.context = context;
            this.studentRepository = new StudentRepository(context);
        }

        public string CheckStudentFavoriteCourse(long studentId, long courseId)
        {
            Student student = context.Students
                    .Find(studentId);

            if(student is null)
            {
                return "Student not found";
            }

            Course course = Course.FromId(courseId);

            if(course is null)
            {
                return "Course not found";
            }

            return student.FavoriteCourse == course ? "Yes" : "No";
        }

        public string EnrollStudent(long studentId, long courseId, Grade grade)
        {
            var student = studentRepository.GetById(studentId);

            if (student is null)
            {
                return "Student not found";
            }

            Course course = Course.FromId(courseId);

            if (course is null)
            {
                return "Course not found";
            }

            var result = student.EnrollIn(course, grade);

            context.SaveChanges();

            return result;

        }
        
    }
}
