using MySql.Data.MySqlClient;
using Org.BouncyCastle.Bcpg.Sig;
using Org.BouncyCastle.Math;
using Project4.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Project4
{
    /// <summary>
    /// Interaction logic for Pizzas.xaml
    /// </summary>
    public partial class Pizza : Window, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
        #region fields
        private readonly ProjectDB db = new ProjectDB();
        private readonly string serviceDeskBericht = "\n\nNeem contact op met de service desk";
        #endregion
        #region Properties
        private ObservableCollection<Products> pizzas = new();
        public ObservableCollection<Products> Pizzas
        {
            get { return pizzas; }
            set { pizzas = value; OnPropertyChanged(); }
        }
        private Products? selectedPizza;
        public Products? SelectedPizza
        {
            get { return selectedPizza; }
            set { selectedPizza = value; OnPropertyChanged(); }
        }
        private Products? pizzaInEdit;
        public Products? PizzaInEdit
        {
            get { return pizzaInEdit; }
            set { pizzaInEdit = value; OnPropertyChanged(); }
        }
        #endregion
        public Pizza()
        {
            InitializeComponent();
            PopulatePizzas();
            DataContext = this;
        }
        private readonly string connString =
            ConfigurationManager.ConnectionStrings["project4"].ConnectionString;
        private void PopulatePizzas()
        {
            string dbResult = db.GetPizzas(Pizzas);
            if (dbResult != ProjectDB.OK)
            {
                MessageBox.Show(dbResult + serviceDeskBericht);
            }
        }
        //private void AddPizza_Click(object sender, RoutedEventArgs e)
        //{

        //}

        private void SelectedPizza_sc(object sender, SelectionChangedEventArgs e)
        {
            DoMyThing();
        }

        private void rbklein_Checked(object sender, RoutedEventArgs e)
        {
            DoMyThing();
        }
        private void rbmedium_Checked(object sender, RoutedEventArgs e)
        {
            DoMyThing();
        }
        private void rbgroot_Checked(object sender, RoutedEventArgs e)
        {
            DoMyThing();
        }
        private void DoMyThing()
        {
            if (selectedPizza != null)
            {
                string naam = selectedPizza.Name;
                double prijs = selectedPizza.Price;

                if (klein_rb.IsChecked == true)
                {
                    double nieuwprijs = prijs * 0.8;
                    NaamPizza_tb.Text = naam;
                    Prijs_tb.Text = nieuwprijs.ToString();
                }
                else if (groot_rb.IsChecked == true)
                {
                    double nieuwprijs = prijs * 1.2;
                    NaamPizza_tb.Text = naam;
                    Prijs_tb.Text = nieuwprijs.ToString();
                }
                else
                {
                    NaamPizza_tb.Text = naam;
                    Prijs_tb.Text = prijs.ToString();
                }
            }
            else
            {
                Prijs_tb.Text = "";
            }
        }


        private void PizzaMessage_Click(object sender, RoutedEventArgs e)
        {
            Bread pizza = new Bread();
            pizza.Name = selectedPizza.Name;
            pizza.MakeSound();
        }

        private void AddtoCart_Click(object sender, RoutedEventArgs e)
        {
            int aantal = int.Parse(aantal_tb.Text);
            double prijs = double.Parse(Prijs_tb.Text);
            double totaalprijs = aantal * prijs;
            double eindprijs = double.Parse(TotaalPrijs_tb.Text);
            if (selectedPizza != null)
            {
                if (klein_rb.IsChecked == true)
                {
                        
                    aantal_tb.Text = "1";
                    groot_rb.IsChecked = false;
                    medium_rb.IsChecked = true;
                    klein_rb.IsChecked = false;
                    eindprijs = totaalprijs + eindprijs;
                    TotaalPrijs_tb.Text = eindprijs.ToString("0.00");
                }
                else if (groot_rb.IsChecked == true)
                {
                    Shoppingcart_lb.Items.Add(aantal + "x " + selectedPizza.Name + "\nFormaat: Groot \n€ " + totaalprijs.ToString("0.00"));
                    aantal_tb.Text = "1";
                    groot_rb.IsChecked = false;
                    medium_rb.IsChecked = true;
                    klein_rb.IsChecked = false;
                    eindprijs = totaalprijs + eindprijs;
                    TotaalPrijs_tb.Text = eindprijs.ToString("0.00");
                }
                else
                {
                    Shoppingcart_lb.Items.Add(aantal + "x " + selectedPizza.Name + "\nFormaat: Medium \n€ " + totaalprijs.ToString("0.00"));
                    aantal_tb.Text = "1";
                    eindprijs = totaalprijs + eindprijs;
                    TotaalPrijs_tb.Text = eindprijs.ToString("0.00");
                }
            }
            else
            {
                MessageBox.Show("There is no pizza selected", "Error", MessageBoxButton.OK);
            }
        }

        private void PizzaDelete_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult myResult = MessageBox.Show("Weet je zeker dat je " + selectedPizza.Name + " wilt verwijderen? \nAls je die wilt verwijderen klik dan op 'Ja'! \nals dat niet je bedoeling is klik dan op 'nee'!", "Menu verwijderen", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (myResult == MessageBoxResult.Yes)
            {
                Shoppingcart_lb.Items.RemoveAt(Shoppingcart_lb.Items.IndexOf(Shoppingcart_lb.SelectedItem.ToString()));
            }
            else
            {
                return;
            }
        }
        private void Bestel_Click(object sender, EventArgs e)
        {
            int orderId = 1; //GetCurrentOrderId(); // Haal het huidige OrderID op
            foreach (string pizzaString in Shoppingcart_lb.Items)
            {
                Item pizza = ConvertStringToItem(pizzaString);

                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    int aantalkeer = Convert.ToInt32(pizza.aantal);
                    while (aantalkeer < 1)
                    {
                        MySqlCommand pizzaCommand = new MySqlCommand("INSERT INTO shoppingcart (order_id, name, price) VALUES (@order_id, @name, @price)", conn);
                        pizzaCommand.Parameters.AddWithValue("@order_id", orderId);
                        pizzaCommand.Parameters.AddWithValue("@name", pizza.name);
                        pizzaCommand.Parameters.AddWithValue("@price", pizza.price);
                        conn.Open();
                        pizzaCommand.ExecuteNonQuery();
                        conn.Close();
                        aantalkeer = -1;
                        orderId = +1;
                    }
                }
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    MySqlCommand pizzaCommand = new MySqlCommand("INSERT INTO pizza_orders (first_name, last_name, adress, postal_code, city, order_id) VALUES (@voornaam, @achternaam, @adres, @postcode, @stad, @order_id)", conn);
                    pizzaCommand.Parameters.AddWithValue("@voornaam", tboxVoornaam.Text);
                    pizzaCommand.Parameters.AddWithValue("@achternaam", tboxAchternaam.Text);
                    pizzaCommand.Parameters.AddWithValue("@adres", tboxAdres.Text);
                    pizzaCommand.Parameters.AddWithValue("@postcode", tboxPostcode.Text);
                    pizzaCommand.Parameters.AddWithValue("@stad", tboxStad.Text);
                    pizzaCommand.Parameters.AddWithValue("@order_id", orderId);
                    conn.Open();
                    pizzaCommand.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        public class Item
        {
            public string name { get; set; }
            public decimal price { get; set; }
            public string aantal { get; set; }  

            public override string ToString()
            {
                return name;
            }
        }

        private Item ConvertStringToItem(string pizzaString)
        {
            // Voer hier de noodzakelijke conversie uit
            // Bijvoorbeeld:
            string[] parts = pizzaString.Split('x', ' ', '\n');
            string aantal = parts[0];
            string name = parts[2];
            decimal price = decimal.Parse(parts[7]);
            return new Item { aantal = aantal, name = name, price = price };
        }

    }
}
