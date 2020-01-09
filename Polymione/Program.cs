using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polymione;

namespace Polynome
{
    class Program
    {
        static void Main(string[] args)
        {
            //Polynome p = new Polynome("3x^2 + 2 - 1/2x^3 - 6x");
           
            //Polynome p3 = new Polynome("3/2x + 5x^4 - 3/6x^3 - 1");

            //Polynome p2 = new Polynome("3x^2 + 6x - 7");
            //Polynome p4 = new Polynome("16x^1+5");

            //Console.WriteLine(p2);
            //Console.WriteLine(p4);

            //Polynome p5 = p2 * p4;

            //var p6 = p3 / p;

            //Console.WriteLine(p5);
            

            // НУЖГЫЙ КОД
            var p1 = new Polynome("-16x^6 + 20x^5 - 4x^3 - 12x^2 + 1");
            var p2 = new Polynome("2x^4 - 3x^3 - 5x^2 - 14x - 1");
            //var p1 = new Polynome("8x^6 + 32x^3 - 16x^2 + 40");
            //var p2 = new Polynome("-2x^4 - 3x^3 + 5x^2 - x - 1");

            Console.WriteLine("Polynome 1: " + p1);
            Console.WriteLine("Polynome 2: " + p2);

            var a = (p1 / p2).Key;
            var b = (p1 / p2).Value;
            Console.WriteLine("Full : " + a);
            Console.WriteLine("Remains : " + b);

            //var num = MathUtils.Solve(new List<int>() { 5, 7, 11, 13 }, new List<int>() { 2, 1, 3, 8 });
            var num = MathUtils.Solve(new[] { 3, 7, 13 }, new[] { 1, 3, 5 });
            //var num = MathUtils.Solve(new[] { 5, 7, 11 }, new[] { 1, 3, 5 });

            Console.WriteLine($"CHINA RESULT!: {num}");

            var res = a.AtPoint(num);
            Console.WriteLine($"full part of polymonopolynom at {num} is {res}");

            res = b.AtPoint(num);
            Console.WriteLine($"remains of monononomopolynom at {num} is {res}");


            Console.WriteLine("GCF = " + Polynome.GCF(p1, p2));

            Console.WriteLine("Kroneker's diveders of polynome 2: ");
            var krek = p2.Krek();
            var pol = krek[0];
            Console.WriteLine(pol);
            Console.WriteLine((p2 / pol).Key + " + " + (p2 / pol).Value);
            Console.WriteLine("RES: ");
            Console.WriteLine((p2 / pol).Key * pol);
            Console.WriteLine("Derivative: ");
            Console.WriteLine(p2.Derivative());
            //Console.WriteLine(krek);

            Console.ReadKey();
        }
    }
}
