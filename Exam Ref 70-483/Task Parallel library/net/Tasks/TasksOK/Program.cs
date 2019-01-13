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

            //CreateTasksAndUseWaitAnyMethod();

            //CreateTasksAndUseContinueWith();

            //CreateTasksGenericAndUseContinueWith();

            //UseTasksWithContinueWithMethodTwoTwice();

            CreateChildrenTasks();

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

        private static void CreateTasksAndUseContinueWith()
        {
            var AntecedentTask = Task.Run(new Action(TaskHello));

            var ContinuationTask = AntecedentTask.ContinueWith(antecedentTask => TaskWorld());

            ContinuationTask.Wait();

            Console.WriteLine("Finalizando ejecución de tarea de continuación...");
        }

        private static void CreateTasksGenericAndUseContinueWith()
        {
            var AntecedentTask = Task.Run<string>(delegate () { TaskHello(); return "Hello "; });

            var ContinuationTask = AntecedentTask.ContinueWith(antecedentTask => { Console.WriteLine("|Texto impreso en antecedente: " + antecedentTask.Result); TaskWorld(); });

            ContinuationTask.Wait();

            Console.WriteLine("Finalizando ejecución de tarea de continuación...");
        }

        private static void DoChild(object taskNumber)
        {
            Console.WriteLine("Start runing child Task #" + ((int)taskNumber + 1));
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("Task finishing #" + ((int)taskNumber + 1));
        }

        private static void CreateChildrenTasks()
        {
            Task ParentTask = Task.Factory.StartNew(delegate
            {
                Console.WriteLine("Iniciando ejecución de tarea padre");

                for (int i = 0; i < 10; i++)
                {
                    Task.Factory.StartNew(DoChild, i, TaskCreationOptions.AttachedToParent);
                }

                Console.WriteLine("Finalizando creación de tareas hijas atachadas");
            }, TaskCreationOptions.DenyChildAttach);

            ParentTask.Wait();
            Console.WriteLine("Finalizando ejecución de tarea padre");
        }

        private static void TaskHello()
        {
            System.Threading.Thread.Sleep(1000);
            Console.Write("Hello ");

            throw new Exception("caca");
        }

        private static void TaskWorld()
        {
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("World");
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

        private static void UseTasksWithContinueWithMethodTwoTwice()
        {
            Task AntecedentTask = Task.Run(new Action(TaskHello));

            Task ContinuationOk = AntecedentTask.ContinueWith(antecedentTask => TaskWorld(), TaskContinuationOptions.OnlyOnRanToCompletion);
            Task ContinuationException = AntecedentTask.ContinueWith(antecedentTask => ExceptionHandler(antecedentTask), TaskContinuationOptions.OnlyOnFaulted);

            Task.Run(delegate
            {
                System.Threading.Thread.Sleep(5000);
                if (ContinuationOk.Status == TaskStatus.RanToCompletion)
                {
                    Console.WriteLine("Se ha ejecutado la tarea de continuación de éxito.");
                }
                else
                {
                    Console.WriteLine("Se ha ejecutado la tarea de continuación de manejo de excepción.");
                }
            });
        }

        private static void ExceptionHandler(Task antecedentTask)
        {
            Console.WriteLine($"Estatus de tarea antecedente: {antecedentTask.Status}");
        }
    }
}
