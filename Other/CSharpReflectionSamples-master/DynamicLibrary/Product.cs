using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicLibrary
{
    class Product
    {
        private float _Price;
        public float Price
        {
            get { return _Price; }
            set
            {
                if(_Price != value && PriceChanged != null)
                {
                    _Price = value;
                    PriceChanged(this, EventArgs.Empty);
                }
            }
        }
        private string _Name;
        public string Name
        {
            get { return _Name;}
            set { _Name = value;}
        }
        private void WritePrice()
        {
            Console.WriteLine("Price = {0}",Math.Round(Price).ToString());
        }
        public void WriteName()
        {
            Console.WriteLine("Name = {0}",_Name);
        }
        public event EventHandler PriceChanged;
        public Product()
        {
        }
        public Product(string Name)
        {
            _Name = Name;
        }
    }
}
