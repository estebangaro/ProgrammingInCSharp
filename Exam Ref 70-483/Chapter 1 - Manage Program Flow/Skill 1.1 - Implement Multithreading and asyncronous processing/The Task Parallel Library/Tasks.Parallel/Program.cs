using System;

namespace Tasks.Parallel
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Parallel Class!");
            //UseInvokeMethod();
            UseForEeachMethod();
        }

        static void UseForEeachMethod(){
            Console.WriteLine("Iniciando ejecución de InvokeMethod - NamedMethod");

            System.Collections.Generic.IEnumerable<int> IntegersCollection = 
                System.Linq.Enumerable.Range(1,500);
            System.Threading.Tasks.Parallel.ForEach(IntegersCollection, delegate(int integer){
                Console.WriteLine("Trabando en entero " + integer);
                System.Threading.Thread.Sleep(integer * 2);
                if(integer == 999){
                    Console.WriteLine("Por generar excepción");
                    throw new NotSupportedException("Excepción en entero 25");
                }
                Console.WriteLine("Finalizando con entero " + integer);
            });
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
