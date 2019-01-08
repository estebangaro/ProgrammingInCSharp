using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLINQ
{
    class Program
    {
        static void Main(string[] args)
        {
            Person[] Persons = new[] {
            new Person { Name = "Esteban", City = "México" },
            new Person { Name = "Ambar", City = "México"},
            new Person { Name = "Pepe", City = "Perú"},
            new Person { Name = "Dalia", City = "Argentina"},
            new Person { Name = "Rosa", City = "Dinamarca"},
            new Person { Name = "Rocio", City = "México"},
            new Person { Name = "Samantha", City = "Panama"},
            new Person { Name = "Sebastian", City = "Brasil"},
            new Person { Name = "Damian", City = "Republica Dominicana"},
            new Person { Name = "Arturo", City = "México"}
            };

            var PersonsFromMexico = from person in Persons.AsParallel()
                                    where person.City == "México"
                                    select new Person { Name = person.Name, City = person.City};

            ShowPersonsFromQuery(PersonsFromMexico);
            //QuertWithFurtherInform(Persons);
            //QuertWithAsOrdered(Persons);
            QuertWithAsSequiential2(Persons);

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

        static void QuertWithAsSequiential2(Person[] dataSource)
        {
            Console.WriteLine($"Iniciando consulta con AsSequential2: {Environment.ProcessorCount}");
            var ParallelQueryForced = (from person in dataSource.AsParallel()
                                       where person.City == "México"
                                       //orderby person.Name
                                       select new Person(person.Name == "Ambar" ? 5000 : 200, person.Name))
                                       .WithExecutionMode(ParallelExecutionMode.ForceParallelism).Take(4);

            foreach (var name in ParallelQueryForced)
            {
                Console.WriteLine(name);
            }
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

        public override string ToString()
        {
            return $"Nombre: {Name}, Ciudad: {City}";
        }
    }
}
