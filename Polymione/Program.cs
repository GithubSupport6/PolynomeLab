using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polymione
{
    class Program
    {
        static void Main(string[] args)
        {
            Polynome p = new Polynome("3x^2 + 2");
           
            Polynome p3 = new Polynome("3/2x + 5x^4 - 3/6x^3 - 1");

            Polynome p4 = p + p3;

            Console.WriteLine(p4);
            Console.ReadKey();
        }
    }
}
