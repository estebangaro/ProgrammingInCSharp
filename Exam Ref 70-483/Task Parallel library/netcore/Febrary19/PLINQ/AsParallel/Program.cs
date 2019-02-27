using System;
using System.Linq;

namespace AsParallel
{
    class Program
    {
        class Person{

            public Person(){
                System.Threading.Thread.Sleep(100);
            }

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
            6) *Pendiente ejecutar ejemplo con AsSequential.
             */
            Console.WriteLine("Iniciando demo de PLINQ!");
            Person[] Persons = new Person[]{
                new Person { Name = "Ámbar", Age = 24, City = "México", DateOfBirth = new DateTime(1994, 12, 24)},
                new Person { Name = "Esteban", Age = 27, City = "México", DateOfBirth = new DateTime(1991, 9, 25)},
                new Person { Name = "Bárbara", Age = 0, City = "México", DateOfBirth = new DateTime(2019, 9, 11)},
                new Person { Name = "Arturo", Age = 0, City = "México", DateOfBirth = new DateTime(2019, 9, 12)},
                new Person { Name = "Rocio", Age = 47, City = "México", DateOfBirth = new DateTime(1981, 8, 11)},
                new Person { Name = "Jacquita", Age = 13, City = "Panama", DateOfBirth = new DateTime(1981, 8, 11)},
                new Person { Name = "Luis", Age = 23, City = "Nicaragua", DateOfBirth = new DateTime(1981, 8, 11)},
                new Person { Name = "Samantha", Age = 23, City = "E.U.A", DateOfBirth = new DateTime(1981, 8, 11)},
                new Person { Name = "Elias", Age = 47, City = "México", DateOfBirth = new DateTime(1981, 8, 11)},
                new Person { Name = "Pepe", Age = 47, City = "México", DateOfBirth = new DateTime(1981, 8, 11)},
                new Person { Name = "Ámbar2", Age = 24, City = "México", DateOfBirth = new DateTime(1994, 12, 24)},
                new Person { Name = "Esteban2", Age = 27, City = "México", DateOfBirth = new DateTime(1991, 9, 25)},
                new Person { Name = "Bárbara2", Age = 0, City = "México", DateOfBirth = new DateTime(2019, 9, 11)},
                new Person { Name = "Arturo2", Age = 0, City = "México", DateOfBirth = new DateTime(2019, 9, 12)},
                new Person { Name = "Rocio2", Age = 47, City = "México", DateOfBirth = new DateTime(1981, 8, 11)},
                new Person { Name = "Jacquita2", Age = 13, City = "Panama", DateOfBirth = new DateTime(1981, 8, 11)},
                new Person { Name = "Luis2", Age = 23, City = "Nicaragua", DateOfBirth = new DateTime(1981, 8, 11)},
                new Person { Name = "Samantha2", Age = 23, City = "E.U.A", DateOfBirth = new DateTime(1981, 8, 11)},
                new Person { Name = "Elias2", Age = 47, City = "México", DateOfBirth = new DateTime(1981, 8, 11)},
                new Person { Name = "Pepe2", Age = 47, City = "México", DateOfBirth = new DateTime(1981, 8, 11)}
            };

            //UseAsParallel(Persons);
            UseInformingParallelization(Persons);

            Console.WriteLine("Finalizando demo, de método AsParallel...");
            Console.ReadKey();
        }

        static void UseAsParallel(System.Collections.Generic.IEnumerable<Person> Persons){
            var PersonsOfMexico = from person in Persons.AsParallel()
                                    where person.City == "México"
                                    select person.Name;
            
            Console.WriteLine("Personas viviendo en México:");
            foreach (var PersonName in PersonsOfMexico)
            {
                Console.WriteLine(PersonName);
            }
        }

        static void UseInformingParallelization(System.Collections.Generic.IEnumerable<Person> Persons){
            //UseWithDegreeOfParallelism(Persons);
            //UseWithCancellation(Persons);
            //UseWithExecutionMode(Persons);
            //UseAsOrdered(Persons);
            UseLinq(Persons);
        }

        static void UseAsOrdered(System.Collections.Generic.IEnumerable<Person> Persons){
            Console.WriteLine("Iniciando demo de AsOrdered...");

            var PersonsOfMexico = Persons.AsParallel()
                                    .AsOrdered()
                                    .WithDegreeOfParallelism(3)
                                    .Where(person => { ShowThreadCurrentInfo("\"Where city (plinq)\"");
                                        return person.City == "México"; })
                                    .Select(person => person.Name);
            
            Console.WriteLine("Personas viviendo en México:");
            foreach (var PersonName in PersonsOfMexico)
            {
                Console.WriteLine(PersonName);
            }
        }

