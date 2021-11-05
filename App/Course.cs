using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class Course:Entity
    {
        public static readonly Course Calculus = new Course(1, "Caculus");
        public static readonly Course Chemistry = new Course(2, "Chemistry");

        public static readonly Course[] AllCourses = { Calculus, Chemistry };

        public string Name { get; }

        protected Course()
        {

        }

        private Course(long id, string name) : base(id)
        {
            this.Name = name;
        }

        public static Course FromId(long id)
        {
            return AllCourses.SingleOrDefault(x => x.Id == id);
        }


    }
}
