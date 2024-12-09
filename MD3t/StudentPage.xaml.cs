using MD3t.DB;
using Microsoft.Maui.Controls;

namespace MD3t;

public partial class StudentPage : ContentPage
{
    private readonly SchoolDbContext _dbContext;

    public StudentPage(SchoolDbContext dbContext)
    {
        InitializeComponent();
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    // Display all students
    private void OnDisplayStudentsClicked(object sender, EventArgs e)
    {
        try
        {
            StudentsList.Children.Clear();

            var students = _dbContext.Students.ToList();
            foreach (var student in students)
            {
                var studentLayout = new HorizontalStackLayout
                {
                    Spacing = 10
                };

                var nameLabel = new Label
                {
                    Text = student.FullName,
                    VerticalOptions = LayoutOptions.Center
                };

                var editButton = new Button
                {
                    Text = "Edit",
                    CommandParameter = student.Id // Pass the student ID
                };
                editButton.Clicked += OnEditStudentClicked;

                var deleteButton = new Button
                {
                    Text = "Delete",
                    CommandParameter = student.Id // Pass the student ID
                };
                deleteButton.Clicked += OnDeleteStudentClicked;

                studentLayout.Children.Add(nameLabel);
                studentLayout.Children.Add(editButton);
                studentLayout.Children.Add(deleteButton);

                StudentsList.Children.Add(studentLayout);
            }
        }
        catch (Exception ex)
        {
            // Display error message
            DisplayAlert("Error", $"An error occurred while displaying students: {ex.Message}", "OK");
        }
    }

    // Add a new student
    private async void OnAddStudentClicked(object sender, EventArgs e)
    {
        string name = await DisplayPromptAsync("New Student", "Enter the student's name:");
        string surname = await DisplayPromptAsync("New Student", "Enter the student's surname:");
        string gender = await DisplayPromptAsync("New Student", "Enter the student's gender (Man, Woman, Other):");
        string idNumber = await DisplayPromptAsync("New Student", "Enter the student's ID number:");

        try
        {
            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(surname) &&
                !string.IsNullOrWhiteSpace(gender) && !string.IsNullOrWhiteSpace(idNumber))
            {
                var student = new Student(name, surname, gender, idNumber);
                _dbContext.Students.Add(student);
                await _dbContext.SaveChangesAsync();
                OnDisplayStudentsClicked(null, null); // Refresh the list
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to add student: {ex.Message}", "OK");
        }
    }

    // Edit a student
    private async void OnEditStudentClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is int studentId)
        {
            var student = _dbContext.Students.FirstOrDefault(s => s.Id == studentId);
            if (student == null) return;

            string newName = await DisplayPromptAsync("Edit Student", "Enter the new name:", initialValue: student.Name);
            string newSurname = await DisplayPromptAsync("Edit Student", "Enter the new surname:", initialValue: student.Surname);
            string newGender = await DisplayPromptAsync("Edit Student", "Enter the new gender (Man, Woman, Other):", initialValue: student.Gender);
            string newIdNumber = await DisplayPromptAsync("Edit Student", "Enter the new ID number:", initialValue: student.StudentIdNumber);

            try
            {
                student.Name = newName;
                student.Surname = newSurname;
                student.Gender = newGender;
                student.StudentIdNumber = newIdNumber;

                await _dbContext.SaveChangesAsync();
                OnDisplayStudentsClicked(null, null); // Refresh the list
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to edit student: {ex.Message}", "OK");
            }
        }
    }

    // Delete a student
    private async void OnDeleteStudentClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is int studentId)
        {
            var student = _dbContext.Students.FirstOrDefault(s => s.Id == studentId);
            if (student == null) return;

            bool confirm = await DisplayAlert("Delete Student", $"Are you sure you want to delete {student.FullName}?", "Yes", "No");
            if (confirm)
            {
                try
                {
                    _dbContext.Students.Remove(student);
                    await _dbContext.SaveChangesAsync();
                    OnDisplayStudentsClicked(null, null); // Refresh the list
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to delete student: {ex.Message}", "OK");
                }
            }
        }
    }
}