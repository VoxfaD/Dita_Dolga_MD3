using MD3t.DB;
using Microsoft.Maui.Controls;
using Microsoft.EntityFrameworkCore;

namespace MD3t;

public partial class SubmissionPage : ContentPage
{
    private readonly SchoolDbContext _dbContext;

    public SubmissionPage(SchoolDbContext dbContext)
    {
        InitializeComponent();
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    // Display all submissions
    private void OnDisplaySubmissionsClicked(object sender, EventArgs e) // parāda iesniegumus
    {
        SubmissionsList.Children.Clear();

        try
        {
            // Include related Assignment and Student entities
            var submissions = _dbContext.Submissions
                .Include(s => s.Assignment)
                .Include(s => s.Student)
                .ToList();

            foreach (var submission in submissions)
            {
                var submissionLayout = new HorizontalStackLayout
                {
                    Spacing = 10
                };

                var descriptionLabel = new Label
                {
                    Text = $"Assignment: {submission.Assignment?.Description ?? "N/A"}, " +
                           $"Student: {submission.Student?.FullName ?? "N/A"}, " +
                           $"Score: {submission.Score}",
                    VerticalOptions = LayoutOptions.Center
                };

                var editButton = new Button
                {
                    Text = "Edit",
                    CommandParameter = submission.Id // Pass the submission ID
                };
                editButton.Clicked += OnEditSubmissionClicked;

                var deleteButton = new Button
                {
                    Text = "Delete",
                    CommandParameter = submission.Id // Pass the submission ID
                };
                deleteButton.Clicked += OnDeleteSubmissionClicked;

                submissionLayout.Children.Add(descriptionLabel);
                submissionLayout.Children.Add(editButton);
                submissionLayout.Children.Add(deleteButton);

                SubmissionsList.Children.Add(submissionLayout);
            }
        }
        catch (Exception ex)
        {
            // Display the error in a popup or log it
            DisplayAlert("Error", $"Failed to load submissions: {ex.Message}", "OK");
        }
    }

    // Add a new submission
    private async void OnAddSubmissionClicked(object sender, EventArgs e) // pieliek iesniegumus
    {
        try
        {
            var assignments = _dbContext.Assignements.ToList();
            var assignmentNames = assignments.Select(a => a.Description).ToArray();
            var selectedAssignment = await DisplayActionSheet("Select Assignment", "Cancel", null, assignmentNames);

            var assignment = assignments.FirstOrDefault(a => a.Description == selectedAssignment);
            if (assignment == null) return;

            var students = _dbContext.Students.ToList();
            var studentNames = students.Select(s => s.FullName).ToArray();
            var selectedStudent = await DisplayActionSheet("Select Student", "Cancel", null, studentNames);

            var student = students.FirstOrDefault(s => s.FullName == selectedStudent);
            if (student == null) return;

            string scoreString = await DisplayPromptAsync("New Submission", "Enter the score (0-100):");
            if (int.TryParse(scoreString, out var score))
            {
                var submission = new Submission
                {
                    AssignmentId = assignment.Id,
                    Assignment = assignment,
                    StudentId = student.Id,
                    Student = student,
                    Score = score,
                    SubmissionTime = DateTime.Now
                };

                _dbContext.Submissions.Add(submission);
                await _dbContext.SaveChangesAsync();

                OnDisplaySubmissionsClicked(null, null); // Refresh the list
            }
            else
            {
                await DisplayAlert("Invalid Input", "Please enter a valid score.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred while adding the submission: {ex.Message}", "OK");
        }
    }

    // Edit a submission
    private async void OnEditSubmissionClicked(object sender, EventArgs e) // rediģēt iesniegumu
    {
        try
        {
            if (sender is Button button && button.CommandParameter is int submissionId)
            {
                // Fetch the submission with its associated assignment and student
                var submission = _dbContext.Submissions
                    .Include(s => s.Assignment)
                    .Include(s => s.Student)
                    .FirstOrDefault(s => s.Id == submissionId);

                if (submission == null) return;

                // Prompt to select a new assignment
                var assignments = _dbContext.Assignements.ToList();
                string[] assignmentNames = assignments.Select(a => a.Description).ToArray();
                string selectedAssignment = await DisplayActionSheet("Select Assignment", "Cancel", null, assignmentNames);

                var assignment = assignments.FirstOrDefault(a => a.Description == selectedAssignment);
                if (assignment == null) return;

                // Prompt to select a new student
                var students = _dbContext.Students.ToList();
                string[] studentNames = students.Select(s => s.FullName).ToArray();
                string selectedStudent = await DisplayActionSheet("Select Student", "Cancel", null, studentNames);

                var student = students.FirstOrDefault(s => s.FullName == selectedStudent);
                if (student == null) return;

                // Prompt to edit the score
                string newScoreString = await DisplayPromptAsync("Edit Submission", "Enter the new score (0-100):", initialValue: submission.Score.ToString());
                if (int.TryParse(newScoreString, out var newScore))
                {
                    // Update the submission
                    submission.AssignmentId = assignment.Id;
                    submission.Assignment = assignment;
                    submission.StudentId = student.Id;
                    submission.Student = student;
                    submission.Score = newScore;

                    // Save changes to the database
                    await _dbContext.SaveChangesAsync();
                    OnDisplaySubmissionsClicked(null, null); // Refresh the list
                }
                else
                {
                    await DisplayAlert("Invalid Input", "Please enter a valid score.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred while editing the submission: {ex.Message}", "OK");
        }
    }

    // Delete a submission
    private async void OnDeleteSubmissionClicked(object sender, EventArgs e) // dzēst iesniegumu
    {
        try
        {
            if (sender is Button button && button.CommandParameter is int submissionId)
            {
                var submission = _dbContext.Submissions.FirstOrDefault(s => s.Id == submissionId);
                if (submission == null) return;

                bool confirm = await DisplayAlert("Delete Submission", $"Are you sure you want to delete this submission?", "Yes", "No");
                if (confirm)
                {
                    _dbContext.Submissions.Remove(submission);
                    await _dbContext.SaveChangesAsync();

                    OnDisplaySubmissionsClicked(null, null); // Refresh the list
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred while deleting the submission: {ex.Message}", "OK");
        }
    }
}