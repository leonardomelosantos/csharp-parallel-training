using System.Threading.Tasks;

namespace MonteCarlo
{
    class MonteCarloParallel(int numThreads) : IMonteCarlo
    {
        public double Calc(long n)
        {
            long pointsPerThreads = n / numThreads;
            long[] result = new long[numThreads];

            Parallel.For(0, numThreads, i =>
            {
                long insideCircle = MonteCarloHelper.CalcPoints(pointsPerThreads);
                result[i] += insideCircle;
            });

            long reduced = 0;
            for (int i = 0; i < result.Length; i++)
            {
                reduced += result[i];
            }

            return MonteCarloHelper.CalcPi(reduced, n);
        }
    }
}
