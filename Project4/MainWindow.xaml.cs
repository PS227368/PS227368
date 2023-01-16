using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
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
using System.Data.SqlClient;
using System.Linq.Expressions;
using Project4.screens;
using BCrypt.Net;

namespace Project4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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
                        ingelogd = (BCrypt.Net.BCrypt.Verify(password.Text, (string)reader["password"]));
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
    }


}
