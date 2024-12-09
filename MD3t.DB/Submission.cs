using System;

namespace MD3t.DB
{
    public class Submission
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }  // Ensure this is present
        public Assignement Assignment { get; set; } = new Assignement();  // Navigation property
        public int StudentId { get; set; }
        public Student Student { get; set; } = new Student();  // Navigation property
        public DateTime SubmissionTime { get; set; } = DateTime.Now;
        public int Score { get; set; }

        public Submission() { }

        public Submission(Assignement assignment, Student student, DateTime submissionTime, int score)
        {
            Assignment = assignment;
            AssignmentId = assignment.Id;
            Student = student;
            StudentId = student.Id;
            SubmissionTime = submissionTime;
            Score = score;
        }

        public override string ToString()
        {
            return $"Assignment: {Assignment.Description}, Course: {Assignment.Course.Name}, Student: {Student.FullName}, Submission Time: {SubmissionTime}, Score: {Score}";
        }
    }
}