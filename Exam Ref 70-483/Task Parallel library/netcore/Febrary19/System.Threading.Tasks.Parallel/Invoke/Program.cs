using System;

namespace Invoke
{
    class Program
    {
        static System.Threading.CancellationTokenSource Cts;
        static void Main(string[] args)
        {
            /*Observaciones: 
            1) Si un delegado Action genera una excepción esta no afecta la ejecución del resto 
                de delegados (item of work).
            2) Se pueden definir los delegados Action mediante el uso del construtor 
                parametrizado (que recibe el identificador del método con nombre a encapsular),
                delegados anónimos/expresiones lambda en cuyo cuerpo se invoque al método con nombre (si aplica) ó
                colocando únicamente el identificador del método.
            3) La instancia ParallelOptions, nos permite especificar el grado máximo de paralelismo
                (número máximo de Threads que ejecutan una tarea en particular), así como la posibilidad
                de solicitar la cancelación de la ejecución de Invoke, siempre y cuando
                existan 1 o más unidades de trabajo por ejecutarse.
            */
            Console.WriteLine("Iniciando prueba con método Invoke");
            Cts = new System.Threading.CancellationTokenSource();
            System.Threading.CancellationToken Ct = Cts.Token;


            System.Threading.Tasks.Parallel.Invoke(new System.Threading.Tasks.ParallelOptions{
                MaxDegreeOfParallelism = 2,
                CancellationToken = Ct },
                new Action(Task1), new Action(Task2), Task4);
            
            Console.WriteLine("Finalizando ejecución de método Invoke");
            Console.WriteLine("Presione una tecla para continuar...");
            Console.ReadKey();
        }

        static void Task1(){ // Unit of work to be performed
        Console.WriteLine($"Iniciando tarea 1; Hilo #{System.Threading.Thread.CurrentThread.ManagedThreadId}");
        System.Threading.Thread.Sleep(8000);
        //Console.WriteLine("Iniciando solicitud de cancelación...");
        //Cts.Cancel();
        Console.WriteLine("Finalizando tarea 1");
    }

    static void Task2(){ // Unit of work to be performed
        Console.WriteLine($"Iniciando tarea 2; Hilo #{System.Threading.Thread.CurrentThread.ManagedThreadId}");
        System.Threading.Thread.Sleep(15000);
        Console.WriteLine("Finalizando tarea 2");
    }

    static void Task3(){ // Unit of work to be performed
        Console.WriteLine("Iniciando tarea 3");
        System.Threading.Thread.Sleep(2000);
        throw new Exception("Excepción en tarea 3");
    }

    static void Task4(){ // Unit of work to be performed
        Console.WriteLine($"Iniciando tarea 4; Hilo #{System.Threading.Thread.CurrentThread.ManagedThreadId}");
        System.Threading.Thread.Sleep(2000);
        Console.WriteLine("Finalizando tarea 4");
    }
    }
}
