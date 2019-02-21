using System;

namespace ForEach
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Observaciones:
            1) El delegado Action especificado, como argumento para la invocación de ForEach,
                no puede especificar un tipo Base (ej.Object VS int) como tipo cerrado del delegado.
            2) El método ForEach cuenta con 20 sobrecargas.
            3) *Queda pendiente el uso de las 19 sobrecargas restantes, hasta la revisión, de
                los objetos ParallelLoopState, ParallelLoopResult y ThreadLocal.
            4) A diferencia del método Invoke, el método ForEach detiene prematuramente la ejecución
                del procesamiento pendiente de elementos de la colección si se desencadena
                1 o más excepciones no controladas.
             */
            Console.WriteLine("Iniciando demostración de método ForEach");
            System.Threading.Tasks.ParallelLoopResult ResultState 
                = System.Threading.Tasks.Parallel.ForEach(System.Linq.Enumerable.Range(0, 500), 
                WorkOnItem);
            Console.WriteLine("Finalizando proceso, presione una tecla para continuar");
            Console.ReadKey();
        }

        static void WorkOnItem(int item){
            try{
            Console.WriteLine("Iniciando trabajo en " + item);
            System.Threading.Thread.Sleep(100);
            if(item == 110){
                throw new Exception("Excepción en item " + 100);
            }
            Console.WriteLine("Finalizando trabajo en " + item);}
            catch{
                Console.WriteLine("Manejando excepción en item: " + 100);
            }
        }
    }
}
