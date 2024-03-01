using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager
{
    internal class User
    {

        public static string? name;
        public static string? password;
        
        public User(string name, string password)
        {
            User.name = name;
            User.password = password;
        }

    }
}
