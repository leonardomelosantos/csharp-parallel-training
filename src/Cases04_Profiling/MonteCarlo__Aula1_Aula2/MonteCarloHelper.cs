using System;

namespace MonteCarlo
{
    public interface IMonteCarlo
    {
        public double Calc(long n);
    }

    public static class MonteCarloHelper
    {

        public static long CalcPoints(long n)
        {
            Random rnd = new();
            long insideCircle = 0;
            for (int i = 0; i < n; i++)
            {
                CalcPoint(rnd, ref insideCircle);
            }
            return insideCircle;
        }

        public static double CalcPi(long pointsInsideCircle, long totalPoints)
        {
            return 4.0 * pointsInsideCircle / totalPoints;
        }

        public static void CalcPoint(Random rnd, ref long insideCircle)
        {
            double x = rnd.NextDouble();
            double y = rnd.NextDouble();
            if (x * x + y * y <= 1)
            {
                insideCircle++;
            }
        }
    }
}
