using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Task newTask = new System.Threading.Tasks.Task(DoWork);
            newTask.Start();
            newTask.Wait();

            Console.WriteLine("Process finished!");
            Console.WriteLine("Press any key to continue...");
            Console.Read();
        }

        private static void DoWork()
        {
            Console.WriteLine("Work Starting");
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine("Work Finished");
        }
    }
}
