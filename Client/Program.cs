using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client("kirill1998fed@yandex.ru", "fkmlbyf123");
            client.RegisterUser();

        }
    }
}
