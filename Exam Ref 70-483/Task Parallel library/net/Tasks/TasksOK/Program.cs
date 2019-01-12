using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksOK
{
    class Program
    {
        public static void Main(string[] args)
        {
            //CreateTasksWithConstructor();
            //CreateTasksWithRunMethod();
            //CreateTasksGenericsWithRunMethod();

            //Console.WriteLine("Inciando ejecución de versión Rob");
            //CreateTasksAndUseWaitAllMethod();
            //Console.WriteLine("Inciando ejecución de versión 2");
            //CreateTasksAndUseWaitAllMethodV2();
            //Console.WriteLine("Inciando ejecución de versión 3");
            //CreateTasksAndUseWaitAllMethodV3();

            CreateTasksAndUseWaitAnyMethod();

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

        private static void DoWork(object numberOfTask)
        {
            Console.WriteLine($"Task {numberOfTask} starting");
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine($"Task {numberOfTask} finished");
        }

        private static void CreateTasksWithConstructor()
        {
            Task newTask = new System.Threading.Tasks.Task(DoWork);
            newTask.Start();
            newTask.Wait();
        }


        private static void CreateTasksWithRunMethod()
        {
            Task newTask = Task.Run(new Action(DoWork));
            newTask.Wait();
        }

        private static int CalculateResult()
        {
            Console.WriteLine("Work Starting");
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine("Work Finished");

            return 2509;
        }

        private static void CreateTasksGenericsWithRunMethod()
        {
            Task<int> newTask = Task.Run<int>(new Func<int>(CalculateResult));
            Console.WriteLine("Result: " + newTask.Result);
        }

        private static void CreateTasksAndUseWaitAllMethod()
        {
            Task[] Tasks = new Task[10];

            for (int i = 0; i < 10; i++)
            {
                int TaskNumber = i;

                Tasks[i] = Task.Run(() => DoWork(i));
            }

            Task.WaitAll(Tasks);
        }

        private static void CreateTasksAndUseWaitAnyMethod()
        {
            //Comentarios: Analogía de carrera de caballos :).
            Task[] Tasks = new Task[10];

            for (int i = 0; i < 10; i++)
            {
                int TaskNumber = i;

                Tasks[i] = Task.Run(() => DoWork(TaskNumber));
            }

            int FirstTaskCompleted = Task.WaitAny(Tasks);
            Console.WriteLine("Al menos 1 tarea se ha completado; Tarea #" + FirstTaskCompleted);
        }

        private static void CreateTasksAndUseWaitAllMethodV2()
        {
            Task[] Tasks = new Task[10];

            for (int i = 0; i < 10; i++)
            {
                int TaskNumber = i;

                Tasks[i] = Task.Factory.StartNew(tasknumber => DoWork(tasknumber), i);
            }

            Task.WaitAll(Tasks);
        }

        private static void CreateTasksAndUseWaitAllMethodV3()
        {
            Action[] Tasks = new Action[10];

            for (int i = 0; i < 10; i++)
            {
                int TaskNumber = i;

                Tasks[i] = () => DoWork(TaskNumber);
            }

            Parallel.Invoke(Tasks);
        }
    }
}
