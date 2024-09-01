/*
Exercício 2: Estatísticas de um Conjunto de Dados

Objetivo: Familiarizar-se com a criação e gerenciamento de tasks em C#.

Descrição: Escreva um programa que recebe um número inteiro N e gera um vetor de
números reais (double) com N elementos aleatórios. Em seguida, o programa deve calcular
a média, mediana, variância e desvio padrão do vetor usando tarefas (tasks). Ao final, o
programa deve imprimir essas estatísticas do vetor no console.

Observação: Algumas das estatísticas possuem dependências entre si. Por exemplo, o
cálculo da variância depende do valor da média. Considere como essas dependências
podem ser exploradas para otimizar a construção e a execução das tarefas.
*/

using System.Diagnostics;

namespace Lab02.Ex2;

class Program
{
    static void Main(string[] args)
    {
        double[] sequence = LerObterEntrada();

        Console.WriteLine("=== Iniciando execução");
        ExecutarSync(sequence);

        Console.WriteLine("=== Iniciando execução async");
        ExecutarAsync(sequence);
    }

    private static double[] LerObterEntrada()
    {
        // Ler N da entrada
        Console.Write("Digite a entrada - Qtd números aleatórios:");
        int N = int.Parse(Console.ReadLine());

        double[] sequence = new double[N];

        // Inicializar o array com reais aleatórios entre 0 e 100
        Random rand = new Random();
        for (int i = 0; i < N; i++)
        {
            sequence[i] = rand.NextDouble() * 100;
        }
        return sequence;
    }

    #region Sync methods
    private static void ExecutarSync(double[] sequence)
    {
        Stopwatch timer = new Stopwatch();
        timer.Start();

        double resultMediana = CalcularMediana(sequence);
        double resultMedia = CalcularMedia(sequence);
        double resultVariancia = CalcularVariancia(sequence, resultMedia);
        double resultDesvioPadrao = CalcularDesvioPadrao(resultVariancia, timer);
    }

    private static double CalcularMedia(double[] sequence)
    {
        return sequence.Sum(x => x) / sequence.Length;
    }

    private static double CalcularMediana(double[] sequence)
    {
        Array.Sort(sequence);

        int numbersAmount = sequence.Length;
        int middleIndex = numbersAmount / 2;

        double mediana = (numbersAmount % 2 == 1)
            ? sequence[middleIndex]
            : ((sequence[middleIndex - 1] + sequence[middleIndex]) / 2);

        Console.WriteLine("Mediana: " + mediana);
        return mediana;
    }

    private static double CalcularVariancia(double[] sequence, double avarage)
    {
        int length = sequence.Length;

        double squaredSum = 0;
        foreach (double number in sequence)
        {
            double deviation = number - avarage;
            squaredSum += Math.Pow(deviation, 2);
        }

        double varianceValue = squaredSum / length; // Calculate the varianceValue
        Console.WriteLine("Variância: " + varianceValue);
        return varianceValue;
    }

    private static double CalcularDesvioPadrao(double variancia, Stopwatch timer)
    {
        double desvioPadrao = Math.Sqrt(variancia);
        Console.WriteLine("Desvio padrão: " + desvioPadrao);

        timer.Stop();
        Console.WriteLine("Tempo de execução: " + timer.ElapsedMilliseconds + " ms");

        return desvioPadrao;
    }

    #endregion

    #region Async methods

    private static void ExecutarAsync(double[] sequence)
    {
        Stopwatch timer = new Stopwatch();
        timer.Start();

        Task<double> resultMedianaAsync = CalcularMedianaAsync(sequence);
        Task<double> resultMediaAsync = CalcularMediaAsync(sequence);
        Task<double> resultVarianciaAsync = CalcularVarianciaAsync(sequence, resultMediaAsync.Result);
        Task<double> resultDesvioPadraoAsync = CalcularDesvioPadraoAsync(resultVarianciaAsync.Result, timer);
        resultDesvioPadraoAsync.Wait();
    }

    private static async Task<double> CalcularMediaAsync(double[] sequence)
    {
        return await Task.Run(() =>
        {
            return CalcularMedia(sequence);
        });
    }

    private static async Task<double> CalcularMedianaAsync(double[] sequence)
    {
        return await Task.Run(() =>
        {
            return CalcularMediana(sequence);
        });
    }

    private static async Task<double> CalcularVarianciaAsync(double[] sequence, double avarage)
    {
        return await Task.Run(() =>
        {
            return CalcularVariancia(sequence, avarage);
        });
    }

    private static async Task<double> CalcularDesvioPadraoAsync(double variancia, Stopwatch timer)
    {
        return await Task.Run(() =>
        {
            return CalcularDesvioPadrao(variancia, timer);
        });
    }

    #endregion
}