using System;
using System.Collections.Generic;
using System.Linq;
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
            var c = elem.Split('^')[0].Replace("x", "").Split('/');

            Coeff toAdd;
            if (c.Length > 1)
            {
                var right = double.Parse(c[0]);
                var left = double.Parse(c[1]);
                toAdd = new Coeff(right, left);
            }
            else
            {
                toAdd = new Coeff(double.Parse(c[0]), 1);
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

        public static Polynome operator*(Polynome p1, Polynome p2)
        {
            List<Coeff> result = new List<Coeff>();

            var maxPow = p1.coeffs.Count + p2.coeffs.Count;
            for (int i = 0; i < maxPow; i++)
            {
                result.Add(new Coeff(0,0));
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
                double sub = Math.Pow(x, pow) * c.up;
                if (c.down != 0)
                {
                    sub /= c.down;
                }

                res += sub;
                pow++;
            }

            return res;
        }

        public static Polynome operator /(Polynome p1, Polynome p2)
        {
            List<Coeff> result = new List<Coeff>();



            return null;
        }

        public override string ToString()
        {
            string res = "";
            int pow = 0;
            var tmp = coeffs.ToList();

            foreach (var c in coeffs)
            {
                if (c.up != 0)
                {
                    var coeff = new Coeff(c);
                    if (c.up>0 && res.Length > 0)
                    {
                        res += "+";
                    }
                    if (c.up < 0)
                    {
                        coeff.up = -coeff.up;
                        res += "-";
                    }
                    res += coeff + "x^" + pow;
                }
                pow++;
            }
            return res;
        }
    }
}
