using MySql.Data.MySqlClient;
using System.Windows;

namespace ProjectManagementSystem
{
    public partial class ProjectDetailsWindow : Window
    {
        private Project project;
        private string connectionString = "server=localhost;user=root;password=HgS82_klj1;database=ProjectDB";

        public ProjectDetailsWindow(Project project)
        {
            InitializeComponent();
            this.project = project;
            DataContext = project;
        }

        private void DeleteProjectFromDb(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string deleteQuery = "DELETE FROM Projects WHERE Id = @id";
                MySqlCommand cmd = new MySqlCommand(deleteQuery, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bu projeyi silmek istediğinize emin misiniz?",
                                         "Onayla",
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                DeleteProjectFromDb(project.Id);

                MessageBox.Show("Proje silindi.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);

                this.DialogResult = true;
                this.Close();
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Güncelleme özelliği henüz eklenmedi.");
        }
    }
}