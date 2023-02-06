﻿using Org.BouncyCastle.Bcpg.Sig;
using Project4.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
                    Prijs_tb.Text = "€ " + nieuwprijs.ToString();
                }
                else if (groot_rb.IsChecked == true)
                {
                    double nieuwprijs = prijs * 1.2;
                    NaamPizza_tb.Text = naam;
                    Prijs_tb.Text = "€ " + nieuwprijs.ToString();
                }
                else
                {
                    NaamPizza_tb.Text = naam;
                    Prijs_tb.Text = "€ " + prijs.ToString();
                }
            }
            else
            {
                Prijs_tb.Text = "";
            }
        }

        private void DogMessage_Click(object sender, RoutedEventArgs e)
        {
            Bread myDog = new Bread();
            myDog.Name = "mollitia";
            myDog.MakeSound(); // Output: "The dog barks."
        }
    }
}
