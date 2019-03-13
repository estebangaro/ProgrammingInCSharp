using System;

namespace Tasks
{
    class Program
    {
        static void Main(string[] args)
        {
            UsingTaskRun();
        }

        static void DoWork(){
            Console.WriteLine("Do Working...");
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("Finishing working...");
        }

        static void UsingTaskConstructor(){
            Console.WriteLine("Iniciando ejecución de método UsingTaskConstructor");
            var Task = new System.Threading.Tasks.Task(new Action(DoWork));//Creación de Task.
            Task.Start();//Inicialización de ejecución.
            Task.Wait();//Esperar a que tarea concluya.
            Console.WriteLine("Finalizando ejecución de método UsingTaskConstructor");
        }

        static void UsingTaskRun(){
            Console.WriteLine("Iniciando ejecución de método UsingTaskRun");
            var Task = System.Threading.Tasks.Task.Run(new Action(DoWork));//Creación e inicialización de Task.
            Task.Wait();//Esperar a que tarea concluya.
            Console.WriteLine("Finalizando ejecución de método UsingTaskRun");
        }
        
    }
}
