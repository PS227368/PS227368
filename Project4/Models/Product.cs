using Google.Protobuf.WellKnownTypes;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace Project4.Models
{
    public class Products
    {
        private UInt64 id;

        public UInt64 Id
        {
            get { return id; }
            set { id = value; }
        }
        private string name = null!;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private int price;

        public int Price
        {
            get { return price; }
            set { price = value; }
        }
        private string description = null!;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        private string imgpizza = null!;

        public string Imgpizza
        {
            get { return imgpizza; }
            set { imgpizza = value; }
        }
    }
}
