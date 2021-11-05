using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class Student: Entity
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public virtual Course FavoriteCourse{ get; private set; }

        private readonly List<Enrollment> enrollments = new List<Enrollment>();
        public virtual IReadOnlyList<Enrollment> Enrollments
        {
            get
            {
                return enrollments.ToList();
            }
        }

        protected Student()
        {
        }

        public Student(string name, string email, Course favoriteCourse): this()
        {
            Name = name;
            Email = email;
            FavoriteCourse = favoriteCourse;
        }

        public void EnrollIn(Course course, Grade grade)
        {
            var enroll = new Enrollment(course, this, grade);
            enrollments.Add(enroll);
        }
    }
}
