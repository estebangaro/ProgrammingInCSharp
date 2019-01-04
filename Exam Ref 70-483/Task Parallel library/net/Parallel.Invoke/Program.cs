using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallel.Invoke
{
    class Program //Al no especificar modificador de acceso, se establece el modificador default "internal"
    {
        static void Main(string[] args)
        {
            //System.Threading.Tasks.Parallel.Invoke(Task1, Task2);
            System.Threading.Tasks.Parallel.Invoke(delegate { Task(1, 2000); }, () => Task(2, 1000));
            Console.WriteLine("Finish procesing. Press any key to end.");
            Console.ReadKey();
        }

        static void Task1() //Al no especificar modificador de acceso, se establece el modificador default "private"
        {
            Console.WriteLine("Task 1 starting");

            System.Threading.Thread.Sleep(2000);

            Console.WriteLine("Task 1 ending");
        }

        static void Task2()
        {
            Console.WriteLine("Task 2 starting");

            System.Threading.Thread.Sleep(1000);

            Console.WriteLine("Task 2 ending");
        }

        static void Task(short taskNumber, int millisecondsTimeout)
        {
            Console.WriteLine($"Task {taskNumber} starting");

            System.Threading.Thread.Sleep(millisecondsTimeout);

            Console.WriteLine($"Task {taskNumber} ending");
        }
    }
}
