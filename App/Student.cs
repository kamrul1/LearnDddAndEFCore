using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class Student: Entity
    {
        public string Name { get; set; }
        public Email Email { get; set; }
        public virtual Course FavoriteCourse{ get; set; }

        private readonly List<Enrollment> enrollments = new List<Enrollment>();
        public virtual IReadOnlyList<Enrollment> Enrollments => enrollments.ToList();

        protected Student()
        {
        }

        public Student(string name, Email email, Course favoriteCourse, Grade favouriteCourseGrade): this()
        {
            Name = name;
            Email = email;
            FavoriteCourse = favoriteCourse;

            EnrollIn(favoriteCourse, favouriteCourseGrade);
        }

        public string EnrollIn(Course course, Grade grade)
        {
            if (enrollments.Any(x => x.Course == course))
            {
                return $"Already enrolled in course '{course.Name}'";
            }
            var enroll = new Enrollment(course, this, grade);
            enrollments.Add(enroll);

            return "OK";
        }

        public void Disenroll(Course course)
        {
            Enrollment enrollment = enrollments.FirstOrDefault(x => x.Course == course);

            if(enrollment is null)
            {
                return;
            }

            enrollments.Remove(enrollment);
        }


    }
}
