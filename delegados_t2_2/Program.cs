using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace delegados_t2_2
{
class Program
{
static void Main(string[] args)
{
    Console.
    WriteLine("Introducir número A: ");
    int A = int.
        Parse(Console.ReadLine());
    Console.
    WriteLine("Introducir número B: ");
    int B = int.Parse(Console.ReadLine());

    delegados_t2_1.Operaciones operacion = 
    new delegados_t2_1.Operaciones();
    operacion.MostrarMsj = Console.WriteLine;

    float division = operacion.Divide(A, B);
}
}
}
