using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MD3t.DB
{
    public class SchoolSystem
    {
        // Kolekcijas ar klašu instancēm
        // Kolekcijas ar klašu instancēm
        [XmlElement("Teacher")]
        public List<Teacher> Teachers { get; set; }

        [XmlElement("Student")]
        public List<Student> Students { get; set; }

        [XmlElement("Course")]
        public List<Course> Courses { get; set; }

        [XmlElement("Assignment")]
        public List<Assignement> Assignements { get; set; }

        [XmlElement("Submission")]
        public List<Submission> Submissions { get; set; }

        // Konstruktors
        public SchoolSystem()
        {
            Teachers = new List<Teacher>();
            Students = new List<Student>();
            Courses = new List<Course>();
            Assignements = new List<Assignement>();
            Submissions = new List<Submission>();
        }

        // Metode, lai pievienotu skolotāju
        public void AddTeacher(Teacher teacher)
        {
            Teachers.Add(teacher);
        }

        // Metode, lai pievienotu studentu
        public void AddStudent(Student student)
        {
            Students.Add(student);
        }

        // Metode, lai pievienotu kursu
        public void AddCourse(Course course)
        {
            Courses.Add(course);
        }

        // Metode, lai pievienotu uzdevumu
        public void AddAssignement(Assignement assignement)
        {
            Assignements.Add(assignement);
        }

        // Metode, lai pievienotu iesniegumu
        public void AddSubmission(Submission submission)
        {
            Submissions.Add(submission);
        }

        // Metode, lai izvadītu visu informāciju par skolotājiem
        public void DisplayTeachers()
        {
            foreach (var teacher in Teachers)
            {
                Console.WriteLine(teacher.ToString());
            }
        }

        // Metode, lai izvadītu visu informāciju par studentiem
        public void DisplayStudents()
        {
            foreach (var student in Students)
            {
                Console.WriteLine(student.ToString());
            }
        }

        // Metode, lai izvadītu visu informāciju par kursiem
        public void DisplayCourses()
        {
            foreach (var course in Courses)
            {
                Console.WriteLine(course.ToString());
            }
        }

        // Metode, lai izvadītu visu informāciju par uzdevumiem
        public void DisplayAssignements()
        {
            foreach (var assignement in Assignements)
            {
                Console.WriteLine(assignement.ToString());
            }
        }

        // Metode, lai izvadītu visu informāciju par iesniegumiem
        public void DisplaySubmissions()
        {
            foreach (var submission in Submissions)
            {
                Console.WriteLine(submission.ToString());
            }
        }
    }
}
