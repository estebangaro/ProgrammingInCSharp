using System;

namespace For
{
    class Program
    {
        static bool Es499;
        static void Main(string[] args)
        {
            /*Observaciones:
            1) El método For de Parallel, carece de las secciones de evaluación de condición de continuación
                así como de la sección incrementos sobre la variable de control a diferencia de la estructura
                tradicional.
            2) Se compone de 12 sobrecargas, entre las cuales se puede hacer uso de objetos como ParallelLoopState,
                ParallelLoopResult y delegados para generación de valores ThreadLocal.
            3) Al igual que sus hermana ForEach, no concluye su ejecución, hasta que todos los elementos
                hayan sido procesados ó 1 o más procesos generen excepciones no controladas.
             */
             Es499 = false;
            Console.WriteLine("Inciando demostración de método For");
            try{
            System.Threading.Tasks.ParallelLoopResult ResultState 
                = System.Threading.Tasks.Parallel.For(0, 500, WorkOnItem);
                }catch(AggregateException ae){

                }
            Console.WriteLine("Finalizando la ejecución del método For, presione una tecla para continuar");
            Console.WriteLine("Se proceso elemento 499? " + Es499);
            Console.ReadKey();
        }

        static void WorkOnItem(int item){
            if(item == 499){
                Es499 = true;
            }
            Console.WriteLine("Iniciando trabajo en " + item);
            System.Threading.Thread.Sleep(100);
            if(item == 325){
                throw new Exception("Excepción en item " + item);
            }
            Console.WriteLine("Finalizando trabajo en " + item);
        }
    }
}
