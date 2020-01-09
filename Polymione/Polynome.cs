using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Polynome
{
    class Polynome
    {
        List<Coeff> coeffs = new List<Coeff>();
        
        public Polynome(List<Coeff> coeffs)
        {
            this.coeffs = coeffs;
        }

        public Polynome(string poly)
        {
            if (poly.Contains("E"))
            {
                poly = poly.Replace("E+", "Ep");
                poly = poly.Replace("E-", "Em");
            }

            poly = poly.Replace(" ", "");
            var elems = poly.Split("+".ToArray());
            foreach (var elem in elems)
            {

                if (elem.Contains("-"))
                {
                    var ms = elem.Split('-');


                    workWithCoeff(ms[0]);

                    for (int i = 1; i < ms.Length; i++)
                    {
                        workWithCoeff("-" + ms[i]);
                    }
                }
                else
                    workWithCoeff(elem);
            }
        }

        private void workWithCoeff(string elem)
        {
            if (elem == "")
            {
                return;
            }

            if (!elem.Contains("x"))
            {
                addInCoeffs(0, getCoeff(elem));
                return;
            }

            if (!elem.Contains("^"))
            {
                addInCoeffs(1, getCoeff(elem));
                return;
            }

            var pow = int.Parse(elem.Split('^')[1]);


            Coeff toAdd = getCoeff(elem);


            addInCoeffs(pow, toAdd);
        }

        private Coeff getCoeff(string elem)
        {
            if (elem.Contains("E"))
            {
                elem = elem.Replace("Ep", "E+");
                elem = elem.Replace("Em", "Em");
            }

            var c = elem.Split('^')[0].Replace("x", "").Replace("(", "").Replace(")", "").Split('/');

            Coeff toAdd;
            if (c.Length > 1)
            {
                var right = BigInteger.Parse(c[0]);
                var left = BigInteger.Parse(c[1]);
                toAdd = new Coeff(right, left);
            }
            else
            {
                if (c.First() == "-")
                {
                    toAdd = new Coeff(-1, 1);
                }
                else if (c.First() != "" && c.First() != "-" && c.First() != "+") toAdd = new Coeff(BigInteger.Parse(c[0]), 1);
                else toAdd = new Coeff(1, 1);
            }
            return toAdd;
        }

        private void addInCoeffs(int pow, Coeff coeff)
        {
            if (coeffs.Count <= pow)
            {
                int dest = pow - coeffs.Count;
                for (int i = 0;i<=dest;i++)
                {
                    coeffs.Add(new Coeff(0, 0));
                }
            }
            coeffs[pow] = coeff;
        }
        
        public static Polynome operator+ (Polynome p1, Polynome p2)
        {
            List<Coeff> coeffs = new List<Coeff>();
            int dest = Math.Min(p1.coeffs.Count, p2.coeffs.Count);
            for (int i =0;i<dest;i++)
            {
                coeffs.Add(p1.coeffs[i] + p2.coeffs[i]);
            }
            if (p1.coeffs.Count > dest)
            {
                for (int i = dest;i<p1.coeffs.Count;i++)
                {
                    coeffs.Add(p1.coeffs[i]);
                }
            }
            if (p2.coeffs.Count > dest)
            {
                for (int i = dest; i < p2.coeffs.Count; i++)
                {
                    coeffs.Add(p2.coeffs[i]);
                }
            }

            return new Polynome(coeffs);
        }

        public static Polynome operator -(Polynome p1, Polynome p2)
        {
            List<Coeff> coeffs = new List<Coeff>();
            int dest = Math.Min(p1.coeffs.Count, p2.coeffs.Count);
            for (int i = 0; i < dest; i++)
            {
                coeffs.Add(p1.coeffs[i] - p2.coeffs[i]);
            }
            if (p1.coeffs.Count > dest)
            {
                for (int i = dest; i < p1.coeffs.Count; i++)
                {
                    coeffs.Add(p1.coeffs[i]);
                }
            }
            if (p2.coeffs.Count > dest)
            {
                for (int i = dest; i < p2.coeffs.Count; i++)
                {
                    coeffs.Add(new Coeff(-p2.coeffs[i].up,p2.coeffs[i].down));
                }
            }

            return new Polynome(coeffs);
        }

        public static Polynome operator *(Polynome p1, Polynome p2)
        {
            List<Coeff> result = new List<Coeff>();

            var maxPow = p1.coeffs.Count + p2.coeffs.Count;
            for (int i = 0; i < maxPow; i++)
            {
                result.Add(new Coeff(0, 0));
            }

            for (int i = 0; i < p1.coeffs.Count; i++)
            {
                for (int j = 0; j < p2.coeffs.Count; j++)
                {
                    var c = new Coeff(p1.coeffs[i] * p2.coeffs[j]);
                    var pow = i + j;
                    result[pow] += c;
                }
            }

            return new Polynome(result);
        }

        public double AtPoint(double x)
        {
            double res = 0;
            int pow = 0;
            foreach (var c in coeffs)
            {

                BigInteger sub = BigInteger.Pow((BigInteger)x, pow) * c.up;
                if (c.down != 0)
                {
                    sub /= c.down;
                }

                res += (double)sub;
                pow++;
            }

            return res;
        }

        public static Polynome GCF(Polynome p1, Polynome p2)
        {
            Polynome copy1 = new Polynome(p1.coeffs);
            Polynome copy2 = new Polynome(p2.coeffs);

            var CoeffGCF = new BigInteger();
            bool order = false;

            while (copy1.coeffs.Count != 1 && copy2.coeffs.Count != 1)
            {
                if (copy1.coeffs.Count > copy2.coeffs.Count)
                {
                    copy1 = (copy1 / copy2).Value;
                    order = true;

                }
                else
                {
                    copy2 = (copy2 / copy1).Value;
                    order = false;
                }
                Console.WriteLine(copy1);
                Console.WriteLine(copy2);

            }

            if (order)
            {
                return (copy1 / new Polynome(copy1.coeffs[0].ToString())).Key;
            }
            else return (copy2 / new Polynome(copy2.coeffs[0].ToString())).Key;

            return copy2 + copy1;

        }

        public static KeyValuePair<Polynome, Polynome> operator /(Polynome p1, Polynome p2)
        {
            var result = new Polynome("0");

            if (p1.coeffs.Count < p2.coeffs.Count) return new KeyValuePair<Polynome, Polynome>(new Polynome("0"), p1);

            var index = p1.coeffs.Count - 1;

            int pow = p1.coeffs.Count - p2.coeffs.Count;

            var divided = new Polynome(p1.coeffs);

            var divider = new Polynome("0");

            do
            {
                divider = new Polynome("0");

                Coeff c = divided.coeffs[index] / p2.coeffs[p2.coeffs.Count - 1];

                result = result + new Polynome(c + "x^" + pow);

                divider = p2 * new Polynome(c + "x^" + pow);

                divided -= divider;

                index--;
                pow--;
            } while (pow >= 0);

            return new KeyValuePair<Polynome, Polynome>(result, divided);
        }

        //public Polynome Krek(Polynome p2)
        //{
        //    var m = 1;
        //    for (var i = 0; i < coeffs.Count / 2; i++)
        //    {
        //        // if at point i == 0
        //        if (Math.Abs(AtPoint(i)) < 0.0000001)
        //        {
        //            m = 1;
        //            return new Polynome($"x - {i}");
        //        }
        //    }

        //    var u = new List<Polynome>();
        //    for (var i = 1; i < coeffs.Count / 2; i++)
        //    {
        //        var M = new List<Polynome>();

        //    }
        //}

        public List<Polynome> Krek()
        {
            var parts = new List<Polynome>();
            for (int i = 0; i <= (coeffs.Count - 1) / 2; ++i)
            {
                if (Math.Abs(AtPoint(i)) < 0.00001)
                {
                    return new List<Polynome> { new Polynome($"x - {i}") };
                }
            }
            var U = GetDivs((int)AtPoint(0)).Select(i => new List<int> { i }).ToList();

            var partList = new List<Polynome>();
            for (int i = 1; i <= (coeffs.Count - 1) / 2; ++i)
            {
                var M = GetDivs((int)AtPoint(i));
                int length = Math.Max(M.Count, U.Count);
                U = Mul(U, M);
                foreach (var u in U)
                {
                    var polynom = Lagrange(u);
                    if (!polynom.IsInteger()) continue;
                    var remainder = (this / polynom).Value;
                    if (polynom.coeffs.Count > 1 && CheckPloynome(remainder)) 
                    {
                        partList.Add(polynom);
                    }
                }
            }
            parts = partList.ToList();
            return parts;
        }

        public Polynome Derivative()
        {
            Polynome polynome = new Polynome("0");
            for (int i = 1;i<coeffs.Count;i++)
            {
                polynome.addInCoeffs(i - 1, coeffs[i] * new Coeff(i,1));
            }
            return polynome;
        }

        private bool CheckPloynome(Polynome p)
        {

            for (int i = 1;i<p.coeffs.Count;i++)
            {
                if (p.coeffs[i].up!=0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsInteger()
        {
            foreach (var coeff in coeffs)
            {
                // if denom != 1
                if (coeff.down != BigInteger.One) return false;
            }

            return true;
        }

        public static Polynome Lagrange(List<int> values)
        {
            List<Polynome> parts = new List<Polynome>();
            for (int i = 0; i < values.Count; ++i)
            {
                var coef = 1;
                var tek = new List<Polynome>();
                for (int j = 0; j < values.Count; ++j)
                {
                    if (i == j) continue;
                    var newTemp = new Polynome(new List<Coeff>()
                    {
                        new Coeff(-j, 1),
                        new Coeff(1, 1)
                    });
                    //var newTemp = new Coeff(-j, 1);new Fraction[]
                    //{
                    //    new Fraction(-j, 1),
                    //    new Fraction(1, 1)
                    //});
                    tek.Add(newTemp);
                    coef *= (i - j);
                }
                if (tek.Count != 0)
                {
                    var t = new Coeff(values[i], coef);
                    var temp = tek.Aggregate((x, y) => x * y);
                    temp.coeffs = temp.coeffs.Select(x => x * t).ToList();
                    //temp.coeffs.Clear();
                    if (temp.coeffs.Count != 0)
                        parts.Add(temp);
                }
            }

            if (parts.Count == 0)
                return new Polynome("0");

            return parts.Aggregate((i, k) => i + k);
            //return null;
        }

        private static List<List<int>> Mul(List<List<int>> first, List<int> second)
        {
            var result = new List<List<int>>();
            foreach (var x in first)
            {
                foreach (var y in second)
                {
                    var copy = x.ToList();
                    copy.Add(y);
                    result.Add(copy);
                }
            }
            
            return result;
        }

        private static List<int> GetDivs(int num)
        {
            List<int> result = new List<int>()
            {
                1, -1
            };
            for (int i = 2; i <= BigInteger.Abs(num); ++i)
            {
                if (num % i == 0)
                {
                    result.Add(i);
                    result.Add(-i);
                }
            }

            return result;
        }

        private void removeZerosBefore()
        {
            var tmp = coeffs.ToList();

            tmp.Reverse();

            int i = 0;
            Coeff c = tmp[i];

            while (c.up==0)
            {
                tmp.Remove(c);
                if (tmp.Count > 0) c = tmp[i];
                else c = new Coeff(0, 1);
            }

            coeffs = tmp;
            coeffs.Reverse();
        }

        public override string ToString()
        {
            removeZerosBefore();
            string res = "";
            int pow = coeffs.Count - 1;
            var tmp = coeffs.ToList();

            tmp.Reverse();

            foreach (var c in tmp) { 

                if (c.up == 0 || c.down == 0)
                {
                    pow--;
                    continue;
                    
                }


                if (pow != coeffs.Count - 1 && c.up > 0)
                {
                        res += "+";
                }

                if (c.up != 1 || c.down != 1 || pow == 0)
                {
                    res += c;
                }

                if (pow == 1) res += "x";
                else if (pow != 0) res += "x^" + pow;

                pow--;
            }
            if (res == "") res = "0";
            return res;
        }
    }
}
