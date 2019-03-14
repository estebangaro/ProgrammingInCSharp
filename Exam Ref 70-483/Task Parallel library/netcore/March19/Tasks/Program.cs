using System;

namespace Tasks
{
    class Program
    {
        static void Main(string[] args)
        {
            //UsingTaskRun();
            UsingTaskGeneric();
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

        static void UsingTaskGeneric(){
            Console.WriteLine($"Iniciando ejecución de método UsingTaskGeneric - {nameof(UsingTaskGeneric)}");

            var Task = new System.Threading.Tasks.Task<System.Threading.Tasks.Task<int>>(
                async delegate(){
                    Console.WriteLine("Hilo de delegado anónimo asincrono - antes de await " + System.Threading.Thread.CurrentThread.ManagedThreadId);
                    await System.Threading.Tasks.Task.Delay(3000);
                    Console.WriteLine("Hilo de delegado anónimo asincrono - después de await " + System.Threading.Thread.CurrentThread.ManagedThreadId);
                    return 2509;
                });
            
            Task.Start();
            Console.WriteLine("Antes de Result " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            var IntegerFromTaskGeneric = Task.Result;
            Console.WriteLine("Después de Result " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            var integer = IntegerFromTaskGeneric.Result;
            Console.WriteLine("Número obtenido: " + integer);
            Console.WriteLine($"Finalizando ejecución de método UsingTaskGeneric - {nameof(UsingTaskGeneric)}");
        }
        
    }
}
