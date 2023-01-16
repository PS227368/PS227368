using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities.Collections;
using Project4.screens;

namespace Project4.screens
{
    /// <summary>
    /// Interaction logic for RegisterLoginScreen.xaml
    /// </summary>
    public partial class RegisterLoginScreen : Window
    {

        public RegisterLoginScreen()
        {
            InitializeComponent();
        }

        private void register_Click(object sender, RoutedEventArgs e)
        {
            using (MySqlConnection _connection = new MySqlConnection("Server=localhost;Database=project4;Uid=root;Pwd=;"))
                try
            {
                _connection.Open();
                string reg = "INSERT INTO users(name, email, password) VALUES(@name,@email,@password)";
                MySqlCommand cmd = new MySqlCommand(reg, _connection);
                 
                    //hash het wachtwoord:
                    string salt = BCrypt.Net.BCrypt.GenerateSalt();
                    string hash = BCrypt.Net.BCrypt.HashPassword(pasreg.Text, salt);

                cmd.Parameters.AddWithValue("@name", naamreg.Text);
                cmd.Parameters.AddWithValue("@email", emailreg.Text);
                cmd.Parameters.AddWithValue("@password", hash);

                int Count = Convert.ToInt32(cmd.ExecuteScalar());

                MessageBox.Show("uw account is aangemaakt log nu in");
                MainWindow m1 = new MainWindow();
                this.Close();
                m1.Show();
               

            }
                catch (Exception error)
                {
                    MessageBox.Show($"Fout {error.Message}.");
                }


        }
    }
}
