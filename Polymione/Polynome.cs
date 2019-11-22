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
                    foreach (var m in ms)
                    {
                        if (m!="")
                        workWithCoeff("-" + m);
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
                    coeffs.Add(p2.coeffs[i]);
                }
            }

            return new Polynome(coeffs);
        }

        public override string ToString()
        {
            string res = "";
            int pow = 0;
            foreach (var c in coeffs)
            {
                if (!(c.down == 0 && c.up == 0))
                {
                    if (c.up>0 && res.Length > 0)
                    {
                        res += "+";
                    }
                    if (c.up < 0)
                    {
                        c.up = -c.up;
                        res += "-";
                    }
                    res += c + "x^" + pow;
                }
                pow++;
            }
            return res;
        }
    }
}
