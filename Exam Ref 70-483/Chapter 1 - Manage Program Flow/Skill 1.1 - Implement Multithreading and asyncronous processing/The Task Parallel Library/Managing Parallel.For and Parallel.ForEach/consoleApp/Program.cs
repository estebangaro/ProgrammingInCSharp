using System;

namespace consoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            UseStopMethodOnForEach();
        }

        static void UseBreakMethodOnFor(){
            //para implementar.
        }

        static void UseStopMethodOnForEach(){
            //System.Collections.IEnumerable RangeInterations = System.Linq.Enumerable.Range(0, 500);
            int StartIteration = 1, EndIteration = 500;

            var ParallelResult = System.Threading.Tasks.Parallel.For(StartIteration, 
                EndIteration, new System.Threading.Tasks.ParallelOptions{MaxDegreeOfParallelism=3},
                (indexIteration, loopState) => {
                int ManagedThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                Console.WriteLine($"Hilo # {ManagedThreadId} - Inciando ejecución de iteración #" + indexIteration);
                    if(loopState.ShouldExitCurrentIteration){
                        WriteLoopStateState(loopState, "Inicio", indexIteration);
                    }
                    if(indexIteration == 49){
                        WriteLoopStateState(loopState, "Trigger Detención", indexIteration);
                        loopState.Stop();
                    }
                    System.Threading.Thread.Sleep(1000);
                    if(loopState.ShouldExitCurrentIteration){
                        WriteLoopStateState(loopState, "Fin", indexIteration);
                    }

                Console.WriteLine($"Hilo # {ManagedThreadId} - Finalizando ejecución de iteración #" + indexIteration);
                });

            Console.WriteLine("Finalizando ejecución de método For con ParallelLoopState");
            Console.WriteLine($"Resultado: \"IsCompleted\" : \"{ParallelResult.IsCompleted}\"");
            if(ParallelResult.LowestBreakIteration.HasValue){
            Console.WriteLine($"Resultado: \"LowestBreakIteration\" : \"{ParallelResult.LowestBreakIteration}\"");
            }
            else{
                Console.WriteLine($"Resultado: \"LowestBreakIteration\" : \"NULL\"");}
            }

        static void WriteLoopStateState(System.Threading.Tasks.ParallelLoopState loopState,
            string phase, int indexIteration){
            int ManagedThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            string  Hilo = $"Hilo #{ManagedThreadId} - Fase {phase} - Iteración #{indexIteration}: ";
            Console.WriteLine($"{Hilo}{loopState.GetInfo()}");
        }
    }

    static class ParallelLoopStateExtensions{
        public static string GetInfo(this System.Threading.Tasks.ParallelLoopState loopState){
            return string.Concat($"\"IsExceptional\" : \"{loopState.IsExceptional}\"{Environment.NewLine}",
                $"\"IsStopped\" : \"{loopState.IsStopped}\"{Environment.NewLine}",
                $"\"LowestBreakIteration\" : \"{loopState.LowestBreakIteration}\"{Environment.NewLine}",
                $"\"ShouldExitCurrentIteration\" : \"{loopState.ShouldExitCurrentIteration}\"{Environment.NewLine}");
        }
    }
}
