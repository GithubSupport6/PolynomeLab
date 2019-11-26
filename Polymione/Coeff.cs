using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Polynome
{
    class Coeff
    {
        public Coeff(Coeff c)
        {
            this.down = c.down;
            this.up = c.up;
        }

        public Coeff(double up, double down)
        {
            var nod = NOD(Math.Abs(up), Math.Abs(down));

            if (nod!=0)
            {
                up = up / nod;
                down = down / nod;
            }

            this.up = up;
            if (Math.Sign(down) < 0)
                this.down = -down;
            else
                this.down = down;
        }

        private double NOD(double up, double down)
        {
            while (up!=0 && down!=0)
            {
                if (up > down)
                {
                    up = up % down;
                }
                else
                {
                    down = down % up;
                }
            }
            return up + down;
        }

        public double up { get; set; }
        public double down { get; set; }

        public static Coeff operator+(Coeff c1, Coeff c2)
        {
            if (c1.down ==0 && c1.up == 0)
            {
                return c2;
            }
            if (c2.up == 0 && c2.down == 0)
            {
                return c1;
            }
            Coeff res = new Coeff(c1.up * c2.down + c2.up * c1.down, c1.down * c2.down);
            return res;
        }

        public static Coeff operator -(Coeff c1, Coeff c2)
        {
            if (c1.down == 0 && c1.up == 0)
            {
                return new Coeff(-c2.up,c2.down);
            }
            if (c2.up == 0 && c2.down == 0)
            {
                return c1;
            }
            Coeff res = new Coeff(c1.up * c2.down - c2.up * c1.down, c1.down * c2.down);
            return res;
        }

        public static Coeff operator /(Coeff c1, Coeff c2)
        {
            Coeff temp = new Coeff(c2.down,c2.up);
            return c1 * temp;
        }

        public static Coeff operator *(Coeff left, Coeff right)
        {
            double up = left.up * right.up;
            double down = left.down * right.down;
            return  new Coeff(up,down);
        }

        public override string ToString()
        {
            if (down == 1)
            {
                return up.ToString();
            }

            if (up == 0)
            {
                return "";
            }

            string res = "";
            if (up < 0 || down < 0)
            {
                res += '-';
            }
            return res + "(" + Math.Abs(up) + "/" + Math.Abs(down) + ")";
        }
    }
}
