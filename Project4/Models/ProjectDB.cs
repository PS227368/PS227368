using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Project4.Models
{
    public class ProjectDB
    {
        public static readonly string UNKNOWN = "Unknown";
        public static readonly string OK = "Ok";

        private readonly string connString =
            ConfigurationManager.ConnectionStrings["project4"].ConnectionString;
        public string GetPizzas(ICollection<Products> pizzas)
        {
            if (pizzas == null)
            {
                throw new ArgumentNullException("Ongeldig argument bij gebruik van GetModes");
            }
            string methodResult = UNKNOWN;
            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                            SELECT id, name, description, price, image
                            FROM products 
                    ";
                    MySqlDataReader reader = sql.ExecuteReader();

                    while (reader.Read())
                    {
                        Products product = new Products()
                        {
                            Id = (UInt64)reader["id"],
                            Name = (string)reader["name"],
                            Description = (string)reader["description"],
                            Price = (int)reader["price"],
                            Imgpizza = (string)reader["image"],
                        };
                        pizzas.Add(product);
                    }
                    methodResult = OK;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(GetPizzas));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }
    }
}
