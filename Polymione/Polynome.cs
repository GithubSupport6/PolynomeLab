using System;
using System.CodeDom.Compiler;
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
            var c = elem.Split('^')[0].Replace("x", "").Replace("(", "").Replace(")", "").Split('/');

            Coeff toAdd;
            if (c.Length > 1)
            {
                var right = double.Parse(c[0]);
                var left = double.Parse(c[1]);
                toAdd = new Coeff(right, left);
            }
            else
            {
                if (c.First() == "-")
                {
                    toAdd = new Coeff(-1,1);
                }
                else if (c.First() != "" && c.First() != "-" && c.First() != "+") toAdd = new Coeff(double.Parse(c[0]), 1);
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

        public static KeyValuePair<Polynome,Polynome> operator /(Polynome p1, Polynome p2)
        {
            var result = new Polynome("0");

            if (p1.coeffs.Count < p2.coeffs.Count) return new KeyValuePair<Polynome, Polynome>(new Polynome("0"),p1);

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
            } while (pow>=0);

            return new KeyValuePair<Polynome, Polynome>(result,divided);
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
                c = tmp[i];
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

            return res;
        }
    }
}
