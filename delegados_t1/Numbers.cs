using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace delegados_t1
{
    public class Numbers
    {
        public List<int> GetNumbers(int max, 
            Action<int> showProcess = null)
        {
            List<int> list = new List<int> { };

            for(int i = 0; i<max; i++)
            {
                if(i % 2 == 0)
                {
                    showProcess?.Invoke(i);
                    list.Add(i);
                    System.Threading.Thread.Sleep(10000);
                }
            }

            return list;
        }
    }
}
