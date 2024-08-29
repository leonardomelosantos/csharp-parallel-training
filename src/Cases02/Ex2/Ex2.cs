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

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Lab02.Ex2;

class Program
{
    static void Main(string[] args)
    {
        // Ler N da entrada
        int N = int.Parse(Console.ReadLine());

        double[] sequence = new double[N];

        // Inicializar o array com reais aleatórios entre 0 e 100
        Random rand = new Random();
        for (int i = 0; i < N; i++)
        {
            sequence[i] = rand.NextDouble() * 100;
        }

        // Continue a Implementação
        Task<double> resultMediana = CalcularMedianaAsync(sequence);
        Task<double> resultMedia = CalcularMediaAsync(sequence);
        Task<double> resultVariancia = CalcularVariancia(sequence);
        Task<double> resultDesvioPadrao = CalcularDesvioPadrao(sequence);

        Task.WaitAll(resultMediana, resultMedia, resultVariancia, resultDesvioPadrao);

        Console.WriteLine("Fim do programa");
    }

    private static async Task<double> CalcularMediaAsync(double[] sequence)
    {
        return await Task.Run(() =>
        {
            return sequence.Sum(x => x) / sequence.Length;
        });
    }

    private static async Task<double> CalcularMedianaAsync(double[] sequence)
    {
        return await Task.Run(() =>
        {
            Array.Sort(sequence);

            int numbersAmount = sequence.Length;
            int middleIndex = numbersAmount / 2;

            if (numbersAmount % 2 == 1)
            {
                return sequence[middleIndex];
            }
            else
            {
                return (sequence[middleIndex - 1] + sequence[middleIndex]) / 2;
            }
        });
    }

    private static async Task<double> CalcularVarianciaAsync(double[] sequence, double avarage)
    {
        return await Task.Run(() =>
        {
            int length = sequence.Length;

            // Calculate the avarage
            /*double sum = 0;
            foreach (double number in sequence)
            {
                sum += number;
            }
            double avarage = sum / length;*/

            // Calculate the squared deviations

            double squaredSum = 0;
            foreach (double number in sequence)
            {
                double deviation = number - avarage;
                squaredSum += Math.Pow(deviation, 2);
            }
            
            double varianceValue = squaredSum / length; // Calculate the varianceValue

            return varianceValue;
        });
    }

    private static async Task<double> CalcularDesvioPadraoAsync(double variancia)
    {
        return await Task.Run(() =>
        {
            return Math.Sqrt(variancia);
        });
    }
}