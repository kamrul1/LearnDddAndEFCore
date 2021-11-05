using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class StudentRepository
    {
        private readonly SchoolContext context;

        public StudentRepository(SchoolContext context)
        {
            this.context = context;
        }

        public Student GetById(long studentId)
        {
            Student student = context.Students.Find(studentId);

            if(student is null)
            {
                return null;
            }

            context.Entry(student).Collection(x => x.Enrollments).Load();

            return student;
        }
        
    }
}
