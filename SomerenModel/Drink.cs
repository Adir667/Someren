using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenModel
{
    public class Drink
    { 
        public string Name { get; set; }

        public int Number { get; set; }

        public int Price { get; set; }

        public int Stock { get; set; }

        public int Sold { get; set; }

        public bool Alcoholic { get; set; } // (VAT can be calculated subsequently)
    }
}
