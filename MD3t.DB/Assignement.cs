using System;

namespace MD3t.DB
{
    public class Assignement
    {
        public int Id { get; set; } // Primary Key
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public int CourseId { get; set; } // Foreign Key
        public Course Course { get; set; }

        public Assignement() { }

        public Assignement(DateTime deadline, Course course, string description)
        {
            Deadline = deadline;
            Course = course;
            Description = description;
            CourseId = course.Id;
        }

        public override string ToString()
        {
            return $"Assignment: {Description}, Deadline: {Deadline}, Course: {Course.Name}";
        }
    }
}