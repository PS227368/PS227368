using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project4.Models
{
    public abstract class LoadingImage
    {
        private string imgpizza = null!;

        public string Imgpizza
        {
            get { return imgpizza; }
            set { imgpizza = value; }
        }
    }
}
