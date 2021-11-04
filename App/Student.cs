﻿using System;
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

        protected Student()
        {
        }

        public Student(string name, string email, Course favoriteCourse): this()
        {
            Name = name;
            Email = email;
            FavoriteCourse = favoriteCourse;
        }
    }
}
