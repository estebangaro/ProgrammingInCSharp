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

            ShowPersonsFromQuery(PersonsFromMexico);
            //QuertWithFurtherInform(Persons);
            //QuertWithAsOrdered(Persons);
            QuerytWithAsSequiential2(Persons);

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
            return $"Nombre: {Name}, Ciudad: {City}";
        }
    }
}
