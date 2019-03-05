using System;

namespace Managing_For___ForEach
{
    class Program
    {
        static System.IO.StreamWriter Sw;
        static object obj;
        static int NumberOfProcessed;

        static void Main(string[] args)
        {
            /*Observaciones:
            1) La invocación al método Stop de ParallelLoopState, previene la ejecución de las iteraciones
                pendientes por ejecutarse, sin importar que la iteración sea mayor o menor a la iteración
                de ruptura más baja (LowestBreakIteration). NOTA: Al invocar a este método se establece
                el valor de IsStopped y ShouldExitCurrentIteration en TRUE.
            2) La invocación al método Break de ParallelLoopState, previene la ejecución de iteraciones
                pendientes por ejecutarse, si y solo si son iteraciones mayores a la iteración de ruptura
                mas baja (LowestBreakIteration), asegurando que las iteraciones pendientes por ejecutar
                menores se ejecutarán (siempre y cuando no existan excepciones no controladas). NOTA:
                Al invocar a este método se establece la propiedad de LowestBreakIteration y ShouldExitCurrentIteration.
            3) ParallelLoopResult, se conforma de 2 propiedades; IsCompleted es "false", si no se terminaron
                de ejecutar todas las iteraciones por alguna solicitud de ruptura o detención (Break/Stop) y "true"
                si se ejecutaron todos las iteraciones;
                mientras que LowestBreakIteration es distinto a NULL, si se solicitó la detención
                prematura de las iteraciones mediante el método Break.
            4) Se puede invocar al meétodo Break desde distintas iteraciones, tomando el número de iteración
                mas baja como valor de LowestBreakIteration.
            5) La propiedad IsExceptional de ParallelLoopState se establece en TRUE, cuando 1 o más iteraciones generán excepciones no controladas,
                siendo de utilidad únicamente para las iteraciones que se encuentren en ejecución al momento
                de generarse la excepción no controlada, pues el resto de iteraciones (pendientes por ejecutar)
                no podrán ejecutarse debido a que se interrumpe la ejecución de iteraciones para reelanzar la
                excepción al invocador del ciclo en paralleo (For/ForEach). NOTA: Comportamiento similar a
                Stop.
            6) Se crea una instancía ParallelLoopState por Ciclo (por invocación de método For / ForEach),
                la cual es utilizada por todas las iteraciones del ciclo en paralelo.
            7)*Considerar la revisión de las sobrecargas de For / ForEach (debido a la documentación,
                en ParallelLoopOptions, ParallelLoopResult y ParallelLoopState).
             */
            Console.WriteLine("Iniciando demo de administración de ciclos paralelos.");
            obj = new object();
            NumberOfProcessed = 0;
            using(Sw = new System.IO.StreamWriter(System.IO.File.OpenWrite("resultsBreak2.txt"))){
            System.Threading.Tasks.ParallelOptions options = new System.Threading.Tasks.ParallelOptions();
            options.MaxDegreeOfParallelism = 3;
            WriteLine("Iniciando demo de administración de ciclos paralelos.");

            System.Threading.Tasks.ParallelLoopResult Result;
            try{
            Result = System.Threading.Tasks.Parallel.For(0, 500, 
             options,
             delegate(int index, 
                System.Threading.Tasks.ParallelLoopState parallelLoopState){
                    if(index == 197){
                        //parallelLoopState.Stop();
                        parallelLoopState.Break();
                        //throw new Exception("error");
                    }
                    WorkOnItemBreak(index, parallelLoopState);
                });
            WriteLine("Finalizando ejecución de \"demo de administración de ciclos paralelos\".");
            WriteLine($"Estado de ParallelLoopResult: IsCompleted = {Result.IsCompleted}, "
                + $"LowestBreakIteration = {Result.LowestBreakIteration}");
            WriteLine("Número de iteraciones procesadas: " + NumberOfProcessed);
            }catch(AggregateException){
                Console.WriteLine("Finalizando ejecución de \"demo de administración de ciclos paralelos\" por control de excepción.");
                Console.WriteLine("Número de iteraciones procesadas: " + NumberOfProcessed);
            }
        }
        }

        static void WorkOnItemExceptional(int item, 
            System.Threading.Tasks.ParallelLoopState parallelLoopState){
            lock(obj){
                NumberOfProcessed++;
            }
            Console.WriteLine("Iniciando trabajo en " + item);
            System.Threading.Thread.Sleep(300);
            if(!parallelLoopState.IsExceptional){
                Console.WriteLine("Finalizando trabajo en " + item);
            }else{
                Console.WriteLine($"Finalizando por generación de execpción no controlada, en item#{item}");
                System.Threading.Thread.Sleep(5000);
                Console.WriteLine($"Valor de ShouldExitCurrentIteration:{parallelLoopState.ShouldExitCurrentIteration}; item#{item}");
            }
            }

        static void WorkOnItemStop(int item, 
            System.Threading.Tasks.ParallelLoopState parallelLoopState){
            lock(obj){
                NumberOfProcessed++;
            }
            WriteLine("Iniciando trabajo en " + item);
            System.Threading.Thread.Sleep(300);
            if(!parallelLoopState.IsStopped){
                WriteLine("Finalizando trabajo en " + item);
            }else{
                WriteLine($"Finalizando por solicitud de cancelación, en item#{item}");
                WriteLine($"Valor de ShouldExitCurrentIteration:{parallelLoopState.ShouldExitCurrentIteration}; item#{item}");
            }
        }

        static void WorkOnItemBreak(int item, 
            System.Threading.Tasks.ParallelLoopState parallelLoopState){
            lock(obj){
                NumberOfProcessed++;
            }
            WriteLine("Iniciando trabajo en " + item);
            System.Threading.Thread.Sleep(300);
            if(!parallelLoopState.LowestBreakIteration.HasValue){
                WriteLine("Finalizando trabajo en " + item);
            }else if(parallelLoopState.LowestBreakIteration.Value < item){
                WriteLine($"Finalizando por solicitud de ruptura, en item#{item}");
                WriteLine($"Valor de ShouldExitCurrentIteration:{parallelLoopState.ShouldExitCurrentIteration}; item#{item}");
            }else{
                if(item != 162){
                WriteLine("Finalizando trabajo en " + item + ", después de invocar a Break");
                }else{
                    throw new Exception("error");
                }
            }
            }

        static void WriteLine(string stringmessage){
            if(Sw != null){
                lock(Sw){
                    Sw.WriteLine(stringmessage);
                }
            }
        }
    }
}
