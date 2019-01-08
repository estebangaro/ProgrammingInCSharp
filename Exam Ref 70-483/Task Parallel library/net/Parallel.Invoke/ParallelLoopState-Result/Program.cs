using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelLoopState_Result
{
    class Program
    {
        static void Main(string[] args)
        {
            var items = Enumerable.Range(0, 500);
            ParallelLoopResult result;
            int countOk = 0, countStop = 0, countTotal = 0;

            using (var FileStream = File.OpenWrite("break_loop.txt"))
            {
                using (StreamWriter Sw = new StreamWriter(FileStream))
                {
                    result = Parallel.For(0, items.Count(),
                        (int index, ParallelLoopState ParallelLoopState) =>
                    {
                        if (index == 200)
                        {
                            ParallelLoopState.Stop();
                        }
                        bool IsStopped = ParallelLoopState.IsStopped;
                        if (!IsStopped)
                        {
                            Interlocked.Increment(ref countOk);
                            WorkOnItem(index, Sw);
                        }
                        else
                        {
                            Interlocked.Increment(ref countStop);
                        }

                        Interlocked.Increment(ref countTotal);

                        Console.WriteLine($"Valor de IsStopped: {IsStopped}, elemento: {index}");
                    });
                }
            }

            Console.WriteLine("Completed: " + result.IsCompleted);
            Console.WriteLine("Items: " + result.LowestBreakIteration);
            Console.WriteLine("Finish procesing. Press any key to end.");
            Console.WriteLine("Número de elementos procesados exitosamente: " + countOk);
            Console.WriteLine("Número de elementos sin procesar: " + countStop);
            Console.WriteLine("Número de elementos totales: " + countTotal);
            Console.ReadKey();
        }

        static void WorkOnItem(object item, StreamWriter Console)
        {
            Console.WriteLine("Starting working on: " + item);
            System.Console.WriteLine("Starting working on: " + item);

            System.Threading.Thread.Sleep(100);

            Console.WriteLine("Finished working on: " + item);
            System.Console.WriteLine("Finished working on: " + item);
        }
    }
}
