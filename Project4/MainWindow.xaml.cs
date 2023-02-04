using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using BCrypt.Net;
using Project4.screens;
using System.Runtime.CompilerServices;

namespace Project4
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            using (MySqlConnection _connection = new MySqlConnection("Server=localhost;Database=project4;Uid=root;Pwd=;"))
            {
                try
                {
                    _connection.Open();
                    string voegdata = "SELECT * FROM users where email=@email";
                    MySqlCommand cmd = new MySqlCommand(voegdata, _connection);

                    cmd.Parameters.AddWithValue("@email", email.Text);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    bool ingelogd = false;
                    while (!ingelogd && reader.Read())
                    {
                        ingelogd = (BCrypt.Net.BCrypt.Verify(password.Name, (string)reader["password"]));
                    }

                    if (ingelogd)
                    {
                        HomeScreen h1 = new HomeScreen();
                        this.Close();
                        h1.Show();
                    }
                    else
                    {
                        MessageBox.Show("Wachtwoord en of Email is onjuist");
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show($"Fout {error.Message}.");
                }
            }
        }

        private void register_Click(object sender, RoutedEventArgs e)
        {
            RegisterLoginScreen l1 = new RegisterLoginScreen();
            this.Close();
            l1.Show();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
