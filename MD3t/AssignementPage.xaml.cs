using MD3t.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Controls;

namespace MD3t;

public partial class AssignementPage : ContentPage
{
    private readonly SchoolDbContext _dbContext;

    public AssignementPage(SchoolDbContext dbContext)
    {
        InitializeComponent();
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    // Display all assignments
    private void OnDisplayAssignementsClicked(object sender, EventArgs e)
    {
        try
        {
            AssignementsList.Children.Clear();

            var assignements = _dbContext.Assignements.Include(a => a.Course).ToList();
            foreach (var assignement in assignements)
            {
                var assignementLayout = new HorizontalStackLayout
                {
                    Spacing = 10
                };

                var descriptionLabel = new Label
                {
                    Text = $"{assignement.Description} (Course: {assignement.Course?.Name ?? "N/A"})",
                    VerticalOptions = LayoutOptions.Center
                };

                var editButton = new Button
                {
                    Text = "Edit",
                    CommandParameter = assignement.Id // Pass the assignment ID
                };
                editButton.Clicked += OnEditAssignementClicked;

                var deleteButton = new Button
                {
                    Text = "Delete",
                    CommandParameter = assignement.Id // Pass the assignment ID
                };
                deleteButton.Clicked += OnDeleteAssignementClicked;

                assignementLayout.Children.Add(descriptionLabel);
                assignementLayout.Children.Add(editButton);
                assignementLayout.Children.Add(deleteButton);

                AssignementsList.Children.Add(assignementLayout);
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", $"An error occurred while displaying assignments: {ex.Message}", "OK");
        }
    }

    // Add a new assignment
    private async void OnAddAssignementClicked(object sender, EventArgs e)
    {
        try
        {
            string description = await DisplayPromptAsync("New Assignment", "Enter the assignment description:");
            string deadlineString = await DisplayPromptAsync("New Assignment", "Enter the deadline (yyyy-MM-dd):");

            if (DateTime.TryParse(deadlineString, out var deadline))
            {
                var courses = _dbContext.Courses.ToList();
                string[] courseNames = courses.Select(c => c.Name).ToArray();
                string selectedCourse = await DisplayActionSheet("Select Course", "Cancel", null, courseNames);

                var course = courses.FirstOrDefault(c => c.Name == selectedCourse);
                if (course == null) return;

                var assignement = new Assignement
                {
                    Description = description,
                    Deadline = deadline,
                    CourseId = course.Id,
                    Course = course
                };

                _dbContext.Assignements.Add(assignement);
                await _dbContext.SaveChangesAsync();

                OnDisplayAssignementsClicked(null, null); // Refresh the list
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred while adding the assignment: {ex.Message}", "OK");
        }
    }

    // Edit an assignment
    private async void OnEditAssignementClicked(object sender, EventArgs e)
    {
        try
        {
            if (sender is Button button && button.CommandParameter is int assignementId)
            {
                // Fetch the assignment with its associated course
                var assignement = _dbContext.Assignements.Include(a => a.Course).FirstOrDefault(a => a.Id == assignementId);
                if (assignement == null) return;

                // Prompt for new description
                string newDescription = await DisplayPromptAsync("Edit Assignment", "Enter the new description:", initialValue: assignement.Description);

                // Prompt for new deadline
                string newDeadlineString = await DisplayPromptAsync("Edit Assignment", "Enter the new deadline (yyyy-MM-dd):", initialValue: assignement.Deadline.ToString("yyyy-MM-dd"));

                // Fetch courses for picker
                var courses = _dbContext.Courses.ToList();
                string[] courseNames = courses.Select(c => c.Name).ToArray();
                string selectedCourse = await DisplayActionSheet("Select Course", "Cancel", null, courseNames);

                // Find the selected course
                var course = courses.FirstOrDefault(c => c.Name == selectedCourse);
                if (course == null) return;

                // Update the assignment if inputs are valid
                if (DateTime.TryParse(newDeadlineString, out var newDeadline))
                {
                    assignement.Description = newDescription;
                    assignement.Deadline = newDeadline;
                    assignement.CourseId = course.Id;
                    assignement.Course = course;

                    // Save changes to the database
                    await _dbContext.SaveChangesAsync();
                    OnDisplayAssignementsClicked(null, null); // Refresh the list
                }
                else
                {
                    await DisplayAlert("Invalid Input", "Please enter a valid date.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred while editing the assignment: {ex.Message}", "OK");
        }
    }

    // Delete an assignment
    private async void OnDeleteAssignementClicked(object sender, EventArgs e)
    {
        try
        {
            if (sender is Button button && button.CommandParameter is int assignementId)
            {
                var assignement = _dbContext.Assignements.FirstOrDefault(a => a.Id == assignementId);
                if (assignement == null) return;

                bool confirm = await DisplayAlert("Delete Assignment", $"Are you sure you want to delete '{assignement.Description}'?", "Yes", "No");
                if (confirm)
                {
                    _dbContext.Assignements.Remove(assignement);
                    await _dbContext.SaveChangesAsync();
                    OnDisplayAssignementsClicked(null, null); // Refresh the list
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred while deleting the assignment: {ex.Message}", "OK");
        }
    }
}