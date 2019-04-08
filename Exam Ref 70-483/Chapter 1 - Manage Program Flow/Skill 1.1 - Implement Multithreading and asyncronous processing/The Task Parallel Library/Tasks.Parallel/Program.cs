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
            //UseForMethod();
            UseForAndForEachWithParallelOptions();
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
                    int ManageThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                    Console.WriteLine("Inciando ejecución de índice de iteración " + loopIndex + $", en hilo {ManageThreadId}");
                    System.Threading.Thread.Sleep(loopIndex * 5);
                    Console.WriteLine("Finalizando ejecución de índice de iteración " + loopIndex  + $", en hilo {ManageThreadId}");
                });
            Console.WriteLine("Estado de cliclo For Paralelizado: " + Result.IsCompleted);
        }

        static void ManagingForAndForEachMethods(){
            
        }

        static void UseForAndForEachWithParallelOptions(){
            
            /*Console.WriteLine("Iniciando ejecución de For Method - PO MaxDegreeOfParallelism");
            var ResultFor = System.Threading.Tasks.Parallel.For(1, 51, new System.Threading.Tasks.ParallelOptions{
                MaxDegreeOfParallelism = 3
            }, (loopIndex, loopState) => {
                int ManageThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                Console.WriteLine("Inciando ejecución de índice de iteración " + loopIndex + $", en hilo {ManageThreadId}");
                System.Threading.Thread.Sleep(loopIndex * 5);
                Console.WriteLine("Finalizando ejecución de índice de iteración " + loopIndex + $", en hilo {ManageThreadId}");
            });
            Console.WriteLine("Estado de cliclo For Paralelizado: " + ResultFor.IsCompleted);*/

            System.Threading.CancellationTokenSource Cts = new System.Threading.CancellationTokenSource();
            System.Threading.CancellationToken Ct = Cts.Token;
            try{
                var ResultForEach = System.Threading.Tasks.Parallel.ForEach(System.Linq.Enumerable.Range(1, 50), 
                    new System.Threading.Tasks.ParallelOptions{
                        CancellationToken = Ct
                    }, integer => {
                        if(Ct.IsCancellationRequested){
                            Console.WriteLine("Iniciando ejeución con solicitud de cancelación");
                        }
                        int ManageThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                        Console.WriteLine(string.Concat( "Trabajando en entero ", integer, $", en hilo {ManageThreadId}"));
                System.Threading.Thread.Sleep(integer * 10);
                if(integer == 16){
                    Console.WriteLine("Por solicitar cancelación");
                    Cts.Cancel();
                }
                //Ct.ThrowIfCancellationRequested();
                if(!Ct.IsCancellationRequested){
                Console.WriteLine(string.Concat("Finalizando con entero ", integer, $", en hilo {ManageThreadId}"));
                }else{
                    Console.WriteLine(string.Concat("Finalizando con entero ", integer, $", en hilo {ManageThreadId}",
                    " con manejo de solicitud de cancelación"));
                }
                    });
            }catch(AggregateException ae){
                Console.WriteLine("Iniciando control de excepciones acumuladas");
                foreach (Exception item in ae.InnerExceptions)
                {
                    Console.WriteLine(item);
                }
            }catch(OperationCanceledException ex){
                Console.WriteLine("Iniciando control de excepción " + ex.Message);
            }
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
