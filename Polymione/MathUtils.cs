using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polymione
{
    class MathUtils
    {
        private static int M;

        static void CalcM(List<int> m)
        {
            M = m[0];
            for (int i = 1; i < m.Count; i++)
                M *= m[i];
        }

        
        //public static int Solve(List<int> modules, List<int> remains)
        //{
        //    var num = 1;
        //    var countOfRight = 0;

        //    while (true)
        //    {
        //        for (int i = 0; i < modules.Count; i++)
        //        {
        //            if (num % modules[i] == remains[i])
        //            {
        //                countOfRight++;
        //            }
        //            else
        //            {
        //                break;
        //            }
        //        }

        //        if (countOfRight == remains.Count) break;

        //        countOfRight = 0;
        //        num++;
        //    }

        //    return num;
        //}

        public static long Solve(int[] modules, int[] remains)
        {
            CalcM(modules.ToList());
            long result = 0;
            long Marr = 0;
            long carr = 0;
            long coefX = 0, coefY = 0;

            var res = new List<long>();
            var num = 0;
            while ((num++) != 3)
            {
                for (int i = 0; i < modules.Length; i++)
                {
                    Marr = M / modules[i];
                    Euclid(Marr, modules[i], ref coefX, ref coefY);
                    carr = coefX * Marr;
                    result += carr * remains[i];
                }
            }
            
            return (modules.Length * M + result) % M;
        }

        static long Euclid(long a, long b, ref long x, ref long y)
        {
            long g = 1, u, v, A = 1, B = 0, C = 0, D = 1;
            while (((a & 1) | (b & 1)) == 0)
            {
                a >>= 1;
                b >>= 1;
                g <<= 1;
            }
            u = a;
            v = b;
            while (u != 0)
            {
                while ((u & 1) == 0)
                {
                    u >>= 1;
                    if (((A & 1) | (B & 1)) == 0)
                    {
                        A >>= 1;
                        B >>= 1;
                    }
                    else
                    {
                        A = (A + b) >> 1;
                        B = (B - a) >> 1;
                    }
                }
                while ((v & 1) == 0)
                {
                    v >>= 1;
                    if (((C & 1) | (D & 1)) == 0)
                    {
                        C >>= 1;
                        D >>= 1;
                    }
                    else
                    {
                        C = (C + b) >> 1;
                        D = (D - a) >> 1;
                    }
                }
                if (u >= v)
                {
                    u -= v;
                    A -= C;
                    B -= D;
                }
                else
                {
                    v -= u;
                    C -= A;
                    D -= B;
                }
            }
            x = C;
            y = D;
            return g * v;
        }
    }
}
