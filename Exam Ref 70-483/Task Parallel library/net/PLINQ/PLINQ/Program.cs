using System;
using System.Collections.Generic;
using System.Linq;

namespace PLINQ
{
    class Program
    {
        static void Main(string[] args)
        {
            Person[] Persons = new[] {
            new Person { Name = "Esteban", City = "México", Color = "Amarillo" },
            new Person { Name = "Ambar", City = "México", Color = "Azul"},
            new Person { Name = "Pepe", City = "Perú", Color = "Morado"},
            new Person { Name = "Dalia", City = "Argentina", Color = "Rosa"},
            new Person { Name = "Rosa", City = "Dinamarca", Color = "Verde"},
            new Person { Name = "Rocio", City = "México", Color = "Azul"},
            new Person { Name = "Samantha", City = "Panama", Color = "Rojo"},
            new Person { Name = "Sebastian", City = "Brasil", Color = "Azul"},
            new Person { Name = "Damian", City = "Republica Dominicana", Color = "Purpura"},
            new Person { Name = "Arturo", City = "México", Color = "Amarillo"}
            };

            var PersonsFromMexico = from person in Persons.AsParallel()
                                    where person.City == "México"
                                    select new Person { Name = person.Name, City = person.City};

            //ShowPersonsFromQuery(PersonsFromMexico);
            //QuertWithFurtherInform(Persons);
            //QuertWithAsOrdered(Persons);
            //QuerytWithAsSequiential2(Persons);
            UsingTryCatchInPLINQQuery(Persons);

            Console.WriteLine("Finished processing. Press a key to end.");
            Console.ReadKey();
        }

        private static void ShowPersonsFromQuery(ParallelQuery<Person> personsFromMexico)
        {
            Console.WriteLine("Listando personas mexicanas");
            foreach (var Person in personsFromMexico)
            {
                Console.WriteLine(Person);
            }
        }

        private static void ShowPersonsFromQuery(IEnumerable<Person> personsFromMexico)
        {
            Console.WriteLine("Listando personas mexicanas");
            foreach (var Person in personsFromMexico)
            {
                Console.WriteLine(Person);
            }
        }

        static void QuertWithFurtherInform(Person[] dataSource)
        {
            Console.WriteLine($"Número de procesadores disponibles: {Environment.ProcessorCount}");
            var ParallelQueryForced = from person in dataSource.AsParallel()
                                      .WithDegreeOfParallelism(Environment.ProcessorCount)
                                      .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                                      where person.City == "México"
                                      select new Person { Name = person.Name, City = person.City };

            ShowPersonsFromQuery(ParallelQueryForced);
        }

        static void QuertWithAsOrdered(Person[] dataSource)
        {
            Console.WriteLine($"Iniciando consulta con AsOrdered: {Environment.ProcessorCount}");
            var ParallelQueryForced = (from person in dataSource.AsParallel()
                                      .AsOrdered()
                                      where person.City == "México"
                                       select new Person { Name = person.Name, City = person.City });

            ShowPersonsFromQuery(ParallelQueryForced);
        }

        static void QuertWithAsSequiential(Person[] dataSource)
        {
            Console.WriteLine($"Iniciando consulta con AsSequential: {Environment.ProcessorCount}");
            var ParallelQueryForced = (from person in dataSource.AsParallel().AsSequential()
                                       where person.City == "México"
                                       select new Person { Name = person.Name, City = person.City }).Select(person => person);

            ShowPersonsFromQuery(ParallelQueryForced);
        }

        static void QuerytWithAsSequiential2(Person[] dataSource)
        {
            Console.WriteLine($"Iniciando consulta con AsSequential2: {Environment.ProcessorCount}");
            //var ParallelQueryForced = (from person in dataSource.AsParallel()
            //                           where person.City == "México"
            //                           orderby person.Name
            //                           select new Person(person.Name == "Ambar" ? 5000 : 200, person.Name)).Take(4).ToList();

            var ParallelQueryForced = dataSource.AsParallel()
                .Where(person => person.City == "México")
                .OrderBy(person => person.Name)
                //.GroupBy(person => person.Color)
                //.WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .Select(grupoColores => new Person(grupoColores.Name == "Ambar" ? 5000 : 200, grupoColores.Name))
                //.WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                //.AsSequential()
                .Take(4);

            ParallelQueryForced.
                ForAll(name =>
            {
                Console.WriteLine(name);
            });
        }

        static void UsingTryCatchInPLINQQuery(Person[] dataSource)
        {
            //Comentarios: Se detiene la ejecución de la consulta paralelizada, a la primera excepción originada.
            try
            {
                var Result = (from person in dataSource.AsParallel()
                             where !person.Name.StartsWith("a", StringComparison.InvariantCultureIgnoreCase)
                             select GetPersonFromPerson(person));

                //System.Threading.Tasks.Parallel.Invoke(() => GetPersonFromPerson(dataSource[0]), () => GetPersonFromPerson(dataSource[1]), () => GetPersonFromPerson(dataSource[2]),
                //() => GetPersonFromPerson(dataSource[3]), () => GetPersonFromPerson(dataSource[4]));

                Result.ForAll(Person => Console.WriteLine(Person));
                //foreach (var Person in Result)
                //{
                //    Console.WriteLine(Person);
                //}
            }
            catch (AggregateException ae)
            {
                Console.WriteLine("Iniciando control de excepciones!!");
                int i = 1;
                System.Threading.Tasks.Parallel.ForEach(ae.InnerExceptions, innerException => Console.WriteLine($"Excepción #{i++}, " +
                    $"Error: {ae.Message}"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Inciando control de excepción; Error " + ex.Message);
            }
        }

        static Person GetPersonFromPerson(Person person)
        {
            Console.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId + ") Obteniendo persona: " + person.Name);
            if(person.Name.StartsWith("a", StringComparison.CurrentCultureIgnoreCase))
            {
                throw new ArgumentException("No se permiten nombres con letra de inicio en \"A\"; Nombre " + person.Name);
            }

            return new Person
            {
                City = person.City,
                Color = person.Color,
                Name = person.Name
            };
        }
    }
    
    class Person
    {
        private static Random NumberGenerator = new Random();
        public Person()
        {
            System.Threading.Thread.Sleep(NumberGenerator.Next(10, 1000));
        }

        public Person(int ms, string name)
        {
            System.Threading.Thread.Sleep(ms);
            Name = name;
        }

        public string Name { get; set; }
        public string City { get; set; }
        public string Color { get; set; }

        public override string ToString()
        {
            return $"Nombre: {Name}, Ciudad: {(City ?? "No registrada")}, Color {(Color ?? "No registrado")}";
        }
    }
}
