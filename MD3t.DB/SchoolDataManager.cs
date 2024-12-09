using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;



namespace MD3t.DB
{
    public class SchoolDataManager : IDataManager
    {
        public SchoolSystem schoolSystem { get; private set; }

        public SchoolDataManager()
        {
            schoolSystem = new SchoolSystem();
        }

        // Metode, kas atgriež informāciju par visiem kolekcijās esošajiem elementiem
        public string Print()
        {
            StringBuilder output = new StringBuilder();

            // Informācija par skolotājiem
            output.AppendLine("Skolotāji:");
            foreach (var teacher in schoolSystem.Teachers)
            {
                output.AppendLine(teacher.ToString());
            }

            // Informācija par studentiem
            output.AppendLine("Studenti:");
            foreach (var student in schoolSystem.Students)
            {
                output.AppendLine(student.ToString());
            }

            // Informācija par kursiem
            output.AppendLine("Kursi:");
            foreach (var course in schoolSystem.Courses)
            {
                output.AppendLine(course.ToString());
            }

            // Informācija par uzdevumiem
            output.AppendLine("Uzdevumi:");
            foreach (var assignement in schoolSystem.Assignements)
            {
                output.AppendLine(assignement.ToString());
            }

            // Informācija par iesniegumiem
            output.AppendLine("Iesniegumi:");
            foreach (var submission in schoolSystem.Submissions)
            {
                output.AppendLine(submission.ToString());
            }

            return output.ToString();
        }

        // Metode, kas saglabā visus kolekciju datus failā
        public void Save(string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SchoolSystem));
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    serializer.Serialize(fileStream, schoolSystem);
                }
                Console.WriteLine($"Dati veiksmīgi saglabāti uz {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kļūda, saglabājot datus: {ex.Message}");
            }
        }

        // Metode, kas nolasītu visus kolekciju datus no faila
        public void Load(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Fails nav atrasts.");
                    return;
                }

                XmlSerializer serializer = new XmlSerializer(typeof(SchoolSystem));
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    schoolSystem = (SchoolSystem)serializer.Deserialize(fileStream);
                }
                Console.WriteLine($"Dati veiksmīgi ielādēti no {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kļūda, ielādējot datus: {ex.Message}");
            }
        }

        

        //Metode, kas parāda datus
        public void DisplayAllData()
        {
            schoolSystem.DisplayTeachers();
            schoolSystem.DisplayStudents();
            schoolSystem.DisplayCourses();
            schoolSystem.DisplayAssignements();
            schoolSystem.DisplaySubmissions();
        }

        // Metode, kas rada testa datus
        public void CreateTestData()
        {
            // Pievieno skolotājus
            Teacher teacher1 = new Teacher("Liene", "Lapa", "Woman", new DateTime(2023, 5, 1));
            Teacher teacher2 = new Teacher("Rihards", "Trenko", "Man", new DateTime(2015, 9, 7));
            schoolSystem.AddTeacher(teacher1);
            schoolSystem.AddTeacher(teacher2);

            // Pievieno studentus
            Student student1 = new Student("Loreta", "Berzina", "Woman", "S28901");
            Student student2 = new Student("Ronalds", "Kokle", "Man", "S54321");
            schoolSystem.AddStudent(student1);
            schoolSystem.AddStudent(student2);

            // Pievieno kursus
            Course course1 = new Course("Matemātika", teacher1);
            Course course2 = new Course("Fizika", teacher2);
            schoolSystem.AddCourse(course1);
            schoolSystem.AddCourse(course2);

            // Pievieno uzdevumus
            Assignement assignement1 = new Assignement(new DateTime(2024, 10, 30), course1, "Matemātikas MD 1");
            Assignement assignement2 = new Assignement(new DateTime(2024, 11, 5), course2, "Fizikas Laboratorijas darbs");
            schoolSystem.AddAssignement(assignement1);
            schoolSystem.AddAssignement(assignement2);

            // Pievieno iesniegumus
            Submission submission1 = new Submission(assignement1, student1, DateTime.Now, 90);
            Submission submission2 = new Submission(assignement2, student2, DateTime.Now, 85);
            schoolSystem.AddSubmission(submission1);
            schoolSystem.AddSubmission(submission2);

            Console.WriteLine("Testa dati veiksmīgi sataisīti.");

        }

        // Metode, kas izdzēš visus datus
        public void Reset()
        {
            schoolSystem = new SchoolSystem();
            Console.WriteLine("Visi dati atietastīti.");
        }
    }
}
