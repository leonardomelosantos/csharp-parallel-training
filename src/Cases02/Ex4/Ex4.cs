/*
Exercício 4: Encontrar Números Primos (2.5 pontos)

Objetivo: Utilizar diferentes recursos da Task Parallel Library (TPL) do C#.

Descrição: Elabore uma aplicação que receba um número inteiro N, seguido por N
números inteiros. Para cada número k informado na entrada, a aplicação deve imprimir
todos os números primos entre 1 e k.
A. Faça uma versão que utilize Tasks.
B. Faça uma segunda versão que utiliza Parallel.For.
C. Faça uma terceira versão que utiliza Parallel.ForEach.
D. Compare o tempo de execução das três implementações.
*/

using System.Collections.Concurrent;
using System.Diagnostics;

namespace Lab02.Ex4;

class Program
{
    static void Main(string[] args)
    {
        (int N, int[] sequence) = GetInputData();

        // Continue a Implementação
        var timeForTasks = new PrimosTasks().Execute(sequence);
        var timeForParallelFor = new PrimosParallelFor().Execute(sequence);
        var timeForParallelForEach = new PrimosParallelForEach().Execute(sequence);

        Console.WriteLine($"Tasks: {timeForTasks}ms; " +
            $"ParallelFor: {timeForParallelFor}ms, " +
            $"ParallelForEach: {timeForParallelForEach}ms");
    }

    private static (int N, int[] sequence) GetInputData()
    {
        int N = int.Parse(Console.ReadLine());
        string[] input = Console.ReadLine().Split(' ');
        int[] sequence = new int[N];

        for (int i = 0; i < N; i++)
        {
            sequence[i] = int.Parse(input[i]);
        }
        return (N, sequence);
    }
}

class PrimosTasks : PrimosBase
{
    /// <summary>
    /// Using Tasks
    /// </summary>
    protected override void PrintPrimesNumbers(int[] sequence)
    {
        IList<Task> tasks = new List<Task>();
        foreach (int numberK in sequence)
        {
            Task taskForK = Task.Run(() =>
            {
                List<int> primes = GetPrimesFrom(numberK);
                PrintPrimesResult(numberK, primes);
            });
            tasks.Add(taskForK);
        }
        Task.WaitAll(tasks.ToArray());
    }

    protected override List<int> GetPrimesFrom(int k)
    {
        List<int> primes = new();
        for (int number = 1; number <= k; number++)
        {
            if (IsPrime(number))
            {
                primes.Add(number);
            }
        };
        return primes;
    }
}


class PrimosParallelFor : PrimosBase
{
    /// <summary>
    /// Using Parallel.For
    /// </summary>
    protected override void PrintPrimesNumbers(int[] sequence)
    {
        Parallel.For(0, sequence.Length, (index) =>
        {
            int k = sequence[index];
            List<int> primes = GetPrimesFrom(k);
            PrintPrimesResult(k, primes);
        });
    }

    /// <summary>
    /// Using Parallel.For
    /// </summary>
    protected override List<int> GetPrimesFrom(int k)
    {
        ConcurrentBag<int> primes = new ConcurrentBag<int>();
        Parallel.For(1, k + 1, (number) =>
        {
            if (IsPrime(number))
            {
                primes.Add(number);
            }
        });
        return primes.ToList();
    }
}


class PrimosParallelForEach : PrimosBase
{
    /// <summary>
    /// Using Parallel.ForEach
    /// </summary>
    protected override void PrintPrimesNumbers(int[] sequence)
    {
        Parallel.ForEach<int>(sequence, (k) =>
        {
            List<int> primes = GetPrimesFrom(k);
            PrintPrimesResult(k, primes);
        });
    }

    /// <summary>
    /// Using Parallel.ForEach
    /// </summary>
    protected override List<int> GetPrimesFrom(int k)
    {
        ConcurrentBag<int> primes = new ConcurrentBag<int>();
        Parallel.ForEach<int>(Enumerable.Range(1, k), (number) =>
        {
            if (IsPrime(number))
            {
                primes.Add(number);
            }
        });
        return primes.ToList();
    }
}

abstract class PrimosBase
{
    abstract protected void PrintPrimesNumbers(int[] sequence);
    abstract protected List<int> GetPrimesFrom(int k);

    public double Execute(int[] sequence)
    {
        Stopwatch timer = new();
        timer.Start();

        PrintPrimesNumbers(sequence); // Abstract

        timer.Stop();
        return timer.ElapsedMilliseconds;
    }

    protected static bool IsPrime(int number)
    {
        if (number < 2)
        {
            return false;
        }
        for (int i = 2; i <= Math.Sqrt(number); i++)
        {
            if (number % i == 0)
            {
                return false;
            }
        }
        return true;
    }

    protected static void PrintPrimesResult(int k, List<int> primesToK)
    {
        var intsToStrs = primesToK.Select(number => number.ToString()).ToList();
        var strsJoined = string.Join(", ", intsToStrs);
        Console.WriteLine($"Primes (1..{k}): {strsJoined}");
    }
}