        static void UseLinq(System.Collections.Generic.IEnumerable<Person> Persons){
            Console.WriteLine("Iniciando demo de LINQ...");

            var PersonsOfMexico = Persons
                                    .Where(person => { ShowThreadCurrentInfo("\"Where city (plinq)\"");
                                        return person.City == "México"; })
                                    .Select(person => person.Name);
            
            Console.WriteLine("Personas viviendo en México:");
            foreach (var PersonName in PersonsOfMexico)
            {
                Console.WriteLine(PersonName);
            }
        }

        static void UseWithDegreeOfParallelism(System.Collections.Generic.IEnumerable<Person> Persons){
            Console.WriteLine("Iniciando demo de WithDegreeOfParallelism...");

            var PersonsOfMexico = Persons.AsParallel()
                                    .WithDegreeOfParallelism(3)
                                    .Where(person => { ShowThreadCurrentInfo("\"Where city (plinq)\"");
                                        return person.City == "México"; })
                                    .Select(person => person.Name);
            
            Console.WriteLine("Personas viviendo en México:");
            foreach (var PersonName in PersonsOfMexico)
            {
                Console.WriteLine(PersonName);
            }
        }

        static void UseWithMergeOptions(System.Collections.Generic.IEnumerable<Person> Persons){
            Console.WriteLine("Iniciando demo de WithMergeOptions(...");

            var PersonsOfMexico = Persons.AsParallel()
                                    .WithDegreeOfParallelism(3)
                                    .WithMergeOptions(System.Linq.ParallelMergeOptions.NotBuffered)
                                    .Where(person => { ShowThreadCurrentInfo("\"Where\"");
                                        return person.City == "México"; })
                                    .Select(person => person.Name);
            
            Console.WriteLine("Personas viviendo en México:");
            foreach (var PersonName in PersonsOfMexico)
            {
                Console.WriteLine(PersonName);
            }
        }

        static void UseWithCancellation(System.Collections.Generic.IEnumerable<Person> Persons){
            Console.WriteLine("Iniciando demo de WithCancellation..");
            System.Threading.CancellationToken Ct;
            System.Threading.CancellationTokenSource Cts = new System.Threading.CancellationTokenSource();
            Ct = Cts.Token;

            System.Threading.Tasks.Task.Run(() => {
                Console.WriteLine("Iniciando tarea canceladora...");
                System.Threading.Thread.Sleep(3000);
                Console.WriteLine("Iniciando solicitud de cancelación");
                Cts.Cancel();
            });

            var PersonsOfMexico = Persons.AsParallel()
                                    .WithDegreeOfParallelism(2)
                                    .WithMergeOptions(System.Linq.ParallelMergeOptions.AutoBuffered)
                                    .WithCancellation(Ct)
                                    .Where(person => { ShowThreadCurrentInfo("\"Where\"");
                                        return person.City == "México"; })
                                    .Select(person => person.Name);
            
            Console.WriteLine("Personas viviendo en México:");
            foreach (var PersonName in PersonsOfMexico)
            {
                Console.WriteLine(PersonName);
            }
        }

        static void UseWithExecutionMode(System.Collections.Generic.IEnumerable<Person> Persons){
            Console.WriteLine("Iniciando demo de WithExecutionMode...");

            var PersonsOfMexico = Persons.AsParallel()
                                    .WithDegreeOfParallelism(3)
                                    .WithMergeOptions(System.Linq.ParallelMergeOptions.AutoBuffered)
                                    .WithExecutionMode(System.Linq.ParallelExecutionMode.Default)
                                    .Where(person => { ShowThreadCurrentInfo("\"Where\"");
                                        return person.City == "México"; })
                                    .Select(person => person.Name);
            
            Console.WriteLine("Personas viviendo en México:");
            foreach (var PersonName in PersonsOfMexico)
            {
                Console.WriteLine(PersonName);
            }
        }

        static void ShowThreadCurrentInfo(string opertor){
            Console.WriteLine($"Executing operator: {opertor} in ManageThreadId: " + 
                $"{System.Threading.Thread.CurrentThread.ManagedThreadId}");
            //System.Threading.Thread.Sleep(4000);
        }
    }
}
