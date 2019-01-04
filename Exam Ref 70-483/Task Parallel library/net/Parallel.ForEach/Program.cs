using System;

namespace Parallel.ForEach
{
    class Program
    {
        static void Main(string[] args)
        {
            var items = System.Linq.Enumerable.Range(0, 500);
            System.Threading.Tasks.Parallel.ForEach(items, item => WorkOnItem(item));
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
