/*
Contagem Simultânea de Votos (1.5 pontos)

Descrição: Implemente um sistema de contagem de votos distribuídos em R regiões para
uma eleição com duas chapas: A e B. Cada região conta os votos de forma independente e,
em seguida, tenta incrementar de forma concorrente os contadores globais de votos das
chapas A e B. Para garantir a consistência da soma global, você deve implementar um
mecanismo de sincronização que evite acesso simultâneo aos contadores globais. Seu
programa deve receber R linhas de entrada, cada uma contendo dois inteiros que
representam o total de votos para as chapas A e B em uma região. Insira um delay aleatório
(Task.Delay) para simular o envio dos resultados regionais para o contador global. Ao final,
o programa deve indicar qual chapa venceu a eleição.

Exemplo:
Entrada:
4
23 78
56 21
234 189
123 236

Saída:
Chapa A: 436 Votos
Chapa B: 524 Votos
A chapa B venceu a eleição!
*/

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Lab03.Ex1;

class Program
{
    static int _totalChapaA = 0;
    static int _totalChapaB = 0;
    static object _lockGlobalUpdater = new object();

    static void Main(string[] args)
    {
        // Leitura do número de regiões
        int R = int.Parse(Console.ReadLine());

        var votos = new List<(int votosChapaA, int votosChapaB)>();

        // Leitura dos votos em cada região
        for (int i = 0; i < R; i++)
        {
            string[] entrada = Console.ReadLine().Split();
            int votosRegiaoA = int.Parse(entrada[0]);
            int votosRegiaoB = int.Parse(entrada[1]);
            votos.Add((votosRegiaoA, votosRegiaoB));
        }

        // Continue a implementação
        Parallel.ForEach<(int votosChapaA, int votosChapaB)>(votos, AtualizadorContagem);

        Console.WriteLine($"Chapa A: {_totalChapaA} votos");
        Console.WriteLine($"Chapa B: {_totalChapaB} votos");
        Console.WriteLine(GetChapaVencedora());
    }

    private static string GetChapaVencedora()
    {
        if (_totalChapaA == _totalChapaB)
            return "Eleição empatada";

        string chapaVencedora = _totalChapaA > _totalChapaB ? "A" : "B";
        return $"A chapa {chapaVencedora} venceu a eleição!";
    }

    private static void AtualizadorContagem((int votosChapaA, int votosChapaB) votes, ParallelLoopState state, long arg3)
    {
        Task.Delay(new Random().Next(1000));

        Interlocked.Add(ref _totalChapaA, votes.votosChapaA);
        Interlocked.Add(ref _totalChapaB, votes.votosChapaB);
    }
}
