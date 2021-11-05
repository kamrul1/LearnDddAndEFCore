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

        public string DisenrollStudent(long studentId, long courseId, Grade grade)
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

            student.Disenroll(course);
            context.SaveChanges();

            return "OK";

        }

        public string RegisterStudent(
            string name, string email, long favouriteCourseId, Grade favoriteCourseGrade)
        {
            Course course = context.Courses.Find(favouriteCourseId);

            Course favouriteCourse = Course.FromId(favouriteCourseId);
            if (favouriteCourse is null)
            {
                return "Course not found";
            }

            var student = new Student(name, email, favouriteCourse, favoriteCourseGrade);

            studentRepository.Save(student);

            context.SaveChanges();

            return "OK";

        }

        public string EditPersonalInfo(
            long studentId, string name, string email, long favoriteCourseId)
        {
            var student = studentRepository.GetById(studentId);

            if (student is null)
            {
                return "Student not found";
            }

            Course favoriteCourse = Course.FromId(favoriteCourseId);
            if (favoriteCourse is null)
            {
                return "Course not found";
            }

            student.Name = name;
            student.Email = email;
            student.FavoriteCourse = favoriteCourse;



            context.SaveChanges();

            return "OK";
        }



    }
}
