using System;
using System.Linq;

namespace PLINQ
{
    class Program
    {
        class Person{
            public string Name { get; set; } 
            public string City { get; set; } 
        }

        static void Main(string[] args)
        {
            Person[] Persons = new Person[]{
                new Person{Name = "Amber Trejo", City = "México"},
                new Person{Name = "Esteban GaRo", City = "México"},
                new Person{Name = "Bebé García Trejo", City = "México"},
                new Person{Name = "Fernando Arellano", City = "México"},
                new Person{Name = "Víctor Arellano", City = "México"},
                new Person{Name = "Rocío Rosas", City = "México"},
                new Person{Name = "Dalia García", City = "Perú"},
                new Person{Name = "ESteban Rosas", City = "México"},
            };
            Console.WriteLine("Hello PLINQ!");

            UseAsParallel(Persons);
            
        }

        static void ShowInfo(string operator_, Person person){
            Console.WriteLine($"Ejecutando {operator_} con persona {person.Name}, en HILO {System.Threading.Thread.CurrentThread.ManagedThreadId}");
        }

        static void UseAsParallel(Person[] persons){
            Console.WriteLine("Iniciando ejecución de método AsParallel");

            var PersonsFromMexico = persons
                /*.AsParallel()
                .WithDegreeOfParallelism(3)*/
                .Where(person => { ShowInfo("where", person); return person.City == "México"; })
                //.GroupBy(person => {ShowInfo("groupby", person); return person.Name.First();})
                //.Select(persongroup => {ShowInfo("select", new Person{Name = persongroup.Key.ToString()}); return new Person{Name = persongroup.Key.ToString(), City = string.Join(',', persongroup.Select(person => person.Name))};});
                .Select(person => {ShowInfo("select", person); return person;});

            foreach (var person in PersonsFromMexico)
            {
                Console.WriteLine(string.Concat(person.Name, "-", person.City));
            }
            
            Console.WriteLine("Finalizando ejecución de método AsParallel");
        }
    }
}
