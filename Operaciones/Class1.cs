using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operaciones
{
    public class Matematicas
    {
        public Action<string> MostrarMsj { get; set; }
        public int Divide(int a, int b)
        {
            if (b == 0)
                MostrarMsj($"División entre 0, a:{a}, b:{b}");
            else
                MostrarMsj($"La división {a}/{b} = {a/b}");

            return b != 0 ? a / b : 0;
        }
    }
}
