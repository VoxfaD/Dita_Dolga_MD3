
using MD3t.DB;

namespace MD3t;

public partial class MainPage : ContentPage
{
    private readonly SchoolDbContext _dbContext;

    public MainPage(SchoolDbContext dbContext)
    {
        InitializeComponent();
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    private async void OnDisplayTablesClicked(object sender, EventArgs e)
    {
        try
        {
            // Fetch data from each table and format it
            var teachers = _dbContext.Teachers.ToList();
            var students = _dbContext.Students.ToList();
            var courses = _dbContext.Courses.ToList();
            var assignments = _dbContext.Assignements.ToList();
            var submissions = _dbContext.Submissions.ToList();

            string result = "Database Tables:\n\n";

            result += "Teachers:\n" + string.Join("\n", teachers.Select(t => t.ToString())) + "\n\n";
            result += "Students:\n" + string.Join("\n", students.Select(s => s.ToString())) + "\n\n";
            result += "Courses:\n" + string.Join("\n", courses.Select(c => c.ToString())) + "\n\n";
            result += "Assignments:\n" + string.Join("\n", assignments.Select(a => a.ToString())) + "\n\n";
            result += "Submissions:\n" + string.Join("\n", submissions.Select(s => s.ToString())) + "\n\n";

            ResultsLabel.Text = result;
        }
        catch (Exception ex)
        {
            ResultsLabel.Text = $"Error fetching data: {ex.Message}";
        }
    }


    private async void OnGenerateTestDataClicked(object sender, EventArgs e)
    {
        try
        {
            // Clear existing data
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            // Generate test data
            var teacher = new Teacher("Alise", "Smurla", "Woman", DateTime.Now.AddYears(-5));
            var student = new Student("Bobbijs", "Julijs", "Man", "S12345");
            var course = new Course("Matematika", teacher);
            var assignment = new Assignement(DateTime.Now.AddDays(10), course, "MD #1");
            var submission = new Submission(assignment, student, DateTime.Now, 95);

            var teacher2 = new Teacher("Rihards", "Dovalo", "Man", DateTime.Now.AddYears(-12));
            var student2 = new Student("Liene", "Žiglā", "Woman", "S78901");
            var course2 = new Course("Māksla", teacher2);
            var assignment2 = new Assignement(DateTime.Now.AddDays(55), course2, "Zīmējums");
            var submission2 = new Submission(assignment2, student2, DateTime.Now, 95);

            _dbContext.Teachers.Add(teacher);
            _dbContext.Students.Add(student);
            _dbContext.Courses.Add(course);
            _dbContext.Assignements.Add(assignment);
            _dbContext.Submissions.Add(submission);

            _dbContext.Teachers.Add(teacher2);
            _dbContext.Students.Add(student2);
            _dbContext.Courses.Add(course2);
            _dbContext.Assignements.Add(assignment2);
            _dbContext.Submissions.Add(submission2);

            _dbContext.SaveChanges();

            ResultsLabel.Text = "Test data generated successfully!";
        }
        catch (Exception ex)
        {
            ResultsLabel.Text = $"Error generating test data: {ex.Message}";
        }

    }

    private async void OnDeleteAllClicked(object sender, EventArgs e)
    {
        try
        {
            // Remove all records from each table
            _dbContext.Teachers.RemoveRange(_dbContext.Teachers);
            _dbContext.Students.RemoveRange(_dbContext.Students);
            _dbContext.Courses.RemoveRange(_dbContext.Courses);
            _dbContext.Assignements.RemoveRange(_dbContext.Assignements);
            _dbContext.Submissions.RemoveRange(_dbContext.Submissions);

            // Save changes to apply deletions
            await _dbContext.SaveChangesAsync();

            ResultsLabel.Text = "All data has been deleted successfully.";
        }
        catch (Exception ex)
        {
            ResultsLabel.Text = $"Error deleting data: {ex.Message}";
        }
    }
    private async void OnNavigateToStudentPageClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new StudentPage(_dbContext));
    }

    private async void OnNavigateToAssignementPageClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AssignementPage(_dbContext));
    }

    private async void OnNavigateToSubmissionPageClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SubmissionPage(_dbContext));
    }

}