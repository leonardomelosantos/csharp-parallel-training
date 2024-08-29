/*
Exercício 1: Soma de Quadrados (1 ponto)

Objetivo: Exercitar paralelismo de dados e compartilhamento de recursos usando TPL.

Descrição: Dado um número inteiro N lido da entrada, seu programa deve calcular a soma
dos quadrados dos números inteiros no intervalo de 0 a N. Por fim, imprima o resultado.
Implemente uma versão sequencial e uma versão usando paralelismo. Qual é o speedup
que a sua implementação paralela obteve em relação a sequencial?
*/

using System.Diagnostics;

namespace Lab02.Ex1;

class Program
{
    static double somaTotal = 0;
    static int N;
    static int qtdThreads;
    static String _lockUtil = new String("");

    static void Main(string[] args)
    {
        Console.Write("Informe N para qtde numeros: ");
        N = int.Parse(Console.ReadLine());

        Console.Write("Informe W para numero de workers: ");
        qtdThreads = int.Parse(Console.ReadLine());

        // Iniciar a medição de tempo
        Stopwatch timer = new Stopwatch();
        timer.Start();

        // Continue a Implementação
        if (qtdThreads > 0)
            ProcessarSomatorioEmParalelo();
        else
            ProcessarSomatorioEmSequencial();

        // Contagem do tempo de execução
        timer.Stop();
        Console.WriteLine("Resultado da soma: " + somaTotal);
        Console.WriteLine("Tempo de execução: " + timer.ElapsedMilliseconds + " ms");
    }

    private static void ProcessarSomatorioEmSequencial()
    {
        for (int numero = 0; numero <= N; numero++)
        {
            somaTotal += Math.Pow(numero, 2);
        }
    }

    private static void ProcessarSomatorioEmParalelo()
    {
        Parallel.For(1, qtdThreads + 1, SomadorWorker);
    }

    private static void SomadorWorker(int indexThread, ParallelLoopState state)
    {
        int qtdNumerosPorThread = N / qtdThreads;

        // Preparando o bloco de inteiros que foi repartido para enviar como parâmetro para cada thread.
        double somaThread = (indexThread < qtdThreads || qtdThreads == 1)
            ? SumInterval((indexThread - 1) * qtdNumerosPorThread, qtdNumerosPorThread)
            : SumIntervalSpecificEnd((indexThread - 1) * qtdNumerosPorThread, N);

        lock (_lockUtil)
        {
            somaTotal += somaThread;
        }
    }

    private static double SumInterval(int start, int size)
    {
        return SumIntervalSpecificEnd(start, start + size);
    }

    private static double SumIntervalSpecificEnd(int start, int end)
    {
        double sum = 0;
        for (int number = start; number <= end; number++)
        {
            sum += Math.Pow(number, 2);
        }
        return sum;
    }
}