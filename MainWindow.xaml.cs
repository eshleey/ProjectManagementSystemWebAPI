using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ProjectManagementSystem
{
    public partial class MainWindow : Window
    {
        string connectionString = "server=localhost;user=root;password=HgS82_klj1;database=ProjectDB";

        private ObservableCollection<Project> projects = new ObservableCollection<Project>();

        public MainWindow()
        {
            InitializeComponent();
            LoadProjectsFromDb();
        }

        private void LoadProjectsFromDb()
        {
            projects.Clear();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Id, Name, Description FROM Projects";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        projects.Add(new Project
                        {
                            Id = reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                            Description = reader.GetString("Description"),
                            IsAddButton = false
                        });
                    }
                }
            }

            projects.Add(new Project { IsAddButton = true });

            ProjectItemsControl.ItemsSource = projects;
        }

        private void AddProject(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description))
                return;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string insertQuery = "INSERT INTO Projects (Name, Description) VALUES (@name, @desc)";
                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);

                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@desc", description);

                cmd.ExecuteNonQuery();
            }

            LoadProjectsFromDb();
        }

        private void AddProjectButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddProjectWindow();
            bool? result = addWindow.ShowDialog();

            if (result == true)
            {
                string name = addWindow.ProjectName;
                string desc = addWindow.ProjectDescription;

                AddProject(name, desc);
            }
        }

        private void ProjectBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Project project)
            {
                if (project.IsAddButton)
                    return;

                ProjectDetailsWindow detailsWindow = new ProjectDetailsWindow(project);
                bool? result = detailsWindow.ShowDialog();

                if (result == true)
                {
                    LoadProjectsFromDb();
                }
            }
        }

    }
}
