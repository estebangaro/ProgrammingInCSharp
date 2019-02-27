using System;
using System.Linq;

namespace AsParallel
{
    class Program
    {
        class Person{
            public string Name { get; set; }
            public int Age { get; set; }
            public string City { get; set; }
            public DateTime DateOfBirth { get; set; }
        }

        static void Main(string[] args)
        {
            /*Observaciones:
            1) El método AsParallel de System.Linq.ParallelEnumerable, retorna una instancia del tipo
                System.Linq.ParallelQuery / System.Linq.ParallelQuery<TSource>.
            2) El método AsParallel tiene 3 sobrecargas (IEnumerable, IEnumerable<TSource> y Partiotioner<TSource>).
            3) El método AsParallel es el encargado de analizar si la ejecución paralelizada de los elementos
                de una consulta (los operadores) acelerarán la ejecución de la misma.
            4) System.Linq.ParallelEnumerable, implementa los operadores de consulta estandar de LINQ para el tipo
                ParallelQuery<TSource> / ParallelQuery.
            5) System.Linq.ParallelQuery, cuenta con diversos métodos (operadores) que nos permiten proporcionar
                información adicional al proceso de paralelización;
                5.1) WithCancellation, permite especificar un token de cancelación System.Threading.CancellationToken,
                    para poder cancelar la ejecución de una consulta parelelizada que suponga una tarea de larga duración.
                5.2) WithDegreeOfParallelims,  permite especificar el número máximo de Threads, disponibles
                    para ejecutar los procesos en los que es rota la consulta paralelizada.
                5.3) WithExecutionMode, permite especificar O NO la evaluación de la estructura y forma de la consulta
                    para la implementación del paralelismo (en otras palabras forzar la ejecución paralelizada).
                5.4) WithMergeOptions, permite especificar el modo en el que serán amortiguados los resultados
                    de salida (AutoBuffered, Default, FullyBuffered, NotBuffered), aunque estos valores son
                    considerados meramente como sugerencias de fusión.
             */
            Console.WriteLine("Iniciando demo de PLINQ!");
            Person[] Persons = new Person[]{
                new Person { Name = "Ámbar", Age = 24, City = "México", DateOfBirth = new DateTime(1994, 12, 24)},
                new Person { Name = "Esteban", Age = 27, City = "México", DateOfBirth = new DateTime(1991, 9, 25)},
                new Person { Name = "Bárbara", Age = 0, City = "México", DateOfBirth = new DateTime(2019, 9, 11)},
                new Person { Name = "Arturo", Age = 0, City = "México", DateOfBirth = new DateTime(2019, 9, 12)},
                new Person { Name = "Rocio", Age = 47, City = "México", DateOfBirth = new DateTime(1981, 8, 11)}
            };

            var PersonsOfMexico = from person in Persons.AsParallel()
                                    where person.City == "México"
                                    select person.Name;
            
            Console.WriteLine("Personas viviendo en México:");
            foreach (var PersonName in PersonsOfMexico)
            {
                Console.WriteLine(PersonName);
            }

            Console.WriteLine("Finalizando demo, de método AsParallel...");
            Console.ReadKey();
        }
    }
}
