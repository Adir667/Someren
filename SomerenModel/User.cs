using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenModel
{
    public class User
    {
        public int Number { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public string Salt { get; set; }

    }
}
