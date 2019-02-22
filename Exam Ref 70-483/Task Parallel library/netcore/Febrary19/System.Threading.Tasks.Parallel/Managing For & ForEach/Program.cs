using System;

namespace Managing_For___ForEach
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Observaciones:
            1)*Concluir verificación de funcionamiento de métodos Stop / Break.
            2)*Concluir la verificación de las 2 propiedades de ParallelLoopResult 
                que coincidan con la invocación de los métodos Stop / Break.
            3)*Concluir verificación de uso de valor de propiedad IsExceptional, pues el comportamiento
                obtenido durante los ejercicios previos, es que las iteraciones cesan una vez
                que se genera 1 o más excepciones.
            4)*Considerar la revisión de las sobrecargas de For / ForEach (debido a la documentación,
                en ParallelLoopOptions, ParallelLoopResult y ParallelLoopState).
             */
            System.Threading.Tasks.ParallelOptions options = new System.Threading.Tasks.ParallelOptions();
            options.MaxDegreeOfParallelism = 3;
            Console.WriteLine("Iniciando demo de administración de ciclos paralelos.");
            System.Threading.Tasks.ParallelLoopResult Result = System.Threading.Tasks.Parallel.For(0, 500, 
             options,
             delegate(int index, 
                System.Threading.Tasks.ParallelLoopState parallelLoopState){
                    if(index == 199){
                        parallelLoopState.Stop();
                    }
                    WorkOnItem(index, parallelLoopState);
                });
            Console.WriteLine("Finalizando ejecución de \"demo de administración de ciclos paralelos\".");
            Console.WriteLine($"Estado de ParallelLoopResult: IsCompleted = {Result.IsCompleted}, "
                + $"LowestBreakIteration = {Result.LowestBreakIteration}");
        }

        static void WorkOnItem(int item, 
            System.Threading.Tasks.ParallelLoopState parallelLoopState){
            
            Console.WriteLine("Iniciando trabajo en " + item);
            System.Threading.Thread.Sleep(300);
            if(!parallelLoopState.IsStopped){
                Console.WriteLine("Finalizando trabajo en " + item);
            }else{
                Console.WriteLine($"Finalizando por solicitud de cancelación, en item#{item}");
                Console.WriteLine($"Valor de ShouldExitCurrentIteration:{parallelLoopState.ShouldExitCurrentIteration}; item#{item}");
            }
        }
    }
}
