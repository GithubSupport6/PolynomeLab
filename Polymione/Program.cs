using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polynome
{
    class Program
    {
        static void Main(string[] args)
        {
            Polynome p = new Polynome("3x^2 + 2 - 1/2x^3 - 6x");
           
            Polynome p3 = new Polynome("3/2x + 5x^4 - 3/6x^3 - 1");

            Polynome p2 = new Polynome("3x^2 + 6x - 7");
            Polynome p4 = new Polynome("16x^1+5");

            Console.WriteLine(p2);
            Console.WriteLine(p4);

            Polynome p5 = p2 * p4;
            Console.WriteLine(p5);
            


            Console.ReadKey();
        }
    }
}
