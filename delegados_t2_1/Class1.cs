using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace delegados_t2_1
{
    public class Operaciones
    {
        public Action<string> MostrarMsj;
        public int Divide(int A, int B)
        {
            if (B == 0)
                MostrarMsj($"División entre cero {A}/{B}");
            else
                MostrarMsj($"{A}/{B} = {A / B}");

            return B > 0 ? A / B : 0;
        }
    }
}
