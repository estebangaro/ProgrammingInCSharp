using System;

namespace Thread.Pool
{
    class Program
    {
        static void DoThread(){
            Console.WriteLine("Hello from Thread!!");
            System.Threading.Thread.Sleep(5000);
            Console.WriteLine($"Finalizando ejecución de Thread!!");
            Console.WriteLine($"Valor de prioridad de Hilo #{System.Threading.Thread.CurrentThread.ManagedThreadId} actual (DoThread): " +
                $"{System.Threading.Thread.CurrentThread.Priority}");
            Console.WriteLine("Es proceso en segundo plano? " + (System.Threading.Thread.CurrentThread.IsBackground? "Si": "No"));
        }

        static void CreateFirstThread(){
System.Threading.Thread MyFirstThread = new System.Threading.Thread(DoThread);
            //System.Threading.Thread MyFirstThread = new System.Threading.Thread(
            //    new System.Threading.ThreadStart(DoThread));
            MyFirstThread.Start();
        }

        static void CreateATask(){
            System.Threading.Tasks.Task Task = 
                System.Threading.Tasks.Task.Run(delegate{
                    Console.WriteLine("Iniciando ejecución de tarea!!!");
                    System.Threading.Thread.Sleep(5000);
                    Console.WriteLine("FInalizando ejecución de tarea!!!");
                });
        }
        static void Main(string[] args)
        {
            //CreateATask();
            CreateFirstThread();
            System.Threading.Thread.Sleep(1500);
            Console.WriteLine("Finalizando ejecución de método MAIN");
            Console.WriteLine($"Valor de prioridad de Hilo #{System.Threading.Thread.CurrentThread.ManagedThreadId} actual (main): " +
                $"{System.Threading.Thread.CurrentThread.Priority}");
            Console.WriteLine("Es proceso en segundo plano? " + (System.Threading.Thread.CurrentThread.IsBackground? "Si": "No"));
        }
    }
}
