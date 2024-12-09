using System.Collections.Generic;

namespace MD3t.DB
{
    public class Course
    {
        public int Id { get; set; } // Primary Key
        public string Name { get; set; }
        public int TeacherId { get; set; } // Foreign Key
        public Teacher Teacher { get; set; }

        public Course() { }

        public Course(string name, Teacher teacher)
        {
            Name = name;
            Teacher = teacher;
            TeacherId = teacher.Id;
        }

        public override string ToString()
        {
            return $"Course Name: {Name}, Taught by: {Teacher.FullName}";
        }
    }
}