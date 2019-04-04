using System;

namespace Tasks.Parallel
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Parallel Class!");
            //UseInvokeMethod();
            //UseForEeachMethod();
            UseForMethod();
        }

        static void UseForEeachMethod(){
            Console.WriteLine("Iniciando ejecución de ForEach - Method");

            System.Collections.Generic.IEnumerable<int> IntegersCollection = 
                System.Linq.Enumerable.Range(1,500);
            var Result = System.Threading.Tasks.Parallel.ForEach(IntegersCollection, delegate(int integer){
                Console.WriteLine("Trabajando en entero " + integer);
                System.Threading.Thread.Sleep(integer * 2);
                if(integer == 126){
                    Console.WriteLine("Por generar excepción");
                    throw new NotSupportedException("Excepción en entero 25");
                }
                Console.WriteLine("Finalizando con entero " + integer);
            });
            Console.WriteLine("Estado de cliclo ForEach Paralelizado: " + Result.IsCompleted);
        }

        static void UseForMethod(){
            Console.WriteLine("Iniciando ejecución de For - Method"); 
            //Se define rango de iteración (Inicio/fin en iteraciones de 1).
            int FromInclusive = 1, ToExclusive = 26;
            var Result = System.Threading.Tasks.Parallel.For(FromInclusive, ToExclusive, 
                loopIndex => {
                    Console.WriteLine("Inciando ejecución de índice de iteración " + loopIndex);
                    System.Threading.Thread.Sleep(loopIndex * 5);
                    Console.WriteLine("Finalizando ejecución de índice de iteración " + loopIndex);
                });
            Console.WriteLine("Estado de cliclo For Paralelizado: " + Result.IsCompleted);
        }

        static void UseForAndForEachWithParallelOptions(){
            Console.WriteLine("Iniciando ejecución de For - Method"); 
        }

        static void UseInvokeMethod(){
            Console.WriteLine("Iniciando ejecución de InvokeMethod - NamedMethod");

            System.Threading.Tasks.Parallel.Invoke(new System.Threading.Tasks.ParallelOptions{
                MaxDegreeOfParallelism = 2
            }, 
            ActionMethod, new Action(
                delegate{
                    int CurrentManageId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                    System.Threading.Thread.Sleep(9000);
                    Console.WriteLine($"{CurrentManageId} - Executing Anonimous Method - delegate anonimous!!");
                }
            ), () => {
                    int CurrentManageId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                    Console.WriteLine($"{CurrentManageId} - Executing Anonimous Method - lambda expression!!");
                    System.Threading.Thread.Sleep(1000);
                    throw new NotSupportedException("Excepción en operación!");
                });

            Console.WriteLine("Finalizando ejecución de InvokeMethod");
        }


        static void ActionMethod(){
            int CurrentManageId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine($"{CurrentManageId} - Executing ActionMethod!!");
        }
    }
}
