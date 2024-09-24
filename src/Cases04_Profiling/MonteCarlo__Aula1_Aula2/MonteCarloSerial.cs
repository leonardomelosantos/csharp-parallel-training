using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MonteCarlo
{
    public class MonteCarloSerial : IMonteCarlo
    {
        public MonteCarloSerial() { }

        public double Calc(long n)
        {
            long insideCircle = MonteCarloHelper.CalcPoints(n);
            return MonteCarloHelper.CalcPi(insideCircle, n);
        }
    }
}
