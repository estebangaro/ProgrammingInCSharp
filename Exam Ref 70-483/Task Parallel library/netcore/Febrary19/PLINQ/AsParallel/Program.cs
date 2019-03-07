using System;
using System.Linq;

namespace AsParallel
{
    class Program
    {
        class Person: System.Collections.Generic.IComparer<Person>{

            public Person(){
                System.Threading.Thread.Sleep(100);
            }

            public string Name { get; set; }
            public int Age { get; set; }
            public string City { get; set; }
            public DateTime DateOfBirth { get; set; }

            public int Compare(Person x, Person y)
            {
                return string.Compare(x.Name, y.Name);
            }
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
            6) El operador AsOrdered, permite presentar los resultados de salida preservando el
                orden original de los datos de entrada, independientemente del tipo de dato
                proyectado (el propio objeto o alguno otro tipo complejo / primitivo).
            7) El operador AsOrdered, solo puede ser ejecutado precedido de los siguientes operadores:
                7.1) AsParallel.
                7.2) Range y Repeat.
            8) AsSequential, permite ejecutar operadores de consulta (precedidos por AsSequiential), como
                operadores LINQ to Objects al convertir la colección ParallelQuery en IEnumerable, esto
                es de mucha ayuda si deseamos ejecutar ciertos operadores de manera ordenada / secuencial.
            9) El operador OrderBy de ParallelQuery, no garantiza un ordenamiento estable, por lo que
                para asegurar el ordenamiento en una consulta paralelizada, se recomienda implementar
                la técnica expuesta en la URL: https://docs.microsoft.com/en-us/dotnet/api/system.linq.parallelenumerable.orderby?view=netcore-2.2 
                sección "Remarks".
            10) No se logŕo alterar el orden producido por el operador OrderBy (+ técnica para asegurar
                ordenamiento estable), con el objetivo de implementar caso de uso de operador AsSequiential.
            11) El operador ForAll, permite obtener los resultados de la consulta (de salida) como stream,
                de modo que tan pronto la consulta arroje cada uno de los elementos resultantes, estos
                se vuelven disponibles inmediatamente para el consumidor de la consulta, por lo que
                las sugerencias de fusión de datos de salida, junto con las operaciones de ordenamiento
                solicitadas (asordered, order by) son ignoradas (MergeOptions.NotBuffered).
            12) El método ForAll, a diferencia de la construcción tradicional foreach de C#, se ejecuta en paralelo
                y además antes de que la consulta finalice.
            13) El operador ForAll, a diferencia de la implementación de System.Threading.Tasks.Parallel.ForEach,
                igualmente se ejecuta en paralelo, sin embargo hace disponibles los resultados de salida, tan pronto se
                encuentren disponibles (MergeOptions Not Buffered).
            14) Se puede implementar control de excepciones, mediente el uso de la instrucción TRY, en consultas
                paralelizadas.
            15) Se recomienda implementar bloques catch para la posible generación de excepciones, en función
                de los operadores utilizados. Ejem: Where; ArgumentNullException, OperationCanceledException, AggregateException
                generandose excepiones agregadas (AggregateException's), para excepciones generadas al ejecutar
                los métodos anónimos proporcionados a los operadores de consulta.
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
            //UseInformingParallelization(Persons);
            UseExceptionsInQueries(Persons);

            Console.WriteLine("Finalizando demo, de método AsParallel...");
            Console.ReadKey();
        }

        static void UseExceptionsInQueries(System.Collections.Generic.IEnumerable<Person> Persons){
            try
            {
                var Cts = new System.Threading.CancellationTokenSource();
                var Ct = Cts.Token;
                Cts.Cancel();
                Func<Person, bool> Predicate = null;
                    //person => { ShowThreadCurrentInfo($"where (age); {person.Name}", true); if(person.Age > 15) { 
                    //throw new NotSupportedException($"Edad mayor a 15 no permitida, Thread {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                //} return person.Age > 10; };
                var QueryPersons = Persons.AsParallel()
                    .WithCancellation(Ct).Where(Predicate).ToList();
            }
            catch(AggregateException ae)
            {
                Console.WriteLine("Primera excepción: " + ae.InnerException.Message);
                foreach(var Exception in ae.InnerExceptions){
                    Console.WriteLine("Exepción: " + Exception.Message + "; Tipo: " + Exception.GetType());
                }
            }
            catch(ArgumentNullException an){
                Console.WriteLine("Argument null exception handled!");
            }
            catch(OperationCanceledException oc){
                Console.WriteLine("Operation Cancelled exception handled!");
            }catch{
                Console.WriteLine("Excepción no manejada");
            }
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

        static void UseForEach(System.Collections.Generic.IEnumerable<Person> Persons){
         Console.WriteLine("Iniciando demo de ForEach...");

            var PersonsOfMexico = 
            Persons.AsParallel()
                                    .Where(person => { ShowThreadCurrentInfo($"\"Where city (plinq - {person.Name})\"");
                                        return person.City == "México"; })
                                    .Select((person,index) => { ShowThreadCurrentInfo($"\"Select person-index (plinq - {person.Name})\""); return new { Person=person, Index=index }; })
                                    .OrderBy((person) => { ShowThreadCurrentInfo($"\"order by name (plinq - {person.Person.Name})\""); return person.Person.Name; })
                                    .ThenBy(person => { ShowThreadCurrentInfo($"\"then by (plinq - {person.Person.Name})\""); return person.Index; })
                                    .Select((person) => { ShowThreadCurrentInfo($"\"Select person (plinq - {person.Person.Name})\""); return person.Person.Name; })
                                    //.ForAll(personName => Console.WriteLine(personName))
                                    ;

            Console.WriteLine("Personas que viven en México");
            System.Threading.Tasks.Parallel.ForEach(PersonsOfMexico, 
                personName => { ShowThreadCurrentInfo($"\"ForEach person-name (parallel- {personName})\"", true); });
        }

        static void UseInformingParallelization(System.Collections.Generic.IEnumerable<Person> Persons){
            UseWithDegreeOfParallelism(Persons);
            //UseWithCancellation(Persons);
            //UseWithExecutionMode(Persons);
            //UseAsOrdered(Persons);
            //UseLinq(Persons);
            //UseOrderBy(Persons);
            //UseForAll(Persons);
            //UseForEach(Persons);
        }
        static void UseLinq(System.Collections.Generic.IEnumerable<Person> Persons){
            Console.WriteLine("Iniciando demo de LINQ...");

            var PersonsOfMexico = Persons
                                    .Where(person => { ShowThreadCurrentInfo("\"Where city (plinq)\"");
                                        return person.City == "México"; })
                                    .Select(person => person.Name);
            
            Console.WriteLine("Personas viviendo en México:");
            //var PersonsOfMexicoList = PersonsOfMexico.ToList();
            foreach (var PersonName in PersonsOfMexico)
            {
                Console.WriteLine(PersonName);
            }
        }

        static void UseAsOrdered(System.Collections.Generic.IEnumerable<Person> Persons){
            Console.WriteLine("Iniciando demo de AsOredered...");

            var PersonsOfMexico = Persons.AsParallel()
                                    .AsOrdered()
                                    .WithDegreeOfParallelism(3)
                                    .Where(person => { ShowThreadCurrentInfo($"\"Where city (plinq - {person.Name})\"");
                                        return person.City == "México"; })
                                    .Select(person => { ShowThreadCurrentInfo($"\"Select name (plinq - {person.Name})\""); 
                                        return person.Name; });
            
            Console.WriteLine("Personas viviendo en México:");
            foreach (var PersonName in PersonsOfMexico)
            {
                Console.WriteLine(PersonName);
            }
        }

        static void UseOrderBy(System.Collections.Generic.IEnumerable<Person> Persons){
            Console.WriteLine("Iniciando demo de OrderBy...");

            var PersonsOfMexico = Persons.AsParallel()
                                    .WithDegreeOfParallelism(3)
                                    .Where(person => { ShowThreadCurrentInfo($"\"Where city (plinq - {person.Name})\"");
                                        return person.City == "México"; })
                                    /*.OrderBy(person => { ShowThreadCurrentInfo($"\"OrderBy name (plinq - {person.Name})\"", true); return person.Age; })
                                    .Select(person => { ShowThreadCurrentInfo($"\"Select name (plinq - {person.Name})\"");
                                        return person; })*/
                                    .Select((person,index) => { ShowThreadCurrentInfo($"\"Select person-index (plinq - {person.Name})\""); return new { Person=person, Index=index }; })
                                    .OrderBy((person) => { ShowThreadCurrentInfo($"\"order by name (plinq - {person.Person.Name})\""); return person.Person.Name; })
                                    .ThenBy(person => { ShowThreadCurrentInfo($"\"then by (plinq - {person.Person.Name})\""); return person.Index; })
                                    .Select((person) => { ShowThreadCurrentInfo($"\"Select person (plinq - {person.Person.Name})\""); return person.Person.Name; })
                                    /*. AsSequential()
                                    .Where(person => { ShowThreadCurrentInfo($"\"Where name after s (plinq - {person.Name})\"");
                                        return person.Name.Contains('a'); })
                                    .Select(person => { ShowThreadCurrentInfo($"\"Select name after s (plinq - {person.Name})\"");
                                        return person.Name; })*/
                                    ;
            
            Console.WriteLine("Personas viviendo en México:");
            foreach (var PersonName in PersonsOfMexico)
            {
                Console.WriteLine(PersonName);
            }
        }

        static void 
        UseForAll(System.Collections.Generic.IEnumerable<Person> Persons){
         Console.WriteLine("Iniciando demo de ForAll...");

            Console.WriteLine("Personas viviendo en México:");
            var PersonsOfMexico = 
            Persons.AsParallel().AsOrdered()
                                    .WithDegreeOfParallelism(3)
                                    .Where(person => { ShowThreadCurrentInfo($"\"Where city (plinq - {person.Name})\"");
                                        return person.City == "México"; })
                                    //.Select((person,index) => { ShowThreadCurrentInfo($"\"Select person-index (plinq - {person.Name})\""); return new { Person=person, Index=index }; })
                                    //.OrderBy((person) => { ShowThreadCurrentInfo($"\"order by name (plinq - {person.Person.Name})\""); return person.Person.Name; })
                                    //.ThenBy(person => { ShowThreadCurrentInfo($"\"then by (plinq - {person.Person.Name})\""); return person.Index; })
                                    .Select((person) => { ShowThreadCurrentInfo($"\"Select person (plinq - {person.Name})\""); return person.Name; })
                                    //.ForAll(personName => Console.WriteLine(personName))
                                    ;

            Console.WriteLine("Personas viviendo en México:");
            foreach (var PersonName in PersonsOfMexico)
            {
                Console.WriteLine(PersonName);
            }
        }

        static bool IsFromMexico(Person person) { 
            ShowThreadCurrentInfo($"\"Where city (plinq - {person.Name})\"");
            /*if(person.Name == "Ámbar"){
                throw new NotSupportedException("es Barbarita ! :)");
            }*/
            return person.City == "México"; 
        }

        static void UseWithDegreeOfParallelism(System.Collections.Generic.IEnumerable<Person> Persons){
            Console.WriteLine("Iniciando demo de WithDegreeOfParallelism...");

            try{
            var PersonsOfMexico = Persons.AsParallel()
                                    .WithDegreeOfParallelism(3)
                                    //.WithMergeOptions(ParallelMergeOptions.NotBuffered)
                                    .Where(person => person.City == "México")
                                    .Select((person) => { ShowThreadCurrentInfo($"\"select name (plinq - {person.Name})\""); return person.Name; });
            
            Console.WriteLine("Personas viviendo en México:");
            PersonsOfMexico.ForAll(PersonName => Console.WriteLine($"{PersonName} - Thread {System.Threading.Thread.CurrentThread.ManagedThreadId}"));
            
            /*
            System.Threading.Tasks.Parallel.ForEach(PersonsOfMexico, 
                PersonName => Console.WriteLine($"{PersonName} - Thread {System.Threading.Thread.CurrentThread.ManagedThreadId}"));*/
            /*foreach (var PersonName in PersonsOfMexico)
            {
                Console.WriteLine($"{PersonName} - Thread {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            }*/
            }catch(AggregateException ae){

                Console.WriteLine("Manejando excepcion de agregación (Aggregate exception)");

                foreach(var e in ae.InnerExceptions){
                    Console.WriteLine(e.Message);
                }
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

        static void ShowThreadCurrentInfo(string opertor, bool excludeSlepp = false){
            Console.WriteLine($"Executing operator: {opertor} in ManageThreadId: " + 
                $"{System.Threading.Thread.CurrentThread.ManagedThreadId}");
            int Interval = new Random().Next(100, 3000);
            if(!excludeSlepp){
                System.Threading.Thread.Sleep(Interval);
            }
        }
    }
}
