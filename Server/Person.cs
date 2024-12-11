using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Person
    {
        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public int? Balance { get; set; }

        public override string ToString()
        {
            return $"UserName: {UserName}, Email: {Email}, Password: {Password}, Balance {Balance}";
        }

    }
}
