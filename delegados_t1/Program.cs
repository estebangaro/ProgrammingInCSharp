using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace delegados_t1
{
    class Program
    {
        static void Main(string[] args)
        {
            Numbers numbers = new Numbers();
            numbers.GetNumbers(5, Console.WriteLine);
        }
    }
}
