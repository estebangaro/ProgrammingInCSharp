using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallel.For
{
    class Program
    {
        static void Main(string[] args)
        {
            var items = System.Linq.Enumerable.Range(0, 500);
            System.Threading.Tasks.Parallel.For(0, items.Count(), index => WorkOnItem(items.ElementAt(index)));
            Console.WriteLine("Finish procesing. Press any key to end.");
            Console.ReadKey();
        }

        static void WorkOnItem(object item)
        {
            Console.WriteLine("Starting working on: " + item);

            System.Threading.Thread.Sleep(100);

            Console.WriteLine("Finished working on: " + item);
        }
    }
}
