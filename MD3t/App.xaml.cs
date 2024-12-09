using MD3t.DB;

namespace MD3t
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            // Resolve DbContext
            var dbContext = serviceProvider.GetRequiredService<SchoolDbContext>();

            // Set the MainPage with navigation
            MainPage = new NavigationPage(new MainPage(dbContext));
        }
    }
}
